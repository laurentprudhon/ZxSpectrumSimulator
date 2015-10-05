using Z80Simulator.Instructions;

namespace Z80Simulator.CPU
{
    /// <summary>
    /// ZiLOG Z80 CPU
    /// 
    /// Instructions - Bit Set, Reset, and Test Group
    /// </summary>
    public partial class Z80CPU
    {
        /// <summary>
        /// OpCodeName = "BIT",
        /// Param1Type = AddressingMode.Bit,
        /// Param2Type = AddressingMode.Register,
        /// Equation = "Z <- rb",
        /// Param1List = "0","1","2","3","4","5","6","7"
        /// Param2List = "A","B","C","D","E","H","L"
        /// UserManualPage = "224",
        /// Description = "This instruction tests bit b in register r and sets the Z flag accordingly.",
        /// ConditionBitsAffected = "S is unknown\nZ is set if specified bit is 0, reset otherwise\nH is set\nP/V is unknown\nN is reset\nC is not affected",
        /// Example = "If bit 2 in register B contains 0, at execution of BIT 2, B the Z flag in the\nF register contains 1, and bit 2 in register B remains 0. Bit 0 in register B is\nthe least-significant bit."
        /// </summary>
        private void Instruction_15_BIT_Bit_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        {
            // MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                InternalDataBus_SampleFrom((Register)instructionOrigin.OpCode.Param2);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                BitTest(BitUtils.ToByte((Bit)instructionOrigin.OpCode.Param1), AddressingMode.Register);
            }
        }

