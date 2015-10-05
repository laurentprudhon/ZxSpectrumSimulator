using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Z80Simulator.Instructions;

namespace Z80Simulator.Assembly
{
    public enum ProgramSource { ObjectCodeBinary, AssemblyLanguageText }

    public class Program
    {
        public Program(string sourcePath, ProgramSource sourceType)
        {
            SourcePath = sourcePath;
            SourceType = sourceType;

            Lines = new List<ProgramLine>();
            LinesWithErrors = new List<ProgramLine>();
            Variables = new Dictionary<string, NumberExpression>();
        }

        public string SourcePath { get; private set; }
        public ProgramSource SourceType { get; private set; }
        public int BaseAddress { get; set; }
        public int MaxAddress { get; set; }

        public IList<ProgramLine> Lines { get; set; }

        public int ErrorCount { get; set; }
        public IList<ProgramLine> LinesWithErrors { get; set; }

        public IDictionary<string, NumberExpression> Variables { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (ProgramLine line in Lines)
            {
                sb.AppendLine(line.Text);
            }
            return sb.ToString();
        }

        private IDictionary<int, ProgramLine> _debuggingCache;

        public ProgramLine GetLineFromAddress(int instructionAddress)
        {
            if (_debuggingCache == null)
            {
                _debuggingCache = new Dictionary<int, ProgramLine>();
                foreach (ProgramLine line in Lines)
                {
                    if (line.Type == ProgramLineType.OpCodeInstruction || (line.Type == ProgramLineType.AssemblerDirective && line.DirectiveName.StartsWith("DEF")))
                    {
                        _debuggingCache.Add(line.LineAddress, line);
                    }
                }            
            }
            ProgramLine resultLine;
            _debuggingCache.TryGetValue(instructionAddress, out resultLine);
            return resultLine;
        }

        public int GetAddressFromLine(int programLineNumber)
        {
            ProgramLine programLine = Lines[programLineNumber - 1];
            return programLine.LineAddress;
        }

        // --- Assisted program refactoring ---

        public enum StringTerminationType { FixedLength, InvertedChar, SpecialChar }

        public delegate string DecodeCharacter(byte charCode);
        private delegate bool StringTerminationTest(byte charCode);
        
        public void CommentStringChars(int startAddress, DecodeCharacter decodeChar, StringTerminationType terminationType, int expectedLength, byte terminationChar) 
        {
            int stringLength = 0;
            StringTerminationTest terminationTest = b => stringLength == expectedLength;
            if (terminationType == StringTerminationType.InvertedChar)
            {
                terminationTest = b => b >= 128;
            }
            else if (terminationType == StringTerminationType.SpecialChar)
            {
                terminationTest = b => b == terminationChar; 
            }
            int firstLineNumber = GetLineFromAddress(startAddress).LineNumber;
            for (int lineNumber = firstLineNumber; ; lineNumber++)
            {
                ProgramLine currentLine = Lines[lineNumber-1];
                byte charCode = currentLine.MemoryBytes[0];
                stringLength++;
                if (terminationTest(charCode))
                {
                    return;
                }
                else 
                {
                    AppendCommentToProgramLine(currentLine.LineAddress, decodeChar(charCode));
                }
            }
        }

