using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Z80Simulator.Instructions;

namespace Z80Simulator.Assembly
{
    public class Disassembler
    {
        public static Program GenerateProgram(string sourcePath, Stream machinecodeStream, int programStartAddress, CallTarget[] entryPoints, MemoryMap memoryMap)
        {
            // Load machine code in memory
            int b = -1;
            int currentAddress = programStartAddress;
            while((b = machinecodeStream.ReadByte()) >= 0)
            {
                memoryMap.MemoryCells[currentAddress] = (byte)b;
                memoryMap.MemoryCellDescriptions[currentAddress] = null; // Reset previous cell descriptions
                currentAddress++;
            }
            int programEndAddress = currentAddress - 1;

            // Check entry points
            if (entryPoints == null || entryPoints.Length == 0)
            {
                throw new Exception("Entry point adresses are mandatory to try to decompile machine code");
            }
            foreach (CallTarget entryPoint in entryPoints)
            {
                if (entryPoint.Address < programStartAddress || entryPoint.Address > programEndAddress)
                {
                    throw new Exception("Entry point adrress : " + entryPoint.Address + " is out of the range of the program");
                }
            }

            // Initialize program
            Program program = new Program(sourcePath, ProgramSource.ObjectCodeBinary);
            program.BaseAddress = programStartAddress;
            program.MaxAddress = programEndAddress;
            memoryMap.Programs.Add(program);

            program.Lines.Add( Assembler.ParseProgramLine("; **************************************************************************") );
            program.Lines.Add( Assembler.ParseProgramLine("; * Decompiled assembly program for : " + sourcePath) );
            program.Lines.Add( Assembler.ParseProgramLine("; **************************************************************************") );
            program.Lines.Add( Assembler.ParseProgramLine("" ) );
            program.Lines.Add( Assembler.ParseProgramLine("ORG " + FormatHexAddress(programStartAddress)) );
            program.Lines.Add( Assembler.ParseProgramLine("") );

            // Initialize code addresses to explore with entry points received as parameters
            Queue<CallTarget> callTargetsToExplore = new Queue<CallTarget>();
            foreach(CallTarget entryPoint in entryPoints) 
            {
                callTargetsToExplore.Enqueue(entryPoint);
            }

            // The tracing decompiler will follow all the code paths
            // and it will generate the program lines completely out of order.
            // We can't add them to the program in the ascending
            // order of the addresses during the first pass.
            // So we store them in a temporary map  for the second pass.
            IDictionary<int, ProgramLine> generatedProgramLines = new SortedDictionary<int, ProgramLine>();

            // Start from each call target and follow code path
            while(callTargetsToExplore.Count > 0)
            {
                // Dequeue one code address to explore
                CallTarget entryPoint = callTargetsToExplore.Dequeue();
                currentAddress = entryPoint.Address;

                // If this code address was already explored, do nothing
                if (memoryMap.MemoryCellDescriptions[currentAddress] != null)
                {
                    continue;
                }                

                // Check if there is a limit in the number of opcodes to disassemble
                bool stopDisassemblyAfterMaxNumberOfBytes = entryPoint.Source.CodeRelocationBytesCount > 0;
                int maxNumberOfBytes = entryPoint.Source.CodeRelocationBytesCount;
                
                InstructionFlowBehavior instructionFlowBehavior = 0;
                do
                {
                    // Check if instruction start address stays in the boundaries of the current program
                    if (currentAddress < programStartAddress || currentAddress > programEndAddress)
                    {
                        throw new Exception("Entry point address : " + currentAddress + " is out of the range of the program");
                    }

                    // Check if current address was already explored
                    if (memoryMap.MemoryCellDescriptions[currentAddress] != null)
                    {
                        break;
                    }   

                    // Check if the maximum number of bytes to disassemble after the current entry point was reached
                    if (stopDisassemblyAfterMaxNumberOfBytes && (currentAddress - entryPoint.Address) >= maxNumberOfBytes)
                    {
                        break;
                    }

                    // Read one instruction code
                    int instructionStartAddress = currentAddress;
                    byte displacement;
                    InstructionCode instructionCode = ReadOneInstructionCode(memoryMap, ref currentAddress, out displacement);

                    // Check that instruction end address stays in the boundaries of the current program
                    int instructionEndAddress = instructionStartAddress + instructionCode.OpCodeByteCount + instructionCode.OperandsByteCount - 1;
                    if (instructionEndAddress > programEndAddress)
                    {
                        throw new Exception("End of instruction : " + instructionCode.InstructionType.OpCodeName + ", at address : " + instructionStartAddress + " is out of the range of the program");
                    }

                    // Read instruction code operands
                    byte operandByte1 = 0;
                    byte operandByte2 = 0;
                    if (instructionCode.OpCodeByteCount == 3)
                    {
                        operandByte1 = displacement;
                    }
                    else
                    {
                        if (instructionCode.OperandsByteCount >= 1)
                        {
                            operandByte1 = memoryMap.MemoryCells[currentAddress++];
                        }
                        if (instructionCode.OperandsByteCount == 2)
                        {
                            operandByte2 = memoryMap.MemoryCells[currentAddress++];
                        }
                    }

                    // Generate assembly text for the current instruction
                    string instructionLineText = GenerateInstructionLineText(instructionCode, operandByte1, operandByte2);

                    // Create a new program line from its textual representation 
                    ProgramLine programLine = Assembler.ParseProgramLine(instructionLineText);
                    
                    // Register the binary representation of the program line
                    programLine.LineAddress = instructionStartAddress;
                    programLine.InstructionCode = instructionCode;
                    programLine.InstructionType= instructionCode.InstructionType;
                    programLine.OperandByte1 = operandByte1;
                    programLine.OperandByte2 = operandByte2;

                    // Store the generated program line in the temporay map
                    generatedProgramLines.Add(instructionStartAddress, programLine);

                    // Register the signification of all the bytes of the instruction in the memory map
                    MemoryCellDescription instructionDescription = new MemoryCellDescription(MemoryDescriptionType.ProgramLine, programLine);
                    for (int instructionAddress = instructionStartAddress; instructionAddress <= instructionEndAddress; instructionAddress++)
                    {
                        memoryMap.MemoryCellDescriptions[instructionAddress] = instructionDescription;
                    }

                    // Analyse instruction effect on the program flow
                    // - check if the program flow continues to the next line
                    // - check if the current program line can jump elswhere (either always or depending on a condition)
                    //   => register the outgoing call address in the program line
                    instructionFlowBehavior = ProgramFlowAnalyzer.GenerateOutgoingCall(programLine, null);
                    if(programLine.OutgoingCall != null)
                    {
                        // If target address was not explored before, add a new entry point to the list
                        if (memoryMap.MemoryCellDescriptions[programLine.OutgoingCall.Address] == null)
                        {
                            callTargetsToExplore.Enqueue(programLine.OutgoingCall);
                        }
                    }
                }
                // Continue to next instruction if the current instruction enables it
                while ((instructionFlowBehavior & InstructionFlowBehavior.ContinueToNextInstruction) > 0);
            }
            
            // Find all the outgoing calls and register them as incoming calls on their target lines
            ProgramFlowAnalyzer.GenerateIncomingCalls(generatedProgramLines.Values, memoryMap);

            // To enhance the readability of the generated program
            // Find all the incoming calls : 
            // - generate labels for the target lines        
            // Find all the outgoing calls
            // - replace address references with labels in the source lines 
            Assembler.GenerateLabelsForIncomingAndOutgoingCalls(generatedProgramLines.Values, program.Variables);

            // All the memory cells which were not hit during the exploration 
            // of all the code paths must represent data
            MemoryCellDescription dataDescription = new MemoryCellDescription(MemoryDescriptionType.Data, null);
            for (int programAddress = programStartAddress; programAddress <= programEndAddress; programAddress++)
            {
                if (memoryMap.MemoryCellDescriptions[programAddress] == null)
                {
                    // Register in the memory map that this address contains data
                    memoryMap.MemoryCellDescriptions[programAddress] = dataDescription;

                    // Generate a program line to insert this byte of data
                    ProgramLine dataDirectiveLine = Assembler.ParseProgramLine("DEFB " + String.Format("{0:X2}H", memoryMap.MemoryCells[programAddress]));

                    // Register the binary representation of the program line
                    dataDirectiveLine.LineAddress = programAddress;
                    dataDirectiveLine.MemoryBytes = new byte[] { memoryMap.MemoryCells[programAddress] };

                    // Store the generated program line in the temporay map
                    generatedProgramLines.Add(programAddress, dataDirectiveLine);
                }
            }

            // Add all generated program lines in the ascending order of their addresses
            foreach (ProgramLine programLine in generatedProgramLines.Values)
            {
                programLine.LineNumber = program.Lines.Count + 1;
                program.Lines.Add(programLine);
            }

            // Try to compile the generated program and check that it produces the right machine code output
            // NB : !! remove this step when the decompiler is well tested and mature !!
            MemoryMap generatedProgramMachineCode = new MemoryMap(memoryMap.MemoryCells.Length);
            Assembler.CompileProgram(program, programStartAddress, generatedProgramMachineCode);
            for (int programAddress = programStartAddress; programAddress <= programEndAddress; programAddress++)
            {
                if (generatedProgramMachineCode.MemoryCells[programAddress] != memoryMap.MemoryCells[programAddress])
                {
                    throw new Exception("Error in decompiled program at address " + programAddress + " : source byte = " + memoryMap.MemoryCells[programAddress] + ", decompiled program line + " + ((ProgramLine)memoryMap.MemoryCellDescriptions[programAddress].Description).Text + " produced byte = " + generatedProgramMachineCode.MemoryCells[programAddress]);
                }
            }

            return program;
        }            

