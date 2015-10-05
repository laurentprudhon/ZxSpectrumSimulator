using Z80Simulator.CPU;
using Z80Simulator.System;

namespace Z80Simulator.Test.System
{
    public class TestSystemWithTestSignals : TestSystem
    {
        TestSignal BUSREQSignal;
        TestSignal INTSignal;
        TestSignal NMISignal;
        TestSignal RESETSignal;
        TestSignal WAITSignal;

        public TestSystemWithTestSignals(
            int[][] BUSREQstartAfterTStatesAndActivateDuringTStates,
            int[][] INTstartAfterTStatesAndActivateDuringTStates,
            int[][] NMIstartAfterTStatesAndActivateDuringTStates,
            int[][] RESETstartAfterTStatesAndActivateDuringTStates,
            int[][] WAITstartAfterTStatesAndActivateDuringTStates
            )
        {
            // Initialize components
            clock = new __Clock(3500000L);
            cpu = new __Z80CPU();
            memory = new _TestMemory(65536, 0);
            BUSREQSignal = new _TestSignal(BUSREQstartAfterTStatesAndActivateDuringTStates);
            INTSignal = new _TestSignal(INTstartAfterTStatesAndActivateDuringTStates);
            NMISignal = new _TestSignal(NMIstartAfterTStatesAndActivateDuringTStates);
            RESETSignal = new _TestSignal(RESETstartAfterTStatesAndActivateDuringTStates);
            WAITSignal = new _TestSignal(WAITstartAfterTStatesAndActivateDuringTStates);

            // Connect buses
            cpu.Address.ConnectTo(addressBus);
            cpu.Data.ConnectTo(dataBus);
            memory.Address.ConnectTo(addressBus);
            memory.Data.ConnectTo(dataBus);

            // Connect individual PINs
            ((__Clock)clock).ConnectTo(cpu, (TestMemory)memory, BUSREQSignal, INTSignal, NMISignal, RESETSignal, WAITSignal);
            ((__Z80CPU)cpu).ConnectTo(clock, (TestMemory)memory, BUSREQSignal, INTSignal, NMISignal, RESETSignal, WAITSignal);
            ((_TestMemory)memory).ConnectTo(clock, cpu);
        }

        protected class __Clock : _Clock
        {
            public __Clock(long frequencyHerz) : base(frequencyHerz)
            { }

            private TestSignal busreqSignal;
            private TestSignal intSignal;
            private TestSignal nmiSignal;
            private TestSignal resetSignal;
            private TestSignal waitSignal;
            public void ConnectTo(Z80CPU cpu, TestMemory memory, TestSignal busreqSignal, TestSignal intSignal, TestSignal nmiSignal, TestSignal resetSignal, TestSignal waitSignal)
            {
                base.ConnectTo(cpu, memory);
                this.busreqSignal = busreqSignal;
                this.intSignal = intSignal;
                this.nmiSignal = nmiSignal;
                this.resetSignal = resetSignal;
                this.waitSignal = waitSignal;
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
                    busreqSignal.ExecuteHalfTState();
                    intSignal.ExecuteHalfTState();
                    nmiSignal.ExecuteHalfTState();
                    resetSignal.ExecuteHalfTState();
                    waitSignal.ExecuteHalfTState();
                }
            }
        }

        protected class __Z80CPU : _Z80CPU
        {
            private TestSignal busreqSignal;
            private TestSignal intSignal;
            private TestSignal nmiSignal;
            private TestSignal resetSignal;
            private TestSignal waitSignal;
            public void ConnectTo(Clock clock, TestMemory memory, TestSignal busreqSignal, TestSignal intSignal, TestSignal nmiSignal, TestSignal resetSignal, TestSignal waitSignal)
            {
                base.ConnectTo(clock, memory);
                this.busreqSignal = busreqSignal;
                this.intSignal = intSignal;
                this.nmiSignal = nmiSignal;
                this.resetSignal = resetSignal;
                this.waitSignal = waitSignal;
            }

            // Input PINS source
            
            public override SignalState BUSREQ
            {
                get
                {
                    return busreqSignal.CPUControlPIN;
                }
            }

            public override SignalState INT
            {
                get
                {
                    return intSignal.CPUControlPIN;
                }
            }

            public override SignalState NMI
            {
                get
                {
                    return nmiSignal.CPUControlPIN;
                }
            }

            public override SignalState RESET
            {
                get
                {
                    return resetSignal.CPUControlPIN;
                }
            }

            public override SignalState WAIT
            {
                get
                {
                    return waitSignal.CPUControlPIN;
                }
            }            
        }

        protected class _TestSignal : TestSignal
        {
            public _TestSignal(int[][] startInterruptAfterTStatesAndDuringTStates) : base(startInterruptAfterTStatesAndDuringTStates)
            { }

            protected Clock clock;
            internal void ConnectTo(Clock clock)
            {
                this.clock = clock;
            }

            // Input PINs sources

            public override SignalState CPUClock
            {
                get { return clock.CLK; }
            }
        }
    }
}
