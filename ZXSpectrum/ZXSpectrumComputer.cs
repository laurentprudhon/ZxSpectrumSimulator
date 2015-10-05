using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Z80Simulator.Assembly;
using Z80Simulator.CPU;
using Z80Simulator.System;

namespace ZXSpectrum
{
    public class ZXSpectrumComputer
    {
        public Clock   Clock { get; private set; }

        public Z80CPU  CPU { get; private set; }

        public Bus<ushort> AddressBus { get; private set; }
        public Bus<byte>   DataBus    { get; private set; }

        public ROM                        ROM    { get; private set; }
        public DualAccessMemoryMappedChip RAM16K { get; private set; }
        public MemoryMappedChip           RAM32K { get; private set; }

        public Bus<ushort> VideoAddressBus { get; private set; }
        public Bus<byte>   VideoDataBus    { get; private set; }

        public ULA      ULA { get; private set; }

        public Screen       Screen      { get; private set; }

        public Keyboard     Keyboard    { get; private set; }
        public Bus<byte>    KeyboardDataBus { get; private set; }

        public Speaker      Speaker      { get; private set; }
        public TapeRecorder TapeRecorder { get; private set; }

        public Button ResetButton { get; private set; }

        public Joystick Joystick1 { get; private set; }
        public Joystick Joystick2 { get; private set; }

        public ZXSpectrumComputer()
        {
            // -- Initialize components --
            
            // System clock
            Clock = new _Clock(Clock.FREQ_3_5MHZ);
            // Init CPU
            CPU = new _Z80CPU();
            // Reset button
            ResetButton = new Button(SignalState.HIGH);

            // ZX Spectrum memory map
            /// NB : in the real machine, there is also a multiplexed address bus for each device : 
            /// select row (7 bits) then select colum (7bits). 
            /// Row and column address strobe - RAS/CAS - signals must therefore be generated.
            /// We do not simulate theses details here, as they are complex to understand 
            /// and can be ignored without losing any fildelity to the behavior of the machine.
            
            // 16K ROM
            // 0 - 16383 : ROM Program
            ROM = new _ROM();
            // 16K RAM
            // 16384 - 23295 : Video display memory (6910 bytes)
            // 23296 -  : System variables
            // User programs
            RAM16K = new _DualAccessMemoryMappedChip(Memory.BYTES_16K, 0x4000);
            // 32K RAM (only in 48K models)
            RAM32K = new _MemoryMappedChip(Memory.BYTES_32K, 0x8000);

            // Initialize ULA :
            //   Video generator
            //   CPU clock generator
            //   Memory access governor
            //   Keyboard controller
            //   Tape I/O controller
            //   Speaker controller
            ULA = new _ULA();

            // Initialize peripherals
            Keyboard = new _Keyboard();
            Screen = new _Screen();
            Speaker = new _Speaker();
            TapeRecorder = new _TapeRecorder();

            // Initialize Sinclair joysticks
            Joystick1 = new Joystick(true, Keyboard);
            Joystick2 = new Joystick(false, Keyboard);

            // -- Initialize and connect system buses --

            // CPU buses
            AddressBus = new Bus<ushort>();
            DataBus = new Bus<byte>();
            CPU.Address.ConnectTo(AddressBus);
            CPU.Data.ConnectTo(DataBus);
            ROM.Address.ConnectTo(AddressBus);
            ROM.Data.ConnectTo(DataBus);
            RAM16K.AddressInput1.ConnectTo(AddressBus);
            RAM16K.DataInput1.ConnectTo(DataBus);
            RAM32K.Address.ConnectTo(AddressBus);
            RAM32K.Data.ConnectTo(DataBus);
            ULA.Address.ConnectTo(AddressBus);
            ULA.Data.ConnectTo(DataBus);
            
            // Dedicated video buses : firect Video Memory Access for the ULA
            VideoAddressBus = new Bus<ushort>();
            VideoDataBus = new Bus<byte>();
            ULA.VideoAddress.ConnectTo(VideoAddressBus);
            ULA.VideoData.ConnectTo(VideoDataBus);
            RAM16K.AddressInput2.ConnectTo(VideoAddressBus);
            RAM16K.DataInput2.ConnectTo(VideoDataBus);

            // Connect Keyboard to the ULA
            KeyboardDataBus = new Bus<byte>();
            ULA.KeyboardData.ConnectTo(KeyboardDataBus);
            Keyboard.Address.ConnectTo(AddressBus);
            Keyboard.Data.ConnectTo(KeyboardDataBus);

             // Connect ULA analog video signal to screen
            ULA.ColorSignal.ConnectTo(Screen.ColorSignal);

            // Connect tape recorder sound output to ULA
            TapeRecorder.SoundSignal.ConnectTo(ULA.TapeInputSignal);

            // Connect ULA sound output to the speaker
            ULA.SpeakerSoundSignal.ConnectTo(Speaker.SoundSignal);

            // -- Connect individual PINs --

            ((_Clock)Clock).ConnectTo(ULA, Screen, TapeRecorder);
            ((_Z80CPU)CPU).ConnectTo(ResetButton, ULA, ROM, RAM16K, RAM32K);
            ((_ROM)ROM).ConnectTo(CPU);
            ((_DualAccessMemoryMappedChip)RAM16K).ConnectTo(CPU, ULA);
            ((_MemoryMappedChip)RAM32K).ConnectTo(CPU);
            ((_ULA)ULA).ConnectTo(Clock, CPU, RAM16K, Keyboard, Screen, Speaker);
            ((_Keyboard)Keyboard).ConnectTo(ULA);
            ((_Screen)Screen).ConnectTo(ULA);
            ((_Speaker)Speaker).ConnectTo(ULA);
            ((_TapeRecorder)TapeRecorder).ConnectTo(Clock);            
        }

