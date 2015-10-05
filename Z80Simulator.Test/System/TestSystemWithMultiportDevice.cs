using System.Collections.Generic;
using Z80Simulator.CPU;
using Z80Simulator.System;

namespace Z80Simulator.Test.System
{
    public class TestSystemWithMultiportDevice : TestSystem
    {
        TestMultiportDevice multiportDevice;

        public TestSystemWithMultiportDevice(IDictionary<byte, IList<byte>> valuesForPortAddresses)
        {
            // Initialize components
            clock = new _Clock(3500000L);
            cpu = new __Z80CPU();
            memory = new _TestMemory(65536, 0);
            multiportDevice = new _TestMultiportDevice();
            multiportDevice.ReadValuesForPortAddresses = valuesForPortAddresses;

            // Connect buses
            cpu.Address.ConnectTo(addressBus);
            cpu.Data.ConnectTo(dataBus);
            memory.Address.ConnectTo(addressBus);
            memory.Data.ConnectTo(dataBus);
            multiportDevice.Address.ConnectTo(addressBus);
            multiportDevice.Data.ConnectTo(dataBus);

            // Connect individual PINs
            ((_Clock)clock).ConnectTo(cpu, (TestMemory)memory);
            ((__Z80CPU)cpu).ConnectTo(clock, (TestMemory)memory, multiportDevice);
            ((_TestMemory)memory).ConnectTo(clock, cpu);
            ((_TestMultiportDevice)multiportDevice).ConnectTo(cpu);
        }
        
        protected class __Z80CPU : _Z80CPU
        {
            protected TestMultiportDevice multiportDevice;
            public void ConnectTo(Clock clock, TestMemory memory, TestMultiportDevice multiportDevice)
            {
                base.ConnectTo(clock, memory);
                this.multiportDevice = multiportDevice;
            }

            // Output PINs with notifications
            
            public override SignalState RD
            {
                protected set
                {
                    _rd = value;
                    if (value == SignalState.LOW)
                    {
                        multiportDevice.ReadEnable_OnEdgeLow();
                        memory.ReadEnable_OnEdgeLow();
                    }
                    else
                    {
                        multiportDevice.ReadEnable_OnEdgeHigh();
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
                        multiportDevice.WriteEnable_OnEdgeLow();
                        memory.WriteEnable_OnEdgeLow();
                    }
                }
            }
        }

        protected class _TestMultiportDevice : TestMultiportDevice
        {
            protected Z80CPU cpu;
            internal void ConnectTo(Z80CPU cpu)
            {
                this.cpu = cpu;
            }

            // Input PINs sources

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

        public TestMultiportDevice MultiportDevice { get { return multiportDevice; } }
    }
}
