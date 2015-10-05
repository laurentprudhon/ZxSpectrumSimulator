using System;
using Z80Simulator.Instructions;
using Z80Simulator.System;

namespace Z80Simulator.CPU
{
    /// <summary>
    /// ZiLOG Z80 CPU
    /// 
    /// Machine cycles logic
    /// 
    /// The Z80 CPU executes instructions by stepping through a precise set of
    /// basic operations. These include:
    /// - Memory Read or Write
    /// - I/O Device Read or Write
    /// - Interrupt Acknowledge
    /// 
    /// All instructions are series of basic operations. Each of these operations can
    /// take from three to six clock periods to complete or they can be lengthened to
    /// synchronize the CPU to the speed of external devices. The clock periods are
    /// referred to as T (time) cycles and the operations are referred to as M
    /// (machine) cycles. 
    /// 
    /// The first machine cycle of any
    /// instruction is a fetch cycle which is four, five, or six T cycles long (unless
    /// lengthened by the WAIT signal, which is described in the next section). The
    /// fetch cycle (M1) is used to fetch the opcode of the next instruction to be
    /// executed. 
    /// 
    /// Subsequent machine cycles move data between the CPU and
    /// memory or I/O devices, and they may have anywhere from three to five T
    /// cycles (again, they may be lengthened by wait states to synchronize the
    /// external devices to the CPU). 
    /// </summary>
    public partial class Z80CPU
    {
        // Prototype for all the machine cycles
        private delegate void ExecuteMachineCycle(byte halfTStateIndex);

        // Select the right machine cycle implementation for each machine cycle type
        private void DecodeMachineCycleExecutionMethod()
        {
            switch (currentMachineCycle.Type)
            {
                case MachineCycleType.OCF:
                    executeMachineCycle = FetchOpcode;
                    break;
                case MachineCycleType.OC4:
                    executeMachineCycle = FetchLastOpcode;
                    break;
                case MachineCycleType.CPU:
                    executeMachineCycle = InternalCPUOperation;
                    break;
                case MachineCycleType.MR:
                case MachineCycleType.MRH:
                case MachineCycleType.MRL:
                case MachineCycleType.OD:
                case MachineCycleType.ODH:
                case MachineCycleType.ODL:
                case MachineCycleType.SRH:
                case MachineCycleType.SRL:
                    executeMachineCycle = MemoryRead;
                    break;
                case MachineCycleType.MW:
                case MachineCycleType.MWH:
                case MachineCycleType.MWL:
                case MachineCycleType.SWH:
                case MachineCycleType.SWL:
                    executeMachineCycle = MemoryWrite;
                    break;
                case MachineCycleType.PR:
                    executeMachineCycle = Input;
                    break;
                case MachineCycleType.PW:
                    executeMachineCycle = Output;
                    break;
                case MachineCycleType.INTA:
                    executeMachineCycle = InterruptRequestAcknowledge;
                    break;
                case MachineCycleType.BRQA:
                    executeMachineCycle = BusRequestAcknowledge;
                    break;
                default:
                    throw new Exception(String.Format("Machine cycle type {0} is not yet implemented", currentMachineCycle.Type.ToString()));
            }
        }

