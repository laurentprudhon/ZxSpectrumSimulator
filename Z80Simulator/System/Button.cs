using System;

namespace Z80Simulator.System
{
    public class Button
    {
        public virtual SignalState Output
        {
            get { return _output; }
            private set { _output = value; }
        }
        private SignalState _output;

        private SignalState releasedState;

        public Button(SignalState releasedState)
        {
            this.releasedState = releasedState;
            _output = releasedState;
        }

        public void Push()
        {
            if (releasedState == SignalState.HIGH)
            {
                Output = SignalState.LOW;
            }
            else
            {
                Output = SignalState.HIGH;
            }
        }

        public void Release()
        {
            Output = releasedState;
        }
    }
}
