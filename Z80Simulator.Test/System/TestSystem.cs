using System.IO;
using System.Text;
using Z80Simulator.Assembly;
using Z80Simulator.CPU;
using Z80Simulator.System;

namespace Z80Simulator.Test.System
{
    public class TestSystem : Z80SystemBase
    {
        public TestSystem() : this(false)
        { }

        public TestSystem(bool simulateSlowMemoryAndDevices)
        {
            // Initialize components
            clock = new _Clock(3500000L);
            cpu = new _Z80CPU();
            if (!simulateSlowMemoryAndDevices)
            {
                memory = new _TestMemory(65536, 0);
            }
            else
            {
                memory = new _TestMemory(65536, 2);
            }

            // Connect buses
            cpu.Address.ConnectTo(addressBus);
            cpu.Data.ConnectTo(dataBus);
            memory.Address.ConnectTo(addressBus);
            memory.Data.ConnectTo(dataBus);

            // Connect individual PINs
            ((_Clock)clock).ConnectTo(cpu, (TestMemory)memory);
            ((_Z80CPU)cpu).ConnectTo(clock, (TestMemory)memory);
            ((_TestMemory)memory).ConnectTo(clock, cpu);
        }

        protected class _Clock : Clock
        {
            public _Clock(long frequencyHerz) : base(frequencyHerz)
            { }

            protected Z80CPU cpu;
            protected TestMemory memory;
            public void ConnectTo(Z80CPU cpu, TestMemory memory)
            {
                this.cpu = cpu;
                this.memory = memory;
            }

            // Output PINs with notifications

            public override SignalState CLK
            {
                protected set
                {
                    _clk = value;
                    if (value == SignalState.HIGH)
                    {
                        memory.CPUClock_OnEdgeHigh();
                    }
                    cpu.CLK_OnEdge();
                }
            }
        }

        protected class _Z80CPU : Z80CPU
        {
            protected Clock clock;
            protected TestMemory memory;
            public void ConnectTo(Clock clock, TestMemory memory)
            {
                this.clock = clock;
                this.memory = memory;
            }

            // Input PINs sources

            public override SignalState BUSREQ
            {
                get { return SignalState.HIGH; }
            }

            public override SignalState CLK
            {
                get { return clock.CLK; }
            }

            public override SignalState INT
            {
                get { return SignalState.HIGH; }
            }

            public override SignalState NMI
            {
                get { return SignalState.HIGH; }
            }

            public override SignalState RESET
            {
                get { return SignalState.HIGH; }
            }

            public override SignalState WAIT
            {
                get { return memory.WAIT; }
            }

            // Output PINs with notifications
       
            public override SignalState MREQ
            {
                protected set
                {
                    base.MREQ = value;
                    if (value == SignalState.LOW)
                    {
                        memory.ChipSelect_OnEdgeLow();
                    }
                }
            }

            public override SignalState RD
            {
                protected set
                {
                    base.RD = value;
                    if (value == SignalState.LOW)
                    {
                        memory.ReadEnable_OnEdgeLow();
                    }
                    else
                    {
                        memory.ReadEnable_OnEdgeHigh();
                    }
                }
            }

            public override SignalState WR
            {
                protected set
                {
                    base.WR = value;
                    if (value == SignalState.LOW)
                    {
                        memory.WriteEnable_OnEdgeLow();
                    }
                }
            }
        }

        public class _TestMemory : TestMemory
        {
            public _TestMemory(int capacityBytes, int waitCycles) : base(capacityBytes, waitCycles)
            { }

            protected Clock clock;
            protected Z80CPU cpu;
            internal void ConnectTo(Clock clock, Z80CPU cpu)
            {
                this.clock = clock;
                this.cpu = cpu;
            }
            
            protected bool readOnlyMemoryAboveAddress = false;
            protected ushort startOfReadOnlyMemory;
            protected ushort endOfReadOnlyMemory;
            public void SetReadOnlySection(ushort startOfReadOnlyMemory, ushort endOfReadOnlyMemory)
            {
                readOnlyMemoryAboveAddress = true;
                this.startOfReadOnlyMemory = startOfReadOnlyMemory;
                this.endOfReadOnlyMemory = endOfReadOnlyMemory;
            }

            protected override void WriteData(ushort writeAddress, byte data)
            {
                if(!readOnlyMemoryAboveAddress || writeAddress > endOfReadOnlyMemory || writeAddress < startOfReadOnlyMemory)
                {
                    base.WriteData(writeAddress, data);
                }
            }

            // Input PINs sources

            public override SignalState CPUClock
            {
	            get { return clock.CLK; }
            }

            public override SignalState ChipSelect
            {
                get { return cpu.MREQ; }
            }

            public override SignalState ReadEnable
            {
                get { return cpu.RD; }
            }

            public override SignalState Refresh
            {
                get { return cpu.RFSH; }
            }

            public override SignalState WriteEnable
            {
                get { return cpu.WR; }
            }
        }

        public Program LoadProgramInMemory(string filename, Encoding encoding, bool ignoreCase)
        {
            Stream stream = PlatformSpecific.GetStreamForProjectFile(filename);
            return LoadProgramInMemory(filename, stream, encoding, ignoreCase);
        }
    }   
}