        // Execution Interface

        public void ExecuteInstructionCount(int instructionCount)
        {
            InstructionCountCondition instructionCountCondition = new InstructionCountCondition(instructionCount);
            int exitConditionIndex = CPU.AddExitCondition(instructionCountCondition);
            Clock.TickUntilExitCondition();
            CPU.RemoveExitCondition(exitConditionIndex);
        }

        public void ExecuteFor20Milliseconds()
        {
            Clock.Tick(ULA.ONE_FRAME_TICKS_COUNT);
        }

        // Debug Interface

        public class ProgramInMemory
        {
            public Program Program { get; set; }
            public ushort BaseAddress { get; set; }
            public int MemorySize { get; set; }
        }
        
        public ProgramInMemory LoadProgramInROM(string program)
        {
            byte[] byteArray = Encoding.Unicode.GetBytes(program);
            MemoryStream stream = new MemoryStream(byteArray);
            return LoadProgramInMemory(String.Empty, stream, Encoding.Unicode, false, 0, ROM.Size);
        }

        public ProgramInMemory LoadProgramInMemory(string sourcePath, Stream inputStream, Encoding encoding, bool ignoreCase, ushort baseAddress, int memorySize)
        {
            Program program = Assembler.ParseProgram(sourcePath, inputStream, encoding, ignoreCase);
            MemoryMap programBytes = new MemoryMap(memorySize);
            Assembler.CompileProgram(program, 0, programBytes);
            LoadDataInMemory(programBytes.MemoryCells, baseAddress);
            return new ProgramInMemory() { Program = program, BaseAddress = baseAddress, MemorySize = memorySize };
        }
        
        public void LoadDataInMemory(Stream inputStream, ushort baseAddress, int memorySize)
        {
            using (BinaryReader reader = new BinaryReader(inputStream))
            {
                LoadDataInMemory(reader.ReadBytes(memorySize), baseAddress);
            }
        }

        public void LoadDataInMemory(byte[] data, ushort baseAddress)
        {
            if (baseAddress < 0x4000)
            {
                ROM.LoadData(data, baseAddress);
            }
            else if (baseAddress < 0x8000)
            {
                RAM16K.LoadData(data, baseAddress & 0x3FFF);
            }
            else if (baseAddress <= 0xFFFF)
            {
                RAM32K.LoadData(data, baseAddress & 0x7FFF);
            }
        }

        public int AddBreakpointOnProgramLine(ProgramInMemory programInMemory, int lineNumber)
        {
            ProgramLine programLine = programInMemory.Program.Lines[lineNumber - 1];
            InstructionAddressCondition instructionAddressCondition = new InstructionAddressCondition(programLine.LineAddress);
            int breakpointIndex = CPU.AddExitCondition(instructionAddressCondition);
            return breakpointIndex;
        }

        public void RemoveBreakpoint(int breakPointIndex)
        {
            CPU.RemoveExitCondition(breakPointIndex);
        }

