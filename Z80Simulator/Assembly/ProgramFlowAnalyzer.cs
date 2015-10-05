using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Z80Simulator.Instructions;

namespace Z80Simulator.Assembly
{
    [Flags]
    public enum InstructionFlowBehavior
    {
        ContinueToNextInstruction = 0x01,
        JumpToKnownLocation = 0x02,
        JumpToLocationComputedAtRuntime = 0x04,
        PopProgramCounterFromStack = 0x08,
        ReadProgramCounterFromMemory = 0x10,
        RepeatSelfSeveralTimes = 0x20,
        PushProgramCounterToStack = 0x40
    }

    /**
     * Analyze assembly program flow 
     */
    public class ProgramFlowAnalyzer
    {
        public static InstructionFlowBehavior GetInstructionFlowBehavior(InstructionType instructionType)
        {
            switch (instructionType.Index)
            {
                // -- Call and jump instructions --
                //  Instruction_18_CALL_Address                           PC => stack, PC = Param1 (ODh|ODl)
                case 18:
                    return InstructionFlowBehavior.ContinueToNextInstruction | InstructionFlowBehavior.JumpToKnownLocation | InstructionFlowBehavior.PushProgramCounterToStack;
                //  Instruction_19_CALL_FlagCondition_Address             (PC += instruction size) OR (PC => stack, PC = Param2 (ODh|ODl))
                case 19:
                    return InstructionFlowBehavior.ContinueToNextInstruction | InstructionFlowBehavior.JumpToKnownLocation | InstructionFlowBehavior.PushProgramCounterToStack;
                //  Instruction_36_DJNZ_RelativeDisplacement              (PC += instruction size) OR (PC += Param1 (OD))
                case 36:
                    return InstructionFlowBehavior.ContinueToNextInstruction | InstructionFlowBehavior.JumpToKnownLocation;
                //  Instruction_54_JP_Address	                            PC = Param1 (ODh|ODl)
                case 54:
                    return InstructionFlowBehavior.JumpToKnownLocation;
                //  Instruction_55_JP_Register16Address                   ?? PC = Memory.ReadValueAtAddress((Register16)Param1)
                case 55:
                    return InstructionFlowBehavior.JumpToLocationComputedAtRuntime | InstructionFlowBehavior.ReadProgramCounterFromMemory;
                //  Instruction_56_JP_FlagCondition_Address               (PC += instruction size) OR (PC = Param2 (ODh|ODl))
                case 56:
                    return InstructionFlowBehavior.ContinueToNextInstruction | InstructionFlowBehavior.JumpToKnownLocation;
                //  Instruction_57_JR_RelativeDisplacement                PC += Param1 (OD)
                case 57:
                    return InstructionFlowBehavior.JumpToKnownLocation;
                //  Instruction_58_JR_FlagCondition_RelativeDisplacement  (PC += instruction size) OR (PC += Param2 (OD))
                case 58:
                    return InstructionFlowBehavior.ContinueToNextInstruction | InstructionFlowBehavior.JumpToKnownLocation;
                //  Instruction_122_RST_ResetAddress                      PC => stack, PC = (AddressModifiedPageZero)Param1
                case 122:
                    return InstructionFlowBehavior.ContinueToNextInstruction | InstructionFlowBehavior.JumpToKnownLocation | InstructionFlowBehavior.PushProgramCounterToStack;
                // -- Return instructions --
                //  Instruction_96_RET                                    ? stack => PC
                case 96:
                    return InstructionFlowBehavior.JumpToLocationComputedAtRuntime | InstructionFlowBehavior.PopProgramCounterFromStack;
                //  Instruction_97_RET_FlagCondition                      ? (PC += instruction size) OR (stack => PC)	
                case 97:
                    return InstructionFlowBehavior.ContinueToNextInstruction | InstructionFlowBehavior.JumpToLocationComputedAtRuntime | InstructionFlowBehavior.PopProgramCounterFromStack;
                //  Instruction_98_RETI                                   ? stack => PC
                case 98:
                    return InstructionFlowBehavior.JumpToLocationComputedAtRuntime | InstructionFlowBehavior.PopProgramCounterFromStack;
                //  Instruction_99_RETN                                   ? stack => PC
                case 99:
                    return InstructionFlowBehavior.JumpToLocationComputedAtRuntime | InstructionFlowBehavior.PopProgramCounterFromStack;
                // -- Instruction repeating themselves --
                //  Instruction_26_CPDR
                case 26:
                //  Instruction_28_CPIR
                case 28:
                //  Instruction_41_HALT
                case 41:
                //  Instruction_51_INDR
                case 51:
                //  Instruction_53_INIR
                case 53:
                //  Instruction_74_LDDR
                case 74:
                //  Instruction_76_LDIR
                case 76:
                //  Instruction_83_OTDR
                case 83:
                //  Instruction_84_OTIR
                case 84:
                    return InstructionFlowBehavior.ContinueToNextInstruction | InstructionFlowBehavior.RepeatSelfSeveralTimes;
                // -- Additional entry points --
                // (depending of the apperance of : Instruction_42_IM_InterruptMode, and on the occurence of en external exception)
                //  Instruction_157_NMI                                   PC => stack, PC = 0066H
                case 157:
                    return InstructionFlowBehavior.JumpToKnownLocation | InstructionFlowBehavior.PushProgramCounterToStack;
                //  Instruction_158_INT_0                                 ?? (PC += size of the instruction put on the databus by interrupting device) OR (PC set by the instruction put on the databus by interrupting device)
                case 158:
                    return InstructionFlowBehavior.JumpToLocationComputedAtRuntime;
                //  Instruction_159_INT_1                                 PC => stack, PC = 0038H
                case 159:
                    return InstructionFlowBehavior.JumpToKnownLocation | InstructionFlowBehavior.PushProgramCounterToStack;
                //  Instruction_160_INT_2                                 ?? PC => stack, PC = Memory.ReadValueAtAddress(I | databus set by interrupting device / +1)
                case 160:
                    return InstructionFlowBehavior.JumpToLocationComputedAtRuntime | InstructionFlowBehavior.PushProgramCounterToStack | InstructionFlowBehavior.ReadProgramCounterFromMemory;
                // Instruction_161_RESET
                case 161:
                    return InstructionFlowBehavior.JumpToKnownLocation;
                // Any other instruction :                                PC += instruction size
                default:
                    return InstructionFlowBehavior.ContinueToNextInstruction;
            }
        }

