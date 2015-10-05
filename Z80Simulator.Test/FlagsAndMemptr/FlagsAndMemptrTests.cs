using System;
using System.Linq;
using System.IO;
using System.Text;
using Z80Simulator.Assembly;
using Z80Simulator.CPU;
using Z80Simulator.Test.CPU;
using Z80Simulator.Test.System;

namespace Z80Simulator.Test.FlagsAndMemptr
{
    public class FlagsAndMemptrTests
    {
        public static int FLAGS_TEST_01_ADDR = 0x0000;
        public static string FLAGS_TEST_01_DESC = "SCF";
        public static int FLAGS_TEST_02_ADDR = 0x000E;
        public static string FLAGS_TEST_02_DESC = "CCF";
        public static int FLAGS_TEST_03_ADDR = 0x001C;
        public static string FLAGS_TEST_03_DESC = "DAA";
        public static int FLAGS_TEST_04_ADDR = 0x0029;
        public static string FLAGS_TEST_04_DESC = "CPL";
        public static int FLAGS_TEST_05_ADDR = 0x0036;
        public static string FLAGS_TEST_05_DESC = "NEG";
        public static int FLAGS_TEST_06_ADDR = 0x0044;
        public static string FLAGS_TEST_06_DESC = "AND";
        public static int FLAGS_TEST_07_ADDR = 0x0052;
        public static string FLAGS_TEST_07_DESC = "OR";
        public static int FLAGS_TEST_08_ADDR = 0x0060;
        public static string FLAGS_TEST_08_DESC = "XOR";
        public static int FLAGS_TEST_09_ADDR = 0x006E;
        public static string FLAGS_TEST_09_DESC = "CP";
        public static int FLAGS_TEST_11_ADDR = 0x007C;
        public static string FLAGS_TEST_11_DESC = "INC8";
        public static int FLAGS_TEST_12_ADDR = 0x008A;
        public static string FLAGS_TEST_12_DESC = "ADD8";
        public static int FLAGS_TEST_14_ADDR = 0x0098;
        public static string FLAGS_TEST_14_DESC = "ADC8";
        public static int FLAGS_TEST_15_ADDR = 0x00A6;
        public static string FLAGS_TEST_15_DESC = "DEC8";
        public static int FLAGS_TEST_10_ADDR = 0x00B4;
        public static string FLAGS_TEST_10_DESC = "SUB8";
        public static int FLAGS_TEST_13_ADDR = 0x00C2;
        public static string FLAGS_TEST_13_DESC = "SBC8";
        public static int FLAGS_TEST_16_ADDR = 0x00D0;
        public static string FLAGS_TEST_16_DESC = "ADD16";
        public static int FLAGS_TEST_17_ADDR = 0x00DD;
        public static string FLAGS_TEST_17_DESC = "ADC16";
        public static int FLAGS_TEST_18_ADDR = 0x00ED;
        public static string FLAGS_TEST_18_DESC = "SBC16";
        public static int FLAGS_TEST_19_ADDR = 0x00FD;
        public static string FLAGS_TEST_19_DESC = "RLA/RRA";
        public static int FLAGS_TEST_20_ADDR = 0x0110;
        public static string FLAGS_TEST_20_DESC = "RLCA/RRCA";
        public static int FLAGS_TEST_21_ADDR = 0x0123;
        public static string FLAGS_TEST_21_DESC = "RLC/RRC";
        public static int FLAGS_TEST_22_ADDR = 0x0138;
        public static string FLAGS_TEST_22_DESC = "RL/RR";
        public static int FLAGS_TEST_23_ADDR = 0x014D;
        public static string FLAGS_TEST_23_DESC = "SLA/SRA";
        public static int FLAGS_TEST_24_ADDR = 0x0162;
        public static string FLAGS_TEST_24_DESC = "SLL/SRL";
        public static int FLAGS_TEST_25_ADDR = 0x0177;
        public static string FLAGS_TEST_25_DESC = "RLD/RRD";
        public static int FLAGS_TEST_26_ADDR = 0x0190;
        public static string FLAGS_TEST_26_DESC = "LD A,I/R";
        public static int FLAGS_TEST_27_ADDR = 0x01A3;
        public static string FLAGS_TEST_27_DESC = "BIT n,(HL)";
        public static int FLAGS_TEST_28_ADDR = 0x01D7;
        public static string FLAGS_TEST_28_DESC = "BIT n,(IX+d)";
        public static int FLAGS_TEST_29_ADDR = 0x021C;
        public static string FLAGS_TEST_29_DESC = "BIT n,(IY+d)";
        public static int FLAGS_TEST_30_ADDR = 0x029C;
        public static string FLAGS_TEST_30_DESC = "LDI";
        public static int FLAGS_TEST_31_ADDR = 0x02A4;
        public static string FLAGS_TEST_31_DESC = "LDD";
        public static int FLAGS_TEST_32_ADDR = 0x02AC;
        public static string FLAGS_TEST_32_DESC = "LDIR";
        public static int FLAGS_TEST_33_ADDR = 0x02B4;
        public static string FLAGS_TEST_33_DESC = "LDDR";
        public static int FLAGS_TEST_34_ADDR = 0x02BC;
        public static string FLAGS_TEST_34_DESC = "CPI";
        public static int FLAGS_TEST_35_ADDR = 0x02C4;
        public static string FLAGS_TEST_35_DESC = "CPD";
        public static int FLAGS_TEST_36_ADDR = 0x02CC;
        public static string FLAGS_TEST_36_DESC = "INI";
        public static int FLAGS_TEST_37_ADDR = 0x02D4;
        public static string FLAGS_TEST_37_DESC = "IND";
        public static int FLAGS_TEST_38_ADDR = 0x02DC;
        public static string FLAGS_TEST_38_DESC = "OUTI";
        public static int FLAGS_TEST_39_ADDR = 0x02E4;
        public static string FLAGS_TEST_39_DESC = "OUTD";
        public static int FLAGS_TEST_40_ADDR = 0x0320;
        public static string FLAGS_TEST_40_DESC = "DD CB (00-FF)  ROM";
        public static int FLAGS_TEST_41_ADDR = 0x0329;
        public static string FLAGS_TEST_41_DESC = "DD CB (00-FF)  RAM";
        public static int FLAGS_TEST_42_ADDR = 0x0383;
        public static string FLAGS_TEST_42_DESC = "FD CB (00-FF)  ROM";
        public static int FLAGS_TEST_43_ADDR = 0x038B;
        public static string FLAGS_TEST_43_DESC = "FD CB (00-FF)  RAM";
        public static int FLAGS_TEST_46_ADDR = 0x03E7;
        public static string FLAGS_TEST_46_DESC = "CB (00-FF)     ROM";
        public static int FLAGS_TEST_47_ADDR = 0x03F5;
        public static string FLAGS_TEST_47_DESC = "CB (00-FF)     RAM";
        public static int FLAGS_TEST_44_ADDR = 0x040E;
        public static string FLAGS_TEST_44_DESC = "CB (00-FF) 5+3 ROM";
        public static int FLAGS_TEST_45_ADDR = 0x041B;
        public static string FLAGS_TEST_45_DESC = "CB (00-FF) 5+3 RAM";


