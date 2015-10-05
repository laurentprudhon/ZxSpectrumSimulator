using System;
using System.Text;
using Z80Simulator.CPU;
using Z80Simulator.Test.CPU;
using Z80Simulator.Test.System;

namespace Z80Simulator.Test.ALU
{
    public partial class ALUInstructionsTests
    {
        public static void Check_Instruction_100_RL_Register()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD B,00000001B");
            testProgram.AppendLine("RL B");
            testProgram.AppendLine("RL B");
            testProgram.AppendLine("RL B");
            testProgram.AppendLine("RL B");
            testProgram.AppendLine("RL B");
            testProgram.AppendLine("RL B");
            testProgram.AppendLine("RL B");
            testProgram.AppendLine("RL B");
            testProgram.AppendLine("RL B");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(10);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_100_RL_Register"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_101_RL_Register16Address()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("LD (HL),1");
            testProgram.AppendLine("RL (HL)");
            testProgram.AppendLine("LD A,(HL)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_101_RL_Register16Address"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_102_RL_IndexedAddress()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD IX,100");
            testProgram.AppendLine("LD (IX+1),1");
            testProgram.AppendLine("RL (IX+1)");
            testProgram.AppendLine("LD A,(IX+1)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_102_RL_IndexedAddress"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_103_RL_IndexedAddress_Register()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD IY,100");
            testProgram.AppendLine("LD (IY+1),1");
            testProgram.AppendLine("RL (IY+1),B");
            testProgram.AppendLine("LD A,(IY+1)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_103_RL_IndexedAddress_Register"))
            {
                throw new Exception("Log compare failed");
            }
        }
        
