using System;
using System.Text;
using Z80Simulator.CPU;
using Z80Simulator.Test.System;

namespace Z80Simulator.Test.CPU
{
    public partial class CPUInstructionsTests
    {
        public static void Check_Instruction_25_CPD()
        {
            StringBuilder testProgram = new StringBuilder();            
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("LD (HL),1");
            testProgram.AppendLine("LD HL,101");
            testProgram.AppendLine("LD (HL),2");
            testProgram.AppendLine("LD HL,102");
            testProgram.AppendLine("LD (HL),3");
            testProgram.AppendLine("LD BC,3");
            testProgram.AppendLine("LD A,1");
            testProgram.AppendLine("CPD");
            testProgram.AppendLine("CPD");
            testProgram.AppendLine("CPD");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(11);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/ExchangeBlockTransferAndSearch/Logs/Check_Instruction_25_CPD"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_26_CPDR()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("LD (HL),1");
            testProgram.AppendLine("LD HL,101");
            testProgram.AppendLine("LD (HL),2");
            testProgram.AppendLine("LD HL,102");
            testProgram.AppendLine("LD (HL),3");
            testProgram.AppendLine("LD BC,3");
            testProgram.AppendLine("LD A,5");
            testProgram.AppendLine("CPDR");
            testProgram.AppendLine("LD HL,102");
            testProgram.AppendLine("LD BC,3");
            testProgram.AppendLine("LD A,2");
            testProgram.AppendLine("CPDR");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(17);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/ExchangeBlockTransferAndSearch/Logs/Check_Instruction_26_CPDR"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_27_CPI()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("LD (HL),1");
            testProgram.AppendLine("LD HL,101");
            testProgram.AppendLine("LD (HL),2");
            testProgram.AppendLine("LD HL,102");
            testProgram.AppendLine("LD (HL),3");
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("LD BC,3");
            testProgram.AppendLine("LD A,3");
            testProgram.AppendLine("CPI");
            testProgram.AppendLine("CPI");
            testProgram.AppendLine("CPI");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(12);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/ExchangeBlockTransferAndSearch/Logs/Check_Instruction_27_CPI"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_28_CPIR()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("LD (HL),1");
            testProgram.AppendLine("LD HL,101");
            testProgram.AppendLine("LD (HL),2");
            testProgram.AppendLine("LD HL,102");
            testProgram.AppendLine("LD (HL),3");
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("LD BC,3");
            testProgram.AppendLine("LD A,5");
            testProgram.AppendLine("CPIR");
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("LD BC,3");
            testProgram.AppendLine("LD A,2");
            testProgram.AppendLine("CPIR");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(18);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/ExchangeBlockTransferAndSearch/Logs/Check_Instruction_28_CPIR"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_38_EX_Register16_Register16()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD A,10");
            testProgram.AppendLine("SCF");
            testProgram.AppendLine("EX AF,AF'");
            testProgram.AppendLine("LD A,20");
            testProgram.AppendLine("CCF");
            testProgram.AppendLine("EX AF,AF'");
            testProgram.AppendLine("EX AF,AF'");
            testProgram.AppendLine("LD DE,1000");
            testProgram.AppendLine("LD HL,2000");
            testProgram.AppendLine("EX DE,HL");
            testProgram.AppendLine("EX DE,HL");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(11);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/ExchangeBlockTransferAndSearch/Logs/Check_Instruction_38_EX_Register16_Register16"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_39_EX_Register16Address_Register16()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD BC,1000");
            testProgram.AppendLine("LD HL,2000");
            testProgram.AppendLine("LD IX,3000");
            testProgram.AppendLine("LD IY,4000");
            testProgram.AppendLine("PUSH BC");
            testProgram.AppendLine("EX (SP),HL");
            testProgram.AppendLine("EX (SP),IX");
            testProgram.AppendLine("EX (SP),IY");
            testProgram.AppendLine("POP BC");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(9);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/ExchangeBlockTransferAndSearch/Logs/Check_Instruction_39_EX_Register16Address_Register16"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_40_EXX()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD A,10");
            testProgram.AppendLine("LD BC,1000");
            testProgram.AppendLine("LD DE,2000");
            testProgram.AppendLine("LD HL,3000");
            testProgram.AppendLine("EXX");
            testProgram.AppendLine("LD BC,1111");
            testProgram.AppendLine("LD DE,2222");
            testProgram.AppendLine("LD HL,3333");
            testProgram.AppendLine("EXX");
            testProgram.AppendLine("EXX");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(10);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/ExchangeBlockTransferAndSearch/Logs/Check_Instruction_40_EXX"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_73_LDD()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("LD (HL),1");
            testProgram.AppendLine("LD HL,101");
            testProgram.AppendLine("LD (HL),2");
            testProgram.AppendLine("LD HL,102");
            testProgram.AppendLine("LD (HL),3");
            testProgram.AppendLine("LD DE,52");
            testProgram.AppendLine("LD HL,102");
            testProgram.AppendLine("LD BC,3");
            testProgram.AppendLine("LDD");
            testProgram.AppendLine("LDD");
            testProgram.AppendLine("LDD");
            testProgram.AppendLine("LD A,(50)");
            testProgram.AppendLine("LD A,(51)");
            testProgram.AppendLine("LD A,(52)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(15);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/ExchangeBlockTransferAndSearch/Logs/Check_Instruction_73_LDD"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_74_LDDR()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("LD (HL),1");
            testProgram.AppendLine("LD HL,101");
            testProgram.AppendLine("LD (HL),2");
            testProgram.AppendLine("LD HL,102");
            testProgram.AppendLine("LD (HL),3");
            testProgram.AppendLine("LD DE,52");
            testProgram.AppendLine("LD HL,102");
            testProgram.AppendLine("LD BC,3");
            testProgram.AppendLine("LDDR");
            testProgram.AppendLine("LD A,(50)");
            testProgram.AppendLine("LD A,(51)");
            testProgram.AppendLine("LD A,(52)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(15);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/ExchangeBlockTransferAndSearch/Logs/Check_Instruction_74_LDDR"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_75_LDI()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("LD (HL),1");
            testProgram.AppendLine("LD HL,101");
            testProgram.AppendLine("LD (HL),2");
            testProgram.AppendLine("LD HL,102");
            testProgram.AppendLine("LD (HL),3");
            testProgram.AppendLine("LD DE,50");
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("LD BC,3");
            testProgram.AppendLine("LDI");
            testProgram.AppendLine("LDI");
            testProgram.AppendLine("LDI");
            testProgram.AppendLine("LD A,(50)");
            testProgram.AppendLine("LD A,(51)");
            testProgram.AppendLine("LD A,(52)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(15);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/ExchangeBlockTransferAndSearch/Logs/Check_Instruction_75_LDI"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_76_LDIR()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("LD (HL),1");
            testProgram.AppendLine("LD HL,101");
            testProgram.AppendLine("LD (HL),2");
            testProgram.AppendLine("LD HL,102");
            testProgram.AppendLine("LD (HL),3");
            testProgram.AppendLine("LD DE,50");
            testProgram.AppendLine("LD HL,100");
            testProgram.AppendLine("LD BC,3");
            testProgram.AppendLine("LDIR");
            testProgram.AppendLine("LD A,(50)");
            testProgram.AppendLine("LD A,(51)");
            testProgram.AppendLine("LD A,(52)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(15);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/ExchangeBlockTransferAndSearch/Logs/Check_Instruction_76_LDIR"))
            {
                throw new Exception("Log compare failed");
            }
        }
    }
}
