using Z80Simulator.System;

namespace Z80Simulator.Test.System
{
    public abstract class TestDevice : IODevice
    {
        /// <summary>
        /// Notifications : CPUClock_OnEdge, CPUClock_OnEdge
        /// </summary>
        public abstract SignalState CPUClock { get; }
        public virtual SignalState WAIT
        {
            get { return _wait; }
            private set { _wait = value; }
        }
        private SignalState _wait;

        private int waitCycles;
        private int waitCountdown = -1;

        private byte internalValue;

        public TestDevice(byte portAddress, byte initialValue, int waitCycles) : base(portAddress)
        {
            this.waitCycles = waitCycles;

            _wait = SignalState.HIGH;

            internalValue = initialValue;
        }

        // Specific implementation

        // Insert wait cycles each time an IO access is requested        
        protected override void OnIORequest()
        {
            if (waitCycles > 0)
            {
                waitCountdown = waitCycles;
            }
        }

        protected override byte ReadData()
        {            
            return internalValue;
        }

        protected override void WriteData(byte data)
        {
            internalValue = data;
        }

        public virtual void CPUClock_OnEdge()
        {
            if (waitCountdown >= 0)
            {
                if (CPUClock == SignalState.HIGH)
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

        // Debug interface
        
        public byte InternalValue { get { return internalValue; } set { internalValue = value; } }
    }
}