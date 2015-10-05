using Z80Simulator.Instructions;
using Z80Simulator.System;

namespace Z80Simulator.CPU
{
    /// <summary>
    /// ZiLOG Z80 CPU
    /// 
    /// External CPU Control interface
    /// 
    /// Bus request, Halt, Interrupts
    /// </summary>
    public partial class Z80CPU
    {
        #region Bus request

        // Sample of BUSREQ signal
        // (at the rising edge of the last clock period of any machine cycle)
        private bool busRequestPending = false;

        private void SampleBusControlSignals()
        {
            // BUSREQ signal is sampled by the CPU with the rising edge of the last clock
            // period of any machine cycle.
            if (BUSREQ == SignalState.LOW)
            {
                busRequestPending = true;
            }
        }

        /// <summary>
        /// Sets all address bus, data bus, and tristate control signals to the high-impedance state
        /// </summary> 
        private void BusControl_EnterDMAMode()
        {
            // The address and data bus go to a high-impedance state, 
            Address.ReleaseValue();
            Data.ReleaseValue();

            // Nothing to do for the external control signals because we do not simulate high-impedance
            // state yet for performance reasons (they are not tristate)

            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.CPUControlEnterDMAMode));
            }
        }

        /// <summary>
        /// Restores all tristate control signals to the inactive state
        /// </summary>
        private void BusControl_ExitDMAMode()
        {
            // Nothing to do for the external control signals because we do not simulate high-impedance
            // state yet for performance reasons

            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.CPUControlExitDMAMode));
            }
        }

        #endregion

        #region Halt

        /// <summary>
        // HALT mode - start
        /// </summary>
        private void CPUControl_EnterOrExitHaltMode()
        {
            if (HALT == SignalState.HIGH) // enter halt mode only if CPU is not ALREADY in halt mode
            {
                // Each cycle in the HALT state is a normal M1 (fetch) cycle except that the data received from the memory is ignored and a NOP instruction is forced internally to the CPU. 
                // The purpose of executing NOP instructions while in the HALT state is to keep the memory refresh signals active. 
                // The HALT acknowledge signal is active during this time indicating that the processor is in the HALT state.
                HALT = SignalState.LOW;
            }

            // The two interrupt lines are sampled with the rising clock edge during each T4 state.
            // If a non-maskable interrupt has been received or a maskable interrupt has been received and the interrupt enable flip-flop is set, 
            // then the HALT state is exited on the next rising clock edge. 
            // The following cycle is an interrupt acknowledge cycle corresponding to the type of interrupt that was received.
            if (nonMaskableInterruptPending || (maskableInterruptPending && IFF1))
            {
                HALT = SignalState.HIGH;
            }
            else
            {
                // HALT Instruction is repeated during this Memory Cycle
                Register16_Decrement(InternalAddressBusConnection.PC);
            }

            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.CPUControlEnterOrExitHaltMode));
            }
        }

        #endregion

        #region Interrupts

        // Sample of INT signal
        // (at the rising edge of the last clock period at the end of any instruction if IFF1 is set)
        private bool maskableInterruptPending = false;

        // Sample of NMI signal
        // (at the rising edge of the last clock period at the end of any instruction)
        private bool nonMaskableInterruptPending = false;

        private void SampleInterruptControlSignals()
        {            
            // If NMI Pin is active, register a pending non maskable interrupt.
            // NMI has a higher priority than INT.
            if (NMI == SignalState.LOW)
            {
                nonMaskableInterruptPending = true;
            }
            // If INT Pin is active and IFF1 is set and current instruction is not EI,
            // register a pending maskable interrupt
            else if (INT == SignalState.LOW && IFF1 && currentInstruction.Type.Index != 37/*EI*/)
            {
                maskableInterruptPending = true;
            }
        }

        /// <summary>
        /// Interrupt enable flip-flop
        /// 
        /// In the Z80 CPU, there is an interrupt enable flip-flop (IFF) that is set or reset 
        /// by the programmer using the Enable Interrupt (EI) and Disable Interrupt (DI) instructions. 
        /// When the IFF is reset, an interrupt cannot be accepted by the CPU.
        /// The state of IFF1 is used to inhibit interrupts.
        /// </summary>
        public bool IFF1
        {
            get;
            private set;
        }

        /// <summary>
        /// IFF2 is used as a temporary storage location for IFF1.
        /// </summary>
        public bool IFF2
        {
            get;
            private set;
        }

        /// <summary>
        /// Interrupt Mode
        /// 
        /// The CPU can be programmed to respond to the maskable interrupt in any one of three possible modes.
        /// * Mode 0
        /// With this mode, the interrupting device can place any instruction on the data bus and the CPU executes it.
        /// * Mode 1
        /// When this mode is selected by the programmer, the CPU responds to an interrupt by executing a restart to location 0038H. 
        /// * Mode 2
        /// In this mode, the programmer maintains a table of 16-bit starting addresses for every interrupt service routine. 
        /// This table may be located anywhere in memory. When an interrupt is accepted, a 16-bit pointer must be formed to
        /// obtain the desired interrupt service routine starting address from the table.
        /// The upper eight bits of this pointer is formed from the contents of the I register. 
        /// The lower eight bits of the pointer must be supplied by the interrupting device. 
        /// </summary>
        public byte IM
        {
            get;
            private set;
        }

        /// <summary>
        /// Interrupt Page Address Register (I)
        /// 
        /// The Z80 CPU can be operated in a mode where an indirect call to any
        /// memory location can be achieved in response to an interrupt. The I register
        /// is used for this purpose and stores the high order eight bits of the indirect
        /// address while the interrupting device provides the lower eight bits of the
        /// address. This feature allows interrupt routines to be dynamically located
        /// anywhere in memory with minimal access time to the routine.         
        /// </summary>
        public byte I { get; private set; }

        private void CPUControl_ResetIFFs()
        {
            IFF1 = false;
            IFF2 = false;
            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.CPUControlResetIFFs));
            }
        }

        private void CPUControl_SetIFFs()
        {
            IFF1 = true;
            IFF2 = true;
            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.CPUControlSetIFFs));
            }
        }

        private void CPUControl_ResetIFF1()
        {
            IFF1 = false;
            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.CPUControlResetIFF1));
            }
        }

        private void CPUControl_CopyIFF2ToIFF1()
        {
            IFF1 = IFF2;
            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.CPUControlCopyIFF2ToIFF1));
            }
        }

        private void CPUControl_SetInterruptMode(InterruptMode interruptMode)
        {
            switch (interruptMode)
            {
                case InterruptMode.IM0:
                    IM = 0;
                    break;
                case InterruptMode.IM1:
                    IM = 1;
                    break;
                case InterruptMode.IM2:
                    IM = 2;
                    break;
            }
            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.CPUControlSetInterruptMode));
            }
        }

        #endregion
    }
}
