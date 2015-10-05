
namespace Z80Simulator.System
{    
    // For performance reasons, we assume that there will never be bus ownership conflicts in the test system, 
    // and that the bus value is always clearly defined.

    public class Bus<T>
    {
        private T currentValue;

        public T Value
        {
            get
            {
                return currentValue;
            }
        }

        internal void SetValue(T newValue/*, BusConnector<T> valueOrigin*/)
        {
            currentValue = newValue;
        }

        internal void ReleaseValue(/*BusConnector<T> valueOrigin*/)
        {            
            // optimization -> we do not simulate bus access conflicts, and assume that the value of the bus is always defined
            currentValue = default(T); 
        }
    }

    public class BusConnector<T>
    {
        protected Bus<T> connectedBus;

        public void ConnectTo(Bus<T> bus)
        {
            connectedBus = bus;
        }
        
        public void SetValue(T newValue)
        {
            //if (connectedBus != null) -> optimization : no BusConnector should be used before it is connected to a bus
            //{
                connectedBus.SetValue(newValue/*, this*/);
            //}
        }

        public void ReleaseValue()
        {
            //if (connectedBus != null) -> optimization : no BusConnector should be used before it is connected to a bus
            //{
                connectedBus.ReleaseValue(/*this*/);
            //}
        }

        public T SampleValue()
        {
            //if (connectedBus != null && !connectedBus.IsUndefined) -> optimization : no BusConnector should be used before it is connected to a bus
            //{
                return connectedBus.Value;
            //}
        }
    }

    /// <summary>
    /// Not derived from BusConnector for performance reasons
    /// </summary>
    public class PartialAddressBusConnector
    {
        protected Bus<ushort> connectedBus;
        private ushort bitMask;

        public PartialAddressBusConnector(int bitsCount)
        {
            bitMask = (ushort)(0xFFFF >> (16 - bitsCount));
        }

        public void ConnectTo(Bus<ushort> bus)
        {
            connectedBus = bus;
        }

        public void SetValue(ushort newValue)
        {
            connectedBus.SetValue((ushort)(newValue & bitMask));
        }

        public void ReleaseValue()
        {
            connectedBus.ReleaseValue();
        }

        public ushort SampleValue()
        {
            return (ushort)(connectedBus.Value & bitMask);
        }
    }
}