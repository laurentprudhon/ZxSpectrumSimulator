using System.Collections.Generic;
using Z80Simulator.Instructions;
using Z80Simulator.Test.ALU;
using Z80Simulator.Test.Assembly;
using Z80Simulator.Test.CPU;
using Z80Simulator.Test.CPU.MachineCycles;
using Z80Simulator.Test.FlagsAndMemptr;
using Z80Simulator.Test.Instructions;
using Z80Simulator.Test.MicroInstructions;
using Z80Simulator.Test.Zexall;

namespace Z80Simulator.Test
{
    public class TestCollection
    {        
        public static void RunInstructionsDefinitionTests()
        {
            IList<InstructionCode> opcodesList = Z80OpCodesTests.CheckInstructionCodeSizeInBytes();
            Z80OpCodesTests.CheckInstructionCodeCount(opcodesList);
            Z80OpCodesTests.CheckInstructionCodeNameAndParameters(opcodesList);

            Z80InstructionTypesTests.CheckInstructionParametersList();
            Z80InstructionTypesTests.CheckInstructionMachineCycles();
            Z80InstructionTypesTests.CheckInstructionUndocumentedStatus(opcodesList);
        }

        public static void RunAssemblyTests()
        {
            AssemblerTests.CheckProgram("basmul");
            AssemblerTests.CheckProgram("zx80");
            AssemblerTests.CheckProgram("zx81");
            AssemblerTests.CheckProgram("zxspectrum");
            AssemblerTests.CheckProgram("zxSpectrum128_ROM0");
            AssemblerTests.CheckProgram("zxSpectrum128_ROM1");
           
            DisassemblerTests.CheckMemoryDump("basmul");
            DisassemblerTests.CheckMemoryDump("basprint");
        }

        public static void RunMachineCyclesTests()
        {
            // Check execution timings of all machine cycles
            CPUMachineCyclesTests.Check_FetchOpcode(false);
            CPUMachineCyclesTests.Check_MemoryRead(false);
            CPUMachineCyclesTests.Check_MemoryWrite(false);
            CPUMachineCyclesTests.Check_StackMemoryReadAndWrite();
            CPUMachineCyclesTests.Check_Input(false);
            CPUMachineCyclesTests.Check_Output(false);

            // Check execution timings with slow memory and devices and extra WAIT states
            CPUMachineCyclesTests.Check_FetchOpcode(true);
            CPUMachineCyclesTests.Check_MemoryRead(true);
            CPUMachineCyclesTests.Check_MemoryWrite(true);
            CPUMachineCyclesTests.Check_Input(true);
            CPUMachineCyclesTests.Check_Output(true);

            // Check external CPU control PINs acknowledge cycles
            CPUMachineCyclesTests.Check_BusRequestAcknowledge();
            CPUMachineCyclesTests.Check_InterruptRequestAcknowledge();

            // Check micro instructions logging
            CPUMachineCyclesTests.Check_MicroInstructions();
                        
            // Check instructions decoder
            CPUMachineCyclesTests.Check_TwoBytesOpCodesAndDDFDPrefixes();
            CPUMachineCyclesTests.Check_FourBytesOpCodes();

            // Check simulation performance - disabled during development
            CPUMachineCyclesTests.Check_Speed();
        }

