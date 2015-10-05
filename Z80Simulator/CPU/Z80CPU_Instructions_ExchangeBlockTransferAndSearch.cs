using Z80Simulator.Instructions;

namespace Z80Simulator.CPU
{
    /// <summary>
    /// ZiLOG Z80 CPU
    /// 
    /// Instructions - Exchange, Block Transfer, and Search Group
    /// </summary>
    public partial class Z80CPU
    {
        /// <summary>
        /// OpCodeName = "CPD",
        /// Equation = "A - (HL), HL <- HL -1, BC <- BC -1",
        /// UserManualPage = "137",
        /// Description = "The contents of the memory location addressed by the HL register pair is\ncompared with the contents of the Accumulator. In case of a true\ncompare, a condition bit is set. The HL and Byte Counter (register pair\nBC) are decremented.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if A equals (HL), reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is set if BC -1 x 0, reset otherwise\nN is set\nC is not affected",
        /// Example = "If the HL register pair contains 1111H, memory location 1111H contains\n3BH, the Accumulator contains 3BH, and the Byte Counter contains 0001H.\nAt execution of CPD the Byte Counter contains 0000H, the HL register\npair contains 1110H, the flag in the F register sets, and the P/V flag in the F\nregister resets. There is no effect on the contents of the Accumulator or\naddress 1111H."
        /// </summary>
        private void Instruction_25_CPD(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.MR, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                BlockCompare();
            }
            // MachineCycleType.CPU, TStates = 5
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 9)
            {
                Register16_Decrement(Register16.HL);
                Register16_Decrement(Register16.BC);
                InternalAddressBus_SampleFromAndDecrement(InternalAddressBusConnection.WZ);
            }
        }

        /// <summary>
        /// OpCodeName = "CPDR",
        /// Equation = "A - (HL), HL <- HL -1, BC <- BC -1",
        /// UserManualPage = "138",
        /// Description = "The contents of the memory location addressed by the HL register pair is\ncompared with the contents of the Accumulator. In case of a true compare,\na condition bit is set. The HL and BC (Byte Counter) register pairs are\ndecremented. If decrementing causes the BC to go to zero or if A = (HL),\nthe instruction is terminated. If BC is not zero and A = (HL), the program\ncounter is decremented by two and the instruction is repeated. Interrupts are\nrecognized and two refresh cycles execute after each data transfer. When\nBC is set to zero, prior to instruction execution, the instruction loops\nthrough 64 Kbytes if no match is found.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if A = (HL), reset otherwise\nH is set if borrow form bit 4, reset otherwise\nP/V is set if BC -1 ? 0, reset otherwise\nN is set\nC is not affected",
        /// Example = "If the HL register pair contains 1118H, the Accumulator contains F3H, the\nByte Counter contains 0007H, and memory locations have these contents.\n(1118H) contains 52H\n(1117H) contains 00H\n(1116H) contains F3H\nThen, at execution of CPDR the contents of register pair HL are 1115H,\nthe contents of the Byte Counter are 0004H, the P/V flag in the F register\nsets, and the Z flag in the F register sets."
        /// </summary>
        private void Instruction_26_CPDR(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.MR, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                BlockCompare();
            }
            // MachineCycleType.CPU, TStates = 5
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 9)
            {
                Register16_Decrement(Register16.HL);
                Register16_Decrement(Register16.BC);
                InternalAddressBus_SampleFromAndDecrement(InternalAddressBusConnection.WZ);
                SelectAlternateExecutionTimings_IfRegisterIsZero(RegisterZeroCondition.BCZF);
            }
            // --- if BC != 0
            // MachineCycleType.CPU, TStates = 5
            if (machineCycleCountAfterInstruction == 4 && halfTStateIndex == 9)
            {
                RegisterPC_RepeatInstruction();
                InternalAddressBus_SampleFrom(InternalAddressBusConnection.PC);
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromAndIncrement(InternalAddressBusConnection.WZ);
            }
        }

        /// <summary>
        /// OpCodeName = "CPI",
        /// Equation = "A - (HL), HL <- HL +1, BC <- BC -1",
        /// UserManualPage = "134",
        /// Description = "The contents of the memory location addressed by the HL register is\ncompared with the contents of the Accumulator. In case of a true compare,\na condition bit is set. Then HL is incremented and the Byte Counter\n(register pair BC) is decremented.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if A is (HL), reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is set if BC -1 is not 0, reset otherwise\nN is set\nC is not affected",
        /// Example = "If the HL register pair contains 1111H, memory location 1111H contains\n3BH, the Accumulator contains 3BH, and the Byte Counter contains 0001H.\nAt execution of CPI the Byte Counter contains 0000H, the HL register\npair contains 1112H, the Z flag in the F register sets, and the P/V flag in the\nF register resets. There is no effect on the contents of the Accumulator or\naddress 1111H."
        /// </summary>
        private void Instruction_27_CPI(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.MR, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                BlockCompare();
            }
            // MachineCycleType.CPU, TStates = 5
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 9)
            {
                Register16_Increment(Register16.HL);
                Register16_Decrement(Register16.BC);
                InternalAddressBus_SampleFromAndIncrement(InternalAddressBusConnection.WZ);
            }
        }

        /// <summary>
        /// OpCodeName = "CPIR",
        /// Equation = "A - (HL), HL <- HL +1, BC <- BC -1",
        /// UserManualPage = "135",
        /// Description = "The contents of the memory location addressed by the HL register pair is\ncompared with the contents of the Accumulator. In case of a true compare, a\ncondition bit is set. HL is incremented and the Byte Counter (register pair\nBC) is decremented. If decrementing causes BC to go to zero or if A = (HL),\nthe instruction is terminated. If BC is not zero and A ? (HL), the program\ncounter is decremented by two and the instruction is repeated. Interrupts are\nrecognized and two refresh cycles are executed after each data transfer.\nIf BC is set to zero before instruction execution, the instruction loops\nthrough 64 Kbytes if no match is found.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if A equals (HL), reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is set if BC -1 does not equal 0, reset otherwise\nN is set\nC is not affected",
        /// Example = "If the HL register pair contains 1111H, the Accumulator contains F3H, the\nByte Counter contains 0007H, and memory locations have these contents:\n(1111H) contains 52H\n(1112H) contains 00H\n(1113H) contains F3H\nThen, at execution of CPIR the contents of register pair HL is 1114H, the\ncontents of the Byte Counter is 0004H, the P/V flag in the F register sets,\nand the Z flag in the F register sets."
        /// </summary>
        private void Instruction_28_CPIR(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.MR, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                BlockCompare();
            }
            // MachineCycleType.CPU, TStates = 5
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 9)
            {
                Register16_Increment(Register16.HL);
                Register16_Decrement(Register16.BC);
                InternalAddressBus_SampleFromAndIncrement(InternalAddressBusConnection.WZ);
                SelectAlternateExecutionTimings_IfRegisterIsZero(RegisterZeroCondition.BCZF);
            }
            // --- if BC != 0
            // MachineCycleType.CPU, TStates = 5
            if (machineCycleCountAfterInstruction == 4 && halfTStateIndex == 9)
            {
                RegisterPC_RepeatInstruction();
                InternalAddressBus_SampleFrom(InternalAddressBusConnection.PC);
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromAndIncrement(InternalAddressBusConnection.WZ);
            }
        }

        /// <summary>
        /// OpCodeName = "EX",
        /// Param1Type = AddressingMode.Register16,
        /// Param2Type = AddressingMode.Register16,
        /// Equation = "DE <-> HL",
        /// Param1List = "AF"
        /// Param2List = "AF'"
        /// Param1List = "DE"
        /// Param2List = "HL"
        /// UserManualPage = "123",
        /// Description = "The 2-byte contents of register pairs DE and HL are exchanged.",
        /// ConditionBitsAffected = "None",
        /// Example = "If the content of register pair DE is the number 2822H, and the content of\nthe register pair HL is number 499AH, at instruction EX DE, HL the content\nof register pair DE is 499AH, and the content of register pair HL is 2822H."
        /// </summary>
        private void Instruction_38_EX_Register16_Register16(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                SwitchRegister16((Register16)instructionOrigin.OpCode.Param1);
            }
        }        

        /// <summary>
        /// OpCodeName = "EX",
        /// Param1Type = AddressingMode.RegisterIndirect,
        /// Param2Type = AddressingMode.Register16,
        /// Equation = "H <-> (SP+1), L <-> (SP)",
        /// Param1List = "(SP)",
        /// Param2List = "HL","IX","IY"
        /// UserManualPage = "125",
        /// Description = "The low order byte contained in register pair HL is exchanged with the\ncontents of the memory address specified by the contents of register pair SP\n(Stack Pointer), and the high order byte of HL is exchanged with the next\nhighest memory address (SP+1).",
        /// ConditionBitsAffected = "None",
        /// Example = "If the HL register pair contains 7012H, the SP register pair contains 8856H,\nthe memory location 8856H contains byte 11H, and memory location\n8857H contains byte 22H, then the instruction EX (SP), HL results in the\nHL register pair containing number 2211H, memory location 8856H\ncontaining byte 12H, memory location 8857H containing byte 70H and\nStack Pointer containing 8856H."
        /// MEMPTR = rp value after the operation
        /// </summary>
        private void Instruction_39_EX_Register16Address_Register16(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4            
            // MachineCycleType.SRL, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.Z);
            }
            // MachineCycleType.SRH, TStates = 4
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.W);                
            }
            // MachineCycleType.SWH, TStates = 3
            if (machineCycleCountAfterInstruction == 4 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom(RegisterUtils.GetHigherPart((Register16)instructionOrigin.OpCode.Param2));                
            }
            // MachineCycleType.SWL, TStates = 5
            if (machineCycleCountAfterInstruction == 5 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom(RegisterUtils.GetLowerPart((Register16)instructionOrigin.OpCode.Param2));                
            }
            if (machineCycleCountAfterInstruction == 5 && halfTStateIndex == 9)
            {
                InternalDataBus_SampleFrom(InternalDataBusConnection.W);
                InternalDataBus_SendTo(RegisterUtils.GetHigherPart((Register16)instructionOrigin.OpCode.Param2));
                InternalDataBus_SampleFrom(InternalDataBusConnection.Z);
                InternalDataBus_SendTo(RegisterUtils.GetLowerPart((Register16)instructionOrigin.OpCode.Param2));
            }
        }

        /// <summary>
        /// OpCodeName = "EXX",
        /// Equation = "BC <-> BC', DE <-> DE', HL <-> HL'",
        /// UserManualPage = "124",
        /// Description = "Each 2-byte value in register pairs BC, DE, and HL is exchanged with the\n2-byte value in BC', DE', and HL', respectively.",
        /// ConditionBitsAffected = "None",
        /// Example = "If the contents of register pairs BC, DE, and HL are the numbers 445AH,\n3DA2H, and 8859H, respectively, and the contents of register pairs BC',\nDE', and HL' are 0988H, 9300H, and 00E7H, respectively, at instruction\nEXX the contents of the register pairs are as follows: BC' contains 0988H,\nDE' contains 9300H, HL contains 00E7H, BC' contains 445AH, DE'\ncontains 3DA2H, and HL' contains 8859H."
        /// </summary>
        private void Instruction_40_EXX(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                SwitchAlternateRegisterSet();
            }
        }

        /// <summary>
        /// OpCodeName = "LDD",
        /// Equation = "(DE) <- (HL), DE <- DE -1, HL <- HL-1, BC <- BC-1",
        /// UserManualPage = "131",
        /// Description = "This 2-byte instruction transfers a byte of data from the memory location\naddressed by the contents of the HL register pair to the memory location\naddressed by the contents of the DE register pair. Then both of these register\npairs including the BC (Byte Counter) register pair are decremented.",
        /// ConditionBitsAffected = "S is not affected\nZ is not affected\nH is reset\nP/V is set if BC -1 ? 0, reset otherwise\nN is reset\nC is not affected",
        /// Example = "If the HL register pair contains 1111H, memory location 1111H contains\nbyte 88H, the DE register pair contains 2222H, memory location 2222H\ncontains byte 66H, and the BC register pair contains 7H, then instruction\nLDD results in the following contents in register pairs and memory\naddresses:\nHL contains 1110H\n(1111H) contains 88H\nDE contains 2221H\n(2222H) contains 88H\nBC contains 6H"
        /// </summary>
        private void Instruction_73_LDD(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.MR, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
            }
            // MachineCycleType.MW, TStates = 5
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.DE);
            }
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 9)
            {
                Register16_Decrement(Register16.BC);
                Register16_Decrement(Register16.DE);
                Register16_Decrement(Register16.HL);
                ComputeFlagsForMemBlockLoad();
            }
        }

        /// <summary>
        /// OpCodeName = "LDDR",
        /// Equation = "(DE) <- (HL), DE <- DE -1, HL <- HL-1, BC <- BC-1",
        /// UserManualPage = "132",
        /// Description = "This 2-byte instruction transfers a byte of data from the memory\nlocation addressed by the contents of the HL register pair to the memory\nlocation addressed by the contents of the DE register pair. Then both of\nthese registers, as well as the BC (Byte Counter), are decremented. If\ndecrementing causes BC to go to zero, the instruction is terminated. If\nBC is not zero, the program counter is decremented by two and the\ninstruction is repeated. Interrupts are recognized and two refresh cycles\nexecute after each data transfer.\nWhen BC is set to zero, prior to instruction execution, the instruction loops\nthrough 64 Kbytes.",
        /// ConditionBitsAffected = "S is not affected\nZ is not affected\nH is reset\nP/V is reset\nN is reset\nC is not affected",
        /// Example = "If the HL register pair contains 1114H, the DE register pair contains\n2225H, the BC register pair contains 0003H, and memory locations have\nthese contents:\n(1114H) contains A5H (2225H) contains C5H\n(1113H) contains 36H (2224H) contains 59H\n(1112H) contains 88H (2223H) contains 66H\nThen at execution of LDDR the contents of register pairs and memory\nlocations are:\nHL contains 1111H\nDE contains 2222H\nDC contains 0000H\n(1114H) contains A5H (2225H) contains A5H\n(1113H) contains 36H (2224H) contains 36H\n(1112H) contains 88H (2223H) contains 88H"
        /// </summary>
        private void Instruction_74_LDDR(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.MR, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
            }
            // MachineCycleType.MW, TStates = 5
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.DE);
            }
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 9)
            {
                Register16_Decrement(Register16.BC);
                Register16_Decrement(Register16.DE);
                Register16_Decrement(Register16.HL);
                ComputeFlagsForMemBlockLoad();
                SelectAlternateExecutionTimings_IfRegisterIsZero(RegisterZeroCondition.BC);
            }
            // --- if BC != 0
            // MachineCycleType.CPU, TStates = 5
            if (machineCycleCountAfterInstruction == 4 && halfTStateIndex == 9)
            {                
                RegisterPC_RepeatInstruction();
                InternalAddressBus_SampleFrom(InternalAddressBusConnection.PC);
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromAndIncrement(InternalAddressBusConnection.WZ);
            }
        }

        /// <summary>
        /// OpCodeName = "LDI",
        /// Equation = "(DE) <- (HL), DE <- DE + 1, HL <- HL + 1, BC <- BC -1",
        /// UserManualPage = "128",
        /// Description = "A byte of data is transferred from the memory location addressed, by the\ncontents of the HL register pair to the memory location addressed by the\ncontents of the DE register pair. Then both these register pairs are\nincremented and the BC (Byte Counter) register pair is decremented.",
        /// ConditionBitsAffected = "S is not affected\nZ is not affected\nH is reset\nP/V is set if BC -1 ? 0, reset otherwise\nN is reset\nC is not affected",
        /// Example = "If the HL register pair contains 1111H, memory location 1111H contains\nbyte 88H, the DE register pair contains 2222H, the memory location 2222H\ncontains byte 66H, and the BC register pair contains 7H, then the instruction\nLDI results in the following contents in register pairs and memory addresses:\nHL contains 1112H\n(1111H) contains 88H\nDE contains 2223H\n(2222H) contains 88H\nBC contains 6H"
        /// </summary>
        private void Instruction_75_LDI(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.MR, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
            }
            // MachineCycleType.MW, TStates = 5
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.DE);
            }
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 9)
            {
                Register16_Decrement(Register16.BC);
                Register16_Increment(Register16.DE);
                Register16_Increment(Register16.HL);
                ComputeFlagsForMemBlockLoad();
            }
        }

        /// <summary>
        /// OpCodeName = "LDIR",
        /// Equation = "(DE) <- (HL), DE <- DE + 1, HL <- HL + 1, BC <- BC -1",
        /// UserManualPage = "129",
        /// Description = "This 2-byte instruction transfers a byte of data from the memory location\naddressed by the contents of the HL register pair to the memory location\naddressed by the DE register pair. Both these register pairs are incremented\nand the BC (Byte Counter) register pair is decremented. If decrementing\ncauses the BC to go to zero, the instruction is terminated. If BC is not zero,\nthe program counter is decremented by two and the instruction is repeated.\nInterrupts are recognized and two refresh cycles are executed after each\ndata transfer. When BC is set to zero prior to instruction execution, the\ninstruction loops through 64 Kbytes.",
        /// ConditionBitsAffected = "S is not affected\nZ is not affected\nH is reset\nP/V is reset\nN is reset\nC is not affected",
        /// Example = "2222H, the BC register pair contains 0003H, and memory locations have\nthese contents:\n(1111H) contains 88H (2222H) contains 66H\n(1112H) contains 36H (2223H) contains 59H\n(1113H) contains A5H (2224H) contains C5H\nthen at execution of LDIR the contents of register pairs and memory\nlocations are:\nHL contains 1114H\nDE contains 2225H\nBC contains 0000H\n(1111H) contains 88H (2222H) contains 88H\n(1112H) contains 36H (2223H) contains 36H\n(1113H) contains A5H (2224H) contains A5H"
        /// </summary>
        private void Instruction_76_LDIR(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.MR, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
            }
            // MachineCycleType.MW, TStates = 5
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.DE);
            }
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 9)
            {
                Register16_Decrement(Register16.BC);
                Register16_Increment(Register16.DE);
                Register16_Increment(Register16.HL);
                ComputeFlagsForMemBlockLoad();
                SelectAlternateExecutionTimings_IfRegisterIsZero(RegisterZeroCondition.BC);
            }
            // --- if BC != 0
            // MachineCycleType.CPU, TStates = 5
            if (machineCycleCountAfterInstruction == 4 && halfTStateIndex == 9)
            {
                RegisterPC_RepeatInstruction();
                InternalAddressBus_SampleFrom(InternalAddressBusConnection.PC);
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromAndIncrement(InternalAddressBusConnection.WZ);
            }
        }
    }
}