        private static InstructionCode ReadOneInstructionCode(MemoryMap memoryMap, ref int currentAddress, out byte displacement)
        {
            byte currentOpCodeTable = 0;
            displacement = 0;
            InstructionCode instrCode = null;
            do
            {
                // Read one code byte in memory and look for a coresponding instruction code in the instructions table
                byte codeByte = memoryMap.MemoryCells[currentAddress++];
                instrCode = Z80OpCodes.Tables[currentOpCodeTable, codeByte];

                // If more code bytes are expected, switch to another instruction decoding table
                if(instrCode.SwitchToTableNumber.HasValue)
                {
                    currentOpCodeTable = instrCode.SwitchToTableNumber.Value;
                    if (currentOpCodeTable >= 5)
                    {
                        // 4 bytes opcode, next byte is displacement value
                        displacement = memoryMap.MemoryCells[currentAddress++];
                    }
                }
            }
            // Continue to read code bytes until a complete instruction has been recognized
            while(instrCode.FetchMoreBytes > 0);

            // Special case : NONI[DDH] and NONI[FDH]
            if (instrCode.SwitchToTableNumber.HasValue)
            {
                // A second byte was read, which did not yield a valid instruction code => backtrack
                currentAddress--;
            }

            return instrCode;
        }

        private static string GenerateInstructionLineText(InstructionCode instructionCode, byte operandByte1, byte operandByte2)
        {
            InstructionType instructionType = instructionCode.InstructionType;

            string instructionText =  "\t" + instructionType.OpCodeName;
            if (instructionCode.IsDuplicate)
            {
                string instrCodeBytes = "";
                if(instructionCode.Prefix != null)
                {
                    foreach (byte b in instructionCode.Prefix)
                    {
                        instrCodeBytes += b.ToString("X2")+"H ";
                    }
                }
                instrCodeBytes += instructionCode.OpCode.ToString("X2");
                instructionText += "[" + instrCodeBytes + "H]";
            }
            if (instructionType.Param1Type.HasValue)
            {
                AddressingMode paramType = instructionType.Param1Type.Value;
                string enumString = instructionCode.Param1.ToString();
                instructionText += " " + GetParamTextFromEnumStringAndOperand(instructionType, paramType, enumString, false, operandByte1, operandByte2);
            }
            if (instructionType.Param2Type.HasValue)
            {
                AddressingMode paramType = instructionType.Param2Type.Value;
                string enumString = instructionCode.Param2.ToString();
                instructionText += "," + GetParamTextFromEnumStringAndOperand(instructionType, paramType, enumString, false, operandByte1, operandByte2);
            }
            if (instructionType.Param3Type.HasValue)
            {
                AddressingMode paramType = instructionType.Param3Type.Value;
                string enumString = instructionCode.Param3.ToString();
                instructionText += "," + GetParamTextFromEnumStringAndOperand(instructionType, paramType, enumString, false, operandByte1, operandByte2);
            }
            return instructionText;
        }
        