        public static void RunCPUTests()
        {
            // CPUControl

            CPUInstructionsTests.Check_Instruction_35_DI();
            CPUInstructionsTests.Check_Instruction_37_EI();
            CPUInstructionsTests.Check_Instruction_41_HALT();
            CPUInstructionsTests.Check_Instruction_42_IM_InterruptMode();
            CPUInstructionsTests.Check_Instruction_78_NOP();
            CPUInstructionsTests.Check_Instruction_157_NMI();
            CPUInstructionsTests.Check_Instruction_158_INT_0();
            CPUInstructionsTests.Check_Instruction_159_INT_1();
            CPUInstructionsTests.Check_Instruction_160_INT_2();
            CPUInstructionsTests.Check_Instruction_161_RESET();

            // Load_8Bit

            CPUInstructionsTests.Check_Instruction_59_LD_Register_Number8();
            CPUInstructionsTests.Check_Instruction_60_LD_Register_Register();
            CPUInstructionsTests.Check_Instruction_61_LD_Register_Address();
            CPUInstructionsTests.Check_Instruction_62_LD_Register_Register16Address();
            CPUInstructionsTests.Check_Instruction_63_LD_Register_IndexedAddress();
            CPUInstructionsTests.Check_Instruction_67_LD_Address_Register();
            CPUInstructionsTests.Check_Instruction_69_LD_Register16Address_Number8();
            CPUInstructionsTests.Check_Instruction_70_LD_Register16Address_Register();
            CPUInstructionsTests.Check_Instruction_71_LD_IndexedAddress_Number8();
            CPUInstructionsTests.Check_Instruction_72_LD_IndexedAddress_Register();

            // Load16Bit

            CPUInstructionsTests.Check_Instruction_64_LD_Register16_Number16();
            CPUInstructionsTests.Check_Instruction_65_LD_Register16_Register16();
            CPUInstructionsTests.Check_Instruction_66_LD_Register16_Address();
            CPUInstructionsTests.Check_Instruction_68_LD_Address_Register16();

            CPUInstructionsTests.Check_Instruction_90_POP_Register16();
            CPUInstructionsTests.Check_Instruction_91_PUSH_Register16();

            // InputAndOutput

            CPUInstructionsTests.Check_Instruction_43_IN_Register_IOPort();
            CPUInstructionsTests.Check_Instruction_44_IN_Register_RegisterIOPort();
            CPUInstructionsTests.Check_Instruction_45_IN_Flags_RegisterIOPort();

            CPUInstructionsTests.Check_Instruction_50_IND();
            CPUInstructionsTests.Check_Instruction_51_INDR();
            CPUInstructionsTests.Check_Instruction_52_INI();
            CPUInstructionsTests.Check_Instruction_53_INIR();
            
            CPUInstructionsTests.Check_Instruction_83_OTDR();
            CPUInstructionsTests.Check_Instruction_84_OTIR();
            
            CPUInstructionsTests.Check_Instruction_85_OUT_IOPort_Register();
            CPUInstructionsTests.Check_Instruction_86_OUT_RegisterIOPort();
            CPUInstructionsTests.Check_Instruction_87_OUT_RegisterIOPort_Register();
            
            CPUInstructionsTests.Check_Instruction_88_OUTD();
            CPUInstructionsTests.Check_Instruction_89_OUTI();

            // Jump

            CPUInstructionsTests.Check_Instruction_36_DJNZ_RelativeDisplacement();
            CPUInstructionsTests.Check_Instruction_54_JP_Address();
            CPUInstructionsTests.Check_Instruction_55_JP_Register16Address();
            CPUInstructionsTests.Check_Instruction_56_JP_FlagCondition_Address();
            CPUInstructionsTests.Check_Instruction_57_JR_RelativeDisplacement();
            CPUInstructionsTests.Check_Instruction_58_JR_FlagCondition_RelativeDisplacement();

            // CallAndReturn

            CPUInstructionsTests.Check_Instruction_18_CALL_Address();
            CPUInstructionsTests.Check_Instruction_19_CALL_FlagCondition_Address();
            CPUInstructionsTests.Check_Instruction_96_RET();
            CPUInstructionsTests.Check_Instruction_97_RET_FlagCondition();
            CPUInstructionsTests.Check_Instruction_98_RETI();
            CPUInstructionsTests.Check_Instruction_99_RETN();
            CPUInstructionsTests.Check_Instruction_122_RST_ResetAddress();

            // ExchangeBlockTransferAndSearch

            CPUInstructionsTests.Check_Instruction_25_CPD();
            CPUInstructionsTests.Check_Instruction_26_CPDR();
            CPUInstructionsTests.Check_Instruction_27_CPI();
            CPUInstructionsTests.Check_Instruction_28_CPIR();

            CPUInstructionsTests.Check_Instruction_38_EX_Register16_Register16();
            CPUInstructionsTests.Check_Instruction_39_EX_Register16Address_Register16();
            CPUInstructionsTests.Check_Instruction_40_EXX();

            CPUInstructionsTests.Check_Instruction_73_LDD();
            CPUInstructionsTests.Check_Instruction_74_LDDR();
            CPUInstructionsTests.Check_Instruction_75_LDI();
            CPUInstructionsTests.Check_Instruction_76_LDIR();
        }

