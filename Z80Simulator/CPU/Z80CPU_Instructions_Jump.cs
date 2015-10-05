using Z80Simulator.Instructions;

namespace Z80Simulator.CPU
{
    /// <summary>
    /// ZiLOG Z80 CPU
    /// 
    /// Instructions - Jump Group
    /// </summary>
    public partial class Z80CPU
    {
        /// <summary>
        /// OpCodeName = "DJNZ",
        /// Param1Type = AddressingMode.Relative,
        /// Param1List = "e",
        /// UserManualPage = "253",
        /// Description = "This instruction is similar to the conditional jump instructions except that\na register value is used to determine branching. The B register is\ndecremented, and if a non zero value remains, the value of the\ndisplacement e is added to the Program Counter (PC). The next\ninstruction is fetched from the location designated by the new contents of\nthe PC. The jump is measured from the address of the instruction Op\nCode and has a range of -126 to +129 bytes. The assembler automatically\nadjusts for the twice incremented PC.\nIf the result of decrementing leaves B with a zero value, the next instruction\nexecuted is taken from the location following this instruction.",
        /// ConditionBitsAffected = "None",
        /// Example = "A typical software routine is used to demonstrate the use of the DJNZ\ninstruction. This routine moves a line from an input buffer (INBUF) to an\noutput buffer (OUTBUF). It moves the bytes until it finds a CR, or until it\nhas moved 80 bytes, whichever occurs first.\nLD B, 80 ,Set up counter\nLD HL, Inbuf ,Set up pointers\nLD DE, Outbuf\nLOOP: LD A, (HL) ,Get next byte from\n,input buffer\nLD (DE), A ,Store in output buffer\nCP ODH ,Is it a CR?\nJR Z, DONE ,Yes finished\nINC HL ,Increment pointers\nINC DE\nDJNZ LOOP ,Loop back if 80\n,bytes have not\n,been moved\nDONE:"
        /// </summary>
        private void Instruction_36_DJNZ_RelativeDisplacement(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 5
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 9)
            {
                Register_Decrement(Register.B);
                SelectAlternateExecutionTimings_IfRegisterIsZero(RegisterZeroCondition.B);
            }
            // MachineCycleType.OD, TStates = 3
            // -- Condition = "B != 0",
            // MachineCycleType.CPU, TStates = 5
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 9)
            {
                InternalAddressBus_SampleFrom(InternalAddressBusConnection.PC);
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromRegisterWZPlusDisplacement();
                InternalAddressBus_SendTo(InternalAddressBusConnection.PC);
            }
        }

        /// <summary>
        /// OpCodeName = "JP",
        /// Equation = "PC <- nn",
        /// Param1List = "nn" ,
        /// UserManualPage = "238",
        /// Description = "Operand nn is loaded to register pair PC (Program Counter). The next\ninstruction is fetched from the location designated by the new contents of\nthe PC.",
        /// ConditionBitsAffected = "None"
        /// </summary>
        private void Instruction_54_JP_Address(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.ODL, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.Z);
            }
            // MachineCycleType.ODH, TStates = 3
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.W);
                InternalAddressBus_SampleFrom(InternalAddressBusConnection.WZ);
                InternalAddressBus_SendTo(InternalAddressBusConnection.PC);
            }
        }

        /// <summary>
        /// OpCodeName = "JP",
        /// Param1Type = AddressingMode.RegisterIndirect,
        /// Equation = "pc <- hL",
        /// Param1List = "(HL)","(IX)","(IY)"
        /// UserManualPage = "250",
        /// Description = "The Program Counter (register pair PC) is loaded with the contents of the\nHL register pair. The next instruction is fetched from the location\ndesignated by the new contents of the PC.",
        /// ConditionBitsAffected = "None",
        /// Example = "If the contents of the Program Counter are 1000H, and the contents of the\nHL register pair are 4800H, at execution of JP (HL) the contents of the\nProgram Counter are 4800H."    
        /// </summary>
        private void Instruction_55_JP_Register16Address(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                InternalAddressBus_SampleFrom((Register16)instructionOrigin.OpCode.Param1);
                InternalAddressBus_SendTo(InternalAddressBusConnection.PC);
            }
        }

        /// <summary>
        /// OpCodeName = "JP",
        /// Param1Type = AddressingMode.FlagCondition,
        /// Param2Type = AddressingMode.Extended,
        /// Equation = "IF cc true, PC <- nn",
        /// Param1List = "C","M","NC","NZ","P","PE","PO","Z",
        /// Param2List = "nn",
        /// UserManualPage = "239",
        /// Description = "If condition cc is true, the instruction loads operand nn to register pair PC\n(Program Counter), and the program continues with the instruction\nbeginning at address nn. If condition cc is false, the Program Counter is\nincremented as usual, and the program continues with the next sequential\ninstruction. Condition cc is programmed as one of eight status that\ncorresponds to condition bits in the Flag Register (register F). These eight\nstatus are defined in the table below :\nCondition Flag\nNZ non zero Z\nZ zero Z\nNC no carry C\nC carry C\nPO parity odd P/V\nPE parity even P/V\nP sign positive S\nM sign negative S",
        /// ConditionBitsAffected = "None",
        /// Example = "If the Carry flag (C flag in the F register) is set and the contents of address\n1520 are 03H, at execution of JP C, 1520H the Program Counter contains\n1520H, and on the next machine cycle the CPD fetches byte 03H from\naddress 1520H."   
        /// </summary>
        private void Instruction_56_JP_FlagCondition_Address(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.ODL, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.Z);
            }
            // MachineCycleType.ODH, TStates = 3
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.W);
                SendOperandToPC_IfFlagIsSet((FlagCondition)instructionOrigin.OpCode.Param1);
            }
        }             

        /// <summary>
        /// OpCodeName = "JR",
        /// Param1Type = AddressingMode.Relative,
        /// Equation = "PC <- PC + e",
        /// Param1List = "e"
        /// UserManualPage = "241",
        /// Description = "This instruction provides for unconditional branching to other segments of\na program. The value of the displacement e is added to the Program\nCounter (PC) and the next instruction is fetched from the location\ndesignated by the new contents of the PC. This jump is measured from the\naddress of the instruction Op Code and has a range of-126 to +129 bytes.\nThe assembler automatically adjusts for the twice incremented PC.",
        /// ConditionBitsAffected = "None"
        /// </summary>
        private void Instruction_57_JR_RelativeDisplacement(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        {
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.OD, TStates = 3
            // MachineCycleType.CPU, TStates = 5
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 9)
            {
                InternalAddressBus_SampleFrom(InternalAddressBusConnection.PC);
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromRegisterWZPlusDisplacement();
                InternalAddressBus_SendTo(InternalAddressBusConnection.PC);
            }
        }

        /// <summary>
        /// OpCodeName = "JR",
        /// Param1Type = AddressingMode.FlagCondition,
        /// Param2Type = AddressingMode.Relative,
        /// Equation = "If C = 0, continue\nIf C = 1, PC <- PC+ e",
        /// Param1List = "C","NC","NZ","Z"
        /// Param2List = "e"
        /// UserManualPage = "242\n244\n246\n248",
        /// Description = "This instruction provides for conditional branching to other segments of a\nprogram depending on the results of a test on the Carry Flag. If the flag is\nequal to a 1, the value of the displacement e is added to the Program\nCounter (PC) and the next instruction is fetched from the location\ndesignated by the new contents of the PC. The jump is measured from the\naddress of the instruction Op Code and has a range of -126 to +129 bytes.\nThe assembler automatically adjusts for the twice incremented PC.\nIf the flag is equal to a 0, the next instruction executed is taken from the\nlocation following this instruction.",
        /// ConditionBitsAffected = "None",
        /// Example = "The Carry flag is set and it is required to jump back four locations from\n480. The assembly language statement is JR C, $ - 4\nThe resulting object code and final PC value is shown below:\nLocation Instruction\n47C <- PC after jump\n47D -\n47E -\n47F -\n480 38\n481 FA (two�s complement - 6)"
        /// </summary>
        private void Instruction_58_JR_FlagCondition_RelativeDisplacement(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                SelectAlternateExecutionTimings_IfFlagIsNotSet((FlagCondition)instructionOrigin.OpCode.Param1);
            }
            // MachineCycleType.OD, TStates = 3
            // -- Condition = "cc == 1",
            // MachineCycleType.CPU, TStates = 5
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 9)
            {
                InternalAddressBus_SampleFrom(InternalAddressBusConnection.PC);
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromRegisterWZPlusDisplacement();
                InternalAddressBus_SendTo(InternalAddressBusConnection.PC);
            }
        }
    }
}