        public static void Check_Flags(int launchAddress, string testDescription)
        {
            Console.Write(testDescription + "...");

            TestSystem testSystem = new TestSystem();

            string testSourcePath = "FlagsAndMemptr/z80tests-Flags.asm";
            using (Stream stream = PlatformSpecific.GetStreamForProjectFile(testSourcePath))
            {
                testSystem.LoadProgramInMemory(testSourcePath, stream, Encoding.UTF8, false);
                ((TestSystem._TestMemory)testSystem.Memory).SetReadOnlySection(0x500,0x5FF);
            }
            testSystem.CPU.InitProgramCounter((ushort)launchAddress);
            testSystem.CPU.InitRegister(Z80Simulator.Instructions.Register.I, 63);
            testSystem.AddBreakpointOnProgramLine(619);

            /*StringBuilder sb = new StringBuilder();
            int counter = 1;
            foreach (string variable in testSystem.ProgramInMemory.Variables.Keys)
            {
                if (variable.Contains("LAUNCH"))
                {
                    sb.AppendLine("public static int " + variable.Replace("LAUNCH", "ADDR") + " = 0x" + testSystem.ProgramInMemory.Variables[variable].GetValue(testSystem.ProgramInMemory.Variables, null).ToString("X4") +";");
                    sb.AppendLine("public static string " + variable.Replace("LAUNCH", "DESC") + " = \"" + testSystem.ProgramInMemory.Lines.First(line => (line.Comment !=null && line.Comment.Contains("test "+counter.ToString("D2")+" : "))).Comment.Split(':')[1].Trim() + "\";");
                    counter++;
                }
            }
            sb.Clear();
            for (counter = 1; counter <= 45; counter++)
            {
                sb.AppendLine("FlagsAndMemptrTests.Check_Flags(FlagsAndMemptrTests.FLAGS_TEST_" + counter.ToString("D2") + "_ADDR, FlagsAndMemptrTests.FLAGS_TEST_" + counter.ToString("D2") + "_DESC);");
            }*/


            /*CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
               CPUStateLogger.CPUStateElements.InternalState |
               CPUStateLogger.CPUStateElements.Registers,
               new Z80CPU.LifecycleEventType[] { 
                    Z80CPU.LifecycleEventType.InstructionEnd
                });*/
            testSystem.ExecuteUntilNextBreakpoint();
            //logger.StopLogging();

            if (!(testSystem.CPU.A == 0 ||
                 (launchAddress == FLAGS_TEST_27_ADDR && testSystem.CPU.HL == 0x3063)))
            {
                throw new Exception("Flags test for " + testDescription + " failed : expected checksum " + testSystem.CPU.BC.ToString("X4") + " / computed checksum " + testSystem.CPU.HL.ToString("X4"));
            }

            Console.WriteLine("OK");
        }