        public void DefineWordData(int startAddress)
        {
            ProgramLine firstLine = GetLineFromAddress(startAddress);
            if(firstLine.Type != ProgramLineType.AssemblerDirective || firstLine.DirectiveName != "DEFB")
            {
                throw new Exception("Address should point to a DEFB directive");
            }
            ProgramLine secondLine = GetLineFromAddress(startAddress + 1);
            if(secondLine.Type != ProgramLineType.AssemblerDirective || secondLine.DirectiveName != "DEFB")
            {
                throw new Exception("Address + 1 should point to a DEFB directive");
            }

            int lsb = firstLine.DirectiveParameters[0].NumberExpression.GetValue(Variables, firstLine);
            int msb = secondLine.DirectiveParameters[0].NumberExpression.GetValue(Variables, secondLine);
            int word = msb * 256 + lsb;

            string newDirectiveText = "DEFW " + word.ToString("X4") + "H";
            bool hasLeadingWhitespace = firstLine.DirectiveNameToken.LeadingWhiteSpace != null;
            if (hasLeadingWhitespace)
            {
                newDirectiveText = firstLine.DirectiveNameToken.LeadingWhiteSpace.Text + newDirectiveText;
            }
            if(firstLine.LabelToken != null)
            {
                newDirectiveText = firstLine.LabelToken.TextWithLeadingWhiteSpace + (hasLeadingWhitespace ? "" : "\t") + newDirectiveText;
            }
            ProgramLine newDirectiveLine = Assembler.ParseProgramLine(newDirectiveText, firstLine.LineNumber, false);
            newDirectiveLine.LineAddress = firstLine.LineAddress;

            Lines.RemoveAt(firstLine.LineNumber - 1);
            Lines.RemoveAt(firstLine.LineNumber - 1);
            Lines.Insert(firstLine.LineNumber - 1, newDirectiveLine);
            RenumberLinesAfterLineNumber(firstLine.LineNumber);          
        }

        public static void PrefixLabelToProgramLine(string labelName, ProgramLine programLine)
        {
            if (programLine.Label == null)
            {
                // Parse new line from text
                ProgramLine newTextLine = Assembler.ParseProgramLine(labelName + ":" + programLine.Text);

                // Copy textual representation to the original program Line
                programLine.CopyTextualRepresentationFrom(newTextLine);
            }
            else { throw new InvalidOperationException("Program line is already prefixed by a Label"); }
        }

        public static void ReplaceAddressWithSymbolInProgramLine(ProgramLine instructionLine, int addressParameterIndexToReplace, bool addressIsRelative, string symbolName)
        {
            if (addressParameterIndexToReplace == 0 || addressParameterIndexToReplace == 1)
            {
                if (instructionLine.OpCodeParameters != null && instructionLine.OpCodeParameters.Count > 0)
                {
                    InstructionLineParam paramToReplace = instructionLine.OpCodeParameters[addressParameterIndexToReplace];
                    
                    // Find token to replace
                    AsmToken tokenToReplace = paramToReplace.Tokens.FirstOrDefault<AsmToken>(t => t.Type == AsmTokenType.NUMBER);
                    if (tokenToReplace != null)
                    {
                        // Generate new text for the line, replacing only the address token
                        StringBuilder lineText = new StringBuilder();
                        foreach (AsmToken token in instructionLine.AllTokens)
                        {
                            if ((!addressIsRelative && token != tokenToReplace) || !paramToReplace.Tokens.Contains(token))
                            {
                                lineText.Append(token.TextWithLeadingWhiteSpace);
                            }
                            else if (token == tokenToReplace)
                            {
                                if (tokenToReplace.LeadingWhiteSpace != null)
                                {
                                    lineText.Append(tokenToReplace.LeadingWhiteSpace.Text);
                                }
                                else if (addressIsRelative && (paramToReplace.Tokens[0] != tokenToReplace))
                                {
                                    if (paramToReplace.Tokens[0].LeadingWhiteSpace != null)
                                    {
                                        lineText.Append(paramToReplace.Tokens[0].LeadingWhiteSpace.Text);
                                    }
                                }
                                lineText.Append(symbolName);
                            }
                        }

                        // Parse new line from text
                        ProgramLine newTextLine = Assembler.ParseProgramLine(lineText.ToString());

                        // Copy textual representation to the original program Line
                        instructionLine.CopyTextualRepresentationFrom(newTextLine);
                    }
                }
            }
        }

        public void PrefixLabelToProgramLine(int lineAddress, string labelName)
        {
            PrefixLabelToProgramLine(lineAddress, labelName, false);
        }

