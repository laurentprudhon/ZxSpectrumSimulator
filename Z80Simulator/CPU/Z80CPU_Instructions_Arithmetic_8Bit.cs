using Z80Simulator.Instructions;

namespace Z80Simulator.CPU
{
    /// <summary>
    /// ZiLOG Z80 CPU
    /// 
    /// Instructions - 8-Bit Arithmetic Group
    /// </summary>
    public partial class Z80CPU
    {       
        /// <summary>
        /// OpCodeName = "ADC",
        /// Param1Type = AddressingMode.Register,
        /// Param2Type = AddressingMode.Immediate,
        /// Equation = "A <- A + s + CY",
        /// Param1List = "A"
        /// Param2List = "n"
        /// UserManualPage = "146",
        /// Description = "This s operand is any of r, n, (HL), (IX+d), or (lY+d) as defined for the\nanalogous ADD instruction.\nThe s operand, along with the Carry Flag (C in the F register) is added to the\ncontents of the Accumulator, and the result is stored in the Accumulator.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if carry from bit 3, reset otherwise\nP/V is set if overflow, reset otherwise\nN is reset\nC is set if carry from bit 7: reset otherwise",
        /// Example = "If the Accumulator contents are 16H, the Carry Flag is set, the HL register\npair contains 6666H, and address 6666H contains 10H, at execution of\nADC A, (HL) the Accumulator contains 27H."
        /// </summary>
        private void Instruction_1_ADC_Register_Number8(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        {
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.OD, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                // Sample accumulator value into ALULeftBuffer
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Add(true, false, true);
                InternalDataBus_SendTo(Register.A);
            }
        }

        /// <summary>
        /// OpCodeName = "ADC",
        /// Param1Type = AddressingMode.Register,
        /// Param2Type = AddressingMode.Register,
        /// Equation = "A <- A + s + CY",
        /// Param1List = "A"
        /// Param2List = "A","B","C","D","E","H","L","IXh","IXl","IYh","IYl"
        /// UserManualPage = "146",
        /// Description = "This s operand is any of r, n, (HL), (IX+d), or (lY+d) as defined for the\nanalogous ADD instruction.\nThe s operand, along with the Carry Flag (C in the F register) is added to the\ncontents of the Accumulator, and the result is stored in the Accumulator.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if carry from bit 3, reset otherwise\nP/V is set if overflow, reset otherwise\nN is reset\nC is set if carry from bit 7: reset otherwise",
        /// Example = "If the Accumulator contents are 16H, the Carry Flag is set, the HL register\npair contains 6666H, and address 6666H contains 10H, at execution of\nADC A, (HL) the Accumulator contains 27H."
        /// </summary>
        private void Instruction_2_ADC_Register_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        {
            //  MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                // Sample accumulator value into ALULeftBuffer
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SampleFrom((Register)instructionOrigin.OpCode.Param2);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Add(true, false, true);
                InternalDataBus_SendTo(Register.A);
            }
        }

        /// <summary>
        /// OpCodeName = "ADC",
        /// Param1Type = AddressingMode.Register,
        /// Param2Type = AddressingMode.RegisterIndirect,
        /// Equation = "A <- A + s + CY",
        /// Param1List = "A"
        /// Param2List = "(HL)"
        /// UserManualPage = "146",
        /// Description = "This s operand is any of r, n, (HL), (IX+d), or (lY+d) as defined for the\nanalogous ADD instruction.\nThe s operand, along with the Carry Flag (C in the F register) is added to the\ncontents of the Accumulator, and the result is stored in the Accumulator.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if carry from bit 3, reset otherwise\nP/V is set if overflow, reset otherwise\nN is reset\nC is set if carry from bit 7: reset otherwise",
        /// Example = "If the Accumulator contents are 16H, the Carry Flag is set, the HL register\npair contains 6666H, and address 6666H contains 10H, at execution of\nADC A, (HL) the Accumulator contains 27H."
        /// </summary>
        private void Instruction_3_ADC_Register_Register16Address(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        {
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.MR, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                // Sample accumulator value into ALULeftBuffer
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Add(true, false, true);
                InternalDataBus_SendTo(Register.A);
            }
        }

