using Z80Simulator.Instructions;

namespace Z80Simulator.CPU
{
    /// <summary>
    /// ZiLOG Z80 CPU
    /// 
    /// Instructions - Rotate and Shift Group
    /// </summary>
    public partial class Z80CPU
    {
        /// <summary>
        /// OpCodeName = "RL",
        /// Param1Type = AddressingMode.Register,
        /// Param1List =  "A","B","C","D","E","H","L"
        /// UserManualPage = "202",
        /// Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nThe contents of the m operand are rotated left 1-bit position. The content of\nbit 7 is copied to the Carry flag and the previous content of the Carry flag is\ncopied to bit 0.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise\nN is reset\nC is data from bit 7 of source register",
        /// Example = "If the contents of register D and the Carry flag are\n0 - 1 0 0 0 1 1 1 1\nat execution of RL D the contents of register D and the Carry flag are\n1 - 0 0 0 1 1 1 1 0"
        /// </summary>
        private void Instruction_100_RL_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                InternalDataBus_SampleFrom((Register)instructionOrigin.OpCode.Param1);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Rotate(OperationDirection.Left, true, false);
                InternalDataBus_SendTo((Register)instructionOrigin.OpCode.Param1);
            }
        }

        /// <summary>
        /// OpCodeName = "RL",
        /// Param1Type = AddressingMode.RegisterIndirect,
        /// Param1List =  "(HL)"
        /// UserManualPage = "202",
        /// Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nThe contents of the m operand are rotated left 1-bit position. The content of\nbit 7 is copied to the Carry flag and the previous content of the Carry flag is\ncopied to bit 0.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise\nN is reset\nC is data from bit 7 of source register",
        /// Example = "If the contents of register D and the Carry flag are\n0 - 1 0 0 0 1 1 1 1\nat execution of RL D the contents of register D and the Carry flag are\n1 - 0 0 0 1 1 1 1 0" 
        /// </summary>
        private void Instruction_101_RL_Register16Address(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.MR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                Rotate(OperationDirection.Left, true, false);
            }
            // MachineCycleType.MW, TStates = 3
        } 

        /// <summary>
        /// OpCodeName = "RL",
        /// Param1Type = AddressingMode.Indexed,
        /// Param1List =  "(IX+d)","(IY+d)"
        /// UserManualPage = "202",
        /// Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nThe contents of the m operand are rotated left 1-bit position. The content of\nbit 7 is copied to the Carry flag and the previous content of the Carry flag is\ncopied to bit 0.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise\nN is reset\nC is data from bit 7 of source register",
        /// Example = "If the contents of register D and the Carry flag are\n0 - 1 0 0 0 1 1 1 1\nat execution of RL D the contents of register D and the Carry flag are\n1 - 0 0 0 1 1 1 1 0"
        /// </summary>
        private void Instruction_102_RL_IndexedAddress(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 5
            // MachineCycleType.MR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom(InternalDataBusConnection.W);
                InternalAddressBus_SampleFrom(RegisterUtils.GetFromAddressIndexBase((AddressIndexBase)instructionOrigin.OpCode.Param1));
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromRegisterWZPlusDisplacement();
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                Rotate(OperationDirection.Left, true, false);
            }
            // MachineCycleType.MW, TStates = 3
        }

        /// <summary>
        /// OpCodeName = "RL",
        /// Param1Type = AddressingMode.Indexed,
        /// Param2Type = AddressingMode.Register,
        /// Param1List =  "(IX+d)","(IY+d)"
        /// Param2List =  "A","B","C","D","E","H","L"
        /// Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nThe contents of the m operand are rotated left 1-bit position. The content of\nbit 7 is copied to the Carry flag and the previous content of the Carry flag is\ncopied to bit 0.\nThe undocumented DDCB instructions store the result (if any) of the operation in one of the\nseven all-purpose registers.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise\nN is reset\nC is data from bit 7 of source register",
        /// Example = "If the contents of register D and the Carry flag are\n0 - 1 0 0 0 1 1 1 1\nat execution of RL D the contents of register D and the Carry flag are\n1 - 0 0 0 1 1 1 1 0"
        /// </summary>
        private void Instruction_103_RL_IndexedAddress_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 5
            // MachineCycleType.MR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom(InternalDataBusConnection.W);
                InternalAddressBus_SampleFrom(RegisterUtils.GetFromAddressIndexBase((AddressIndexBase)instructionOrigin.OpCode.Param1));
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromRegisterWZPlusDisplacement();
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                Rotate(OperationDirection.Left, true, false);
                InternalDataBus_SendTo((Register)instructionOrigin.OpCode.Param2);
            }
            // MachineCycleType.MW, TStates = 3
        }

        /// <summary>
        /// OpCodeName = "RLA",
        /// UserManualPage = "191",
        /// Description = "The contents of the Accumulator (register A) are rotated left 1-bit position\nthrough the Carry flag. The previous content of the Carry flag is copied to\nbit 0. Bit 0 is the least-significant bit.",
        /// ConditionBitsAffected = "S is not affected\nZ is not affected\nH is reset\nP/V is not affected\nN is reset\nC is data from bit 7 of Accumulator",
        /// Example = "If the contents of the Accumulator and the Carry flag are\n0 - 0 1 1 1 0 1 1 0\nat execution of RLA the contents of the Accumulator and the Carry flag are\n0 - 1 1 1 0 1 1 0 1"
        /// </summary>
        private void Instruction_104_RLA(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                Rotate(OperationDirection.Left, true, true);
                InternalDataBus_SendTo(Register.A);
            }
        }
        
        /// <summary>
        /// OpCodeName = "RLC",
        /// Param1Type = AddressingMode.Register,
        /// Param1List =  "A","B","C","D","E","H","L"
        /// UserManualPage = "194",
        /// Description = "The contents of register r are rotated left 1-bit position. The content of bit 7\nis copied to the Carry flag and also to bit 0.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise\nN is reset\nC is data from bit 7 of source register",
        /// Example = "If the contents of register r are\n1 0 0 0 1 0 0 0\nat execution of RLC r the contents of register r and the Carry flag are\n1 - 0 0 0 1 0 0 0 1"
        /// </summary>
        private void Instruction_105_RLC_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                InternalDataBus_SampleFrom((Register)instructionOrigin.OpCode.Param1);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Rotate(OperationDirection.Left, false, false);
                InternalDataBus_SendTo((Register)instructionOrigin.OpCode.Param1);
            }
        }

        /// <summary>
        /// OpCodeName = "RLC",
        /// Param1Type = AddressingMode.RegisterIndirect,
        /// Param1List =  "(HL)"
        /// UserManualPage = "196",
        /// Description = "The contents of the memory address specified by the contents of register\npair HL are rotated left 1-bit position. The content of bit 7 is copied to the\nCarry flag and also to bit 0. Bit 0 is the least-significant bit.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise\nN is reset\nC is data from bit 7 of source register",
        /// Example = "If the contents of the HL register pair are 2828H, and the contents of\nmemory location 2828H are\n1 0 0 0 1 0 0 0\nat execution of RLC(HL) the contents of memory location 2828H and the\nCarry flag are\n1 - 0 0 0 1 0 0 0 1"
        /// </summary>
        private void Instruction_106_RLC_Register16Address(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.MR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                Rotate(OperationDirection.Left, false, false);
            }
            // MachineCycleType.MW, TStates = 3
        }

        /// <summary>
        /// OpCodeName = "RLC",
        /// Param1Type = AddressingMode.Indexed,
        /// Param1List =  "(IX+d)","(IY+d)"
        /// UserManualPage = "198\n200",
        /// Description = "The contents of the memory address specified by the sum of the contents of\nthe Index Register IX and a two�s complement displacement integer d, are\nrotated left 1-bit position. The content of bit 7 is copied to the Carry flag\nand also to bit 0. Bit 0 is the least-significant bit.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise\nN is reset\nC is data from bit 7 of source register",
        /// Example = "If the contents of the Index Register IX are 1000H, and the contents of\nmemory location 1022H are\n1 0 0 0 1 0 0 0\nat execution of RLC (IX+2H) the contents of memory location 1002H\nand the Carry flag are\n1 - 0 0 0 1 0 0 0 1"
        /// </summary>
        private void Instruction_107_RLC_IndexedAddress(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 5
            // MachineCycleType.MR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom(InternalDataBusConnection.W);
                InternalAddressBus_SampleFrom(RegisterUtils.GetFromAddressIndexBase((AddressIndexBase)instructionOrigin.OpCode.Param1));
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromRegisterWZPlusDisplacement();
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                Rotate(OperationDirection.Left, false, false);
            }
            // MachineCycleType.MW, TStates = 3
        }

        /// <summary>
        /// OpCodeName = "RLC",
        /// Param1Type = AddressingMode.Indexed,
        /// Param2Type = AddressingMode.Register,
        /// Param1List =  "(IX+d)","(IY+d)"
        /// Param2List =  "A","B","C","D","E","H","L"
        /// Description = "The contents of the memory address specified by the sum of the contents of\nthe Index Register IX and a two�s complement displacement integer d, are\nrotated left 1-bit position. The content of bit 7 is copied to the Carry flag\nand also to bit 0. Bit 0 is the least-significant bit.\nThe undocumented DDCB instructions store the result (if any) of the operation in one of the\nseven all-purpose registers",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise\nN is reset\nC is data from bit 7 of source register",
        /// Example = "If the contents of the Index Register IX are 1000H, and the contents of\nmemory location 1022H are\n1 0 0 0 1 0 0 0\nat execution of RLC (IX+2H) the contents of memory location 1002H\nand the Carry flag are\n1 - 0 0 0 1 0 0 0 1"
        /// </summary>
        private void Instruction_108_RLC_IndexedAddress_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 5
            // MachineCycleType.MR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom(InternalDataBusConnection.W);
                InternalAddressBus_SampleFrom(RegisterUtils.GetFromAddressIndexBase((AddressIndexBase)instructionOrigin.OpCode.Param1));
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromRegisterWZPlusDisplacement();
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                Rotate(OperationDirection.Left, false, false);
                InternalDataBus_SendTo((Register)instructionOrigin.OpCode.Param2);
            }
            // MachineCycleType.MW, TStates = 3
        }
        
        /// <summary>
        /// OpCodeName = "RLCA",
        /// UserManualPage = "190",
        /// Description = "The contents of the Accumulator (register A) are rotated left 1-bit position.\nThe sign bit (bit 7) is copied to the Carry flag and also to bit 0. Bit 0 is the\nleast-significant bit.",
        /// ConditionBitsAffected = "S is not affected\nZ is not affected\nH is reset\nP/V is not affected\nN is reset\nC is data from bit 7 of Accumulator",
        /// Example = "If the contents of the Accumulator are\n1 0 0 0 1 0 0 0\nat execution of RLCA the contents of the Accumulator and Carry flag are\n1 - 0 0 0 1 0 0 0 1"
        /// </summary>
        private void Instruction_109_RLCA(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                Rotate(OperationDirection.Left, false, true);
                InternalDataBus_SendTo(Register.A);
            }
        }
        
        /// <summary>
        /// OpCodeName = "RLD",
        /// UserManualPage = "220",
        /// Description = "The contents of the low order four bits (bits 3, 2, 1, and 0) of the memory\nlocation (HL) are copied to the high order four bits (7, 6, 5, and 4) of that\nsame memory location, the previous contents of those high order four bits\nare copied to the low order four bits of the Accumulator (register A), and\nthe previous contents of the low order four bits of the Accumulator are\ncopied to the low order four bits of memory location (HL). The contents of\nthe high order bits of the Accumulator are unaffected.",
        /// ConditionBitsAffected = "S is set if Accumulator is negative after operation, reset otherwise\nZ is set if Accumulator is zero after operation, reset otherwise\nH is reset\nP/V is set if parity of Accumulator is even after operation, reset otherwise\nN is reset\nC is not affected",
        /// Example = "If the contents of the HL register pair are 5000H, and the contents of the\nAccumulator and memory location 5000H are\n0 1 1 1 1 0 1 0 Accumulator\n0 0 1 1 0 0 0 1 (5000H)\nat execution of RLD the contents of the Accumulator and memory location\n5000H are\n0 1 1 1 0 0 1 1 Accumulator\n0 0 0 1 1 0 1 0 (5000H)" 
        /// </summary>
        private void Instruction_110_RLD(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.MR, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromAndIncrement(InternalAddressBusConnection.WZ);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
            }            
            // MachineCycleType.CPU, TStates = 4
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 0)
            {
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
            }
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 7)
            {
                RotateBCDDigit(OperationDirection.Left);
                ALULeftBuffer_SendTo(ALULeftBufferConnection.A);
            }
            // MachineCycleType.MW, TStates = 3
        }
        
        /// <summary>
        /// OpCodeName = "RR",
        /// Param1Type = AddressingMode.Register,
        /// Param1List =  "A","B","C","D","E","H","L"
        /// UserManualPage = "208",
        /// Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nThe contents of operand m are rotated right 1-bit position through the Carry\nflag. The content of bit 0 is copied to the Carry flag and the previous\ncontent of the Carry flag is copied to bit 7. Bit 0 is the least-significant bit.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise,\nN is reset\nC is data from bit 0 of source register",
        /// Example = "If the contents of register D and the Carry flag are\n1 1 0 1 1 1 0 1 - 0\nat execution of RR D the contents of register D and the Carry flag are\n0 1 1 0 1 1 1 0 - 1"  
        /// </summary>
        private void Instruction_111_RR_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                InternalDataBus_SampleFrom((Register)instructionOrigin.OpCode.Param1);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Rotate(OperationDirection.Right, true, false);
                InternalDataBus_SendTo((Register)instructionOrigin.OpCode.Param1);
            }
        }

        /// <summary>
        /// OpCodeName = "RR",
        /// Param1Type = AddressingMode.RegisterIndirect,
        /// Param1List =  "(HL)"
        /// UserManualPage = "208",
        /// Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nThe contents of operand m are rotated right 1-bit position through the Carry\nflag. The content of bit 0 is copied to the Carry flag and the previous\ncontent of the Carry flag is copied to bit 7. Bit 0 is the least-significant bit.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise,\nN is reset\nC is data from bit 0 of source register",
        /// Example = "If the contents of register D and the Carry flag are\n1 1 0 1 1 1 0 1 - 0\nat execution of RR D the contents of register D and the Carry flag are\n0 1 1 0 1 1 1 0 - 1"
        /// </summary>
        private void Instruction_112_RR_Register16Address(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // Type = MachineCycleType.OCF, TStates = 4
            // MachineCycleType.MR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                Rotate(OperationDirection.Right, true, false);
            }
            // MachineCycleType.MW, TStates = 3
        }

        /// <summary>
        /// OpCodeName = "RR",
        /// Param1Type = AddressingMode.Indexed,
        /// Param1List =  "(IX+d)","(IY+d)"
        /// UserManualPage = "208",
        /// Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nThe contents of operand m are rotated right 1-bit position through the Carry\nflag. The content of bit 0 is copied to the Carry flag and the previous\ncontent of the Carry flag is copied to bit 7. Bit 0 is the least-significant bit.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise,\nN is reset\nC is data from bit 0 of source register",
        /// Example = "If the contents of register D and the Carry flag are\n1 1 0 1 1 1 0 1 - 0\nat execution of RR D the contents of register D and the Carry flag are\n0 1 1 0 1 1 1 0 - 1"
        /// </summary>
        private void Instruction_113_RR_IndexedAddress(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 5
            // MachineCycleType.MR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom(InternalDataBusConnection.W);
                InternalAddressBus_SampleFrom(RegisterUtils.GetFromAddressIndexBase((AddressIndexBase)instructionOrigin.OpCode.Param1));
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromRegisterWZPlusDisplacement();
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                Rotate(OperationDirection.Right, true, false);
            }
            // MachineCycleType.MW, TStates = 3
        }

        /// <summary>
        /// OpCodeName = "RR",
        /// Param1Type = AddressingMode.Indexed,
        /// Param2Type = AddressingMode.Register,
        /// Param1List =  "(IX+d)","(IY+d)"
        /// Param2List =  "A","B","C","D","E","H","L"
        /// Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nThe contents of operand m are rotated right 1-bit position through the Carry\nflag. The content of bit 0 is copied to the Carry flag and the previous\ncontent of the Carry flag is copied to bit 7. Bit 0 is the least-significant bit.\nThe undocumented DDCB instructions store the result (if any) of the operation in one of the\nseven all-purpose registers.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise,\nN is reset\nC is data from bit 0 of source register",
        /// Example = "If the contents of register D and the Carry flag are\n1 1 0 1 1 1 0 1 - 0\nat execution of RR D the contents of register D and the Carry flag are\n0 1 1 0 1 1 1 0 - 1"   
        /// </summary>
        private void Instruction_114_RR_IndexedAddress_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 5
            // MachineCycleType.MR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom(InternalDataBusConnection.W);
                InternalAddressBus_SampleFrom(RegisterUtils.GetFromAddressIndexBase((AddressIndexBase)instructionOrigin.OpCode.Param1));
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromRegisterWZPlusDisplacement();
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                Rotate(OperationDirection.Right, true, false);
                InternalDataBus_SendTo((Register)instructionOrigin.OpCode.Param2);
            }
            // MachineCycleType.MW, TStates = 3
        }
        
        /// <summary>
        /// OpCodeName = "RRA",
        /// UserManualPage = "193",
        /// Description = "The contents of the Accumulator (register A) are rotated right 1-bit position\nthrough the Carry flag. The previous content of the Carry flag is copied to\nbit 7. Bit 0 is the least-significant bit.",
        /// ConditionBitsAffected = "S is not affected\nZ is not affected\nH is reset\nP/V is not affected\nN is reset\nC is data from bit 0 of Accumulator",
        /// Example = "If the contents of the Accumulator are\n1 1 1 0 0 0 0 1 - 0\nat execution of RRA the contents of the Accumulator and the Carry flag are\n0 1 1 1 0 0 0 0 - 1\n\n"
        /// </summary>
        private void Instruction_115_RRA(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4 
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                Rotate(OperationDirection.Right, true, true);
                InternalDataBus_SendTo(Register.A);
            }
        }
        
        /// <summary>
        /// OpCodeName = "RRC",
        /// Param1Type = AddressingMode.Register,
        /// Param1List =  "A","B","C","D","E","H","L"
        /// UserManualPage = "205",
        /// Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nThe contents of the m operand are rotated right 1-bit position. The content\nof bit 0 is copied to the Carry flag and also to bit 7. Bit 0 is the leastsignificant\nbit.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise,\nN is reset\nC is data from bit 0 of source register",
        /// Example = "If the contents of register D and the Carry flag are\n0 0 1 1 0 0 0 1\nat execution of RRC D the contents of register D and the Carry flag are\n1 0 0 1 1 0 0 0 - 1"
        /// </summary>
        private void Instruction_116_RRC_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                InternalDataBus_SampleFrom((Register)instructionOrigin.OpCode.Param1);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Rotate(OperationDirection.Right, false, false);
                InternalDataBus_SendTo((Register)instructionOrigin.OpCode.Param1);
            }
        }

        /// <summary>
        /// OpCodeName = "RRC",
        /// Param1Type = AddressingMode.RegisterIndirect,
        /// Param1List =  "(HL)"
        /// UserManualPage = "205",
        /// Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nThe contents of the m operand are rotated right 1-bit position. The content\nof bit 0 is copied to the Carry flag and also to bit 7. Bit 0 is the leastsignificant\nbit.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise,\nN is reset\nC is data from bit 0 of source register",
        /// Example = "If the contents of register D and the Carry flag are\n0 0 1 1 0 0 0 1\nat execution of RRC D the contents of register D and the Carry flag are\n1 0 0 1 1 0 0 0 - 1"
        /// </summary>
        private void Instruction_117_RRC_Register16Address(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.MR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                Rotate(OperationDirection.Right, false, false);
            }
            // MachineCycleType.MW, TStates = 3
        }

        /// <summary>
        /// OpCodeName = "RRC",
        /// Param1Type = AddressingMode.Indexed,
        /// Param1List =  "(IX+d)","(IY+d)"
        /// UserManualPage = "205",
        /// Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nThe contents of the m operand are rotated right 1-bit position. The content\nof bit 0 is copied to the Carry flag and also to bit 7. Bit 0 is the leastsignificant\nbit.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise,\nN is reset\nC is data from bit 0 of source register",
        /// Example = "If the contents of register D and the Carry flag are\n0 0 1 1 0 0 0 1\nat execution of RRC D the contents of register D and the Carry flag are\n1 0 0 1 1 0 0 0 - 1"
        /// </summary>
        private void Instruction_118_RRC_IndexedAddress(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 5
            // MachineCycleType.MR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom(InternalDataBusConnection.W);
                InternalAddressBus_SampleFrom(RegisterUtils.GetFromAddressIndexBase((AddressIndexBase)instructionOrigin.OpCode.Param1));
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromRegisterWZPlusDisplacement();
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                Rotate(OperationDirection.Right, false, false);
            }
            // MachineCycleType.MW, TStates = 3
        }

        /// <summary>
        /// OpCodeName = "RRC",
        /// Param1Type = AddressingMode.Indexed,
        /// Param2Type = AddressingMode.Register,
        /// Param1List =  "(IX+d)","(IY+d)"
        /// Param2List =  "A","B","C","D","E","H","L"
        /// Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nThe contents of the m operand are rotated right 1-bit position. The content\nof bit 0 is copied to the Carry flag and also to bit 7. Bit 0 is the leastsignificant\nbit.\nThe undocumented DDCB instructions store the result (if any) of the operation in one of the\nseven all-purpose registers.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise,\nN is reset\nC is data from bit 0 of source register",
        /// Example = "If the contents of register D and the Carry flag are\n0 0 1 1 0 0 0 1\nat execution of RRC D the contents of register D and the Carry flag are\n1 0 0 1 1 0 0 0 - 1"
        /// </summary>
        private void Instruction_119_RRC_IndexedAddress_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 5
            // MachineCycleType.MR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom(InternalDataBusConnection.W);
                InternalAddressBus_SampleFrom(RegisterUtils.GetFromAddressIndexBase((AddressIndexBase)instructionOrigin.OpCode.Param1));
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromRegisterWZPlusDisplacement();
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                Rotate(OperationDirection.Right, false, false);
                InternalDataBus_SendTo((Register)instructionOrigin.OpCode.Param2);
            }
            // MachineCycleType.MW, TStates = 3
        }
        
        /// <summary>
        /// OpCodeName = "RRCA",
        /// UserManualPage = "192",
        /// Description = "The contents of the Accumulator (register A) are rotated right 1-bit\nposition. Bit 0 is copied to the Carry flag and also to bit 7. Bit 0 is the leastsignificant\nbit.",
        /// ConditionBitsAffected = "S is not affected\nZ is not affected\nH is reset\nP/V is not affected\nN is reset\nC is data from bit 0 of Accumulator",
        /// Example = "If the contents of the Accumulator are\n0 0 1 0 0 0 1 - 0\nat execution of RRCA the contents of the Accumulator and the Carry flag are\n1 0 0 1 0 0 0 - 1\n\n"
        /// </summary>
        private void Instruction_120_RRCA(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                Rotate(OperationDirection.Right, false, true);
                InternalDataBus_SendTo(Register.A);
            }
        }
        
        /// <summary>
        /// OpCodeName = "RRD",             
        /// UserManualPage = "222",
        /// Description = "The contents of the low order four bits (bits 3, 2, 1, and 0) of memory\nlocation (HL) are copied to the low order four bits of the Accumulator\n(register A). The previous contents of the low order four bits of the\nAccumulator are copied to the high order four bits (7, 6, 5, and 4) of\nlocation (HL), and the previous contents of the high order four bits of (HL)\nare copied to the low order four bits of (HL). The contents of the high order\nbits of the Accumulator are unaffected.",
        /// ConditionBitsAffected = "S is set if Accumulator is negative after operation, reset otherwise\nZ is set if Accumulator is zero after operation, reset otherwise\nH is reset\nP/V is set if parity of Accumulator is even after operation, reset otherwise\nN is reset\nC is not affected",
        /// Example = "If the contents of the HL register pair are 5000H, and the contents of the\nAccumulator and memory location 5000H are\n1 0 0 0 0 1 0 0 Accumulator\n0 0 1 0 0 0 0 0 (5000H)\nat execution of RRD the contents of the Accumulator and memory location\n5000H are\n1 0 0 0 0 0 0 0 Accumulator\n0 1 0 0 0 0 1 0 (5000H)"
        /// </summary>
        private void Instruction_121_RRD(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.MR, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromAndIncrement(InternalAddressBusConnection.WZ);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
            }
            // MachineCycleType.CPU, TStates = 4
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 0)
            {
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
            }
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 7)
            {
                RotateBCDDigit(OperationDirection.Right);
                ALULeftBuffer_SendTo(ALULeftBufferConnection.A);
            }
            // MachineCycleType.MW, TStates = 3
        }
        
        /// <summary>
        /// OpCodeName = "SLA",
        /// Param1Type = AddressingMode.Register,
        /// Param1List =  "A","B","C","D","E","H","L"
        /// UserManualPage = "211",
        /// Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nAn arithmetic shift left 1-bit position is performed on the contents of\noperand m. The content of bit 7 is copied to the Carry flag. Bit 0 is the\nleast-significant bit.",
        /// ConditionBitsAffected = "Z is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is data from bit 7",
        /// Example = "If the contents of register L are\n1 0 1 1 0 0 0 1\nat execution of SLA L the contents of register L and the Carry flag are\n1 - 0 1 1 0 0 0 1 0"
        /// </summary>
        private void Instruction_133_SLA_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                InternalDataBus_SampleFrom((Register)instructionOrigin.OpCode.Param1);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Shift(OperationDirection.Left, ShiftType.Arithmetic);
                InternalDataBus_SendTo((Register)instructionOrigin.OpCode.Param1);
            }
        }

        /// <summary>
        /// OpCodeName = "SLA",
        /// Param1Type = AddressingMode.RegisterIndirect,
        /// Param1List =  "(HL)"
        /// UserManualPage = "211",
        /// Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nAn arithmetic shift left 1-bit position is performed on the contents of\noperand m. The content of bit 7 is copied to the Carry flag. Bit 0 is the\nleast-significant bit.",
        /// ConditionBitsAffected = "Z is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is data from bit 7",
        /// Example = "If the contents of register L are\n1 0 1 1 0 0 0 1\nat execution of SLA L the contents of register L and the Carry flag are\n1 - 0 1 1 0 0 0 1 0"
        /// </summary>
        private void Instruction_134_SLA_Register16Address(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.MR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                Shift(OperationDirection.Left, ShiftType.Arithmetic);
            }
            // MachineCycleType.MW, TStates = 3
        }

        /// <summary>
        /// OpCodeName = "SLA",
        /// Param1Type = AddressingMode.Indexed,
        /// Param1List =  "(IX+d)","(IY+d)"
        /// UserManualPage = "211",
        /// Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nAn arithmetic shift left 1-bit position is performed on the contents of\noperand m. The content of bit 7 is copied to the Carry flag. Bit 0 is the\nleast-significant bit.",
        /// ConditionBitsAffected = "Z is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is data from bit 7",
        /// Example = "If the contents of register L are\n1 0 1 1 0 0 0 1\nat execution of SLA L the contents of register L and the Carry flag are\n1 - 0 1 1 0 0 0 1 0"
        /// </summary>
        private void Instruction_135_SLA_IndexedAddress(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 5
            // MachineCycleType.MR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom(InternalDataBusConnection.W);
                InternalAddressBus_SampleFrom(RegisterUtils.GetFromAddressIndexBase((AddressIndexBase)instructionOrigin.OpCode.Param1));
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromRegisterWZPlusDisplacement();
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                Shift(OperationDirection.Left, ShiftType.Arithmetic);
            }
            // MachineCycleType.MW, TStates = 3
        }

        /// <summary>
        /// OpCodeName = "SLA",
        /// Param1Type = AddressingMode.Indexed,
        /// Param2Type = AddressingMode.Register,
        /// Param1List =  "(IX+d)","(IY+d)"
        /// Param2List =  "A","B","C","D","E","H","L"
        /// Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nAn arithmetic shift left 1-bit position is performed on the contents of\noperand m. The content of bit 7 is copied to the Carry flag. Bit 0 is the\nleast-significant bit.\nThe undocumented DDCB instructions store the result (if any) of the operation in one of the\nseven all-purpose registers.",
        /// ConditionBitsAffected = "Z is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is data from bit 7",
        /// Example = "If the contents of register L are\n1 0 1 1 0 0 0 1\nat execution of SLA L the contents of register L and the Carry flag are\n1 - 0 1 1 0 0 0 1 0"
        /// </summary>
        private void Instruction_136_SLA_IndexedAddress_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 5
            // MachineCycleType.MR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom(InternalDataBusConnection.W);
                InternalAddressBus_SampleFrom(RegisterUtils.GetFromAddressIndexBase((AddressIndexBase)instructionOrigin.OpCode.Param1));
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromRegisterWZPlusDisplacement();
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                Shift(OperationDirection.Left, ShiftType.Arithmetic);
                InternalDataBus_SendTo((Register)instructionOrigin.OpCode.Param2);
            }
            // MachineCycleType.MW, TStates = 3
        }
        
        /// <summary>
        /// OpCodeName = "SLL",
        /// Param1Type = AddressingMode.Register,
        /// Param1List =  "A","B","C","D","E","H","L"
        /// Description = "There are a few\ninstructions missing from the o�cial list, which are usually denoted with SLL (Shift Logical Left).\nIt works like SLA, for one exception: it sets bit 0 (SLA resets it).",
        /// ConditionBitsAffected = "Z is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is data from bit 7",
        /// Example = "If the contents of register L are\n1 0 1 1 0 0 0 1\nat execution of SLL L the contents of register L and the Carry flag are\n1 - 0 1 1 0 0 0 1 1"
        /// </summary>
        private void Instruction_137_SLL_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                InternalDataBus_SampleFrom((Register)instructionOrigin.OpCode.Param1);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Shift(OperationDirection.Left, ShiftType.Logical);
                InternalDataBus_SendTo((Register)instructionOrigin.OpCode.Param1);
            }
        }

        /// <summary>
        /// OpCodeName = "SLL",
        /// Param1Type = AddressingMode.RegisterIndirect,
        /// Param1List =  "(HL)"
        /// Description = "There are a few\ninstructions missing from the o�cial list, which are usually denoted with SLL (Shift Logical Left).\nIt works like SLA, for one exception: it sets bit 0 (SLA resets it).",
        /// ConditionBitsAffected = "Z is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is data from bit 7",
        /// Example = "If the contents of register L are\n1 0 1 1 0 0 0 1\nat execution of SLL L the contents of register L and the Carry flag are\n1 - 0 1 1 0 0 0 1 1" 
        /// </summary>
        private void Instruction_138_SLL_Register16Address(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.MR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                Shift(OperationDirection.Left, ShiftType.Logical);
            }
            // MachineCycleType.MW, TStates = 3
        }

        /// <summary>
        /// OpCodeName = "SLL",
        /// Param1Type = AddressingMode.Indexed,
        /// Param1List =  "(IX+d)","(IY+d)"
        /// Description = "There are a few\ninstructions missing from the o�cial list, which are usually denoted with SLL (Shift Logical Left).\nIt works like SLA, for one exception: it sets bit 0 (SLA resets it).",
        /// ConditionBitsAffected = "Z is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is data from bit 7",
        /// Example = "If the contents of register L are\n1 0 1 1 0 0 0 1\nat execution of SLL L the contents of register L and the Carry flag are\n1 - 0 1 1 0 0 0 1 1"
        /// </summary>
        private void Instruction_139_SLL_IndexedAddress(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 5
            // MachineCycleType.MR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom(InternalDataBusConnection.W);
                InternalAddressBus_SampleFrom(RegisterUtils.GetFromAddressIndexBase((AddressIndexBase)instructionOrigin.OpCode.Param1));
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromRegisterWZPlusDisplacement();
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                Shift(OperationDirection.Left, ShiftType.Logical);
            }
            // MachineCycleType.MW, TStates = 3
        }

        /// <summary>
        /// OpCodeName = "SLL",
        /// Param1Type = AddressingMode.Indexed,
        /// Param2Type = AddressingMode.Register,
        /// Param1List =  "(IX+d)","(IY+d)"
        /// Param2List =  "A","B","C","D","E","H","L"
        /// Description = "There are a few\ninstructions missing from the o�cial list, which are usually denoted with SLL (Shift Logical Left).\nIt works like SLA, for one exception: it sets bit 0 (SLA resets it).\nThe undocumented DDCB instructions store the result (if any) of the operation in one of the\nseven all-purpose registers.",
        /// ConditionBitsAffected = "Z is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is data from bit 7",
        /// Example = "If the contents of register L are\n1 0 1 1 0 0 0 1\nat execution of SLL L the contents of register L and the Carry flag are\n1 - 0 1 1 0 0 0 1 1"
        /// </summary>
        private void Instruction_140_SLL_IndexedAddress_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 5
            // MachineCycleType.MR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom(InternalDataBusConnection.W);
                InternalAddressBus_SampleFrom(RegisterUtils.GetFromAddressIndexBase((AddressIndexBase)instructionOrigin.OpCode.Param1));
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromRegisterWZPlusDisplacement();
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                Shift(OperationDirection.Left, ShiftType.Logical);
                InternalDataBus_SendTo((Register)instructionOrigin.OpCode.Param2);
            }
            // MachineCycleType.MW, TStates = 3
        }
        
        /// <summary>
        /// OpCodeName = "SRA",
        /// Param1Type = AddressingMode.Register,
        /// Param1List =  "A","B","C","D","E","H","L"
        /// UserManualPage = "214",
        /// Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous PLC instructions.\nAn arithmetic shift right 1-bit position is performed on the contents of\noperand m. The content of bit 0 is copied to the Carry flag and the previous\ncontent of bit 7 is unchanged. Bit 0 is the least-significant bit.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is data from bit 0 of source register",
        /// Example = "If the contents of the Index Register IX are 1000H, and the contents of\nmemory location 1003H are\n1 0 1 1 1 0 0 0\nat execution of SRA (IX+3H) the contents of memory location 1003H\nand the Carry flag are\n1 1 0 1 1 1 0 0 - 0"
        /// </summary>
        private void Instruction_141_SRA_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                InternalDataBus_SampleFrom((Register)instructionOrigin.OpCode.Param1);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Shift(OperationDirection.Right, ShiftType.Arithmetic);
                InternalDataBus_SendTo((Register)instructionOrigin.OpCode.Param1);
            }
        }

        /// <summary>
        /// OpCodeName = "SRA",
        /// Param1Type = AddressingMode.RegisterIndirect,
        /// Param1List =  "(HL)"
        /// UserManualPage = "214",
        /// Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous PLC instructions.\nAn arithmetic shift right 1-bit position is performed on the contents of\noperand m. The content of bit 0 is copied to the Carry flag and the previous\ncontent of bit 7 is unchanged. Bit 0 is the least-significant bit.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is data from bit 0 of source register",
        /// Example = "If the contents of the Index Register IX are 1000H, and the contents of\nmemory location 1003H are\n1 0 1 1 1 0 0 0\nat execution of SRA (IX+3H) the contents of memory location 1003H\nand the Carry flag are\n1 1 0 1 1 1 0 0 - 0"
        /// </summary>
        private void Instruction_142_SRA_Register16Address(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.MR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                Shift(OperationDirection.Right, ShiftType.Arithmetic);
            }
            // MachineCycleType.MW, TStates = 3
        }

        /// <summary>
        /// OpCodeName = "SRA",
        /// Param1Type = AddressingMode.Indexed,
        /// Param1List =  "(IX+d)","(IY+d)"
        /// UserManualPage = "214",
        /// Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous PLC instructions.\nAn arithmetic shift right 1-bit position is performed on the contents of\noperand m. The content of bit 0 is copied to the Carry flag and the previous\ncontent of bit 7 is unchanged. Bit 0 is the least-significant bit.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is data from bit 0 of source register",
        /// Example = "If the contents of the Index Register IX are 1000H, and the contents of\nmemory location 1003H are\n1 0 1 1 1 0 0 0\nat execution of SRA (IX+3H) the contents of memory location 1003H\nand the Carry flag are\n1 1 0 1 1 1 0 0 - 0"
        /// </summary>
        private void Instruction_143_SRA_IndexedAddress(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 5
            // MachineCycleType.MR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom(InternalDataBusConnection.W);
                InternalAddressBus_SampleFrom(RegisterUtils.GetFromAddressIndexBase((AddressIndexBase)instructionOrigin.OpCode.Param1));
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromRegisterWZPlusDisplacement();
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                Shift(OperationDirection.Right, ShiftType.Arithmetic);
            }
            // MachineCycleType.MW, TStates = 3
        }

        /// <summary>
        /// OpCodeName = "SRA",
        /// Param1Type = AddressingMode.Indexed,
        /// Param2Type = AddressingMode.Register,
        /// Param1List =  "(IX+d)","(IY+d)"
        /// Param2List =  "A","B","C","D","E","H","L"
        /// Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous PLC instructions.\nAn arithmetic shift right 1-bit position is performed on the contents of\noperand m. The content of bit 0 is copied to the Carry flag and the previous\ncontent of bit 7 is unchanged. Bit 0 is the least-significant bit.\nThe undocumented DDCB instructions store the result (if any) of the operation in one of the\nseven all-purpose registers.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is data from bit 0 of source register",
        /// Example = "If the contents of the Index Register IX are 1000H, and the contents of\nmemory location 1003H are\n1 0 1 1 1 0 0 0\nat execution of SRA (IX+3H) the contents of memory location 1003H\nand the Carry flag are\n1 1 0 1 1 1 0 0 - 0"
        /// </summary>
        private void Instruction_144_SRA_IndexedAddress_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 5
            // MachineCycleType.MR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom(InternalDataBusConnection.W);
                InternalAddressBus_SampleFrom(RegisterUtils.GetFromAddressIndexBase((AddressIndexBase)instructionOrigin.OpCode.Param1));
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromRegisterWZPlusDisplacement();
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                Shift(OperationDirection.Right, ShiftType.Arithmetic);
                InternalDataBus_SendTo((Register)instructionOrigin.OpCode.Param2);
            }
            // MachineCycleType.MW, TStates = 3
        }

        /// <summary>
        /// OpCodeName = "SRL",
        /// Param1Type = AddressingMode.Register,
        /// Param1List =  "A","B","C","D","E","H","L"
        /// UserManualPage = "217",
        /// Description = "The operand m is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nThe contents of operand m are shifted right 1-bit position. The content of\nbit 0 is copied to the Carry flag, and bit 7 is reset. Bit 0 is the leastsignificant\nbit.",
        /// ConditionBitsAffected = "S is reset\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is data from bit 0 of source register",
        /// Example = "If the contents of register B are\n1 0 0 0 1 1 1 1\nat execution of SRL B the contents of register B and the Carry flag are\n0 1 0 0 0 1 1 1 - 1"
        /// </summary>
        private void Instruction_145_SRL_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4 
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                InternalDataBus_SampleFrom((Register)instructionOrigin.OpCode.Param1);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Shift(OperationDirection.Right, ShiftType.Logical);
                InternalDataBus_SendTo((Register)instructionOrigin.OpCode.Param1);
            }
        }

        /// <summary>
        /// OpCodeName = "SRL",
        /// Param1Type = AddressingMode.RegisterIndirect,
        /// Param1List =  "(HL)"
        /// UserManualPage = "217",
        /// Description = "The operand m is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nThe contents of operand m are shifted right 1-bit position. The content of\nbit 0 is copied to the Carry flag, and bit 7 is reset. Bit 0 is the leastsignificant\nbit.",
        /// ConditionBitsAffected = "S is reset\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is data from bit 0 of source register",
        /// Example = "If the contents of register B are\n1 0 0 0 1 1 1 1\nat execution of SRL B the contents of register B and the Carry flag are\n0 1 0 0 0 1 1 1 - 1"
        /// </summary>
        private void Instruction_146_SRL_Register16Address(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.MR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                Shift(OperationDirection.Right, ShiftType.Logical);
            }
            // MachineCycleType.MW, TStates = 3
        }

        /// <summary>
        /// OpCodeName = "SRL",
        /// Param1Type = AddressingMode.Indexed,
        /// Param1List =  "(IX+d)","(IY+d)"
        /// UserManualPage = "217",
        /// Description = "The operand m is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nThe contents of operand m are shifted right 1-bit position. The content of\nbit 0 is copied to the Carry flag, and bit 7 is reset. Bit 0 is the leastsignificant\nbit.",
        /// ConditionBitsAffected = "S is reset\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is data from bit 0 of source register",
        /// Example = "If the contents of register B are\n1 0 0 0 1 1 1 1\nat execution of SRL B the contents of register B and the Carry flag are\n0 1 0 0 0 1 1 1 - 1"
        /// </summary>
        private void Instruction_147_SRL_IndexedAddress(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 5
            // MachineCycleType.MR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom(InternalDataBusConnection.W);
                InternalAddressBus_SampleFrom(RegisterUtils.GetFromAddressIndexBase((AddressIndexBase)instructionOrigin.OpCode.Param1));
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromRegisterWZPlusDisplacement();
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                Shift(OperationDirection.Right, ShiftType.Logical);
            }
            // MachineCycleType.MW, TStates = 3
        }

        /// <summary>
        /// OpCodeName = "SRL",
        /// Param1Type = AddressingMode.Indexed,
        /// Param2Type = AddressingMode.Register,
        /// Param1List =  "(IX+d)","(IY+d)"
        /// Param2List =  "A","B","C","D","E","H","L"
        /// Description = "The operand m is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nThe contents of operand m are shifted right 1-bit position. The content of\nbit 0 is copied to the Carry flag, and bit 7 is reset. Bit 0 is the leastsignificant\nbit.\nThe undocumented DDCB instructions store the result (if any) of the operation in one of the\nseven all-purpose registers.",
        /// ConditionBitsAffected = "S is reset\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is data from bit 0 of source register",
        /// Example = "If the contents of register B are\n1 0 0 0 1 1 1 1\nat execution of SRL B the contents of register B and the Carry flag are\n0 1 0 0 0 1 1 1 - 1"
        /// </summary>
        private void Instruction_148_SRL_IndexedAddress_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 5
            // MachineCycleType.MR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom(InternalDataBusConnection.W);
                InternalAddressBus_SampleFrom(RegisterUtils.GetFromAddressIndexBase((AddressIndexBase)instructionOrigin.OpCode.Param1));
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromRegisterWZPlusDisplacement();
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                Shift(OperationDirection.Right, ShiftType.Logical);
                InternalDataBus_SendTo((Register)instructionOrigin.OpCode.Param2);
            }
            // MachineCycleType.MW, TStates = 3
        }
    }
}