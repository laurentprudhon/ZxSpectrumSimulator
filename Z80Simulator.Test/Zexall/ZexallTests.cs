using System;
using System.Linq;
using System.IO;
using System.Text;
using Z80Simulator.Assembly;
using Z80Simulator.CPU;
using Z80Simulator.Test.CPU;
using Z80Simulator.Test.System;
using Z80Simulator.Instructions;

namespace Z80Simulator.Test.Zexall
{
    public class ZexallTests
    {
        public static int ZEXALL2_TEST_SCFOP_ADDR = 0x0010;
        public static string ZEXALL2_TEST_SCFOP_DESC = "scf";
        public static int ZEXALL2_TEST_CCFOP_ADDR = 0x006F;
        public static string ZEXALL2_TEST_CCFOP_DESC = "ccf";
        public static int ZEXALL2_TEST_SCFCCF_ADDR = 0x00CE;
        public static string ZEXALL2_TEST_SCFCCF_DESC = "scf+ccf";
        public static int ZEXALL2_TEST_CCFSCF_ADDR = 0x012D;
        public static string ZEXALL2_TEST_CCFSCF_DESC = "ccf+scf";
        public static int ZEXALL2_TEST_BITA_ADDR = 0x0484;
        public static string ZEXALL2_TEST_BITA_DESC = "bit n,a";
        public static int ZEXALL2_TEST_BITHL_ADDR = 0x04E3;
        public static string ZEXALL2_TEST_BITHL_DESC = "bit n,(hl)";
        public static int ZEXALL2_TEST_BITX_ADDR = 0x0542;
        public static string ZEXALL2_TEST_BITX_DESC = "bit n,(<ix,iy>+1)";
        public static int ZEXALL2_TEST_BITZ80_ADDR = 0x05A1;
        public static string ZEXALL2_TEST_BITZ80_DESC = "bit n,<b,c,d,e,h,l,(hl),a>";
        public static int ZEXALL2_TEST_DAAOP_ADDR = 0x06BE;
        public static string ZEXALL2_TEST_DAAOP_DESC = "daa";
        public static int ZEXALL2_TEST_CPLOP_ADDR = 0x071D;
        public static string ZEXALL2_TEST_CPLOP_DESC = "cpl";
        public static int ZEXALL2_TEST_ADC16_ADDR = 0x018C;
        public static string ZEXALL2_TEST_ADC16_DESC = "<adc,sbc> hl,<bc,de,hl,sp>";
        public static int ZEXALL2_TEST_ADD16_ADDR = 0x01EB;
        public static string ZEXALL2_TEST_ADD16_DESC = "add hl,<bc,de,hl,sp>";
        public static int ZEXALL2_TEST_ADD16X_ADDR = 0x024A;
        public static string ZEXALL2_TEST_ADD16X_DESC = "add ix,<bc,de,ix,sp>";
        public static int ZEXALL2_TEST_ADD16Y_ADDR = 0x02A9;
        public static string ZEXALL2_TEST_ADD16Y_DESC = "add iy,<bc,de,iy,sp>";
        public static int ZEXALL2_TEST_ALU8I_ADDR = 0x0308;
        public static string ZEXALL2_TEST_ALU8I_DESC = "aluop a,nn";
        public static int ZEXALL2_TEST_ALU8R_ADDR = 0x0367;
        public static string ZEXALL2_TEST_ALU8R_DESC = "aluop a,<b,c,d,e,h,l,(hl),a>";
        public static int ZEXALL2_TEST_ALU8RX_ADDR = 0x03C6;
        public static string ZEXALL2_TEST_ALU8RX_DESC = "aluop a,<ixh,ixl,iyh,iyl>";
        public static int ZEXALL2_TEST_ALU8X_ADDR = 0x0425;
        public static string ZEXALL2_TEST_ALU8X_DESC = "aluop a,(<ix,iy>+1)";
        public static int ZEXALL2_TEST_CPD1_ADDR = 0x0600;
        public static string ZEXALL2_TEST_CPD1_DESC = "cpd<r>";
        public static int ZEXALL2_TEST_CPI1_ADDR = 0x065F;
        public static string ZEXALL2_TEST_CPI1_DESC = "cpi<r>";
        public static int ZEXALL2_TEST_INCA_ADDR = 0x077C;
        public static string ZEXALL2_TEST_INCA_DESC = "<inc,dec> a";
        public static int ZEXALL2_TEST_INCB_ADDR = 0x07DB;
        public static string ZEXALL2_TEST_INCB_DESC = "<inc,dec> b";
        public static int ZEXALL2_TEST_INCBC_ADDR = 0x083A;
        public static string ZEXALL2_TEST_INCBC_DESC = "<inc,dec> bc";
        public static int ZEXALL2_TEST_INCC_ADDR = 0x0899;
        public static string ZEXALL2_TEST_INCC_DESC = "<inc,dec> c";
        public static int ZEXALL2_TEST_INCD_ADDR = 0x08F8;
        public static string ZEXALL2_TEST_INCD_DESC = "<inc,dec> d";
        public static int ZEXALL2_TEST_INCDE_ADDR = 0x0957;
        public static string ZEXALL2_TEST_INCDE_DESC = "<inc,dec> de";
        public static int ZEXALL2_TEST_INCE_ADDR = 0x09B6;
        public static string ZEXALL2_TEST_INCE_DESC = "<inc,dec> e";
        public static int ZEXALL2_TEST_INCH_ADDR = 0x0A15;
        public static string ZEXALL2_TEST_INCH_DESC = "<inc,dec> h";
        public static int ZEXALL2_TEST_INCHL_ADDR = 0x0A74;
        public static string ZEXALL2_TEST_INCHL_DESC = "<inc,dec> hl";
        public static int ZEXALL2_TEST_INCIX_ADDR = 0x0AD3;
        public static string ZEXALL2_TEST_INCIX_DESC = "<inc,dec> ix";
        public static int ZEXALL2_TEST_INCIY_ADDR = 0x0B32;
        public static string ZEXALL2_TEST_INCIY_DESC = "<inc,dec> iy";
        public static int ZEXALL2_TEST_INCL_ADDR = 0x0B91;
        public static string ZEXALL2_TEST_INCL_DESC = "<inc,dec> l";
        public static int ZEXALL2_TEST_INCM_ADDR = 0x0BF0;
        public static string ZEXALL2_TEST_INCM_DESC = "<inc,dec> (hl)";
        public static int ZEXALL2_TEST_INCSP_ADDR = 0x0C4F;
        public static string ZEXALL2_TEST_INCSP_DESC = "<inc,dec> sp";
        public static int ZEXALL2_TEST_INCX_ADDR = 0x0CAE;
        public static string ZEXALL2_TEST_INCX_DESC = "<inc,dec> (<ix,iy>+1)";
        public static int ZEXALL2_TEST_INCXH_ADDR = 0x0D0D;
        public static string ZEXALL2_TEST_INCXH_DESC = "<inc,dec> ixh";
        public static int ZEXALL2_TEST_INCXL_ADDR = 0x0D6C;
        public static string ZEXALL2_TEST_INCXL_DESC = "<inc,dec> ixl";
        public static int ZEXALL2_TEST_INCYH_ADDR = 0x0DCB;
        public static string ZEXALL2_TEST_INCYH_DESC = "<inc,dec> iyh";
        public static int ZEXALL2_TEST_INCYL_ADDR = 0x0E2A;
        public static string ZEXALL2_TEST_INCYL_DESC = "<inc,dec> iyl";
        public static int ZEXALL2_TEST_LD161_ADDR = 0x0E89;
        public static string ZEXALL2_TEST_LD161_DESC = "ld <bc,de>,(nnnn)";
        public static int ZEXALL2_TEST_LD162_ADDR = 0x0EE8;
        public static string ZEXALL2_TEST_LD162_DESC = "ld hl,(nnnn)";
        public static int ZEXALL2_TEST_LD163_ADDR = 0x0F47;
        public static string ZEXALL2_TEST_LD163_DESC = "ld sp,(nnnn)";
        public static int ZEXALL2_TEST_LD164_ADDR = 0x0FA6;
        public static string ZEXALL2_TEST_LD164_DESC = "ld <ix,iy>,(nnnn)";
        public static int ZEXALL2_TEST_LD165_ADDR = 0x1005;
        public static string ZEXALL2_TEST_LD165_DESC = "ld (nnnn),<bc,de>";
        public static int ZEXALL2_TEST_LD166_ADDR = 0x1064;
        public static string ZEXALL2_TEST_LD166_DESC = "ld (nnnn),hl";
        public static int ZEXALL2_TEST_LD167_ADDR = 0x10C3;
        public static string ZEXALL2_TEST_LD167_DESC = "ld (nnnn),sp";
        public static int ZEXALL2_TEST_LD168_ADDR = 0x1122;
        public static string ZEXALL2_TEST_LD168_DESC = "ld (nnnn),<ix,iy>";
        public static int ZEXALL2_TEST_LD16IM_ADDR = 0x1181;
        public static string ZEXALL2_TEST_LD16IM_DESC = "ld <bc,de,hl,sp>,nnnn";
        public static int ZEXALL2_TEST_LD16IX_ADDR = 0x11E0;
        public static string ZEXALL2_TEST_LD16IX_DESC = "ld <ix,iy>,nnnn";
        public static int ZEXALL2_TEST_LD8BD_ADDR = 0x123F;
        public static string ZEXALL2_TEST_LD8BD_DESC = "ld a,<(bc),(de)>";
        public static int ZEXALL2_TEST_LD8IM_ADDR = 0x129E;
        public static string ZEXALL2_TEST_LD8IM_DESC = "ld <b,c,d,e,h,l,(hl),a>,nn";
        public static int ZEXALL2_TEST_LD8IMX_ADDR = 0x12FD;
        public static string ZEXALL2_TEST_LD8IMX_DESC = "ld (<ix,iy>+1),nn";
        public static int ZEXALL2_TEST_LD8IX1_ADDR = 0x135C;
        public static string ZEXALL2_TEST_LD8IX1_DESC = "ld <b,c,d,e>,(<ix,iy>+1)";
        public static int ZEXALL2_TEST_LD8IX2_ADDR = 0x13BB;
        public static string ZEXALL2_TEST_LD8IX2_DESC = "ld <h,l>,(<ix,iy>+1)";
        public static int ZEXALL2_TEST_LD8IX3_ADDR = 0x141A;
        public static string ZEXALL2_TEST_LD8IX3_DESC = "ld a,(<ix,iy>+1)";
        public static int ZEXALL2_TEST_LD8IXY_ADDR = 0x1479;
        public static string ZEXALL2_TEST_LD8IXY_DESC = "ld <ixh,ixl,iyh,iyl>,nn";
        public static int ZEXALL2_TEST_LD8RR_ADDR = 0x14D8;
        public static string ZEXALL2_TEST_LD8RR_DESC = "ld <bcdehla>,<bcdehla>";
        public static int ZEXALL2_TEST_LD8RRX_ADDR = 0x1537;
        public static string ZEXALL2_TEST_LD8RRX_DESC = "ld <bcdexya>,<bcdexya>";
        public static int ZEXALL2_TEST_LDA_ADDR = 0x1596;
        public static string ZEXALL2_TEST_LDA_DESC = "ld a,(nnnn) / ld (nnnn),a";
        public static int ZEXALL2_TEST_LDD1_ADDR = 0x15F5;
        public static string ZEXALL2_TEST_LDD1_DESC = "ldd<r> (1)";
        public static int ZEXALL2_TEST_LDD2_ADDR = 0x1654;
        public static string ZEXALL2_TEST_LDD2_DESC = "ldd<r> (2)";
        public static int ZEXALL2_TEST_LDI1_ADDR = 0x16B3;
        public static string ZEXALL2_TEST_LDI1_DESC = "ldi<r> (1)";
        public static int ZEXALL2_TEST_LDI2_ADDR = 0x1712;
        public static string ZEXALL2_TEST_LDI2_DESC = "ldi<r> (2)";
        public static int ZEXALL2_TEST_NEGOP_ADDR = 0x1771;
        public static string ZEXALL2_TEST_NEGOP_DESC = "neg";
        public static int ZEXALL2_TEST_RLDOP_ADDR = 0x17D0;
        public static string ZEXALL2_TEST_RLDOP_DESC = "<rrd,rld>";
        public static int ZEXALL2_TEST_ROT8080_ADDR = 0x182F;
        public static string ZEXALL2_TEST_ROT8080_DESC = "<rlca,rrca,rla,rra>";
        public static int ZEXALL2_TEST_ROTXY_ADDR = 0x188E;
        public static string ZEXALL2_TEST_ROTXY_DESC = "shf/rot (<ix,iy>+1)";
        public static int ZEXALL2_TEST_ROTZ80_ADDR = 0x18ED;
        public static string ZEXALL2_TEST_ROTZ80_DESC = "shf/rot <b,c,d,e,h,l,(hl),a>";
        public static int ZEXALL2_TEST_SRZ80_ADDR = 0x194C;
        public static string ZEXALL2_TEST_SRZ80_DESC = "<set,res> n,<bcdehl(hl)a>";
        public static int ZEXALL2_TEST_SRZX_ADDR = 0x19AB;
        public static string ZEXALL2_TEST_SRZX_DESC = "<set,res> n,(<ix,iy>+1)";
        public static int ZEXALL2_TEST_ST8IX1_ADDR = 0x1A0A;
        public static string ZEXALL2_TEST_ST8IX1_DESC = "ld (<ix,iy>+1),<b,c,d,e>";
        public static int ZEXALL2_TEST_ST8IX2_ADDR = 0x1A69;
        public static string ZEXALL2_TEST_ST8IX2_DESC = "ld (<ix,iy>+1),<h,l>";
        public static int ZEXALL2_TEST_ST8IX3_ADDR = 0x1AC8;
        public static string ZEXALL2_TEST_ST8IX3_DESC = "ld (<ix,iy>+1),a";
        public static int ZEXALL2_TEST_STABD_ADDR = 0x1B27;
        public static string ZEXALL2_TEST_STABD_DESC = "ld (<bc,de>),a";

