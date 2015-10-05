using System;
using System.Text;
using Z80Simulator.CPU;
using Z80Simulator.Test.System;

namespace Z80Simulator.Test.CPU
{
    public partial class CPUInstructionsTests
    {

        public static void Check_Instruction_35_DI()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "4"
            testProgram.AppendLine("EI");
            // TStatesByMachineCycle = "4"
            testProgram.AppendLine("DI");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(2);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/CPUControl/Logs/Check_Instruction_35_DI"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_37_EI()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "4"
            testProgram.AppendLine("DI");
            // TStatesByMachineCycle = "4"
            testProgram.AppendLine("EI");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(2);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/CPUControl/Logs/Check_Instruction_37_EI"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_41_HALT()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("IM 1");
            testProgram.AppendLine("EI");
            testProgram.AppendLine("HALT");
            testProgram.AppendLine("INC A");
            testProgram.AppendLine("ORG 38H");
            testProgram.AppendLine("INC B");
            testProgram.AppendLine("EI");
            testProgram.AppendLine("RETI");
            testProgram.AppendLine("ORG 66H");
            testProgram.AppendLine("INC C");
            testProgram.AppendLine("RETN");

            int[][] startNMIAfterTStatesAndActivateDuringTStates = new int[][] { 
                new int[] { 29, 3 }
            };           

            int[][] startRESETAfterTStatesAndActivateDuringTStates = new int[][] { 
                new int[] { 62, 3 }
            };

            int[][] startINTAfterTStatesAndActivateDuringTStates = new int[][] { 
                new int[] { 97, 3 }
            };

            TestSystemWithTestSignals testSystem = new TestSystemWithTestSignals(null, startINTAfterTStatesAndActivateDuringTStates, startNMIAfterTStatesAndActivateDuringTStates, startRESETAfterTStatesAndActivateDuringTStates, null);
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers | 
                CPUStateLogger.CPUStateElements.ControlPins | CPUStateLogger.CPUStateElements.MicroInstructions,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.Clock.Tick(139);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/CPUControl/Logs/Check_Instruction_41_HALT"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_42_IM_InterruptMode()
        {
            StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "8 (4,4)"
            testProgram.AppendLine("IM 2");
            // TStatesByMachineCycle = "8 (4,4)"
            testProgram.AppendLine("IM 1");
            // TStatesByMachineCycle = "8 (4,4)"
            testProgram.AppendLine("IM 0");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(3);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/CPUControl/Logs/Check_Instruction_42_IM_InterruptMode"))
            {
                throw new Exception("Log compare failed");
            }
        }
        
        public static void Check_Instruction_78_NOP() 
        {
             StringBuilder testProgram = new StringBuilder();
            // TStatesByMachineCycle = "4"
            testProgram.AppendLine("NOP");
            // TStatesByMachineCycle = "4"
            testProgram.AppendLine("NOP");

            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
                CPUStateLogger.CPUStateElements.InternalState | CPUStateLogger.CPUStateElements.Registers,
                new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.MachineCycleEnd });
            testSystem.ExecuteInstructionCount(2);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/CPUControl/Logs/Check_Instruction_78_NOP"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_157_NMI()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("EI");
            testProgram.AppendLine("START: INC A");
            testProgram.AppendLine("JR START");
            testProgram.AppendLine("ORG 66H");
            testProgram.AppendLine("INC B");
            testProgram.AppendLine("RETN");

            int[][] startNMIAfterTStatesAndActivateDuringTStates = new int[][] { 
                new int[] { 4, 2 },
                new int[] { 18, 1 },
                new int[] { 50, 3 },
                new int[] { 66, 1 }
            };

            int[][] startINTAfterTStatesAndActivateDuringTStates = new int[][] { 
                new int[] { 18, 3 }
            };

