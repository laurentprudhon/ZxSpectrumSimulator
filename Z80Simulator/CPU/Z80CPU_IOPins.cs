using Z80Simulator.Instructions;
using Z80Simulator.System;

namespace Z80Simulator.CPU
{
    /// <summary>
    /// ZiLOG Z80 CPU
    /// 
    /// I/O Pins
    /// 
    /// The Z80 CPU is integrated in a computer system through 40 I/O pins.
    /// </summary>
    public abstract partial class Z80CPU
    {
        #region Address Bus

        /// <summary>
        /// A15–A0
        /// 
        /// Address Bus (output, active High, tristate). A15-A0 form a 16-bit address
        /// bus. The Address Bus provides the address for memory data bus exchanges
        /// (up to 64 Kbytes) and for I/O device exchanges.
        /// </summary>
        public Z80CPUBusConnector<ushort> Address { get; private set; }

        #endregion

        #region Data bus

        /// <summary>
        /// D7–D0
        /// 
        /// Data Bus (input/output, active High, tristate). D7–D0 constitute an
        /// 8-bit bidirectional data bus, used for data exchanges with memory and I/O.
        /// </summary>
        public Z80CPUBusConnector<byte> Data { get; private set; }

        #endregion

        #region System Control

        /// <summary>
        /// M1
        /// 
        /// Machine Cycle One (output, active Low). M1, together with MREQ,
        /// indicates that the current machine cycle is the opcode fetch cycle of an
        /// instruction execution. M1 together with IORQ, indicates an interrupt
        /// acknowledge cycle.
        /// </summary>        
        public virtual SignalState M1 
        {
            get { return _m1; }
            protected set
            {
                _m1 = value;
                if (TraceMicroInstructions)
                {
                    if (_m1 == SignalState.LOW)
                    {
                        TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.OutputPinSetLow, "M1"));
                    }
                    else
                    {
                        TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.OutputPinSetHigh, "M1"));
                    }
                }
            }
        }
        private SignalState _m1;

        /// <summary>
        /// MREQ
        /// 
        /// Memory Request (output, active Low, tristate). MREQ indicates that the
        /// address bus holds a valid address for a memory read of memory write
        /// operation.
        /// </summary>
        public virtual SignalState MREQ
        {
            get { return _mreq; }
            protected set
            {
                _mreq = value;
                if (TraceMicroInstructions)
                {
                    if (_mreq == SignalState.LOW)
                    {
                        TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.OutputPinSetLow, "MREQ"));
                    }
                    else
                    {
                        TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.OutputPinSetHigh, "MREQ"));
                    }
                }
            }
        }
        private SignalState _mreq;                

        /// <summary>
        ///  IORQ
        ///  
        /// Input/Output Request (output, active Low, tristate). IORQ indicates that
        /// the lower half of the address bus holds a valid I/O address for an I/O read or
        /// write operation. IORQ is also generated concurrently with M1 during an
        /// interrupt acknowledge cycle to indicate that an interrupt response vector can
        /// be placed on the data bus.
        /// </summary>
        public virtual SignalState IORQ
        {
            get { return _iorq; }
            protected set
            {
                _iorq = value;
                if (TraceMicroInstructions)
                {
                    if (_iorq == SignalState.LOW)
                    {
                        TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.OutputPinSetLow, "IOREQ"));
                    }
                    else
                    {
                        TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.OutputPinSetHigh, "IOREQ"));
                    }
                }
            }
        }
        private SignalState _iorq;                

        /// <summary>
        /// RD
        /// 
        /// Read (output, active Low, tristate). RD indicates that the CPU wants to
        /// read data from memory or an I/O device. The addressed I/O device or
        /// memory should use this signal to gate data onto the CPU data bus.
        /// </summary>
        public virtual SignalState RD
        {
            get { return _rd; }
            protected set
            {
                _rd = value;
                if (TraceMicroInstructions)
                {
                    if (_rd == SignalState.LOW)
                    {
                        TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.OutputPinSetLow, "RD"));
                    }
                    else
                    {
                        TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.OutputPinSetHigh, "RD"));
                    }
                }
            }
        }
        protected SignalState _rd;                

        /// <summary>
        /// WR
        /// 
        /// Write (output, active Low, tristate). WR indicates that the CPU data bus
        /// holds valid data to be stored at the addressed memory or I/O location.
        /// </summary>
        public virtual SignalState WR
        {
            get { return _wr; }
            protected set
            {
                _wr = value;
                if (TraceMicroInstructions)
                {
                    if (_wr == SignalState.LOW)
                    {
                        TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.OutputPinSetLow, "WR"));
                    }
                    else
                    {
                        TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.OutputPinSetHigh, "WR"));
                    }
                }
            }
        }
        protected SignalState _wr;

        /// <summary>
        /// RFSH
        /// 
        /// Refresh (output, active Low). RFSH, together with MREQ indicates that
        /// the lower seven bits of the system’s address bus can be used as a refresh
        /// address to the system’s dynamic memories.
        /// </summary>
        public virtual SignalState RFSH
        {
            get { return _rfsh; }
            protected set
            {
                _rfsh = value;
                if (TraceMicroInstructions)
                {
                    if (_rfsh == SignalState.LOW)
                    {
                        TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.OutputPinSetLow, "RFSH"));
                    }
                    else
                    {
                        TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.OutputPinSetHigh, "RFSH"));
                    }
                }
            }
        }
        private SignalState _rfsh;

        #endregion

        #region CPU Control

        /// <summary>
        /// HALT
        /// 
        /// HALT State (output, active Low). HALT indicates that the CPU has
        /// executed a HALT instruction and is waiting for either a non-maskable or a
        /// maskable interrupt (with the mask enabled) before operation can resume.
        /// During HALT, the CPU executes NOPs to maintain memory refresh.
        /// </summary>
        public virtual SignalState HALT
        {
            get { return _halt; }
            protected set
            {
                _halt = value;
                if (TraceMicroInstructions)
                {
                    if (_halt == SignalState.LOW)
                    {
                        TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.OutputPinSetLow, "HALT"));
                    }
                    else
                    {
                        TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.OutputPinSetHigh, "HALT"));
                    }
                }
            }
        }
        private SignalState _halt;

        /// <summary>
        /// WAIT
        /// 
        /// WAIT (input, active Low). WAIT communicates to the CPU that the
        /// addressed memory or I/O devices are not ready for a data transfer. The CPU
        /// continues to enter a WAIT state as long as this signal is active. Extended
        /// WAIT periods can prevent the CPU from properly refreshing dynamic
        /// memory.
        /// </summary>
        public abstract SignalState WAIT { get; }

        /// <summary>
        /// INT
        /// 
        /// Interrupt Request (input, active Low). Interrupt Request is generated by
        /// I/O devices. The CPU honors a request at the end of the current instruction if
        /// the internal software-controlled interrupt enable flip-flop (IFF) is enabled.
        /// INT is normally wired-OR and requires an external
        /// pull-up for these applications.
        /// </summary>
        public abstract SignalState INT { get; }

        /// <summary>
        /// NMI
        /// 
        /// Non-Maskable Interrupt (input, negative edge-triggered). NMI has a
        /// higher priority than INT. NMI is always recognized at the end of the current
        /// instruction, independent of the status of the interrupt enable flip-flop, and
        /// automatically forces the CPU to restart at location 0066H.
        /// </summary>
        public abstract SignalState NMI { get; }

        /// <summary>
        /// RESET
        /// 
        /// Reset (input, active Low). RESET initializes the CPU as follows: it resets
        /// the interrupt enable flip-flop, clears the PC and registers I and R, and sets the
        /// interrupt status to Mode 0. AF and SP are always set to FFFF after a reset, and
        /// all other registers are undefined (different depending on how long the CPU has
        /// been powered off, different for different Z80 chips).During reset time, the address 
        /// and data bus go to a high-impedance state, and all control output signals go to the 
        /// inactive state. Notice that RESET must be active for a minimum of three full clock
        /// cycles before the reset operation is complete.
        /// </summary>
        public abstract SignalState RESET { get; }

        #endregion

        #region CPU Bus Control

        /// <summary>
        /// BUSACK
        /// 
        /// Bus Acknowledge (output, active Low). Bus Acknowledge indicates to the
        /// requesting device that the CPU address bus, data bus, and control signals
        /// MREQ, IORQ RD, and WR have entered their high-impedance states. The
        /// external circuitry can now control these lines.
        /// </summary>
        public virtual SignalState BUSACK
        {
            get { return _busack; }
            protected set
            {
                _busack = value;
                if (TraceMicroInstructions)
                {
                    if (_busack == SignalState.LOW)
                    {
                        TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.OutputPinSetLow, "BUSACK"));
                    }
                    else
                    {
                        TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.OutputPinSetHigh, "BUSACK"));
                    }
                }
            }
        }
        private SignalState _busack;

        /// <summary>
        /// BUSREQ
        /// 
        /// Bus Request (input, active Low). Bus Request has a higher priority than
        /// NMI and is always recognized at the end of the current machine cycle.
        /// BUSREQ forces the CPU address bus, data bus, and control signals MREQ
        /// IORQ, RD, and WR to go to a high-impedance state so that other devices
        /// can control these lines. BUSREQ is normally wired-OR and requires an
        /// external pull-up for these applications. Extended BUSREQ periods due to
        /// extensive DMA operations can prevent the CPU from properly refreshing
        /// dynamic RAMS.
        /// </summary>
        public abstract SignalState BUSREQ { get; }
        
        #endregion
        
        #region Clock

        /// <summary>
        /// CLK
        /// 
        /// Clock (input). Single-phase MOS-level clock.
        /// 
        /// Notifications : CLK_OnEdge, CLK_OnEdge
        /// </summary>
        public abstract SignalState CLK { get; }

        #endregion
    }
}
