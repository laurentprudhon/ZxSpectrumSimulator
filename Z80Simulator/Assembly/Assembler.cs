using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Z80Simulator.Instructions;

namespace Z80Simulator.Assembly
{
    /// <summary>
    /// This assembler is intended to be a test utility of the z80 simulator.
    /// It does not support macros and is very permissive on the syntax.
    /// </summary>
    public class Assembler
    {
        public static Program ParseProgram(string sourcePath, Stream inputStream, Encoding encoding, bool ignoreCase)
        {
            // Create a new program
            Program program = new Program(sourcePath, ProgramSource.AssemblyLanguageText);

            // Read each line from the input stream
            using (StreamReader reader = new StreamReader(inputStream, encoding))
            {
                string programLineText = null;
                int lineNumber = 1;
                while ((programLineText = reader.ReadLine()) != null)
                {
                    // Parse the program line and append it to the program
                    ProgramLine programLine = ParseProgramLine(programLineText, lineNumber, ignoreCase);
                    program.Lines.Add(programLine);

                    // Register line error at program level 
                    if (programLine.HasError)
                    {
                        program.LinesWithErrors.Add(programLine);
                        program.ErrorCount++;
                    }

                    lineNumber++;
                }
            }

            return program;
        }

        public static ProgramLine ParseProgramLine(string programLineText)
        {
            return ParseProgramLine(programLineText, 0, false);
        }

        public static ProgramLine ParseProgramLine(string programLineText, int lineNumber, bool ignoreCase)
        {
            // Create a new program line from text
            ProgramLine programLine = new ProgramLine() { LineNumber = lineNumber, Text = programLineText };

            // Ignore custom assembler directives
            if (programLineText.Length > 0)
            {
                string trimText = programLineText.Trim();
                if (trimText.Length > 0 && (trimText[0] == '#' || trimText[0] == '.'))
                {
                    programLine.Comment = programLineText;
                    return programLine;
                }
            }

            // Parse the program line
            AsmLexer lexer = new AsmLexer(programLine.Text, programLine.LineNumber, ignoreCase);
            AsmParser parser = new AsmParser(lexer, programLine);
            try
            {
                parser.ParseProgramLine();
            }
            catch (Exception e)
            {
                programLine.Error = new ProgramLineError() { ErrorMessage = e.Message, StartColumnIndex = 0, EndColumnIndex = programLine.Text.Length - 1 };
            }

            return programLine;
        }

        enum OperandType
        {
            Unsigned8,
            Signed8,
            Unsigned16,
        }

        class Operand
        {
            public int Address;
            public OperandType Type;
            public NumberExpression Expression;
            public ProgramLine Line;
        }

        public static void CompileProgram(Program program, int baseAddress, MemoryMap memoryMap)
        {
            // 1. First pass :
            // - execute assembler directives
            // - recognize and generate opcodes
            // => compute all adresses for labels
            // - collect all operands and their adresses, for delayed evaluation during the second pass

            int address = baseAddress;
            IList<Operand> operands = new List<Operand>();

            foreach(ProgramLine programLine in program.Lines)
            {
                // Skip lines with parsing errors
                if(programLine.HasError)
                {
                    continue;
                }

                // Set line address
                programLine.LineAddress = address;

                try
                {
                    // --------------------------------------
                    // Assembler directives
                    // --------------------------------------
                    if (programLine.Type == ProgramLineType.AssemblerDirective)
                    {
                        bool isEndDirective = CompileDirectiveLine(program, memoryMap, ref address, operands, programLine);
                        if (isEndDirective)
                        {
                            // END : indicates the end of the program. Any other statements following it will be ignored.
                            break;
                        }
                    }
                    // --------------------------------------
                    // Instructions opcodes
                    // --------------------------------------
                    else if (programLine.Type == ProgramLineType.OpCodeInstruction)
                    {
                        CompileInstructionLine(program, memoryMap, ref address, operands, programLine);
                    }
                    // --------------------------------------
                    // Comments
                    // --------------------------------------
                    else if (programLine.Type == ProgramLineType.CommentOrWhitespace)
                    {
                        CompileCommentLine(programLine);
                    }
                }
                catch (Exception e)
                {
                    programLine.Error = new ProgramLineError() { ErrorMessage = e.Message, StartColumnIndex = 0, EndColumnIndex = programLine.Text.Length - 1 };
                    program.ErrorCount++;
                    program.LinesWithErrors.Add(programLine);
                }
            }

            // 2. Second pass
            // - compute and generate values for all operands
            try
            {
                ComputeAndGenerateOperands(program, memoryMap, operands);
            }
            catch (Exception e)
            {
                ProgramLine firstProgramLine = program.Lines[0];
                firstProgramLine.Error = new ProgramLineError() { ErrorMessage = "Failed to compute values for all operands : " + e.Message, StartColumnIndex = 0, EndColumnIndex = firstProgramLine.Text.Length - 1 };
                program.ErrorCount++;
                program.LinesWithErrors.Add(firstProgramLine);
            }
        }

        private static void CompileCommentLine(ProgramLine programLine)
        {
            if (!String.IsNullOrEmpty(programLine.Label))
            {
                throw new Exception(String.Format("Line {0} : a label is not allowed for a comments or empty line", programLine.LineNumber));
            }
        }

        private static void CompileInstructionLine(Program program, MemoryMap memoryMap, ref int address, IList<Operand> operands, ProgramLine programLine)
        {
            if (!String.IsNullOrEmpty(programLine.Label))
            {
                string label = programLine.Label;
                if (!program.Variables.ContainsKey(label))
                {
                    program.Variables.Add(label, new LabelAddress(address));
                }
            }

            // Find instruction code and instruction type
            string instructionText = GenerateInstructionText(programLine);
            InstructionCode instructionCode = null;
            if (!Z80OpCodes.Dictionary.TryGetValue(instructionText, out instructionCode))
            {
                throw new Exception(String.Format("Line {0} : invalid instruction type {1}", programLine.LineNumber, instructionText));
            }
            InstructionType instructionType = Z80InstructionTypes.Table[instructionCode.InstructionTypeIndex];

            // Register binary representation of program line
            programLine.InstructionCode = instructionCode;
            programLine.InstructionType = instructionType;
            MemoryCellDescription programLineDescription = new MemoryCellDescription(MemoryDescriptionType.ProgramLine, programLine);

            // One or two byte opcodes
            if (instructionCode.OpCodeByteCount <= 2)
            {
                //  Optional one byte prefix
                if (instructionCode.Prefix != null)
                {
                    memoryMap.MemoryCellDescriptions[address] = programLineDescription;
                    memoryMap.MemoryCells[address++] = instructionCode.Prefix[0];
                }

                // Opcode
                memoryMap.MemoryCellDescriptions[address] = programLineDescription;
                memoryMap.MemoryCells[address++] = instructionCode.OpCode;

                // Parameters
                AddressingMode? paramType1 = instructionType.Param1Type;
                if (paramType1.HasValue)
                {
                    InstructionLineParam lineParam1 = programLine.OpCodeParameters[0];
                    AddOperandForLineParam(ref address, operands, programLine, paramType1, lineParam1);
                }
                AddressingMode? paramType2 = instructionType.Param2Type;
                if (paramType2.HasValue)
                {
                    InstructionLineParam lineParam2 = programLine.OpCodeParameters[1];
                    AddOperandForLineParam(ref address, operands, programLine, paramType2, lineParam2);
                }
            }
            // Four bytes opcodes
            else
            {
                // Two bytes prefix
                memoryMap.MemoryCellDescriptions[address] = programLineDescription;
                memoryMap.MemoryCells[address++] = instructionCode.Prefix[0];
                memoryMap.MemoryCellDescriptions[address] = programLineDescription;
                memoryMap.MemoryCells[address++] = instructionCode.Prefix[1];

                // Displacement
                AddressingMode? paramType1 = instructionType.Param1Type;
                if (paramType1.HasValue)
                {
                    InstructionLineParam lineParam1 = programLine.OpCodeParameters[0];
                    AddOperandForLineParam(ref address, operands, programLine, paramType1, lineParam1);
                }
                AddressingMode? paramType2 = instructionType.Param2Type;
                if (paramType2.HasValue)
                {
                    InstructionLineParam lineParam2 = programLine.OpCodeParameters[1];
                    AddOperandForLineParam(ref address, operands, programLine, paramType2, lineParam2);
                }

                // Opcode
                memoryMap.MemoryCellDescriptions[address] = programLineDescription;
                memoryMap.MemoryCells[address++] = instructionCode.OpCode;
            }
        }