        public void ExecuteUntilNextBreakpoint()
        {
            Clock.TickUntilExitCondition();
        }
        
        private class _Clock : Clock
        {           
            public _Clock(long frequencyHerz) : base(frequencyHerz)
            {}

            private ULA ula;
            private Screen screen;
            private TapeRecorder tapeRecorder;
            public void ConnectTo(ULA ula, Screen screen, TapeRecorder tapeRecorder)
            {
                this.ula = ula;
                this.screen = screen;
                this.tapeRecorder = tapeRecorder;
            }

            // Output PINs with notifications

            public override SignalState CLK
            {
                protected set
                {
                    base.CLK = value;
                    ula.ExecuteOnPixelClock();
                    screen.ExecuteOnPixelClock();
                    if(value == SignalState.LOW)
                    {
                        tapeRecorder.Tick();
                    }
                }
            } 
        }
        
        private class _Z80CPU : Z80CPU
        {
            private Button resetButton;
            private ULA ula;
            private ROM rom;
            private DualAccessMemoryMappedChip ram16K;
            private MemoryMappedChip ram32K;
            public void ConnectTo(Button resetButton, ULA ula, ROM rom, DualAccessMemoryMappedChip ram16K, MemoryMappedChip ram32K)
            {
                this.resetButton = resetButton;
                this.ula = ula;
                this.rom = rom;
                this.ram16K = ram16K;
                this.ram32K = ram32K;
            }

            // Input PINs sources

            public override SignalState BUSREQ
            {
                get { return SignalState.HIGH; }
            }

            public override SignalState CLK
            {
                get { return ula.CpuCLK; }
            }

            public override SignalState INT
            {
                get { return ula.CpuINT; }
            }

            public override SignalState NMI
            {
                get { return SignalState.HIGH; }
            }

            public override SignalState RESET
            {
                get { return resetButton.Output; }
            }

            public override SignalState WAIT
            {
                get { return SignalState.HIGH; }
            }

            // Output PINs with notifications

            public override SignalState RD
            {
                protected set
                {
                    base.RD = value;
                    if (value == SignalState.LOW)
                    {
                        rom.ReadEnable_OnEdgeLow();
                        ram16K.ReadEnable1_OnEdgeLow();
                        ram32K.ReadEnable_OnEdgeLow();
                    }
                    else
                    {
                        rom.ReadEnable_OnEdgeHigh();
                        ram16K.ReadEnable1_OnEdgeHigh();
                        ram32K.ReadEnable_OnEdgeHigh();
                    }
                }
            }

            public override SignalState WR
            {
                protected set
                {
                    base.WR = value;
                    if (value == SignalState.LOW)
                    {
                        ram16K.WriteEnable1_OnEdgeLow();
                        ram32K.WriteEnable_OnEdgeLow();
                    }
                }
            }
        }

        private class _ROM : ROM
        {
            private Z80CPU cpu;
            public void ConnectTo(Z80CPU cpu)
            {
                this.cpu = cpu;
            }

            // Bit masks
            private ushort A15A14 = 0xC000;
            private int A15A14_ROM = 0x0000;
        
            // Input PINs sources

            public override SignalState ChipSelect
            {
                get 
                {
                    if(cpu.MREQ == SignalState.LOW &&
                       (cpu.Address.SampleValue() & A15A14) == A15A14_ROM)
                    {
                        return SignalState.LOW;
                    }
                    else
                    {
                        return SignalState.HIGH;
                    }
                }
            }

            public override SignalState ReadEnable
            {
                get { return cpu.RD; }
            }
        }
        
        private class _DualAccessMemoryMappedChip : DualAccessMemoryMappedChip
        {
            public _DualAccessMemoryMappedChip(ushort capacityBytes, ushort startAddress) : base(capacityBytes, startAddress)
            { }

            private Z80CPU cpu;
            private ULA ula;
            public void ConnectTo(Z80CPU cpu, ULA ula)
            {
                this.cpu = cpu;
                this.ula = ula;
            }

            // Bit masks
            private ushort A15A14 = 0xC000;
            private int A15A14_RAM16K = 0x4000;

            // Input PINs sources

            public override SignalState ChipSelectInput1
            {
                get 
                {
                    if (cpu.MREQ == SignalState.LOW &&
                       (cpu.Address.SampleValue() & A15A14) == A15A14_RAM16K)
                    {
                        return SignalState.LOW;
                    }
                    else
                    {
                        return SignalState.HIGH;
                    }
                }
            }