        /// <summary>
        /// Instruction Fetch cycle (user manual page 12)
        /// </summary>
        private void FetchOpcode(byte halfTStateIndex)
        {
            switch (halfTStateIndex)
            {
                case 0:
                    // The PC is placed on the address bus at the beginning of the M1 cycle.
                    M1 = SignalState.LOW;
                    // The PC is automatically incremented after its contents have been transferred to the address lines
                    // (except for internal CPU instructions)
                    InternalAddressBus_SampleFromPCAndIncrementForInstruction();
                    InternalAddressBus_SendTo(InternalAddressBusConnection.AddressBus);
                    break;
                case 1:
                    // One half clock cycle later the MREQ signal goes active. At this time the address to the
                    // memory has had time to stabilize so that the falling edge of MREQ can be
                    // used directly as a chip enable clock to dynamic memories. 
                    MREQ = SignalState.LOW;
                    // The RD line also goes active to indicate that the memory read data should be enabled onto the
                    // CPU data bus. 
                    RD = SignalState.LOW;
                    break;
                case 2:
                    break;
                case 3:
                    SampleWaitSignal();
                    break;
                case 4:
                    // The CPU samples the data from the memory on the data bus
                    // with the rising edge of the clock of state T3.                   
                    InternalDataBus_SampleFrom(InternalDataBusConnection.DataBus);
                    InternalDataBus_SendTo(InternalDataBusConnection.IR);

                    // This same edge is used by
                    // the CPU to turn off the RD and MREQ signals. Thus, the data has already
                    // been sampled by the CPU before the RD signal becomes inactive.
                    RD = SignalState.HIGH;
                    MREQ = SignalState.HIGH;
                    Address.ReleaseValue();
                    M1 = SignalState.HIGH;

                    // Clock state T3 and T4 of a fetch cycle are used to refresh dynamic memories. 
                    // The CPU uses this time to decode and execute the fetched instruction so that no
                    // other operation could be performed at this time.
                    DecodeInstruction();

                    // During every first machine cycle (beginning of an instruction or part of it, prefixes have their
                    // own M1 two), the memory refresh cycle is issued. The whole I+R register is put on the address
                    // bus, and the RFSH pin is lowered. It is unclear whether the Z80 increases the R register before
                    // or after putting I+R on the bus.                    
                    InternalAddressBus_SampleFromAndIncrement(InternalAddressBusConnection.IandR);
                    InternalAddressBus_SendTo(InternalAddressBusConnection.AddressBus);

                    // During T3 and T4, the lower seven bits of the address bus contain a memory
                    // refresh address and the RFSH signal becomes active indicating that a
                    // refresh read of all dynamic memories must be accomplished. An RD signal
                    // is not generated during refresh time to prevent data from different memory
                    // segments from being gated onto the data bus.
                    RFSH = SignalState.LOW;
                    break;
                case 5:
                    // The MREQ signal during
                    // refresh time should be used to perform a refresh read of all memory
                    // elements. The refresh signal can not be used by itself because the refresh
                    // address is only guaranteed to be stable during MREQ time.
                    MREQ = SignalState.LOW;
                    break;
                case 7:
                    MREQ = SignalState.HIGH;
                    // The instruction can execute internal commands at the end of the machine cycle
                     executeInstruction(machineCycleCountAfterInstruction, halfTStateIndex);
                    break;
                case 8:
                    RFSH = SignalState.HIGH;
                    Address.ReleaseValue();
                    break;
            }
            if (halfTStateIndex > 8)
            {
                // Some instructions need to execute internal CPU operations after the end the external exchange
                executeInstruction(machineCycleCountAfterInstruction, halfTStateIndex);
            }
        }

        /// <summary>
        /// Last Instruction Fetch cycle for 4 bytes opcodes (undocumented)
        /// </summary>
        private void FetchLastOpcode(byte halfTStateIndex)
        {
            switch (halfTStateIndex)
            {
                case 0:
                    // The last opcode fetch is not a M1 cycle - with no memory refresh

                    // The PC is automatically incremented after its contents have been transferred to the address lines
                    // (except for internal CPU instructions)
                    InternalAddressBus_SampleFromPCAndIncrementForInstruction();
                    InternalAddressBus_SendTo(InternalAddressBusConnection.AddressBus);
                    break;
                case 1:
                    // One half clock cycle later the MREQ signal goes active. At this time the address to the
                    // memory has had time to stabilize so that the falling edge of MREQ can be
                    // used directly as a chip enable clock to dynamic memories. 
                    MREQ = SignalState.LOW;
                    // The RD line also goes active to indicate that the memory read data should be enabled onto the
                    // CPU data bus. 
                    RD = SignalState.LOW;
                    break;
                case 2:
                    break;
                case 3:
                    SampleWaitSignal();
                    break;
                case 4:
                    // The CPU samples the data from the memory on the data bus
                    // with the rising edge of the clock of state T3.                   
                    InternalDataBus_SampleFrom(InternalDataBusConnection.DataBus);
                    InternalDataBus_SendTo(InternalDataBusConnection.IR);

                    // This same edge is used by
                    // the CPU to turn off the RD and MREQ signals. Thus, the data has already
                    // been sampled by the CPU before the RD signal becomes inactive.
                    RD = SignalState.HIGH;
                    MREQ = SignalState.HIGH;
                    Address.ReleaseValue();

                    // Clock state T3 and T4 of a fetch cycle are used to refresh dynamic memories. 
                    // The CPU uses this time to decode and execute the fetched instruction so that no
                    // other operation could be performed at this time.
                    DecodeInstruction();

                    // The last opcode fetch is not a M1 cycle - with no memory refresh
                    break;                
                case 7:
                    // The instruction can execute internal commands at the end of the machine cycle
                    executeInstruction(machineCycleCountAfterInstruction, halfTStateIndex);
                    break;
            }
            if (halfTStateIndex >= 8)
            {
                // Some instructions need to execute internal CPU operations after the end the external exchange
                executeInstruction(machineCycleCountAfterInstruction, halfTStateIndex);
            }
        }

