using System;
using System.Collections.Generic;
using System.Text;
using Z80Simulator.CPU;
using Z80Simulator.Test.System;

namespace Z80Simulator.Test.CPU
{
    public partial class CPUInstructionsTests
    {
        public static void Check_Instruction_43_IN_Register_IOPort()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "11 (4, 3, 4)"
            testProgram.AppendLine("IN A,(1)");
            // TStatesByMachineCycle = "11 (4, 3, 4)"
            testProgram.AppendLine("IN A,(2)");

            TestSystem testSystem = new TestSystemWithTwoDevices(false);
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(2);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/InputAndOutput/Logs/Check_Instruction_43_IN_Register_IOPort"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_44_IN_Register_RegisterIOPort()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD B,8FH");
            testProgram.AppendLine("LD C,1");
            testProgram.AppendLine("IN D,(C)");
            testProgram.AppendLine("LD C,2");
            testProgram.AppendLine("IN B,(C)");
            testProgram.AppendLine("LD C,3");
            testProgram.AppendLine("IN H,(C)");

            IDictionary<byte, IList<byte>> valuesForPortAddresses = new Dictionary<byte, IList<byte>>();
            valuesForPortAddresses[1] = new byte[] { 0xAA };
            valuesForPortAddresses[2] = new byte[] { 0x00 };
            valuesForPortAddresses[3] = new byte[] { 0x29 };
            TestSystem testSystem = new TestSystemWithMultiportDevice(valuesForPortAddresses);
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers | CPUStateLogger.CPUStateElements.Buses,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.HalfTState });
            testSystem.ExecuteInstructionCount(7);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/InputAndOutput/Logs/Check_Instruction_44_IN_Register_RegisterIOPort"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_45_IN_Flags_RegisterIOPort()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD B,8FH");
            testProgram.AppendLine("LD C,1");
            testProgram.AppendLine("IN F,(C)");
            testProgram.AppendLine("LD C,2");
            testProgram.AppendLine("IN F,(C)");
            testProgram.AppendLine("LD C,3");
            testProgram.AppendLine("IN F,(C)");

            IDictionary<byte, IList<byte>> valuesForPortAddresses = new Dictionary<byte, IList<byte>>();
            valuesForPortAddresses[1] = new byte[] { 0xAA };
            valuesForPortAddresses[2] = new byte[] { 0x00 };
            valuesForPortAddresses[3] = new byte[] { 0x29 };
            TestSystem testSystem = new TestSystemWithMultiportDevice(valuesForPortAddresses);
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers | CPUStateLogger.CPUStateElements.Buses,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.HalfTState });
            testSystem.ExecuteInstructionCount(7);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/InputAndOutput/Logs/Check_Instruction_45_IN_Flags_RegisterIOPort"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_50_IND()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD B,3");
            testProgram.AppendLine("LD C,1");
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("IND");
            testProgram.AppendLine("LD C,2");
            testProgram.AppendLine("IND");
            testProgram.AppendLine("LD C,3");
            testProgram.AppendLine("IND");

            IDictionary<byte, IList<byte>> valuesForPortAddresses = new Dictionary<byte, IList<byte>>();
            valuesForPortAddresses[1] = new byte[] { 0xAA };
            valuesForPortAddresses[2] = new byte[] { 0x00 };
            valuesForPortAddresses[3] = new byte[] { 0x29 };
            TestSystem testSystem = new TestSystemWithMultiportDevice(valuesForPortAddresses);
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers | CPUStateLogger.CPUStateElements.Buses,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.HalfTState });
            testSystem.ExecuteInstructionCount(8);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/InputAndOutput/Logs/Check_Instruction_50_IND") ||
                !(testSystem.Memory.CheckByte(100) == 0xAA && testSystem.Memory.CheckByte(99) == 0x00 && testSystem.Memory.CheckByte(98) == 0x29) )
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_51_INDR()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD B,3");
            testProgram.AppendLine("LD C,87");
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("INDR");

            IDictionary<byte, IList<byte>> valuesForPortAddresses = new Dictionary<byte, IList<byte>>();
            valuesForPortAddresses[87] = new byte[] { 0xAA, 0x00, 0x29 };
            TestSystem testSystem = new TestSystemWithMultiportDevice(valuesForPortAddresses);
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers | CPUStateLogger.CPUStateElements.Buses,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.HalfTState });
            testSystem.ExecuteInstructionCount(7);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/InputAndOutput/Logs/Check_Instruction_51_INDR") ||
                !(testSystem.Memory.CheckByte(100) == 0xAA && testSystem.Memory.CheckByte(99) == 0x00 && testSystem.Memory.CheckByte(98) == 0x29))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_52_INI()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD B,3");
            testProgram.AppendLine("LD C,1");
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("INI");
            testProgram.AppendLine("LD C,2");
            testProgram.AppendLine("INI");
            testProgram.AppendLine("LD C,3");
            testProgram.AppendLine("INI");

            IDictionary<byte, IList<byte>> valuesForPortAddresses = new Dictionary<byte, IList<byte>>();
            valuesForPortAddresses[1] = new byte[] { 0xAA };
            valuesForPortAddresses[2] = new byte[] { 0x00 };
            valuesForPortAddresses[3] = new byte[] { 0x29 };
            TestSystem testSystem = new TestSystemWithMultiportDevice(valuesForPortAddresses);
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers | CPUStateLogger.CPUStateElements.Buses,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.HalfTState });
            testSystem.ExecuteInstructionCount(8);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/InputAndOutput/Logs/Check_Instruction_52_INI") ||
                !(testSystem.Memory.CheckByte(100) == 0xAA && testSystem.Memory.CheckByte(101) == 0x00 && testSystem.Memory.CheckByte(102) == 0x29))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_53_INIR()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD B,3");
            testProgram.AppendLine("LD C,85");
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("INIR");

            IDictionary<byte, IList<byte>> valuesForPortAddresses = new Dictionary<byte, IList<byte>>();
            valuesForPortAddresses[85] = new byte[] { 0xAA, 0x00, 0x29 };
            TestSystem testSystem = new TestSystemWithMultiportDevice(valuesForPortAddresses);
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers | CPUStateLogger.CPUStateElements.Buses,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.HalfTState });
            testSystem.ExecuteInstructionCount(7);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/InputAndOutput/Logs/Check_Instruction_53_INIR") ||
                !(testSystem.Memory.CheckByte(100) == 0xAA && testSystem.Memory.CheckByte(101) == 0x00 && testSystem.Memory.CheckByte(102) == 0x29))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_83_OTDR()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD B,3");
            testProgram.AppendLine("LD C,1");
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("LD (HL),0AAH");
            testProgram.AppendLine("DEC HL");
            testProgram.AppendLine("LD (HL),00H");
            testProgram.AppendLine("DEC HL");
            testProgram.AppendLine("LD (HL),29H");
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("OTDR");

            IDictionary<byte, IList<byte>> valuesForPortAddresses = new Dictionary<byte, IList<byte>>();
            valuesForPortAddresses[1] = new byte[] { 0x00 };
            TestSystemWithMultiportDevice testSystem = new TestSystemWithMultiportDevice(valuesForPortAddresses);
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers | CPUStateLogger.CPUStateElements.Buses,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.HalfTState });
            testSystem.ExecuteInstructionCount(13);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/InputAndOutput/Logs/Check_Instruction_83_OTDR") ||
                !(testSystem.MultiportDevice.WrittenValuesForPortAddresses[1][0] == 0xAA &&
                  testSystem.MultiportDevice.WrittenValuesForPortAddresses[1][1] == 0x00 && 
                  testSystem.MultiportDevice.WrittenValuesForPortAddresses[1][2] == 0x29))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_84_OTIR()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD B,3");
            testProgram.AppendLine("LD C,1");
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("LD (HL),0AAH");
            testProgram.AppendLine("INC HL");
            testProgram.AppendLine("LD (HL),00H");
            testProgram.AppendLine("INC HL");
            testProgram.AppendLine("LD (HL),29H");
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("OTIR");

            IDictionary<byte, IList<byte>> valuesForPortAddresses = new Dictionary<byte, IList<byte>>();
            valuesForPortAddresses[1] = new byte[] { 0x00 };
            TestSystemWithMultiportDevice testSystem = new TestSystemWithMultiportDevice(valuesForPortAddresses);
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers | CPUStateLogger.CPUStateElements.Buses,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.HalfTState });
            testSystem.ExecuteInstructionCount(13);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/InputAndOutput/Logs/Check_Instruction_84_OTIR") ||
                !(testSystem.MultiportDevice.WrittenValuesForPortAddresses[1][0] == 0xAA &&
                  testSystem.MultiportDevice.WrittenValuesForPortAddresses[1][1] == 0x00 &&
                  testSystem.MultiportDevice.WrittenValuesForPortAddresses[1][2] == 0x29))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_85_OUT_IOPort_Register()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,23");
            // TStatesByMachineCycle = "11 (4, 3, 4)"
            testProgram.AppendLine("OUT (1),A");
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,0");
            // TStatesByMachineCycle = "11 (4, 3, 4)"
            testProgram.AppendLine("OUT (2),A");
            // TStatesByMachineCycle = "11 (4, 3, 4)"
            testProgram.AppendLine("IN A,(1)");
            // TStatesByMachineCycle = "11 (4, 3, 4)"
            testProgram.AppendLine("IN A,(2)");

            TestSystem testSystem = new TestSystemWithTwoDevices(false);
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(6);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/InputAndOutput/Logs/Check_Instruction_85_OUT_IOPort_Register"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_86_OUT_RegisterIOPort()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD B,0FFH");
            testProgram.AppendLine("OUT (1),A");
            testProgram.AppendLine("OUT (2),A");
            testProgram.AppendLine("LD C,1");
            testProgram.AppendLine("OUT (C)");
            testProgram.AppendLine("LD C,2");
            testProgram.AppendLine("OUT (C)");
            testProgram.AppendLine("LD A,0FFH");
            testProgram.AppendLine("IN A,(1)");
            testProgram.AppendLine("LD A,0FFH");
            testProgram.AppendLine("IN A,(2)");

            TestSystem testSystem = new TestSystemWithTwoDevices(false);
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers | CPUStateLogger.CPUStateElements.Buses,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.HalfTState });
            testSystem.ExecuteInstructionCount(11);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/InputAndOutput/Logs/Check_Instruction_86_OUT_RegisterIOPort"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_87_OUT_RegisterIOPort_Register()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD A,0AFH");
            testProgram.AppendLine("LD B,0BFH");
            testProgram.AppendLine("LD L,0CFH");
            testProgram.AppendLine("LD C,1");
            testProgram.AppendLine("OUT (C),A");
            testProgram.AppendLine("LD C,2");
            testProgram.AppendLine("OUT (C),L");
            testProgram.AppendLine("LD A,0");
            testProgram.AppendLine("IN A,(1)");
            testProgram.AppendLine("LD A,0");
            testProgram.AppendLine("IN A,(2)");

            TestSystem testSystem = new TestSystemWithTwoDevices(false);
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers | CPUStateLogger.CPUStateElements.Buses,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.HalfTState });
            testSystem.ExecuteInstructionCount(11);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/InputAndOutput/Logs/Check_Instruction_87_OUT_RegisterIOPort_Register"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_88_OUTD()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD B,3");
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("LD (HL),0AAH");
            testProgram.AppendLine("DEC HL");
            testProgram.AppendLine("LD (HL),00H");
            testProgram.AppendLine("DEC HL");
            testProgram.AppendLine("LD (HL),29H");
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("LD C,1");
            testProgram.AppendLine("OUTD");
            testProgram.AppendLine("LD C,2");
            testProgram.AppendLine("OUTD");
            testProgram.AppendLine("LD C,3");
            testProgram.AppendLine("OUTD");

            IDictionary<byte, IList<byte>> valuesForPortAddresses = new Dictionary<byte, IList<byte>>();
            valuesForPortAddresses[1] = new byte[] { 0x00 };
            valuesForPortAddresses[2] = new byte[] { 0x00 };
            valuesForPortAddresses[3] = new byte[] { 0x00 };
            TestSystemWithMultiportDevice testSystem = new TestSystemWithMultiportDevice(valuesForPortAddresses);
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers | CPUStateLogger.CPUStateElements.Buses,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.HalfTState });
            testSystem.ExecuteInstructionCount(14);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/InputAndOutput/Logs/Check_Instruction_88_OUTD") ||
                !(testSystem.MultiportDevice.WrittenValuesForPortAddresses[1][0] == 0xAA &&
                  testSystem.MultiportDevice.WrittenValuesForPortAddresses[2][0] == 0x00 &&
                  testSystem.MultiportDevice.WrittenValuesForPortAddresses[3][0] == 0x29))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_89_OUTI()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD B,3");
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("LD (HL),0AAH");
            testProgram.AppendLine("INC HL");
            testProgram.AppendLine("LD (HL),00H");
            testProgram.AppendLine("INC HL");
            testProgram.AppendLine("LD (HL),29H");
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("LD C,1");
            testProgram.AppendLine("OUTI");
            testProgram.AppendLine("LD C,2");
            testProgram.AppendLine("OUTI");
            testProgram.AppendLine("LD C,3");
            testProgram.AppendLine("OUTI");

            IDictionary<byte, IList<byte>> valuesForPortAddresses = new Dictionary<byte, IList<byte>>();
            valuesForPortAddresses[1] = new byte[] { 0x00 };
            valuesForPortAddresses[2] = new byte[] { 0x00 };
            valuesForPortAddresses[3] = new byte[] { 0x00 };
            TestSystemWithMultiportDevice testSystem = new TestSystemWithMultiportDevice(valuesForPortAddresses);
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers | CPUStateLogger.CPUStateElements.Buses,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.HalfTState });
            testSystem.ExecuteInstructionCount(14);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/InputAndOutput/Logs/Check_Instruction_89_OUTI") ||
                !(testSystem.MultiportDevice.WrittenValuesForPortAddresses[1][0] == 0xAA &&
                  testSystem.MultiportDevice.WrittenValuesForPortAddresses[2][0] == 0x00 &&
                  testSystem.MultiportDevice.WrittenValuesForPortAddresses[3][0] == 0x29))
            {
                throw new Exception("Log compare failed");
            }
        }        
    }
}