        private static bool CompileDirectiveLine(Program program, MemoryMap memoryMap, ref int address, IList<Operand> operands, ProgramLine programLine)
        {
            switch (programLine.DirectiveName)
            {
                case "EQU":
                    // label EQU nn : this directive is used to assign a value to a label.
                    if (String.IsNullOrEmpty(programLine.Label))
                    {
                        throw new Exception(String.Format("Line {0} : EQU directive needs a label", programLine.LineNumber));
                    }
                    else if (programLine.DirectiveParameters.Count != 1)
                    {
                        throw new Exception(String.Format("Line {0} : EQU directive needs exactly one parameter", programLine.LineNumber));
                    }
                    else
                    {
                        DirectiveLineParam param = programLine.DirectiveParameters[0];
                        string label = programLine.Label;
                        if (program.Variables.ContainsKey(label))
                        {
                            throw new Exception(String.Format("Line {0} : EQU directive - this label can not be defined twice", programLine.LineNumber));
                        }
                        program.Variables.Add(label, param.NumberExpression);
                    }
                    break;
                case "DEFL":
                    // label DEFL nn : this directive also assigns a value nn to a label,
                    // but may be repeated whithin the program with different values for 
                    // the same label, whereas EQU may be used only once.
                    
                    // ==> Symbol table is much more complicated to handle in incremental parsing scenarios if it does not contain constants only
                    throw new Exception(String.Format("Line {0} : DEFL directive is not supported", programLine.LineNumber));
                case "ORG":
                    // ORG nn : this directive will set the address counter to the value nn.
                    // In other words, the first executable instruction encountered after
                    // this directive will reside at the value nn. It can be used to locate
                    // different segments of a program at different memory locations.
                    if (!String.IsNullOrEmpty(programLine.Label))
                    {
                        throw new Exception(String.Format("Line {0} : ORG directive does not allow a label", programLine.LineNumber));
                    }
                    else if (programLine.DirectiveParameters.Count != 1)
                    {
                        throw new Exception(String.Format("Line {0} : ORG directive needs exactly one parameter", programLine.LineNumber));
                    }
                    else
                    {
                        DirectiveLineParam param = programLine.DirectiveParameters[0];
                        address = param.NumberExpression.GetValue(program.Variables, programLine);
                    }
                    break;
                case "DEFS":
                    // DEFS nn : reserves a bloc of memory size nn bytes, starting at the 
                    // current value of the reference counter.
                case "RESERVE":
                    // label RESERVE nn : using the RESERVE pseudo-operation, you assign a 
                    // name to the memory area and declare the number of locations to be assigned.
                    if (String.IsNullOrEmpty(programLine.Label))
                    {
                        string label = programLine.Label;
                        program.Variables.Add(label, new LabelAddress(address));
                    }
                    if (programLine.DirectiveParameters.Count != 1)
                    {
                        throw new Exception(String.Format("Line {0} : DEFS or RESERVE directive needs exactly one parameter", programLine.LineNumber));
                    }
                    else
                    {
                        string label = programLine.Label;
                        program.Variables.Add(label, new NumberOperand(address));

                        DirectiveLineParam param = programLine.DirectiveParameters[0];
                        int increment = param.NumberExpression.GetValue(program.Variables, programLine);
                        address += increment;
                    }
                    break;
                case "DEFB":
                    // DEFB n,n,... : this directive assigns eight-bit contents to a byte
                    // residing at the current reference counter.
                    // DEFB 'S',... : assigns the ASCII value of 'S' to the byte.
                    if (!String.IsNullOrEmpty(programLine.Label))
                    {
                        string label = programLine.Label;
                        program.Variables.Add(label, new LabelAddress(address));
                    }
                    if (programLine.DirectiveParameters == null)
                    {
                        throw new Exception(String.Format("Line {0} : DEFB directive needs at least one parameter", programLine.LineNumber));
                    }
                    else
                    {
                        foreach (DirectiveLineParam param in programLine.DirectiveParameters)
                        {
                            Operand operand = new Operand() { Type = OperandType.Unsigned8, Address = address, Expression = param.NumberExpression, Line = programLine };
                            address += 1;
                            operands.Add(operand);
                        }
                    }
                    break;
                case "DEFW":
                    // DEFW nn,nn,... : this assigns the value nn to the two-byte word residing at
                    // the current reference counter and the following location.
                    if (!String.IsNullOrEmpty(programLine.Label))
                    {
                        string label = programLine.Label;
                        program.Variables.Add(label, new LabelAddress(address));
                    }
                    if (programLine.DirectiveParameters == null)
                    {
                        throw new Exception(String.Format("Line {0} : DEFW directive at least one parameter", programLine.LineNumber));
                    }
                    else
                    {
                        foreach (DirectiveLineParam param in programLine.DirectiveParameters)
                        {
                            Operand operand = new Operand() { Type = OperandType.Unsigned16, Address = address, Expression = param.NumberExpression, Line = programLine };
                            address += 2;
                            operands.Add(operand);
                        }
                    }
                    break;
                case "DEFM":
                    // DEFM "S" : stores into memory the string "S" starting at the current 
                    // reference counter. It must less than 63 in length.
                    if (!String.IsNullOrEmpty(programLine.Label))
                    {
                        string label = programLine.Label;
                        program.Variables.Add(label, new LabelAddress(address));
                    }
                    if (programLine.DirectiveParameters.Count != 1)
                    {
                        throw new Exception(String.Format("Line {0} : DEFM directive needs exactly one parameter", programLine.LineNumber));
                    }
                    else
                    {
                        DirectiveLineParam param = programLine.DirectiveParameters[0];
                        string stringValue = param.StringValue;
                        MemoryCellDescription programLineDescription = new MemoryCellDescription(MemoryDescriptionType.ProgramLine, programLine);

                        foreach (char chr in stringValue.ToCharArray())
                        {
                            memoryMap.MemoryCellDescriptions[address] = programLineDescription;
                            memoryMap.MemoryCells[address] = (byte)chr;
                            address += 1;
                        }
                    }
                    break;
                case "DATA":
                    // label DATA n,nn,'S' : the DATA pseudo-operation allows the programmer
                    // to enter fixed data into memory. Most assemblers allow more elaborate 
                    // DATA instructions that handle a large amount of data at one time.

                    // => Not useful since we already have DEFB / DEFW / DEFM, and one type of value per line is better for readability
                    throw new Exception(String.Format("Line {0} : DATA directive is not supported", programLine.LineNumber));
                case "END":
                    // END : indicates the end of the program. Any other statements following 
                    // it will be ignored.
                    if (!String.IsNullOrEmpty(programLine.Label))
                    {
                        throw new Exception(String.Format("Line {0} : END directive does not allow a label", programLine.LineNumber));
                    }
                    else
                    {
                        return true;
                    }

                // ==> Not supported yet, spectrum programs remain short by nature
                case "MACRO":
                    // MACRO P0 P1 .. Pn : is used to define a label as a macro, and to define
                    // its formal parameter list.
                    throw new Exception(String.Format("Line {0} : MACRO directive is not supported", programLine.LineNumber));
                case "ENDM":
                    // ENDM : is used to mark the end of macro definition.
                    throw new Exception(String.Format("Line {0} : ENDM directive is not supported", programLine.LineNumber));

            }
            return false;
        }

        private static void ComputeAndGenerateOperands(Program program, MemoryMap memoryMap, IList<Operand> operands)
        {
            foreach (Operand operand in operands)
            {
                switch (operand.Type)
                {
                    // 8 bit unsigned operand
                    case OperandType.Unsigned8:
                        byte uOperand = (byte)operand.Expression.GetValue(program.Variables, operand.Line);
                        if (operand.Line.Type == ProgramLineType.OpCodeInstruction)
                        {
                            memoryMap.MemoryCellDescriptions[operand.Address] = memoryMap.MemoryCellDescriptions[operand.Address - 1];
                        }
                        memoryMap.MemoryCells[operand.Address] = uOperand;
                        break;
                    // 8 bit signed operand
                    case OperandType.Signed8:
                        sbyte sOperand = (sbyte)operand.Expression.GetValue(program.Variables, operand.Line);
                        if (operand.Line.Type == ProgramLineType.OpCodeInstruction)
                        {
                            memoryMap.MemoryCellDescriptions[operand.Address] = memoryMap.MemoryCellDescriptions[operand.Address - 1];
                        }
                        memoryMap.MemoryCells[operand.Address] = unchecked((byte)sOperand);
                        break;
                    // 16 bit unsigned operand
                    case OperandType.Unsigned16:
                        ushort lOperand = (ushort)operand.Expression.GetValue(program.Variables, operand.Line);
                        if (operand.Line.Type == ProgramLineType.OpCodeInstruction)
                        {
                            memoryMap.MemoryCellDescriptions[operand.Address] = memoryMap.MemoryCellDescriptions[operand.Address - 1];
                        }
                        memoryMap.MemoryCells[operand.Address] = (byte)(lOperand & 0xFF);
                        if (operand.Line.Type == ProgramLineType.OpCodeInstruction)
                        {
                            memoryMap.MemoryCellDescriptions[operand.Address + 1] = memoryMap.MemoryCellDescriptions[operand.Address - 1];
                        }
                        memoryMap.MemoryCells[operand.Address + 1] = (byte)(lOperand >> 8);
                        break;
                }
            }
        }

