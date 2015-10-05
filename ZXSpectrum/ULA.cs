using System;
using Z80Simulator.System;

namespace ZXSpectrum
{
    /// <summary>
    /// The ZX Spectrum ULA
    /// How To Design A Microcomputer
    /// Chris Smith
    /// ZX Design and Media - http://www.zxdesign.info
    /// ISBN-13: 978-0-9565071-0-5
    /// 
    /// CPU clock generator / memory acces scheduler
    /// Video generator
    /// Keyboard controller
    /// Tape I/O controller
    /// Speaker controller
    /// </summary>
    public abstract partial class ULA
    {
        /// <summary>
        /// 7 MHz pixel clock input
        /// 
        /// Notifications : ExecuteOnPixelClock, ExecuteOnPixelClock
        /// </summary>
        public abstract SignalState PixelCLK { get; }

        /// <summary>
        /// Inverted 3.5 Mhz clock for the Z80
        /// </summary>
        public virtual SignalState CpuCLK
        {
            get { return _cpuCLK; }
            protected set { _cpuCLK = value; }
        }
        protected SignalState _cpuCLK;

        /// <summary>
        /// Z80 interrupt request signal
        /// </summary>
        public virtual SignalState CpuINT
        {
            get { return _cpuINT; }
            private set { _cpuINT = value; }
        }
        private SignalState _cpuINT;

        /// <summary>
        /// System address bus
        /// IO request : if A0 = 0, ULA port read or write
        /// Memory request : if A15.A14 = 0.1, video memory read or write
        /// </summary>
        public BusConnector<ushort> Address { get; private set; }

        /// <summary>
        /// System data bus
        /// </summary>
        public BusConnector<byte> Data { get; private set; }

        /// <summary>
        /// The ULA must handle CPU IO requests when A0 = 0
        /// </summary>
        public abstract SignalState CpuIORQ { get; }

        /// <summary>
        /// Z80 write enable. Indicates when the processor is performing a write operation to ULA port.
        /// </summary>
        public abstract SignalState CpuWR { get; }

        /// <summary>
        /// Z80 read enable. Indicates when the processor is performing a read operation to ULA port.
        /// </summary>
        public abstract SignalState CpuRD { get; }

        /// <summary>
        /// Dedicated video address bus (Direct Memory Access)
        /// </summary>
        public BusConnector<ushort> VideoAddress { get; private set; }

        /// <summary>
        /// Dedicated video data bus (Direct Memory Access)
        /// </summary>
        public BusConnector<byte> VideoData { get; private set; }

        /// <summary>
        /// Video memory request
        /// </summary>
        public virtual SignalState VideoMREQ
        {
            get { return _videoMREQ; }
            private set { _videoMREQ = value; }
        }
        private SignalState _videoMREQ;

        /// <summary>
        /// Read enable for video memory
        /// </summary>
        public virtual SignalState VideoRD
        {
            get { return _videoRD; }
            protected set { _videoRD = value; }
        }
        protected SignalState _videoRD;

        /// <summary>
        /// One byte represents the color of a pixel as follows
        ///   color = 0 0 0 0 bright green red blue
        /// 
        /// Examples :
        ///   black pixel = 0
        ///   dark red pixel = 2
        ///   gray pixel = 7
        ///   bright blue pixel = 8
        ///   white pixel = 15
        /// </summary>
        public AnalogOutputPin<byte> ColorSignal { get; private set; }

        /// <summary>
        /// Horizontal video synchronization signal : sends the electron beam back to the left of the screen
        /// </summary>
        public virtual SignalState HSync
        {
            get { return _hSync; }
            protected set { _hSync = value; }
        }
        protected SignalState _hSync;

        /// <summary>
        /// Vertical video synchronization signal : returns the electron beam to the top of the screen.
        /// </summary>
        public virtual SignalState VSync
        {
            get { return _vSync; }
            protected set { _vSync = value; }
        }
        protected SignalState _vSync;
        
