using System;
using Z80Simulator.Instructions;
using Z80Simulator.System;

namespace Z80Simulator.CPU
{
    /// <summary>
    /// ZiLOG Z80 CPU
    /// 
    /// Cycle accurate emulator based on the ZiLOG Z80 CPU User's Manual : 
    /// http://www.zilog.com/docs/z80/um0080.pdf
    /// 
    /// The ZiLOG Z80 CPU family of components are fourth-generation enhanced
    /// microprocessors with exceptional computational power. They offer higher
    /// system throughput and more efficient memory utilization than comparable
    /// second- and third-generation microprocessors. The speed offerings from 6–
    /// 20 MHz suit a wide range of applications which migrate software. The
    /// internal registers contain 208 bits of read/write memory that are accessible
    /// to the programmer. These registers include two sets of six general purpose
    /// registers which may be used individually as either 8-bit registers or as 16-bit
    /// register pairs. In addition, there are two sets of accumulator and flag
    /// registers.
    /// The Z80 CPU also contains a Stack Pointer, Program Counter, two index
    /// registers, a REFRESH register, and an INTERRUPT register. The CPU is easy
    /// to incorporate into a system since it requires only a single +5V power
    /// source. All output signals are fully decoded and timed to control standard
    /// memory or peripheral circuits; the Z80 CPU is supported by an extensive
    /// family of peripheral controllers.
    /// </summary>
    public partial class Z80CPU
    {
        #region Power On
        
        /// <summary>
        /// Power on
        /// </summary>
        public Z80CPU()
        {
            Address = new Z80CPUBusConnector<ushort>(this, "AddressBus");
            Data = new Z80CPUBusConnector<byte>(this, "DataBus");

            _m1 = SignalState.HIGH;
            _mreq = SignalState.HIGH;
            _iorq = SignalState.HIGH;
            _rd = SignalState.HIGH;
            _wr = SignalState.HIGH;
            _rfsh = SignalState.HIGH;

            _halt = SignalState.HIGH;
            
            _busack = SignalState.HIGH;
            
            ResetCPUControlState();
            ResetCPURegistersState(true);
        }

        #endregion

        #region Reset

        // Reset CPU control section and instruction decoder internal state
        private void ResetCPUControlState()
        {
            // Z80CPU.cs internal state 
            tStateCounter = 0;
            machineCycleCounter = 0;
            instructionCounter = 0;
            instructionOrigin = null;
            currentInstruction = null;
            executeInstruction = null;
            halfTStateIndex = 0;
            machineCycleIndex = 0;
            machineCycleCountAfterInstruction = 0;
            currentMachineCycle = null;
            executeMachineCycle = null;

            // Z80CPU_Instruction_Decoder.cs internal state
            opcodeTableIndex = 0;

            // Z80CPU_ExternalControl.cs internal state
            busRequestPending = false;
            maskableInterruptPending = false;
            nonMaskableInterruptPending = false;
        }

