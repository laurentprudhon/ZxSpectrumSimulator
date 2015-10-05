using Z80Simulator.System;

namespace Z80Simulator.Test.System
{
    public abstract class TestMemory : Memory
    {
        /// <summary>
        /// Notifications : null, CPUClock_OnEdgeHigh
        /// </summary>
        public abstract SignalState CPUClock { get; }
        public virtual SignalState WAIT
        {
            get { return _wait; }
            protected set { _wait = value; }
        }
        private SignalState _wait;

        private int waitCycles;
        private int waitCountdown = -1;

        public TestMemory(int capacityBytes, int waitCycles)
            : base(capacityBytes)
        {
            this.waitCycles = waitCycles;

            _wait = SignalState.HIGH;
        }

        // Insert wait cycles each time a memory access is requested
        protected override void OnMemoryRequest()
        {
            if (waitCycles > 0)
            {
                waitCountdown = waitCycles;
            }
        }

        public void CPUClock_OnEdgeHigh()
        {
            if (waitCountdown >= 0)
            {
                if (waitCountdown == waitCycles)
                {
                    WAIT = SignalState.LOW;
                }
                else if (waitCountdown == 0)
                {
                    WAIT = SignalState.HIGH;
                }

                waitCountdown--;
            }
        }
    }
}