        /// <summary>
        /// Keyboard data input K0 - K4 (5 bits)
        /// </summary>
        public BusConnector<byte> KeyboardData { get; private set; }

        /// <summary>
        /// Read enable for keyboard
        /// </summary>
        public virtual SignalState KeyboardRD
        {
            get { return _keyboardRD; }
            protected set { _keyboardRD = value; }
        }
        protected SignalState _keyboardRD;

        /// <summary>
        /// One byte represents the sound level sent to the speaker (0 or 1)
        /// </summary>
        public AnalogOutputPin<byte> SpeakerSoundSignal { get; private set; }

        /// <summary>
        /// One byte represents the sound level received from the tape recorder (0 or 1)
        /// </summary>
        public AnalogInputPin<byte> TapeInputSignal { get; private set; }

        /// <summary>
        /// One byte represents the sound level sent to the tape recorder (0 or 1)
        /// </summary>
        public AnalogOutputPin<byte> TapeOuputSignal { get; private set; }

        /// <summary>
        /// 15.6 Khz time reference for digital signal output in the Speaker
        /// </summary>
        public virtual SignalState SoundSampleCLK
        {
            get { return _soundSampleCLK; }
            protected set { _soundSampleCLK = value; }
        }
        protected SignalState _soundSampleCLK;


        /// <summary>
        /// Power On
        /// </summary>
        public ULA()
        {
            _cpuCLK = SignalState.LOW;
            _cpuINT = SignalState.HIGH;

            Address = new BusConnector<ushort>();
            Data = new BusConnector<byte>();
                        
            VideoAddress = new BusConnector<ushort>();
            VideoData = new BusConnector<byte>();

            _videoMREQ = SignalState.HIGH;
            _videoRD = SignalState.HIGH;

            ColorSignal = new AnalogOutputPin<byte>(0);
            _hSync = SignalState.HIGH;
            _vSync = SignalState.HIGH;

            KeyboardData = new BusConnector<byte>();
            _keyboardRD = SignalState.HIGH;

            SpeakerSoundSignal = new AnalogOutputPin<byte>(0);
            _soundSampleCLK = SignalState.HIGH;

            TapeInputSignal = new AnalogInputPin<byte>();
            TapeOuputSignal = new AnalogOutputPin<byte>(0);
        } 
                       
        /// <summary>
        /// The choice of a 7Mhz pixel clock produces 448 cycles in a 64 microsec display scan line period. 
        /// The master horizontal counter is configured to count through these 448 distinct states,
        /// providing the reference for display control signals such as the horizontal sync and
        /// all other internal system signals.
        /// 
        /// --- Horizontal timing ------
        /// Pixel output     0       255
        /// Right border     256     319
        /// Blanking period  320     415
        /// Horizontal sync  336     367 (ULA 5C)
        /// Horizontal sync  344     375 (ULA 6C)
        /// Left border      416     447
        /// ----------------------------
        /// </summary>
        private int column = 1; // C0 is directly used to drive CPU clock, but CPU T state starts with High state while ULA counter should start with C0 low

        /// <summary>
        /// The ULA keeps track of how many scan-line periods have occured, and therefore which line it is
        /// currently diplaying. The PAL variant of the ZX Spectrum ULA develops 312 scan lines per televison
        /// frame, and so the vertical line counter is reset as it advances from 311.
        /// 
        /// --- Vertical timing --------
        /// Display          0       191      
        /// Bottom border    192     247
        /// Sync period      248     255
        /// Sync pulse       248     251
        /// Top border       256     311
        /// ----------------------------
        /// </summary>
        private int line = 0;

        /// <summary>
        /// These signals indicate when the pixel display is being generated, otherwise the border is generated.
        /// </summary>
        private bool generateBorder = false;
        private bool displayPixels = false; // offset from generate border by 8 clock periods, to let the ULA fetch pixels from memory
            
        private int frame = 0;
        private bool flashClock = false;
        
        private bool videoMemAccessTimeFrame = false;
        private bool haltCpuClock = false;
        private byte cpuMemoryOrIORequestHalfTState = 0;
        private bool cpuIORequestToULA = false;
        private bool cpuIORequestToULAIsREAD = true;

