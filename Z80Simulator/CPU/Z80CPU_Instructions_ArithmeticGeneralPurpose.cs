using Z80Simulator.Instructions;

namespace Z80Simulator.CPU
{
    /// <summary>
    /// ZiLOG Z80 CPU
    /// 
    /// Instructions - General-Purpose Arithmetic Group
    /// </summary>
    public partial class Z80CPU
    {
        /// <summary>
        /// OpCodeName = "CCF",
        /// Equation = "CY <- ~CY",
        /// UserManualPage = "170",
        /// Description = "The Carry flag in the F register is inverted.",
        /// ConditionBitsAffected = "S is not affected\nZ is not affected\nH, previous carry is copied\nP/V is not affected\nN is reset\nC is set if CY was 0 before operation, reset otherwise"
        /// </summary>        
        private void Instruction_20_CCF(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                InvertCarryFlag();
            }
        }

        /// <summary>
        /// OpCodeName = "CPL",
        /// Equation = "A <- ~A",
        /// UserManualPage = "168",
        /// Description = "The contents of the Accumulator (register A) are inverted (one's complement).",
        /// ConditionBitsAffected = "S is not affected\nZ is not affected\nH is set\nP/V is not affected\nN is set\nC is not affected",
        /// Example = "If the contents of the Accumulator are 1011 0100, at execution of CPL\nthe Accumulator contents are 0100 1011." 
        /// </summary>
        private void Instruction_29_CPL(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                InvertAccumulator();
            }
        }

        /// <summary>
        /// OpCodeName = "DAA",
        /// UserManualPage = "166",
        /// Description = "This instruction conditionally adjusts the Accumulator for BCD addition and\nsubtraction operations. For addition (ADD, ADC, INC) or subtraction (SUB,\nSBC, DEC, NEG), the following table indicates the operation performed:\nOperation\nC Before\nDAA\nHex Value In\nUpper Digit\n(bit 7-4)\nH Before\nDAA\nHex Value\nIn Lower\nDigit\n(bit 3-0)\nNumber\nAdded To\nByte\nC After\nDAA\n0 9-0 0 0-9 00 0\n0 0-8 0 A-F 06 0\n0 0-9 1 0-3 06 0\nADD 0 A-F 0 0-9 60 1\nADC 0 9-F 0 A-F 66 1\nINC 0 A-F 1 0-3 66 1\n1 0-2 0 0-9 60 1\n1 0-2 0 A-F 66 1\n1 0-3 1 0-3 66 1\nSUB 0 0-9 0 0-9 00 0\nSBC 0 0-8 1 6-F FA 0\nDEC 1 7-F 0 0-9 A0 1\nNEG 1 6-7 1 6-F 9A 1",
        /// ConditionBitsAffected = "S is set if most-significant bit of Accumulator is 1 after operation, reset\notherwise\nZ is set if Accumulator is zero after operation, reset otherwise\nH, see instruction\nP/V is set if Accumulator is even parity after operation, reset otherwise\nN is not affected\nC, see instruction",
        /// Example = "If an addition operation is performed between 15 (BCD) and 27 (BCD),\nsimple decimal arithmetic gives this result:\n15\n+27\n42\nBut when the binary representations are added in the Accumulator\naccording to standard binary arithmetic.\n0001 0101\n+ 0010 0111\n0011 1100 = 3C\nthe sum is ambiguous. The DAA instruction adjusts this result so that the\ncorrect BCD representation is obtained:\n0011 1100\n+ 0000 0110\n0100 0010 = 42"
        /// </summary>
        private void Instruction_30_DAA(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        {
            // MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                // Sample accumulator value into ALULeftBuffer
                ALULeftBuffer_SampleFrom(ALULeftBufferConnection.A);
                DecimalAdjust();
                InternalDataBus_SendTo(Register.A);
            }
        }

        /// <summary>
        /// OpCodeName = "NEG",
        /// Equation = "A <- 0 - A",
        /// UserManualPage = "169",
        /// Description = "The contents of the Accumulator are negated (two's complement). This is\nthe same as subtracting the contents of the Accumulator from zero. Note\nthat 80H is left unchanged.",
        /// ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is 0, reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is set if Accumulator was 80H before operation, reset otherwise\nN is set\nC is set if Accumulator was not 00H before operation, reset otherwise",
        /// Example = "If the contents of the Accumulator are\n1 0 0 1 1 0 0 0\nat execution of NEG the Accumulator contents are\n0 1 1 0 1 0 0 0"  
        /// </summary>
        private void Instruction_77_NEG(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                // Load value 0 into ALULeftBuffer
                ALULeftBuffer_Reset();
                // Sample accumulator value into ALURightBuffer
                InternalDataBus_SampleFrom(Register.A);
                InternalDataBus_SendTo(InternalDataBusConnection.ALURightBuffer);
                Sub(false, false);
                InternalDataBus_SendTo(Register.A);
            }
        }

        /// <summary>
        ///  OpCodeName = "SCF",
        ///  Equation = "CY <- 1",
        ///  UserManualPage = "171",
        ///  Description = "The Carry flag in the F register is set.",
        ///  ConditionBitsAffected = "S is not affected\nZ is not affected\nH is reset\nP/V is not affected\nN is reset\nC is set"
        /// </summary>
        private void Instruction_128_SCF(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        {
            // MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                SetCarryFlag();
            }
        }
    }
}