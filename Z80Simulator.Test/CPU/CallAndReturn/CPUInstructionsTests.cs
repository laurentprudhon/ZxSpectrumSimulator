using System;
using System.Text;
using Z80Simulator.CPU;
using Z80Simulator.Test.System;

namespace Z80Simulator.Test.CPU
{
    public partial class CPUInstructionsTests
    {
        public static void Check_Instruction_18_CALL_Address()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD A,0");
            testProgram.AppendLine("CALL FUNC");
            testProgram.AppendLine("LD A,2");
            testProgram.AppendLine("ORG 100");
            testProgram.AppendLine("FUNC: LD A,1");
            testProgram.AppendLine("RET");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(5);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/CallAndReturn/Logs/Check_Instruction_18_CALL_Address"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_19_CALL_FlagCondition_Address()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD A,0");
            testProgram.AppendLine("CALL Z,FUNC");
            testProgram.AppendLine("LD A,2");
            testProgram.AppendLine("ORG 100");
            testProgram.AppendLine("FUNC: LD A,1");
            testProgram.AppendLine("CALL NZ,FUNC");
            testProgram.AppendLine("RET");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(6);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/CallAndReturn/Logs/Check_Instruction_19_CALL_FlagCondition_Address"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_96_RET()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD A,0");
            testProgram.AppendLine("CALL FUNC1");
            testProgram.AppendLine("LD A,4");
            testProgram.AppendLine("ORG 100");
            testProgram.AppendLine("FUNC1: LD A,1");
            testProgram.AppendLine("CALL FUNC2");
            testProgram.AppendLine("LD A,3");
            testProgram.AppendLine("RET");
            testProgram.AppendLine("ORG 200");
            testProgram.AppendLine("FUNC2: LD A,2");
            testProgram.AppendLine("RET");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(9);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/CallAndReturn/Logs/Check_Instruction_96_RET"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_97_RET_FlagCondition()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD A,0");
            testProgram.AppendLine("CALL Z,FUNC");
            testProgram.AppendLine("LD A,2");
            testProgram.AppendLine("ORG 100");
            testProgram.AppendLine("FUNC: LD A,1");
            testProgram.AppendLine("RET NZ");
            testProgram.AppendLine("RET Z");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(6);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/CallAndReturn/Logs/Check_Instruction_97_RET_FlagCondition"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_98_RETI()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD A,0");
            testProgram.AppendLine("CALL FUNC");
            testProgram.AppendLine("LD A,2");
            testProgram.AppendLine("ORG 100");
            testProgram.AppendLine("FUNC: LD A,1");
            testProgram.AppendLine("RETI");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(5);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/CallAndReturn/Logs/Check_Instruction_98_RETI"))
            {
                throw new Exception("Log compare failed");
            }
        }
 
        public static void Check_Instruction_99_RETN()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD A,0");
            testProgram.AppendLine("CALL FUNC");
            testProgram.AppendLine("LD A,2");
            testProgram.AppendLine("ORG 100");
            testProgram.AppendLine("FUNC: LD A,1");
            testProgram.AppendLine("RETN");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(5);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/CallAndReturn/Logs/Check_Instruction_99_RETN"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_122_RST_ResetAddress()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD A,0");
            testProgram.AppendLine("RST 08H");
            testProgram.AppendLine("ORG 08H");
            testProgram.AppendLine("LD A,1");
            testProgram.AppendLine("RST 10H");
            testProgram.AppendLine("ORG 10H");
            testProgram.AppendLine("LD A,2");
            testProgram.AppendLine("RST 18H");
            testProgram.AppendLine("ORG 18H");
            testProgram.AppendLine("LD A,3");
            testProgram.AppendLine("RST 20H");
            testProgram.AppendLine("ORG 20H");
            testProgram.AppendLine("LD A,4");
            testProgram.AppendLine("RST 28H");
            testProgram.AppendLine("ORG 28H");
            testProgram.AppendLine("LD A,5");
            testProgram.AppendLine("RST 30H");
            testProgram.AppendLine("ORG 30H");
            testProgram.AppendLine("LD A,6");
            testProgram.AppendLine("RST 38H");
            testProgram.AppendLine("ORG 38H");
            testProgram.AppendLine("LD A,7");
            testProgram.AppendLine("RST 00H");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(17);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/CallAndReturn/Logs/Check_Instruction_122_RST_ResetAddress"))
            {
                throw new Exception("Log compare failed");
            }
        }
    }
}