        // --- Display ---
        // The ZX Spectrum display has a resolution of 256 * 192 pixels,
        // organised as a virtual 32 * 24 character grid,
        // in which a character is one byte wide and eight bytes high.
        //
        // Each character cell is assigned a background and foreground color.
        // The colour information folows the display bytes in memory,
        // and divides the display into a grid of 32 * 24 colour cells.
        //
        // To keep the display updated, the ZX Spectrum repeatedly fetches
        // pairs of bytes from memory, one containing the pixel information,
        // the other colour. It feeds the pixel display byte through a shift register
        // to extract each individual pixel in turn, and uses them to select wether
        // a foreground or a background colour is sent to the television.
        //
        // The foreground and the background colour themselves are determined
        // by the pixel's associated colour byte.
        // Attribute byte format :
        // Modifier Background Foreground
        // 7  6     5  4  3    2  1  0 
        // FL HL    G  R  B    G  R  B
        // FL : flash mode / HL : highlight, brightness increase / G R B : three bit RGB colour palette
        // ---------------    

        private ushort displayAddress = 0;
        private ushort attributeAddress = 0;

        private byte displayLatch = 0;
        private byte displayRegister = 0;
        private byte attributeLatch = 0;
        private byte attributeRegister = 0;
        private byte borderColorRegister = 0;

        // Bit masks used to check address bus values
        private int A15A14 = 0xC000;
        private int VideoMem = 0x4000;
        private int A0 = 0x0001;
        private int ULAPort = 0x00FE;

        private byte ULAPort_Read_Keyboard  = 0x1F;
        private byte ULAPort_Read_EAR       = 6;
        private byte ULAPort_Read_Bits5And7 = 0xA0;
        private byte ULAPort_Write_Border   = 0x07;
        private byte ULAPort_Write_MIC      = 0x08;
        private byte ULAPort_Write_Speaker  = 0x10;

        private bool soundOutputWasSetByTheCPU = false;

        /// <summary>
        /// Number of ticks of the pixel clock to generate a complete frame
        /// </summary>
        public const int ONE_FRAME_TICKS_COUNT = 69888;

