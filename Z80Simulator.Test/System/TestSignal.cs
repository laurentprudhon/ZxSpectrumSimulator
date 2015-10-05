using Z80Simulator.System;

namespace Z80Simulator.Test.System
{
    public abstract class TestSignal
    {
        /// <summary>
        /// Notifications : ExecuteHalfTState, ExecuteHalfTState
        /// </summary>
        public abstract SignalState CPUClock { get; }
        public virtual SignalState CPUControlPIN
        {
            get { return _cpuControlPin; }
            protected set { _cpuControlPin = value; }
        }
        protected SignalState _cpuControlPin;    

        private int[][] startAfterTStatesAndActivateDuringTStates;

        private int halfTStatesCounter = 0;

        public TestSignal(int[][] startAfterTStatesAndActivateDuringTStates)
        {
            this.startAfterTStatesAndActivateDuringTStates = startAfterTStatesAndActivateDuringTStates;

            _cpuControlPin = SignalState.HIGH;
        }

        public void ExecuteHalfTState()
        {
            if (startAfterTStatesAndActivateDuringTStates != null)
            {
                foreach (int[] startAndDurationCouple in startAfterTStatesAndActivateDuringTStates)
                {
                    int startAfterTStates = startAndDurationCouple[0];
                    int activateDuringTStates = startAndDurationCouple[1];
                    if (halfTStatesCounter == 2 * startAfterTStates)
                    {
                        CPUControlPIN = SignalState.LOW;
                    }
                    if (halfTStatesCounter == 2 * (startAfterTStates + activateDuringTStates))
                    {
                        CPUControlPIN = SignalState.HIGH;
                    }
                }

                halfTStatesCounter++;
            }
        }
    }
}