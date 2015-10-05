using System;

namespace Z80Simulator.System
{
    public enum SignalState { LOW, HIGH }

    public class AnalogOutputPin<T>
    {
        public AnalogOutputPin(T powerOnLevel)
        {
            Level = powerOnLevel;
        }

        public void ConnectTo(AnalogInputPin<T> inputPin)
        {
            inputPin.ConnectFrom(this);
        }

        public T Level;
    }

    public class AnalogInputPin<T>
    {
        private AnalogOutputPin<T> sourceSignal;

        internal void ConnectFrom(AnalogOutputPin<T> sourceSignal)
        {
            this.sourceSignal = sourceSignal;
        }

        public T Level
        {
            get
            {
                return sourceSignal.Level;
            }
        }
    }

/* Performance optimization
 
    public class OutputPin
    {
        protected SignalState currentState;
        
        private Action edgeHighSignalListener1;
        private Action edgeHighSignalListener2;
        private Action edgeHighSignalListener3;
        private Action edgeHighSignalListener4;

        protected Action notifyEdgeHighSignalListeners;

        private void Notify0EdgeHighSignalListeners() { }
        private void Notify1EdgeHighSignalListeners() { edgeHighSignalListener1(); }
        private void Notify2EdgeHighSignalListeners() { edgeHighSignalListener1(); edgeHighSignalListener2(); }
        private void Notify3EdgeHighSignalListeners() { edgeHighSignalListener1(); edgeHighSignalListener2(); edgeHighSignalListener3(); }
        private void Notify4EdgeHighSignalListeners() { edgeHighSignalListener1(); edgeHighSignalListener2(); edgeHighSignalListener3(); edgeHighSignalListener4(); }

        private Action edgeLowSignalListener1;
        private Action edgeLowSignalListener2;
        private Action edgeLowSignalListener3;
        private Action edgeLowSignalListener4;

        protected Action notifyEdgeLowSignalListeners;

        private void Notify0EdgeLowSignalListeners() { }
        private void Notify1EdgeLowSignalListeners() { edgeLowSignalListener1(); }
        private void Notify2EdgeLowSignalListeners() { edgeLowSignalListener1(); edgeLowSignalListener2(); }
        private void Notify3EdgeLowSignalListeners() { edgeLowSignalListener1(); edgeLowSignalListener2(); edgeLowSignalListener3(); }
        private void Notify4EdgeLowSignalListeners() { edgeLowSignalListener1(); edgeLowSignalListener2(); edgeLowSignalListener3(); edgeLowSignalListener4(); }
        
        public OutputPin(SignalState powerOnState)
        {
            currentState = powerOnState;
            notifyEdgeHighSignalListeners = Notify0EdgeHighSignalListeners;
            notifyEdgeLowSignalListeners = Notify0EdgeLowSignalListeners;
        }
        
        public void ConnectTo(InputPin inputPin)
        {
            inputPin.ConnectFrom(this);
            if (inputPin.OnEdgeHigh != null)
            {
                if (edgeHighSignalListener1 == null)
                {
                    edgeHighSignalListener1 = inputPin.OnEdgeHigh;
                    notifyEdgeHighSignalListeners = Notify1EdgeHighSignalListeners;
                }
                else if (edgeHighSignalListener2 == null)
                {
                    edgeHighSignalListener2 = inputPin.OnEdgeHigh;
                    notifyEdgeHighSignalListeners = Notify2EdgeHighSignalListeners;
                }
                else if (edgeHighSignalListener3 == null)
                {
                    edgeHighSignalListener3 = inputPin.OnEdgeHigh;
                    notifyEdgeHighSignalListeners = Notify3EdgeHighSignalListeners;
                }
                else if (edgeHighSignalListener4 == null)
                {
                    edgeHighSignalListener4 = inputPin.OnEdgeHigh;
                    notifyEdgeHighSignalListeners = Notify4EdgeHighSignalListeners;
                }
                else
                {
                    throw new NotSupportedException("Can not connect more than four InputPins with pin logic to the same OutputPin");
                }
            }
            if (inputPin.OnEdgeLow != null)
            {
                if (edgeLowSignalListener1 == null)
                {
                    edgeLowSignalListener1 = inputPin.OnEdgeLow;
                    notifyEdgeLowSignalListeners = Notify1EdgeLowSignalListeners;
                }
                else if (edgeLowSignalListener2 == null)
                {
                    edgeLowSignalListener2 = inputPin.OnEdgeLow;
                    notifyEdgeLowSignalListeners = Notify2EdgeLowSignalListeners;
                }
                else if (edgeLowSignalListener3 == null)
                {
                    edgeLowSignalListener3 = inputPin.OnEdgeLow;
                    notifyEdgeLowSignalListeners = Notify3EdgeLowSignalListeners;
                }
                else if (edgeLowSignalListener4 == null)
                {
                    edgeLowSignalListener4 = inputPin.OnEdgeLow;
                    notifyEdgeLowSignalListeners = Notify4EdgeLowSignalListeners;
                }
                else
                {
                    throw new NotSupportedException("Can not connect more than four InputPins with pin logic to the same OutputPin");
                }
            }
        }
        
        public SignalState State
        {
            get
            {
                return currentState;
            }
            set
            {                
                currentState = value;
                if (currentState == SignalState.HIGH)
                {
                    notifyEdgeHighSignalListeners();
                }
                else
                {
                    notifyEdgeLowSignalListeners();
                }
            }
        }

        public void InvertState()
        {
            if (currentState == SignalState.LOW)
            {
                currentState = SignalState.HIGH;
                notifyEdgeHighSignalListeners();
            }
            else
            {
                currentState = SignalState.LOW;
                notifyEdgeLowSignalListeners();
            }
        }
    }

    public delegate void InputPinLogic(SignalState newState);

    public class InputPin
    {
        private OutputPin sourceSignal1;
        private OutputPin sourceSignal2;
        private OutputPin sourceSignal3;
        private OutputPin sourceSignal4;

        private Func<SignalState> sampleSourceSignals;

        private SignalState Sample0SourceSignals() { throw new NotSupportedException("Disconnected InputPin is in an undefined state"); }
        private SignalState Sample1SourceSignals() { return sourceSignal1.State; }
        private SignalState Sample2SourceSignals() { if (sourceSignal1.State == SignalState.LOW) { return sourceSignal1.State; } else { return sourceSignal2.State; } }
        private SignalState Sample3SourceSignals() { if (sourceSignal1.State == SignalState.LOW) { return sourceSignal1.State; } else { if (sourceSignal2.State == SignalState.LOW) { return sourceSignal2.State; } else { return sourceSignal3.State; } } }
        private SignalState Sample4SourceSignals() { if (sourceSignal1.State == SignalState.LOW) { return sourceSignal1.State; } else { if (sourceSignal2.State == SignalState.LOW) { return sourceSignal2.State; } else { if (sourceSignal3.State == SignalState.LOW) { return sourceSignal3.State; } else { return sourceSignal4.State; } } } }

        internal Action OnEdgeHigh { get; private set; }
        internal Action OnEdgeLow { get; private set; }

        public InputPin(Action onEdgeLow, Action onEdgeHigh)
        {
            OnEdgeHigh = onEdgeHigh;
            OnEdgeLow = onEdgeLow;
            sampleSourceSignals = Sample0SourceSignals;
        }

        internal void ConnectFrom(OutputPin sourceSignal)
        {
            if (sourceSignal1 == null)
            {
                sourceSignal1 = sourceSignal;
                sampleSourceSignals = Sample1SourceSignals;
            }
            else if (sourceSignal2 == null)
            {
                sourceSignal2 = sourceSignal;
                sampleSourceSignals = Sample2SourceSignals;
            }
            else if (sourceSignal3 == null)
            {
                sourceSignal3 = sourceSignal;
                sampleSourceSignals = Sample3SourceSignals;
            }
            else if (sourceSignal4 == null)
            {
                sourceSignal4 = sourceSignal;
                sampleSourceSignals = Sample4SourceSignals;
            }
            else
            {
                throw new NotSupportedException("Can not connect more than four OutputPins to the same InputPin");
            }
        }
        
        public SignalState State
        {
            get
            {
                if (sourceSignal2 == null)
                {
                    return sourceSignal1.State;
                }
                else
                {
                    return sampleSourceSignals();
                }
            }
        }
    }
*/    
}