        private static void AddOperandForLineParam(ref int address, IList<Operand> operands, ProgramLine programLine, AddressingMode? paramType, InstructionLineParam lineParam)
        {
            Operand operand = null;
            switch (paramType.Value)
            {
                // 8 bit unsigned operand
                case AddressingMode.Immediate:
                case AddressingMode.IOPortImmediate:
                    operand = new Operand() { Type = OperandType.Unsigned8, Address = address, Expression = lineParam.NumberExpression, Line = programLine};
                    address += 1;
                    break;
                // 8 bit signed operand
                case AddressingMode.Indexed:
                case AddressingMode.Relative:
                    operand = new Operand() { Type = OperandType.Signed8, Address = address, Expression = lineParam.NumberExpression, Line = programLine };
                    address += 1;
                    if (paramType.Value == AddressingMode.Relative && operand.Expression is SymbolOperand)
                    {
                        // If a label address is used where a relative address is expected, 
                        // we have to compute the relative displacement from the current addres to the label address
                        operand.Expression = new RelativeDisplacementFromLabelAddressExpression((SymbolOperand)operand.Expression, NumberOperationType.Subtraction, new NumberOperand(address));
                    }
                    else if (paramType.Value == AddressingMode.Relative && operand.Expression is NumberOperand)
                    {
                        // Relative adressing mode definition (DJNZ and JR instructions) :
                        // The jump is measured from the address of the instruction OpCode and has a range of -126 to +129 bytes. 
                        // The assembler automatically adjusts for the twice incremented PC.
                        operand.Expression = new NumberOperationExpression(operand.Expression, NumberOperationType.Subtraction, new NumberOperand(2));
                    }
                    break;
                // 16 bit unsigned operand
                case AddressingMode.Immediate16:
                case AddressingMode.Extended:
                    operand = new Operand() { Type = OperandType.Unsigned16, Address = address, Expression = lineParam.NumberExpression, Line = programLine};
                    address += 2;
                    break;
            }
            if (operand != null)
            {
                operands.Add(operand);
            }
        }

        private static string GenerateInstructionText(ProgramLine programLine)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(programLine.OpCodeName);
            if (programLine.OpCodeBytes !=null)
            {
                sb.Append("[");
                for (int i = 0; i < programLine.OpCodeBytes.Length; i++)
                {
                    if (i > 0) sb.Append(' ');
                    sb.Append(String.Format("{0:X2}H", programLine.OpCodeBytes[i]));
                }
                sb.Append("]");
            }
            if (programLine.OpCodeParameters != null)
            {
                if (programLine.OpCodeParameters.Count > 0)
                {
                    sb.Append(' ');
                }
                int paramIndex = 0;
                foreach (InstructionLineParam param in programLine.OpCodeParameters)
                {
                    if (paramIndex > 0)
                    {
                        sb.Append(',');
                    }
                    paramIndex++;
                    switch (param.Type)
                    {
                        case InstructionLineParamType.Address:
                            // At parsing time, a number between parentheses can be used for the following addressing modes :
                            //->Extended
                            //->IOPortImmediate
                            sb.Append("(n)");
                            break;
                        case InstructionLineParamType.Bit:
                            sb.Append(param.Bit.ToString()[1]);
                            break;
                        case InstructionLineParamType.FlagCondition:
                            sb.Append(param.FlagCondition.ToString());
                            break;
                        case InstructionLineParamType.Flags:
                            sb.Append("F");
                            break;
                        case InstructionLineParamType.IOPortRegister:
                            sb.Append(String.Format("({0})", param.Register.ToString()));
                            break;
                        case InstructionLineParamType.Indexed:
                            if (param.Register16 == Register16.IX)
                            {
                                sb.Append("(IX+d)");
                            }
                            else if (param.Register16 == Register16.IY)
                            {
                                sb.Append("(IY+d)");
                            }
                            break;
                        case InstructionLineParamType.InterruptMode:
                            sb.Append(param.InterruptMode.ToString()[2]);
                            break;
                        case InstructionLineParamType.Number:
                            // At parsing time, a simple number can be used for the following addressing modes :                            
                            //->ModifiedPageZero
                            if (programLine.OpCodeName == "RST")
                            {
                                sb.Append(String.Format("{0:X}H", param.NumberExpression.GetValue(null, programLine)));
                            }
                            //->Immediate
                            //->Relative
                            //->Immediate16
                            //->Extended
                            else
                            {
                                sb.Append("n");
                            }
                            break;
                        case InstructionLineParamType.Register:
                            sb.Append(param.Register.ToString());
                            break;
                        case InstructionLineParamType.Register16:
                            if (param.Register16 == Register16.AF2)
                            {
                                sb.Append("AF'");
                            }
                            else
                            {
                                sb.Append(param.Register16.ToString());
                            }
                            break;
                        case InstructionLineParamType.RegisterIndirect:
                            sb.Append(String.Format("({0})", param.Register16.ToString()));
                            break;
                    }
                }
            }
            return sb.ToString();
        }

        // To enhance the readability of the generated program
        public static void GenerateLabelsForIncomingAndOutgoingCalls(IEnumerable<ProgramLine> programLines, IDictionary<string, NumberExpression> variables)
        {
            // Find all the incoming calls : 
            // - generate labels for the target lines            
            foreach (ProgramLine instructionLine in programLines)
            {
                if (instructionLine.IncomingCalls != null)
                {
                    string labelName = String.Format("L{0:X4}", instructionLine.LineAddress);
                    Program.PrefixLabelToProgramLine(labelName, instructionLine);                    
                }
            }
            // Find all the outgoing calls
            // - replace address references with labels in the source lines 
            foreach (ProgramLine instructionLine in programLines)
            {
                if (instructionLine.OutgoingCall != null)
                {
                    ProgramLine targetLine = instructionLine.OutgoingCall.Line;
                    string targetLabel = targetLine.Label;
                    if (!String.IsNullOrEmpty(targetLabel))
                    {
                        if (!variables.ContainsKey(targetLabel))
                        {
                            variables.Add(targetLabel, new LabelAddress(targetLine.LineAddress));
                            instructionLine.Comment = targetLine.Comment;
                        }
                        bool addressIsRelative;
                        int addressParameterIndexToReplace = ProgramFlowAnalyzer.GetAddressParameterIndexToReplaceWithLabel(instructionLine.InstructionType, out addressIsRelative);
                        Program.ReplaceAddressWithSymbolInProgramLine(instructionLine, addressParameterIndexToReplace, addressIsRelative, targetLabel);
                    }
                }
            }
        }        