        /// <summary>
        /// Memory Read cycle (user manual page 13)
        /// </summary>
        private void MemoryRead(byte halfTStateIndex)
        {
            switch (halfTStateIndex)
            {
                case 0:
                    switch (currentMachineCycle.Type)
                    {
                        // Read operand -> send PC to address bus and increment PC
                        case MachineCycleType.OD:
                        case MachineCycleType.ODH:
                        case MachineCycleType.ODL:
                            InternalAddressBus_SampleFromAndIncrement(InternalAddressBusConnection.PC);
                            break;
                        // Pop data from the stack in memory -> send SP to addressBus and increment SP
                        case MachineCycleType.SRH:
                        case MachineCycleType.SRL:
                            InternalAddressBus_SampleFromAndIncrement(InternalAddressBusConnection.SP);
                            break;
                        // Read an address in memory -> the source of the address depends on the instruction
                        case MachineCycleType.MR:
                        case MachineCycleType.MRH:
                        case MachineCycleType.MRL:
                            // The instruction must place a memory address on the internal address bus
                            executeInstruction(machineCycleCountAfterInstruction, halfTStateIndex);
                            break;
                    }
                    InternalAddressBus_SendTo(InternalAddressBusConnection.AddressBus);
                    break;
                case 1:
                    // The MREQ signal and the RD signal are used the same as in the fetch cycle.
                    MREQ = SignalState.LOW;
                    RD = SignalState.LOW;
                    break;
                case 2:
                    break;
                case 3:
                    // These cycles are generally three clock periods long
                    // unless wait states are requested by the memory through the WAIT signal.
                    SampleWaitSignal();
                    break;
                case 5:
                    InternalDataBus_SampleFrom(InternalDataBusConnection.DataBus);
                    RD = SignalState.HIGH;
                    MREQ = SignalState.HIGH;

                    // The instruction must read data from the internal data bus
                    executeInstruction(machineCycleCountAfterInstruction, halfTStateIndex);
                    break;
                case 6:
                    Address.ReleaseValue();
                    break;
            }
            if (halfTStateIndex > 6)
            {
                // Some instructions need to execute internal CPU operations after the end the external exchange
                executeInstruction(machineCycleCountAfterInstruction, halfTStateIndex);
            }
        }

        /// <summary>
        /// Memory Write cycle (user manual page 13)
        /// </summary>
        private void MemoryWrite(byte halfTStateIndex)
        {
            switch (halfTStateIndex)
            {
                case 0:
                    switch (currentMachineCycle.Type)
                    {
                        // Push data to the stack in memory -> decrement SP and send SP to addressBus
                        case MachineCycleType.SWH:
                        case MachineCycleType.SWL:
                            InternalAddressBus_DecrementAndSampleFrom(InternalAddressBusConnection.SP);
                            break;
                        default:
                            // The instruction must place a memory address on the internal address bus
                            break;
                    }
                    // The instruction must place data on the internal data bus
                    executeInstruction(machineCycleCountAfterInstruction, halfTStateIndex);                 
                    InternalAddressBus_SendTo(InternalAddressBusConnection.AddressBus);
                    break;
                case 1:
                    // In a memory write cycle, the MREQ also becomes active when the address
                    // bus is stable so that it can be used directly as a chip enable for dynamic
                    // memories. 
                    MREQ = SignalState.LOW;
                    InternalDataBus_SendTo(InternalDataBusConnection.DataBus);
                    break;
                case 2:
                    break;
                case 3:
                    // The WR line is active when data on the data bus is stable so that
                    // it can be used directly as a R/W pulse to virtually any type of semiconductor
                    // memory. 
                    WR = SignalState.LOW;

                    // These cycles are generally three clock periods long
                    // unless wait states are requested by the memory through the WAIT signal.
                    SampleWaitSignal();
                    break;
                case 5:
                    // Furthermore, the WR signal goes inactive one-half T state before
                    // the address and data bus contents are changed so that the overlap
                    // requirements for almost any type of semiconductor memory type is met.
                    WR = SignalState.HIGH;
                    MREQ = SignalState.HIGH;

                    // Some instructions (CALL) need to execute internal logic at this point
                    executeInstruction(machineCycleCountAfterInstruction, halfTStateIndex);
                    break;
                case 6:
                    Address.ReleaseValue();
                    Data.ReleaseValue();
                    break;
            }
            if (halfTStateIndex > 6)
            {
                // Some instructions need to execute internal CPU operations after the end the external exchange
                executeInstruction(machineCycleCountAfterInstruction, halfTStateIndex);
            }
        }