        // Set all registers state to initial value
        private void ResetCPURegistersState(bool powerOn)
        {
            // It resets the interrupt enable flip-flop, 
            IFF1 = false;
            IFF2 = false;
            // clears the PC and registers I and R, 
            PC = 0;
            I = 0;
            R = 0;
            // and sets the interrupt status to Mode 0. 
            IM = 0;

            // Undocumented : 
            // AF and SP are always set to FFFFh after a reset
            AF = 0xFFFF; AF2 = 0xFFFF;
            SP = 0xFFFF;
            // and all other registers are undefined (different depending on how long the CPU has been powered of, different for different Z80 chips).
            
            if (TraceMicroInstructions && !powerOn)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.CPUControlResetRegistersState));
            }
        }   

        /// <summary>
        // Reset - start
        /// </summary>
        private void CPUControl_EnterResetMode()
        {
            // During reset time, the address and data bus go to a high-impedance state, 
            Address.ReleaseValue();
            Data.ReleaseValue();

            // and all control output signals go to the inactive state
            M1 = SignalState.HIGH;
            MREQ = SignalState.HIGH;
            IORQ = SignalState.HIGH;
            RD = SignalState.HIGH;
            WR = SignalState.HIGH;
            RFSH = SignalState.HIGH;
            HALT = SignalState.HIGH;
            BUSACK = SignalState.HIGH;

            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.CPUControlEnterResetMode));
            }
        }

        /// <summary>
        // Reset - end
        /// </summary>
        private void CPUControl_ExitResetMode()
        {
            ResetCPURegistersState(false);

            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.CPUControlExitResetMode));
            }
        }
        
        #endregion
        
        #region Instructions and machine cycles Sequencer

        // Absolute counters since power on (only for debug & display purposes)
        private long tStateCounter = 0;
        private long machineCycleCounter = 0;
        private long instructionCounter = 0;

        // Current instruction
        private InstructionOrigin instructionOrigin;
        private Instruction currentInstruction;
        private ExecuteInstruction executeInstruction;

        // Relative counters within one instruction
        private byte halfTStateIndex = 0;
        private byte machineCycleIndex = 0;                 // Incremented for each machine cycle
        private byte machineCycleCountAfterInstruction = 0; // Not incremented while reading the prefix
       
        // Current machine cycle
        private MachineCycle currentMachineCycle;
        private ExecuteMachineCycle executeMachineCycle;
        
        // Main entry point : logic executed on each edge of the clock signal
        public void CLK_OnEdge()
        {
            // Count T States
            if (halfTStateIndex % 2 == 0)
            {
                tStateCounter++;
            }

            // Debug interface : breakpoints management
            ExitConditionException pendingExitException = null;
            
            // Start a new instruction if no instruction is currently in progress
            if (currentInstruction == null)
            {
                // If RESET Pin is active, execute a special reset "instruction"
                if (RESET == SignalState.LOW)
                {
                    ResetCPUControlState();
                    StartInstruction(Instruction.RESET);                    
                }
                // If a non maskable interrupt is pending, execute NMI instruction
                else if (nonMaskableInterruptPending)
                {
                    nonMaskableInterruptPending = false;
                    StartInstruction(Instruction.NMI);
                }
                // If a maskable interrupt is pending, execute INT instruction
                else if (maskableInterruptPending)
                {
                    maskableInterruptPending = false;
                    switch (IM)
                    {
                        case 0:
                            StartInstruction(Instruction.INT0);
                            break;
                        case 1:
                            StartInstruction(Instruction.INT1);
                            break;
                        case 2:
                            StartInstruction(Instruction.INT2);
                            break;
                    }
                }
                // If no external CPU control Pin is active, read the next instruction opcode in memory
                else
                {
                    StartInstruction(Instruction.FetchNewInstruction);
                }

                // Debug notification
                if (pendingExitException == null)
                {
                    pendingExitException = NotifyLifecycleEvent(LifecycleEventType.InstructionStart);
                }
            }

            // Start a new machine cycle if no machine cycle is currently in progress
            if (currentMachineCycle == null)
            {
                // If a Bus Request is pending, start a special Bus Request Acknowledge machine cycle
                if (busRequestPending)
                {
                    busRequestPending = false;
                    StartMachineCycle(MachineCycle.BusRequestAcknowledge);
                }
                // The following machine cycles are defined by the documentation of the current instruction
                else
                {
                    MachineCycle nextMachineCycle = currentInstruction.ExecutionTimings.MachineCycles[machineCycleIndex];
                    StartMachineCycle(nextMachineCycle);
                }

                // Debug notification
                if (pendingExitException == null)
                {
                    pendingExitException = NotifyLifecycleEvent(LifecycleEventType.MachineCycleStart);
                }
            }

            // Execute the current machine cycle logic during one halfTState
            executeMachineCycle(halfTStateIndex);

            // Check if the current machine cycle was a bus request
            bool machineCycleWasBusRequest = currentMachineCycle.Type == MachineCycleType.BRQA;

            // Check if this halfTState is the rising edge of the last clock period of this machine cycle
            bool isRisingEdgeOfLastClockPeriodOfCurrentMachineCycle = (halfTStateIndex == currentMachineCycle.PenultimateHalfTStateIndex);

            // Check if this halfTState is the last one for the current machine cycle
            bool isLastHalfTStateOfCurrentMachineCycle = (halfTStateIndex == currentMachineCycle.LastHalfTStateIndex);
                     
            // Check if the current machine cycle was the last machine cycle of the instruction
            bool isLastMachineCycleOfCurrentInstruction = !machineCycleWasBusRequest && (machineCycleIndex == currentInstruction.LastMachineCycleIndex);
            
            // Sample BUSREQ, NMI, and INT signals at the rising edge of the last clock period of any machine cycle
            if (isRisingEdgeOfLastClockPeriodOfCurrentMachineCycle)
            {
                SampleBusControlSignals();
                if(isLastMachineCycleOfCurrentInstruction)
                {
                    SampleInterruptControlSignals();
                }
            }

            // Debug notification
            if (pendingExitException == null)
            {
                pendingExitException = NotifyLifecycleEvent(LifecycleEventType.HalfTState);
            }

            // If machine cycle is finished       
            if (isLastHalfTStateOfCurrentMachineCycle)
            {
                // Execute one last time the machine cycle logic 
                // to release buses and controls signals before the next machine cycle
                executeMachineCycle((byte)(halfTStateIndex + 1));

                // Debug notification
                if (pendingExitException == null)
                {
                    pendingExitException = NotifyLifecycleEvent(LifecycleEventType.MachineCycleEnd);
                }

                EndMachineCycle();          
            }

            // If instruction is finished
            if (isLastMachineCycleOfCurrentInstruction && isLastHalfTStateOfCurrentMachineCycle)
            {
                // Debug notification
                if (pendingExitException == null)
                {
                    pendingExitException = NotifyLifecycleEvent(LifecycleEventType.InstructionEnd);
                }
                
                EndInstruction();
            }
            
            // Count half T states whithin one machine cycle
            if (isLastHalfTStateOfCurrentMachineCycle)
            {
                halfTStateIndex = 0;                
            }
            else
            {
                halfTStateIndex++;
            }

            // Count machine cycles within one instruction
            if (isLastHalfTStateOfCurrentMachineCycle)
            {
                if (isLastMachineCycleOfCurrentInstruction)
                {
                    machineCycleIndex = 0;
                    machineCycleCountAfterInstruction = 0;
                }
                else
                {
                    if (!machineCycleWasBusRequest) // Bus request "steals" machine cycles from the instruction
                    {
                        machineCycleIndex++;
                        if (currentInstruction != Instruction.FetchNewInstruction) // Incremented only after instruction decoding
                        {
                            machineCycleCountAfterInstruction++;
                        }
                    }
                }
            }

            // Debug : if a breakpoint was hit, notify the system clock via a specific exception
            if(pendingExitException != null)
            {
                throw pendingExitException;
            }
        }       
        
        private void StartInstruction(Instruction instruction)
        {
            if (instruction == Instruction.RESET)
            {
                // Instruction count should be zero after a RESET
                instructionCounter = 0;                
            }
            else
            {
                // Any other internal instruction increments the instruction counter    
                instructionCounter++;
            }

            if (instruction == Instruction.FetchNewInstruction)
            {
                instructionOrigin = new InstructionOrigin(InstructionSource.Memory, null, PC);
            }
            else
            {
                instructionOrigin = InstructionOrigin.Internal;
            }
            currentInstruction = instruction;
            DecodeInstructionExecutionMethod();
        }
        
        private void EndInstruction()
        {
            instructionOrigin = null;
            currentInstruction = null;
            executeInstruction = null;
        }

        private void StartMachineCycle(MachineCycle machineCycle)
        {
            machineCycleCounter++;

            currentMachineCycle = machineCycle;
            DecodeMachineCycleExecutionMethod();
        }

        private void AddOneTStateToCurrentMachineCycleIfSignalLow(SignalState signalState, string signalName)
        {
            if (signalState == SignalState.LOW)
            {
                // Add one more TState to the machine cycle by decreasing the halfTState counter
                this.halfTStateIndex -= 2;
            }

            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.CPUControlAddOneTStateIfSignalLow, signalName));
            }
        }

        private void EndMachineCycle()
        {
            currentMachineCycle = null;
            executeMachineCycle = null;
        }        

        #endregion
    }
}
