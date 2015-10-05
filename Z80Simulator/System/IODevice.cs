
namespace Z80Simulator.System
{
    public abstract class IODevice
    {
        private byte portAddress;

        public BusConnector<ushort> Address { get; private set; }
        public BusConnector<byte> Data { get; private set; }

        /// <summary>
        /// Notifications : IORequest_OnEdgeLow, null
        /// </summary>
        public abstract SignalState IORequest { get; }
        /// <summary>
        /// Notifications : ReadEnable_OnEdgeLow, ReadEnable_OnEdgeHigh
        /// </summary>
        public abstract SignalState ReadEnable { get; }
        /// <summary>
        /// Notifications : WriteEnable_OnEdgeLow, null
        /// </summary>
        public abstract SignalState WriteEnable { get; }

        public IODevice(byte portAddress)
        {
            this.portAddress = portAddress;

            Address = new BusConnector<ushort>();
            Data = new BusConnector<byte>();
        }

        public void IORequest_OnEdgeLow()
        {
            OnIORequest();
        }

        public void ReadEnable_OnEdgeLow()
        {
            if (IORequest == SignalState.LOW)
            {
                byte readAddress = (byte)(Address.SampleValue() & 0xFF);
                if (readAddress == portAddress)
                {
                    Data.SetValue(ReadData());
                }
            }
        }

        public void ReadEnable_OnEdgeHigh()
        {
            if (IORequest == SignalState.LOW)
            {
                Data.ReleaseValue();
            }
        }

        public void WriteEnable_OnEdgeLow()
        {
            if (IORequest == SignalState.LOW)
            {                
                byte writeAddress = (byte)(Address.SampleValue() & 0xFF);
                if (writeAddress == portAddress)
                {
                    byte data = Data.SampleValue();
                    WriteData(data);
                }
            }
        }

        // Override for each specific implementation

        protected abstract void OnIORequest();
        protected abstract byte ReadData();
        protected abstract void WriteData(byte data);

        // Debug interface

        public byte PortAddress { get { return portAddress; } }


    }
}