        /// <summary>
        /// Input cycle (user manual page 14)
        /// </summary>
        private void Input(byte halfTStateIndex)
        {
            switch (halfTStateIndex)
            {
                case 0:
                     // The instruction must place a port address on the internal address bus
                    executeInstruction(machineCycleCountAfterInstruction, halfTStateIndex);
                    InternalAddressBus_SendTo(InternalAddressBusConnection.AddressBus);
                    break;
                case 2:
                    IORQ = SignalState.LOW;
                    // During a read I/O operation, the RD line is used to enable the addressed port
                    // onto the data bus just as in the case of a memory read.
                    RD = SignalState.LOW;
                    break;
                case 5:
                    // During I/O operations,
                    // a single wait state is automatically inserted. The reason is that during I/O
                    // operations, the time from when the IORQ signal goes active until the CPU
                    // must sample the WAIT line is very short. Without this extra state, sufficient
                    // time does not exist for an I/O port to decode its address and activate the
                    // WAIT line if a wait is required. Also, without this wait state, it is difficult to
                    // design MOS I/O devices that can operate at full CPU speed. During this wait
                    // state time, the WAIT request signal is sampled.
                    SampleWaitSignal();
                    break;
                case 7:
                    InternalDataBus_SampleFrom(InternalDataBusConnection.DataBus);
                    RD = SignalState.HIGH;
                    IORQ = SignalState.HIGH;

                    // The instruction must read data from the internal data bus
                    executeInstruction(machineCycleCountAfterInstruction, halfTStateIndex);
                    break;
                case 8:
                    Address.ReleaseValue();
                    break;
            }
        }

        /// <summary>
        /// Output cycle (user manual page 14)
        /// </summary>
        private void Output(byte halfTStateIndex)
        {
            switch (halfTStateIndex)
            {
                case 0:
                    // The instruction must place data on the internal data bus
                    // The instruction must place a port address on the internal address bus
                    executeInstruction(machineCycleCountAfterInstruction, halfTStateIndex);
                    InternalAddressBus_SendTo(InternalAddressBusConnection.AddressBus);
                    break;
                case 1:
                    InternalDataBus_SendTo(InternalDataBusConnection.DataBus);
                    break;
                case 2:
                    IORQ = SignalState.LOW;
                    // For I/O write operations, the WR line is used as a clock to the I/O port.
                    WR = SignalState.LOW;
                    break;
                case 5:
                    // During I/O operations,
                    // a single wait state is automatically inserted. The reason is that during I/O
                    // operations, the time from when the IORQ signal goes active until the CPU
                    // must sample the WAIT line is very short. Without this extra state, sufficient
                    // time does not exist for an I/O port to decode its address and activate the
                    // WAIT line if a wait is required. Also, without this wait state, it is difficult to
                    // design MOS I/O devices that can operate at full CPU speed. During this wait
                    // state time, the WAIT request signal is sampled.
                    SampleWaitSignal();
                    break;
                case 7:
                    WR = SignalState.HIGH;
                    IORQ = SignalState.HIGH;

                    // The instruction may need to execute an internal operation after the port write
                    executeInstruction(machineCycleCountAfterInstruction, halfTStateIndex);
                    break;
                case 8:
                    Data.ReleaseValue();
                    Address.ReleaseValue();
                    break;
            }
        }
        
        /// <summary>
        /// Execute internal CPU Operation
        /// </summary>
        private void InternalCPUOperation(byte halfTStateIndex)
        {
            // The instruction can execute any internal operation during each halfTState
            executeInstruction(machineCycleCountAfterInstruction, halfTStateIndex);
        }

