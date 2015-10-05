using System;
using Z80Simulator.Instructions;

namespace Z80Simulator.CPU
{
    /// <summary>
    /// ZiLOG Z80 CPU
    /// 
    /// Instructions decoder
    /// 
    /// As each instruction is fetched from memory, it is placed in the
    /// INSTRUCTION register and decoded. The control sections performs this
    /// function and then generates and supplies the control signals necessary to
    /// read or write data from or to the registers, control the ALU, and provide
    /// required external control signals.
    /// </summary>
    public partial class Z80CPU
    {          
        #region Decode instructions from memory opcodes during opcode fetch

        /// <summary>
        /// Instruction Register (IR)
        /// 
        /// The instruction register is 8 bits wide and is used to contain the instruction
        /// just fetched from memory. It is not accessible to the programmer.
        /// Once the instruction is contained in IR, the control unit of the microprocessor
        /// will be able to generate the correct sequence of internal and external signals
        /// for the execution of the specified instruction.
        /// </summary>
        private byte IR { get; set; }

        // Table used to decode the instruction being fetched from memory (see Z80OpCodes.Tables)
        private byte opcodeTableIndex = 0;
                
        // Read instruction register and decode instruction
        private void DecodeInstruction()
        {
            if (instructionOrigin.Source != InstructionSource.Internal) // internal instruction already decoded, nothing to do
            {
                InstructionCode candidateInstructionCode = Z80OpCodes.Tables[opcodeTableIndex, IR];

                // Ignore DD and FD prefixes if the second byte of the opcode does not lead to a valid instruction
                if (candidateInstructionCode.FetchMoreBytes == 0 && candidateInstructionCode.SwitchToTableNumber == 0)
                {
                    // Ignore the prefix and try to find the candidate instruction in the first opcode table
                    candidateInstructionCode = Z80OpCodes.Tables[0, IR];

                    // The previous machine cycle did not contribute to the new instruction
                    machineCycleIndex--;
                    instructionOrigin.Address++;
                }

                // An opcode was recognized, decode it and prepare its execution
                if (candidateInstructionCode.FetchMoreBytes == 0 && candidateInstructionCode.SwitchToTableNumber == null)
                {
                    opcodeTableIndex = 0;
                    InstructionCode instructionCode = candidateInstructionCode;

                    // Register decoded instruction
                    instructionOrigin.OpCode = instructionCode;
                    currentInstruction = new Instruction(instructionCode.InstructionTypeIndex, instructionCode.InstructionTypeParamVariant);
                    DecodeInstructionExecutionMethod();

                    // Correct the actual duration of the current opcode fetch cycle (depends on the instruction)
                    currentMachineCycle = currentInstruction.ExecutionTimings.MachineCycles[machineCycleIndex];
                }
                // We need to read more bytes to recognize a multi-byte opcode
                else if (candidateInstructionCode.FetchMoreBytes > 0)
                {
                    opcodeTableIndex = candidateInstructionCode.SwitchToTableNumber.Value;
                }
            }

            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.CPUControlDecodeInstruction));
            }
        }

        #endregion

        #region Interrupt mode 0 - Interrupting device instruction inserted on the data bus

        private void DecodeInterruptingDeviceInstruction()
        {
            // The interrupting device inserts an instruction code on the data bus
            InstructionCode candidateInstructionCode = Z80OpCodes.Tables[0, InternalDataBus];            

            // An opcode was recognized, decode it and prepare its execution
            if (candidateInstructionCode.FetchMoreBytes == 0 && candidateInstructionCode.SwitchToTableNumber == null)
            {
                InstructionCode instructionCode = candidateInstructionCode;

                // Register decoded instruction
                instructionOrigin = new InstructionOrigin(InstructionSource.InterruptingDevice, instructionCode, 0);
                currentInstruction = new Instruction(instructionCode.InstructionTypeIndex, instructionCode.InstructionTypeParamVariant);
                DecodeInstructionExecutionMethod();                

                // Correct the actual duration of the current opcode fetch cycle (depends on the instruction)
                currentMachineCycle = currentInstruction.ExecutionTimings.MachineCycles[machineCycleIndex];

                // Decrease the half T state index to execute the decoded instruction as if it was fetched from memory
                halfTStateIndex = 4;
            }
            // We need to read more bytes to recognize a multi-byte opcode
            else
            {
                throw new NotSupportedException("Multi bytes opcodes are not supported in interrupt mode 0 by this simulator");
            }

            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.CPUControlDecodeInterruptingDeviceInstruction));
            }
        }

        #endregion

        #region Prepare CPU internal state to execute an instruction

        // Prototype for all the instructions
        private delegate void ExecuteInstruction(byte machineCycleCountAfterInstruction, byte halfTStateIndex);  

        private void DecodeInstructionExecutionMethod()
        {
            // Initialize the machine cycle counter AFTER intruction decoding
            if (currentInstruction != Instruction.FetchNewInstruction)
            {
                machineCycleCountAfterInstruction = 1;
            }

            // Select instruction execution method
            switch (currentInstruction.Type.Index)
            {
                case 1:
                    executeInstruction = Instruction_1_ADC_Register_Number8;
                    break;
                case 2:
                    executeInstruction = Instruction_2_ADC_Register_Register;
                    break;
                case 3:
                    executeInstruction = Instruction_3_ADC_Register_Register16Address;
                    break;
                case 4:
                    executeInstruction = Instruction_4_ADC_Register_IndexedAddress;
                    break;
                case 5:
                    executeInstruction = Instruction_5_ADC_Register16_Register16;
                    break;
                case 6:
                    executeInstruction = Instruction_6_ADD_Register_Number8;
                    break;
                case 7:
                    executeInstruction = Instruction_7_ADD_Register_Register;
                    break;
                case 8:
                    executeInstruction = Instruction_8_ADD_Register_Register16Address;
                    break;
                case 9:
                    executeInstruction = Instruction_9_ADD_Register_IndexedAddress;
                    break;
                case 10:
                    executeInstruction = Instruction_10_ADD_Register16_Register16;
                    break;
                case 11:
                    executeInstruction = Instruction_11_AND_Number8;
                    break;
                case 12:
                    executeInstruction = Instruction_12_AND_Register;
                    break;
                case 13:
                    executeInstruction = Instruction_13_AND_Register16Address;
                    break;
                case 14:
                    executeInstruction = Instruction_14_AND_IndexedAddress;
                    break;
                case 15:
                    executeInstruction = Instruction_15_BIT_Bit_Register;
                    break;
                case 16:
                    executeInstruction = Instruction_16_BIT_Bit_Register16Address;
                    break;
                case 17:
                    executeInstruction = Instruction_17_BIT_Bit_IndexedAddress;
                    break;
                case 18:
                    executeInstruction = Instruction_18_CALL_Address;
                    break;
                case 19:
                    executeInstruction = Instruction_19_CALL_FlagCondition_Address;
                    break;
                case 20:
                    executeInstruction = Instruction_20_CCF;
                    break;
                case 21:
                    executeInstruction = Instruction_21_CP_Number8;
                    break;
                case 22:
                    executeInstruction = Instruction_22_CP_Register;
                    break;
                case 23:
                    executeInstruction = Instruction_23_CP_Register16Address;
                    break;
                case 24:
                    executeInstruction = Instruction_24_CP_IndexedAddress;
                    break;
                case 25:
                    executeInstruction = Instruction_25_CPD;
                    break;
                case 26:
                    executeInstruction = Instruction_26_CPDR;
                    break;
                case 27:
                    executeInstruction = Instruction_27_CPI;
                    break;
                case 28:
                    executeInstruction = Instruction_28_CPIR;
                    break;
                case 29:
                    executeInstruction = Instruction_29_CPL;
                    break;
                case 30:
                    executeInstruction = Instruction_30_DAA;
                    break;
                case 31:
                    executeInstruction = Instruction_31_DEC_Register;
                    break;
                case 32:
                    executeInstruction = Instruction_32_DEC_Register16Address;
                    break;
                case 33:
                    executeInstruction = Instruction_33_DEC_IndexedAddress;
                    break;
                case 34:
                    executeInstruction = Instruction_34_DEC_Register16;
                    break;
                case 35:
                    executeInstruction = Instruction_35_DI;
                    break;
                case 36:
                    executeInstruction = Instruction_36_DJNZ_RelativeDisplacement;
                    break;
                case 37:
                    executeInstruction = Instruction_37_EI;
                    break;
                case 38:
                    executeInstruction = Instruction_38_EX_Register16_Register16;
                    break;
                case 39:
                    executeInstruction = Instruction_39_EX_Register16Address_Register16;
                    break;
                case 40:
                    executeInstruction = Instruction_40_EXX;
                    break;
                case 41:
                    executeInstruction = Instruction_41_HALT;
                    break;
                case 42:
                    executeInstruction = Instruction_42_IM_InterruptMode;
                    break;
                case 43:
                    executeInstruction = Instruction_43_IN_Register_IOPort;
                    break;
                case 44:
                    executeInstruction = Instruction_44_IN_Register_RegisterIOPort;
                    break;
                case 45:
                    executeInstruction = Instruction_45_IN_Flags_RegisterIOPort;
                    break;
                case 46:
                    executeInstruction = Instruction_46_INC_Register;
                    break;
                case 47:
                    executeInstruction = Instruction_47_INC_Register16Address;
                    break;
                case 48:
                    executeInstruction = Instruction_48_INC_IndexedAddress;
                    break;
                case 49:
                    executeInstruction = Instruction_49_INC_Register16;
                    break;
                case 50:
                    executeInstruction = Instruction_50_IND;
                    break;
                case 51:
                    executeInstruction = Instruction_51_INDR;
                    break;
                case 52:
                    executeInstruction = Instruction_52_INI;
                    break;
                case 53:
                    executeInstruction = Instruction_53_INIR;
                    break;
                case 54:
                    executeInstruction = Instruction_54_JP_Address;
                    break;
                case 55:
                    executeInstruction = Instruction_55_JP_Register16Address;
                    break;
                case 56:
                    executeInstruction = Instruction_56_JP_FlagCondition_Address;
                    break;
                case 57:
                    executeInstruction = Instruction_57_JR_RelativeDisplacement;
                    break;
                case 58:
                    executeInstruction = Instruction_58_JR_FlagCondition_RelativeDisplacement;
                    break;
                case 59:
                    executeInstruction = Instruction_59_LD_Register_Number8;
                    break;
                case 60:
                    executeInstruction = Instruction_60_LD_Register_Register;
                    break;
                case 61:
                    executeInstruction = Instruction_61_LD_Register_Address;
                    break;
                case 62:
                    executeInstruction = Instruction_62_LD_Register_Register16Address;
                    break;
                case 63:
                    executeInstruction = Instruction_63_LD_Register_IndexedAddress;
                    break;
                case 64:
                    executeInstruction = Instruction_64_LD_Register16_Number16;
                    break;
                case 65:
                    executeInstruction = Instruction_65_LD_Register16_Register16;
                    break;
                case 66:
                    executeInstruction = Instruction_66_LD_Register16_Address;
                    break;
                case 67:
                    executeInstruction = Instruction_67_LD_Address_Register;
                    break;
                case 68:
                    executeInstruction = Instruction_68_LD_Address_Register16;
                    break;
                case 69:
                    executeInstruction = Instruction_69_LD_Register16Address_Number8;
                    break;
                case 70:
                    executeInstruction = Instruction_70_LD_Register16Address_Register;
                    break;
                case 71:
                    executeInstruction = Instruction_71_LD_IndexedAddress_Number8;
                    break;
                case 72:
                    executeInstruction = Instruction_72_LD_IndexedAddress_Register;
                    break;
                case 73:
                    executeInstruction = Instruction_73_LDD;
                    break;
                case 74:
                    executeInstruction = Instruction_74_LDDR;
                    break;
                case 75:
                    executeInstruction = Instruction_75_LDI;
                    break;
                case 76:
                    executeInstruction = Instruction_76_LDIR;
                    break;
                case 77:
                    executeInstruction = Instruction_77_NEG;
                    break;
                case 78:
                    executeInstruction = Instruction_78_NOP;
                    break;
                case 79:
                    executeInstruction = Instruction_79_OR_Number8;
                    break;
                case 80:
                    executeInstruction = Instruction_80_OR_Register;
                    break;
                case 81:
                    executeInstruction = Instruction_81_OR_Register16Address;
                    break;
                case 82:
                    executeInstruction = Instruction_82_OR_IndexedAddress;
                    break;
                case 83:
                    executeInstruction = Instruction_83_OTDR;
                    break;
                case 84:
                    executeInstruction = Instruction_84_OTIR;
                    break;
                case 85:
                    executeInstruction = Instruction_85_OUT_IOPort_Register;
                    break;
                case 86:
                    executeInstruction = Instruction_86_OUT_RegisterIOPort;
                    break;
                case 87:
                    executeInstruction = Instruction_87_OUT_RegisterIOPort_Register;
                    break;
                case 88:
                    executeInstruction = Instruction_88_OUTD;
                    break;
                case 89:
                    executeInstruction = Instruction_89_OUTI;
                    break;
                case 90:
                    executeInstruction = Instruction_90_POP_Register16;
                    break;
                case 91:
                    executeInstruction = Instruction_91_PUSH_Register16;
                    break;
                case 92:
                    executeInstruction = Instruction_92_RES_Bit_Register;
                    break;
                case 93:
                    executeInstruction = Instruction_93_RES_Bit_Register16Address;
                    break;
                case 94:
                    executeInstruction = Instruction_94_RES_Bit_IndexedAddress;
                    break;
                case 95:
                    executeInstruction = Instruction_95_RES_Bit_IndexedAddress_Register;
                    break;
                case 96:
                    executeInstruction = Instruction_96_RET;
                    break;
                case 97:
                    executeInstruction = Instruction_97_RET_FlagCondition;
                    break;
                case 98:
                    executeInstruction = Instruction_98_RETI;
                    break;
                case 99:
                    executeInstruction = Instruction_99_RETN;
                    break;
                case 100:
                    executeInstruction = Instruction_100_RL_Register;
                    break;
                case 101:
                    executeInstruction = Instruction_101_RL_Register16Address;
                    break;
                case 102:
                    executeInstruction = Instruction_102_RL_IndexedAddress;
                    break;
                case 103:
                    executeInstruction = Instruction_103_RL_IndexedAddress_Register;
                    break;
                case 104:
                    executeInstruction = Instruction_104_RLA;
                    break;
                case 105:
                    executeInstruction = Instruction_105_RLC_Register;
                    break;
                case 106:
                    executeInstruction = Instruction_106_RLC_Register16Address;
                    break;
                case 107:
                    executeInstruction = Instruction_107_RLC_IndexedAddress;
                    break;
                case 108:
                    executeInstruction = Instruction_108_RLC_IndexedAddress_Register;
                    break;
                case 109:
                    executeInstruction = Instruction_109_RLCA;
                    break;
                case 110:
                    executeInstruction = Instruction_110_RLD;
                    break;
                case 111:
                    executeInstruction = Instruction_111_RR_Register;
                    break;
                case 112:
                    executeInstruction = Instruction_112_RR_Register16Address;
                    break;
                case 113:
                    executeInstruction = Instruction_113_RR_IndexedAddress;
                    break;
                case 114:
                    executeInstruction = Instruction_114_RR_IndexedAddress_Register;
                    break;
                case 115:
                    executeInstruction = Instruction_115_RRA;
                    break;
                case 116:
                    executeInstruction = Instruction_116_RRC_Register;
                    break;
                case 117:
                    executeInstruction = Instruction_117_RRC_Register16Address;
                    break;
                case 118:
                    executeInstruction = Instruction_118_RRC_IndexedAddress;
                    break;
                case 119:
                    executeInstruction = Instruction_119_RRC_IndexedAddress_Register;
                    break;
                case 120:
                    executeInstruction = Instruction_120_RRCA;
                    break;
                case 121:
                    executeInstruction = Instruction_121_RRD;
                    break;
                case 122:
                    executeInstruction = Instruction_122_RST_ResetAddress;
                    break;
                case 123:
                    executeInstruction = Instruction_123_SBC_Register_Number8;
                    break;
                case 124:
                    executeInstruction = Instruction_124_SBC_Register_Register;
                    break;
                case 125:
                    executeInstruction = Instruction_125_SBC_Register_Register16Address;
                    break;
                case 126:
                    executeInstruction = Instruction_126_SBC_Register_IndexedAddress;
                    break;
                case 127:
                    executeInstruction = Instruction_127_SBC_Register16_Register16;
                    break;
                case 128:
                    executeInstruction = Instruction_128_SCF;
                    break;
                case 129:
                    executeInstruction = Instruction_129_SET_Bit_Register;
                    break;
                case 130:
                    executeInstruction = Instruction_130_SET_Bit_Register16Address;
                    break;
                case 131:
                    executeInstruction = Instruction_131_SET_Bit_IndexedAddress;
                    break;
                case 132:
                    executeInstruction = Instruction_132_SET_Bit_IndexedAddress_Register;
                    break;
                case 133:
                    executeInstruction = Instruction_133_SLA_Register;
                    break;
                case 134:
                    executeInstruction = Instruction_134_SLA_Register16Address;
                    break;
                case 135:
                    executeInstruction = Instruction_135_SLA_IndexedAddress;
                    break;
                case 136:
                    executeInstruction = Instruction_136_SLA_IndexedAddress_Register;
                    break;
                case 137:
                    executeInstruction = Instruction_137_SLL_Register;
                    break;
                case 138:
                    executeInstruction = Instruction_138_SLL_Register16Address;
                    break;
                case 139:
                    executeInstruction = Instruction_139_SLL_IndexedAddress;
                    break;
                case 140:
                    executeInstruction = Instruction_140_SLL_IndexedAddress_Register;
                    break;
                case 141:
                    executeInstruction = Instruction_141_SRA_Register;
                    break;
                case 142:
                    executeInstruction = Instruction_142_SRA_Register16Address;
                    break;
                case 143:
                    executeInstruction = Instruction_143_SRA_IndexedAddress;
                    break;
                case 144:
                    executeInstruction = Instruction_144_SRA_IndexedAddress_Register;
                    break;
                case 145:
                    executeInstruction = Instruction_145_SRL_Register;
                    break;
                case 146:
                    executeInstruction = Instruction_146_SRL_Register16Address;
                    break;
                case 147:
                    executeInstruction = Instruction_147_SRL_IndexedAddress;
                    break;
                case 148:
                    executeInstruction = Instruction_148_SRL_IndexedAddress_Register;
                    break;
                case 149:
                    executeInstruction = Instruction_149_SUB_Number8;
                    break;
                case 150:
                    executeInstruction = Instruction_150_SUB_Register;
                    break;
                case 151:
                    executeInstruction = Instruction_151_SUB_Register16Address;
                    break;
                case 152:
                    executeInstruction = Instruction_152_SUB_IndexedAddress;
                    break;
                case 153:
                    executeInstruction = Instruction_153_XOR_Number8;
                    break;
                case 154:
                    executeInstruction = Instruction_154_XOR_Register;
                    break;
                case 155:
                    executeInstruction = Instruction_155_XOR_Register16Address;
                    break;
                case 156:
                    executeInstruction = Instruction_156_XOR_IndexedAddress;
                    break;
                case 157:
                    executeInstruction = Instruction_157_NMI;
                    break;
                case 158:
                    executeInstruction = Instruction_158_INT_0;
                    break;
                case 159:
                    executeInstruction = Instruction_159_INT_1;
                    break;
                case 160:
                    executeInstruction = Instruction_160_INT_2;
                    break;
                case 161:
                    executeInstruction = Instruction_161_RESET;
                    break;
                case 162:
                    executeInstruction = Instruction_162_FetchNewInstruction;
                    break;
            }
        }

        #endregion

        #region Select alternate execution timings

        private void SelectAlternateExecutionTimings_IfFlagIsNotSet(FlagCondition flagCondition)
        {
            if (!CheckFlagCondition(flagCondition))
            {
                currentInstruction.SelectAlternateExecutionTimings();
            }

            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.CPUControlSelectAltExecTimingsIfFlagIsNotSet, flagCondition));
            }            
        }

        private enum RegisterZeroCondition
        {
            B,
            BC,
            BCZF
        }

        private void SelectAlternateExecutionTimings_IfRegisterIsZero(RegisterZeroCondition regzCondition)
        {
            bool evalCondition = false;
            switch(regzCondition)
            {
                case RegisterZeroCondition.B:
                    evalCondition = RegisterB_CheckZero();
                    break;
                case RegisterZeroCondition.BC:
                    evalCondition = RegisterBC_CheckZero();
                    break;
                case RegisterZeroCondition.BCZF:
                    evalCondition = RegisterBC_CheckZero() || CheckFlagCondition(FlagCondition.Z);
                    break;
            }
            if (evalCondition)
            {
                currentInstruction.SelectAlternateExecutionTimings();
            }

            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.CPUControlSelectAltExecTimingsIfRegisterIsZero, regzCondition));
            }
        }

        #endregion
    }
}