            public override SignalState ChipSelectInput2
            {
                get { return ula.VideoMREQ; }
            }

            public override SignalState ReadEnable1
            {
                get { return cpu.RD; }
            }

            public override SignalState ReadEnable2
            {
                get { return ula.VideoRD; }
            }

            public override SignalState WriteEnable1
            {
                get { return cpu.WR; }
            }

            public override SignalState WriteEnable2
            {
                get { return SignalState.HIGH; }
            }
        }

        private class _MemoryMappedChip : MemoryMappedChip
        {
            public _MemoryMappedChip(ushort capacityBytes, ushort startAddress) : base(capacityBytes, startAddress)
            { }

            private Z80CPU cpu;
            public void ConnectTo(Z80CPU cpu)
            {
                this.cpu = cpu;
            }

            // Bit masks
            private ushort A15 = 0x8000;
            private int A15_RAM32K = 0x8000;

            // Input PINs sources

            public override SignalState ChipSelect
            {
                get
                {
                    if (cpu.MREQ == SignalState.LOW &&
                       (cpu.Address.SampleValue() & A15) == A15_RAM32K)
                    {
                        return SignalState.LOW;
                    }
                    else
                    {
                        return SignalState.HIGH;
                    }
                }
            }

            public override SignalState ReadEnable
            {
                get { return cpu.RD; }
            }

            public override SignalState WriteEnable
            {
                get { return cpu.WR; }
            }
        }

        private class _ULA : ULA
        {
            private Clock clock;
            private Z80CPU cpu;
            private DualAccessMemoryMappedChip ram16K;
            private Keyboard keyboard;
            private Screen screen;
            private Speaker speaker;
            public void ConnectTo(Clock clock, Z80CPU cpu, DualAccessMemoryMappedChip ram16K, Keyboard keyboard, Screen screen, Speaker speaker)
            {
                this.clock = clock;
                this.cpu = cpu;
                this.ram16K = ram16K;
                this.keyboard = keyboard;
                this.screen = screen;
                this.speaker = speaker;
            }

            // Input PINs sources

            public override SignalState CpuIORQ
            {
                get { return cpu.IORQ; }
            }

            public override SignalState CpuRD
            {
                get { return cpu.RD; }
            }

            public override SignalState CpuWR
            {
                get { return cpu.WR; }
            }

            public override SignalState PixelCLK
            {
                get { return clock.CLK; }
            }

            // Output PINs with notifications

            public override SignalState CpuCLK
            {
                protected set
                {
                    base.CpuCLK = value;
                    cpu.CLK_OnEdge();
                }
            }

            public override SignalState VideoRD
            {
                protected set
                {
                    base.VideoRD = value;
                    if (value == SignalState.LOW)
                    {
                        ram16K.ReadEnable2_OnEdgeLow();
                    }
                    else
                    {
                        ram16K.ReadEnable2_OnEdgeHigh();
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

            public override SignalState KeyboardRD
            {
                protected set
                {
                    base.KeyboardRD = value;
                    if (value == SignalState.LOW)
                    {
                        keyboard.ChipSelect_OnEdgeLow();
                    }
                    else
                    {
                        keyboard.ChipSelect_OnEdgeHigh();
                    }
                }
            }

            public override SignalState SoundSampleCLK
            {
                protected set
                {
                    base.SoundSampleCLK = value;
                    speaker.SampleSoundLevel();
                }
            }
        }

        private class _Keyboard : Keyboard
        {
            private ULA ula;
            public void ConnectTo(ULA ula)
            {
                this.ula = ula;
            }

            public override SignalState ChipSelect
            {
                get { return ula.KeyboardRD; }
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
        
        private class _Speaker : Speaker
        {
            private ULA ula;
            public void ConnectTo(ULA ula)
            {
                this.ula = ula;
            }

            // Input PINs sources

            public override SignalState SoundSampleCLK
            {
                get { return ula.SoundSampleCLK; }
            }
        }
            
        private class _TapeRecorder: TapeRecorder
        {
            private Clock clock;
            public void ConnectTo(Clock clock)
            {
                this.clock = clock;
            }

            // Input PINs sources

            public override SignalState TimeCLK
            {
                get { return clock.CLK; }
            }
        }
    }
}
