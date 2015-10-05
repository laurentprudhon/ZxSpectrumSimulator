using System;

namespace Z80Simulator.System
{
    public abstract class Memory
    {
        public const ushort BYTES_16K = 16384;
        public const ushort BYTES_32K = 32768;

        protected byte[] cells;

        public BusConnector<ushort> Address { get; private set; }
        public BusConnector<byte> Data { get; private set; }

        /// <summary>
        /// Notifications : ChipSelect_OnEdgeLow, null
        /// </summary>
        public abstract SignalState ChipSelect { get; }
        /// <summary>
        /// Notifications : ReadSignal_OnEdgeLow, ReadSignal_OnEdgeHigh
        /// </summary>
        public abstract SignalState ReadEnable { get; }
        /// <summary>
        /// Notifications : WriteSignal_OnEdgeLow, null
        /// </summary>
        public abstract SignalState WriteEnable { get; }
        /// <summary>
        /// Notifications : null, null
        /// </summary>
        public abstract SignalState Refresh { get; }

        public Memory(int capacityBytes)
        {
            cells = new byte[capacityBytes];

            Address = new BusConnector<ushort>();
            Data = new  BusConnector<byte>();
        }
                
        public void ChipSelect_OnEdgeLow()
        {
            //if (Refresh.State == SignalState.LOW)    // here we could simulate DRAM refresh (the Z80 CPU activates chip select after refresh during the second part of M1 cycle)
            //{
            //    RefreshSignalChanged(Refresh.State); 
            //}
            if (Refresh == SignalState.HIGH)
            {
                OnMemoryRequest();
            }
        }

        public void ReadEnable_OnEdgeLow()
        {
            if (ChipSelect == SignalState.LOW)
            {
                ushort readAddress = Address.SampleValue();
                byte data = ReadData(readAddress);
                Data.SetValue(data);
            }
        }

        public void ReadEnable_OnEdgeHigh()
        {
            if (ChipSelect == SignalState.LOW)
            {
                Data.ReleaseValue();
            }
        }

        public void WriteEnable_OnEdgeLow()
        {
            if (ChipSelect == SignalState.LOW)
            {
                ushort writeAddress = Address.SampleValue();
                byte data = Data.SampleValue();
                WriteData(writeAddress, data);
            }
        }

        // Override for specific implementation

        protected virtual void OnMemoryRequest()
        {
        }

        protected virtual byte ReadData(ushort readAddress)
        {
            return cells[readAddress];
        }

        protected virtual void WriteData(ushort writeAddress, byte data)
        {
            cells[writeAddress] = data;
        }

        // Debug interface

        public int Size { get { return cells.Length; } }

        public void LoadBytes(byte[] data)
        {
            Array.Clear(cells, 0, cells.Length);
            Buffer.BlockCopy(data, 0, cells, 0, data.Length);
        }

        public byte CheckByte(ushort address)
        {
            return cells[address];
        }

        public byte[] Cells { get { return cells; } }
    }
}