        public static void RunALUTests()
        {
            // Arithmetic_8bits

            ALUInstructionsTests.Check_Instruction_1_ADC_Register_Number8();
            ALUInstructionsTests.Check_Instruction_2_ADC_Register_Register();
            ALUInstructionsTests.Check_Instruction_3_ADC_Register_Register16Address();
            ALUInstructionsTests.Check_Instruction_4_ADC_Register_IndexedAddress();
            
            ALUInstructionsTests.Check_Instruction_6_ADD_Register_Number8();
            ALUInstructionsTests.Check_Instruction_7_ADD_Register_Register();
            ALUInstructionsTests.Check_Instruction_8_ADD_Register_Register16Address();
            ALUInstructionsTests.Check_Instruction_9_ADD_Register_IndexedAddress();

            ALUInstructionsTests.Check_Instruction_11_AND_Number8();
            ALUInstructionsTests.Check_Instruction_12_AND_Register();
            ALUInstructionsTests.Check_Instruction_13_AND_Register16Address();
            ALUInstructionsTests.Check_Instruction_14_AND_IndexedAddress();

            ALUInstructionsTests.Check_Instruction_21_CP_Number8();
            ALUInstructionsTests.Check_Instruction_22_CP_Register();
            ALUInstructionsTests.Check_Instruction_23_CP_Register16Address();
            ALUInstructionsTests.Check_Instruction_24_CP_IndexedAddress();

            ALUInstructionsTests.Check_Instruction_31_DEC_Register();
            ALUInstructionsTests.Check_Instruction_32_DEC_Register16Address();
            ALUInstructionsTests.Check_Instruction_33_DEC_IndexedAddress();
           
            ALUInstructionsTests.Check_Instruction_46_INC_Register();
            ALUInstructionsTests.Check_Instruction_47_INC_Register16Address();
            ALUInstructionsTests.Check_Instruction_48_INC_IndexedAddress();
            
            ALUInstructionsTests.Check_Instruction_79_OR_Number8();
            ALUInstructionsTests.Check_Instruction_80_OR_Register();
            ALUInstructionsTests.Check_Instruction_81_OR_Register16Address();
            ALUInstructionsTests.Check_Instruction_82_OR_IndexedAddress();

            ALUInstructionsTests.Check_Instruction_123_SBC_Register_Number8();
            ALUInstructionsTests.Check_Instruction_124_SBC_Register_Register();
            ALUInstructionsTests.Check_Instruction_125_SBC_Register_Register16Address();
            ALUInstructionsTests.Check_Instruction_126_SBC_Register_IndexedAddress();

            ALUInstructionsTests.Check_Instruction_149_SUB_Number8();
            ALUInstructionsTests.Check_Instruction_150_SUB_Register();
            ALUInstructionsTests.Check_Instruction_151_SUB_Register16Address();
            ALUInstructionsTests.Check_Instruction_152_SUB_IndexedAddress();

            ALUInstructionsTests.Check_Instruction_153_XOR_Number8();
            ALUInstructionsTests.Check_Instruction_154_XOR_Register();
            ALUInstructionsTests.Check_Instruction_155_XOR_Register16Address();
            ALUInstructionsTests.Check_Instruction_156_XOR_IndexedAddress();

            // Arithmetic16bits

            ALUInstructionsTests.Check_Instruction_5_ADC_Register16_Register16() ;
            ALUInstructionsTests.Check_Instruction_10_ADD_Register16_Register16();
            ALUInstructionsTests.Check_Instruction_34_DEC_Register16() ;
            ALUInstructionsTests.Check_Instruction_49_INC_Register16();
            ALUInstructionsTests.Check_Instruction_127_SBC_Register16_Register16();

            // ArithmeticGeneralPurpose

            ALUInstructionsTests.Check_Instruction_20_CCF();
            ALUInstructionsTests.Check_Instruction_29_CPL();
            ALUInstructionsTests.Check_Instruction_30_DAA();
            ALUInstructionsTests.Check_Instruction_77_NEG();
            ALUInstructionsTests.Check_Instruction_128_SCF();

            // BitSetResetAndTest

            ALUInstructionsTests.Check_Instruction_15_BIT_Bit_Register();
            ALUInstructionsTests.Check_Instruction_16_BIT_Bit_Register16Address();
            ALUInstructionsTests.Check_Instruction_17_BIT_Bit_IndexedAddress();

            ALUInstructionsTests.Check_Instruction_92_RES_Bit_Register();
            ALUInstructionsTests.Check_Instruction_93_RES_Bit_Register16Address();
            ALUInstructionsTests.Check_Instruction_94_RES_Bit_IndexedAddress();
            ALUInstructionsTests.Check_Instruction_95_RES_Bit_IndexedAddress_Register();

           ALUInstructionsTests.Check_Instruction_129_SET_Bit_Register();
           ALUInstructionsTests.Check_Instruction_130_SET_Bit_Register16Address();
           ALUInstructionsTests.Check_Instruction_131_SET_Bit_IndexedAddress();
           ALUInstructionsTests.Check_Instruction_132_SET_Bit_IndexedAddress_Register();

            // RotateAndShift

           ALUInstructionsTests.Check_Instruction_100_RL_Register();
           ALUInstructionsTests.Check_Instruction_101_RL_Register16Address();
           ALUInstructionsTests.Check_Instruction_102_RL_IndexedAddress();
           ALUInstructionsTests.Check_Instruction_103_RL_IndexedAddress_Register();

           ALUInstructionsTests.Check_Instruction_104_RLA();

           ALUInstructionsTests.Check_Instruction_105_RLC_Register();
           ALUInstructionsTests.Check_Instruction_106_RLC_Register16Address();
           ALUInstructionsTests.Check_Instruction_107_RLC_IndexedAddress();
           ALUInstructionsTests.Check_Instruction_108_RLC_IndexedAddress_Register();

           ALUInstructionsTests.Check_Instruction_109_RLCA();

           ALUInstructionsTests.Check_Instruction_110_RLD();

           ALUInstructionsTests.Check_Instruction_111_RR_Register();
           ALUInstructionsTests.Check_Instruction_112_RR_Register16Address();
           ALUInstructionsTests.Check_Instruction_113_RR_IndexedAddress();
           ALUInstructionsTests.Check_Instruction_114_RR_IndexedAddress_Register();

           ALUInstructionsTests.Check_Instruction_115_RRA();

           ALUInstructionsTests.Check_Instruction_116_RRC_Register();
           ALUInstructionsTests.Check_Instruction_117_RRC_Register16Address();
           ALUInstructionsTests.Check_Instruction_118_RRC_IndexedAddress();
           ALUInstructionsTests.Check_Instruction_119_RRC_IndexedAddress_Register();

           ALUInstructionsTests.Check_Instruction_120_RRCA();

           ALUInstructionsTests.Check_Instruction_121_RRD();
                
           ALUInstructionsTests.Check_Instruction_133_SLA_Register();
           ALUInstructionsTests.Check_Instruction_134_SLA_Register16Address();
           ALUInstructionsTests.Check_Instruction_135_SLA_IndexedAddress();
           ALUInstructionsTests.Check_Instruction_136_SLA_IndexedAddress_Register();

           ALUInstructionsTests.Check_Instruction_137_SLL_Register();
           ALUInstructionsTests.Check_Instruction_138_SLL_Register16Address();
           ALUInstructionsTests.Check_Instruction_139_SLL_IndexedAddress();
           ALUInstructionsTests.Check_Instruction_140_SLL_IndexedAddress_Register();

           ALUInstructionsTests.Check_Instruction_141_SRA_Register();
           ALUInstructionsTests.Check_Instruction_142_SRA_Register16Address();
           ALUInstructionsTests.Check_Instruction_143_SRA_IndexedAddress();
           ALUInstructionsTests.Check_Instruction_144_SRA_IndexedAddress_Register();

           ALUInstructionsTests.Check_Instruction_145_SRL_Register();
           ALUInstructionsTests.Check_Instruction_146_SRL_Register16Address();
           ALUInstructionsTests.Check_Instruction_147_SRL_IndexedAddress();
           ALUInstructionsTests.Check_Instruction_148_SRL_IndexedAddress_Register();
        }

