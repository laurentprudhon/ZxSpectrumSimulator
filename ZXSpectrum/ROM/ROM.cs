using System;
using System.IO;
using System.Text;
using Z80Simulator.Assembly;
using Z80Simulator.System;

namespace ZXSpectrum
{
    /// <summary>
    /// ZX Spectrum 16K ROM
    /// 0 - 16383 : ROM Program
    /// John Grant & Steven Vickers
    /// Nine Tiles Information Handling Ltd
    /// </summary>
    public abstract class ROM // Not derived from Memory for performance reasons
    {
        protected byte[] cells;

        public BusConnector<ushort> Address { get; private set; }
        public BusConnector<byte> Data { get; private set; }

        /// <summary>
        /// No notifications
        /// </summary>
        public abstract SignalState ChipSelect { get; }
        /// <summary>
        /// Notifications : ReadEnable_OnEdgeLow, ReadEnable_OnEdgeHigh
        /// </summary>
        public abstract SignalState ReadEnable { get; }

        public ROM()
        {
            cells = new byte[Memory.BYTES_16K];

            Address = new BusConnector<ushort>();
            Data = new BusConnector<byte>();

            string sourcePath = "zxspectrum.asm";
            Stream romProgramFileStream = PlatformSpecific.GetStreamForProjectFile(sourcePath);
            Program = Assembler.ParseProgram(sourcePath, romProgramFileStream, Encoding.UTF8, false);
            MemoryMap programBytes = new MemoryMap(cells.Length);
            Assembler.CompileProgram(Program, 0, programBytes);
            cells = programBytes.MemoryCells;
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

        // Debug interface

        public int Size { get { return cells.Length; } }

        public byte[] Cells { get { return cells; } }

        public Program Program { get; private set; }

        public void LoadData(byte[] data, ushort baseAddress)
        {
            Buffer.BlockCopy(data, 0, cells, baseAddress, data.Length);
        }
    }
}
