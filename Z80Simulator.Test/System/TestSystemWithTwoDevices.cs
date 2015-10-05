using Z80Simulator.CPU;
using Z80Simulator.System;

namespace Z80Simulator.Test.System
{
    public class TestSystemWithTwoDevices : TestSystem
    {
        TestDevice device1;
        TestDevice device2;
        
        public TestSystemWithTwoDevices(bool simulateSlowMemoryAndDevices) : base (simulateSlowMemoryAndDevices)
        {
            // Initialize components
            clock = new __Clock(3500000L);
            cpu = new __Z80CPU();
            if (!simulateSlowMemoryAndDevices)
            {
                memory = new _TestMemory(65536, 0);
                device1 = new _TestDevice(1, 10, 0);
                device2 = new _TestDevice(2, 20, 0);
            }
            else
            {
                memory = new _TestMemory(65536, 2);
                device1 = new _TestDevice(1, 10, 2);
                device2 = new _TestDevice(2, 20, 2);      
            }

            // Connect buses
            cpu.Address.ConnectTo(addressBus);
            cpu.Data.ConnectTo(dataBus);
            memory.Address.ConnectTo(addressBus);
            memory.Data.ConnectTo(dataBus);
            device1.Address.ConnectTo(addressBus);
            device1.Data.ConnectTo(dataBus);
            device2.Address.ConnectTo(addressBus);
            device2.Data.ConnectTo(dataBus);

            // Connect individual PINs
            ((__Clock)clock).ConnectTo(cpu, (TestMemory)memory, device1, device2);
            ((__Z80CPU)cpu).ConnectTo(clock, (TestMemory)memory, device1, device2);
            ((_TestMemory)memory).ConnectTo(clock, cpu);
            ((_TestDevice)device1).ConnectTo(clock, cpu);
            ((_TestDevice)device2).ConnectTo(clock, cpu);
        }

        protected class __Clock : _Clock
        {
            public __Clock(long frequencyHerz) : base(frequencyHerz)
            { }

            protected TestDevice device1;
            protected TestDevice device2;
            public void ConnectTo(Z80CPU cpu, TestMemory memory, TestDevice device1, TestDevice device2)
            {
                base.ConnectTo(cpu, memory);
                this.device1 = device1;
                this.device2 = device2;
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
                    device1.CPUClock_OnEdge();
                    device2.CPUClock_OnEdge();
                    cpu.CLK_OnEdge();
                }
            }
        }

        protected class __Z80CPU : _Z80CPU
        {
            protected TestDevice device1;
            protected TestDevice device2;
            public void ConnectTo(Clock clock, TestMemory memory, TestDevice device1, TestDevice device2)
            {
                base.ConnectTo(clock, memory);
                this.device1 = device1;
                this.device2 = device2;
            }

            // Input PINs sources
                        
            public override SignalState WAIT
            {
                get 
                {
                    if (memory.WAIT == SignalState.LOW || device1.WAIT == SignalState.LOW || device2.WAIT == SignalState.LOW)
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
                        device1.IORequest_OnEdgeLow();
                        device2.IORequest_OnEdgeLow();
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
                        device1.ReadEnable_OnEdgeLow();
                        device2.ReadEnable_OnEdgeLow();
                        memory.ReadEnable_OnEdgeLow();
                    }
                    else
                    {
                        device1.ReadEnable_OnEdgeHigh();
                        device2.ReadEnable_OnEdgeHigh();
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
                        device1.WriteEnable_OnEdgeLow();
                        device2.WriteEnable_OnEdgeLow();
                        memory.WriteEnable_OnEdgeLow();
                    }
                }
            }
        }

        protected class _TestDevice : TestDevice
        {
            public _TestDevice(byte portAddress, byte initialValue, int waitCycles) : base(portAddress, initialValue, waitCycles)
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