        public static InstructionFlowBehavior GenerateOutgoingCall(ProgramLine programLine, IDictionary<string, NumberExpression> programVariables)
        {
            if (programLine.Type == ProgramLineType.OpCodeInstruction)
            {
                // Analyse instruction effect on the program flow
                InstructionFlowBehavior instructionFlowBehavior = ProgramFlowAnalyzer.GetInstructionFlowBehavior(programLine.InstructionCode.InstructionType);

                // - check if the program flow continues to the next line
                programLine.ContinueToNextLine = (instructionFlowBehavior & InstructionFlowBehavior.ContinueToNextInstruction) > 0;

                // - check if the current program line can jump elswhere (either always or depending on a condition)
                if ((instructionFlowBehavior & InstructionFlowBehavior.JumpToKnownLocation) > 0)
                {
                    // Register the outgoing call address in the program line
                    CallSourceType callInstructionType;
                    InstructionLineParam targetAddressParameter;
                    AddressingMode targetAddressParameterType;

                    switch (programLine.InstructionType.Index)
                    {
                        // -- Call and jump instructions --
                        //  Instruction_18_CALL_Address                           PC => stack, PC = Param1 (ODh|ODl)
                        case 18:
                            callInstructionType = CallSourceType.CallInstruction;
                            targetAddressParameter = programLine.OpCodeParameters[0];
                            targetAddressParameterType = programLine.InstructionType.Param1Type.Value;
                            break;
                        //  Instruction_19_CALL_FlagCondition_Address             (PC += instruction size) OR (PC => stack, PC = Param2 (ODh|ODl))
                        case 19:
                            callInstructionType = CallSourceType.CallInstruction;
                            targetAddressParameter = programLine.OpCodeParameters[1];
                            targetAddressParameterType = programLine.InstructionType.Param2Type.Value;
                            break;
                        //  Instruction_36_DJNZ_RelativeDisplacement              (PC += instruction size) OR (PC += Param1 (OD))
                        case 36:
                            callInstructionType = CallSourceType.JumpInstruction;
                            targetAddressParameter = programLine.OpCodeParameters[0];
                            targetAddressParameterType = programLine.InstructionType.Param1Type.Value;
                            break;
                        //  Instruction_54_JP_Address	                            PC = Param1 (ODh|ODl)
                        case 54:
                            callInstructionType = CallSourceType.JumpInstruction;
                            targetAddressParameter = programLine.OpCodeParameters[0];
                            targetAddressParameterType = programLine.InstructionType.Param1Type.Value;
                            break;
                        //  Instruction_56_JP_FlagCondition_Address               (PC += instruction size) OR (PC = Param2 (ODh|ODl))
                        case 56:
                            callInstructionType = CallSourceType.JumpInstruction;
                            targetAddressParameter = programLine.OpCodeParameters[1];
                            targetAddressParameterType = programLine.InstructionType.Param2Type.Value;
                            break;
                        //  Instruction_57_JR_RelativeDisplacement                PC += Param1 (OD)
                        case 57:
                            callInstructionType = CallSourceType.JumpInstruction;
                            targetAddressParameter = programLine.OpCodeParameters[0];
                            targetAddressParameterType = programLine.InstructionType.Param1Type.Value;
                            break;
                        //  Instruction_58_JR_FlagCondition_RelativeDisplacement  (PC += instruction size) OR (PC += Param2 (OD))
                        case 58:
                            callInstructionType = CallSourceType.JumpInstruction;
                            targetAddressParameter = programLine.OpCodeParameters[1];
                            targetAddressParameterType = programLine.InstructionType.Param2Type.Value;
                            break;
                        //  Instruction_122_RST_ResetAddress                      PC => stack, PC = (AddressModifiedPageZero)Param1
                        case 122:
                            callInstructionType = CallSourceType.CallInstruction;
                            targetAddressParameter = programLine.OpCodeParameters[0];
                            targetAddressParameterType = programLine.InstructionType.Param1Type.Value;
                            break;
                        default:
                            throw new NotImplementedException("Unexpected instruction type " + programLine.InstructionType.Index);
                    }

                    // Compute target address
                    int targetAddress;
                    if (targetAddressParameterType == AddressingMode.Relative && !(targetAddressParameter.NumberExpression is SymbolOperand))
                    {
                        targetAddress = programLine.LineAddress + targetAddressParameter.NumberExpression.GetValue(programVariables, programLine) /* + 2 not necessary because the assembler already adjusts when parsing the expression */;
                    }
                    else
                    {
                        targetAddress = targetAddressParameter.NumberExpression.GetValue(programVariables, programLine);
                    }

                    // Register call source and call target
                    CallSource callSource = new CallSource(callInstructionType, programLine.LineAddress, programLine);
                    CallTarget callTarget = new CallTarget(callSource, targetAddress);
                    programLine.OutgoingCall = callTarget;
                }

                return instructionFlowBehavior;
            }
            else
            {
                return 0;
            }
        }