        public static string GetParamTextFromEnumStringAndOperand(InstructionType instrType, AddressingMode addressingMode, string strenum, bool genericForm, byte operandByte1, byte operandByte2)
        {
            switch (addressingMode)
            {
                case AddressingMode.Bit:
                    return strenum.Substring(1);
                case AddressingMode.Extended:
                    string hexaValue = "nn";
                    if (!genericForm)
                    {
                        hexaValue = String.Format("{0:X2}{1:X2}H", operandByte2, operandByte1);
                    }
                    if (instrType.InstructionGroup == "Jump" || instrType.InstructionGroup == "Call And Return")
                    {
                        return hexaValue;
                    }
                    else
                    {
                        return String.Format("({0})", hexaValue);
                    }
                case AddressingMode.FlagCondition:
                    return strenum;
                case AddressingMode.Flags:
                    return strenum;
                case AddressingMode.Immediate:
                    if (genericForm)
                    {
                        return "n";
                    }
                    else
                    {
                        return String.Format("{0:X2}H", operandByte1);
                    }
                case AddressingMode.Immediate16:
                    if (genericForm)
                    {
                        return "nn";
                    }
                    else
                    {
                        return String.Format("{0:X2}{1:X2}H", operandByte2, operandByte1);
                    }
                case AddressingMode.IOPortImmediate:
                    if (genericForm)
                    {
                        return "(n)";
                    }
                    else
                    {
                        return String.Format("({0:X2}H)", operandByte1);
                    }
                case AddressingMode.IOPortRegister:
                    return "(" + strenum + ")";
                case AddressingMode.Indexed:
                    string d = "+d";
                    if (!genericForm)
                    {
                        sbyte displacement = unchecked((sbyte)operandByte1);
                        d = displacement.ToString();
                        if (displacement >= 0)
                        {
                            d = "+" + d;
                        }
                    }
                    return String.Format("({0}{1})", strenum, d);
                case AddressingMode.InterruptMode:
                    return strenum.Substring(2);
                case AddressingMode.ModifiedPageZero:
                    switch (strenum)
                    {
                        case "rst00":
                            return "0H";
                        case "rst08":
                            return "8H";
                        case "rst10":
                            return "10H";
                        case "rst18":
                            return "18H";
                        case "rst20":
                            return "20H";
                        case "rst28":
                            return "28H";
                        case "rst30":
                            return "30H";
                        case "rst38":
                            return "38H";
                        default:
                            throw new Exception("Value not allowed");
                    }
                case AddressingMode.Register:
                    return strenum;
                case AddressingMode.Register16:
                    return strenum.Replace('2', '\'');
                case AddressingMode.RegisterIndirect:
                    return "(" + strenum + ")";
                case AddressingMode.Relative:
                    if (genericForm)
                    {
                        return "e";
                    }
                    else
                    {
                        int displacement = unchecked((sbyte)operandByte1) + 2; // JR / DJNZ : "The assembler automatically adjusts for the  PC." 
                        return displacement.ToString();
                    }
                default:
                    throw new Exception("Unknown adressing mode");
            }
        }

        private static string FormatHexAddress(int address)
        {
            return String.Format("{0:X4}H", address);
        }
    }
}