        /// <summary>
        /// All ULA timing states are synchronized to this internal 7MHz clock.
        /// </summary>
        public void ExecuteOnPixelClock()
        {
            // Debug interface : breakpoints management
            ExitConditionException pendingExitException = null;

            // Video signals generation is aligned on a 16 pixels pattern
            int videoMem16StepsAccessPatternIndex = column % 16;

            // --- CPU/ULA video memory contention
            
            // The ULA accesses video memory from pixels 8 to 15
            // outside the border generation periods. To make sure
            // no CPU memory or IO access can overlap this period
            // we must stop any CPU memory or IO instruction starting
            // after pixel 4 and halt the CPU clock until next pixel 0.
            switch (videoMem16StepsAccessPatternIndex)
            {
                case 0:
                    videoMemAccessTimeFrame = false;
                    break;

                case 4:
                    if (!generateBorder)
                    {
                        videoMemAccessTimeFrame = true;
                    }
                    break;
            }

            if (videoMemAccessTimeFrame)
            {
                if (!haltCpuClock) // No need to check for CPU memory or IO requests while it is already halted
                {
                    if (// CPU is about to access the video memory
                        // CPU is about to access the ULA IO port
                        cpuMemoryOrIORequestHalfTState == 1)
                    {
                        haltCpuClock = true;
                    }
                }
            }
            else
            {
                if (haltCpuClock) 
                {
                    haltCpuClock = false;
                }
            }

            // --- CPU interrupt
            if (line == 248)
            {
                if (column == 0 /* Offset : because of the time necessary to read the first pixels in video memory, color output begins only 13 cycles after the master counter */ + 13)
                {
                    CpuINT = SignalState.LOW;
                }
                else if (column == 32 /* Offset : because of the time necessary to read the first pixels in video memory, color output begins only 13 cycles after the master counter */ + 13)
                {
                    CpuINT = SignalState.HIGH;
                }
            }

            // Connect TapeInput and SoundOutput
            if (TapeInputSignal.Level == 1)
            {
                SpeakerSoundSignal.Level = 1;
            }
            else if (!soundOutputWasSetByTheCPU)
            {
                SpeakerSoundSignal.Level = 0;
            }

            if (!haltCpuClock)
            {
                // --- CPU clock
                CpuCLK = PixelCLK; // C0 is directly used to drive CPU clock, but CPU T state starts with High state while ULA counter should start with C0 low

                // Check for CPU memory or IO access that could interfere with ULA operations
                if (cpuMemoryOrIORequestHalfTState == 0)
                {
                    // CPU is about to access the video memory
                    bool cpuMemoryOrIORequestToVideoAddress = (Address.SampleValue() & A15A14) == VideoMem;
                    // CPU is about to access the ULA IO port
                    cpuIORequestToULA = (CpuIORQ == SignalState.LOW) && ((Address.SampleValue() & A0) == (ULAPort & A0));
                    if (cpuIORequestToULA)
                    {
                        // Check to see if it is a read operation
                        cpuIORequestToULAIsREAD = CpuWR == SignalState.HIGH;
                    }
                    if (cpuMemoryOrIORequestToVideoAddress || cpuIORequestToULA)
                    {
                        cpuMemoryOrIORequestHalfTState = 1;
                    }
                }
                else if (cpuMemoryOrIORequestHalfTState > 0)
                {
                    cpuMemoryOrIORequestHalfTState++;
                }

                // --- CPU IO requests to ULA port
                if (cpuIORequestToULA)
                {
                    if (cpuIORequestToULAIsREAD)
                    {
                        if (cpuMemoryOrIORequestHalfTState == 5)
                        {
                            byte inputData = 0;
                            // Read keyboard state
                            KeyboardRD = SignalState.LOW;
                            inputData = (byte)(KeyboardData.SampleValue() & ULAPort_Read_Keyboard);
                            KeyboardRD = SignalState.HIGH;
                            // Load cassette
                            inputData |= (byte)(TapeInputSignal.Level << ULAPort_Read_EAR);
                            // Bits 5 and 7 as read by INning from Port 0xfe are always one
                            inputData |= ULAPort_Read_Bits5And7;
                            Data.SetValue(inputData);
                        }
                        else if (cpuMemoryOrIORequestHalfTState == 6)
                        {
                            Data.ReleaseValue(); 
                        }
                    }
                    else
                    {
                        if (cpuMemoryOrIORequestHalfTState == 3)
                        {
                            byte writeData = Data.SampleValue();
                            // Write borderColor register
                            borderColorRegister = (byte)(writeData & ULAPort_Write_Border);
                            // Output sound
                            SpeakerSoundSignal.Level = (byte)((writeData & ULAPort_Write_Speaker) >> 4);
                            if (SpeakerSoundSignal.Level == 1)
                            {
                                soundOutputWasSetByTheCPU = true;
                            }
                            else { soundOutputWasSetByTheCPU = false; }
                            // Save cassette
                            TapeOuputSignal.Level = (byte)(writeData & ULAPort_Write_MIC);
                        }
                    }
                }
                else if (cpuMemoryOrIORequestHalfTState == 3)
                {
                    // CPU is about to access the ULA IO port
                    cpuIORequestToULA = (CpuIORQ == SignalState.LOW) && ((Address.SampleValue() & A0) == (ULAPort & A0));
                    if (cpuIORequestToULA)
                    {
                        // Check to see if it is a read operation
                        cpuIORequestToULAIsREAD = CpuWR == SignalState.HIGH;
                    }
                    if (cpuIORequestToULA)
                    {
                        cpuMemoryOrIORequestHalfTState = 1;
                    }
                }
            }

            if (cpuMemoryOrIORequestHalfTState == 6)
            {
                cpuMemoryOrIORequestHalfTState = 0;
            }

            // --- Compute pixel colors ---
            if (displayPixels) // First thing to do at the falling edge of the clock
            {
                switch (videoMem16StepsAccessPatternIndex)
                {
                    case 5:
                        displayRegister = displayLatch;
                        break;

                    case 13:
                        displayRegister = displayLatch;
                        break;
                }
            }
            switch (videoMem16StepsAccessPatternIndex)
            {
                case 5:
                    MultiplexAttributeLatchWithBorderColor();
                    break;

                case 13:
                    MultiplexAttributeLatchWithBorderColor();
                    break;
            }

            // --- Load two display and attribute bytes for 16 pixels from video memory ---
            if (!generateBorder)
            {
                switch (videoMem16StepsAccessPatternIndex)
                {
                    case 7:
                        ComputeVideoAddresses();
                        break;

                    case 8:
                        // display address -> address bus
                        VideoAddress.SetValue(displayAddress);
                        VideoMREQ = SignalState.LOW;
                        VideoRD = SignalState.LOW;
                        break;

                    case 9:
                        displayLatch = VideoData.SampleValue();
                        VideoRD = SignalState.HIGH;
                        VideoMREQ = SignalState.HIGH;
                        VideoAddress.ReleaseValue();
                        break;

                    case 10:
                        // attribute adress -> address bus
                        VideoAddress.SetValue(attributeAddress);
                        VideoMREQ = SignalState.LOW;
                        VideoRD = SignalState.LOW;
                        break;

                    case 11:
                        attributeLatch = VideoData.SampleValue();
                        VideoRD = SignalState.HIGH;
                        VideoMREQ = SignalState.HIGH;
                        VideoAddress.ReleaseValue();
                        ComputeVideoAddresses();
                        break;

                    case 12:
                        // display colum address -> address bus
                        VideoAddress.SetValue(displayAddress);
                        VideoMREQ = SignalState.LOW;
                        VideoRD = SignalState.LOW;
                        break;

                    case 13:
                        displayLatch = VideoData.SampleValue();
                        VideoRD = SignalState.HIGH;
                        VideoMREQ = SignalState.HIGH;
                        VideoAddress.ReleaseValue();
                        break;

                    case 14:
                        // attribute adress -> address bus
                        VideoAddress.SetValue(attributeAddress);
                        VideoMREQ = SignalState.LOW;
                        VideoRD = SignalState.LOW;
                        break;

                    case 15:
                        attributeLatch = VideoData.SampleValue();
                        VideoRD = SignalState.HIGH;
                        VideoMREQ = SignalState.HIGH;
                        VideoAddress.ReleaseValue();
                        break;
                }
            }

            // --- For each pixel : Video output signals ---

            // Generate horizontal and vertical synchronization signals
            if (column == 320 /* Offset : because of the time necessary to read the first pixels in video memory, color output begins only 13 cycles after the master counter */ + 13)
            {
                HSync = SignalState.LOW;
            }
            else if (column == 416 /* Offset : because of the time necessary to read the first pixels in video memory, color output begins only 13 cycles after the master counter */ + 13)
            {
                HSync = SignalState.HIGH;
                if (line == 247)
                {
                    VSync = SignalState.LOW;
                }
                else if (line == 255)
                {
                    VSync = SignalState.HIGH;
                }
            }

            // Compute pixel color for video output
            if (HSync == SignalState.LOW || VSync == SignalState.LOW)
            {
                // Blanking
                ColorSignal.Level = 0;
            }
            else
            {
                MultiplexDisplayRegisterWithAttributeRegisterAndFlashClock();
            }

            // Debug notification
            if (pendingExitException == null)
            {
                pendingExitException = NotifyLifecycleEvent(LifecycleEventType.ClockCycle);
            }
            
            // --- Increment master counters and prepare next iteration ---

            // Shift display register bits
            displayRegister = (byte)(displayRegister << 1);

            column++;
            if(column == 8)
            {
                if (!generateBorder)
                {
                    displayPixels = true;
                }
            }
            else if (column == 256)
            {
                generateBorder = true;
                videoMemAccessTimeFrame = false;
            }
            else if (column == 264)
            {
                if (displayPixels)
                {
                    displayPixels = false;
                }
            }
            else if (column == 448)
            {
                column = 0;
                line++;

                // At each end of line send sound sample signal to the speaker
                // => 7 Mhz pixel clock / 448 pixels per line = 15.6 Khz sound sampling frequency
                SoundSampleCLK = SoundSampleCLK == SignalState.LOW ? SignalState.HIGH : SignalState.LOW;

                if (line < 192)
                {
                    generateBorder = false;
                }

                if (line == 312)
                {
                    line = 0;
                    generateBorder = false;
                    frame++;

                    // Each 16 frames, invert flash clock
                    if (frame == 16)
                    {
                        frame = 0;
                        flashClock = !flashClock;
                    }
                }
            }

            // Debug : if a breakpoint was hit, notify the pixel clock via a specific exception
            if (pendingExitException != null)
            {
                throw pendingExitException;
            }
        }

