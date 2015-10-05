using Z80Simulator.Instructions;

namespace Z80Simulator.CPU
{
    /// <summary>
    /// ZiLOG Z80 CPU
    /// 
    /// Instructions - Input and Output Group
    /// </summary>
    public partial class Z80CPU
    {
        /// <summary>
        /// OpCodeName = "IN",
        /// Param1Type = AddressingMode.Register,
        /// Param2Type = AddressingMode.IOPortImmediate,
        /// Equation = "A <- (n)",
        /// Param1List = "A"
        /// Param2List = "(n)",
        /// UserManualPage = "269",
        /// Description = "The operand n is placed on the bottom half (A0 through A7) of the address\nbus to select the I/O device at one of 256 possible ports. The contents of the\nAccumulator also appear on the top half (A8 through A15) of the address\nbus at this time. Then one byte from the selected port is placed on the data\nbus and written to the Accumulator (register A) in the CPU.",      
        /// </summary>
        private void Instruction_43_IN_Register_IOPort(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.OD, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.Z);
                InternalDataBus_SampleFrom(Register.A);
                InternalDataBus_SendTo(InternalDataBusConnection.W);
            }
            // MachineCycleType.PR, TStates = 4
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFromAndIncrement(InternalAddressBusConnection.WZ);
            }
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 7)
            {
                InternalDataBus_SendTo(Register.A);
            }
        }

        /// <summary>
        /// OpCodeName = "IN",
        /// Param1Type = AddressingMode.Register,
        /// Param2Type = AddressingMode.IOPortRegister,
        /// Equation = "r <- (C)",
        /// Param1List = "A","B","C","D","E","H","L"
        /// Param2List = "(C)"
        /// UserManualPage = "270",
        /// Description = "The contents of register C are placed on the bottom half (A0 through A7) of\nthe address bus to select the I/O device at one of 256 possible ports. The\ncontents of Register B are placed on the top half (A8 through A15) of the\naddress bus at this time. Then one byte from the selected port is placed on\nthe data bus and written to register r in the CPU. Register r identifies any of\nthe CPU registers shown in the following table, which also indicates the\ncorresponding 3-bit r field for each. The flags are affected, checking the\ninput data.",
        /// ConditionBitsAffected = "S is set if input data is negative, reset otherwise\nZ is set if input data is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is not affected",
        /// Example = "If the contents of register C are 07H, the contents of register B are 10H,\nand byte 7BH is available at the peripheral device mapped to I/O port\naddress 07H. After execution of IN D, (C) register D contains 7BH."
        /// </summary>
        private void Instruction_44_IN_Register_RegisterIOPort(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        {
            // MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                InternalDataBus_SampleFrom(Register.C);
                InternalDataBus_SendTo(InternalDataBusConnection.Z);
                InternalDataBus_SampleFrom(Register.B);
                InternalDataBus_SendTo(InternalDataBusConnection.W);
            }
            // MachineCycleType.PR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFromAndIncrement(InternalAddressBusConnection.WZ);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                InternalDataBus_SendTo((Register)instructionOrigin.OpCode.Param1);
                ComputeFlagsForInput();
            }
        }
        
        /// <summary>
        /// OpCodeName = "IN",
        /// Param1Type = AddressingMode.Flags,
        /// Param2Type = AddressingMode.IOPortRegister,
        /// Param1List = "F"
        /// Param2List = "(C)"
        /// Description = "The ED70 instruction reads from I/O port C, but does not store the result. It just affects the flags like the other IN x,(C) instructions.",
        /// ConditionBitsAffected = "S is set if input data is negative, reset otherwise\nZ is set if input data is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is not affected",
        /// Example = "If the contents of register C are 07H, the contents of register B are 10H,\nand byte 7BH is available at the peripheral device mapped to I/O port\naddress 07H. After execution of IN F, (C) value 7BH is not stored in any register, only flag F is affected."
        /// </summary>
        private void Instruction_45_IN_Flags_RegisterIOPort(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                InternalDataBus_SampleFrom(Register.C);
                InternalDataBus_SendTo(InternalDataBusConnection.Z);
                InternalDataBus_SampleFrom(Register.B);
                InternalDataBus_SendTo(InternalDataBusConnection.W);
            }
            // MachineCycleType.PR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFromAndIncrement(InternalAddressBusConnection.WZ);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                ComputeFlagsForInput();
            }
        }

        /// <summary>
        /// OpCodeName = "IND",
        /// Equation = "(HL) <- (C), B <- B -1, HL <- HL -1",
        /// UserManualPage = "275",
        /// Description = "The contents of register C are placed on the bottom half (A0 through A7) of\nthe address bus to select the I/O device at one of 256 possible ports.\nRegister B may be used as a byte counter, and its contents are placed on the\ntop half (A8 through A15) of the address bus at this time. Then one byte\nfrom the selected port is placed on the data bus and written to the CPU. The\ncontents of the HL register pair are placed on the address bus and the input\nbyte is written to the corresponding location of memory. Finally, the byte\ncounter and register pair HL are decremented.",
        /// ConditionBitsAffected = "S is unknown\nZ is set if B�1 = 0, reset otherwise\nH is unknown\nP/V is unknown\nN is set\nC is not affected",
        /// Example = "If the contents of register C are 07H, the contents of register B are 10H, the\ncontents of the HL register pair are 1000H, and byte 7BH is available at the\nperipheral device mapped to I/O port address 07H. At execution of IND\nmemory location 1000H contains 7BH, the HL register pair contains\n0FFFH, and register B contains 0FH."
        /// </summary>
        private void Instruction_50_IND(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 5
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 9)
            {
                InternalDataBus_SampleFrom(Register.C);
                InternalDataBus_SendTo(InternalDataBusConnection.Z);
                InternalDataBus_SampleFrom(Register.B);
                InternalDataBus_SendTo(InternalDataBusConnection.W);
            }
            // MachineCycleType.PR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFromAndDecrement(InternalAddressBusConnection.WZ);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                ComputeFlagsForIOBlockInstruction(IOBlockInstructionType.IND);
            }
            // MachineCycleType.MW, TStates = 3
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
            }
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 5)
            {
                Register16_Decrement(Register16.HL);
                Register_Decrement(Register.B);
            }
        }

        /// <summary>
        /// OpCodeName = "INDR",
        /// Equation = "(HL) <- (C), B <- B -1, HL <- HL -1",
        /// UserManualPage = "277",
        /// Description = "The contents of register C are placed on the bottom half (A0 through A7)\nof the address bus to select the I/O device at one of 256 possible ports.\nRegister B is used as a byte counter, and its contents are placed on the top\nhalf (A8 through A15) of the address bus at this time. Then one byte from\nthe selected port is placed on the data bus and written to the CPU. The\ncontents of the HL register pair are placed on the address bus and the\ninput byte is written to the corresponding location of memory. Then HL\nand the byte counter are decremented. If decrementing causes B to go to\nzero, the instruction is terminated. If B is not zero, the PC is decremented\nby two and the instruction repeated. Interrupts are recognized and two\nrefresh cycles are executed after each data transfer.\nWhen B is set to zero prior to instruction execution, 256 bytes of data are\ninput.",
        /// ConditionBitsAffected = "S is unknown\nZ is set\nH is unknown\nP/V is unknown\nN is set\nC is not affected",
        /// Example = "If the contents of register C are 07H, the contents of register B are 03H, the\ncontents of the HL register pair are 1000H, and the following sequence of\nbytes are available at the peripheral device mapped to I/O port address 07H:\n51H\nA9H\n03H\nthen at execution of INDR the HL register pair contains 0FFDH, register B\ncontains zero, and memory locations contain the following:\n0FFEH contains 03H\n0FFFH contains A9H\n1000H contains 51H"
        /// </summary>
        private void Instruction_51_INDR(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 5
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 9)
            {
                InternalDataBus_SampleFrom(Register.C);
                InternalDataBus_SendTo(InternalDataBusConnection.Z);
                InternalDataBus_SampleFrom(Register.B);
                InternalDataBus_SendTo(InternalDataBusConnection.W);
            }
            // MachineCycleType.PR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFromAndDecrement(InternalAddressBusConnection.WZ);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                ComputeFlagsForIOBlockInstruction(IOBlockInstructionType.IND);
            }
            // MachineCycleType.MW, TStates = 3
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
            }
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 5)
            {
                Register16_Decrement(Register16.HL);
                Register_Decrement(Register.B);
                SelectAlternateExecutionTimings_IfRegisterIsZero(RegisterZeroCondition.B);
            }
            // --- if B != 0
            // MachineCycleType.CPU, TStates = 5
            if (machineCycleCountAfterInstruction == 4 && halfTStateIndex == 9)
            {
                RegisterPC_RepeatInstruction();
            }
        }

        /// <summary>
        /// OpCodeName = "INI",
        /// Equation = "(HL) <- (C), B <- B -1, HL <- HL + 1",
        /// UserManualPage = "272",
        /// Description = "The contents of register C are placed on the bottom half (A0 through A7) of\nthe address bus to select the I/O device at one of 256 possible ports.\nRegister B may be used as a byte counter, and its contents are placed on the\ntop half (A8 through A15) of the address bus at this time. Then one byte\nfrom the selected port is placed on the data bus and written to the CPU. The\ncontents of the HL register pair are then placed on the address bus and the\ninput byte is written to the corresponding location of memory. Finally, the\nbyte counter is decremented and register pair HL is incremented.",
        /// ConditionBitsAffected = "S is unknown\nZ is set if B�1 = 0, reset otherwise\nH is unknown\nP/V is unknown\nN is set\nC is not affected",
        /// Example = "If the contents of register C are 07H, the contents of register B are 10H, the\ncontents of the HL register pair are 1000H, and byte 7BH is available at the\nperipheral device mapped to I /O port address 07H. At execution of INI\nmemory location 1000H contains 7BH, the HL register pair contains\n1001H, and register B contains 0FH."
        /// </summary>
        private void Instruction_52_INI(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 5
           if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 9)
            {
                InternalDataBus_SampleFrom(Register.C);
                InternalDataBus_SendTo(InternalDataBusConnection.Z);
                InternalDataBus_SampleFrom(Register.B);
                InternalDataBus_SendTo(InternalDataBusConnection.W);
            }
            // MachineCycleType.PR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFromAndIncrement(InternalAddressBusConnection.WZ);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                ComputeFlagsForIOBlockInstruction(IOBlockInstructionType.INI);
            }
            // MachineCycleType.MW, TStates = 3
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
            }
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 5)
            {
                Register16_Increment(Register16.HL);
                Register_Decrement(Register.B);            
            }
        }

        /// <summary>
        /// OpCodeName = "INIR",
        /// Equation = "(HL) <- (C), B <- B -1, HL <- HL + 1",
        /// UserManualPage = "273",
        /// Description = "The contents of register C are placed on the bottom half (A0 through A7) of\nthe address bus to select the I/O device at one of 256 possible ports.\nRegister B is used as a byte counter, and its contents are placed on the top\nhalf (A8 through A15) of the address bus at this time. Then one byte from\nthe selected port is placed on the data bus and written to the CPU. The\ncontents of the HL register pair are placed on the address bus and the input\nbyte is written to the corresponding location of memory. Then register pair\nHL is incremented, the byte counter is decremented. If decrementing causes\nB to go to zero, the instruction is terminated. If B is not zero, the PC is\ndecremented by two and the instruction repeated. Interrupts are recognized\nand two refresh cycles execute after each data transfer.\nNote: if B is set to zero prior to instruction execution, 256 bytes of data\nare input.",
        /// ConditionBitsAffected = "S is unknown\nZ is set\nH is unknown\nP/V is unknown\nN is set\nC is not affected",
        /// Example = "If the contents of register C are 07H, the contents of register B are 03H,\nthe contents of the HL register pair are 1000H, and the following\nsequence of bytes are available at the peripheral device mapped to I/O\nport of address 07H:\n51H\nA9H\n03H\nthen at execution of INIR the HL register pair contains 1003H, register B\ncontains zero, and memory locations contain the following:\n1000H contains 51H\n1001H contains A9H\n1002H contains 03H"
        /// </summary>
        private void Instruction_53_INIR(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 5
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 9)
            {
                InternalDataBus_SampleFrom(Register.C);
                InternalDataBus_SendTo(InternalDataBusConnection.Z);
                InternalDataBus_SampleFrom(Register.B);
                InternalDataBus_SendTo(InternalDataBusConnection.W);
            }
            // MachineCycleType.PR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFromAndIncrement(InternalAddressBusConnection.WZ);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                ComputeFlagsForIOBlockInstruction(IOBlockInstructionType.INI);
            }
            // MachineCycleType.MW, TStates = 3
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
            }
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 5)
            {
                Register16_Increment(Register16.HL);
                Register_Decrement(Register.B);
                SelectAlternateExecutionTimings_IfRegisterIsZero(RegisterZeroCondition.B);
            }
            // --- if B != 0
            // MachineCycleType.CPU, TStates = 5
            if (machineCycleCountAfterInstruction == 4 && halfTStateIndex == 9)
            {
                RegisterPC_RepeatInstruction();
            }
        }

        /// <summary>
        /// OpCodeName = "OTDR",
        /// Equation = "(C) <- (HL), B <- B -1, HL <- HL - 1",
        /// UserManualPage = "286",
        /// Description = "The contents of the HL register pair are placed on the address bus to select a\nlocation in memory. The byte contained in this memory location is temporarily\nstored in the CPU. Then, after the byte counter (B) is decremented,\nthe contents of register C are placed on the bottom half (A0 through A7) of\nthe address bus to select the I/O device at one of 256 possible ports. Register\nB may be used as a byte counter, and its decremented value is placed on\nthe top half (A8 through A15) of the address bus at this time. Next, the byte\nto be output is placed on the data bus and written to the selected peripheral\ndevice. Then, register pair HL is decremented and if the decremented B\nregister is not zero, the Program Counter (PC) is decremented by two and\nthe instruction is repeated. If B has gone to zero, the instruction is terminated.\nInterrupts are recognized and two refresh cycles are executed after\neach data transfer.\nNote: When B is set to zero prior to instruction execution, the instruction\noutputs 256 bytes of data.",
        /// ConditionBitsAffected = "Z is set\nH is unknown\nP/V is unknown\nN is set\nC is not affected",
        /// Example = "If the contents of register C are 07H, the contents of register B are 03H, the\ncontents of the HL register pair are 1000H, and memory locations have the\nfollowing contents:\n0FFEH contains 51H\n0FFFH contains A9H\n1000H contains 03H\nthen at execution of OTDR the HL register pair contain 0FFDH, register B\ncontains zero, and a group of bytes is written to the peripheral device\nmapped to I/O port address 07H in the following sequence:\n03H\nA9H\n51H"
        /// </summary>
        private void Instruction_83_OTDR(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 5
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                Register_Decrement(Register.B);                
            }
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 9)
            {
                InternalDataBus_SampleFrom(Register.C);
                InternalDataBus_SendTo(InternalDataBusConnection.Z);
                InternalDataBus_SampleFrom(Register.B);
                InternalDataBus_SendTo(InternalDataBusConnection.W);
            }
            // MachineCycleType.MR, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
            }
            // MachineCycleType.PW, TStates = 4
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFromAndDecrement(InternalAddressBusConnection.WZ);
            }
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 7)
            {
                Register16_Decrement(Register16.HL);
                ComputeFlagsForIOBlockInstruction(IOBlockInstructionType.OUT);
                SelectAlternateExecutionTimings_IfRegisterIsZero(RegisterZeroCondition.B);
            }
            // --- if B != 0
            // MachineCycleType.CPU, TStates = 5
            if (machineCycleCountAfterInstruction == 4 && halfTStateIndex == 9)
            {
                RegisterPC_RepeatInstruction();
            }
        }

        /// <summary>
        /// OpCodeName = "OTIR",
        /// Equation = "(C) <- (HL), B <- B -1, HL <- HL + 1",
        /// UserManualPage = "283",
        /// Description = "The contents of the HL register pair are placed on the address bus to select\na location in memory. The byte contained in this memory location is temporarily\nstored in the CPU. Then, after the byte counter (B) is decremented, the\ncontents of register C are placed on the bottom half (A0 through A7) of the\naddress bus to select the I/O device at one of 256 possible ports. Register B\nmay be used as a byte counter, and its decremented value is placed on the top\nhalf (A8 through A15) of the address bus at this time. Next, the byte to be\noutput is placed on the data bus and written to the selected peripheral device.\nThen register pair HL is incremented. If the decremented B register is not\nzero, the Program Counter (PC) is decremented by two and the instruction is\nrepeated. If B has gone to zero, the instruction is terminated. Interrupts are\nrecognized and two refresh cycles are executed after each data transfer.\nNote: When B is set to zero prior to instruction execution, the instruction\noutputs 256 bytes of data.",
        /// ConditionBitsAffected = "S is unknown\nZ is set\nH is unknown\nP/V is unknown\nN is set\nC is not affected",
        /// Example = "If the contents of register C are 07H, the contents of register B are 03H, the\ncontents of the HL register pair are 1000H, and memory locations have the\nfollowing contents:\n1000H contains 51H\n1001H contains A9H\n1002H contains 03H\nthen at execution of OTIR the HL register pair contains 1003H, register B\ncontains zero, and a group of bytes is written to the peripheral device\nmapped to I/O port address 07H in the following sequence:\n51H\nA9H\n03H"
        /// </summary>
        private void Instruction_84_OTIR(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 5
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                Register_Decrement(Register.B);
            }
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 9)
            {
                InternalDataBus_SampleFrom(Register.C);
                InternalDataBus_SendTo(InternalDataBusConnection.Z);
                InternalDataBus_SampleFrom(Register.B);
                InternalDataBus_SendTo(InternalDataBusConnection.W);
            }
            // MachineCycleType.MR, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
            }
            // MachineCycleType.PW, TStates = 4
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFromAndIncrement(InternalAddressBusConnection.WZ);
            }
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 7)
            {
                Register16_Increment(Register16.HL);
                ComputeFlagsForIOBlockInstruction(IOBlockInstructionType.OUT);
                SelectAlternateExecutionTimings_IfRegisterIsZero(RegisterZeroCondition.B);
            }
            // --- if B != 0
            // MachineCycleType.CPU, TStates = 5
            if (machineCycleCountAfterInstruction == 4 && halfTStateIndex == 9)
            {
                RegisterPC_RepeatInstruction();
            }
        }

        /// <summary>
        /// OpCodeName = "OUT",
        /// Param1Type = AddressingMode.IOPortImmediate,
        /// Param2Type = AddressingMode.Register,
        /// Equation = "(n) <- A",
        /// Param1List = "(n)"
        /// Param2List = "A"
        /// UserManualPage = "279",
        /// Description = "The operand n is placed on the bottom half (A0 through A7) of the address\nbus to select the I/O device at one of 256 possible ports. The contents of the\nAccumulator (register A) also appear on the top half (A8 through A15) of\nthe address bus at this time. Then the byte contained in the Accumulator is\nplaced on the data bus and written to the selected peripheral device.",
        /// </summary>
        private void Instruction_85_OUT_IOPort_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.OD, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.Z);
                InternalDataBus_SampleFrom(Register.A);
                InternalDataBus_SendTo(InternalDataBusConnection.W);
            }            
            // MachineCycleType.PW, TStates = 4
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFromAndIncrement(InternalAddressBusConnection.WZ);
                InternalDataBus_SampleFrom(Register.A);
                InternalDataBus_SendTo(InternalDataBusConnection.W);
            }
        }

        /// <summary>
        /// OpCodeName = "OUT",
        /// Param1Type = AddressingMode.IOPortRegister,
        /// Equation = "(C) <- 0",
        /// Description = "ED71 simply outs the value 0 to I/O port C.",
        /// ConditionBitsAffected = "None",
        /// Example = "If the contents of register C are 01H, at execution of OUT (C), byte 0 is written to the peripheral device\nmapped to I/O port address 01H."
        /// </summary>
        private void Instruction_86_OUT_RegisterIOPort(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                InternalDataBus_SampleFrom(Register.C);
                InternalDataBus_SendTo(InternalDataBusConnection.Z);
                InternalDataBus_SampleFrom(Register.B);
                InternalDataBus_SendTo(InternalDataBusConnection.W);
            }
            // MachineCycleType.PW, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalDataBus_Reset();
                InternalAddressBus_SampleFromAndIncrement(InternalAddressBusConnection.WZ);
            }
        }

        /// <summary>
        /// OpCodeName = "OUT",
        /// Param1Type = AddressingMode.IOPortRegister,
        /// Param2Type = AddressingMode.Register,
        /// Equation = "(C) <- r",
        /// Param1List = "(C)"
        /// Param2List = "A","B","C","D","E","H","L"
        /// UserManualPage = "280",
        /// Description = "The contents of register C are placed on the bottom half (A0 through A7) of\nthe address bus to select the I/O device at one of 256 possible ports. The\ncontents of Register B are placed on the top half (A8 through A15) of the\naddress bus at this time. Then the byte contained in register r is placed on\nthe data bus and written to the selected peripheral device. Register r\nidentifies any of the CPU registers shown in the following table :\nB, C, D, E, H, L, A",
        /// ConditionBitsAffected = "None",
        /// Example = "If the contents of register C are 01H, and the contents of register D are 5AH,\nat execution of OUT (C),D byte 5AH is written to the peripheral device\nmapped to I/O port address 01H."
        /// </summary>
        private void Instruction_87_OUT_RegisterIOPort_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                InternalDataBus_SampleFrom(Register.C);
                InternalDataBus_SendTo(InternalDataBusConnection.Z);
                InternalDataBus_SampleFrom(Register.B);
                InternalDataBus_SendTo(InternalDataBusConnection.W);
            }
            // MachineCycleType.PW, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom((Register)instructionOrigin.OpCode.Param2);
                InternalAddressBus_SampleFromAndIncrement(InternalAddressBusConnection.WZ);
            }
        }

        /// <summary>
        /// OpCodeName = "OUTD",
        /// Equation = "(C) <- (HL), B <- B -1, HL <- HL - 1",
        /// UserManualPage = "285",
        /// Description = "The contents of the HL register pair are placed on the address bus to select a\nlocation in memory. The byte contained in this memory location is\ntemporarily stored in the CPU. Then, after the byte counter (B) is\ndecremented, the contents of register C are placed on the bottom half (A0\nthrough A7) of the address bus to select the I/O device at one of 256\npossible ports. Register B may be used as a byte counter, and its\ndecremented value is placed on the top half (A8 through A15) of the\naddress bus at this time. Next, the byte to be output is placed on the data bus\nand written to the selected peripheral device. Finally, the register pair HL is\ndecremented.",
        /// ConditionBitsAffected = "S is unknown\nZ is set if B�1 = 0, reset otherwise\nH is unknown\nP/V is unknown\nN is set\nC is not affected",
        /// Example = "If the contents of register C are 07H, the contents of register B are 10H, the\ncontents of the HL register pair are 1000H, and the contents of memory\nlocation 1000H are 59H, at execution of OUTD register B contains 0FH, the\nHL register pair contains 0FFFH, and byte 59H is written to the peripheral\ndevice mapped to I/O port address 07H.
        /// </summary>
        private void Instruction_88_OUTD(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 5
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                Register_Decrement(Register.B);
            }
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 9)
            {
                InternalDataBus_SampleFrom(Register.C);
                InternalDataBus_SendTo(InternalDataBusConnection.Z);
                InternalDataBus_SampleFrom(Register.B);
                InternalDataBus_SendTo(InternalDataBusConnection.W);
            }
            // MachineCycleType.MR, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
            }
            // MachineCycleType.PW, TStates = 4
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFromAndDecrement(InternalAddressBusConnection.WZ);
            }
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 7)
            {
                Register16_Decrement(Register16.HL);
                ComputeFlagsForIOBlockInstruction(IOBlockInstructionType.OUT);
            }
        }

        /// <summary>
        /// OpCodeName = "OUTI",
        /// Equation = "(C) <- (HL), B <- B -1, HL <- HL + 1",
        /// UserManualPage = "282",
        /// Description = "The contents of the HL register pair are placed on the address bus to select a\nlocation in memory. The byte contained in this memory location is\ntemporarily stored in the CPU. Then, after the byte counter (B) is\ndecremented, the contents of register C are placed on the bottom half (A0\nthrough A7) of the address bus to select the I/O device at one of 256\npossible ports. Register B may be used as a byte counter, and its\ndecremented value is placed on the top half (A8 through A15) of the\naddress bus. The byte to be output is placed on the data bus and written to a\nselected peripheral device. Finally, the register pair HL is incremented.",
        /// ConditionBitsAffected = "S is unknown\nZ is set if B�1 = 0, reset otherwise\nH is unknown\nP/V is unknown\nN is set\nC is not affected",
        /// Example = "If the contents of register C are 07H, the contents of register B are 10H, the\ncontents of the HL register pair are 100014, and the contents of memory\naddress 1000H are 5914, then after thee execution of OUTI register B\ncontains 0FH, the HL register pair contains 1001H, and byte 59H is written\nto the peripheral device mapped to I/O port address 07H."
        /// </summary>
        private void Instruction_89_OUTI(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 5
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                Register_Decrement(Register.B);
            }
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 9)
            {
                InternalDataBus_SampleFrom(Register.C);
                InternalDataBus_SendTo(InternalDataBusConnection.Z);
                InternalDataBus_SampleFrom(Register.B);
                InternalDataBus_SendTo(InternalDataBusConnection.W);
            }
            // MachineCycleType.MR, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
            }
            // MachineCycleType.PW, TStates = 4
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFromAndIncrement(InternalAddressBusConnection.WZ);
            }
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 7)
            {
                Register16_Increment(Register16.HL);
                ComputeFlagsForIOBlockInstruction(IOBlockInstructionType.OUT);
            }
        }
    }
}