using System;
using System.Text;
using Z80Simulator.CPU;
using Z80Simulator.Test.CPU;
using Z80Simulator.Test.System;

namespace Z80Simulator.Test.ALU
{
    public partial class ALUInstructionsTests
    {
        public static void Check_Instruction_15_BIT_Bit_Register()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD A,00000001B");
            testProgram.AppendLine("BIT 0,A");
            testProgram.AppendLine("LD B,10000000B");
            testProgram.AppendLine("BIT 7,B");
            testProgram.AppendLine("LD C,00100000B");
            testProgram.AppendLine("BIT 5,C");
            testProgram.AppendLine("LD D,00001000B");
            testProgram.AppendLine("BIT 3,D");
            testProgram.AppendLine("BIT 4,A");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(9);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/BitSetResetAndTest/Logs/Check_Instruction_15_BIT_Bit_Register"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_16_BIT_Bit_Register16Address()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD HL,10100000000000B");
            testProgram.AppendLine("LD BC,1");
            testProgram.AppendLine("ADD HL,BC");
            testProgram.AppendLine("LD (HL),00000001B");
            testProgram.AppendLine("BIT 0,(HL)");
            testProgram.AppendLine("LD (HL),10000000B");
            testProgram.AppendLine("BIT 7,(HL)");
            testProgram.AppendLine("BIT 4,(HL)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(8);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/BitSetResetAndTest/Logs/Check_Instruction_16_BIT_Bit_Register16Address"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_17_BIT_Bit_IndexedAddress()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD IX,10100000000000B");
            testProgram.AppendLine("LD (IX+1),00000001B");
            testProgram.AppendLine("BIT 0,(IX+1)");
            testProgram.AppendLine("LD (IX+1),10000000B");
            testProgram.AppendLine("BIT 7,(IX+1)");
            testProgram.AppendLine("BIT 4,(IX+1)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(6);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/BitSetResetAndTest/Logs/Check_Instruction_17_BIT_Bit_IndexedAddress"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_92_RES_Bit_Register()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD B,0FFH");
            testProgram.AppendLine("RES 4,B");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(2);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/BitSetResetAndTest/Logs/Check_Instruction_92_RES_Bit_Register"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_93_RES_Bit_Register16Address()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("LD (HL),0FFH");
            testProgram.AppendLine("RES 2,(HL)");
            testProgram.AppendLine("LD A,(HL)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/BitSetResetAndTest/Logs/Check_Instruction_93_RES_Bit_Register16Address"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_94_RES_Bit_IndexedAddress()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD IX,100");
            testProgram.AppendLine("LD (IX+1),0FFH");
            testProgram.AppendLine("RES 6,(IX+1)");
            testProgram.AppendLine("LD A,(IX+1)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/BitSetResetAndTest/Logs/Check_Instruction_94_RES_Bit_IndexedAddress"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_95_RES_Bit_IndexedAddress_Register()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD IX,100");
            testProgram.AppendLine("LD (IX+1),0FFH");
            testProgram.AppendLine("RES 6,(IX+1),B");
            testProgram.AppendLine("LD A,(IX+1)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/BitSetResetAndTest/Logs/Check_Instruction_95_RES_Bit_IndexedAddress_Register"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_129_SET_Bit_Register()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD A,0");
            testProgram.AppendLine("SET 3,A");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(2);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/BitSetResetAndTest/Logs/Check_Instruction_129_SET_Bit_Register"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_130_SET_Bit_Register16Address()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("LD (HL),0");
            testProgram.AppendLine("SET 7,(HL)");
            testProgram.AppendLine("LD A,(HL)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/BitSetResetAndTest/Logs/Check_Instruction_130_SET_Bit_Register16Address"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_131_SET_Bit_IndexedAddress()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD IY,100");
            testProgram.AppendLine("LD (IY+1),0");
            testProgram.AppendLine("SET 7,(IY+1)");
            testProgram.AppendLine("LD A,(IY+1)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/BitSetResetAndTest/Logs/Check_Instruction_131_SET_Bit_IndexedAddress"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_132_SET_Bit_IndexedAddress_Register()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD IY,100");
            testProgram.AppendLine("LD (IY+1),0");
            testProgram.AppendLine("SET 7,(IY+1),C");
            testProgram.AppendLine("LD A,(IY+1)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/BitSetResetAndTest/Logs/Check_Instruction_132_SET_Bit_IndexedAddress_Register"))
            {
                throw new Exception("Log compare failed");
            }
        }
    }
}