        public static int MEMPTR_TEST_01_ADDR = 0x0040;
        public static string MEMPTR_TEST_01_DESC = "LD A,(BC)";
        public static int MEMPTR_TEST_02_ADDR = 0x0062;
        public static string MEMPTR_TEST_02_DESC = "LD A,(DE)";
        public static int MEMPTR_TEST_03_ADDR = 0x0086;
        public static string MEMPTR_TEST_03_DESC = "LD A,(HL)";
        public static int MEMPTR_TEST_04_ADDR = 0x00AB;
        public static string MEMPTR_TEST_04_DESC = "LD (BC),A";
        public static int MEMPTR_TEST_05_ADDR = 0x00D0;
        public static string MEMPTR_TEST_05_DESC = "LD (DE),A";
        public static int MEMPTR_TEST_06_ADDR = 0x00F5;
        public static string MEMPTR_TEST_06_DESC = "LD (HL),A";
        public static int MEMPTR_TEST_07_ADDR = 0x011C;
        public static string MEMPTR_TEST_07_DESC = "LD HL,(addr)";
        public static int MEMPTR_TEST_08_ADDR = 0x0143;
        public static string MEMPTR_TEST_08_DESC = "LD HL,(addr) [ED]";
        public static int MEMPTR_TEST_09_ADDR = 0x016A;
        public static string MEMPTR_TEST_09_DESC = "LD DE,(addr)";
        public static int MEMPTR_TEST_10_ADDR = 0x018C;
        public static string MEMPTR_TEST_10_DESC = "LD BC,(addr)";
        public static int MEMPTR_TEST_11_ADDR = 0x01AF;
        public static string MEMPTR_TEST_11_DESC = "LD IX,(addr)";
        public static int MEMPTR_TEST_12_ADDR = 0x01D2;
        public static string MEMPTR_TEST_12_DESC = "LD IY,(addr)";
        public static int MEMPTR_TEST_13_ADDR = 0x01F5;
        public static string MEMPTR_TEST_13_DESC = "LD SP,(addr)";
        public static int MEMPTR_TEST_14_ADDR = 0x021A;
        public static string MEMPTR_TEST_14_DESC = "LD (addr),HL";
        public static int MEMPTR_TEST_15_ADDR = 0x023F;
        public static string MEMPTR_TEST_15_DESC = "LD (addr),HL [ED]";
        public static int MEMPTR_TEST_16_ADDR = 0x0267;
        public static string MEMPTR_TEST_16_DESC = "LD (addr),DE";
        public static int MEMPTR_TEST_17_ADDR = 0x0287;
        public static string MEMPTR_TEST_17_DESC = "LD (addr),BC";
        public static int MEMPTR_TEST_18_ADDR = 0x02A8;
        public static string MEMPTR_TEST_18_DESC = "LD (addr),IX";
        public static int MEMPTR_TEST_19_ADDR = 0x02C9;
        public static string MEMPTR_TEST_19_DESC = "LD (addr),IY";
        public static int MEMPTR_TEST_20_ADDR = 0x02EA;
        public static string MEMPTR_TEST_20_DESC = "LD (addr),SP";
        public static int MEMPTR_TEST_21_ADDR = 0x030B;
        public static string MEMPTR_TEST_21_DESC = "EX (SP),HL";
        public static int MEMPTR_TEST_22_ADDR = 0x032C;
        public static string MEMPTR_TEST_22_DESC = "EX (SP),IX";
        public static int MEMPTR_TEST_23_ADDR = 0x034D;
        public static string MEMPTR_TEST_23_DESC = "EX (SP),IY";
        public static int MEMPTR_TEST_24_ADDR = 0x0371;
        public static string MEMPTR_TEST_24_DESC = "ADD HL,BC";
        public static int MEMPTR_TEST_25_ADDR = 0x039A;
        public static string MEMPTR_TEST_25_DESC = "ADD IX,BC";
        public static int MEMPTR_TEST_26_ADDR = 0x03C3;
        public static string MEMPTR_TEST_26_DESC = "ADD IY,BC";
        public static int MEMPTR_TEST_27_ADDR = 0x03EB;
        public static string MEMPTR_TEST_27_DESC = "ADC HL,BC";
        public static int MEMPTR_TEST_28_ADDR = 0x0417;
        public static string MEMPTR_TEST_28_DESC = "SBC HL,BC";
        public static int MEMPTR_TEST_29_ADDR = 0x0443;
        public static string MEMPTR_TEST_29_DESC = "DJNZ (taken)";
        public static int MEMPTR_TEST_30_ADDR = 0x046D;
        public static string MEMPTR_TEST_30_DESC = "DJNZ (not taken)";
        public static int MEMPTR_TEST_31_ADDR = 0x0000;
        public static string MEMPTR_TEST_31_DESC = "LD A,(addr)";
        public static int MEMPTR_TEST_32_ADDR = 0x0020;
        public static string MEMPTR_TEST_32_DESC = "LD (addr),A";
        public static int MEMPTR_TEST_33_ADDR = 0x0497;
        public static string MEMPTR_TEST_33_DESC = "RLD";
        public static int MEMPTR_TEST_34_ADDR = 0x04BF;
        public static string MEMPTR_TEST_34_DESC = "RRD";
        public static int MEMPTR_TEST_35_ADDR = 0x04E7;
        public static string MEMPTR_TEST_35_DESC = "IN A,(port)";
        public static int MEMPTR_TEST_36_ADDR = 0x050A;
        public static string MEMPTR_TEST_36_DESC = "IN A,(C)";
        public static int MEMPTR_TEST_37_ADDR = 0x0530;
        public static string MEMPTR_TEST_37_DESC = "OUT (port),A";
        public static int MEMPTR_TEST_38_ADDR = 0x0553;
        public static string MEMPTR_TEST_38_DESC = "OUT (C),A";
        public static int MEMPTR_TEST_39_ADDR = 0x057B;
        public static string MEMPTR_TEST_39_DESC = "LDI";
        public static int MEMPTR_TEST_40_ADDR = 0x05A7;
        public static string MEMPTR_TEST_40_DESC = "LDD";
        public static int MEMPTR_TEST_41_ADDR = 0x05D3;
        public static string MEMPTR_TEST_41_DESC = "LDIR (BC=1)";
        public static int MEMPTR_TEST_42_ADDR = 0x0600;
        public static string MEMPTR_TEST_42_DESC = "LDIR (BC>1)";
        public static int MEMPTR_TEST_43_ADDR = 0x062D;
        public static string MEMPTR_TEST_43_DESC = "LDDR (BC=1)";
        public static int MEMPTR_TEST_44_ADDR = 0x065A;
        public static string MEMPTR_TEST_44_DESC = "LDDR (BC>1)";
        public static int MEMPTR_TEST_45_ADDR = 0x0687;
        public static string MEMPTR_TEST_45_DESC = "CPI";
        public static int MEMPTR_TEST_46_ADDR = 0x06AA;
        public static string MEMPTR_TEST_46_DESC = "CPD";
        public static int MEMPTR_TEST_47_ADDR = 0x06D0;
        public static string MEMPTR_TEST_47_DESC = "CPIR (BC=1)";
        public static int MEMPTR_TEST_48_ADDR = 0x06FB;
        public static string MEMPTR_TEST_48_DESC = "CPIR (BC>1)";
        public static int MEMPTR_TEST_49_ADDR = 0x0726;
        public static string MEMPTR_TEST_49_DESC = "CPDR (BC=1)";
        public static int MEMPTR_TEST_50_ADDR = 0x0751;
        public static string MEMPTR_TEST_50_DESC = "CPDR (BC>1)";
        public static int MEMPTR_TEST_51_ADDR = 0x077C;
        public static string MEMPTR_TEST_51_DESC = "INI";
        public static int MEMPTR_TEST_52_ADDR = 0x07A5;
        public static string MEMPTR_TEST_52_DESC = "IND";
        public static int MEMPTR_TEST_53_ADDR = 0x07CE;
        public static string MEMPTR_TEST_53_DESC = "INIR";
        public static int MEMPTR_TEST_54_ADDR = 0x07F7;
        public static string MEMPTR_TEST_54_DESC = "INDR";
        public static int MEMPTR_TEST_55_ADDR = 0x0820;
        public static string MEMPTR_TEST_55_DESC = "OUTI";
        public static int MEMPTR_TEST_56_ADDR = 0x0849;
        public static string MEMPTR_TEST_56_DESC = "OUTD";
        public static int MEMPTR_TEST_57_ADDR = 0x0872;
        public static string MEMPTR_TEST_57_DESC = "OTIR";
        public static int MEMPTR_TEST_58_ADDR = 0x089B;
        public static string MEMPTR_TEST_58_DESC = "OTDR";

