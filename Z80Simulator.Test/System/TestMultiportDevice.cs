using System.Collections.Generic;
using Z80Simulator.System;

namespace Z80Simulator.Test.System
{
    public abstract class TestMultiportDevice
    {
        private IDictionary<byte, IList<byte>> readValuesForPortAddresses;
        private IDictionary<byte, IEnumerator<byte>> readIteratorsForPortAddresses;
        private IDictionary<byte, IList<byte>> writtenValuesForPortAddresses;

        public BusConnector<ushort> Address { get; private set; }
        public BusConnector<byte> Data { get; private set; }

        /// <summary>
        /// No notifications : null, null
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

        public TestMultiportDevice()
        {
            Address = new BusConnector<ushort>();
            Data = new BusConnector<byte>();
        }

        public void ReadEnable_OnEdgeLow()
        {
            if (IORequest == SignalState.LOW)
            {
                byte readAddress = (byte)(Address.SampleValue() & 0xFF);
                if (readIteratorsForPortAddresses.ContainsKey(readAddress))
                {
                    IEnumerator<byte> portIterator = readIteratorsForPortAddresses[readAddress];
                    if(!portIterator.MoveNext())
                    {
                        portIterator.Reset();
                        portIterator.MoveNext();
                    }
                    Data.SetValue(portIterator.Current);
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
                if (writtenValuesForPortAddresses.ContainsKey(writeAddress))
                {
                    byte data = Data.SampleValue();
                    writtenValuesForPortAddresses[writeAddress].Add(data);
                }
            }
        }
        
        // Debug interface

        public IDictionary<byte, IList<byte>> ReadValuesForPortAddresses 
        { 
            get { return readValuesForPortAddresses; }
            set
            {                  
                readValuesForPortAddresses = value;
                readIteratorsForPortAddresses = new Dictionary<byte, IEnumerator<byte>>();
                writtenValuesForPortAddresses = new Dictionary<byte, IList<byte>>();
                foreach(byte port in value.Keys)
                {
                    readIteratorsForPortAddresses[port] = value[port].GetEnumerator();
                    writtenValuesForPortAddresses[port] = new List<byte>();
                }
            }
        }

        public IDictionary<byte, IList<byte>> WrittenValuesForPortAddresses { get { return writtenValuesForPortAddresses; } }
    }
}