using System;
using System.Text;
using Z80Simulator.CPU;
using Z80Simulator.Test.System;

namespace Z80Simulator.Test.CPU
{
    public partial class CPUInstructionsTests
    {
        public static void Check_Instruction_59_LD_Register_Number8()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD H,30");
            // TStatesByMachineCycle = "11 (4,4,3)"
            testProgram.AppendLine("LD IXl,55");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(2); 
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/Load_8Bit/Logs/Check_Instruction_59_LD_Register_Number8"))
            {
                throw new Exception("Log compare failed");
            }
        }                        

        public static void Check_Instruction_60_LD_Register_Register()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD C,10");
            // TStatesByMachineCycle = "1 (4)"
            testProgram.AppendLine("LD D,C");
            // TStatesByMachineCycle = "8 (4,4)"
            testProgram.AppendLine("LD IXh,D");
            // TStatesByMachineCycle = "8 (4,4)"
            testProgram.AppendLine("LD E,IXh");
            // TStatesByMachineCycle = "8 (4,4)"
            testProgram.AppendLine("LD IXl,IXh");
            // TStatesByMachineCycle = "11 (4,4,3)"
            testProgram.AppendLine("LD IYh,32");
            // TStatesByMachineCycle = "8 (4,4)"
            testProgram.AppendLine("LD IYl,IYh");
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,55");
            // TStatesByMachineCycle = "9 (4, 5)"
            testProgram.AppendLine("LD R,A");
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,0");
            // TStatesByMachineCycle = "9 (4, 5)"
            testProgram.AppendLine("LD A,R");
            // TStatesByMachineCycle = "9 (4, 5)"
            testProgram.AppendLine("LD A,I");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers, 
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(12); 
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/Load_8Bit/Logs/Check_Instruction_60_LD_Register_Register"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_61_LD_Register_Address()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "13 (4, 3, 3, 3)"
            testProgram.AppendLine("LD A,(100)");
            testProgram.AppendLine("ORG 100");
            testProgram.AppendLine("DEFB 32");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(1);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/Load_8Bit/Logs/Check_Instruction_61_LD_Register_Address"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_62_LD_Register_Register16Address() 
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "10 (4,3,3)"
            testProgram.AppendLine("LD HL,100");
            // TStatesByMachineCycle = "10 (4, 3, 3)"
            testProgram.AppendLine("LD (HL),32");
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD E,(HL)");
            // TStatesByMachineCycle = "10 (4,3,3)"
            testProgram.AppendLine("LD BC,100");
            //  TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,(BC)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(5);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/Load_8Bit/Logs/Check_Instruction_62_LD_Register_Register16Address"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_63_LD_Register_IndexedAddress()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "14 (4, 4, 3, 3)"
            testProgram.AppendLine("LD IX,90");
            // TStatesByMachineCycle = "14 (4, 4, 3, 3)"
            testProgram.AppendLine("LD IY,105");
            // TStatesByMachineCycle = "10 (4,3,3)"
            testProgram.AppendLine("LD HL,100");
            // TStatesByMachineCycle = "10 (4, 3, 3)"
            testProgram.AppendLine("LD (HL),32");
            // TStatesByMachineCycle = "19 (4, 4, 3, 5, 3)"
            testProgram.AppendLine("LD A,(IX+10)");
            // TStatesByMachineCycle = "19 (4, 4, 3, 5, 3)"
            testProgram.AppendLine("LD D,(IY-5)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(6);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/Load_8Bit/Logs/Check_Instruction_63_LD_Register_IndexedAddress"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_67_LD_Address_Register()
        {
            StringBuilder testProgram = new StringBuilder();            
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,32");
            // TStatesByMachineCycle = "13 (4, 3, 3, 3)"
            testProgram.AppendLine("LD (100),A");
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,0");
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

            if (!logger.CompareWithSavedLog("CPU/Load_8Bit/Logs/Check_Instruction_67_LD_Address_Register"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_69_LD_Register16Address_Number8() 
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "10 (4,3,3)"
            testProgram.AppendLine("LD HL,100");
            // TStatesByMachineCycle = "10 (4, 3, 3)"
            testProgram.AppendLine("LD (HL),64");
            // TStatesByMachineCycle = "13 (4, 3, 3, 3)"
            testProgram.AppendLine("LD A,(100)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(3);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/Load_8Bit/Logs/Check_Instruction_69_LD_Register16Address_Number8"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_70_LD_Register16Address_Register() 
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD E,30");
            // TStatesByMachineCycle = "10 (4,3,3)"
            testProgram.AppendLine("LD HL,100");
            // TStatesByMachineCycle = "7 (4, 3)"
            testProgram.AppendLine("LD (HL),E");
            // TStatesByMachineCycle = "13 (4, 3, 3, 3)"
            testProgram.AppendLine("LD A,(100)");
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,40");
            // TStatesByMachineCycle = "10 (4,3,3)"
            testProgram.AppendLine("LD BC,110");
            // TStatesByMachineCycle = "7 (4, 3)"
            testProgram.AppendLine("LD (BC),A");
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD A,0");
            // TStatesByMachineCycle = "13 (4, 3, 3, 3)"
            testProgram.AppendLine("LD A,(110)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(9);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/Load_8Bit/Logs/Check_Instruction_70_LD_Register16Address_Register"))
            {
                throw new Exception("Log compare failed");
            }
        }
        
        public static void Check_Instruction_71_LD_IndexedAddress_Number8() 
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "14 (4, 4, 3, 3)"
            testProgram.AppendLine("LD IX,110");
            // TStatesByMachineCycle = "19 (4, 4, 3,5,3)"
            testProgram.AppendLine("LD (IX-10),20");
            // TStatesByMachineCycle = "13 (4, 3, 3, 3)"
            testProgram.AppendLine("LD A,(100)");
            // TStatesByMachineCycle = "14 (4, 4, 3, 3)"
            testProgram.AppendLine("LD IY,95");
            // TStatesByMachineCycle = "19 (4, 4, 3,5,3)"
            testProgram.AppendLine("LD (IY+5),30");
            // TStatesByMachineCycle = "13 (4, 3, 3, 3)"
            testProgram.AppendLine("LD A,(100)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(6);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/Load_8Bit/Logs/Check_Instruction_71_LD_IndexedAddress_Number8"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_72_LD_IndexedAddress_Register()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "14 (4, 4, 3, 3)"
            testProgram.AppendLine("LD IX,110");
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD B,20");
            // TStatesByMachineCycle = "19 (4, 4, 3,5,3)"
            testProgram.AppendLine("LD (IX-10),B");
            // TStatesByMachineCycle = "13 (4, 3, 3, 3)"
            testProgram.AppendLine("LD A,(100)");
            // TStatesByMachineCycle = "14 (4, 4, 3, 3)"
            testProgram.AppendLine("LD IY,95");
            // TStatesByMachineCycle = "7 (4,3)"
            testProgram.AppendLine("LD L,30");
            // TStatesByMachineCycle = "19 (4, 4, 3,5,3)"
            testProgram.AppendLine("LD (IY+5),L");
            // TStatesByMachineCycle = "13 (4, 3, 3, 3)"
            testProgram.AppendLine("LD A,(100)");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(8);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/Load_8Bit/Logs/Check_Instruction_72_LD_IndexedAddress_Register"))
            {
                throw new Exception("Log compare failed");
            }
        }
    }
}
