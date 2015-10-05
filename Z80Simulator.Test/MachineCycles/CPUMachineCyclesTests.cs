using System;
using System.Text;
using Z80Simulator.CPU;
using Z80Simulator.Test.System;

namespace Z80Simulator.Test.CPU.MachineCycles
{
    public class CPUMachineCyclesTests
    {
        public static void Check_FetchOpcode(bool simulateSlowMemoryAndDevices)
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD A,B");
            testProgram.AppendLine("LD R,A");

            TestSystem testSystem = new TestSystem(simulateSlowMemoryAndDevices);
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
            testSystem.ExecuteInstructionCount(2);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("MachineCycles/Logs/Check_FetchOpcode" + GetSlowSuffix(simulateSlowMemoryAndDevices)))
            {
                throw new Exception("Log compare failed");
            }
        }

        private static string GetSlowSuffix(bool simulateSlowMemoryAndDevices)
        {
            return simulateSlowMemoryAndDevices ? "_wait" : "";
        }

        public static void Check_MemoryRead(bool simulateSlowMemoryAndDevices)
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD B,10");
            testProgram.AppendLine("LD A,(10)");
            testProgram.AppendLine("ORG 10");
            testProgram.AppendLine("DEFB 32");

            TestSystem testSystem = new TestSystem(simulateSlowMemoryAndDevices);
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
            testSystem.ExecuteInstructionCount(2);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("MachineCycles/Logs/Check_MemoryRead" + GetSlowSuffix(simulateSlowMemoryAndDevices)))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_MemoryWrite(bool simulateSlowMemoryAndDevices)
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD A,32");
            testProgram.AppendLine("LD (100),A");
            testProgram.AppendLine("LD A,0");
            testProgram.AppendLine("LD A,(100)");

            TestSystem testSystem = new TestSystem(simulateSlowMemoryAndDevices);
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
            testSystem.ExecuteInstructionCount(4);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("MachineCycles/Logs/Check_MemoryWrite" + GetSlowSuffix(simulateSlowMemoryAndDevices)))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_StackMemoryReadAndWrite()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD A,10");
            testProgram.AppendLine("PUSH AF");
            testProgram.AppendLine("POP BC");

            TestSystem testSystem = new TestSystem();
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
            testSystem.ExecuteInstructionCount(3);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("MachineCycles/Logs/Check_StackMemoryReadAndWrite"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Input(bool simulateSlowMemoryAndDevices)
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("IN A,(1)");
            testProgram.AppendLine("IN A,(2)");

            TestSystem testSystem = new TestSystemWithTwoDevices(simulateSlowMemoryAndDevices);
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
            testSystem.ExecuteInstructionCount(2);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("MachineCycles/Logs/Check_Input" + GetSlowSuffix(simulateSlowMemoryAndDevices)))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Output(bool simulateSlowMemoryAndDevices)
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD A,23");
            testProgram.AppendLine("OUT (1),A");
            testProgram.AppendLine("LD A,0");
            testProgram.AppendLine("OUT (2),A");
            testProgram.AppendLine("IN A,(1)");
            testProgram.AppendLine("IN A,(2)");

            TestSystem testSystem = new TestSystemWithTwoDevices(simulateSlowMemoryAndDevices);
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
            testSystem.ExecuteInstructionCount(6);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("MachineCycles/Logs/Check_Output" + GetSlowSuffix(simulateSlowMemoryAndDevices)))
            {
                throw new Exception("Log compare failed");
            }
        }       

        public static void Check_BusRequestAcknowledge()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("START: INC A");
            testProgram.AppendLine("JR START");

            int[][] startAfterTStatesAndActivateDuringTStates = new int[][] { 
                new int[] { 1, 1 },
                new int[] { 12, 4 },
                new int[] { 20, 3 },
                new int[] { 29, 10 }
            };

            TestSystemWithTestSignals testSystem = new TestSystemWithTestSignals(startAfterTStatesAndActivateDuringTStates, null, null, null, null);
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
               CPUStateLogger.CPUStateElements.InternalState |
               CPUStateLogger.CPUStateElements.Registers |
               CPUStateLogger.CPUStateElements.Buses |
               CPUStateLogger.CPUStateElements.ControlPins |
               CPUStateLogger.CPUStateElements.MicroInstructions,
               new Z80CPU.LifecycleEventType[] { 
                    Z80CPU.LifecycleEventType.HalfTState,
                    Z80CPU.LifecycleEventType.InstructionEnd,
                    Z80CPU.LifecycleEventType.InstructionStart,
                    Z80CPU.LifecycleEventType.MachineCycleEnd,
                    Z80CPU.LifecycleEventType.MachineCycleStart
                });
            testSystem.Clock.Tick(50);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("MachineCycles/Logs/Check_BusRequestAcknowledge"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_InterruptRequestAcknowledge()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("IM 1");
            testProgram.AppendLine("EI");
            testProgram.AppendLine("INC A");

            int[][] startAfterTStatesAndActivateDuringTStates = new int[][] { 
                new int[] { 14, 1 }
            };

            TestSystemWithTestSignals testSystem = new TestSystemWithTestSignals(null, startAfterTStatesAndActivateDuringTStates, null, null, null);
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
               CPUStateLogger.CPUStateElements.InternalState |
               CPUStateLogger.CPUStateElements.Registers |
               CPUStateLogger.CPUStateElements.Buses |
               CPUStateLogger.CPUStateElements.ControlPins |
               CPUStateLogger.CPUStateElements.MicroInstructions,
               new Z80CPU.LifecycleEventType[] { 
                    Z80CPU.LifecycleEventType.HalfTState,
                    Z80CPU.LifecycleEventType.InstructionEnd,
                    Z80CPU.LifecycleEventType.InstructionStart,
                    Z80CPU.LifecycleEventType.MachineCycleEnd,
                    Z80CPU.LifecycleEventType.MachineCycleStart
                });
            testSystem.Clock.Tick(29);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("MachineCycles/Logs/Check_InterruptRequestAcknowledge"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_MicroInstructions()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("START: INC A");
            testProgram.AppendLine("JR START");

            TestSystem testSystem = new TestSystem(false);
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
               CPUStateLogger.CPUStateElements.InternalState |
               CPUStateLogger.CPUStateElements.Registers |
               CPUStateLogger.CPUStateElements.Buses |
               CPUStateLogger.CPUStateElements.ControlPins |
               CPUStateLogger.CPUStateElements.MicroInstructions,
               new Z80CPU.LifecycleEventType[] { 
                    Z80CPU.LifecycleEventType.HalfTState,
                    Z80CPU.LifecycleEventType.InstructionEnd,
                    Z80CPU.LifecycleEventType.InstructionStart,
                    Z80CPU.LifecycleEventType.MachineCycleEnd,
                    Z80CPU.LifecycleEventType.MachineCycleStart
                });
            testSystem.ExecuteInstructionCount(3);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("MachineCycles/Logs/Check_MicroInstructions"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_TwoBytesOpCodesAndDDFDPrefixes()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("INC A");
            testProgram.AppendLine("DEFB 0DDH");
            testProgram.AppendLine("INC B");
            testProgram.AppendLine("DEFB 0FDH");
            testProgram.AppendLine("INC C");
            testProgram.AppendLine("INC IX");
            testProgram.AppendLine("INC IY");
            testProgram.AppendLine("DEFB 0FDH");
            testProgram.AppendLine("DEFB 0DDH");
            testProgram.AppendLine("DEFB 0DDH");
            testProgram.AppendLine("DEFB 0FDH");
            testProgram.AppendLine("DEFB 0FDH");
            testProgram.AppendLine("DEC IY");

            int[][] startAfterTStatesAndActivateDuringTStates = new int[][] { 
                new int[] { 40, 26 }
            };

            TestSystemWithTestSignals testSystem = new TestSystemWithTestSignals(null, null, startAfterTStatesAndActivateDuringTStates, null, null);
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
               CPUStateLogger.CPUStateElements.InternalState |
               CPUStateLogger.CPUStateElements.Registers |
               CPUStateLogger.CPUStateElements.ControlPins,
               new Z80CPU.LifecycleEventType[] { 
                    Z80CPU.LifecycleEventType.MachineCycleEnd
                });
            testSystem.Clock.Tick(74);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("MachineCycles/Logs/Check_TwoBytesOpCodesAndDDFDPrefixes"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_FourBytesOpCodes()
        {
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD IX,100");
            testProgram.AppendLine("LD (IX+0),1000000B");
            testProgram.AppendLine("DEFB 0DDH");
            testProgram.AppendLine("INC B");
            testProgram.AppendLine("INC IY");
            testProgram.AppendLine("BIT 0,(IX+0)");
            testProgram.AppendLine("LD IY,200");
            testProgram.AppendLine("LD (IY+0),0000001B");
            testProgram.AppendLine("DEFB 0FDH");
            testProgram.AppendLine("INC C");
            testProgram.AppendLine("INC IX");
            testProgram.AppendLine("BIT 0,(IY+0)");

            int[][] startAfterTStatesAndActivateDuringTStates = new int[][] { 
                new int[] { 51, 18 }
            };

            TestSystemWithTestSignals testSystem = new TestSystemWithTestSignals(null, null, startAfterTStatesAndActivateDuringTStates, null, null);
            testSystem.LoadProgramInMemory(testProgram.ToString());

            CPUStateLogger logger = new CPUStateLogger(testSystem.CPU);
            logger.StartLogging(
               CPUStateLogger.CPUStateElements.InternalState |
               CPUStateLogger.CPUStateElements.Registers |
               CPUStateLogger.CPUStateElements.ControlPins,
               new Z80CPU.LifecycleEventType[] { 
                    Z80CPU.LifecycleEventType.MachineCycleEnd
                });
            testSystem.ExecuteInstructionCount(10);
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("MachineCycles/Logs/Check_FourBytesOpCodes"))
            {
                throw new Exception("Log compare failed");
            }
        }

        public static void Check_Speed()
        {
#if (DEBUG)
// Do not test speed when the code is not optimized
#else
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("START: INC A");
            testProgram.AppendLine("JR START");

            TestSystem testSystem = new TestSystem(false);
            testSystem.LoadProgramInMemory(testProgram.ToString());

            // 14 millions TStates @ 3,5Mzh => 4 seconds in the real machine
            // The simulator should complete the loop faster
            long startTime = DateTime.Now.Ticks;
            for (int i = 0; i < 14000000; i++)
            {
                testSystem.Clock.Tick();
            }
            long endTime = DateTime.Now.Ticks;
            TimeSpan duration = new TimeSpan(endTime - startTime);
            double durationMs = duration.TotalMilliseconds;
            if (durationMs > 4000)
            {
                throw new Exception("Simlulation too slow : " + (int)durationMs + " ms");
            }
            else
            {
                // Console.WriteLine("Execution time < 4s : " + (int)durationMs + " ms");
            }
#endif
        }
    }
}