        /// <summary>
        /// OpCodeName = "ADC",
        /// Param1Type = AddressingMode.Register,
        /// Param2Type = AddressingMode.Indexed,
        /// Equation = "A <- A + s + CY",
        /// Param1List = "A"
        /// Param2List = "(IX+d)","(IY+d)"
        /// UserManualPage = "146",
        /// Description = "This s operand is any of r, n, (HL), (IX+d), or (lY+d) as defined for the\nanalogous ADD instruction.\nThe s operand, along with the Carry Flag (C in the F register) is added to the\ncontents of the Accumulator, and the result is stored in the Accumulator.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if carry from bit 3, reset otherwise\nP/V is set if overflow, reset otherwise\nN is reset\nC is set if carry from bit 7: reset otherwise",
        /// Example = "If the Accumulator contents are 16H, the Carry Flag is set, the HL register\npair contains 6666H, and address 6666H contains 10H, at execution of\nADC A, (HL) the Accumulator contains 27H."
        /// </summary>
        private void Instruction_4_ADC_Register_IndexedAddress(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.OD, TStates = 3
            // MachineCycleType.CPU, TStates = 5
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 9)
            {
                InternalAddressBus_SampleFrom(RegisterUtils.GetFromAddressIndexBase((AddressIndexBase)instructionOrigin.OpCode.Param2));
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromRegisterWZPlusDisplacement();
            }
            // MachineCycleType.MR, TStates = 3
            if (machineCycleCountAfterInstruction == 4 && halfTStateIndex == 5)
            {
                // Sample accumulator value into ALULeftBuffer
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Add(true, false, true);
                InternalDataBus_SendTo(Register.A);
            }
        }
       
        /// <summary>
        /// OpCodeName = "ADD",
        /// Param1Type = AddressingMode.Register,
        /// Param2Type = AddressingMode.Immediate,
        /// Equation = "A <- A + n",
        /// Param1List = "A"
        /// Param2List = "n"
        /// UserManualPage = "142",
        /// Description = "The integer n is added to the contents of the Accumulator, and the results\nare stored in the Accumulator.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if carry from bit 3, reset otherwise\nP/V is set if overflow, reset otherwise\nN is reset\nC is set if carry from bit 7, reset otherwise",
        /// Example = "If the contents of the Accumulator are 23H, at execution of ADD A, 33H\nthe contents of the Accumulator are 56H."  
        /// </summary>
        private void Instruction_6_ADD_Register_Number8(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        {
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.OD, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                // Sample accumulator value into ALULeftBuffer
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Add(false, false, true);
                InternalDataBus_SendTo(Register.A);
            }
        }

        /// <summary>
        /// OpCodeName = "ADD",
        /// Param1Type = AddressingMode.Register,
        /// Param2Type = AddressingMode.Register,
        /// Equation = "A <- A + r",
        /// Param1List = "A"
        /// Param2List = "A","B","C","D","E","H","L","IXh","IXl","IYh","IYl"
        /// UserManualPage = "140",
        /// Description = "The contents of register r are added to the contents of the Accumulator, and\nthe result is stored in the Accumulator. The symbol r identifies the registers\nA, B, C, D, E, H, or L.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if carry from bit 3, reset otherwise\nP/V is set if overflow, reset otherwise\nN is reset\nC is set if carry from bit 7, reset otherwise",
        /// Example = "If the contents of the Accumulator are 44H, and the contents of register C\nare 11H, at execution of ADD A,C the contents of the Accumulator are\n55H."
        /// </summary>
        private void Instruction_7_ADD_Register_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                // Sample accumulator value into ALULeftBuffer
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SampleFrom((Register)instructionOrigin.OpCode.Param2);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Add(false, false, true);
                InternalDataBus_SendTo(Register.A);
            }
        }

        /// <summary>
        /// OpCodeName = "ADD",
        /// Param1Type = AddressingMode.Register,
        /// Param2Type = AddressingMode.RegisterIndirect,
        /// Equation = "A <- A + (HL)",
        /// Param1List = "A"
        /// Param2List = "(HL)"
        /// UserManualPage = "143",
        /// Description = "The byte at the memory address specified by the contents of the HL register\npair is added to the contents of the Accumulator, and the result is stored in\nthe Accumulator.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if carry from bit 3, reset otherwise\nP/V is set if overflow, reset otherwise\nN is reset\nC is set if carry from bit 7, reset otherwise",
        /// Example = "If the contents of the Accumulator are A0H, and the content of the register\npair HL is 2323H, and memory location 2323H contains byte 08H, at\nexecution of ADD A, (HL) the Accumulator contains A8H."
        /// </summary>
        private void Instruction_8_ADD_Register_Register16Address(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.MR, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                // Sample accumulator value into ALULeftBuffer
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Add(false, false, true);
                InternalDataBus_SendTo(Register.A);
            }
        }

        /// <summary>
        /// OpCodeName = "ADD",
        /// Param1Type = AddressingMode.Register,
        /// Param2Type = AddressingMode.Indexed,
        /// Equation = "A <- A + (IX+d)",
        /// Param1List = "A"
        /// Param2List = "(IX+d)","(IY+d)"
        /// UserManualPage = "144\n145",
        /// Description = "The contents of the Index Register (register pair IX) is added to a two�s\ncomplement displacement d to point to an address in memory. The contents\nof this address is then added to the contents of the Accumulator and the\nresult is stored in the Accumulator.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if carry from bit 3, reset otherwise\nP/V is set if overflow, reset otherwise\nN is reset\nC is set if carry from bit 7, reset otherwise",
        /// Example = "If the Accumulator contents are 11H, the Index Register IX contains\n1000H, and if the contents of memory location 1005H is 22H, at execution\nof ADD A, (IX + 5H) the contents of the Accumulator are 33H."
        /// </summary>
        private void Instruction_9_ADD_Register_IndexedAddress(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.OD, TStates = 3
            // MachineCycleType.CPU, TStates = 5
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 9)
            {
                InternalAddressBus_SampleFrom(RegisterUtils.GetFromAddressIndexBase((AddressIndexBase)instructionOrigin.OpCode.Param2));
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromRegisterWZPlusDisplacement();
            }
            // MachineCycleType.MR, TStates = 3
            if (machineCycleCountAfterInstruction == 4 && halfTStateIndex == 5)
            {
                // Sample accumulator value into ALULeftBuffer
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Add(false, false, true);
                InternalDataBus_SendTo(Register.A);
            }
        }
      
        /// <summary>
        /// OpCodeName = "AND",
        /// Param1Type = AddressingMode.Immediate,
        /// Equation = "A <- A & s",
        /// Param1List = "n"
        /// UserManualPage = "152",
        /// Description = "A logical AND operation is performed between the byte specified by the s\noperand and the byte contained in the Accumulator, the result is stored in\nthe Accumulator.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set\nP/V is reset if overflow, reset otherwise\nN is reset\nC is reset",
        /// Example = "If the B register contains 7BH (0111 1011), and the Accumulator contains\nC3H (1100 0011), at execution of AND B the Accumulator contains 43H\n(0100 0011)."
        /// </summary>
        private void Instruction_11_AND_Number8(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.OD, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                // Sample accumulator value into ALULeftBuffer
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                And();
                InternalDataBus_SendTo(Register.A);
            }
        }

        /// <summary>
        /// OpCodeName = "AND",
        /// Param1Type = AddressingMode.Register,
        /// Equation = "A <- A & s",
        /// Param1List = "A","B","C","D","E","H","L","IXh","IXl","IYh","IYl"
        /// UserManualPage = "152",
        /// Description = "A logical AND operation is performed between the byte specified by the s\noperand and the byte contained in the Accumulator, the result is stored in\nthe Accumulator.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set\nP/V is reset if overflow, reset otherwise\nN is reset\nC is reset",
        /// Example = "If the B register contains 7BH (0111 1011), and the Accumulator contains\nC3H (1100 0011), at execution of AND B the Accumulator contains 43H\n(0100 0011)."
        /// </summary>
        private void Instruction_12_AND_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                // Sample accumulator value into ALULeftBuffer
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SampleFrom((Register)instructionOrigin.OpCode.Param1);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                And();
                InternalDataBus_SendTo(Register.A);
            }
        }

        /// <summary>
        /// OpCodeName = "AND",
        /// Param1Type = AddressingMode.RegisterIndirect,
        /// Equation = "A <- A & s",
        /// Param1List = "(HL)"
        /// UserManualPage = "152",
        /// Description = "A logical AND operation is performed between the byte specified by the s\noperand and the byte contained in the Accumulator, the result is stored in\nthe Accumulator.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set\nP/V is reset if overflow, reset otherwise\nN is reset\nC is reset",
        /// Example = "If the B register contains 7BH (0111 1011), and the Accumulator contains\nC3H (1100 0011), at execution of AND B the Accumulator contains 43H\n(0100 0011)."
        /// </summary>
        private void Instruction_13_AND_Register16Address(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        {
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.MR, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                // Sample accumulator value into ALULeftBuffer
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                And();
                InternalDataBus_SendTo(Register.A);
            }
        }

        /// <summary>
        /// OpCodeName = "AND",
        /// Param1Type = AddressingMode.Indexed,
        /// Equation = "A <- A & s",
        /// Param1List = "(IX+d)","(IY+d)"
        /// UserManualPage = "152",
        /// Description = "A logical AND operation is performed between the byte specified by the s\noperand and the byte contained in the Accumulator, the result is stored in\nthe Accumulator.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set\nP/V is reset if overflow, reset otherwise\nN is reset\nC is reset",
        /// Example = "If the B register contains 7BH (0111 1011), and the Accumulator contains\nC3H (1100 0011), at execution of AND B the Accumulator contains 43H\n(0100 0011)."
        /// </summary>
        private void Instruction_14_AND_IndexedAddress(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.OD, TStates = 3
            // MachineCycleType.CPU, TStates = 5
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 9)
            {
                InternalAddressBus_SampleFrom(RegisterUtils.GetFromAddressIndexBase((AddressIndexBase)instructionOrigin.OpCode.Param1));
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromRegisterWZPlusDisplacement();
            }
            // MachineCycleType.MR, TStates = 3
            if (machineCycleCountAfterInstruction == 4 && halfTStateIndex == 5)
            {
                // Sample accumulator value into ALULeftBuffer
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                And();
                InternalDataBus_SendTo(Register.A);
            }
        }

        /// <summary>
        /// OpCodeName = "CP",
        /// Param1Type = AddressingMode.Immediate,
        /// Equation = "A - s",
        /// Param1List = "n"
        /// UserManualPage = "158",
        /// Description = "The contents of the s operand are compared with the contents of the\nAccumulator. If there is a true compare, the Z flag is set. The execution of\nthis instruction does not affect the contents of the Accumulator.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is set if overflow, reset otherwise\nN is set\nC is set if borrow, reset otherwise",
        /// Example = "If the Accumulator contains 63H, the HL register pair contains 6000H, and\nmemory location 6000H contains 60H, the instruction CP (HL) results in\nthe PN flag in the F register resetting."
        /// </summary>
        private void Instruction_21_CP_Number8(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        {
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.OD, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                // Sample accumulator value into ALULeftBuffer
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Compare();
            }
        }

        /// <summary>
        /// OpCodeName = "CP",
        /// Param1Type = AddressingMode.Register,
        /// Equation = "A - s",
        /// Param1List = "A","B","C","D","E","H","L","IXh","IXl","IYh","IYl"
        /// UserManualPage = "158",
        /// Description = "The contents of the s operand are compared with the contents of the\nAccumulator. If there is a true compare, the Z flag is set. The execution of\nthis instruction does not affect the contents of the Accumulator.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is set if overflow, reset otherwise\nN is set\nC is set if borrow, reset otherwise",
        /// Example = "If the Accumulator contains 63H, the HL register pair contains 6000H, and\nmemory location 6000H contains 60H, the instruction CP (HL) results in\nthe PN flag in the F register resetting."
        /// </summary>
        private void Instruction_22_CP_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            //  MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                // Sample accumulator value into ALULeftBuffer
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SampleFrom((Register)instructionOrigin.OpCode.Param1);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Compare();
            }
        }

        /// <summary>
        /// OpCodeName = "CP",
        /// Param1Type = AddressingMode.RegisterIndirect,
        /// Equation = "A - s",
        /// Param1List = "(HL)"
        /// UserManualPage = "158",
        /// Description = "The contents of the s operand are compared with the contents of the\nAccumulator. If there is a true compare, the Z flag is set. The execution of\nthis instruction does not affect the contents of the Accumulator.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is set if overflow, reset otherwise\nN is set\nC is set if borrow, reset otherwise",
        /// Example = "If the Accumulator contains 63H, the HL register pair contains 6000H, and\nmemory location 6000H contains 60H, the instruction CP (HL) results in\nthe PN flag in the F register resetting."
        /// </summary>
        private void Instruction_23_CP_Register16Address(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.MR, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                // Sample accumulator value into ALULeftBuffer
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Compare();
            }
        }

        /// <summary>
        /// OpCodeName = "CP",
        /// Param1Type = AddressingMode.Indexed,
        /// Equation = "A - s",
        /// Param1List = "(IX+d)","(IY+d)"
        /// UserManualPage = "158",
        /// Description = "The contents of the s operand are compared with the contents of the\nAccumulator. If there is a true compare, the Z flag is set. The execution of\nthis instruction does not affect the contents of the Accumulator.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is set if overflow, reset otherwise\nN is set\nC is set if borrow, reset otherwise",
        /// Example = "If the Accumulator contains 63H, the HL register pair contains 6000H, and\nmemory location 6000H contains 60H, the instruction CP (HL) results in\nthe PN flag in the F register resetting."
        /// </summary>
        private void Instruction_24_CP_IndexedAddress(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.OD, TStates = 3
            // MachineCycleType.CPU, TStates = 5
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 9)
            {
                InternalAddressBus_SampleFrom(RegisterUtils.GetFromAddressIndexBase((AddressIndexBase)instructionOrigin.OpCode.Param1));
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromRegisterWZPlusDisplacement();
            }
            // MachineCycleType.MR, TStates = 3
            if (machineCycleCountAfterInstruction == 4 && halfTStateIndex == 5)
            {
                // Sample accumulator value into ALULeftBuffer
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Compare();
            }
        }
             
        /// <summary>
        /// OpCodeName = "DEC",
        /// Param1Type = AddressingMode.Register,
        /// Equation = "m <- m- 1",
        /// Param1List = "A","B","C","D","E","H","L","IXh","IXl","IYh","IYl"
        /// UserManualPage = "164",
        /// Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous INC instructions.\nThe byte specified by the m operand is decremented.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is set if m was 80H before operation, reset otherwise\nN is set\nC is not affected",
        /// Example = "If the D register contains byte 2AH, at execution of DEC D register D\ncontains 29H."
        /// </summary>
        private void Instruction_31_DEC_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                InternalDataBus_SampleFrom((Register)instructionOrigin.OpCode.Param1);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Decrement();
                InternalDataBus_SendTo((Register)instructionOrigin.OpCode.Param1);
            }
        }

        /// <summary>
        /// OpCodeName = "DEC",
        /// Param1Type = AddressingMode.RegisterIndirect,
        /// Equation = "m <- m- 1",
        /// Param1List = "(HL)",
        /// UserManualPage = "164",
        /// Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous INC instructions.\nThe byte specified by the m operand is decremented.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is set if m was 80H before operation, reset otherwise\nN is set\nC is not affected",
        /// Example = "If the D register contains byte 2AH, at execution of DEC D register D\ncontains 29H."
        /// </summary>
        private void Instruction_32_DEC_Register16Address(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.MR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Decrement();
            }
            // MachineCycleType.MW, TStates = 3
        }

        /// <summary>
        /// OpCodeName = "DEC",
        /// Param1Type = AddressingMode.Indexed,
        /// Equation = "m <- m- 1",
        /// Param1List = "(IX+d)","(IY+d)"
        /// UserManualPage = "164",
        /// Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous INC instructions.\nThe byte specified by the m operand is decremented.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is set if m was 80H before operation, reset otherwise\nN is set\nC is not affected",
        /// Example = "If the D register contains byte 2AH, at execution of DEC D register D\ncontains 29H." 
        /// </summary>
        private void Instruction_33_DEC_IndexedAddress(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.OD, TStates = 3
            // MachineCycleType.CPU, TStates = 5
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 9)
            {
                InternalAddressBus_SampleFrom(RegisterUtils.GetFromAddressIndexBase((AddressIndexBase)instructionOrigin.OpCode.Param1));
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromRegisterWZPlusDisplacement();
            }
            // MachineCycleType.MR, TStates = 4
            if (machineCycleCountAfterInstruction == 4 && halfTStateIndex == 7)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Decrement();
            }
            // MachineCycleType.MW, TStates = 3
        }
            
        /// <summary>
        /// OpCodeName = "INC",
        /// Param1Type = AddressingMode.Register,
        /// Equation = "r <- r + 1",
        /// Param1List = "A","B","C","D","E","H","L","IXh","IXl","IYh","IYl"
        /// UserManualPage = "160",
        /// Description = "Register r is incremented and register r identifies any of the registers A, B,\nC, D, E, H, or L.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if carry from bit 3, reset otherwise\nP/V is set if r was 7FH before operation, reset otherwise\nN is reset\nC is not affected"
        /// </summary>
        private void Instruction_46_INC_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // Type = MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                InternalDataBus_SampleFrom((Register)instructionOrigin.OpCode.Param1);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Increment();
                InternalDataBus_SendTo((Register)instructionOrigin.OpCode.Param1);
            }
        }

        /// <summary>
        /// OpCodeName = "INC",
        /// Param1Type = AddressingMode.RegisterIndirect,
        /// Equation = "(HL) <- (HL) + 1",
        /// Param1List = "(HL)"
        /// UserManualPage = "161",
        /// Description = "The byte contained in the address specified by the contents of the HL\nregister pair is incremented.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if carry from bit 3, reset otherwise\nP/V is set if (HL) was 7FH before operation, reset otherwise\nN is reset\nC is not affected",
        /// Example = "If the contents of the HL register pair are 3434H, and the contents of\naddress 3434H are 82H, at execution of INC (HL) memory location\n3434H contains 83H."
        /// </summary>
        private void Instruction_47_INC_Register16Address(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4 
            // MachineCycleType.MR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Increment();
            }
            // MachineCycleType.MW, TStates = 3
        }

        /// <summary>
        /// OpCodeName = "INC",
        /// Param1Type = AddressingMode.Indexed,
        /// Equation = "(IX+d) <- (IX+d) + 1",
        /// Param1List = "(IX+d)","(IY+d)"
        /// UserManualPage = "162\n163",
        /// Description = "The contents of the Index Register IX (register pair IX) are added to a two�s\ncomplement displacement integer d to point to an address in memory. The\ncontents of this address are then incremented.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if carry from bit 3, reset otherwise\nP/V is set if (IX+d) was 7FH before operation, reset otherwise\nN is reset\nC is not affected",
        /// Example = "If the contents of the Index Register pair IX are 2020H, and the memory\nlocation 2030H contains byte 34H, at execution of INC (IX+10H) the\ncontents of memory location 2030H is 35H."
        /// </summary>
        private void Instruction_48_INC_IndexedAddress(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.OD, TStates = 3
            // MachineCycleType.CPU, TStates = 5
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 9)
            {
                InternalAddressBus_SampleFrom(RegisterUtils.GetFromAddressIndexBase((AddressIndexBase)instructionOrigin.OpCode.Param1));
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromRegisterWZPlusDisplacement();
            }
            // MachineCycleType.MR, TStates = 4
            if (machineCycleCountAfterInstruction == 4 && halfTStateIndex == 7)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Increment();
            }
            // MachineCycleType.MW, TStates = 3
        }
       
        /// <summary>
        /// OpCodeName = "OR",
        /// Param1Type = AddressingMode.Immediate,
        /// Equation = "A <- A | s",
        /// Param1List = "n"
        /// UserManualPage = "154",
        /// Description = "A logical OR operation is performed between the byte specified by the s\noperand and the byte contained in the Accumulator, the result is stored in\nthe Accumulator.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if overflow, reset otherwise\nN is reset\nC is reset",
        /// Example = "If the H register contains 48H (0100 0100), and the Accumulator contains\n12H (0001 0010), at execution of OR H the Accumulator contains 5AH\n(0101 1010)."
        /// </summary>
        private void Instruction_79_OR_Number8(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.OD, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                // Sample accumulator value into ALULeftBuffer
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Or();
                InternalDataBus_SendTo(Register.A);
            }
        }

        /// <summary>
        /// OpCodeName = "OR",
        /// Param1Type = AddressingMode.Register,
        /// Equation = "A <- A | s",
        /// Param1List = "A","B","C","D","E","H","L","IXh","IXl","IYh","IYl"
        /// UserManualPage = "154",
        /// Description = "A logical OR operation is performed between the byte specified by the s\noperand and the byte contained in the Accumulator, the result is stored in\nthe Accumulator.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if overflow, reset otherwise\nN is reset\nC is reset",
        /// Example = "If the H register contains 48H (0100 0100), and the Accumulator contains\n12H (0001 0010), at execution of OR H the Accumulator contains 5AH\n(0101 1010)."
        /// </summary>
        private void Instruction_80_OR_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // Type = MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                // Sample accumulator value into ALULeftBuffer
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SampleFrom((Register)instructionOrigin.OpCode.Param1);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Or();
                InternalDataBus_SendTo(Register.A);
            }
        }

        /// <summary>
        /// OpCodeName = "OR",
        /// Param1Type = AddressingMode.RegisterIndirect,
        /// Equation = "A <- A | s",
        /// Param1List = "(HL)"
        /// UserManualPage = "154",
        /// Description = "A logical OR operation is performed between the byte specified by the s\noperand and the byte contained in the Accumulator, the result is stored in\nthe Accumulator.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if overflow, reset otherwise\nN is reset\nC is reset",
        /// Example = "If the H register contains 48H (0100 0100), and the Accumulator contains\n12H (0001 0010), at execution of OR H the Accumulator contains 5AH\n(0101 1010)."
        /// </summary>
        private void Instruction_81_OR_Register16Address(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.MR, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                // Sample accumulator value into ALULeftBuffer
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Or();
                InternalDataBus_SendTo(Register.A);
            }
        }

        /// <summary>
        /// OpCodeName = "OR",
        /// Param1Type = AddressingMode.Indexed,
        /// Equation = "A <- A | s",
        /// Param1List = "(IX+d)","(IY+d)"
        /// UserManualPage = "154",
        /// Description = "A logical OR operation is performed between the byte specified by the s\noperand and the byte contained in the Accumulator, the result is stored in\nthe Accumulator.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if overflow, reset otherwise\nN is reset\nC is reset",
        /// Example = "If the H register contains 48H (0100 0100), and the Accumulator contains\n12H (0001 0010), at execution of OR H the Accumulator contains 5AH\n(0101 1010)."
        /// </summary>
        private void Instruction_82_OR_IndexedAddress(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.OD, TStates = 3
            // MachineCycleType.CPU, TStates = 5
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 9)
            {
                InternalAddressBus_SampleFrom(RegisterUtils.GetFromAddressIndexBase((AddressIndexBase)instructionOrigin.OpCode.Param1));
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromRegisterWZPlusDisplacement();
            }
            // MachineCycleType.MR, TStates = 3
            if (machineCycleCountAfterInstruction == 4 && halfTStateIndex == 5)
            {
                // Sample accumulator value into ALULeftBuffer
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Or();
                InternalDataBus_SendTo(Register.A);
            }
        }

        /// <summary>
        /// OpCodeName = "SBC",
        /// Param1Type = AddressingMode.Register,
        /// Param2Type = AddressingMode.Immediate,
        /// Equation = "A <- A - s - CY",
        /// Param1List = "A"
        /// Param2List = "n"
        /// UserManualPage = "150",
        /// Description = "The s operand, along with the Carry flag (C in the F register) is subtracted\nfrom the contents of the Accumulator, and the result is stored in the\nAccumulator.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is reset if overflow, reset otherwise\nN is set\nC is set if borrow, reset otherwise",
        /// Example = "If the Accumulator contains 16H, the carry flag is set, the HL register pair\ncontains 3433H, and address 3433H contains 05H, at execution of\nSBC A, (HL) the Accumulator contains 10H."
        /// </summary>
        private void Instruction_123_SBC_Register_Number8(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        {
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.OD, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                // Sample accumulator value into ALULeftBuffer
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Sub(true, false);
                InternalDataBus_SendTo(Register.A);
            }
        }

        /// <summary>
        /// OpCodeName = "SBC",
        /// Param1Type = AddressingMode.Register,
        /// Param2Type = AddressingMode.Register,
        /// Equation = "A <- A - s - CY",
        /// Param1List = "A"
        /// Param2List = "A","B","C","D","E","H","L","IXh","IXl","IYh","IYl"
        /// UserManualPage = "150",
        /// Description = "The s operand, along with the Carry flag (C in the F register) is subtracted\nfrom the contents of the Accumulator, and the result is stored in the\nAccumulator.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is reset if overflow, reset otherwise\nN is set\nC is set if borrow, reset otherwise",
        /// Example = "If the Accumulator contains 16H, the carry flag is set, the HL register pair\ncontains 3433H, and address 3433H contains 05H, at execution of\nSBC A, (HL) the Accumulator contains 10H."
        /// </summary>
        private void Instruction_124_SBC_Register_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        {
            // MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                // Sample accumulator value into ALULeftBuffer
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SampleFrom((Register)instructionOrigin.OpCode.Param2);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Sub(true, false);
                InternalDataBus_SendTo(Register.A);
            }
        }

        /// <summary>
        /// OpCodeName = "SBC",
        /// Param1Type = AddressingMode.Register,
        /// Param2Type = AddressingMode.RegisterIndirect,
        /// Equation = "A <- A - s - CY",
        /// Param1List = "A"
        /// Param2List = "(HL)"
        /// UserManualPage = "150",
        /// Description = "The s operand, along with the Carry flag (C in the F register) is subtracted\nfrom the contents of the Accumulator, and the result is stored in the\nAccumulator.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is reset if overflow, reset otherwise\nN is set\nC is set if borrow, reset otherwise",
        /// Example = "If the Accumulator contains 16H, the carry flag is set, the HL register pair\ncontains 3433H, and address 3433H contains 05H, at execution of\nSBC A, (HL) the Accumulator contains 10H."
        /// </summary>
        private void Instruction_125_SBC_Register_Register16Address(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.MR, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                // Sample accumulator value into ALULeftBuffer
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Sub(true, false);
                InternalDataBus_SendTo(Register.A);
            }
        }

        /// <summary>
        /// OpCodeName = "SBC",
        /// Param1Type = AddressingMode.Register,
        /// Param2Type = AddressingMode.Indexed,
        /// Equation = "A <- A - s - CY",
        /// Param1List = "A"
        /// Param2List = "(IX+d)","(IY+d)"
        /// UserManualPage = "150",
        /// Description = "The s operand, along with the Carry flag (C in the F register) is subtracted\nfrom the contents of the Accumulator, and the result is stored in the\nAccumulator.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is reset if overflow, reset otherwise\nN is set\nC is set if borrow, reset otherwise",
        /// Example = "If the Accumulator contains 16H, the carry flag is set, the HL register pair\ncontains 3433H, and address 3433H contains 05H, at execution of\nSBC A, (HL) the Accumulator contains 10H."
        /// </summary>
        private void Instruction_126_SBC_Register_IndexedAddress(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.OD, TStates = 3
            // MachineCycleType.CPU, TStates = 5
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 9)
            {
                InternalAddressBus_SampleFrom(RegisterUtils.GetFromAddressIndexBase((AddressIndexBase)instructionOrigin.OpCode.Param2));
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromRegisterWZPlusDisplacement();
            }
            // MachineCycleType.MR, TStates = 3
            if (machineCycleCountAfterInstruction == 4 && halfTStateIndex == 5)
            {
                // Sample accumulator value into ALULeftBuffer
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Sub(true, false);
                InternalDataBus_SendTo(Register.A);
            }
        }

        /// <summary>
        /// OpCodeName = "SUB",
        /// Param1Type = AddressingMode.Immediate,
        /// Equation = "A <- A - s",
        /// Param1List = "n"
        /// UserManualPage = "148",
        /// Description = "The s operand is subtracted from the contents of the Accumulator, and the\nresult is stored in the Accumulator.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is set if overflow, reset otherwise\nN is set\nC is set if borrow, reset otherwise",
        /// Example = "If the Accumulator contents are 29H, and register D contains 11H, at\nexecution of SUB D the Accumulator contains 18H."
        /// </summary>
        private void Instruction_149_SUB_Number8(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        {
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.OD, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                // Sample accumulator value into ALULeftBuffer
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Sub(false, false);
                InternalDataBus_SendTo(Register.A);
            }
        }

        /// <summary>
        /// OpCodeName = "SUB",
        /// Param1Type = AddressingMode.Register,
        /// Equation = "A <- A - s",
        /// Param1List = "A","B","C","D","E","H","L","IXh","IXl","IYh","IYl"
        /// UserManualPage = "148",
        /// Description = "The s operand is subtracted from the contents of the Accumulator, and the\nresult is stored in the Accumulator.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is set if overflow, reset otherwise\nN is set\nC is set if borrow, reset otherwise",
        /// Example = "If the Accumulator contents are 29H, and register D contains 11H, at\nexecution of SUB D the Accumulator contains 18H."
        /// </summary>
        private void Instruction_150_SUB_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        {
            // MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                // Sample accumulator value into ALULeftBuffer
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SampleFrom((Register)instructionOrigin.OpCode.Param1);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Sub(false, false);
                InternalDataBus_SendTo(Register.A);
            }
        }

        /// <summary>
        /// OpCodeName = "SUB",
        /// Param1Type = AddressingMode.RegisterIndirect,
        /// Equation = "A <- A - s",
        /// Param1List = "(HL)"
        /// UserManualPage = "148",
        /// Description = "The s operand is subtracted from the contents of the Accumulator, and the\nresult is stored in the Accumulator.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is set if overflow, reset otherwise\nN is set\nC is set if borrow, reset otherwise",
        /// Example = "If the Accumulator contents are 29H, and register D contains 11H, at\nexecution of SUB D the Accumulator contains 18H."
        /// </summary>
        private void Instruction_151_SUB_Register16Address(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        {
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.MR, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                // Sample accumulator value into ALULeftBuffer
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Sub(false, false);
                InternalDataBus_SendTo(Register.A);
            }
        }

        /// <summary>
        /// OpCodeName = "SUB",
        /// Param1Type = AddressingMode.Indexed,
        /// Equation = "A <- A - s",
        /// Param1List = "(IX+d)","(IY+d)"
        /// UserManualPage = "148",
        /// Description = "The s operand is subtracted from the contents of the Accumulator, and the\nresult is stored in the Accumulator.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is set if overflow, reset otherwise\nN is set\nC is set if borrow, reset otherwise",
        /// Example = "If the Accumulator contents are 29H, and register D contains 11H, at\nexecution of SUB D the Accumulator contains 18H."
        /// </summary>
        private void Instruction_152_SUB_IndexedAddress(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        {
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.OD, TStates = 3
            // MachineCycleType.CPU, TStates = 5
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 9)
            {
                InternalAddressBus_SampleFrom(RegisterUtils.GetFromAddressIndexBase((AddressIndexBase)instructionOrigin.OpCode.Param1));
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromRegisterWZPlusDisplacement();
            }
            // MachineCycleType.MR, TStates = 3
            if (machineCycleCountAfterInstruction == 4 && halfTStateIndex == 5)
            {
                // Sample accumulator value into ALULeftBuffer
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Sub(false, false);
                InternalDataBus_SendTo(Register.A);
            }
        }

        /// <summary>
        /// OpCodeName = "XOR",
        /// Param1Type = AddressingMode.Immediate,
        /// Equation = "A <- A ^ s",
        /// Param1List = "n"
        /// UserManualPage = "156",
        /// Description = "The logical exclusive-OR operation is performed between the byte\nspecified by the s operand and the byte contained in the Accumulator, the\nresult is stored in the Accumulator.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise\nN is reset\nC is reset"
        /// </summary>
        private void Instruction_153_XOR_Number8(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.OD, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                // Sample accumulator value into ALULeftBuffer
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Xor();
                InternalDataBus_SendTo(Register.A);
            }
        }

        /// <summary>
        /// OpCodeName = "XOR",
        /// Param1Type = AddressingMode.Register,
        /// Equation = "A <- A ^ s",
        /// Param1List = "A","B","C","D","E","H","L","IXh","IXl","IYh","IYl"
        /// UserManualPage = "156",
        /// Description = "The logical exclusive-OR operation is performed between the byte\nspecified by the s operand and the byte contained in the Accumulator, the\nresult is stored in the Accumulator.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise\nN is reset\nC is reset"
        /// </summary>
        private void Instruction_154_XOR_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // Type = MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                // Sample accumulator value into ALULeftBuffer
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SampleFrom((Register)instructionOrigin.OpCode.Param1);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Xor();
                InternalDataBus_SendTo(Register.A);
            }
        }

        /// <summary>
        /// OpCodeName = "XOR",
        /// Param1Type = AddressingMode.RegisterIndirect,
        /// Equation = "A <- A ^ s",
        /// Param1List = "(HL)"
        /// UserManualPage = "156",
        /// Description = "The logical exclusive-OR operation is performed between the byte\nspecified by the s operand and the byte contained in the Accumulator, the\nresult is stored in the Accumulator.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise\nN is reset\nC is reset"
        /// </summary>
        private void Instruction_155_XOR_Register16Address(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.MR, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                // Sample accumulator value into ALULeftBuffer
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Xor();
                InternalDataBus_SendTo(Register.A);
            }
        }

        /// <summary>
        /// OpCodeName = "XOR",
        /// Param1Type = AddressingMode.Indexed,
        /// Equation = "A <- A ^ s",
        /// Param1List = "(IX+d)","(IY+d)"
        /// UserManualPage = "156",
        /// Description = "The logical exclusive-OR operation is performed between the byte\nspecified by the s operand and the byte contained in the Accumulator, the\nresult is stored in the Accumulator.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise\nN is reset\nC is reset"
        /// </summary>
        private void Instruction_156_XOR_IndexedAddress(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.OD, TStates = 3
            // MachineCycleType.CPU, TStates = 5
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 9)
            {
                InternalAddressBus_SampleFrom(RegisterUtils.GetFromAddressIndexBase((AddressIndexBase)instructionOrigin.OpCode.Param1));
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromRegisterWZPlusDisplacement();
            }
            // MachineCycleType.MR, TStates = 3
            if (machineCycleCountAfterInstruction == 4 && halfTStateIndex == 5)
            {
                // Sample accumulator value into ALULeftBuffer
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Xor();
                InternalDataBus_SendTo(Register.A);
            }
        }
    }
}