        public void PrefixLabelToProgramLine(int lineAddress, string labelName, bool extendSearchToAllNumbers)
        {
            ProgramLine programLine = GetLineFromAddress(lineAddress);
            if (programLine.Label == null)
            {
                // Parse new line from text
                ProgramLine newTextLine = Assembler.ParseProgramLine(labelName + ":" + programLine.Text);

                // Copy textual representation to the original program Line
                programLine.CopyTextualRepresentationFrom(newTextLine);

                // Update all references in the program
                ReplaceAddressWithSymbol(lineAddress, labelName, extendSearchToAllNumbers);
            }
            else { throw new InvalidOperationException("Program line is already prefixed by a Label"); }
        }

        public void ReplaceAddressWithSymbol(int addressToReplace, string symbolName)
        {
            ReplaceAddressWithSymbol(addressToReplace, symbolName, false);
        }

        public void ReplaceAddressWithSymbol(int addressToReplace, string symbolName, bool extendSearchToAllNumbers)
        {
            if (Variables.ContainsKey(symbolName))
            {
                if (Variables[symbolName].GetValue(Variables, null) != addressToReplace)
                {
                    throw new Exception("Symbol " + symbolName + " already has a different meaning");
                }

            }
            else
            {
                Variables[symbolName] = new NumberOperand(addressToReplace);
            }
            foreach (ProgramLine line in Lines)
            {
                if (line.Type == ProgramLineType.OpCodeInstruction)
                {
                    if (line.InstructionType.Param1Type == AddressingMode.Extended ||
                        line.InstructionType.Param1Type == AddressingMode.Relative ||
                        (extendSearchToAllNumbers && line.InstructionType.Param1Type == AddressingMode.Immediate16))
                    {
                        if (line.OpCodeParameters[0].NumberExpression.GetValue(Variables, line) == addressToReplace)
                        {
                            ReplaceAddressWithSymbolInProgramLine(line, 0, line.InstructionType.Param1Type == AddressingMode.Relative, symbolName);
                        }
                    }
                    if (line.InstructionType.Param2Type == AddressingMode.Extended ||
                        line.InstructionType.Param2Type == AddressingMode.Relative ||
                        (extendSearchToAllNumbers && line.InstructionType.Param2Type == AddressingMode.Immediate16))
                    {
                        if (line.OpCodeParameters[1].NumberExpression.GetValue(Variables, line) == addressToReplace)
                        {
                            ReplaceAddressWithSymbolInProgramLine(line, 1, line.InstructionType.Param2Type == AddressingMode.Relative, symbolName);
                        }
                    }
                }
                else if(extendSearchToAllNumbers && line.Type == ProgramLineType.AssemblerDirective)
                {
                    if (line.DirectiveName == "DEFW" && line.DirectiveParameters[0].NumberExpression.GetValue(Variables, line) == addressToReplace)
                    {
                        // Parse new line from text
                        string newDirectiveText = line.DirectiveNameToken.TextWithLeadingWhiteSpace;
                        if (line.LabelToken != null)
                        {
                            newDirectiveText = line.LabelToken.TextWithLeadingWhiteSpace + newDirectiveText;
                        }
                        if (line.DirectiveParameters[0].Tokens[0].LeadingWhiteSpace != null)
                        {
                            newDirectiveText += line.DirectiveParameters[0].Tokens[0].LeadingWhiteSpace.Text;
                        }
                        else
                        {
                            newDirectiveText += " ";
                        }
                        newDirectiveText += symbolName;
                        ProgramLine newDirectiveLine = Assembler.ParseProgramLine(newDirectiveText);

                        // Copy textual representation to the original program Line
                        line.CopyTextualRepresentationFrom(newDirectiveLine);
                    }
                }
            }
        }

