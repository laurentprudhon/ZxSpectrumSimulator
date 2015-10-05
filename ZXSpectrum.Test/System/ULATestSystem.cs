using System;
using System.IO;
using Z80Simulator.System;

namespace ZXSpectrum.Test.System
{
    /// <summary>
    /// Minimum test system : ULA without CPU
    /// clock + ULA + memory
    /// </summary>
    public class ULATestSystem
    {
        public Clock PixelClock { get; private set; }

        public Memory VideoMemory { get; private set; }

        public ULA ULA { get; private set; }

        public Screen Screen { get; private set; } 
        
        public ULATestSystem()
        {
            // -- Initialize components --

            PixelClock = new _Clock(Clock.FREQ_3_5MHZ);
            VideoMemory = new _Memory(32768);
            ULA = new _ULA();
            Screen = new _Screen();

            // -- Initialize and connect system buses --

            Bus<ushort> videoAddressBus = new Bus<ushort>();
            Bus<byte> videoDataBus = new Bus<byte>();
            VideoMemory.Address.ConnectTo(videoAddressBus);
            VideoMemory.Data.ConnectTo(videoDataBus);
            ULA.VideoAddress.ConnectTo(videoAddressBus);
            ULA.VideoData.ConnectTo(videoDataBus);

            // Unused CPU buses
            Bus<ushort> cpuAddressBus = new Bus<ushort>();
            Bus<byte> cpuDataBus = new Bus<byte>();
            ULA.Address.ConnectTo(cpuAddressBus);
            ULA.Data.ConnectTo(cpuDataBus);

            // Connect ULA analog video signal to screen
            ULA.ColorSignal.ConnectTo(Screen.ColorSignal);

            // Connect tape recorder sound output to ULA
            AnalogOutputPin<byte> noSound = new AnalogOutputPin<byte>(0);
            noSound.ConnectTo(ULA.TapeInputSignal);

            // -- Connect individual PINs --

            ((_Clock)PixelClock).ConnectTo(ULA, Screen);
            ((_Memory)VideoMemory).ConnectTo(ULA);
            ((_ULA)ULA).ConnectTo(PixelClock, VideoMemory, Screen);
            ((_Screen)Screen).ConnectTo(ULA);
        }
        
        public void LoadScreenInVideoMemory(string screenFilePath)
        {
            LoadScreenInVideoMemory(screenFilePath, VideoMemory.Cells, 0x4000);
        }

        public static void LoadScreenInVideoMemory(string screenFilePath, byte[] videoMemory, int startIndex)
        {
            // Load sample picture in memory and set green border color
            using (Stream stream = PlatformSpecific.GetStreamForProjectFile(screenFilePath))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    reader.Read(videoMemory, startIndex, 6912);
                }
            }
        }

        private class _Clock : Clock
        {
            public _Clock(long frequencyHerz)
                : base(frequencyHerz)
            { }

            private ULA ula;
            private Screen screen;
            public void ConnectTo(ULA ula, Screen screen)
            {
                this.ula = ula;
                this.screen = screen;
            }

            // Output PINs with notifications

            public override SignalState CLK
            {
                protected set
                {
                    base.CLK = value;
                    ula.ExecuteOnPixelClock();
                    screen.ExecuteOnPixelClock();
                }
            }
        }

        private class _Memory : Memory
        {
            public _Memory(ushort capacityBytes)
                : base(capacityBytes)
            { }

            private ULA ula;
            public void ConnectTo(ULA ula)
            {
                this.ula = ula;
            }

            // Input PINs sources

            public override SignalState ChipSelect
            {
                get { return ula.VideoMREQ; }
            }

            public override SignalState ReadEnable
            {
                get { return ula.VideoRD; }
            }

            public override SignalState Refresh
            {
                get { return SignalState.HIGH; }
            }

            public override SignalState WriteEnable
            {
                get { return SignalState.HIGH; }
            }
        }

        private class _ULA : ULA
        {
            private Clock clock;
            private Memory ram16K;
            private Screen screen;
            public void ConnectTo(Clock clock, Memory ram16K, Screen screen)
            {
                this.clock = clock;
                this.ram16K = ram16K;
                this.screen = screen;
            }

            // Input PINs sources

            public override SignalState CpuIORQ
            {
                get { return SignalState.HIGH; }
            }

            public override SignalState CpuRD
            {
                get { return SignalState.HIGH; }
            }

            public override SignalState CpuWR
            {
                get { return SignalState.HIGH; }
            }

            public override SignalState PixelCLK
            {
                get { return clock.CLK; }
            }

            // Output PINs with notifications

            public override SignalState VideoRD
            {
                protected set
                {
                    base.VideoRD = value;
                    if (value == SignalState.LOW)
                    {
                        ram16K.ReadEnable_OnEdgeLow();
                    }
                    else
                    {
                        ram16K.ReadEnable_OnEdgeHigh();
                    }
                }
            }

            public override SignalState HSync
            {
                protected set
                {
                    base.HSync = value;
                    if (value == SignalState.LOW)
                    {
                        screen.HSync_OnEdgeLow();
                    }
                    else
                    {
                        screen.HSync_OnEdgeHigh();
                    }
                }
            }

            public override SignalState VSync
            {
                protected set
                {
                    base.VSync = value;
                    if (value == SignalState.LOW)
                    {
                        screen.VSync_OnEdgeLow();
                    }
                    else
                    {
                        screen.VSync_OnEdgeHigh();
                    }
                }
            }
        }

        private class _Screen : Screen
        {
            private ULA ula;
            public void ConnectTo(ULA ula)
            {
                this.ula = ula;
            }

            // Input PINs sources

            public override SignalState HSync
            {
                get { return ula.HSync; }
            }

            public override SignalState PixelCLK
            {
                get { return ula.PixelCLK; }
            }

            public override SignalState VSync
            {
                get { return ula.VSync; }
            }
        }
    }    
}
