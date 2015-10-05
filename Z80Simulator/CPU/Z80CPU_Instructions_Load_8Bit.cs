using Z80Simulator.Instructions;

namespace Z80Simulator.CPU
{
    /// <summary>
    /// ZiLOG Z80 CPU
    /// 
    /// Instructions - 8-Bit Load Group
    /// </summary>
    public partial class Z80CPU
    {
        /// <summary>
        /// OpCodeName = "LD",
        /// Param1Type = AddressingMode.Register,
        /// Param2Type = AddressingMode.Immediate,
        /// Equation = "r <- n",
        /// Param1List ="A","B","C","D","E","H","L","IXh","IXl","IYh","IYl"
        /// Param2List = "n"
        /// UserManualPage = "82",
        /// Description = "The 8-bit integer n is loaded to any register r,.",
        /// </summary>
        private void Instruction_59_LD_Register_Number8(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.OD, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo((Register)instructionOrigin.OpCode.Param1);
            }
        }

        /// <summary>
        ///  OpCodeName = "LD",
        ///  Param1Type = AddressingMode.Register,
        ///  Param2Type = AddressingMode.Register,
        ///  Equation = "r <- r'",
        ///  Param1List : "A","B","C","D","E","H","L","IXh","IXl","IYh","IYl","R","I"
        ///  Param2List : "A","B","C","D","E","H","L","IXh","IXl","IYh","IYl","R","I"
        ///  UserManualPage = "81",
        ///  Description = "The contents of any register r' are loaded to any other register r.', 
        /// </summary>
        private void Instruction_60_LD_Register_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                InternalDataBus_SampleFrom((Register)instructionOrigin.OpCode.Param2);
                InternalDataBus_SendTo((Register)instructionOrigin.OpCode.Param1);

