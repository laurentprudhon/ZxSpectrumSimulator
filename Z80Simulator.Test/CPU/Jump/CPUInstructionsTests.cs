using System;
using System.Text;
using Z80Simulator.CPU;
using Z80Simulator.Test.System;

namespace Z80Simulator.Test.CPU
{
    public partial class CPUInstructionsTests
    {
        public static void Check_Instruction_36_DJNZ_RelativeDisplacement()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD A,0");
            testProgram.AppendLine("LD B,3");
            testProgram.AppendLine("INC A");
            testProgram.AppendLine("DJNZ -1");
            testProgram.AppendLine("LD A,0FFH");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(9);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/Jump/Logs/Check_Instruction_36_DJNZ_RelativeDisplacement"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_54_JP_Address()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD A,0");
            testProgram.AppendLine("JP 100");
            testProgram.AppendLine("ORG 100");
            testProgram.AppendLine("LD A,1");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(3);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/Jump/Logs/Check_Instruction_54_JP_Address"))
            {
                throw new Exception("Log compare failed");
            }
        }
        
        public static void Check_Instruction_55_JP_Register16Address()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD A,0");
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("JP (HL)");
            testProgram.AppendLine("ORG 100");
            testProgram.AppendLine("LD IX,110");
            testProgram.AppendLine("JP (IX)");
            testProgram.AppendLine("ORG 110");
            testProgram.AppendLine("LD IY,120");
            testProgram.AppendLine("JP (IY)");
            testProgram.AppendLine("ORG 120");
            testProgram.AppendLine("LD A,1");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(8);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/Jump/Logs/Check_Instruction_55_JP_Register16Address"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_56_JP_FlagCondition_Address()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD A,0");
            testProgram.AppendLine("JP M,100");
            testProgram.AppendLine("ORG 100");
            testProgram.AppendLine("JP P,110");
            testProgram.AppendLine("LD A,1");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/Jump/Logs/Check_Instruction_56_JP_FlagCondition_Address"))
            {
                throw new Exception("Log compare failed");
            }
        }                       

        public static void Check_Instruction_57_JR_RelativeDisplacement()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("NEAR: LD A,0");
            // TStatesByMachineCycle = "12 (4, 3, 5)"
            testProgram.AppendLine("JR FAR");

            testProgram.AppendLine("ORG 100");
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("FAR: LD A,1");
            // TStatesByMachineCycle = "12 (4, 3, 5)"
            testProgram.AppendLine("JR NEAR");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(9);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/Jump/Logs/Check_Instruction_57_JR_RelativeDisplacement"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_58_JR_FlagCondition_RelativeDisplacement()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD A,0");
            testProgram.AppendLine("JR Z,NEAR");
            testProgram.AppendLine("ORG 10");
            testProgram.AppendLine("NEAR: JR NZ,FAR");
            testProgram.AppendLine("LD A,1");
            testProgram.AppendLine("ORG 100");
            testProgram.AppendLine("FAR: LD A,2");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/Jump/Logs/Check_Instruction_58_JR_FlagCondition_RelativeDisplacement"))
            {
                throw new Exception("Log compare failed");
            }
        }
    }
}