        public static void AnalyzeProgramFlow(Program program, MemoryMap memoryMap)
        {       
            // Generate all outgoing calls
            foreach (ProgramLine programLine in program.Lines)
            {
                ProgramFlowAnalyzer.GenerateOutgoingCall(programLine, program.Variables);
            }

            // Generate all incoming calls
            ProgramFlowAnalyzer.GenerateIncomingCalls(program.Lines, memoryMap);
        }
    }

    public enum AsmTokenType
    {
        COLON,         //     :
        COMMA,         //     ,
        OPENINGPAR,    //     (
        CLOSINGPAR,    //     )
        PLUS,          //     +
        MINUS,         //     -
        MULTIPLY,      //     *
        OPENINGBRA,    //     [
        CLOSINGBRA,    //     ] 
        DOLLAR,        //     $
        COMMENT,       //     ;.* 
        STRING,        //     "ascii chars" or 'ascii chars'
        SYMBOL,        //     [A-Z][A-Z1-9_]{0..9} --exclude-- opcodes, registers, flags, directives
        OPCODE,        //     (list)
        REGISTER,      //     (list)
        FLAGS,         //      F
        REGISTER16,    //     (list) ! AF' -> special ' char not in symbol
        FLAGCONDITION, //     (list) ! C can be flag condition only if previous token is OPCODE = CALL, JP, JR, RET
        DIRECTIVE,     //     (list)
        NUMBER,        //     [0-9]+ 'D'? or 0[A-F][0-9A-F]{1,3}|[0-9][0-9A-F]{1,3} 'H' or $[0-9A-F]+ or [0-7]+ ('O'|'Q') or [01]+ 'B' or %[01]+ or 'ascii char' ! must enable escapes \t \' ...
        BIT,           //     [0-7]  ! 0-7 can be bit only if previous token is OPCODE = BIT, RES, SET 
        INTERRUPTMODE, //     [0-2]  ! 0-2 can be interrupt mode only if previous token is OPCODE = IM 
        WHITESPACE,    //     space or tab
        ENDOFLINE
    }

    public class AsmToken
    {
        private string asmLine;

        public AsmTokenType Type { get; private set; }

        public int StartIndex { get; private set; }
        public int EndIndex { get; private set;}

        public string Text { get { return asmLine.Substring(StartIndex, EndIndex - StartIndex); } }        
        public object Value { get; private set; }

        public AsmToken LeadingWhiteSpace { get; private set; }
        public string TextWithLeadingWhiteSpace 
        { 
            get 
            {
                if (LeadingWhiteSpace == null)
                {
                    return Text;
                }
                else 
                {
                    return LeadingWhiteSpace.Text + Text;
                }
            } 
        }

        public AsmToken(string asmLine, AsmToken leadingWhitespaceToken, AsmTokenType type, int startIndex, int endIndex) : 
            this(asmLine, leadingWhitespaceToken, type, startIndex, endIndex, null)
        { }

        public AsmToken(string asmLine, AsmToken leadingWhitespaceToken, AsmTokenType type, int startIndex, int endIndex, object value)
        {
            this.asmLine = asmLine;
            this.LeadingWhiteSpace = leadingWhitespaceToken;
            Type = type;
            StartIndex = startIndex;
            EndIndex = endIndex;     
            Value = value;
        }

        public override string ToString()
        {
            return Type.ToString() + " ["+StartIndex+","+EndIndex+"] " + Text + " >"+(Value!=null?Value.ToString():"")+"<";
        }
    }

    public class AsmLexer
    {
        private int lineNumber;        
        private string asmLine;
        private int nextIndex = 0;

        StringComparison stringComparisonMode;
        StringComparer stringComparer;

        // Register all the tokens found during the lexing phase
        public IList<AsmToken> AllTokens { get; private set; }

        public AsmLexer(string asmLine, int lineNumber, bool ignoreCase)
        {
            this.lineNumber = lineNumber;
            this.asmLine = asmLine;
            if (ignoreCase)
            {
                stringComparisonMode = StringComparison.OrdinalIgnoreCase;
                stringComparer = StringComparer.OrdinalIgnoreCase;
            }
            else
            {
                stringComparisonMode = StringComparison.Ordinal;
                stringComparer = StringComparer.Ordinal;
            }
            AllTokens = new List<AsmToken>();
        }

        private char GetNextChar()
        {
            if(nextIndex < asmLine.Length)
            {
                return asmLine[nextIndex++];
            }
            else
            {
                return (char)0;
            }
        }

        private char PeekNextChar()
        {
            if(nextIndex < asmLine.Length)
            {
                return asmLine[nextIndex];
            }
            else
            {
                return (char)0;
            }
        }

        // BIT and FLAG tokens can only be detected id the previous opcode is known
        private AsmToken currentToken;
        // To remember the result of the computation of PeekNextToken
        private AsmToken nextToken;

        public AsmToken GetNextToken()
        {
            if (nextToken != null)
            {
                currentToken = nextToken;
                nextToken = null;
            }
            else
            {
                currentToken = FindNextToken();
            }
            AllTokens.Add(currentToken);
            if(buildTokenList)
            {
                partialTokenList.Add(currentToken);
            }
            return currentToken;
        }

        // Alias used by the parser to explain that the token is pure syntax and will not be used
        public void ConsumeToken()
        {
            GetNextToken();
        }

        public AsmToken CurrentToken { get { return currentToken; } }

        public AsmToken PeekNextToken()
        {
            if (nextToken == null)
            {
                nextToken = FindNextToken();
            }
            return nextToken;
        }

        private bool buildTokenList = false;
        private IList<AsmToken> partialTokenList;

        public void StartTokenList()
        {
            buildTokenList = true;
            partialTokenList = new List<AsmToken>();
        }

        public IList<AsmToken> EndTokenList()
        {
            buildTokenList = false;
            IList<AsmToken> resultList = partialTokenList;
            partialTokenList = null;
            return resultList;
        }

        private AsmToken FindNextToken()
        {
            int startIndex = nextIndex;
            char startChar = GetNextChar();
            AsmToken leadingWhitespaceToken = null;
            if (IsWhitespace(startChar))
            {
                AsmTokenType type = AsmTokenType.WHITESPACE;
                for (char c = PeekNextChar(); IsWhitespace(c); c = PeekNextChar())
                {
                    GetNextChar();
                }
                int endIndex = nextIndex;
                leadingWhitespaceToken = new AsmToken(asmLine, leadingWhitespaceToken, type, startIndex, endIndex);
                // Start next token
                startIndex = nextIndex;
                startChar = GetNextChar();
            }
            switch (startChar)
            {
                case ':':
                    AsmTokenType type = AsmTokenType.COLON;
                    int endIndex = nextIndex;
                    return new AsmToken(asmLine, leadingWhitespaceToken, type, startIndex, endIndex);
                case ',':
                    type = AsmTokenType.COMMA;   
                    endIndex = nextIndex;
                    return new AsmToken(asmLine, leadingWhitespaceToken, type, startIndex, endIndex);
                case '(':
                    type = AsmTokenType.OPENINGPAR;   
                    endIndex = nextIndex;
                    return new AsmToken(asmLine, leadingWhitespaceToken, type, startIndex, endIndex);
                case ')':
                    type = AsmTokenType.CLOSINGPAR;   
                    endIndex = nextIndex;
                    return new AsmToken(asmLine, leadingWhitespaceToken, type, startIndex, endIndex);
                case '+':
                    type = AsmTokenType.PLUS;   
                    endIndex = nextIndex;
                    return new AsmToken(asmLine, leadingWhitespaceToken, type, startIndex, endIndex);
                case '-':
                    type = AsmTokenType.MINUS;   
                    endIndex = nextIndex;
                    return new AsmToken(asmLine, leadingWhitespaceToken, type, startIndex, endIndex);
                case '*':
                    type = AsmTokenType.MULTIPLY;
                    endIndex = nextIndex;
                    return new AsmToken(asmLine, leadingWhitespaceToken, type, startIndex, endIndex);
                case '[':
                    type = AsmTokenType.OPENINGBRA;   
                    endIndex = nextIndex;
                    return new AsmToken(asmLine, leadingWhitespaceToken, type, startIndex, endIndex);
                case ']':
                    type = AsmTokenType.CLOSINGBRA;   
                    endIndex = nextIndex;
                    return new AsmToken(asmLine, leadingWhitespaceToken, type, startIndex, endIndex);
                case '$':
                    char nc = PeekNextChar();
                    if(!IsHexaDigit(nc))
                    {
                        type = AsmTokenType.DOLLAR;   
                        endIndex = nextIndex;
                        return new AsmToken(asmLine, leadingWhitespaceToken, type, startIndex, endIndex);
                    }
                    else
                    {
                        type = AsmTokenType.NUMBER; 
                        int valStartIndex = nextIndex;
                        for (char c = PeekNextChar(); c != (char)0 && IsHexaDigit(c); c = PeekNextChar())
                        {
                            GetNextChar();
                        }
                        endIndex = nextIndex;
                        int intValue = Convert.ToInt32(asmLine.Substring(valStartIndex, endIndex - valStartIndex), 16);
                        return new AsmToken(asmLine, leadingWhitespaceToken, type, startIndex, endIndex, intValue);
                    }
                case ';':                    
                    type = AsmTokenType.COMMENT;   
                    while(GetNextChar() != 0) { }
                    endIndex = nextIndex;
                    return new AsmToken(asmLine, leadingWhitespaceToken, type, startIndex, endIndex, asmLine.Substring(startIndex + 1, endIndex - startIndex - 1).Trim());
                case '"':
                    type = AsmTokenType.STRING;   
                    int valueStartIndex = nextIndex;
                    char previousChar = (char)0;
                    StringBuilder sbValue;
                    ReadStringToken('"', startIndex, valueStartIndex, previousChar, out endIndex, out sbValue);
                    return new AsmToken(asmLine, leadingWhitespaceToken, type, startIndex, endIndex, sbValue.ToString());
                case '\'':
                    type = AsmTokenType.NUMBER;
                    valueStartIndex = nextIndex;
                    char chr = GetNextChar();
                    if(chr == '\\')
                    {
                        chr = GetNextChar();
                        chr = GetCharValueFromEscapeChar(chr);
                    }
                    if(GetNextChar() != '\'')
                    {
                        // Then switch to string token
                        type = AsmTokenType.STRING;
                        previousChar = chr;
                        ReadStringToken('\'', startIndex, valueStartIndex, previousChar, out endIndex, out sbValue);
                        return new AsmToken(asmLine, leadingWhitespaceToken, type, startIndex, endIndex, sbValue.ToString());
                    }
                    endIndex = nextIndex;
                    int asciiValue = (byte)chr;
                    return new AsmToken(asmLine, leadingWhitespaceToken, type, startIndex, endIndex, asciiValue);
                case '%':
                    type = AsmTokenType.NUMBER; 
                    valueStartIndex = nextIndex;
                    for (char c = PeekNextChar(); c != (char)0 && (c == '0' || c == '1'); c = PeekNextChar())
                    {
                        GetNextChar();
                    }
                    endIndex = nextIndex;
                    int binValue = Convert.ToByte(asmLine.Substring(valueStartIndex, endIndex - valueStartIndex), 2);
                    return new AsmToken(asmLine, leadingWhitespaceToken, type, startIndex, endIndex, binValue);
                case (char)0:
                    type = AsmTokenType.ENDOFLINE;
                    return new AsmToken(asmLine, leadingWhitespaceToken, type, startIndex, startIndex);
                default:
                    // Special case 1 : 0-7 char is ambiguous, it can be a BIT or a NUMBER, but it can be a BIT only if previous token is OPCODE = BIT, RES, SET 
                    if(startChar >= 48 && startChar <= 55 && currentToken.Type == AsmTokenType.OPCODE && 
                            (String.Compare(currentToken.Text, "BIT", stringComparisonMode) == 0 || 
                             String.Compare(currentToken.Text, "RES", stringComparisonMode) == 0 || 
                             String.Compare(currentToken.Text, "SET", stringComparisonMode) == 0 ))
                    {
                        type = AsmTokenType.BIT;
                        endIndex = nextIndex;
                        return new AsmToken(asmLine, leadingWhitespaceToken, type, startIndex, endIndex, startChar - 48);
                    }
                    // Special case 2 : 0-2 char is ambiguous, it can be an INTERRUPTMODE or a NUMBER, but it can be an INTERRUPTMODE only if previous token is OPCODE = IM 
                    else if(startChar >= 48 && startChar <= 50 && currentToken.Type == AsmTokenType.OPCODE && 
                             String.Compare(currentToken.Text, "IM", stringComparisonMode) == 0 )
                    {
                        type = AsmTokenType.INTERRUPTMODE;
                        endIndex = nextIndex;
                        return new AsmToken(asmLine, leadingWhitespaceToken, type, startIndex, endIndex, startChar - 48);
                    }
                    else if(IsFirstSymbolChar(startChar))
                    {
                        for (char c = PeekNextChar(); c != (char)0 && IsSymbolChar(c); c = PeekNextChar())
                        {
                            GetNextChar();
                        }
                        endIndex = nextIndex;
                        string symbol = asmLine.Substring(startIndex, endIndex - startIndex);
                        // Special case 3 : ' is not a valid symbol char, but AF' is a valid register name
                        if(String.Compare(symbol, "AF", stringComparisonMode) == 0 && PeekNextChar() == '\'')
                        {
                            GetNextChar();
                            endIndex = nextIndex;
                            symbol = "AF'";
                        }
                        // Special case 4 : C symbol is ambiguous, it can be a flag condition or a register, but it can be a flag only if previous token is OPCODE = CALL, JP, JR, RET
                        if(String.Compare(symbol, "C", stringComparisonMode) == 0 && currentToken.Type == AsmTokenType.OPCODE && 
                            (String.Compare(currentToken.Text, "CALL", stringComparisonMode) == 0 || 
                             String.Compare(currentToken.Text, "JP", stringComparisonMode) == 0   || 
                             String.Compare(currentToken.Text, "JR", stringComparisonMode) == 0   ||
                             String.Compare(currentToken.Text, "RET", stringComparisonMode) == 0  ))
                        {
                            type = AsmTokenType.FLAGCONDITION;
                            return new AsmToken(asmLine, leadingWhitespaceToken, type, startIndex, endIndex);
                        }
                        else if(OPCODES.Contains<string>(symbol, stringComparer))
                        {
                            type = AsmTokenType.OPCODE;
                            return new AsmToken(asmLine, leadingWhitespaceToken, type, startIndex, endIndex);
                        }
                        else if (REGISTERS8.Contains<string>(symbol, stringComparer))
                        {
                            type = AsmTokenType.REGISTER;
                            return new AsmToken(asmLine, leadingWhitespaceToken, type, startIndex, endIndex);
                        }
                        else if (REGISTERS16.Contains<string>(symbol, stringComparer))
                        {
                            type = AsmTokenType.REGISTER16;
                            return new AsmToken(asmLine, leadingWhitespaceToken, type, startIndex, endIndex);
                        }
                        else if (FLAGS.Contains<string>(symbol, stringComparer))
                        {
                            type = AsmTokenType.FLAGS;
                            return new AsmToken(asmLine, leadingWhitespaceToken, type, startIndex, endIndex);
                        }
                        else if(FLAGCONDITIONS.Contains<string>(symbol, stringComparer))
                        {
                            type = AsmTokenType.FLAGCONDITION;
                            return new AsmToken(asmLine, leadingWhitespaceToken, type, startIndex, endIndex);
                        }
                        else if(DIRECTIVES.Contains<string>(symbol, stringComparer))
                        {
                            type = AsmTokenType.DIRECTIVE;
                            return new AsmToken(asmLine, leadingWhitespaceToken, type, startIndex, endIndex);
                        }
                        // Special case 5 : an hexadecimal number is also a valid symbol
                        else if(hexaNumberRegex.IsMatch(symbol))
                        {
                            type = AsmTokenType.NUMBER;                             
                            Match hexaMatch = hexaNumberRegex.Match(symbol);
                            string numberString = hexaMatch.Groups[1].Value;
                            int intValue = 0;
                            if (numberString.Length <= 2)
                            {
                                intValue = Convert.ToByte(numberString, 16);
                            }
                            else
                            {
                                intValue = Convert.ToUInt16(numberString, 16);
                            }
                            return new AsmToken(asmLine, leadingWhitespaceToken, type, startIndex, endIndex, intValue);
                        }
                        else
                        {
                            type = AsmTokenType.SYMBOL;
                            return new AsmToken(asmLine, leadingWhitespaceToken, type, startIndex, endIndex);
                        }
                    }
                    else if(IsDigit(startChar))
                    {
                        type = AsmTokenType.NUMBER; 
                        for (char c = PeekNextChar(); c != (char)0 && IsHexaDigit(c); c = PeekNextChar())
                        {
                            GetNextChar();
                        }
                        switch (PeekNextChar())
                        {
                            case 'O':
                            case 'Q':
                            case 'H':
                            case 'o':
                            case 'q':
                            case 'h':
                                GetNextChar();
                                break;
                        }
                        int nbEndIndex = nextIndex;
                        string numberString = asmLine.Substring(startIndex, nbEndIndex - startIndex);
                        int intValue = 0;
                        if(hexaNumberRegex.IsMatch(numberString))
                        {
                            Match hexaMatch = hexaNumberRegex.Match(numberString);
                            numberString = hexaMatch.Groups[1].Value;
                            if (numberString.Length <= 2)
                            {
                                intValue = Convert.ToByte(numberString, 16);
                            }
                            else
                            {
                                intValue = Convert.ToUInt16(numberString, 16);
                            }
                        }
                        else if (decimalNumberRegex.IsMatch(numberString))
                        {
                            Match decimalMatch = decimalNumberRegex.Match(numberString);
                            numberString = decimalMatch.Groups[1].Value;
                            intValue = UInt16.Parse(numberString);
                        }
                        else if (binaryNumberRegex.IsMatch(numberString))
                        {
                            Match binMatch = binaryNumberRegex.Match(numberString);
                            numberString = binMatch.Groups[1].Value;
                            if (numberString.Length <= 8)
                            {
                                intValue = Convert.ToByte(numberString, 2);
                            }
                            else
                            {
                                intValue = Convert.ToUInt16(numberString, 2);
                            }
                        }
                        else if (octalNumberRegex.IsMatch(numberString))
                        {
                            Match octalMatch = octalNumberRegex.Match(numberString);
                            numberString = octalMatch.Groups[1].Value;
                            intValue = Convert.ToUInt16(numberString , 8);
                        }
                        else
                        {
                            throw new Exception(String.Format("Line {0} : Invalid number format {1} at column {2}", lineNumber, numberString, startIndex));
                        }
                        return new AsmToken(asmLine, leadingWhitespaceToken, type, startIndex, nbEndIndex, intValue);

                    }
                    else
                    {
                        throw new Exception(String.Format("Line {0} : Unexpected char {1} at column {2}", lineNumber, startChar, startIndex));
                    }
            }
        }

        private void ReadStringToken(char delimiter, int startIndex, int valueStartIndex, char previousChar, out int endIndex, out StringBuilder sbValue)
        {
            for (char c = PeekNextChar(); c != (char)0 && (c != delimiter || previousChar == '\\'); c = PeekNextChar())
            {
                previousChar = c;
                GetNextChar();
            }
            int valueEndIndex = nextIndex;
            if (GetNextChar() != delimiter)
            {
                throw new Exception(String.Format("Line {0} : String value starting at column {1} was not closed with "+delimiter, lineNumber, startIndex));
            }
            endIndex = nextIndex;
            // Evaluate all escape characters
            sbValue = new StringBuilder();
            for (int i = valueStartIndex; i < valueEndIndex; i++)
            {
                char strchr = asmLine[i];
                if (strchr == '\\')
                {
                    strchr = asmLine[++i];
                    strchr = GetCharValueFromEscapeChar(strchr);
                }
                sbValue.Append(strchr);
            }
        }

        private Regex binaryNumberRegex = new Regex("^([01]{1,16})B$", RegexOptions.IgnoreCase);
        private Regex octalNumberRegex = new Regex("^([0-7]{1,8})(?:O|Q)$", RegexOptions.IgnoreCase);
        private Regex decimalNumberRegex = new Regex("^([0-9]{1,5})D?$", RegexOptions.IgnoreCase);
        private Regex hexaNumberRegex = new Regex("^((?:0[A-F][0-9A-F]{1,3}|[0-9A-F]{1,4}))H$", RegexOptions.IgnoreCase);

        private bool IsWhitespace(char c)
        {
            //        space or      tab 
            return c == ' ' || c == '\t';
        }

        private bool IsHexaDigit(char c)
        {
            //             0 - 9                   A - F
            return (c >= 48 && c <= 57) || (c >= 65 && c <= 70);
        }

        private bool IsDigit(char c)
        {
            //             0 - 9                   
            return (c >= 48 && c <= 57);
        }

        private bool IsFirstSymbolChar(char c)
        {
            //             A - Z                   a - z
            return (c >= 65 && c <= 90) || (c >= 97 && c <= 122);
        }

        private bool IsSymbolChar(char c)
        {
            //             0 - 9                   A - Z                   a - z
            return (c >= 48 && c <= 57) || (c >= 65 && c <= 90) || (c >= 97 && c <= 122) || c == '_';
        }

        private static char GetCharValueFromEscapeChar(char chr)
        {
            switch (chr)
            {
                case '0':
                    return (char)0;
                case 'a':
                    return (char)7;
                case 'b':
                    return (char)8;
                case 't':
                    return (char)9;
                case 'n':
                    return (char)10;
                case 'v':
                    return (char)11;
                case 'f':
                    return (char)12;
                case 'r':
                    return (char)13;
                default:
                    return chr;
            }
        }

        public static string[] OPCODES = { "ADC","ADD","AND","BIT","CALL","CCF","CP","CPD",
            "CPDR","CPI","CPIR","CPL","DAA","DEC","DI","DJNZ","EI","EX","EXX","HALT","IM","IN",
            "INC","IND","INDR","INI","INIR","JP","JR","LD","LDD","LDDR","LDI","LDIR","NEG","NONI",
            "NOP","OR","OTDR","OTIR","OUT","OUTD","OUTI","POP","PUSH","RES","RESET","RET","RETI",
            "RETN","RL","RLA","RLC","RLCA","RLD","RR","RRA","RRC","RRCA","RRD","RST","SBC","SCF",
            "SET","SLA","SLL","SRA","SRL","SUB","XOR" };

        public static string[] DIRECTIVES = {"ORG","EQU","DATA","RESERVE","DEFL","DEFB","DEFW",
            "DEFS","DEFM","END","MACRO","ENDM" };

        private static string[] REGISTERS8 = { "A","B","C","D","E","H","L","I","R","IXh","IXl","IYh","IYl" };
        private static string[] REGISTERS16 = { "AF","AF'","BC","DE","HL","IX","IY","SP" };

        private static string[] FLAGS = { "F" };
        private static string[] FLAGCONDITIONS = { "C","M","NC","NZ","P","PE","PO","Z" };

        internal static Bit ParseBitToken(string token)
        {
            switch (token)
            {
                case "0":
                    return Bit.b0;
                case "1":
                    return Bit.b1;
                case "2":
                    return Bit.b2;
                case "3":
                    return Bit.b3;
                case "4":
                    return Bit.b4;
                case "5":
                    return Bit.b5;
                case "6":
                    return Bit.b6;
                case "7":
                    return Bit.b7;
                default:
                    throw new Exception(String.Format("{0} is not a legal value for Bit", token));
            }
        }

        internal static InterruptMode ParseInterruptModeToken(string token)
        {
            switch (token)
            {
                case "0":
                    return InterruptMode.IM0;
                case "1":
                    return InterruptMode.IM1;
                case "2":
                    return InterruptMode.IM2;
                default:
                    throw new Exception(String.Format("{0} is not a legal value for InterruptMode", token));
            }
        }

        internal static Register ParseRegisterToken(string token)
        {
            switch (token)
            {
                case "A":
                    return Register.A;                    
                case "B":
                    return Register.B;
                case "C":
                    return Register.C;
                case "D":
                    return Register.D;
                case "E":
                    return Register.E;
                case "H":
                    return Register.H;
                case "L":
                    return Register.L;
                case "I":
                    return Register.I;
                case "R":
                    return Register.R;
                case "IXh":
                    return Register.IXh;
                case "IXl":
                    return Register.IXl;
                case "IYh":
                    return Register.IYh;
                case "IYl":
                    return Register.IYl;
                default:
                    throw new Exception(String.Format("{0} is not a legal value for Register", token));
            }
        }
        
        internal static Register16 ParseRegister16Token(string token)
        {
            switch (token)
            {
                case "AF":
                    return Register16.AF;
                case "AF'":
                    return Register16.AF2;
                case "BC":
                    return Register16.BC;
                case "DE":
                    return Register16.DE;
                case "HL":
                    return Register16.HL;
                case "IX":
                    return Register16.IX;
                case "IY":
                    return Register16.IY;
                case "SP":
                    return Register16.SP;
                default:
                    throw new Exception(String.Format("{0} is not a legal value for Register16", token));
            }
        }

        internal static FlagCondition ParseFlagConditionToken(string token)
        {
            switch (token)
            {
                case "C":
                    return FlagCondition.C;
                case "M":
                    return FlagCondition.M;
                case "NC":
                    return FlagCondition.NC;
                case "NZ":
                    return FlagCondition.NZ;
                case "P":
                    return FlagCondition.P;
                case "PE":
                    return FlagCondition.PE;
                case "PO":
                    return FlagCondition.PO;
                case "Z":
                    return FlagCondition.Z;
                default:
                    throw new Exception(String.Format("{0} is not a legal value for FlagCondition", token));
            }
        }
    }

    class AsmParser
    {
        private AsmLexer lexer;
        private int lineNumber;
        private ProgramLine prgLine;

        public AsmParser(AsmLexer lexer, ProgramLine prgLine)
        {
            this.lexer = lexer;
            this.lineNumber = prgLine.LineNumber;
            this.prgLine = prgLine;
        }

        // line := ((SYMBOL COLON?)? (opcodeinstruction | directiveinstruction))? COMMENT? ENDOFLINE
        public void ParseProgramLine()
        {            
            // Parsing label
            bool lineHasLabel = false;
            AsmToken nextToken = lexer.PeekNextToken();
            if(nextToken.Type == AsmTokenType.SYMBOL)
            {
                prgLine.LabelToken = lexer.GetNextToken();
                prgLine.Label = prgLine.LabelToken.Text;
                lineHasLabel = true;

                if (lexer.PeekNextToken().Type == AsmTokenType.COLON)
                {
                    lexer.ConsumeToken();
                }
            }

            // Parsing instruction : z80 opcode or assembler directive
            bool lineHasInstruction = false;
            nextToken = lexer.PeekNextToken();
            if(lineHasLabel || (nextToken.Type != AsmTokenType.COMMENT && nextToken.Type != AsmTokenType.ENDOFLINE))
            {
                if (nextToken.Type == AsmTokenType.OPCODE)
                {
                    lineHasInstruction = true;
                    ParseOpcodeInstruction();
                }
                else if(nextToken.Type == AsmTokenType.DIRECTIVE)
                {
                    lineHasInstruction = true;
                    ParseDirectiveInstruction();
                }
                else
                {
                    throw new Exception(String.Format("Line {0} : Expecting opcode name or directive at column {1} instead of {2}", lineNumber, nextToken.StartIndex,nextToken.Text));
                }
            }
            
            // Parsing comment
            if (!lineHasInstruction)
            {
                prgLine.Type = ProgramLineType.CommentOrWhitespace;
            }
            nextToken = lexer.PeekNextToken();
            if (lexer.PeekNextToken().Type == AsmTokenType.COMMENT)
            {                
                prgLine.CommentToken = lexer.GetNextToken();
                prgLine.Comment = prgLine.CommentToken.Value.ToString();
            }   

            // End of line should have been reached tat this point
            nextToken = lexer.PeekNextToken();
            if (lexer.PeekNextToken().Type != AsmTokenType.ENDOFLINE)
            {
                throw new Exception(String.Format("Line {0} : Expecting end of line at column {1} instead of {2}", lineNumber, nextToken.StartIndex, nextToken.Text));
            }

            prgLine.AllTokens = lexer.AllTokens;
        }

        // opcodeinstruction := OPCODE (OPENINGBRA NUMBER+ CLOSINGBRA)? (opcodeparam (COMMA opcodeparam){0,2})?
        private void ParseOpcodeInstruction()
        {
            // Parsing opcode name
            AsmToken nextToken = lexer.PeekNextToken();
            if (nextToken.Type == AsmTokenType.OPCODE)
            {
                prgLine.OpCodeNameToken = lexer.GetNextToken();
                prgLine.Type = ProgramLineType.OpCodeInstruction;
                prgLine.OpCodeName = prgLine.OpCodeNameToken.Text;

                // Parsing optional opcode number between brackets
                nextToken = lexer.PeekNextToken();
                if(nextToken.Type == AsmTokenType.OPENINGBRA)
                {
                    prgLine.OpCodeBytesTokens = new List<AsmToken>();
                    prgLine.OpCodeBytesTokens.Add(lexer.GetNextToken());

                    nextToken = lexer.PeekNextToken();
                    if (nextToken.Type == AsmTokenType.NUMBER)
                    {
                        IList<byte> opcodeBytes = new List<byte>();
                        do
                        {
                            AsmToken numberToken = lexer.GetNextToken();
                            prgLine.OpCodeBytesTokens.Add(numberToken);
                            int opcodeByte = (int)numberToken.Value;
                            opcodeBytes.Add((byte)opcodeByte);

                            nextToken = lexer.PeekNextToken();
                        } while (nextToken.Type == AsmTokenType.NUMBER);

                        prgLine.OpCodeBytes = new byte[opcodeBytes.Count];
                        for (int i = 0; i < opcodeBytes.Count; i++)
                        {
                            prgLine.OpCodeBytes[i] = opcodeBytes[i];
                        }

                        if (nextToken.Type == AsmTokenType.CLOSINGBRA)
                        {
                            prgLine.OpCodeBytesTokens.Add(lexer.GetNextToken());
                        }
                        else
                        {
                            throw new Exception(String.Format("Line {0} : Expecting closing bracket ] at column {1} instead of {2}", lineNumber, nextToken.StartIndex, nextToken.Text));
                        }
                    }
                    else
                    {
                        throw new Exception(String.Format("Line {0} : Expecting number at column {1} instead of {2}", lineNumber, nextToken.StartIndex, nextToken.Text));
                    }
                }
            }
            else
            {
                throw new Exception(String.Format("Line {0} : Expecting opcode name or directive at column {1} instead of {2} : {3}", lineNumber, nextToken.StartIndex, nextToken.Type.ToString(), nextToken.Text));
            }

            // Parsing opcode parameters
            nextToken = lexer.PeekNextToken();
            if(nextToken.Type != AsmTokenType.COMMENT && nextToken.Type != AsmTokenType.ENDOFLINE)
            {
                prgLine.OpCodeParameters = new List<InstructionLineParam>();
                ParseOpcodeParam();

                while (lexer.PeekNextToken().Type == AsmTokenType.COMMA)
                {
                    lexer.ConsumeToken();
                    ParseOpcodeParam();
                }
            }
        }

        // Param type   : Address                        | Bit | FlagCondition | Flags | IOPortRegister                 | 
        //                Indexed                                               | InterruptMode | Number | Register | Register16 | RegisterIndirect
        // opcodeparam := *OPENINGPAR numexpr CLOSINGPAR  | *BIT | *FLAGCONDITION | *FLAGS | *OPENINGPAR REGISTER CLOSINGPAR | 
        //                *OPENINGPAR REGISTER16 (PLUS|MINUS) numexpr CLOSINGPAR | *INTERRUPTMODE | *numexpr | *REGISTER | *REGISTER16 | *OPENINGPAR REGISTER16 CLOSINGPAR              
        private void ParseOpcodeParam()
        {
            AsmToken nextToken = lexer.PeekNextToken();
            if (nextToken.Type == AsmTokenType.BIT)
            {
                AsmToken bitToken = lexer.GetNextToken();
                InstructionLineParam bitParam = new InstructionLineParam() 
                {   
                    Type = InstructionLineParamType.Bit,
                    Bit = AsmLexer.ParseBitToken(bitToken.Text)
                };
                bitParam.Tokens.Add(bitToken);
                prgLine.OpCodeParameters.Add(bitParam);
            }
            else if (nextToken.Type == AsmTokenType.FLAGCONDITION)
            {
                AsmToken flagConditionToken = lexer.GetNextToken();
                InstructionLineParam flagConditionParam = new InstructionLineParam()
                {
                    Type = InstructionLineParamType.FlagCondition,
                    FlagCondition = AsmLexer.ParseFlagConditionToken(flagConditionToken.Text)
                };
                flagConditionParam.Tokens.Add(flagConditionToken);
                prgLine.OpCodeParameters.Add(flagConditionParam);
            }
            else if (nextToken.Type == AsmTokenType.FLAGS)
            {
                AsmToken flagsToken = lexer.GetNextToken();
                InstructionLineParam flagsParam = new InstructionLineParam()
                {
                    Type = InstructionLineParamType.Flags
                };
                flagsParam.Tokens.Add(flagsToken);
                prgLine.OpCodeParameters.Add(flagsParam);
            }
            else if (nextToken.Type == AsmTokenType.INTERRUPTMODE)
            {
                AsmToken interruptModeToken = lexer.GetNextToken();
                InstructionLineParam interruptModeParam = new InstructionLineParam()
                {
                    Type = InstructionLineParamType.InterruptMode,
                    InterruptMode = AsmLexer.ParseInterruptModeToken(interruptModeToken.Text)
                };
                interruptModeParam.Tokens.Add(interruptModeToken);
                prgLine.OpCodeParameters.Add(interruptModeParam);
            }
            else if (nextToken.Type == AsmTokenType.REGISTER)
            {
                AsmToken registerToken = lexer.GetNextToken();
                InstructionLineParam registerParam = new InstructionLineParam()
                {
                    Type = InstructionLineParamType.Register,
                    Register = AsmLexer.ParseRegisterToken(registerToken.Text)
                };
                registerParam.Tokens.Add(registerToken);
                prgLine.OpCodeParameters.Add(registerParam);
            }
            else if (nextToken.Type == AsmTokenType.REGISTER16)
            {
                AsmToken register16Token = lexer.GetNextToken();
                InstructionLineParam register16Param = new InstructionLineParam()
                {
                    Type = InstructionLineParamType.Register16,
                    Register16 = AsmLexer.ParseRegister16Token(register16Token.Text)
                };
                register16Param.Tokens.Add(register16Token);
                prgLine.OpCodeParameters.Add(register16Param);
            }
            else if (nextToken.Type == AsmTokenType.OPENINGPAR)
            {
                lexer.StartTokenList();
                InstructionLineParam resultLineParam = null;

                lexer.ConsumeToken();

                nextToken = lexer.PeekNextToken();
                if (nextToken.Type == AsmTokenType.REGISTER)
                {
                    AsmToken registerToken = lexer.GetNextToken();
                    InstructionLineParam registerParam = new InstructionLineParam()
                    {
                        Type = InstructionLineParamType.IOPortRegister,
                        Register = AsmLexer.ParseRegisterToken(registerToken.Text)
                    };
                    resultLineParam = registerParam;
                    prgLine.OpCodeParameters.Add(registerParam);
                }
                else if (nextToken.Type == AsmTokenType.REGISTER16)
                {
                    AsmToken register16Token = lexer.GetNextToken();
                    Register16 register16 = AsmLexer.ParseRegister16Token(register16Token.Text);

                    nextToken = lexer.PeekNextToken();
                    if (nextToken.Type == AsmTokenType.PLUS || nextToken.Type == AsmTokenType.MINUS)
                    {
                        AsmToken operatorToken = lexer.GetNextToken();
                        NumberExpression numberExpression = ParseNumberExpression();

                        NumberExpression displacementExpression = null;
                        if (operatorToken.Type == AsmTokenType.PLUS)
                        {
                            displacementExpression = numberExpression;
                        }
                        else if (operatorToken.Type == AsmTokenType.MINUS)
                        {
                            displacementExpression = new NumberOperationExpression(
                                new NumberOperand(0),
                                NumberOperationType.Subtraction,
                                numberExpression);
                        }

                        InstructionLineParam indexedParam = new InstructionLineParam()
                        {
                            Type = InstructionLineParamType.Indexed,
                            Register16 = register16,
                            NumberExpression = displacementExpression
                        };
                        resultLineParam = indexedParam;
                        prgLine.OpCodeParameters.Add(indexedParam);
                    }
                    else
                    {
                        InstructionLineParam register16Param = new InstructionLineParam()
                        {
                            Type = InstructionLineParamType.RegisterIndirect,
                            Register16 = register16
                        };
                        resultLineParam = register16Param;
                        prgLine.OpCodeParameters.Add(register16Param);
                    }
                }
                else
                {
                    NumberExpression numberExpression = ParseNumberExpression();
                    InstructionLineParam numberParam = new InstructionLineParam()
                    {
                        Type = InstructionLineParamType.Address,
                        NumberExpression = numberExpression
                    };
                    resultLineParam = numberParam;
                    prgLine.OpCodeParameters.Add(numberParam);
                }

                nextToken = lexer.PeekNextToken();
                if (nextToken.Type == AsmTokenType.CLOSINGPAR)
                {
                    lexer.ConsumeToken();
                }
                else
                {
                    throw new Exception(String.Format("Line {0} : Expecting closing parenthesis ) at column {1} instead of {2} : {3}", lineNumber, nextToken.StartIndex, nextToken.Type.ToString(), nextToken.Text));
                }

                ((List<AsmToken>)resultLineParam.Tokens).AddRange(lexer.EndTokenList());
            }
            else
            {
                lexer.StartTokenList();

                NumberExpression numberExpression = ParseNumberExpression();
                InstructionLineParam numberParam = new InstructionLineParam()
                {
                    Type = InstructionLineParamType.Number,
                    NumberExpression = numberExpression
                };
                prgLine.OpCodeParameters.Add(numberParam);

                ((List<AsmToken>)numberParam.Tokens).AddRange(lexer.EndTokenList());
            }
        }

        // directiveinstruction := DIRECTIVE (directiveparam (COMMA directiveparam)*)?
        private void ParseDirectiveInstruction()
        {
            // Parsing directive name
            AsmToken nextToken = lexer.PeekNextToken();
            if (nextToken.Type == AsmTokenType.DIRECTIVE)
            {
                prgLine.DirectiveNameToken = lexer.GetNextToken();
                prgLine.Type = ProgramLineType.AssemblerDirective;
                prgLine.DirectiveName = prgLine.DirectiveNameToken.Text;
            }
            else
            {
                throw new Exception(String.Format("Line {0} : Expecting opcode name or directive at column {1} instead of {2} : {3}", lineNumber, nextToken.StartIndex, nextToken.Type.ToString(), nextToken.Text));
            }

            // Parsing directive parameters
            nextToken = lexer.PeekNextToken();
            if (nextToken.Type != AsmTokenType.COMMENT && nextToken.Type != AsmTokenType.ENDOFLINE)
            {
                prgLine.DirectiveParameters = new List<DirectiveLineParam>();
                ParseDirectiveParam();

                while (lexer.PeekNextToken().Type == AsmTokenType.COMMA)
                {
                    lexer.ConsumeToken();
                    ParseDirectiveParam();
                }
            }
        }

        // Param type      : NumberExpression    | string
        // directiveparam := numexpr             | STRING
        private void ParseDirectiveParam()
        {
            AsmToken nextToken = lexer.PeekNextToken();
            if (nextToken.Type == AsmTokenType.STRING)
            {
                AsmToken stringToken = lexer.GetNextToken();
                DirectiveLineParam stringParam = new DirectiveLineParam() { StringValue = stringToken.Value.ToString() };
                stringParam.Tokens.Add(stringToken);
                prgLine.DirectiveParameters.Add(stringParam);
            }
            else 
            {
                lexer.StartTokenList();

                NumberExpression numberExpression = ParseNumberExpression();
                DirectiveLineParam numberExpressionParam = new DirectiveLineParam() { NumberExpression = numberExpression };
                prgLine.DirectiveParameters.Add(numberExpressionParam);

                ((List<AsmToken>)numberExpressionParam.Tokens).AddRange(lexer.EndTokenList());
            }
        }

        // numexpr := addexpr ((PLUS|MINUS) addexpr)*
        private NumberExpression ParseNumberExpression()
        {
            NumberExpression operand1 = ParseAddExpression();

            AsmToken nextToken = lexer.PeekNextToken();
            while (nextToken.Type == AsmTokenType.PLUS || nextToken.Type == AsmTokenType.MINUS)
            {
                lexer.ConsumeToken();

                NumberExpression operand2 = ParseAddExpression();
                if (nextToken.Type == AsmTokenType.PLUS)
                {
                    operand1 = new NumberOperationExpression(operand1, NumberOperationType.Addition, operand2);
                }
                else if (nextToken.Type == AsmTokenType.MINUS)
                {
                    operand1 = new NumberOperationExpression(operand1, NumberOperationType.Subtraction, operand2);
                }

                nextToken = lexer.PeekNextToken();
            }

            return operand1;
        }

        // addexpr := operand (MULTIPLY operand)*
        private NumberExpression ParseAddExpression()
        {
            NumberExpression operand1 = ParseOperandExpression();

            AsmToken nextToken = lexer.PeekNextToken();
            while (nextToken.Type == AsmTokenType.MULTIPLY)
            {
                lexer.ConsumeToken();

                NumberExpression operand2 = ParseOperandExpression();
                operand1 = new NumberOperationExpression(operand1, NumberOperationType.Multiplication, operand2);

                nextToken = lexer.PeekNextToken();
            }

            return operand1;
        }

        // operand := MINUS? NUMBER | SYMBOL | DOLLAR | '(' numexpr ')'
        private NumberExpression ParseOperandExpression()
        {
            AsmToken nextToken = lexer.PeekNextToken();
            if (nextToken.Type == AsmTokenType.NUMBER)
            {
                AsmToken numberToken = lexer.GetNextToken();
                return new NumberOperand((int)numberToken.Value);
            }
            else if(nextToken.Type == AsmTokenType.MINUS)
            {
                lexer.ConsumeToken();
                AsmToken numberToken = lexer.GetNextToken();
                if (numberToken.Type == AsmTokenType.NUMBER)
                {
                    return new NumberOperand(-(int)numberToken.Value);
                }
                else
                {
                    throw new Exception(String.Format("Line {0} : Expecting number at column {1} instead of {2} : {3}", lineNumber, numberToken.StartIndex, numberToken.Type.ToString(), numberToken.Text));
                }
            }
            else if (nextToken.Type == AsmTokenType.SYMBOL)
            {
                AsmToken symbolToken = lexer.GetNextToken();
                return new SymbolOperand(symbolToken.Text);
            }
            else if (nextToken.Type == AsmTokenType.DOLLAR)
            {
                lexer.ConsumeToken();
                return new LineAddressOperand();
            }
            else if(nextToken.Type == AsmTokenType.OPENINGPAR)
            {
                lexer.ConsumeToken();

                NumberExpression numberExpression = ParseNumberExpression();

                nextToken = lexer.PeekNextToken();
                if (nextToken.Type == AsmTokenType.CLOSINGPAR)
                {
                    lexer.ConsumeToken();
                }
                else
                {
                    throw new Exception(String.Format("Line {0} : Expecting closing parenthesis ) at column {1} instead of {2} : {3}", lineNumber, nextToken.StartIndex, nextToken.Type.ToString(), nextToken.Text));
                }

                return numberExpression;
            }
            else
            {
                throw new Exception(String.Format("Line {0} : Expecting number, symbol, or $ at column {1} instead of {2} : {3}", lineNumber, nextToken.StartIndex, nextToken.Type.ToString(), nextToken.Text));
            }
        }
    }
}