        // Bit masks used to compute pixel colors
        // -> attribute byte :
        //       [backgd] [foregd]
        // 7  6  5  4  3  2  1  0 
        // FL HL g  r  b  g  r  b
        private byte foregroundColorBits = 0x07;
        private byte backgroundColorBits = 0x38;
        private byte highlightBit = 0x40;
        private byte flashBit = 0x80;
        // -> display byte
        // 7  6  5  4  3  2  1  0
        // p1 p2 p3 p4 p5 p6 p7 p8
        // with : 
        //  p1 = leftmost pixel on the screen
        //  p8 = rightmost pixel on the screen
        //  pixel = 0 => background color
        //  pixel = 1 => foreground color
        private byte nextPixelBit = 0x80;

        private void MultiplexAttributeLatchWithBorderColor()
        {
            if (displayPixels)
            {
                attributeRegister = attributeLatch;
            }
            else
            {
                attributeRegister = (byte)(attributeLatch & foregroundColorBits);
                attributeRegister |= (byte)(borderColorRegister << 3); 
            }
        }

        private void MultiplexDisplayRegisterWithAttributeRegisterAndFlashClock()
        {
            // Highlight value
            ColorSignal.Level = (byte)((attributeRegister & highlightBit) >> 3);

            // Foreground or background color ?
            bool selectForegroundColor = (displayRegister & nextPixelBit) == nextPixelBit;
            if ((attributeRegister & flashBit) == flashBit)
            {
                if (flashClock) selectForegroundColor = !selectForegroundColor;
            }

            // Color value
            if (selectForegroundColor)
            {
                ColorSignal.Level |= (byte)(attributeRegister & foregroundColorBits);
            }
            else
            {
                ColorSignal.Level |= (byte)((attributeRegister & backgroundColorBits) >> 3);
            }
        }

        // Bit masks used to generate video addresses
        private byte c7c6c5c4c3 = 0xF8;
        private byte v2v1v0 = 0x07;
        private byte v5v4v3 = 0x38;
        private byte v7v6 = 0xC0;

        private void ComputeVideoAddresses()
        {
            // common row address       = v4 v3 c7 c6 c5 c4 c3            
            // display column address   =  0 v7 v6 v2 v1 v0 v5
            // attribute column address =  0  1  1  0 v7 v6 v5

            ushort commonPart = (ushort)(
                                        ((column & c7c6c5c4c3) >> 3) |
                                        ((line & v5v4v3) << 2)
                                        );

            displayAddress = (ushort)commonPart;
            displayAddress |= (ushort)(
                                        ((line & v2v1v0) << 8) |
                                        ((line & v7v6) << 5)
                                        );
            displayAddress |= 0x4000; // video memory base address

            attributeAddress = (ushort)commonPart;
            attributeAddress |= (ushort)(
                                        ((line & v7v6) << 2) |
                                        0x1800
                                        );
            attributeAddress |= 0x4000; // video memory base address
        }
    }
}