        public static void RunMicroInstructionsTests()
        {
            InstructionSetGenerator.GenerateProgramToExecuteAllInstructions();
        }

        public static void RunFullInstructionSetTests()
        {
            // ERROR : ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_SCFOP_ADDR, ZexallTests.ZEXALL2_TEST_SCFOP_DESC);
            // ERROR : ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_CCFOP_ADDR, ZexallTests.ZEXALL2_TEST_CCFOP_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_SCFCCF_ADDR, ZexallTests.ZEXALL2_TEST_SCFCCF_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_CCFSCF_ADDR, ZexallTests.ZEXALL2_TEST_CCFSCF_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_BITA_ADDR, ZexallTests.ZEXALL2_TEST_BITA_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_BITHL_ADDR, ZexallTests.ZEXALL2_TEST_BITHL_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_BITX_ADDR, ZexallTests.ZEXALL2_TEST_BITX_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_BITZ80_ADDR, ZexallTests.ZEXALL2_TEST_BITZ80_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_DAAOP_ADDR, ZexallTests.ZEXALL2_TEST_DAAOP_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_CPLOP_ADDR, ZexallTests.ZEXALL2_TEST_CPLOP_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_ADC16_ADDR, ZexallTests.ZEXALL2_TEST_ADC16_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_ADD16_ADDR, ZexallTests.ZEXALL2_TEST_ADD16_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_ADD16X_ADDR, ZexallTests.ZEXALL2_TEST_ADD16X_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_ADD16Y_ADDR, ZexallTests.ZEXALL2_TEST_ADD16Y_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_ALU8I_ADDR, ZexallTests.ZEXALL2_TEST_ALU8I_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_ALU8R_ADDR, ZexallTests.ZEXALL2_TEST_ALU8R_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_ALU8RX_ADDR, ZexallTests.ZEXALL2_TEST_ALU8RX_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_ALU8X_ADDR, ZexallTests.ZEXALL2_TEST_ALU8X_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_CPD1_ADDR, ZexallTests.ZEXALL2_TEST_CPD1_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_CPI1_ADDR, ZexallTests.ZEXALL2_TEST_CPI1_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_INCA_ADDR, ZexallTests.ZEXALL2_TEST_INCA_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_INCB_ADDR, ZexallTests.ZEXALL2_TEST_INCB_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_INCBC_ADDR, ZexallTests.ZEXALL2_TEST_INCBC_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_INCC_ADDR, ZexallTests.ZEXALL2_TEST_INCC_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_INCD_ADDR, ZexallTests.ZEXALL2_TEST_INCD_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_INCDE_ADDR, ZexallTests.ZEXALL2_TEST_INCDE_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_INCE_ADDR, ZexallTests.ZEXALL2_TEST_INCE_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_INCH_ADDR, ZexallTests.ZEXALL2_TEST_INCH_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_INCHL_ADDR, ZexallTests.ZEXALL2_TEST_INCHL_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_INCIX_ADDR, ZexallTests.ZEXALL2_TEST_INCIX_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_INCIY_ADDR, ZexallTests.ZEXALL2_TEST_INCIY_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_INCL_ADDR, ZexallTests.ZEXALL2_TEST_INCL_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_INCM_ADDR, ZexallTests.ZEXALL2_TEST_INCM_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_INCSP_ADDR, ZexallTests.ZEXALL2_TEST_INCSP_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_INCX_ADDR, ZexallTests.ZEXALL2_TEST_INCX_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_INCXH_ADDR, ZexallTests.ZEXALL2_TEST_INCXH_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_INCXL_ADDR, ZexallTests.ZEXALL2_TEST_INCXL_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_INCYH_ADDR, ZexallTests.ZEXALL2_TEST_INCYH_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_INCYL_ADDR, ZexallTests.ZEXALL2_TEST_INCYL_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_LD161_ADDR, ZexallTests.ZEXALL2_TEST_LD161_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_LD162_ADDR, ZexallTests.ZEXALL2_TEST_LD162_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_LD163_ADDR, ZexallTests.ZEXALL2_TEST_LD163_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_LD164_ADDR, ZexallTests.ZEXALL2_TEST_LD164_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_LD165_ADDR, ZexallTests.ZEXALL2_TEST_LD165_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_LD166_ADDR, ZexallTests.ZEXALL2_TEST_LD166_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_LD167_ADDR, ZexallTests.ZEXALL2_TEST_LD167_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_LD168_ADDR, ZexallTests.ZEXALL2_TEST_LD168_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_LD16IM_ADDR, ZexallTests.ZEXALL2_TEST_LD16IM_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_LD16IX_ADDR, ZexallTests.ZEXALL2_TEST_LD16IX_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_LD8BD_ADDR, ZexallTests.ZEXALL2_TEST_LD8BD_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_LD8IM_ADDR, ZexallTests.ZEXALL2_TEST_LD8IM_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_LD8IMX_ADDR, ZexallTests.ZEXALL2_TEST_LD8IMX_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_LD8IX1_ADDR, ZexallTests.ZEXALL2_TEST_LD8IX1_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_LD8IX2_ADDR, ZexallTests.ZEXALL2_TEST_LD8IX2_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_LD8IX3_ADDR, ZexallTests.ZEXALL2_TEST_LD8IX3_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_LD8IXY_ADDR, ZexallTests.ZEXALL2_TEST_LD8IXY_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_LD8RR_ADDR, ZexallTests.ZEXALL2_TEST_LD8RR_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_LD8RRX_ADDR, ZexallTests.ZEXALL2_TEST_LD8RRX_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_LDA_ADDR, ZexallTests.ZEXALL2_TEST_LDA_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_LDD1_ADDR, ZexallTests.ZEXALL2_TEST_LDD1_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_LDD2_ADDR, ZexallTests.ZEXALL2_TEST_LDD2_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_LDI1_ADDR, ZexallTests.ZEXALL2_TEST_LDI1_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_LDI2_ADDR, ZexallTests.ZEXALL2_TEST_LDI2_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_NEGOP_ADDR, ZexallTests.ZEXALL2_TEST_NEGOP_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_RLDOP_ADDR, ZexallTests.ZEXALL2_TEST_RLDOP_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_ROT8080_ADDR, ZexallTests.ZEXALL2_TEST_ROT8080_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_ROTXY_ADDR, ZexallTests.ZEXALL2_TEST_ROTXY_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_ROTZ80_ADDR, ZexallTests.ZEXALL2_TEST_ROTZ80_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_SRZ80_ADDR, ZexallTests.ZEXALL2_TEST_SRZ80_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_SRZX_ADDR, ZexallTests.ZEXALL2_TEST_SRZX_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_ST8IX1_ADDR, ZexallTests.ZEXALL2_TEST_ST8IX1_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_ST8IX2_ADDR, ZexallTests.ZEXALL2_TEST_ST8IX2_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_ST8IX3_ADDR, ZexallTests.ZEXALL2_TEST_ST8IX3_DESC);
            ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_STABD_ADDR, ZexallTests.ZEXALL2_TEST_STABD_DESC);
        }