            TestSystemWithTestSignals testSystem = new TestSystemWithTestSignals(null, startINTAfterTStatesAndActivateDuringTStates, startNMIAfterTStatesAndActivateDuringTStates, null, null);
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
               CPUStateLogger.CPUStateElements.InternalState |
               CPUStateLogger.CPUStateElements.Registers |
               CPUStateLogger.CPUStateElements.Buses |
               CPUStateLogger.CPUStateElements.ControlPins,
               new Z80CPU.LifecycleEventType[] { 
                    Z80CPU.LifecycleEventType.HalfTState,
                    Z80CPU.LifecycleEventType.InstructionEnd,
                    Z80CPU.LifecycleEventType.InstructionStart,
                    Z80CPU.LifecycleEventType.MachineCycleEnd,
                    Z80CPU.LifecycleEventType.MachineCycleStart
                });
            testSystem.Clock.Tick(127);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/CPUControl/Logs/Check_Instruction_157_NMI"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_158_INT_0()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("IM 0");
            testProgram.AppendLine("EI");
            testProgram.AppendLine("START: INC A");
            testProgram.AppendLine("JR START");
            testProgram.AppendLine("ORG 18H"); //OpCode = 0xDF,"RST 18H",
            testProgram.AppendLine("INC B");
            testProgram.AppendLine("EI");
            testProgram.AppendLine("RETI");

            int[][] startAfterTStatesAndActivateDuringTStates = new int[][] { 
                new int[] { 14, 1 }
            };

            TestSystemWithInterruptingDevice testSystem = new TestSystemWithInterruptingDevice(0xFE, 0, 0, startAfterTStatesAndActivateDuringTStates, 0xDF);
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
               CPUStateLogger.CPUStateElements.InternalState |
               CPUStateLogger.CPUStateElements.Registers |
               CPUStateLogger.CPUStateElements.Buses |
               CPUStateLogger.CPUStateElements.ControlPins,
               new Z80CPU.LifecycleEventType[] { 
                    Z80CPU.LifecycleEventType.HalfTState,
                    Z80CPU.LifecycleEventType.InstructionEnd,
                    Z80CPU.LifecycleEventType.InstructionStart,
                    Z80CPU.LifecycleEventType.MachineCycleEnd,
                    Z80CPU.LifecycleEventType.MachineCycleStart
                });
            testSystem.Clock.Tick(67);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/CPUControl/Logs/Check_Instruction_158_INT_0"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_159_INT_1()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("IM 1");
            testProgram.AppendLine("EI");
            testProgram.AppendLine("EI");
            testProgram.AppendLine("START: INC A");
            testProgram.AppendLine("JR START");
            testProgram.AppendLine("ORG 38H");
            testProgram.AppendLine("INC B");
            testProgram.AppendLine("EI");
            testProgram.AppendLine("RETI");

            int[][] startAfterTStatesAndActivateDuringTStates = new int[][] { 
                new int[] { 6, 1 },
                new int[] { 10, 1 },
                new int[] { 14, 1 },
                new int[] { 18, 1 },
                new int[] { 35, 1 },
                new int[] { 39, 1 }
            };

            TestSystemWithTestSignals testSystem = new TestSystemWithTestSignals(null, startAfterTStatesAndActivateDuringTStates, null, null, null);
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
               CPUStateLogger.CPUStateElements.InternalState |
               CPUStateLogger.CPUStateElements.Registers |
               CPUStateLogger.CPUStateElements.Buses |
               CPUStateLogger.CPUStateElements.ControlPins,
               new Z80CPU.LifecycleEventType[] { 
                    Z80CPU.LifecycleEventType.HalfTState,
                    Z80CPU.LifecycleEventType.InstructionEnd,
                    Z80CPU.LifecycleEventType.InstructionStart,
                    Z80CPU.LifecycleEventType.MachineCycleEnd,
                    Z80CPU.LifecycleEventType.MachineCycleStart
                });
            testSystem.Clock.Tick(71);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/CPUControl/Logs/Check_Instruction_159_INT_1"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_160_INT_2()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("IM 2");
            testProgram.AppendLine("EI");
            testProgram.AppendLine("LD A,2");
            testProgram.AppendLine("LD I,A");
            testProgram.AppendLine("START: INC A");
            testProgram.AppendLine("JR START");
            testProgram.AppendLine("ORG 520"); // 520 = 2 x 256 + 8
            testProgram.AppendLine("DEFB 10H");
            testProgram.AppendLine("DEFB 04H");
            testProgram.AppendLine("ORG 1040"); // 1040 = 0410H 
            testProgram.AppendLine("INC B");
            testProgram.AppendLine("EI");
            testProgram.AppendLine("RETI");

            int[][] startAfterTStatesAndActivateDuringTStates = new int[][] { 
                new int[] { 30, 1 }
            };
            TestSystemWithInterruptingDevice testSystem = new TestSystemWithInterruptingDevice(0xFE, 0, 0, startAfterTStatesAndActivateDuringTStates, 8);
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
               CPUStateLogger.CPUStateElements.InternalState |
               CPUStateLogger.CPUStateElements.Registers |
               CPUStateLogger.CPUStateElements.Buses |
               CPUStateLogger.CPUStateElements.ControlPins,
               new Z80CPU.LifecycleEventType[] { 
                    Z80CPU.LifecycleEventType.HalfTState,
                    Z80CPU.LifecycleEventType.InstructionEnd,
                    Z80CPU.LifecycleEventType.InstructionStart,
                    Z80CPU.LifecycleEventType.MachineCycleEnd,
                    Z80CPU.LifecycleEventType.MachineCycleStart
                });
            testSystem.Clock.Tick(89);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/CPUControl/Logs/Check_Instruction_160_INT_2"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Instruction_161_RESET()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("START: INC A");
            testProgram.AppendLine("JR START");

            int[][] startAfterTStatesAndActivateDuringTStates = new int[][] { new int[]{ 30, 10 } };

            TestSystemWithTestSignals testSystem = new TestSystemWithTestSignals(null, null, null, startAfterTStatesAndActivateDuringTStates, null);
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
               CPUStateLogger.CPUStateElements.InternalState |
               CPUStateLogger.CPUStateElements.Registers |
               CPUStateLogger.CPUStateElements.Buses |
               CPUStateLogger.CPUStateElements.ControlPins,
               new Z80CPU.LifecycleEventType[] { 
                    Z80CPU.LifecycleEventType.HalfTState,
                    Z80CPU.LifecycleEventType.InstructionEnd,
                    Z80CPU.LifecycleEventType.InstructionStart,
                    Z80CPU.LifecycleEventType.MachineCycleEnd,
                    Z80CPU.LifecycleEventType.MachineCycleStart
                });
            testSystem.Clock.Tick(50);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("CPU/CPUControl/Logs/Check_Instruction_161_RESET"))
            {
                throw new Exception("Log compare failed");
            }
        }
    }
}