        public static void Check_InstructionSet(int launchAddress, string testDescription)
        {
            TestSystem testSystem = new TestSystem();

            string testSourcePath = "Zexall/zexall2-Tests.asm";
            using (Stream stream = PlatformSpecific.GetStreamForProjectFile(testSourcePath))
            {
                testSystem.LoadProgramInMemory(testSourcePath, stream, Encoding.UTF8, false);
            }

            /*string[] testNames = new string[] { "SCFOP","CCFOP","SCFCCF","CCFSCF","BITA","BITHL","BITX","BITZ80","DAAOP","CPLOP","ADC16","ADD16","ADD16X",
            "ADD16Y","ALU8I","ALU8R","ALU8RX","ALU8X","CPD1","CPI1","INCA","INCB","INCBC","INCC","INCD","INCDE","INCE","INCH","INCHL","INCIX","INCIY",
            "INCL","INCM","INCSP","INCX","INCXH","INCXL","INCYH","INCYL","LD161","LD162","LD163","LD164","LD165","LD166","LD167","LD168","LD16IM","LD16IX",
            "LD8BD","LD8IM","LD8IMX","LD8IX1","LD8IX2","LD8IX3","LD8IXY","LD8RR","LD8RRX","LDA","LDD1","LDD2","LDI1","LDI2","NEGOP","RLDOP","ROT8080",
            "ROTXY","ROTZ80","SRZ80","SRZX","ST8IX1","ST8IX2","ST8IX3","STABD" };
            string[] testDescriptions = new string[] {"scf","ccf","scf+ccf","ccf+scf","bit n,a","bit n,(hl)","bit n,(<ix,iy>+1)","bit n,<b,c,d,e,h,l,(hl),a>",
            "daa","cpl","<adc,sbc> hl,<bc,de,hl,sp>","add hl,<bc,de,hl,sp>","add ix,<bc,de,ix,sp>","add iy,<bc,de,iy,sp>","aluop a,nn","aluop a,<b,c,d,e,h,l,(hl),a>",
            "aluop a,<ixh,ixl,iyh,iyl>","aluop a,(<ix,iy>+1)","cpd<r>","cpi<r>","<inc,dec> a","<inc,dec> b","<inc,dec> bc","<inc,dec> c","<inc,dec> d","<inc,dec> de",
            "<inc,dec> e","<inc,dec> h","<inc,dec> hl","<inc,dec> ix","<inc,dec> iy","<inc,dec> l","<inc,dec> (hl)","<inc,dec> sp","<inc,dec> (<ix,iy>+1)","<inc,dec> ixh",
            "<inc,dec> ixl","<inc,dec> iyh","<inc,dec> iyl","ld <bc,de>,(nnnn)","ld hl,(nnnn)","ld sp,(nnnn)","ld <ix,iy>,(nnnn)","ld (nnnn),<bc,de>","ld (nnnn),hl",
            "ld (nnnn),sp","ld (nnnn),<ix,iy>","ld <bc,de,hl,sp>,nnnn","ld <ix,iy>,nnnn","ld a,<(bc),(de)>","ld <b,c,d,e,h,l,(hl),a>,nn","ld (<ix,iy>+1),nn",
            "ld <b,c,d,e>,(<ix,iy>+1)","ld <h,l>,(<ix,iy>+1)","ld a,(<ix,iy>+1)","ld <ixh,ixl,iyh,iyl>,nn","ld <bcdehla>,<bcdehla>","ld <bcdexya>,<bcdexya>",
            "ld a,(nnnn) / ld (nnnn),a","ldd<r> (1)","ldd<r> (2)","ldi<r> (1)","ldi<r> (2)","neg","<rrd,rld>","<rlca,rrca,rla,rra>","shf/rot (<ix,iy>+1)",
            "shf/rot <b,c,d,e,h,l,(hl),a>","<set,res> n,<bcdehl(hl)a>","<set,res> n,(<ix,iy>+1)","ld (<ix,iy>+1),<b,c,d,e>","ld (<ix,iy>+1),<h,l>","ld (<ix,iy>+1),a",
            "ld (<bc,de>),a" };

            StringBuilder sb = new StringBuilder();
            int testIndex = 0;
            foreach (string testName in testNames)
            {
                sb.AppendLine("public static int ZEXALL2_TEST_"+testName+"_ADDR = 0x" + testSystem.ProgramInMemory.Variables[testName].GetValue(testSystem.ProgramInMemory.Variables, null).ToString("X4") +";");
                sb.AppendLine("public static string ZEXALL2_TEST_"+testName+"_DESC = \"" + testDescriptions[testIndex++] + "\";");
                //sb.AppendLine("ZexallTests.Check_InstructionSet(ZexallTests.ZEXALL2_TEST_" + testName + "_ADDR, ZexallTests.ZEXALL2_TEST_" + testName + "_DESC);");
            }*/

            // Sample results :
            // scf............................
            // CRC: 9e4dbc94 expected: ff4ceb48
            // ccf............................
            // CRC: 363b6874 expected: 573a3fa8
            // scf+ccf........................OK
            // ccf+scf........................OK

            // Input parameter :
            // HL = test address
            testSystem.CPU.InitRegister16(Register16.HL, (ushort)launchAddress);

            // Test execution routine : STT - address 1B86
            // Exit point : address 1BFB
            testSystem.CPU.InitProgramCounter(0x1B86);
            testSystem.AddBreakpointOnProgramLine(7190);

            /*CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
               CPUStateLogger.CPUStateElements.InternalState |
               CPUStateLogger.CPUStateElements.Registers,
               new Z80CPU.LifecycleEventType[] { 
                    Z80CPU.LifecycleEventType.InstructionEnd
                });*/
            testSystem.ExecuteUntilNextBreakpoint();
            //logger.StopLogging();

            // Output parameters :
            // A = 0 success
            // A = 1 error 
            // =>  HL points to expected CRC, CRCVAL is the address of observed CRC
            if (testSystem.CPU.A != 0)
            {
                int expCRCAddress = testSystem.CPU.HL; // Pointer in CRCTAB
                int obsCRCAddress = 0x1E5E; // CRCVAL

                int expCRC = 0x1000000 * testSystem.Memory.Cells[expCRCAddress] + 0x10000 * testSystem.Memory.Cells[expCRCAddress + 1] + 0x100 * testSystem.Memory.Cells[expCRCAddress + 2] + testSystem.Memory.Cells[expCRCAddress + 3];
                int obsCRC = 0x1000000 * testSystem.Memory.Cells[obsCRCAddress] + 0x10000 * testSystem.Memory.Cells[obsCRCAddress + 1] + 0x100 * testSystem.Memory.Cells[obsCRCAddress + 2] + testSystem.Memory.Cells[obsCRCAddress + 3];

                throw new Exception("Zexall instruction test for " + testDescription + " failed : observed CRC = " + obsCRC.ToString("X8") + ", expected CRC = " + expCRC.ToString("X8"));
            }
        }
    }
}
