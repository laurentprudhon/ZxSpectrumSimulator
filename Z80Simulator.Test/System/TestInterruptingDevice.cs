using Z80Simulator.System;

namespace Z80Simulator.Test.System
{
    public abstract class TestInterruptingDevice : TestDevice
    {
        /// <summary>
        /// No notifications : null, null
        /// </summary>
        public abstract SignalState M1 { get; }
        public virtual SignalState INT
        {
            get { return _int; }
            protected set { _int = value; }
        }
        protected SignalState _int;

        private int[][] startInterruptAfterTStatesAndDuringTStates;
        private int halfTStatesCounter = 0;

        private byte valueForDataBusAfterInterrupt;

        public TestInterruptingDevice(byte portAddress, byte initialValue, int waitCycles, int[][] startInterruptAfterTStatesAndDuringTStates, byte valueForDataBusAfterInterrupt)
            : base(portAddress, initialValue, waitCycles)
        {
            _int = SignalState.HIGH;

            this.startInterruptAfterTStatesAndDuringTStates = startInterruptAfterTStatesAndDuringTStates;
            this.valueForDataBusAfterInterrupt = valueForDataBusAfterInterrupt;
        }
        
        // Specific implementation

        // Insert wait cycles each time an IO access is requested        
        protected override void OnIORequest()
        {
            base.OnIORequest();

            if (M1 == SignalState.LOW)
            {
                Data.SetValue(valueForDataBusAfterInterrupt);
            }
        }

        public override void CPUClock_OnEdge()
        {
            base.CPUClock_OnEdge();

            foreach (int[] startAndDurationCouple in startInterruptAfterTStatesAndDuringTStates)
            {
                int startAfterTStates = startAndDurationCouple[0];
                int activateDuringTStates = startAndDurationCouple[1];
                if (halfTStatesCounter == 2 * startAfterTStates)
                {
                    INT = SignalState.LOW;
                }
                if (halfTStatesCounter == 2 * (startAfterTStates + activateDuringTStates))
                {
                    INT = SignalState.HIGH;
                }
            }

            halfTStatesCounter++;
        }
    }
}