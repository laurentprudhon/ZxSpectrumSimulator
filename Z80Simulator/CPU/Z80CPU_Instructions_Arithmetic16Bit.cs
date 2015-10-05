using Z80Simulator.Instructions;

namespace Z80Simulator.CPU
{
    /// <summary>
    /// ZiLOG Z80 CPU
    /// 
    /// Instructions - 16-Bit Arithmetic Group
    /// </summary>
    public partial class Z80CPU
    {
        /// <summary>
        /// OpCodeName = "ADC",
        /// Param1Type = AddressingMode.Register16,
        /// Param2Type = AddressingMode.Register16,
        /// Equation = "HL <- HL + ss + CY",
        /// Param1List = "HL"
        /// Param2List = "BC","DE","HL","SP"
        /// UserManualPage = "180",
        /// Description = "The contents of register pair ss (any of register pairs BC, DE, HL, or SP)\nare added with the Carry flag (C flag in the F register) to the contents of\nregister pair HL, and the result is stored in HL.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nR is set if carry out of bit 11,. reset otherwise\nP/V is set if overflow, reset otherwise\nN is reset\nC is set if carry from bit 15, reset otherwise",
        /// Example = "If the register pair BC contains 2222H, register pair HL contains 5437H,\nand the Carry Flag is set, at execution of ADC HL, BC the contents of\nHL are 765AH."
        /// </summary>
        private void Instruction_5_ADC_Register16_Register16(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        {
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.CPU, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 1)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.Z);
                if ((Register16)instructionOrigin.OpCode.Param2 == Register16.SP)
                {
                    InternalDataBus_SampleFrom(InternalDataBusConnection.SPl);
                }
                else
                {
                    Register registerLow = RegisterUtils.GetLowerPart((Register16)instructionOrigin.OpCode.Param2);
                    InternalDataBus_SampleFrom(registerLow);
                }
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Add(true, false, true);
                InternalDataBus_SendTo(Register.L);
            }
            // MachineCycleType.CPU, TStates = 3
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 5)
            {
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.W);
                Register16_Increment(InternalAddressBusConnection.WZ);
                if ((Register16)instructionOrigin.OpCode.Param2 == Register16.SP)
                {
                    InternalDataBus_SampleFrom(InternalDataBusConnection.SPh);
                }
                else
                {
                    Register registerHigh = RegisterUtils.GetHigherPart((Register16)instructionOrigin.OpCode.Param2);
                    InternalDataBus_SampleFrom(registerHigh);
                }
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Add(true, true, true);
                InternalDataBus_SendTo(Register.H);
            }
        }

        /// <summary>
        /// OpCodeName = "ADD",
        /// Param1Type = AddressingMode.Register16,
        /// Param2Type = AddressingMode.Register16,
        /// Equation = "HL <- HL + ss",
        /// Param1List = "HL"
        /// Param2List = "BC","DE","HL","SP"
        /// Param1List = "IX"
        /// Param2List = "BC","DE","IX","SP"
        /// Param1List = "IY"
        /// Param2List = "BC","DE","IY","SP"
        /// UserManualPage = "179",
        /// Description = "The contents of register pair ss (any of register pairs BC, DE, HL, or SP)\nare added to the contents of register pair HL and the result is stored in HL.",
        /// ConditionBitsAffected = "S is not affected\nZ is not affected\nH is set if carry out of bit 11, reset otherwise\nP/V is not affected\nN is reset\nC is set if carry from bit 15, reset otherwise",
        /// Example = "If register pair HL contains the integer 4242H, and register pair DE contains\n1111H, at execution of ADD HL, DE the HL register pair contains 5353H."
        /// </summary>
        private void Instruction_10_ADD_Register16_Register16(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.CPU, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 1)
            {
                if ((Register16)instructionOrigin.OpCode.Param1 == Register16.HL)
                {
                    InternalAddressBus_SampleFrom(Register16.HL);
                }
                else if ((Register16)instructionOrigin.OpCode.Param1 == Register16.IX)
                {
                    InternalAddressBus_SampleFrom(InternalAddressBusConnection.IX);
                }
                else if ((Register16)instructionOrigin.OpCode.Param1 == Register16.IY)
                {
                    InternalAddressBus_SampleFrom(InternalAddressBusConnection.IY);
                }
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.Z);
                if ((Register16)instructionOrigin.OpCode.Param2 == Register16.SP)
                {
                    InternalDataBus_SampleFrom(InternalDataBusConnection.SPl);
                }
                else
                {
                    Register registerLow = RegisterUtils.GetLowerPart((Register16)instructionOrigin.OpCode.Param2);
                    InternalDataBus_SampleFrom(registerLow);
                }
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Add(false, false, false);
                if ((Register16)instructionOrigin.OpCode.Param1 == Register16.HL)
                {
                    InternalDataBus_SendTo(Register.L);
                }
                else if ((Register16)instructionOrigin.OpCode.Param1 == Register16.IX)
                {
                    InternalDataBus_SendTo(Register.IXl);
                }
                else if ((Register16)instructionOrigin.OpCode.Param1 == Register16.IY)
                {
                    InternalDataBus_SendTo(Register.IYl);
                }                
            }
            // MachineCycleType.CPU, TStates = 3
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 5)
            {
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.W);
                Register16_Increment(InternalAddressBusConnection.WZ);
                if ((Register16)instructionOrigin.OpCode.Param2 == Register16.SP)
                {
                    InternalDataBus_SampleFrom(InternalDataBusConnection.SPh);
                }
                else
                {
                    Register registerHigh = RegisterUtils.GetHigherPart((Register16)instructionOrigin.OpCode.Param2);
                    InternalDataBus_SampleFrom(registerHigh);
                }
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Add(true, true, false);
                if ((Register16)instructionOrigin.OpCode.Param1 == Register16.HL)
                {
                    InternalDataBus_SendTo(Register.H);
                }
                else if ((Register16)instructionOrigin.OpCode.Param1 == Register16.IX)
                {
                    InternalDataBus_SendTo(Register.IXh);
                }
                else if ((Register16)instructionOrigin.OpCode.Param1 == Register16.IY)
                {
                    InternalDataBus_SendTo(Register.IYh);
                }    
            }
        }

        /// <summary>
        /// OpCodeName = "DEC",
        /// Param1Type = AddressingMode.Register16,
        /// Equation = "ss <- ss - 1",
        /// Param1List = "BC","DE","HL","SP","IX","IY"
        /// UserManualPage = "187",
        /// Description = "The contents of register pair ss (any of the register pairs BC, DE, HL, or\nSP) are decremented.",
        /// ConditionBitsAffected = "None",
        /// Example = "If register pair HL contains 1001H, at execution of DEC HL the contents\nof HL are 1000H."
        /// </summary>
        private void Instruction_34_DEC_Register16(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        {
            // MachineCycleType.OCF, TStates = 6 
            
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 11)
            {
                Register16_Decrement((Register16)instructionOrigin.OpCode.Param1);
            }
        }        

        /// <summary>
        /// OpCodeName = "INC",
        /// Param1Type = AddressingMode.Register16,
        /// Equation = "ss <- ss + 1",
        /// Param1List = "BC","DE","HL","SP","IX","IY"
        /// UserManualPage = "184",
        /// Description = "The contents of register pair ss (any of register pairs BC, DE, HL, or SP)\nare incremented.",
        /// ConditionBitsAffected = "None",
        /// Example = "If the register pair contains 1000H, after the execution of INC HL, HL\ncontains 1001H."
        /// </summary>
        private void Instruction_49_INC_Register16(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 6             
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 11)
            {
                Register16_Increment((Register16)instructionOrigin.OpCode.Param1);
            }
        }
        
        /// <summary>
        /// OpCodeName = "SBC",
        /// Param1Type = AddressingMode.Register16,
        /// Param2Type = AddressingMode.Register16,
        /// Equation = "HL <- HL - ss - CY",
        /// Param1List = "HL"
        /// Param2List = "BC","DE","HL","SP"
        /// UserManualPage = "181",
        /// Description = "The contents of the register pair ss (any of register pairs BC, DE, HL, or\nSP) and the Carry Flag (C flag in the F register) are subtracted from the\ncontents of register pair HL, and the result is stored in HL.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if a borrow from bit 12, reset otherwise\nP/V is set if overflow, reset otherwise\nN is set\nC is set if borrow, reset otherwise",
        /// Example = "If the contents of the HL, register pair are 9999H, the contents of register\npair DE are 1111H, and the Carry flag is set. At execution of SBC HL, DE\nthe contents of HL are 8887H."
        /// </summary>
        private void Instruction_127_SBC_Register16_Register16(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.CPU, TStates = 4
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 1)
            {
                InternalAddressBus_SampleFrom(Register16.HL);
                InternalAddressBus_SendTo(InternalAddressBusConnection.WZ);
            }
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 7)
            {
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.Z);
                if ((Register16)instructionOrigin.OpCode.Param2 == Register16.SP)
                {
                    InternalDataBus_SampleFrom(InternalDataBusConnection.SPl);
                }
                else
                {
                    Register registerLow = RegisterUtils.GetLowerPart((Register16)instructionOrigin.OpCode.Param2);
                    InternalDataBus_SampleFrom(registerLow);
                }
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Sub(true, false);
                InternalDataBus_SendTo(Register.L);
            }
            // MachineCycleType.CPU, TStates = 3
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 5)
            {
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.W);
                Register16_Increment(InternalAddressBusConnection.WZ);
                if ((Register16)instructionOrigin.OpCode.Param2 == Register16.SP)
                {
                    InternalDataBus_SampleFrom(InternalDataBusConnection.SPh);
                }
                else
                {
                    Register registerHigh = RegisterUtils.GetHigherPart((Register16)instructionOrigin.OpCode.Param2);
                    InternalDataBus_SampleFrom(registerHigh);
                }
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Sub(true, true);
                InternalDataBus_SendTo(Register.H);
            }
        }
    }
}