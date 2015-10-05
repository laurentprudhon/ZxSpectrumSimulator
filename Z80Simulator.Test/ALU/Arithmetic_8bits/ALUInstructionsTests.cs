using System;
using System.Text;
using Z80Simulator.CPU;
using Z80Simulator.Test.CPU;
using Z80Simulator.Test.System;

namespace Z80Simulator.Test.ALU
{
    public partial class ALUInstructionsTests
    {
        public static void Check_Instruction_1_ADC_Register_Number8()
        {
            string sampleProgramFileName = "ALU/Arithmetic_8bits/Samples/testadc.asm";
            string sampleResultFileName = "ALU/Arithmetic_8bits/Samples/resultadc.csv";
            CompareExecutionResultsWithSample(sampleProgramFileName, sampleResultFileName, 3, 3);
        }

        public static void Check_Instruction_2_ADC_Register_Register() 
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,5");
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD B,10");
            // TStatesByMachineCycle = 1 (4)
            testProgram.AppendLine("SCF");
            // TStatesByMachineCycle = 1 (4)
            testProgram.AppendLine("ADC A,B");
            // TStatesByMachineCycle = "11 (4,4,3)"
            testProgram.AppendLine("LD IXh,5");
            // TStatesByMachineCycle = 1 (4)
            testProgram.AppendLine("SCF");
            // TStatesByMachineCycle = 8 (4, 4)
            testProgram.AppendLine("ADC A,IXh");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(7);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/Arithmetic_8bits/Logs/Check_Instruction_2_ADC_Register_Register"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_3_ADC_Register_Register16Address() 
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,5");
            // TStatesByMachineCycle = "10 (4, 3, 3)"
            testProgram.AppendLine("LD HL,100");
            // TStatesByMachineCycle = "10 (4, 3, 3)"
            testProgram.AppendLine("LD (HL),10");
            // TStatesByMachineCycle = 1 (4)
            testProgram.AppendLine("SCF");
            // TStatesByMachineCycle = 7 (4, 3)
            testProgram.AppendLine("ADC A,(HL)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(5);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/Arithmetic_8bits/Logs/Check_Instruction_3_ADC_Register_Register16Address"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_4_ADC_Register_IndexedAddress()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,5");
            // TStatesByMachineCycle = "14 (4, 4, 3, 3)"
            testProgram.AppendLine("LD IX,97");
            // TStatesByMachineCycle = "19 (4, 4, 3,5,3)"
            testProgram.AppendLine("LD (IX+3),10");
            // TStatesByMachineCycle = 1 (4)
            testProgram.AppendLine("SCF");
            // TStatesByMachineCycle = "19 (4, 4, 3, 5, 3)"
            testProgram.AppendLine("ADC A,(IX+3)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(5);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/Arithmetic_8bits/Logs/Check_Instruction_4_ADC_Register_IndexedAddress"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_6_ADD_Register_Number8() 
        {
            string sampleProgramFileName = "ALU/Arithmetic_8bits/Samples/testadd.asm";
            string sampleResultFileName = "ALU/Arithmetic_8bits/Samples/resultadd.csv";
            CompareExecutionResultsWithSample(sampleProgramFileName, sampleResultFileName, 2, 3);
        }

        public static void Check_Instruction_7_ADD_Register_Register()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,5");
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD B,252");
            // TStatesByMachineCycle = 1 (4)
            testProgram.AppendLine("ADD A,B");
            // TStatesByMachineCycle = "11 (4,4,3)"
            testProgram.AppendLine("LD IXh,13");
            // TStatesByMachineCycle = "8 (4, 4)"
            testProgram.AppendLine("ADD A,IXh");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(5);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/Arithmetic_8bits/Logs/Check_Instruction_7_ADD_Register_Register"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_8_ADD_Register_Register16Address()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,5");
            // TStatesByMachineCycle = "10 (4, 3, 3)"
            testProgram.AppendLine("LD HL,100");
            // TStatesByMachineCycle = "10 (4, 3, 3)"
            testProgram.AppendLine("LD (HL),10"); ;
            // TStatesByMachineCycle = "7 (4, 3)"
            testProgram.AppendLine("ADD A,(HL)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/Arithmetic_8bits/Logs/Check_Instruction_8_ADD_Register_Register16Address"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_9_ADD_Register_IndexedAddress()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,5");
            // TStatesByMachineCycle = "14 (4, 4, 3, 3)"
            testProgram.AppendLine("LD IX,97");
            // TStatesByMachineCycle = "19 (4, 4, 3,5,3)"
            testProgram.AppendLine("LD (IX+3),10");
            // TStatesByMachineCycle = "19 (4, 4, 3, 5, 3)"
            testProgram.AppendLine("ADD A,(IX+3)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/Arithmetic_8bits/Logs/Check_Instruction_9_ADD_Register_IndexedAddress"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_11_AND_Number8() 
        {
            string sampleProgramFileName = "ALU/Arithmetic_8bits/Samples/testand.asm";
            string sampleResultFileName = "ALU/Arithmetic_8bits/Samples/resultand.csv";
            CompareExecutionResultsWithSample(sampleProgramFileName, sampleResultFileName, 2, 3);
        }

        public static void Check_Instruction_12_AND_Register()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,10101010B");
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD B,10001000B");
            // TStatesByMachineCycle = 1 (4)
            testProgram.AppendLine("AND B");
            // TStatesByMachineCycle = "11 (4,4,3)"
            testProgram.AppendLine("LD IXh,1000B");
            // TStatesByMachineCycle = "8 (4, 4)"
            testProgram.AppendLine("AND IXh");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(5);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/Arithmetic_8bits/Logs/Check_Instruction_12_AND_Register"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_13_AND_Register16Address()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,10101010B");
            // TStatesByMachineCycle = "10 (4, 3, 3)"
            testProgram.AppendLine("LD HL,100");
            // TStatesByMachineCycle = "10 (4, 3, 3)"
            testProgram.AppendLine("LD (HL),10001000B"); ;
            // TStatesByMachineCycle = "7 (4, 3)"
            testProgram.AppendLine("AND (HL)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/Arithmetic_8bits/Logs/Check_Instruction_13_AND_Register16Address"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_14_AND_IndexedAddress()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,10101010B");
            // TStatesByMachineCycle = "14 (4, 4, 3, 3)"
            testProgram.AppendLine("LD IX,97");
            // TStatesByMachineCycle = "19 (4, 4, 3,5,3)"
            testProgram.AppendLine("LD (IX+3),10001000B");
            // TStatesByMachineCycle = "19 (4, 4, 3, 5, 3)"
            testProgram.AppendLine("AND (IX+3)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/Arithmetic_8bits/Logs/Check_Instruction_14_AND_IndexedAddress"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_21_CP_Number8()
        {
            string sampleProgramFileName = "ALU/Arithmetic_8bits/Samples/testcp.asm";
            string sampleResultFileName = "ALU/Arithmetic_8bits/Samples/resultcp.csv";
            CompareExecutionResultsWithSample(sampleProgramFileName, sampleResultFileName, 2, 3);
        }

        public static void Check_Instruction_22_CP_Register()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,1");
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD B,2");
            // TStatesByMachineCycle = 1 (4)
            testProgram.AppendLine("CP B");
            // TStatesByMachineCycle = "11 (4,4,3)"
            testProgram.AppendLine("LD IXh,1");
            // TStatesByMachineCycle = "8 (4, 4)"
            testProgram.AppendLine("CP IXh");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(5);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/Arithmetic_8bits/Logs/Check_Instruction_22_CP_Register"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_23_CP_Register16Address()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,1");
            // TStatesByMachineCycle = "10 (4, 3, 3)"
            testProgram.AppendLine("LD HL,100");
            // TStatesByMachineCycle = "10 (4, 3, 3)"
            testProgram.AppendLine("LD (HL),1"); ;
            // TStatesByMachineCycle = "7 (4, 3)"
            testProgram.AppendLine("CP (HL)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/Arithmetic_8bits/Logs/Check_Instruction_23_CP_Register16Address"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_24_CP_IndexedAddress()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,1");
            // TStatesByMachineCycle = "14 (4, 4, 3, 3)"
            testProgram.AppendLine("LD IX,97");
            // TStatesByMachineCycle = "19 (4, 4, 3,5,3)"
            testProgram.AppendLine("LD (IX+3),1");
            // TStatesByMachineCycle = "19 (4, 4, 3, 5, 3)"
            testProgram.AppendLine("CP (IX+3)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/Arithmetic_8bits/Logs/Check_Instruction_24_CP_IndexedAddress"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_31_DEC_Register()
        {
            string sampleProgramFileName = "ALU/Arithmetic_8bits/Samples/testdec.asm";
            string sampleResultFileName = "ALU/Arithmetic_8bits/Samples/resultdec.csv";
            CompareExecutionResultsWithSample(sampleProgramFileName, sampleResultFileName, 2, 3);
        }

        public static void Check_Instruction_32_DEC_Register16Address()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "10 (4, 3, 3)"
            testProgram.AppendLine("LD HL,100");
            // TStatesByMachineCycle = "10 (4, 3, 3)"
            testProgram.AppendLine("LD (HL),4"); ;
            // TStatesByMachineCycle = "11 (4, 4, 3)"
            testProgram.AppendLine("DEC (HL)");
            // TStatesByMachineCycle = "13 (4, 3, 3, 3)"
            testProgram.AppendLine("LD A,(100)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/Arithmetic_8bits/Logs/Check_Instruction_32_DEC_Register16Address"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_33_DEC_IndexedAddress()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "14 (4, 4, 3, 3)"
            testProgram.AppendLine("LD IX,97");
            // TStatesByMachineCycle = "19 (4, 4, 3,5,3)"
            testProgram.AppendLine("LD (IX+3),4");
            // TStatesByMachineCycle = "23 (4, 4, 3, 5, 4, 3)"
            testProgram.AppendLine("DEC (IX+3)");
            // TStatesByMachineCycle = "13 (4, 3, 3, 3)"
            testProgram.AppendLine("LD A,(100)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/Arithmetic_8bits/Logs/Check_Instruction_33_DEC_IndexedAddress"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_46_INC_Register()
        {
            string sampleProgramFileName = "ALU/Arithmetic_8bits/Samples/testinc.asm";
            string sampleResultFileName = "ALU/Arithmetic_8bits/Samples/resultinc.csv";
            CompareExecutionResultsWithSample(sampleProgramFileName, sampleResultFileName, 2, 3);
        }

        public static void Check_Instruction_47_INC_Register16Address()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "10 (4, 3, 3)"
            testProgram.AppendLine("LD HL,100");
            // TStatesByMachineCycle = "10 (4, 3, 3)"
            testProgram.AppendLine("LD (HL),4"); ;
            // TStatesByMachineCycle = "11 (4, 4, 3)"
            testProgram.AppendLine("INC (HL)");
            // TStatesByMachineCycle = "13 (4, 3, 3, 3)"
            testProgram.AppendLine("LD A,(100)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/Arithmetic_8bits/Logs/Check_Instruction_47_INC_Register16Address"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_48_INC_IndexedAddress()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "14 (4, 4, 3, 3)"
            testProgram.AppendLine("LD IX,97");
            // TStatesByMachineCycle = "19 (4, 4, 3,5,3)"
            testProgram.AppendLine("LD (IX+3),4");
            // TStatesByMachineCycle = "23 (4, 4, 3, 5, 4, 3)"
            testProgram.AppendLine("INC (IX+3)");
            // TStatesByMachineCycle = "13 (4, 3, 3, 3)"
            testProgram.AppendLine("LD A,(100)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/Arithmetic_8bits/Logs/Check_Instruction_48_INC_IndexedAddress"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_79_OR_Number8()
        {
            string sampleProgramFileName = "ALU/Arithmetic_8bits/Samples/testor.asm";
            string sampleResultFileName = "ALU/Arithmetic_8bits/Samples/resultor.csv";
            CompareExecutionResultsWithSample(sampleProgramFileName, sampleResultFileName, 2, 3);
        }

        public static void Check_Instruction_80_OR_Register()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,10101010B");
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD B,10001001B");
            // TStatesByMachineCycle = 1 (4)
            testProgram.AppendLine("OR B");
            // TStatesByMachineCycle = "11 (4,4,3)"
            testProgram.AppendLine("LD IXh,1111B");
            // TStatesByMachineCycle = "8 (4, 4)"
            testProgram.AppendLine("OR IXh");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(5);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/Arithmetic_8bits/Logs/Check_Instruction_80_OR_Register"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_81_OR_Register16Address()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,10101010B");
            // TStatesByMachineCycle = "10 (4, 3, 3)"
            testProgram.AppendLine("LD HL,100");
            // TStatesByMachineCycle = "10 (4, 3, 3)"
            testProgram.AppendLine("LD (HL),10001001B"); ;
            // TStatesByMachineCycle = "7 (4, 3)"
            testProgram.AppendLine("OR (HL)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/Arithmetic_8bits/Logs/Check_Instruction_81_OR_Register16Address"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_82_OR_IndexedAddress()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,10101010B");
            // TStatesByMachineCycle = "14 (4, 4, 3, 3)"
            testProgram.AppendLine("LD IX,97");
            // TStatesByMachineCycle = "19 (4, 4, 3,5,3)"
            testProgram.AppendLine("LD (IX+3),10001001B");
            // TStatesByMachineCycle = "19 (4, 4, 3, 5, 3)"
            testProgram.AppendLine("OR (IX+3)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/Arithmetic_8bits/Logs/Check_Instruction_82_OR_IndexedAddress"))
            {
                throw new Exception("Log compare failed");
            }
        }
                
        public static void Check_Instruction_123_SBC_Register_Number8() 
        {
            string sampleProgramFileName = "ALU/Arithmetic_8bits/Samples/testsbc.asm";
            string sampleResultFileName = "ALU/Arithmetic_8bits/Samples/resultsbc.csv";
            CompareExecutionResultsWithSample(sampleProgramFileName, sampleResultFileName, 3, 3);
        }

        public static void Check_Instruction_124_SBC_Register_Register()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,10");
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD B,5");
            // TStatesByMachineCycle = 1 (4)
            testProgram.AppendLine("SCF");
            // TStatesByMachineCycle = 1 (4)
            testProgram.AppendLine("SBC A,B");
            // TStatesByMachineCycle = "11 (4,4,3)"
            testProgram.AppendLine("LD IXh,2");
            // TStatesByMachineCycle = 1 (4)
            testProgram.AppendLine("SCF");
            // TStatesByMachineCycle = 8 (4, 4)
            testProgram.AppendLine("SBC A,IXh");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(7);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/Arithmetic_8bits/Logs/Check_Instruction_124_SBC_Register_Register"))
            {
                throw new Exception("Log compare failed");
            }            
        }

        public static void Check_Instruction_125_SBC_Register_Register16Address()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,10");
            // TStatesByMachineCycle = "10 (4, 3, 3)"
            testProgram.AppendLine("LD HL,100");
            // TStatesByMachineCycle = "10 (4, 3, 3)"
            testProgram.AppendLine("LD (HL),5");
            // TStatesByMachineCycle = 1 (4)
            testProgram.AppendLine("SCF");
            // TStatesByMachineCycle = 7 (4, 3)
            testProgram.AppendLine("SBC A,(HL)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(5);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/Arithmetic_8bits/Logs/Check_Instruction_125_SBC_Register_Register16Address"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_126_SBC_Register_IndexedAddress()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,10");
            // TStatesByMachineCycle = "14 (4, 4, 3, 3)"
            testProgram.AppendLine("LD IX,97");
            // TStatesByMachineCycle = "19 (4, 4, 3,5,3)"
            testProgram.AppendLine("LD (IX+3),2");
            // TStatesByMachineCycle = 1 (4)
            testProgram.AppendLine("SCF");
            // TStatesByMachineCycle = "19 (4, 4, 3, 5, 3)"
            testProgram.AppendLine("SBC A,(IX+3)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(5);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/Arithmetic_8bits/Logs/Check_Instruction_126_SBC_Register_IndexedAddress"))
            {
                throw new Exception("Log compare failed");
            }
        }
         
        public static void Check_Instruction_149_SUB_Number8() 
        {
            string sampleProgramFileName = "ALU/Arithmetic_8bits/Samples/testsub.asm";
            string sampleResultFileName = "ALU/Arithmetic_8bits/Samples/resultsub.csv";
            CompareExecutionResultsWithSample(sampleProgramFileName, sampleResultFileName, 2, 3);
        }

        public static void Check_Instruction_150_SUB_Register()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,10");
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD B,4");
            // TStatesByMachineCycle = 1 (4)
            testProgram.AppendLine("SUB B");
            // TStatesByMachineCycle = "11 (4,4,3)"
            testProgram.AppendLine("LD IXh,3");
            // TStatesByMachineCycle = "8 (4, 4)"
            testProgram.AppendLine("SUB IXh");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(5);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/Arithmetic_8bits/Logs/Check_Instruction_150_SUB_Register"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_151_SUB_Register16Address()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,10");
            // TStatesByMachineCycle = "10 (4, 3, 3)"
            testProgram.AppendLine("LD HL,100");
            // TStatesByMachineCycle = "10 (4, 3, 3)"
            testProgram.AppendLine("LD (HL),4"); ;
            // TStatesByMachineCycle = "7 (4, 3)"
            testProgram.AppendLine("SUB (HL)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/Arithmetic_8bits/Logs/Check_Instruction_151_SUB_Register16Address"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_152_SUB_IndexedAddress()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,10");
            // TStatesByMachineCycle = "14 (4, 4, 3, 3)"
            testProgram.AppendLine("LD IX,97");
            // TStatesByMachineCycle = "19 (4, 4, 3,5,3)"
            testProgram.AppendLine("LD (IX+3),4");
            // TStatesByMachineCycle = "19 (4, 4, 3, 5, 3)"
            testProgram.AppendLine("SUB (IX+3)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/Arithmetic_8bits/Logs/Check_Instruction_152_SUB_IndexedAddress"))
            {
                throw new Exception("Log compare failed");
            }
        }
         
        public static void Check_Instruction_153_XOR_Number8()
        {
            string sampleProgramFileName = "ALU/Arithmetic_8bits/Samples/testxor.asm";
            string sampleResultFileName = "ALU/Arithmetic_8bits/Samples/resultxor.csv";
            CompareExecutionResultsWithSample(sampleProgramFileName, sampleResultFileName, 2, 3);
        }

        public static void Check_Instruction_154_XOR_Register()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,10101010B");
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD B,10001001B");
            // TStatesByMachineCycle = 1 (4)
            testProgram.AppendLine("XOR B");
            // TStatesByMachineCycle = "11 (4,4,3)"
            testProgram.AppendLine("LD IXh,1111B");
            // TStatesByMachineCycle = "8 (4, 4)"
            testProgram.AppendLine("XOR IXh");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(5);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/Arithmetic_8bits/Logs/Check_Instruction_154_XOR_Register"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_155_XOR_Register16Address()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,10101010B");
            // TStatesByMachineCycle = "10 (4, 3, 3)"
            testProgram.AppendLine("LD HL,100");
            // TStatesByMachineCycle = "10 (4, 3, 3)"
            testProgram.AppendLine("LD (HL),10001001B"); ;
            // TStatesByMachineCycle = "7 (4, 3)"
            testProgram.AppendLine("XOR (HL)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/Arithmetic_8bits/Logs/Check_Instruction_155_XOR_Register16Address"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_156_XOR_IndexedAddress()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,10101010B");
            // TStatesByMachineCycle = "14 (4, 4, 3, 3)"
            testProgram.AppendLine("LD IX,97");
            // TStatesByMachineCycle = "19 (4, 4, 3,5,3)"
            testProgram.AppendLine("LD (IX+3),10001001B");
            // TStatesByMachineCycle = "19 (4, 4, 3, 5, 3)"
            testProgram.AppendLine("XOR (IX+3)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ALU/Arithmetic_8bits/Logs/Check_Instruction_156_XOR_IndexedAddress"))
            {
                throw new Exception("Log compare failed");
            }
        }         
    }
}