        public static void Check_Memptr(int launchAddress, string testDescription)
        {
            TestSystem testSystem = new TestSystem();

            string testSourcePath = "FlagsAndMemptr/z80tests-Memptr.asm";
            using (Stream stream = PlatformSpecific.GetStreamForProjectFile(testSourcePath))
            {
                testSystem.LoadProgramInMemory(testSourcePath, stream, Encoding.UTF8, false);
            }
            testSystem.CPU.InitProgramCounter((ushort)launchAddress);
            testSystem.AddBreakpointOnProgramLine(1974);

            /*StringBuilder sb = new StringBuilder();
            int counter = 1;
            foreach (string variable in testSystem.ProgramInMemory.Variables.Keys)
            {
                if (variable.Contains("LAUNCH"))
                {
                    sb.AppendLine("public static int " + variable.Replace("LAUNCH", "ADDR") + " = 0x" + testSystem.ProgramInMemory.Variables[variable].GetValue(testSystem.ProgramInMemory.Variables, null).ToString("X4") +";");
                    sb.AppendLine("public static string " + variable.Replace("LAUNCH", "DESC") + " = \"" + testSystem.ProgramInMemory.Lines.First(line => (line.Comment !=null && line.Comment.Contains("test "+counter.ToString("D2")+" : "))).Comment.Split(':')[1].Trim() + "\";");
                    counter++;
                }
            }
            sb.Clear();
            for (counter = 1; counter <= 58; counter++)
            {
                sb.AppendLine("FlagsAndMemptrTests.Check_Memptr(FlagsAndMemptrTests.MEMPTR_TEST_" + counter.ToString("D2") + "_ADDR, FlagsAndMemptrTests.MEMPTR_TEST_" + counter.ToString("D2") + "_DESC);");
            }*/

            /*CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
               CPUStateLogger.CPUStateElements.InternalState |
               CPUStateLogger.CPUStateElements.Registers,
               new Z80CPU.LifecycleEventType[] { 
                    Z80CPU.LifecycleEventType.InstructionEnd
                });*/
            testSystem.ExecuteUntilNextBreakpoint();
            //logger.StopLogging();

            if (testSystem.CPU.A != 0)
            {
                throw new Exception("Memptr test for " + testDescription + " failed");
            }
        }
    }
}