                // Special case : LD A,I and LD A,R => impact on the flags
                ComputeFlagsForLDAIandR();    
            }
        }        

        /// <summary>
        /// OpCodeName = "LD",
        /// Param1Type = AddressingMode.Register,
        /// Param2Type = AddressingMode.Extended,
        /// Equation = "A <- (nn)",
        /// Param1List = "A"
        /// Param2List = "(nn)"
        /// UserManualPage = "94",
        /// Description = "The contents of the memory location specified by the operands nn are\nloaded to the Accumulator. The first n operand after the Op Code is the low\norder byte of a 2-byte memory address.",
        /// MEMPTR = addr + 1
        /// </summary>
        private void Instruction_61_LD_Register_Address(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
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
            }
            // MachineCycleType.MR, TStates = 3
            if (machineCycleCountAfterInstruction == 4 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFromAndIncrement(InternalAddressBusConnection.WZ);
            }
            if (machineCycleCountAfterInstruction == 4 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(Register.A);
            }
        }

        /// <summary>
        /// OpCodeName = "LD",
        /// Param1Type = AddressingMode.Register,
        /// Param2Type = AddressingMode.RegisterIndirect,
        /// Equation = "r <- (HL)",
        /// Param1List = new string[] { "A","B","C","D","E","H","L" },
        /// Param2List = new string[] { "(HL)" },
        /// Param1List = new string[] { "A" },
        /// Param2List = new string[] { "(BC)","(DE)" },
        /// UserManualPage = "83",
        /// Description = "The 8-bit contents of memory location (HL) are loaded to register r,\nwhere r identifies register A, B, C, D, E, H, or L.",
        /// ConditionBitsAffected = "None",
        /// Example = "If register pair HL contains the number 75A1H, and memory address\n75A1H contains byte 58H, the execution of LD C, (HL) results in 58H in\nregister C."
        /// LD A,(BC or DE) : MEMPTR = rp + 1
        /// </summary>
        private void Instruction_62_LD_Register_Register16Address(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        {
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.MR, TStates = 3 
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                if ((Register16)instructionOrigin.OpCode.Param2 == Register16.HL)
                {
                    InternalAddressBus_SampleFrom((Register16)instructionOrigin.OpCode.Param2);
                }
                else
                {                     
                    InternalDataBus_SampleFrom(RegisterUtils.GetHigherPart((Register16)instructionOrigin.OpCode.Param2));
                    InternalDataBus_SendTo(InternalDataBusConnection.W);
                    InternalDataBus_SampleFrom(RegisterUtils.GetLowerPart((Register16)instructionOrigin.OpCode.Param2));
                    InternalDataBus_SendTo(InternalDataBusConnection.Z);
                    InternalAddressBus_SampleFromAndIncrement(InternalAddressBusConnection.WZ);
                }
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo((Register)instructionOrigin.OpCode.Param1);
            }
        }
        
        /// <summary>
        /// OpCodeName = "LD",
        /// Param1Type = AddressingMode.Register,
        /// Param2Type = AddressingMode.Indexed,
        /// Equation = "r <- (IX+d)",
        /// Param1List = new string[] { "A","B","C","D","E","H","L" },
        /// Param2List = new string[] { "(IX+d)","(IY+d)" },
        /// UserManualPage = "84\n85",
        /// Description = "The operand (IX+d), (the contents of the Index Register IX summed with\na two�s complement displacement integer d) is loaded to register r, where r identifies register A, B, C, D, E, H, or L.",
        /// ConditionBitsAffected = "None",
        /// Example = "If the Index Register IX contains the number 25AFH, the instruction LD B,\n(IX+19H) causes the calculation of the sum 25AFH + 19H, which points\nto memory location 25C8H. If this address contains byte 39H, the\ninstruction results in register B also containing 39H."
        /// </summary>
        private void Instruction_63_LD_Register_IndexedAddress(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
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
                InternalDataBus_SendTo((Register)instructionOrigin.OpCode.Param1);
            }
        }
 
        /// <summary>
        /// OpCodeName = "LD",
        /// Param1Type = AddressingMode.Extended,
        /// Param2Type = AddressingMode.Register,
        /// Equation = "(nn) <- A",
        /// Param1List = "(nn)"
        /// Param2List = "A"
        /// UserManualPage = "97",
        /// Description = "The contents of the Accumulator are loaded to the memory address\nspecified by the operand nn. The first n operand after the Op Code is the\nlow order byte of nn.",
        /// MEMPTR_low = (addr + 1) & FF, MEMPTR_high = A
        /// </summary>
        private void Instruction_67_LD_Address_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
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
            }
            // MachineCycleType.MW, TStates = 3
            if (machineCycleCountAfterInstruction == 4 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFromAndIncrement(InternalAddressBusConnection.WZ);
                InternalDataBus_SampleFrom(Register.A);
                InternalDataBus_SendTo(InternalDataBusConnection.W);
            }
        }
        
        /// <summary>
        /// OpCodeName = "LD",
        /// Param1Type = AddressingMode.RegisterIndirect,
        /// Param2Type = AddressingMode.Immediate,
        /// Equation = "(HL) <- n",
        /// Param1List = "(HL)"
        /// Param2List = "n"
        /// UserManualPage = "89",
        /// Description = "Integer n is loaded to the memory address specified by the contents of the\nHL register pair.",
        /// ConditionBitsAffected = "None",
        /// Example = "If the HL register pair contains 4444H, the instruction LD (HL), 28H\nresults in the memory location 4444H containing byte 28H."
        /// </summary>
        private void Instruction_69_LD_Register16Address_Number8(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.OD, TStates = 3
            // MachineCycleType.MW, TStates = 3
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom((Register16)instructionOrigin.OpCode.Param1);
                // InternalDataBus already contains the operand to write
            }
        }
        
        /// <summary>
        /// OpCodeName = "LD",
        /// Param1Type = AddressingMode.RegisterIndirect,
        /// Param2Type = AddressingMode.Register,
        /// Equation = "(HL) <- r",
        /// Param1List ="(HL)"
        /// Param2List = "A","B","C","D","E","H","L"
        /// Param1List = "(BC)","(DE)"
        /// Param2List = "A"
        /// UserManualPage = "86",
        /// Description = "The contents of register r are loaded to the memory location specified by\nthe contents of the HL register pair. The symbol r identifies register A, B,\nC, D, E, H, or L.",
        /// ConditionBitsAffected = "None",
        /// Example = "If the contents of register pair HL specifies memory location 2146H, and\nthe B register contains byte 29H, at execution of LD (HL), B memory\naddress 2146H also contains 29H."
        /// LD (BC or DE),A : MEMPTR_low = (rp + 1) & FF, MEMPTR_high = A
        /// </summary>
        private void Instruction_70_LD_Register16Address_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.MW, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                if ((Register16)instructionOrigin.OpCode.Param1 == Register16.HL)
                {
                    InternalAddressBus_SampleFrom((Register16)instructionOrigin.OpCode.Param1);
                    InternalDataBus_SampleFrom((Register)instructionOrigin.OpCode.Param2);
                }
                else
                {
                    InternalDataBus_SampleFrom(RegisterUtils.GetHigherPart((Register16)instructionOrigin.OpCode.Param1));
                    InternalDataBus_SendTo(InternalDataBusConnection.W);
                    InternalDataBus_SampleFrom(RegisterUtils.GetLowerPart((Register16)instructionOrigin.OpCode.Param1));
                    InternalDataBus_SendTo(InternalDataBusConnection.Z);
                    InternalAddressBus_SampleFromAndIncrement(InternalAddressBusConnection.WZ);

                    InternalDataBus_SampleFrom(Register.A);
                    InternalDataBus_SendTo(InternalDataBusConnection.W);
                }                
            }
        }
        
        /// <summary>
        /// OpCodeName = "LD",
        /// Param1Type = AddressingMode.Indexed,
        /// Param2Type = AddressingMode.Immediate,
        /// Equation = "(IX+d) <- n",
        /// Param1List = "(IX+d)","(IY+d)"
        /// Param2List = "n"
        /// UserManualPage = "90\n91",
        /// Description = "The n operand is loaded to the memory address specified by the sum of the\nIndex Register IX and the two�s complement displacement operand d.",
        /// ConditionBitsAffected = "None",
        /// Example = "If the Index Register IX contains the number 219AH, the instruction\nLD (IX+5H), 5AH results in byte 5AH in the memory address 219FH."
        /// </summary>
        private void Instruction_71_LD_IndexedAddress_Number8(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.OD, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.Z);
            }
            // MachineCycleType.OD, TStates = 5
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
            }
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 7)
            {
                InternalDataBus_SampleFrom(InternalDataBusConnection.Z);
            }
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 9)
            {
                InternalAddressBus_SampleFrom(RegisterUtils.GetFromAddressIndexBase((AddressIndexBase)instructionOrigin.OpCode.Param1));
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
                InternalAddressBus_SampleFromRegisterWZPlusDisplacement();
            }
            // MachineCycleType.MW, TStates = 3
            if (machineCycleCountAfterInstruction == 4 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom(InternalDataBusConnection.ALURightBuffer);
            }
        }
        
        /// <summary>
        /// OpCodeName = "LD",
        /// Param1Type = AddressingMode.Indexed,
        /// Param2Type = AddressingMode.Register,
        /// Equation = "(IX+d) <- r",
        /// Param1List = "(IX+d)","(IY+d)"
        /// Param2List = "A","B","C","D","E","H","L"
        /// UserManualPage = "87\n88",
        /// Description = "The contents of register r are loaded to the memory address specified by the\ncontents of Index Register IX summed with d, a two�s complement\ndisplacement integer. The symbol r identifies register A, B, C, D, E, H, or\nL.",
        /// ConditionBitsAffected = "None",
        /// Example = "If the C register contains byte 1CH, and the Index Register IX contains\n3100H, then the instruction LID (IX+6H), C performs the sum 3100H +\n6H and loads 1CH to memory location 3106H."
        /// </summary>
        private void Instruction_72_LD_IndexedAddress_Register(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
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
            // MachineCycleType.MW, TStates = 3
            if (machineCycleCountAfterInstruction == 4 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom((Register)instructionOrigin.OpCode.Param2);
            }
        }
    }
}