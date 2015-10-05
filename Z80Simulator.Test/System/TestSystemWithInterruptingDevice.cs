using Z80Simulator.CPU;
using Z80Simulator.System;

namespace Z80Simulator.Test.System
{
    public class TestSystemWithInterruptingDevice : TestSystem
    {
        TestInterruptingDevice device;

        public TestSystemWithInterruptingDevice(byte portAddress, byte initialValue, int waitCycles, int[][] startInterruptAfterTStatesAndDuringTStates, byte valueForDataBusAfterInterrupt)
        {
            // Initialize components
            clock = new __Clock(3500000L);
            cpu = new __Z80CPU();
            memory = new _TestMemory(65536, 0);
            device = new _TestInterruptingDevice(portAddress, initialValue, waitCycles, startInterruptAfterTStatesAndDuringTStates, valueForDataBusAfterInterrupt);

            // Connect buses
            cpu.Address.ConnectTo(addressBus);
            cpu.Data.ConnectTo(dataBus);
            memory.Address.ConnectTo(addressBus);
            memory.Data.ConnectTo(dataBus);
            device.Address.ConnectTo(addressBus);
            device.Data.ConnectTo(dataBus);

            // Connect individual PINs
            ((__Clock)clock).ConnectTo(cpu, (TestMemory)memory, device);
            ((__Z80CPU)cpu).ConnectTo(clock, (TestMemory)memory, device);
            ((_TestMemory)memory).ConnectTo(clock, cpu);
            ((_TestInterruptingDevice)device).ConnectTo(clock, cpu);
        }

        protected class __Clock : _Clock
        {
            public __Clock(long frequencyHerz) : base(frequencyHerz)
            { }

            protected TestInterruptingDevice device;
            public void ConnectTo(Z80CPU cpu, TestMemory memory, TestInterruptingDevice device)
            {
                base.ConnectTo(cpu, memory);
                this.device = device;
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
                    device.CPUClock_OnEdge();
                }
            }
        }

        protected class __Z80CPU : _Z80CPU
        {
            protected TestInterruptingDevice device;
            public void ConnectTo(Clock clock, TestMemory memory, TestInterruptingDevice device)
            {
                base.ConnectTo(clock, memory);
                this.device = device;
            }

            // Input PINS source

            public override SignalState INT
            {
                get
                {
                    return device.INT;
                }
            }
            
            public override SignalState WAIT
            {
                get
                {
                    if (memory.WAIT == SignalState.LOW || device.WAIT == SignalState.LOW)
                    {
                        return SignalState.LOW;
                    }
                    else
                    {
                        return SignalState.HIGH;
                    }
                }
            }

            // Output PINs with notifications

            public override SignalState IORQ
            {
	            protected set 
	            { 
		            base.IORQ = value;
                    if (value == SignalState.LOW)
                    {
                        device.IORequest_OnEdgeLow();
                    }
	            }
            }

            public override SignalState RD
            {
                protected set
                {
                    _rd = value;
                    if (value == SignalState.LOW)
                    {
                        device.ReadEnable_OnEdgeLow();
                        memory.ReadEnable_OnEdgeLow();
                    }
                    else
                    {
                        device.ReadEnable_OnEdgeHigh();
                        memory.ReadEnable_OnEdgeHigh();
                    }
                }
            }

            public override SignalState WR
            {
                protected set
                {
                    _wr = value;
                    if (value == SignalState.LOW)
                    {
                        device.WriteEnable_OnEdgeLow();
                        memory.WriteEnable_OnEdgeLow();
                    }
                }
            }
        }

        protected class _TestInterruptingDevice : TestInterruptingDevice
        {
            public _TestInterruptingDevice(byte portAddress, byte initialValue, int waitCycles, int[][] startInterruptAfterTStatesAndDuringTStates, byte valueForDataBusAfterInterrupt) : 
                base(portAddress, initialValue, waitCycles, startInterruptAfterTStatesAndDuringTStates, valueForDataBusAfterInterrupt)
            { }

            protected Clock clock;
            protected Z80CPU cpu;
            internal void ConnectTo(Clock clock, Z80CPU cpu)
            {
                this.clock = clock;
                this.cpu = cpu;
            }

            // Input PINs sources

            public override SignalState CPUClock
            {
                get { return clock.CLK; }
            }

            public override SignalState M1
            {
                get { return cpu.M1; }
            }

            public override SignalState IORequest
            {
                get { return cpu.IORQ; }
            }

            public override SignalState ReadEnable
            {
                get { return cpu.RD; }
            }

            public override SignalState WriteEnable
            {
                get { return cpu.WR; }
            }
        }
    }
}
