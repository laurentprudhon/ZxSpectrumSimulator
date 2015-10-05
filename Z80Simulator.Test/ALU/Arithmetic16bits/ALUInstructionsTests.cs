using System;
using System.Text;
using Z80Simulator.CPU;
using Z80Simulator.Test.CPU;
using Z80Simulator.Test.System;

namespace Z80Simulator.Test.ALU
{
    public partial class ALUInstructionsTests
    {        
        public static void Check_Instruction_5_ADC_Register16_Register16() 
        {
            string sampleProgramFileName = "ALU/Arithmetic16bits/Samples/testadc16.asm";
            string sampleResultFileName = "ALU/Arithmetic16bits/Samples/resultadc16.csv";
            Compare16bitsExecutionResultsWithSample(sampleProgramFileName, sampleResultFileName, 4);

            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "10 (4,3,3)"
            testProgram.AppendLine("LD BC,1");
            // TStatesByMachineCycle = "10 (4,3,3)"
            testProgram.AppendLine("LD DE,2");
            // TStatesByMachineCycle = "10 (4,3,3)"
            testProgram.AppendLine("LD HL,3");
            // TStatesByMachineCycle = "10 (4,3,3)"
            testProgram.AppendLine("LD SP,4");
            // TStatesByMachineCycle = "4"
            testProgram.AppendLine("SCF");
            // TStatesByMachineCycle = "11 (4,4,3)"
            testProgram.AppendLine("ADC HL,BC");
            // TStatesByMachineCycle = "4"
            testProgram.AppendLine("SCF");
            // TStatesByMachineCycle = "11 (4,4,3)"
            testProgram.AppendLine("ADC HL,DE");
            // TStatesByMachineCycle = "4"
            testProgram.AppendLine("SCF");
            // TStatesByMachineCycle = "11 (4,4,3)"
            testProgram.AppendLine("ADC HL,HL");
            // TStatesByMachineCycle = "4"
            testProgram.AppendLine("SCF");
            // TStatesByMachineCycle = "11 (4,4,3)"
            testProgram.AppendLine("ADC HL,SP");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(12);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/Arithmetic16bits/Logs/Check_Instruction_5_ADC_Register16_Register16"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_10_ADD_Register16_Register16() 
        {
            string sampleProgramFileName = "ALU/Arithmetic16bits/Samples/testadd16.asm";
            string sampleResultFileName = "ALU/Arithmetic16bits/Samples/resultadd16.csv";
            Compare16bitsExecutionResultsWithSample(sampleProgramFileName, sampleResultFileName, 3);

            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "10 (4,3,3)"
            testProgram.AppendLine("LD BC,1");
            // TStatesByMachineCycle = "10 (4,3,3)"
            testProgram.AppendLine("LD DE,2");
            // TStatesByMachineCycle = "10 (4,3,3)"
            testProgram.AppendLine("LD HL,3");
            // TStatesByMachineCycle = "10 (4,3,3)"
            testProgram.AppendLine("LD SP,4");
            // TStatesByMachineCycle = "4"
            testProgram.AppendLine("SCF");
            // TStatesByMachineCycle = "11 (4,4,3)"
            testProgram.AppendLine("ADD HL,BC");
            // TStatesByMachineCycle = "4"
            testProgram.AppendLine("SCF");
            // TStatesByMachineCycle = "11 (4,4,3)"
            testProgram.AppendLine("ADD HL,DE");
            // TStatesByMachineCycle = "4"
            testProgram.AppendLine("SCF");
            // TStatesByMachineCycle = "11 (4,4,3)"
            testProgram.AppendLine("ADD HL,HL");
            // TStatesByMachineCycle = "4"
            testProgram.AppendLine("SCF");
            // TStatesByMachineCycle = "11 (4,4,3)"
            testProgram.AppendLine("ADD HL,SP");

            testProgram.AppendLine("LD IX,3");
            testProgram.AppendLine("SCF");
            testProgram.AppendLine("ADD IX,BC");
            testProgram.AppendLine("SCF");
            testProgram.AppendLine("ADD IX,DE");
            testProgram.AppendLine("SCF");
            testProgram.AppendLine("ADD IX,IX");
            testProgram.AppendLine("SCF");
            testProgram.AppendLine("ADD IX,SP");

            testProgram.AppendLine("LD IY,3");
            testProgram.AppendLine("SCF");
            testProgram.AppendLine("ADD IY,BC");
            testProgram.AppendLine("SCF");
            testProgram.AppendLine("ADD IY,DE");
            testProgram.AppendLine("SCF");
            testProgram.AppendLine("ADD IY,IY");
            testProgram.AppendLine("SCF");
            testProgram.AppendLine("ADD IY,SP");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(30);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/Arithmetic16bits/Logs/Check_Instruction_10_ADD_Register16_Register16"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_34_DEC_Register16() 
        {
            string sampleProgramFileName = "ALU/Arithmetic16bits/Samples/testdec16.asm";
            string sampleResultFileName = "ALU/Arithmetic16bits/Samples/resultdec16.csv";
            Compare16bitsExecutionResultsWithSample(sampleProgramFileName, sampleResultFileName, 2);

            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "10 (4,3,3)"
            testProgram.AppendLine("LD BC,1");
            // TStatesByMachineCycle = "10 (4,3,3)"
            testProgram.AppendLine("LD DE,2");
            // TStatesByMachineCycle = "10 (4,3,3)"
            testProgram.AppendLine("LD HL,3");
            // TStatesByMachineCycle = "10 (4,3,3)"
            testProgram.AppendLine("LD SP,4");
            // TStatesByMachineCycle = "14 (4,4,3,3)"
            testProgram.AppendLine("LD IX,5");
            // TStatesByMachineCycle = "14 (4,4,3,3)"
            testProgram.AppendLine("LD IY,6");
            // TStatesByMachineCycle = "6"
            testProgram.AppendLine("DEC BC");
            // TStatesByMachineCycle = "6"
            testProgram.AppendLine("DEC DE");
            // TStatesByMachineCycle = "6"
            testProgram.AppendLine("DEC HL");
            // TStatesByMachineCycle = "6"
            testProgram.AppendLine("DEC SP");
            // TStatesByMachineCycle = "10 (4,6)"
            testProgram.AppendLine("DEC IX");
            // TStatesByMachineCycle = "10 (4,6)"
            testProgram.AppendLine("DEC IY");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(12);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/Arithmetic16bits/Logs/Check_Instruction_34_DEC_Register16"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_49_INC_Register16() 
        {
            string sampleProgramFileName = "ALU/Arithmetic16bits/Samples/testinc16.asm";
            string sampleResultFileName = "ALU/Arithmetic16bits/Samples/resultinc16.csv";
            Compare16bitsExecutionResultsWithSample(sampleProgramFileName, sampleResultFileName, 2);

            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "10 (4,3,3)"
            testProgram.AppendLine("LD BC,1");
            // TStatesByMachineCycle = "10 (4,3,3)"
            testProgram.AppendLine("LD DE,2");
            // TStatesByMachineCycle = "10 (4,3,3)"
            testProgram.AppendLine("LD HL,3");
            // TStatesByMachineCycle = "10 (4,3,3)"
            testProgram.AppendLine("LD SP,4");
            // TStatesByMachineCycle = "14 (4,4,3,3)"
            testProgram.AppendLine("LD IX,5");
            // TStatesByMachineCycle = "14 (4,4,3,3)"
            testProgram.AppendLine("LD IY,6");
            // TStatesByMachineCycle = "6"
            testProgram.AppendLine("INC BC");
            // TStatesByMachineCycle = "6"
            testProgram.AppendLine("INC DE");
            // TStatesByMachineCycle = "6"
            testProgram.AppendLine("INC HL");
            // TStatesByMachineCycle = "6"
            testProgram.AppendLine("INC SP");
            // TStatesByMachineCycle = "10 (4,6)"
            testProgram.AppendLine("INC IX");
            // TStatesByMachineCycle = "10 (4,6)"
            testProgram.AppendLine("INC IY");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(12);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/Arithmetic16bits/Logs/Check_Instruction_49_INC_Register16"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_127_SBC_Register16_Register16()
        {
            string sampleProgramFileName = "ALU/Arithmetic16bits/Samples/testsbc16.asm";
            string sampleResultFileName = "ALU/Arithmetic16bits/Samples/resultsbc16.csv";
            Compare16bitsExecutionResultsWithSample(sampleProgramFileName, sampleResultFileName, 4);

            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "10 (4,3,3)"
            testProgram.AppendLine("LD BC,1");
            // TStatesByMachineCycle = "10 (4,3,3)"
            testProgram.AppendLine("LD DE,2");
            // TStatesByMachineCycle = "10 (4,3,3)"
            testProgram.AppendLine("LD HL,10");
            // TStatesByMachineCycle = "10 (4,3,3)"
            testProgram.AppendLine("LD SP,4");
            // TStatesByMachineCycle = "4"
            testProgram.AppendLine("SCF");
            // TStatesByMachineCycle = "11 (4,4,3)"
            testProgram.AppendLine("SBC HL,BC");
            // TStatesByMachineCycle = "4"
            testProgram.AppendLine("SCF");
            // TStatesByMachineCycle = "11 (4,4,3)"
            testProgram.AppendLine("SBC HL,DE");
            // TStatesByMachineCycle = "4"
            testProgram.AppendLine("SCF");
            // TStatesByMachineCycle = "11 (4,4,3)"
            testProgram.AppendLine("SBC HL,HL");
            // TStatesByMachineCycle = "4"
            testProgram.AppendLine("SCF");
            // TStatesByMachineCycle = "11 (4,4,3)"
            testProgram.AppendLine("SBC HL,SP");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(12);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/Arithmetic16bits/Logs/Check_Instruction_127_SBC_Register16_Register16"))
            {
                throw new Exception("Log compare failed");
            }
        }
    }
}
