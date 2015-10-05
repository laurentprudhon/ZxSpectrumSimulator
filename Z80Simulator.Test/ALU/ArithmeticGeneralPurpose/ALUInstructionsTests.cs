using System;
using System.Text;
using Z80Simulator.CPU;
using Z80Simulator.Test.CPU;
using Z80Simulator.Test.System;

namespace Z80Simulator.Test.ALU
{
    public partial class ALUInstructionsTests
    {
        public static void Check_Instruction_20_CCF() 
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "1 (4)"
            testProgram.AppendLine("CCF");
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,0");
            // TStatesByMachineCycle = "1 (4)"
            testProgram.AppendLine("CCF");
            // TStatesByMachineCycle = "1 (4)"
            testProgram.AppendLine("CCF");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/ArithmeticGeneralPurpose/Logs/Check_Instruction_20_CCF"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_29_CPL() 
        {
            string sampleProgramFileName = "ALU/ArithmeticGeneralPurpose/Samples/testcpl.asm";
            string sampleResultFileName = "ALU/ArithmeticGeneralPurpose/Samples/resultcpl.csv";
            CompareExecutionResultsWithSample(sampleProgramFileName, sampleResultFileName, 2, 2);
        }

        public static void Check_Instruction_30_DAA() 
        {
            string sampleProgramFileName = "ALU/ArithmeticGeneralPurpose/Samples/testdaa.asm";
            string sampleResultFileName = "ALU/ArithmeticGeneralPurpose/Samples/resultdaa.csv";
            CompareExecutionResultsWithSample(sampleProgramFileName, sampleResultFileName, 3, 2);
        }

        public static void Check_Instruction_77_NEG() 
        {
            string sampleProgramFileName = "ALU/ArithmeticGeneralPurpose/Samples/testneg.asm";
            string sampleResultFileName = "ALU/ArithmeticGeneralPurpose/Samples/resultneg.csv";
            CompareExecutionResultsWithSample(sampleProgramFileName, sampleResultFileName, 2, 2);
        }

        public static void Check_Instruction_128_SCF()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "1 (4)"
            testProgram.AppendLine("CCF");
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,0");
            // TStatesByMachineCycle = "1 (4)"
            testProgram.AppendLine("SCF");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(3);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/ArithmeticGeneralPurpose/Logs/Check_Instruction_128_SCF"))
            {
                throw new Exception("Log compare failed");
            }
        }
    }
}