        /// <summary>
        /// Interrupt Request/Acknowledge Cycle (user manual page 17)
        /// </summary>
        private void InterruptRequestAcknowledge(byte halfTStateIndex)
        {
            // When the interrupt signal is accepted, a special M1 cycle is generated. 
            // During this special M1 cycle, the IORQ signal becomes active
            // (instead of the normal MREQ) to indicate that the interrupting device can
            // place an 8-bit vector on the data bus. Two wait states are automatically
            // added to this cycle. These states are added so that a ripple priority interrupt
            // scheme can be easily implemented. The two wait states allow sufficient time
            // for the ripple signals to stabilize and identify which
            // I/O device must insert the response vector.

            switch (halfTStateIndex)
            {
                case 0:
                    // When an INT is accepted, both IFF1 and IFF2 are cleared, preventing another interrupt from occurring 
                    // which would end up as an inﬁnite loop (and overﬂowing the stack).
                    CPUControl_ResetIFFs();
                    // The PC is placed on the address bus at the beginning of the M1 cycle.
                    M1 = SignalState.LOW;
                    InternalAddressBus_SampleFrom(InternalAddressBusConnection.PC);
                    InternalAddressBus_SendTo(InternalAddressBusConnection.AddressBus);
                    break;
                case 5:
                    // The IORQ signal becomes active (instead of the normal MREQ) to indicate that the interrupting device can
                    // place an 8-bit vector on the data bus
                    IORQ = SignalState.LOW;
                    break;
                case 7:
                    SampleWaitSignal();
                    break;
                case 8:
                    // The CPU samples the data from the interrupting device on the data bus
                    InternalDataBus_SampleFrom(InternalDataBusConnection.DataBus);
                    // The instruction can read here the opcode or vector placed by the interrupting device on the data bus
                    executeInstruction(machineCycleCountAfterInstruction, halfTStateIndex);
                    
                    // This same edge is used by the CPU to turn off the IORQ signal. 
                    // Thus, the data has already been sampled by the CPU before the IORQ signal becomes inactive.
                    IORQ = SignalState.HIGH;
                    Address.ReleaseValue();
                    M1 = SignalState.HIGH;
                   
                    // During every first machine cycle (beginning of an instruction or part of it, prefixes have their
                    // own M1 two), the memory refresh cycle is issued. The whole I+R register is put on the address
                    // bus, and the RFSH pin is lowered. It is unclear whether the Z80 increases the R register before
                    // or after putting I+R on the bus.                    
                    InternalAddressBus_SampleFromAndIncrement(InternalAddressBusConnection.IandR);
                    InternalAddressBus_SendTo(InternalAddressBusConnection.AddressBus);

                    // During T3 and T4, the lower seven bits of the address bus contain a memory
                    // refresh address and the RFSH signal becomes active indicating that a
                    // refresh read of all dynamic memories must be accomplished. An RD signal
                    // is not generated during refresh time to prevent data from different memory
                    // segments from being gated onto the data bus.
                    RFSH = SignalState.LOW;
                    break;
                case 9:
                    // The MREQ signal during
                    // refresh time should be used to perform a refresh read of all memory
                    // elements. The refresh signal can not be used by itself because the refresh
                    // address is only guaranteed to be stable during MREQ time.
                    MREQ = SignalState.LOW;
                    break;
                case 11:
                    MREQ = SignalState.HIGH; 
                    break;
                case 12:
                    RFSH = SignalState.HIGH;
                    Address.ReleaseValue();
                    break;
                case 13:
                    // The instruction can execute internal commands at the end of the machine cycle
                    executeInstruction(machineCycleCountAfterInstruction, halfTStateIndex);
                    break;
            }
        }

        /// <summary>
        /// Bus Request/Acknowledge Cycle (user manual page 16)
        /// </summary>
        private void BusRequestAcknowledge(byte halfTStateIndex)
        {
            // If the BUSREQ signal is active, the CPU sets its address, data, and tristate control signals 
            // to the high-impedance state with the rising edge of the next clock pulse. At that time, any 
            // external device can control the buses to transfer data between memory and I/O devices. (This
            // operation is generally known as Direct Memory Access [DMA] using cycle stealing.)

            // Enter DMA mode during the first halfTState
            if (halfTStateIndex == 0)
            {
                BusControl_EnterDMAMode();
                BUSACK = SignalState.LOW;
            }
            // Check BUSREQ at each rising edge
            else if (halfTStateIndex == 2)
            {
                // Continue in DMA mode until BUSREQ has been released
                AddOneTStateToCurrentMachineCycleIfSignalLow(BUSREQ, "BUSREQ");
            }
            // Exit DMA mode at the beginning and at the end of the last halfTState, after BUSREQ has been released
            else if (halfTStateIndex == 3)
            {                
                BUSACK = SignalState.HIGH;
            }
            else if (halfTStateIndex == 4)
            {
                BusControl_ExitDMAMode();
            }
        }
        
        /// <summary>
        /// During T2 and every subsequent Tw, the CPU samples the WAIT line with
        /// the falling edge of Clock. If the WAIT line is active at this time, another
        /// WAIT state is entered during the following cycle. Using this technique, the
        /// read can be lengthened to match the access time of any type of memory
        /// device.
        /// </summary>
        private void SampleWaitSignal()
        {
            AddOneTStateToCurrentMachineCycleIfSignalLow(WAIT, "WAIT");
        }                
    }
}