        /// <summary>
        ///  OpCodeName = "BIT",
        ///  Param1Type = AddressingMode.Bit,
        ///  Param2Type = AddressingMode.RegisterIndirect,
        ///  Equation = "Z <- (HL)b",
        ///  Param1List = "0","1","2","3","4","5","6","7"
        ///  Param2List = "(HL)"
        ///  UserManualPage = "226",
        ///  Description = "This instruction tests bit b in the memory location specified by the contents\nof the HL register pair and sets the Z flag accordingly.",
        ///  ConditionBitsAffected = "S is unknown\nZ is set if specified Bit is 0, reset otherwise\nH is set\nP/V is unknown\nH is reset\nC is not affected",
        ///  Example = "If the HL register pair contains 4444H, and bit 4 in the memory location\n444H contains 1, at execution of BIT 4, (HL) the Z flag in the F register\ncontains 0, and bit 4 in memory location 4444H still contains 1. Bit 0 in\nmemory location 4444H is the least-significant bit."
        /// </summary>
        private void Instruction_16_BIT_Bit_Register16Address(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
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
                BitTest(BitUtils.ToByte((Bit)instructionOrigin.OpCode.Param1), AddressingMode.RegisterIndirect);
            }
        }

        /// <summary>
        /// OpCodeName = "BIT",
        /// Param1Type = AddressingMode.Bit,
        /// Param2Type = AddressingMode.Indexed,
        /// Equation = "Z <- (IX+d)b",
        /// Param1List = "0","1","2","3","4","5","6","7"
        /// Param2List = "(IX+d)","(IY+d)"
        /// UserManualPage = "228\n230",
        /// Description = "This instruction tests bit b in the memory location specified by the contents\nof register pair IX combined with the two's complement displacement d\nand sets the Z flag accordingly.",
        /// ConditionBitsAffected = "S is unknown\nZ is set if specified Bit is 0, reset otherwise\nH is set\nP/V is unknown\nN is reset\nC is not affected",
        /// Example = "If the contents of Index Register IX are 2000H, and bit 6 in memory\nlocation 2004H contains 1, at execution of BIT 6, (IX+4H) the Z flag in\nthe F register contains 0, and bit 6 in memory location 2004H still contains\n1. Bit 0 in memory location 2004H is the least-significant bit."
        /// </summary>
        private void Instruction_17_BIT_Bit_IndexedAddress(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        {
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.MR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom(InternalDataBusConnection.W);
                InternalAddressBus_SampleFrom(RegisterUtils.GetFromAddressIndexBase((AddressIndexBase)instructionOrigin.OpCode.Param2));
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromRegisterWZPlusDisplacement();
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                BitTest(BitUtils.ToByte((Bit)instructionOrigin.OpCode.Param1), AddressingMode.Indexed);
            }
        }

        /// <summary>
        ///  OpCodeName = "RES",
        ///  Param1Type = AddressingMode.Bit,
        ///  Param2Type = AddressingMode.Register,
        ///  Equation = "sb <- 0",
        ///  Param1List = "0","1","2","3","4","5","6","7"
        ///  Param2List = "A","B","C","D","E","H","L"
        ///  UserManualPage = "236",
        ///  Description = "Operand b is any bit (7 through 0) of the contents of the m operand, (any of\nr, (HL), (IX+d), or (lY+d)) as defined for the analogous SET instructions.\nBit b in operand m is reset.",
        ///  ConditionBitsAffected = "None",
        ///  Example = "At execution of RES 6, D, bit 6 in register 0 resets. Bit 0 in register D is the\nleast-significant bit."
        /// </summary>
        private void Instruction_92_RES_Bit_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        {
            // Type = MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                InternalDataBus_SampleFrom((Register)instructionOrigin.OpCode.Param2);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                BitReset(BitUtils.ToByte((Bit)instructionOrigin.OpCode.Param1));
                InternalDataBus_SendTo((Register)instructionOrigin.OpCode.Param2);
            }
        }

        /// <summary>
        /// OpCodeName = "RES",
        /// Param1Type = AddressingMode.Bit,
        /// Param2Type = AddressingMode.RegisterIndirect,
        /// Equation = "sb <- 0",
        /// Param1List = "0","1","2","3","4","5","6","7"
        /// Param2List = "(HL)"
        /// UserManualPage = "236",
        /// Description = "Operand b is any bit (7 through 0) of the contents of the m operand, (any of\nr, (HL), (IX+d), or (lY+d)) as defined for the analogous SET instructions.\nBit b in operand m is reset.",
        /// ConditionBitsAffected = "None",
        /// Example = "At execution of RES 6, D, bit 6 in register 0 resets. Bit 0 in register D is the\nleast-significant bit."
        /// </summary>
        private void Instruction_93_RES_Bit_Register16Address(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
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
                BitReset(BitUtils.ToByte((Bit)instructionOrigin.OpCode.Param1));
            }
            // MachineCycleType.MW, TStates = 3 
        }

        /// <summary>
        /// OpCodeName = "RES",
        /// Param1Type = AddressingMode.Bit,
        /// Param2Type = AddressingMode.Indexed,
        /// Equation = "sb <- 0",
        /// Param1List = "0","1","2","3","4","5","6","7" 
        /// Param2List = "(IX+d)","(IY+d)"
        /// UserManualPage = "236",
        /// Description = "Operand b is any bit (7 through 0) of the contents of the m operand, (any of\nr, (HL), (IX+d), or (lY+d)) as defined for the analogous SET instructions.\nBit b in operand m is reset.",
        /// ConditionBitsAffected = "None",
        /// Example = "At execution of RES 6, D, bit 6 in register 0 resets. Bit 0 in register D is the\nleast-significant bit."
        /// </summary>
        private void Instruction_94_RES_Bit_IndexedAddress(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        {
            // MachineCycleType.OCF, TStates = 5
            // MachineCycleType.MR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom(InternalDataBusConnection.W);
                InternalAddressBus_SampleFrom(RegisterUtils.GetFromAddressIndexBase((AddressIndexBase)instructionOrigin.OpCode.Param2));
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromRegisterWZPlusDisplacement();
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                BitReset(BitUtils.ToByte((Bit)instructionOrigin.OpCode.Param1));
            }
            // MachineCycleType.MW, TStates = 3
        }

        /// <summary>
        /// OpCodeName = "RES",
        /// Param1Type = AddressingMode.Bit,
        /// Param2Type = AddressingMode.Indexed,
        /// Param3Type = AddressingMode.Register,
        /// Equation = "sb <- 0\nr <- value",
        /// Param1List = "0","1","2","3","4","5","6","7"
        /// Param2List = "(IX+d)","(IY+d)"
        /// Param3List = "A","B","C","D","E","H","L"
        /// Description = "Operand b is any bit (7 through 0) of the contents of the m operand, (any of\nr, (HL), (IX+d), or (lY+d)) as defined for the analogous SET instructions.\nBit b in operand m is reset.\nThe undocumented DDCB instructions store the result (if any) of the operation in one of the\nseven all-purpose registers.",
        /// ConditionBitsAffected = "None",
        /// Example = "At execution of RES 6, D, bit 6 in register 0 resets. Bit 0 in register D is the\nleast-significant bit."
        /// </summary>
        private void Instruction_95_RES_Bit_IndexedAddress_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        {
            //  MachineCycleType.OCF, TStates = 5
            //  MachineCycleType.MR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom(InternalDataBusConnection.W);
                InternalAddressBus_SampleFrom(RegisterUtils.GetFromAddressIndexBase((AddressIndexBase)instructionOrigin.OpCode.Param2));
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromRegisterWZPlusDisplacement();
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                BitReset(BitUtils.ToByte((Bit)instructionOrigin.OpCode.Param1));
                InternalDataBus_SendTo((Register)instructionOrigin.OpCode.Param3);
            }
            //  MachineCycleType.MW, TStates = 3
        }

        /// <summary>
        /// OpCodeName = "SET",
        /// Param1Type = AddressingMode.Bit,
        /// Param2Type = AddressingMode.Register,
        /// Equation = "rb <- 1",
        /// Param1List = "0","1","2","3","4","5","6","7"
        /// Param2List = "A","B","C","D","E","H","L"
        /// UserManualPage = "232",
        /// Description = "Bit b in register r (any of registers B, C, D, E, H, L, or A) is set.",
        /// ConditionBitsAffected = "None",
        /// Example = "At execution of SET 4, A bit 4 in register A sets. Bit 0 is the leastsignificant\nbit."
        /// </summary>
        private void Instruction_129_SET_Bit_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        {
            // Type = MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                InternalDataBus_SampleFrom((Register)instructionOrigin.OpCode.Param2);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                BitSet(BitUtils.ToByte((Bit)instructionOrigin.OpCode.Param1));
                InternalDataBus_SendTo((Register)instructionOrigin.OpCode.Param2);
            }
        }

        /// <summary>
        /// OpCodeName = "SET",
        /// Param1Type = AddressingMode.Bit,
        /// Param2Type = AddressingMode.RegisterIndirect,
        /// Equation = "(HL)b <- 1",
        /// Param1List = "0","1","2","3","4","5","6","7"
        /// Param2List = "(HL)" 
        /// UserManualPage = "233",
        /// Description = "Bit b in the memory location addressed by the contents of register pair HL\nis set.",
        /// ConditionBitsAffected = "None",
        /// Example = "If the contents of the HL register pair are 3000H, at execution of\nSET 4, (HL) bit 4 in memory location 3000H is 1. Bit 0 in memory\nlocation 3000H is the least-significant bit."
        /// </summary>
        private void Instruction_130_SET_Bit_Register16Address(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
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
                BitSet(BitUtils.ToByte((Bit)instructionOrigin.OpCode.Param1));
            }
            // MachineCycleType.MW, TStates = 3 
        }

        /// <summary>
        /// OpCodeName = "SET",
        /// Param1Type = AddressingMode.Bit,
        /// Param2Type = AddressingMode.Indexed,
        /// Equation = "(IX+d)b <- 1",
        /// Param1List = "0","1","2","3","4","5","6","7"
        /// Param2List = "(IX+d)","(IY+d)"
        /// UserManualPage = "234\n235",
        /// Description = "Bit b in the memory location addressed by the sum of the contents of the IX\nregister pair and the two�s complement integer d is set.",
        /// ConditionBitsAffected = "None",
        /// Example = "If the contents of Index Register are 2000H, at execution of\nSET 0, (IX + 3H) bit 0 in memory location 2003H is 1.\nBit 0 in memory location 2003H is the least-significant bit."
        /// </summary>
        private void Instruction_131_SET_Bit_IndexedAddress(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        {
            // MachineCycleType.OCF, TStates = 5
            // MachineCycleType.MR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom(InternalDataBusConnection.W);
                InternalAddressBus_SampleFrom(RegisterUtils.GetFromAddressIndexBase((AddressIndexBase)instructionOrigin.OpCode.Param2));
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromRegisterWZPlusDisplacement();
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                BitSet(BitUtils.ToByte((Bit)instructionOrigin.OpCode.Param1));
            }
            // MachineCycleType.MW, TStates = 3
        }

        /// <summary>
        /// OpCodeName = "SET",
        /// Param1Type = AddressingMode.Bit,
        /// Param2Type = AddressingMode.Indexed,
        /// Param3Type = AddressingMode.Register,
        /// Equation = "(IX+d)b <- 1",
        /// Param1List = new string[] { "0","1","2","3","4","5","6","7" },
        /// Param2List = new string[] { "(IX+d)","(IY+d)" },
        /// Param3List = new string[] { "A","B","C","D","E","H","L" },
        /// Description = "Bit b in the memory location addressed by the sum of the contents of the IX\nregister pair and the two�s complement integer d is set.\nThe undocumented DDCB instructions store the result (if any) of the operation in one of the\nseven all-purpose registers.",
        /// ConditionBitsAffected = "None",
        /// Example = "If the contents of Index Register are 2000H, at execution of\nSET 0, (IX + 3H) bit 0 in memory location 2003H is 1.\nBit 0 in memory location 2003H is the least-significant bit."
        /// </summary>
        private void Instruction_132_SET_Bit_IndexedAddress_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        {
            //  MachineCycleType.OCF, TStates = 5
            //  MachineCycleType.MR, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom(InternalDataBusConnection.W);
                InternalAddressBus_SampleFrom(RegisterUtils.GetFromAddressIndexBase((AddressIndexBase)instructionOrigin.OpCode.Param2));
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromRegisterWZPlusDisplacement();
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                BitSet(BitUtils.ToByte((Bit)instructionOrigin.OpCode.Param1));
                InternalDataBus_SendTo((Register)instructionOrigin.OpCode.Param3);
            }
            //  MachineCycleType.MW, TStates = 3
        }
    }
}