        public static void GenerateIncomingCalls(IEnumerable<ProgramLine> programLines, MemoryMap memoryMap)
        {
            foreach (ProgramLine instructionLine in programLines)
            {
                if (instructionLine.OutgoingCall != null)
                {
                    // Find the memory description of the call target address
                    int targetAddress = instructionLine.OutgoingCall.Address;
                    MemoryCellDescription memDescription = memoryMap.MemoryCellDescriptions[targetAddress];

                    // This description should be a instruction line starting at the same address
                    if (memDescription == null || memDescription.Type != MemoryDescriptionType.ProgramLine)
                    {
                        throw new Exception("Outgoing call at address " + targetAddress + " points to an unknown area in memory");
                    }
                    ProgramLine targetLine = (ProgramLine)memDescription.Description;                    
                    if (targetLine.Type != ProgramLineType.OpCodeInstruction || targetLine.LineAddress != targetAddress)
                    {
                        throw new Exception("Outgoing call from program line " + instructionLine.Text + " at address " + instructionLine.LineAddress + " points in the middle of the instruction " + targetLine.Text + " starting at address " + targetLine.LineAddress + " in memory");
                    }
                    else
                    {
                        // Connect the source and the target
                        instructionLine.OutgoingCall.Line = targetLine;
                        if (targetLine.IncomingCalls == null)
                        {
                            targetLine.IncomingCalls = new List<CallSource>();
                        }
                        targetLine.IncomingCalls.Add(instructionLine.OutgoingCall.Source);
                    }
                }
            }
        }

        /// <summary>
        /// Returns -1 if no replacement can be made (for RST instruction)
        /// </summary>
        public static int GetAddressParameterIndexToReplaceWithLabel(InstructionType instructionType, out bool isRelative)
        {
            InstructionFlowBehavior flowBehavior = GetInstructionFlowBehavior(instructionType);
            if ((flowBehavior & InstructionFlowBehavior.JumpToKnownLocation) == 0)
            {
                throw new InvalidOperationException("Instruction type does not jump to a location know at compile time");
            }
            else
            {
                switch (instructionType.Index)
                {
                    // -- Call and jump instructions --
                    //  Instruction_18_CALL_Address                           PC => stack, PC = Param1 (ODh|ODl)
                    case 18:
                        isRelative = false;
                        return 0;
                    //  Instruction_19_CALL_FlagCondition_Address             (PC += instruction size) OR (PC => stack, PC = Param2 (ODh|ODl))
                    case 19:
                        isRelative = false;
                        return 1;
                    //  Instruction_36_DJNZ_RelativeDisplacement              (PC += instruction size) OR (PC += Param1 (OD))
                    case 36:
                        isRelative = true;
                        return 0;
                    //  Instruction_54_JP_Address	                            PC = Param1 (ODh|ODl)
                    case 54:
                        isRelative = false;
                        return 0;
                    //  Instruction_56_JP_FlagCondition_Address               (PC += instruction size) OR (PC = Param2 (ODh|ODl))
                    case 56:
                        isRelative = false;
                        return 1;
                    //  Instruction_57_JR_RelativeDisplacement                PC += Param1 (OD)
                    case 57:
                        isRelative = true;
                        return 0;
                    //  Instruction_58_JR_FlagCondition_RelativeDisplacement  (PC += instruction size) OR (PC += Param2 (OD))
                    case 58:
                        isRelative = true;
                        return 1;
                    //  Instruction_122_RST_ResetAddress                      PC => stack, PC = (AddressModifiedPageZero)Param1
                    //case 122:
                    default:
                        isRelative = false;
                        return -1;
                }
            }
        }

        // Go further by applying ideas from
        // http://www.backerstreet.com/decompiler/basic_blocks.php
    }
}