        public void RenameSymbolInProgram(string existingSymbol, string newSymbol)
        {
            if (!Variables.ContainsKey(existingSymbol))
            {
                throw new Exception("Symbol " + existingSymbol + " does not exist, it can not be replaced");
            }
            foreach (ProgramLine line in Lines)
            {
                if (line.Text.Contains(existingSymbol))
                {
                    StringBuilder newLineTextWithRenamedSymbol = new StringBuilder();
                    foreach (AsmToken token in line.AllTokens)
                    {
                        if (token.Type == AsmTokenType.SYMBOL && token.Text == existingSymbol)
                        {
                            if (token.LeadingWhiteSpace != null)
                            {
                                newLineTextWithRenamedSymbol.Append(token.LeadingWhiteSpace.Text);
                            }
                            newLineTextWithRenamedSymbol.Append(newSymbol);
                        }
                        else
                        {
                            newLineTextWithRenamedSymbol.Append(token.TextWithLeadingWhiteSpace);
                        }
                    }
                    ProgramLine newLineWithRenamedSymbol = Assembler.ParseProgramLine(newLineTextWithRenamedSymbol.ToString());

                    line.CopyTextualRepresentationFrom(newLineWithRenamedSymbol);
                }
            }
            Variables.Add(newSymbol, Variables[existingSymbol]);
            Variables.Remove(existingSymbol);
        }
        
        public void AppendCommentToProgramLine(int commentedLineAddress, string comment)
        {
            ProgramLine commentedLine = GetLineFromAddress(commentedLineAddress);

            string newLineTextWithComment = commentedLine.Text + " ; " + comment;
            ProgramLine newLineWithComment = Assembler.ParseProgramLine(newLineTextWithComment);

            commentedLine.CopyTextualRepresentationFrom(newLineWithComment);
        }

        public void InsertCommentAboveProgramLine(int commentedLineAddress, string comment)
        {
            ProgramLine commentedLine = GetLineFromAddress(commentedLineAddress);
            int insertedLineNumber = commentedLine.LineNumber;

            ProgramLine newCommentLine = Assembler.ParseProgramLine("; " + comment, insertedLineNumber, false);
            Lines.Insert(insertedLineNumber - 1, newCommentLine);
            RenumberLinesAfterLineNumber(insertedLineNumber);
        }

        public void RenumberLinesAfterLineNumber(int lastValidLineNumber)
        {
            for (int i = lastValidLineNumber; i < Lines.Count; i++)
            {
                Lines[i].LineNumber = i + 1;
            }
        }
    }

    public class ProgramLine
    {
        public ProgramLineType Type { get; set; }

        // Textual representation

        public int LineNumber { get; set; }

        // ... full text of the program Line 
        public string Text { get; set; }
        
        // Textual representation : Label and Comment

        public string Label { get; set; }
        public AsmToken LabelToken { get; set; }

        public string Comment { get; set; }
        public AsmToken CommentToken { get; set; }

        // Textual representation : Type = OpCodeInstruction

        public string OpCodeName { get; set; }
        public AsmToken OpCodeNameToken { get; set; }

        public byte[] OpCodeBytes { get; set; } // Used to distinguish duplicate opcodes and enable high fidelity disassembly <-> assembly roundtrips
        public IList<AsmToken> OpCodeBytesTokens { get; set; }

        // ... tokens for each opcode parameter
        public IList<InstructionLineParam> OpCodeParameters { get; set; }

        // Textual representation : Type = AssemblerDirective

        public string DirectiveName { get; set; }
        public AsmToken DirectiveNameToken { get; set; }

        // ... tokens for each directive parameters
        public IList<DirectiveLineParam> DirectiveParameters { get; set; }

        // Textual representation : all tokens found in the original text, including separators

        public IList<AsmToken> AllTokens { get; set; }

        // Binary representation

        public int LineAddress { get; set; }

        // Binary representation : Type = OpCodeInstruction

        public InstructionCode InstructionCode { get; set; }
        public InstructionType InstructionType { get; set; }

        public byte OperandByte1 { get; set; }        
        public byte OperandByte2 { get; set; }