        public static void Check_Instruction_104_RLA()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD A,00000001B");
            testProgram.AppendLine("RLA");
            testProgram.AppendLine("RLA");
            testProgram.AppendLine("RLA");
            testProgram.AppendLine("RLA");
            testProgram.AppendLine("RLA");
            testProgram.AppendLine("RLA");
            testProgram.AppendLine("RLA");
            testProgram.AppendLine("RLA");
            testProgram.AppendLine("RLA");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(10);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_104_RLA"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_105_RLC_Register()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD B,00000001B");
            testProgram.AppendLine("RLC B");
            testProgram.AppendLine("RLC B");
            testProgram.AppendLine("RLC B");
            testProgram.AppendLine("RLC B");
            testProgram.AppendLine("RLC B");
            testProgram.AppendLine("RLC B");
            testProgram.AppendLine("RLC B");
            testProgram.AppendLine("RLC B");
            testProgram.AppendLine("RLC B");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(10);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_105_RLC_Register"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_106_RLC_Register16Address()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("LD (HL),1");
            testProgram.AppendLine("RLC (HL)");
            testProgram.AppendLine("LD A,(HL)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_106_RLC_Register16Address"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_107_RLC_IndexedAddress()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD IX,100");
            testProgram.AppendLine("LD (IX+1),1");
            testProgram.AppendLine("RLC (IX+1)");
            testProgram.AppendLine("LD A,(IX+1)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_107_RLC_IndexedAddress"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_108_RLC_IndexedAddress_Register()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD IY,100");
            testProgram.AppendLine("LD (IY+1),1");
            testProgram.AppendLine("RLC (IY+1),B");
            testProgram.AppendLine("LD A,(IY+1)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_108_RLC_IndexedAddress_Register"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_109_RLCA()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD A,00000001B");
            testProgram.AppendLine("RLCA");
            testProgram.AppendLine("RLCA");
            testProgram.AppendLine("RLCA");
            testProgram.AppendLine("RLCA");
            testProgram.AppendLine("RLCA");
            testProgram.AppendLine("RLCA");
            testProgram.AppendLine("RLCA");
            testProgram.AppendLine("RLCA");
            testProgram.AppendLine("RLCA");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(10);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_109_RLCA"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_110_RLD()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("LD (HL),81H");
            testProgram.AppendLine("RLD");
            testProgram.AppendLine("LD B,(HL)");
            testProgram.AppendLine("RLD");
            testProgram.AppendLine("LD B,(HL)");
            testProgram.AppendLine("RLD");
            testProgram.AppendLine("LD B,(HL)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(8);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_110_RLD"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_111_RR_Register()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD B,00000001B");
            testProgram.AppendLine("RR B");
            testProgram.AppendLine("RR B");
            testProgram.AppendLine("RR B");
            testProgram.AppendLine("RR B");
            testProgram.AppendLine("RR B");
            testProgram.AppendLine("RR B");
            testProgram.AppendLine("RR B");
            testProgram.AppendLine("RR B");
            testProgram.AppendLine("RR B");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(10);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_111_RR_Register"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_112_RR_Register16Address()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("LD (HL),1");
            testProgram.AppendLine("RR (HL)");
            testProgram.AppendLine("LD A,(HL)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_112_RR_Register16Address"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_113_RR_IndexedAddress()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD IX,100");
            testProgram.AppendLine("LD (IX+1),1");
            testProgram.AppendLine("RR (IX+1)");
            testProgram.AppendLine("LD A,(IX+1)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_113_RR_IndexedAddress"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_114_RR_IndexedAddress_Register()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD IY,100");
            testProgram.AppendLine("LD (IY+1),1");
            testProgram.AppendLine("RR (IY+1),B");
            testProgram.AppendLine("LD A,(IY+1)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_114_RR_IndexedAddress_Register"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_115_RRA()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD A,00000001B");
            testProgram.AppendLine("RRA");
            testProgram.AppendLine("RRA");
            testProgram.AppendLine("RRA");
            testProgram.AppendLine("RRA");
            testProgram.AppendLine("RRA");
            testProgram.AppendLine("RRA");
            testProgram.AppendLine("RRA");
            testProgram.AppendLine("RRA");
            testProgram.AppendLine("RRA");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(10);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_115_RRA"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_116_RRC_Register()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD B,00000001B");
            testProgram.AppendLine("RRC B");
            testProgram.AppendLine("RRC B");
            testProgram.AppendLine("RRC B");
            testProgram.AppendLine("RRC B");
            testProgram.AppendLine("RRC B");
            testProgram.AppendLine("RRC B");
            testProgram.AppendLine("RRC B");
            testProgram.AppendLine("RRC B");
            testProgram.AppendLine("RRC B");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(10);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_116_RRC_Register"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_117_RRC_Register16Address()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("LD (HL),80H");
            testProgram.AppendLine("RRC (HL)");
            testProgram.AppendLine("LD A,(HL)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_117_RRC_Register16Address"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_118_RRC_IndexedAddress()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD IX,100");
            testProgram.AppendLine("LD (IX+1),80H");
            testProgram.AppendLine("RRC (IX+1)");
            testProgram.AppendLine("LD A,(IX+1)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_118_RRC_IndexedAddress"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_119_RRC_IndexedAddress_Register()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD IY,100");
            testProgram.AppendLine("LD (IY+1),80H");
            testProgram.AppendLine("RRC (IY+1),B");
            testProgram.AppendLine("LD A,(IY+1)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_119_RRC_IndexedAddress_Register"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_120_RRCA()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD A,00000001B");
            testProgram.AppendLine("RRCA");
            testProgram.AppendLine("RRCA");
            testProgram.AppendLine("RRCA");
            testProgram.AppendLine("RRCA");
            testProgram.AppendLine("RRCA");
            testProgram.AppendLine("RRCA");
            testProgram.AppendLine("RRCA");
            testProgram.AppendLine("RRCA");
            testProgram.AppendLine("RRCA");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(10);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_120_RRCA"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_121_RRD()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("LD (HL),81H");
            testProgram.AppendLine("RRD");
            testProgram.AppendLine("LD B,(HL)");
            testProgram.AppendLine("RRD");
            testProgram.AppendLine("LD B,(HL)");
            testProgram.AppendLine("RRD");
            testProgram.AppendLine("LD B,(HL)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(8);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_121_RRD"))
            {
                throw new Exception("Log compare failed");
            }
        }
                
        public static void Check_Instruction_133_SLA_Register()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD B,10000001B");
            testProgram.AppendLine("SLA B");
            testProgram.AppendLine("SLA B");
            testProgram.AppendLine("SLA B");
            testProgram.AppendLine("SLA B");
            testProgram.AppendLine("SLA B");
            testProgram.AppendLine("SLA B");
            testProgram.AppendLine("SLA B");
            testProgram.AppendLine("SLA B");
            testProgram.AppendLine("SLA B");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(10);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_133_SLA_Register"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_134_SLA_Register16Address()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("LD (HL),10000001B");
            testProgram.AppendLine("SLA (HL)");
            testProgram.AppendLine("LD A,(HL)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_134_SLA_Register16Address"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_135_SLA_IndexedAddress()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD IX,100");
            testProgram.AppendLine("LD (IX+1),10000001B");
            testProgram.AppendLine("SLA (IX+1)");
            testProgram.AppendLine("LD A,(IX+1)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_135_SLA_IndexedAddress"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_136_SLA_IndexedAddress_Register()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD IY,100");
            testProgram.AppendLine("LD (IY+1),10000001B");
            testProgram.AppendLine("SLA (IY+1),B");
            testProgram.AppendLine("LD A,(IY+1)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_136_SLA_IndexedAddress_Register"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_137_SLL_Register()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD B,10000001B");
            testProgram.AppendLine("SLL B");
            testProgram.AppendLine("SLL B");
            testProgram.AppendLine("SLL B");
            testProgram.AppendLine("SLL B");
            testProgram.AppendLine("SLL B");
            testProgram.AppendLine("SLL B");
            testProgram.AppendLine("SLL B");
            testProgram.AppendLine("SLL B");
            testProgram.AppendLine("SLL B");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(10);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_137_SLL_Register"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_138_SLL_Register16Address()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("LD (HL),10000001B");
            testProgram.AppendLine("SLL (HL)");
            testProgram.AppendLine("LD A,(HL)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_138_SLL_Register16Address"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_139_SLL_IndexedAddress()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD IX,100");
            testProgram.AppendLine("LD (IX+1),10000001B");
            testProgram.AppendLine("SLL (IX+1)");
            testProgram.AppendLine("LD A,(IX+1)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_139_SLL_IndexedAddress"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_140_SLL_IndexedAddress_Register()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD IY,100");
            testProgram.AppendLine("LD (IY+1),10000001B");
            testProgram.AppendLine("SLL (IY+1),B");
            testProgram.AppendLine("LD A,(IY+1)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_140_SLL_IndexedAddress_Registe"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_141_SRA_Register()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD B,10000001B");
            testProgram.AppendLine("SRA B");
            testProgram.AppendLine("SRA B");
            testProgram.AppendLine("SRA B");
            testProgram.AppendLine("SRA B");
            testProgram.AppendLine("SRA B");
            testProgram.AppendLine("SRA B");
            testProgram.AppendLine("SRA B");
            testProgram.AppendLine("SRA B");
            testProgram.AppendLine("SRA B");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(10);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_141_SRA_Register"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_142_SRA_Register16Address()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("LD (HL),10000001B");
            testProgram.AppendLine("SRA (HL)");
            testProgram.AppendLine("LD A,(HL)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_142_SRA_Register16Address"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_143_SRA_IndexedAddress()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD IX,100");
            testProgram.AppendLine("LD (IX+1),10000001B");
            testProgram.AppendLine("SRA (IX+1)");
            testProgram.AppendLine("LD A,(IX+1)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_143_SRA_IndexedAddress"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_144_SRA_IndexedAddress_Register()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD IY,100");
            testProgram.AppendLine("LD (IY+1),10000001B");
            testProgram.AppendLine("SRA (IY+1),B");
            testProgram.AppendLine("LD A,(IY+1)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_144_SRA_IndexedAddress_Register"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_145_SRL_Register()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD B,10000001B");
            testProgram.AppendLine("SRL B");
            testProgram.AppendLine("SRL B");
            testProgram.AppendLine("SRL B");
            testProgram.AppendLine("SRL B");
            testProgram.AppendLine("SRL B");
            testProgram.AppendLine("SRL B");
            testProgram.AppendLine("SRL B");
            testProgram.AppendLine("SRL B");
            testProgram.AppendLine("SRL B");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(10);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_145_SRL_Register"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_146_SRL_Register16Address()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("LD (HL),10000001B");
            testProgram.AppendLine("SRL (HL)");
            testProgram.AppendLine("LD A,(HL)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_146_SRL_Register16Address"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_147_SRL_IndexedAddress()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD IX,100");
            testProgram.AppendLine("LD (IX+1),10000001B");
            testProgram.AppendLine("SRL (IX+1)");
            testProgram.AppendLine("LD A,(IX+1)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_147_SRL_IndexedAddress"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_148_SRL_IndexedAddress_Register()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD IY,100");
            testProgram.AppendLine("LD (IY+1),10000001B");
            testProgram.AppendLine("SRL (IY+1),B");
            testProgram.AppendLine("LD A,(IY+1)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/RotateAndShift/Logs/Check_Instruction_148_SRL_IndexedAddress_Register"))
            {
                throw new Exception("Log compare failed");
            }
        }
    }
}
