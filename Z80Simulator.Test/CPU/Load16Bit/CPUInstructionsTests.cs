using System;
using System.Text;
using Z80Simulator.CPU;
using Z80Simulator.Test.System;

namespace Z80Simulator.Test.CPU
{
    public partial class CPUInstructionsTests
    {
        public static void Check_Instruction_64_LD_Register16_Number16() 
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "10 (4, 3, 3)"
            testProgram.AppendLine("LD BC,1280");
            // TStatesByMachineCycle = "10 (4, 3, 3)"
            testProgram.AppendLine("LD SP,4163");
            // TStatesByMachineCycle = "14 (4, 4, 3, 3)"
            testProgram.AppendLine("LD IX,9654");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(3);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/Load16Bit/Logs/Check_Instruction_64_LD_Register16_Number16"))
            {
                throw new Exception("Log compare failed");
            }
        }
        
        public static void Check_Instruction_65_LD_Register16_Register16() 
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "10 (4, 3, 3)"
            testProgram.AppendLine("LD HL,1547");
            // TStatesByMachineCycle = "1 (6)"
            testProgram.AppendLine("LD SP,HL");
            // TStatesByMachineCycle = "14 (4, 4, 3, 3)"
            testProgram.AppendLine("LD IY,3225");
            // TStatesByMachineCycle = "10 (4, 6)"
            testProgram.AppendLine("LD SP,IY");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/Load16Bit/Logs/Check_Instruction_65_LD_Register16_Register16"))
            {
                throw new Exception("Log compare failed");
            }
        }
        
        public static void Check_Instruction_66_LD_Register16_Address() 
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,12");
            // TStatesByMachineCycle = "13 (4, 3, 3, 3)"
            testProgram.AppendLine("LD (4253),A");
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,120");
            // TStatesByMachineCycle = "13 (4, 3, 3, 3)"
            testProgram.AppendLine("LD (4254),A");
            // TStatesByMachineCycle = "16 (4, 3, 3, 3, 3)"
            testProgram.AppendLine("LD HL,(4253)");
            // TStatesByMachineCycle = "20 (4, 4, 3, 3, 3, 3)"
            testProgram.AppendLine("LD DE,(4253)");
            // TStatesByMachineCycle = "20 (4, 4, 3, 3, 3, 3)"
            testProgram.AppendLine("LD IX,(4253)");
            // TStatesByMachineCycle = "20 (4, 4, 3, 3, 3, 3)"
            testProgram.AppendLine("LD SP,(4253)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(8);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/Load16Bit/Logs/Check_Instruction_66_LD_Register16_Address"))
            {
                throw new Exception("Log compare failed");
            }
        }
        
        public static void Check_Instruction_68_LD_Address_Register16() 
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "10 (4, 3, 3)"
            testProgram.AppendLine("LD HL,4163");
            // TStatesByMachineCycle = "10 (4, 3, 3)"
            testProgram.AppendLine("LD BC,4164");
            // TStatesByMachineCycle = "14 (4, 4, 3, 3)"
            testProgram.AppendLine("LD IY,4165");
            // TStatesByMachineCycle = "10 (4, 3, 3)"
            testProgram.AppendLine("LD SP,4166");
            // TStatesByMachineCycle = "16 (4, 3, 3, 3, 3)"
            testProgram.AppendLine("LD (4253),HL");
            // TStatesByMachineCycle = "20 (4, 4, 3, 3, 3, 3)"
            testProgram.AppendLine("LD DE,(4253)");
            // TStatesByMachineCycle = "20 (4, 4, 3, 3, 3, 3)"
            testProgram.AppendLine("LD (4253),BC");
            // TStatesByMachineCycle = "20 (4, 4, 3, 3, 3, 3)"
            testProgram.AppendLine("LD DE,(4253)");
            // TStatesByMachineCycle = "20 (4, 4, 3, 3, 3, 3)"
            testProgram.AppendLine("LD (4253),IY");
            // TStatesByMachineCycle = "20 (4, 4, 3, 3, 3, 3)"
            testProgram.AppendLine("LD DE,(4253)");
            // TStatesByMachineCycle = "20 (4, 4, 3, 3, 3, 3)"
            testProgram.AppendLine("LD (4253),SP");
            // TStatesByMachineCycle = "20 (4, 4, 3, 3, 3, 3)"
            testProgram.AppendLine("LD DE,(4253)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(12);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/Load16Bit/Logs/Check_Instruction_68_LD_Address_Register16"))
            {
                throw new Exception("Log compare failed");
            }
        }
                
        public static void Check_Instruction_90_POP_Register16() 
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,17");
            // TStatesByMachineCycle = "13 (4, 3, 3, 3)"
            testProgram.AppendLine("LD (4253),A");
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,18");
            // TStatesByMachineCycle = "13 (4, 3, 3, 3)"
            testProgram.AppendLine("LD (4252),A");
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,19");
            // TStatesByMachineCycle = "13 (4, 3, 3, 3)"
            testProgram.AppendLine("LD (4251),A");
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,20");
            // TStatesByMachineCycle = "13 (4, 3, 3, 3)"
            testProgram.AppendLine("LD (4250),A");
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,21");
            // TStatesByMachineCycle = "13 (4, 3, 3, 3)"
            testProgram.AppendLine("LD (4249),A");
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,22");
            // TStatesByMachineCycle = "13 (4, 3, 3, 3)"
            testProgram.AppendLine("LD (4248),A");
            // TStatesByMachineCycle = "10 (4, 3, 3)"
            testProgram.AppendLine("LD SP,4248");
            // TStatesByMachineCycle = "10 (4, 3, 3)",
            testProgram.AppendLine("POP AF");
            // TStatesByMachineCycle = "10 (4, 3, 3)",
            testProgram.AppendLine("POP HL");
            // TStatesByMachineCycle = "14 (4, 4, 3, 3)"
            testProgram.AppendLine("POP IX");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(16);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/Load16Bit/Logs/Check_Instruction_90_POP_Register16"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_91_PUSH_Register16()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,21");
            // TStatesByMachineCycle = "10 (4, 3, 3)"
            testProgram.AppendLine("LD BC,4370");
            // TStatesByMachineCycle = "14 (4, 4, 3, 3)"
            testProgram.AppendLine("LD IY,4884");
            // TStatesByMachineCycle = "11 (5, 3, 3)"
            testProgram.AppendLine("PUSH AF");
            // TStatesByMachineCycle = "11 (5, 3, 3)"
            testProgram.AppendLine("PUSH BC");
            // TStatesByMachineCycle = "15 (4, 5, 3, 3)"
            testProgram.AppendLine("PUSH IY");
            // TStatesByMachineCycle = "10 (4, 3, 3)",
            testProgram.AppendLine("POP DE");
            // TStatesByMachineCycle = "10 (4, 3, 3)",
            testProgram.AppendLine("POP DE");
            // TStatesByMachineCycle = "10 (4, 3, 3)",
            testProgram.AppendLine("POP DE");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(9);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/Load16Bit/Logs/Check_Instruction_91_PUSH_Register16"))
            {
                throw new Exception("Log compare failed");
            }
        }
    }
}