        // Binary representation : Type = AssemblerDirective

        public byte[] MemoryBytes { get; set; }

        // Flow analysis

        public bool ExecutedAfterPreviousLine  { get; set; }
        public bool ContinueToNextLine         { get; set; }

        public IList<CallSource> IncomingCalls { get; set; }
        public CallTarget        OutgoingCall  { get; set; }

        // Debug and display

        public bool HasError { get; set; }
        public ProgramLineError Error { get; set; }

        public override string ToString()
        {
            return Text;
        }

        // Utility methods for the refactoring

        internal void CopyTextualRepresentationFrom(ProgramLine newTextLine)
        {
            Text = newTextLine.Text;
            Label = newTextLine.Label;
            LabelToken = newTextLine.LabelToken;
            Comment = newTextLine.Comment;
            CommentToken = newTextLine.CommentToken;
            OpCodeName = newTextLine.OpCodeName;
            OpCodeNameToken = newTextLine.OpCodeNameToken;
            OpCodeBytes = newTextLine.OpCodeBytes;
            OpCodeBytesTokens = newTextLine.OpCodeBytesTokens;
            OpCodeParameters = newTextLine.OpCodeParameters;
            DirectiveName = newTextLine.DirectiveName;
            DirectiveNameToken = newTextLine.DirectiveNameToken;
            DirectiveParameters = newTextLine.DirectiveParameters;
            AllTokens = newTextLine.AllTokens;
        }
    }

    public enum ProgramLineType
    {
        CommentOrWhitespace,
        OpCodeInstruction,
        AssemblerDirective
    }
    
    public class InstructionLineParam
    {
        public InstructionLineParamType Type { get; set; }
        
        // Used for types : Number, Address, Indexed
        public NumberExpression NumberExpression { get; set; }

        // Used for types : Register, IOPortRegister
        public Register Register { get; set; }

        // Used for types :  Register16, RegisterIndirect, Indexed
        public Register16 Register16 { get; set; }
        
        // Other enumerated types
        public Bit Bit { get; set; }
        public FlagCondition FlagCondition  { get; set; }
        public InterruptMode InterruptMode { get; set; }

        // Tokens parsed for this param 
        public IList<AsmToken> Tokens = new List<AsmToken>();

        public string TextWithLeadingWhitespace
        {
            get
            {
                StringBuilder textWithWhitespace = new StringBuilder();
                foreach (AsmToken token in Tokens)
                {
                    textWithWhitespace.Append(token.TextWithLeadingWhiteSpace);
                }
                return textWithWhitespace.ToString();
            }
        }
    }

    public enum InstructionLineParamType
    {
        Address,    // At parsing time, a number between parentheses can be used for the following addressing modes :
                    //->Extended
                    //->IOPortImmediate
        Bit,
        FlagCondition,
        Flags,
        IOPortRegister,
        Indexed,
        InterruptMode,
        Number,     // At parsing time, a simple number can be used for the following addressing modes :
                    //->Immediate
                    //->Relative
                    //->Immediate16
                    //->Extended
                    //->ModifiedPageZero
        Register,
        Register16,
        RegisterIndirect       
    }

    public enum CallSourceType
    {
        Boot,
        Interruption,
        ExternalEntryPoint,
        CallInstruction,
        JumpInstruction,
        CodeRelocation // copy opcodes in the execution path
    }

    public class CallSource
    {
        public CallSource(CallSourceType type, int sourceInstructionAddress, ProgramLine programLine)
        {
            Type = type;
            Address = sourceInstructionAddress;
            Line = programLine;
        }

        public CallSourceType Type    { get; private set; }
        public int            Address { get; private set; }
        public ProgramLine    Line    { get; private set; }

        public int CodeRelocationBytesCount { get; set; }
    }

    public class CallTarget
    {
        public CallTarget(CallSource source, int targetAddress)
        {
            Source = source;
            Address = targetAddress;
        }