        public static void RunFlagsAndMemptrTests()
        {
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_01_ADDR, FlagsAndMemptrTests.FLAGS_TEST_01_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_02_ADDR, FlagsAndMemptrTests.FLAGS_TEST_02_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_03_ADDR, FlagsAndMemptrTests.FLAGS_TEST_03_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_04_ADDR, FlagsAndMemptrTests.FLAGS_TEST_04_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_05_ADDR, FlagsAndMemptrTests.FLAGS_TEST_05_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_06_ADDR, FlagsAndMemptrTests.FLAGS_TEST_06_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_07_ADDR, FlagsAndMemptrTests.FLAGS_TEST_07_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_08_ADDR, FlagsAndMemptrTests.FLAGS_TEST_08_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_09_ADDR, FlagsAndMemptrTests.FLAGS_TEST_09_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_10_ADDR, FlagsAndMemptrTests.FLAGS_TEST_10_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_11_ADDR, FlagsAndMemptrTests.FLAGS_TEST_11_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_12_ADDR, FlagsAndMemptrTests.FLAGS_TEST_12_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_13_ADDR, FlagsAndMemptrTests.FLAGS_TEST_13_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_14_ADDR, FlagsAndMemptrTests.FLAGS_TEST_14_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_15_ADDR, FlagsAndMemptrTests.FLAGS_TEST_15_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_16_ADDR, FlagsAndMemptrTests.FLAGS_TEST_16_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_17_ADDR, FlagsAndMemptrTests.FLAGS_TEST_17_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_18_ADDR, FlagsAndMemptrTests.FLAGS_TEST_18_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_19_ADDR, FlagsAndMemptrTests.FLAGS_TEST_19_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_20_ADDR, FlagsAndMemptrTests.FLAGS_TEST_20_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_21_ADDR, FlagsAndMemptrTests.FLAGS_TEST_21_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_22_ADDR, FlagsAndMemptrTests.FLAGS_TEST_22_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_23_ADDR, FlagsAndMemptrTests.FLAGS_TEST_23_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_24_ADDR, FlagsAndMemptrTests.FLAGS_TEST_24_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_25_ADDR, FlagsAndMemptrTests.FLAGS_TEST_25_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_26_ADDR, FlagsAndMemptrTests.FLAGS_TEST_26_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_27_ADDR, FlagsAndMemptrTests.FLAGS_TEST_27_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_28_ADDR, FlagsAndMemptrTests.FLAGS_TEST_28_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_29_ADDR, FlagsAndMemptrTests.FLAGS_TEST_29_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_30_ADDR, FlagsAndMemptrTests.FLAGS_TEST_30_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_31_ADDR, FlagsAndMemptrTests.FLAGS_TEST_31_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_32_ADDR, FlagsAndMemptrTests.FLAGS_TEST_32_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_33_ADDR, FlagsAndMemptrTests.FLAGS_TEST_33_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_34_ADDR, FlagsAndMemptrTests.FLAGS_TEST_34_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_35_ADDR, FlagsAndMemptrTests.FLAGS_TEST_35_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_36_ADDR, FlagsAndMemptrTests.FLAGS_TEST_36_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_37_ADDR, FlagsAndMemptrTests.FLAGS_TEST_37_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_38_ADDR, FlagsAndMemptrTests.FLAGS_TEST_38_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_39_ADDR, FlagsAndMemptrTests.FLAGS_TEST_39_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_40_ADDR, FlagsAndMemptrTests.FLAGS_TEST_40_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_41_ADDR, FlagsAndMemptrTests.FLAGS_TEST_41_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_42_ADDR, FlagsAndMemptrTests.FLAGS_TEST_42_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_43_ADDR, FlagsAndMemptrTests.FLAGS_TEST_43_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_44_ADDR, FlagsAndMemptrTests.FLAGS_TEST_44_DESC);
            FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_45_ADDR, FlagsAndMemptrTests.FLAGS_TEST_45_DESC);

            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_01_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_01_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_02_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_02_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_03_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_03_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_04_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_04_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_05_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_05_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_06_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_06_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_07_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_07_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_08_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_08_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_09_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_09_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_10_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_10_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_11_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_11_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_12_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_12_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_13_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_13_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_14_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_14_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_15_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_15_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_16_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_16_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_17_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_17_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_18_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_18_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_19_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_19_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_20_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_20_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_21_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_21_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_22_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_22_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_23_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_23_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_24_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_24_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_25_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_25_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_26_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_26_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_27_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_27_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_28_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_28_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_29_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_29_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_30_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_30_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_31_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_31_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_32_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_32_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_33_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_33_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_34_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_34_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_35_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_35_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_36_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_36_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_37_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_37_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_38_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_38_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_39_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_39_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_40_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_40_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_41_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_41_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_42_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_42_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_43_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_43_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_44_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_44_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_45_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_45_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_46_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_46_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_47_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_47_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_48_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_48_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_49_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_49_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_50_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_50_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_51_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_51_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_52_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_52_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_53_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_53_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_54_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_54_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_55_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_55_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_56_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_56_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_57_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_57_DESC);
            FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_58_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_58_DESC);
        }
    }
}