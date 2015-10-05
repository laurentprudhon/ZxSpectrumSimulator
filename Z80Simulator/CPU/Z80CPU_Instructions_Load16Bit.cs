using Z80Simulator.Instructions;

namespace Z80Simulator.CPU
{
    /// <summary>
    /// ZiLOG Z80 CPU
    /// 
    /// Instructions - 16-Bit Load Group
    /// </summary>
    public partial class Z80CPU
    {
        /// <summary>
        /// OpCodeName = "LD",
        /// Param1Type = AddressingMode.Register16,
        /// Param2Type = AddressingMode.Immediate16,
        /// Equation = "dd <- nn",
        /// Param1List = "BC","DE","HL","SP","IX","IY"
        /// Param2List = "nn"
        /// UserManualPage = "102",
        /// Description = "The 2-byte integer nn is loaded to the dd register pair, where dd defines the\nBC, DE, HL, or SP register pairs.",
        /// ConditionBitsAffected = "None",
        /// Example = "At execution of LD HL, 5000H the contents of the HL register pair is 5000H."
        /// </summary>
        private void Instruction_64_LD_Register16_Number16(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.ODL, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                if ((Register16)instructionOrigin.OpCode.Param1 == Register16.SP)
                {
                    InternalDataBus_SendTo(InternalDataBusConnection.SPl);
                }
                else
                {
                    Register registerLow = RegisterUtils.GetLowerPart((Register16)instructionOrigin.OpCode.Param1);
                    InternalDataBus_SendTo(registerLow);
                }
            }
            // MachineCycleType.ODH, TStates = 3
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 5)
            {
                if ((Register16)instructionOrigin.OpCode.Param1 == Register16.SP)
                {
                    InternalDataBus_SendTo(InternalDataBusConnection.SPh);
                }
                else
                {
                    Register registerHigh = RegisterUtils.GetHigherPart((Register16)instructionOrigin.OpCode.Param1);
                    InternalDataBus_SendTo(registerHigh);
                }
            }
        }
        
        /// <summary>
        /// OpCodeName = "LD",
        /// Param1Type = AddressingMode.Register16,
        /// Param2Type = AddressingMode.Register16,
        /// Equation = "SP <- HL",
        /// Param1List = "SP"
        /// Param2List = "HL","IX","IY"
        /// UserManualPage = "113",
        /// Description = "The contents of the register pair HL are loaded to the Stack Pointer (SP)",
        /// ConditionBitsAffected = "None",
        /// Example = "If the register pair HL contains 442EH, at instruction LD SP, HL the Stack\nPointer also contains 442EH."
        /// </summary>
        private void Instruction_65_LD_Register16_Register16(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 6
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 9)
            {
                Register registerLow = RegisterUtils.GetLowerPart((Register16)instructionOrigin.OpCode.Param2);
                InternalDataBus_SampleFrom(registerLow);
                InternalDataBus_SendTo(InternalDataBusConnection.SPl);
            }
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 11)
            {
                Register registerHigh = RegisterUtils.GetHigherPart((Register16)instructionOrigin.OpCode.Param2);
                InternalDataBus_SampleFrom(registerHigh);
                InternalDataBus_SendTo(InternalDataBusConnection.SPh);
            }
        }
        
        /// <summary>
        /// OpCodeName = "LD",
        /// Param1Type = AddressingMode.Register16,
        /// Param2Type = AddressingMode.Extended,
        /// Equation = "H <- (nn+1), L <- (nn)",
        /// Param1List = "HL","BC","DE","IX","IY","SP"
        /// Param2List = "(nn)"
        /// UserManualPage = "105",
        /// Description = "The contents of memory address (nn) are loaded to the low order portion of\nregister pair HL (register L), and the contents of the next highest memory\naddress (nn+1) are loaded to the high order portion of HL (register H). The\nfirst n operand after the Op Code is the low order byte of nn.",
        /// ConditionBitsAffected = "None",
        /// Example = "If address 4545H contains 37H, and address 4546H contains A1H, at\ninstruction LD HL, (4545H) the HL register pair contains A137H."
        /// MEMPTR = addr +1
        /// </summary>
        private void Instruction_66_LD_Register16_Address(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
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
            // MachineCycleType.MRL, TStates = 3
            if (machineCycleCountAfterInstruction == 4 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFromAndIncrement(InternalAddressBusConnection.WZ);
            }
            if (machineCycleCountAfterInstruction == 4 && halfTStateIndex == 5)
            {
                if ((Register16)instructionOrigin.OpCode.Param1 == Register16.SP)
                {
                    InternalDataBus_SendTo(InternalDataBusConnection.SPl);
                }
                else
                {
                    Register registerLow = RegisterUtils.GetLowerPart((Register16)instructionOrigin.OpCode.Param1);
                    InternalDataBus_SendTo(registerLow);
                }
            }
            // MachineCycleType.MRH, TStates = 3
            if (machineCycleCountAfterInstruction == 5 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(InternalAddressBusConnection.WZ);
            }
            if (machineCycleCountAfterInstruction == 5 && halfTStateIndex == 5)
            {
                if ((Register16)instructionOrigin.OpCode.Param1 == Register16.SP)
                {
                    InternalDataBus_SendTo(InternalDataBusConnection.SPh);
                }
                else
                {
                    Register registerHigh = RegisterUtils.GetHigherPart((Register16)instructionOrigin.OpCode.Param1);
                    InternalDataBus_SendTo(registerHigh);
                }
            }
        }
        
        /// <summary>
        /// OpCodeName = "LD",
        /// Param1Type = AddressingMode.Extended,
        /// Param2Type = AddressingMode.Register16,
        /// Equation = "(nn+1) <- H, (nn) <- L",
        /// Param1List = "(nn)"
        /// Param2List = "HL","BC","DE","IX","IY","SP"
        /// UserManualPage = "109",
        /// Description = "The contents of the low order portion of register pair HL (register L) are\nloaded to memory address (nn), and the contents of the high order portion\nof HL (register H) are loaded to the next highest memory address (nn+1).\nThe first n operand after the Op Code is the low order byte of nn.",
        /// ConditionBitsAffected = "None",
        /// Example = "If the content of register pair HL is 483AH, at instruction\nLD (B2291-1), HL address B229H contains 3AH, and address B22AH\ncontains 48H."
        /// MEMPTR = addr +1
        /// </summary>
        private void Instruction_68_LD_Address_Register16(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
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
            // MachineCycleType.MWL, TStates = 3
            if (machineCycleCountAfterInstruction == 4 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFromAndIncrement(InternalAddressBusConnection.WZ);
                if ((Register16)instructionOrigin.OpCode.Param2 == Register16.SP)
                {
                    InternalDataBus_SampleFrom(InternalDataBusConnection.SPl);
                }
                else
                {
                    Register registerLow = RegisterUtils.GetLowerPart((Register16)instructionOrigin.OpCode.Param2);
                    InternalDataBus_SampleFrom(registerLow);
                }
            }
            // MachineCycleType.MWH, TStates = 3
            if (machineCycleCountAfterInstruction == 5 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(InternalAddressBusConnection.WZ);
                if ((Register16)instructionOrigin.OpCode.Param2 == Register16.SP)
                {
                    InternalDataBus_SampleFrom(InternalDataBusConnection.SPh);
                }
                else
                {
                    Register registerHigh = RegisterUtils.GetHigherPart((Register16)instructionOrigin.OpCode.Param2);
                    InternalDataBus_SampleFrom(registerHigh);
                }
            }
        }
                
        /// <summary>
        /// OpCodeName = "POP",
        /// Param1Type = AddressingMode.Register16,
        /// Equation = "qqH <- (SP+1), qqL <- (SP)",
        /// Param1List = "AF","BC","DE","HL","IX","IY"
        /// UserManualPage = "119",
        /// Description = "The top two bytes of the external memory LIFO (last-in, first-out) Stack\nare popped to register pair qq. The Stack Pointer (SP) register pair holds\nthe 16-bit address of the current top of the Stack. This instruction first\nloads to the low order portion of qq, the byte at memory location\ncorresponding to the contents of SP, then SP is incriminated and the\ncontents of the corresponding adjacent memory location are loaded to the\nhigh order portion of qq and the SP is now incriminated again. The\noperand qq identifies register pair BC, DE, HL, or AF.",
        /// ConditionBitsAffected = "None",
        /// Example = "If the Stack Pointer contains 1000H, memory location 1000H contains 55H,\nand location 1001H contains 33H, the instruction POP HL results in register\npair HL containing 3355H, and the Stack Pointer containing 1002H."
        /// </summary>
        private void Instruction_90_POP_Register16(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.SRL, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                if ((Register16)instructionOrigin.OpCode.Param1 == Register16.AF)
                {
                    InternalDataBus_SendTo(InternalDataBusConnection.F);
                }
                else
                {
                    Register registerLow = RegisterUtils.GetLowerPart((Register16)instructionOrigin.OpCode.Param1);
                    InternalDataBus_SendTo(registerLow);
                }
            }
            // MachineCycleType.SRH, TStates = 3
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 5)
            {
                if ((Register16)instructionOrigin.OpCode.Param1 == Register16.AF)
                {
                    InternalDataBus_SendTo(Register.A);
                }
                else
                {
                    Register registerHigh = RegisterUtils.GetHigherPart((Register16)instructionOrigin.OpCode.Param1);
                    InternalDataBus_SendTo(registerHigh);
                }
            }
        }
        
        /// <summary>
        /// OpCodeName = "PUSH",
        /// Param1Type = AddressingMode.Register16,
        /// Equation = "(SP-2) <- qqL, (SP-1) <- qqH",
        /// Param1List = "AF","BC","DE","HL","IX","IY"
        /// UserManualPage = "116",
        /// Description = "The contents of the register pair qq are pushed to the external memory\nLIFO (last-in, first-out) Stack. The Stack Pointer (SP) register pair holds the\n16-bit address of the current top of the Stack. This instruction first\ndecrements SP and loads the high order byte of register pair qq to the\nmemory address specified by the SP. The SP is decremented again and\nloads the low order byte of qq to the memory location corresponding to this\nnew address in the SP. The operand qq identifies register pair BC, DE, HL,\nor AF.",
        /// ConditionBitsAffected = "None",
        /// Example = "If the AF register pair contains 2233H and the Stack Pointer contains\n1007H, at instruction PUSH AF memory address 1006H contains 22H,\nmemory address 1005H contains 33H, and the Stack Pointer contains\n1005H."
        /// </summary>
        private void Instruction_91_PUSH_Register16(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 5
            // MachineCycleType.SWH, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                if ((Register16)instructionOrigin.OpCode.Param1 == Register16.AF)
                {
                    InternalDataBus_SampleFrom(Register.A);
                }
                else
                {
                    Register registerHigh = RegisterUtils.GetHigherPart((Register16)instructionOrigin.OpCode.Param1);
                    InternalDataBus_SampleFrom(registerHigh);
                }
            }
            // MachineCycleType.SWL, TStates = 3
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 0)
            {
                if ((Register16)instructionOrigin.OpCode.Param1 == Register16.AF)
                {
                    InternalDataBus_SampleFrom(InternalDataBusConnection.F);
                }
                else
                {
                    Register registerLow = RegisterUtils.GetLowerPart((Register16)instructionOrigin.OpCode.Param1);
                    InternalDataBus_SampleFrom(registerLow);
                }
            }
        }
    }
}