        public CallSource     Source       { get; private set; }
        public int            Address      { get; private set; }
        public ProgramLine    Line         { get; set; }
    }

    public class DirectiveLineParam
    {
        public NumberExpression NumberExpression { get; set; }
        public string StringValue { get; set; }

        // Tokens parsed for this param 
        public IList<AsmToken> Tokens = new List<AsmToken>();
    }

    public abstract class NumberExpression
    {
        public abstract int GetValue(IDictionary<string, NumberExpression> variables, ProgramLine prgLine);
    }

    public class NumberOperationExpression : NumberExpression
    {
        protected NumberExpression left;
        protected NumberOperationType operation;
        protected NumberExpression right;

        public NumberOperationExpression(NumberExpression left, NumberOperationType operation, NumberExpression right)
        {
            this.left = left;
            this.operation = operation;
            this.right = right;
        }

        public override int GetValue(IDictionary<string, NumberExpression> variables, ProgramLine prgLine)
        {
            int leftValue = left.GetValue(variables, prgLine);
            int rightValue = right.GetValue(variables, prgLine);
            switch (operation)
            {
                case NumberOperationType.Addition:
                    return leftValue + rightValue;
                case NumberOperationType.Subtraction:
                    return leftValue - rightValue;
                //case NumberOperationType.Multiplication:
                default:
                    return leftValue * rightValue;
            }
        }
    }

    public enum NumberOperationType
    {
        Addition,
        Subtraction,
        Multiplication
    }

    public class RelativeDisplacementFromLabelAddressExpression : NumberOperationExpression
    {
        public RelativeDisplacementFromLabelAddressExpression(SymbolOperand left, NumberOperationType operation, NumberExpression right) 
            : base(left, operation, right)
        { }

        public override int GetValue(IDictionary<string, NumberExpression> variables, ProgramLine prgLine)
        {
            // Compute relative displacement from program line address
            if (((SymbolOperand)left).IsLabel(variables))
            {
                return base.GetValue(variables, prgLine);
            }
            // Return the original value 
            else 
            {
                return left.GetValue(variables, prgLine);
            }
        }
    }

    public class NumberOperand : NumberExpression
    {
        private int value;

        public NumberOperand(int value)
        {
            this.value = value;
        }

        public override int GetValue(IDictionary<string, NumberExpression> variables, ProgramLine prgLine)
        {
            return value;
        }
    }

    public class LabelAddress : NumberExpression
    {
        private int address;

        public LabelAddress(int address)
        {
            this.address = address;
        }

        public override int GetValue(IDictionary<string, NumberExpression> variables, ProgramLine prgLine)
        {
            return address;
        }
     }

    public class SymbolOperand : NumberExpression
    {
        private string symbol;

        public SymbolOperand(string symbol)
        {
            this.symbol = symbol;
        }

        public override int GetValue(IDictionary<string, NumberExpression> variables, ProgramLine prgLine)
        {
            NumberExpression valueExpression;
            if (variables == null || !variables.TryGetValue(symbol, out valueExpression))
            {
                throw new Exception(String.Format("Line {0} : No value has been assigned to symbol {1} in {2}", prgLine.LineNumber, symbol, prgLine.Text));
            }
            return valueExpression.GetValue(variables, prgLine);
        }

        public bool IsLabel(IDictionary<string, NumberExpression> variables)
        {
            NumberExpression valueExpression;
            if (variables == null || !variables.TryGetValue(symbol, out valueExpression))
            {
                return false;
            }
            return valueExpression is LabelAddress;
        }
    }

    public class LineAddressOperand : NumberExpression
    {
        public override int GetValue(IDictionary<string, NumberExpression> variables, ProgramLine prgLine)
        {
            return prgLine.LineAddress;
        }
    }

    public class ProgramLineError
    {
        public int StartColumnIndex { get; set; }
        public int EndColumnIndex { get; set; }

        public string ErrorMessage { get; set; }
    }
}