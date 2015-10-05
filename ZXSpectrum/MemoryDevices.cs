using System;
using Z80Simulator.System;

namespace ZXSpectrum
{
    /// <summary>
    /// Not derived from Memory for performance reasons
    /// </summary>
    public abstract class MemoryMappedChip
    {
        protected byte[] cells;

        public PartialAddressBusConnector Address { get; private set; }
        public BusConnector<byte> Data { get; private set; }

        /// <summary>
        /// No notifications
        /// </summary>
        public abstract SignalState ChipSelect { get; }
        /// <summary>
        /// Notifications : ReadEnable_OnEdgeLow, ReadEnable_OnEdgeHigh
        /// </summary>
        public abstract SignalState ReadEnable { get; }
        /// <summary>
        /// Notifications : WriteEnable_OnEdgeLow
        /// </summary>
        public abstract SignalState WriteEnable { get; }

        public MemoryMappedChip(ushort capacityBytes, ushort startAddress)
        {
            cells = new byte[capacityBytes];

            if (startAddress == 0x4000)
            {
                Address = new PartialAddressBusConnector(14);
            }
            else if (startAddress == 0x8000)
            {
                Address = new PartialAddressBusConnector(15);
            }
            else
            {
                throw new NotSupportedException();
            }
            Data = new  BusConnector<byte>();
        }

        public void ReadEnable_OnEdgeLow()
        {
            if (ChipSelect == SignalState.LOW)
            {
                ushort readAddress = Address.SampleValue();
                byte data = cells[readAddress];
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
                cells[writeAddress] = data;
            }
        }
        
        // Debug interface

        public int Size { get { return cells.Length; } }

        public void LoadBytes(byte[] data)
        {
            Array.Clear(cells, 0, cells.Length);
            Buffer.BlockCopy(data, 0, cells, 0, data.Length);
        }

        public byte[] Cells { get { return cells; } }

        internal void LoadData(byte[] data, int p)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Not derived from Memory for performance reasons
    /// </summary>
    public abstract class DualAccessMemoryMappedChip
    {
        protected byte[] cells;

        public PartialAddressBusConnector AddressInput1 { get; private set; }
        public BusConnector<byte> DataInput1 { get; private set; }
        /// <summary>
        /// No notifications
        /// </summary>
        public abstract SignalState ChipSelectInput1 { get; }
        /// <summary>
        /// Notifications : ReadEnable1_OnEdgeLow, ReadEnable1_OnEdgeHigh
        /// </summary>
        public abstract SignalState ReadEnable1 { get; }
        /// <summary>
        /// Notifications : WriteEnable1_OnEdgeLow, null
        /// </summary>
        public abstract SignalState WriteEnable1 { get; }

        public PartialAddressBusConnector AddressInput2 { get; private set; }
        public BusConnector<byte> DataInput2 { get; private set; }
        /// <summary>
        /// No notifications
        /// </summary>
        public abstract SignalState ChipSelectInput2 { get; }
        /// <summary>
        /// Notifications : ReadEnable2_OnEdgeLow, ReadEnable2_OnEdgeHigh
        /// </summary>
        public abstract SignalState ReadEnable2 { get; }
        /// <summary>
        /// Notifications : WriteEnable2_OnEdgeLow, null
        /// </summary>
        public abstract SignalState WriteEnable2 { get; }

        public DualAccessMemoryMappedChip(ushort capacityBytes, ushort startAddress)
        {
            cells = new byte[capacityBytes];

            if (startAddress == 0x4000)
            {
                AddressInput1 = new PartialAddressBusConnector(14);
                AddressInput2 = new PartialAddressBusConnector(14);
            }
            else if (startAddress == 0x8000)
            {
                AddressInput1 = new PartialAddressBusConnector(15);
                AddressInput2 = new PartialAddressBusConnector(15);
            }
            else
            {
                throw new NotSupportedException();
            }
            DataInput1 = new BusConnector<byte>();
            DataInput2 = new BusConnector<byte>();
        }

        public void ReadEnable1_OnEdgeLow()
        {
            if (ChipSelectInput1 == SignalState.LOW)
            {
                ushort readAddress = AddressInput1.SampleValue();
                byte data = cells[readAddress];
                DataInput1.SetValue(data);
            }
        }

        public void ReadEnable2_OnEdgeLow()
        {
            if (ChipSelectInput2 == SignalState.LOW)
            {
                ushort readAddress = AddressInput2.SampleValue();
                byte data = cells[readAddress];
                DataInput2.SetValue(data);
            }
        }

        public void ReadEnable1_OnEdgeHigh()
        {
            if (ChipSelectInput1 == SignalState.LOW)
            {
                DataInput1.ReleaseValue();
            }
        }

        public void ReadEnable2_OnEdgeHigh()
        {
            if (ChipSelectInput2 == SignalState.LOW)
            {
                DataInput2.ReleaseValue();
            }
        }

        public void WriteEnable1_OnEdgeLow()
        {
            if (ChipSelectInput1 == SignalState.LOW)
            {
                ushort writeAddress = AddressInput1.SampleValue();
                byte data = DataInput1.SampleValue();
                cells[writeAddress] = data;
            }
        }

        public void WriteEnable2_OnEdgeLow()
        {
            if (ChipSelectInput2 == SignalState.LOW)
            {
                ushort writeAddress = AddressInput2.SampleValue();
                byte data = DataInput2.SampleValue();
                cells[writeAddress] = data;
            }
        }
        
        // Debug interface

        public int Size { get { return cells.Length; } }

        public void LoadBytes(byte[] data)
        {
            Array.Clear(cells, 0, cells.Length);
            Buffer.BlockCopy(data, 0, cells, 0, data.Length);
        }

        public byte[] Cells { get { return cells; } }

        internal void LoadData(byte[] data, int p)
        {
            throw new NotImplementedException();
        }
    }    
}