/*
1. Extract line
2. Treat line as comment if first non whitespace char is . or #

COLON           :
COMMA           ,
OPENINGPAR      (
CLOSINGPAR      )
PLUS            +
MINUS           -
OPENINGBRA      [
CLOSINGBRA      ]
 
DOLLAR          $
NUMBER          $[0-9A-F]+

NUMBER          %[01]+
NUMBER          'ascii char' ! must enable escapes \t \' ...

COMMENT         ;.* 
 
SYMBOL          [A-Z][A-Z1-9-_]{0..9} --exclude-- opcodes, registers, flags, directives
OPCODE          (list)
REGISTER        (list) ! AF' -> special ' char not in symbol
FLAG            (list) ! C can be flag only if previous token is OPCODE = CALL, JP, JR, RET
DIRECTIVE       (list)

NUMBER          [0-9]+ 'D'?
BIT             [0-7]  ! 0-7 can be bit only if previous token is OPCODE = BIT, RES, SET 
NUMBER          [0-9A-F]+ 'H'
NUMBER          [0-7]+ ('O'|'Q')
NUMBER          [01]+ 'B'

STRING          "ascii chars"

*/

/*
program := line*

line := (label? instruction)? comment? (\n | EOF)

label := symbol ':'

instruction := (opcode | directive) \s param? (',' param){1..2}

param := bit | immediate | 
         register | flag |
         extended | registerindirect | indexed |
         macroparam

bit := [0-7]

immediate := numexpr

extended := '(' numexpr ')'

registerindirect  := '(' register ')'

indexed := '(IX+' numexpr ')' | '(IY+' numexpr ')'

macroparam := '#' [A-Z]

numexpr := addexpr (('+' | '-' | NOT | '\' | RES) addexpr)*

addexpr := expoexpr ('**' expoexpr)*

expoexpr := multexpr (('*' | '/' | MOD | SHR | SHL) multexpr)*

multexpr := andexpr ((AND | '&') andexpr)*

andexpr := orexpr ((OR | '|' | XOR) orexpr)*

orexpr := exprelem ((EQ | '=' | GT | '>' | LT | '<' | UGT | ULT) exprelem)*

exprelem := literal | symbol | $

comment := ';' [^\n]*

symbol := [A-Z][A-Z1-9-]{0..9} --exclude-- opcodes, registers, flags, directives

literal := decimal | hexadecimal | octal | binary | ascii

decimal := [0-9]+ 'D'?

hexadecimal := [0-9A-F]+ 'H'

decimal := [0-7]+ ('O'|'Q')

binary := [01]+ 'B'

ascii := 'ascii symbol'

opcode := 
ADC
ADD
AND
BIT
CALL
CCF
CP
CPD
CPDR
CPI
CPIR
CPL
DAA
DEC
DI
DJNZ
EI
EX
EXX
HALT
IM
IN
INC
IND
INDR
INI
INIR
JP
JR
LD
LDD
LDDR
LDI
LDIR
NEG
NONI
NOP
OR
OTDR
OTIR
OUT
OUTD
OUTI
POP
PUSH
RES
RESET
RET
RETI
RETN
RL
RLA
RLC
RLCA
RLD
RR
RRA
RRC
RRCA
RRD
RST
SBC
SCF
SET
SLA
SLL
SRA
SRL
SUB
XOR

register :=
A
F
B
C
D
E
H
L
I
R
IXh
IXl
IYh
IYl
AF
AF'
BC
DE
HL
IX
IY
SP

flag :=
C
M
NC
NZ
P
PE
PO
Z

directive :=
ORG
EQU
DATA
RESERVE
DEFL
DEFB
DEFW
DEFS
DEFM
END
MACRO
ENDM
ENT
EXT

(p595 zaks book)
*/