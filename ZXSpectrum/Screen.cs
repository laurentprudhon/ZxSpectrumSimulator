using System;
using Z80Simulator.System;

namespace ZXSpectrum
{
    public abstract class Screen
    {
        /// <summary>
        /// ZX Spectrum screen width : 32 px left border, 256 px display, 64 px right border
        /// </summary>
        public const int WIDTH = 352;

        /// <summary>
        /// ZX Spectrum screen height : 56 px top border, 192 px display, 56 px bottom border
        /// </summary>
        public const int HEIGHT = 304;
        
        /// <summary>
        /// Common time base with the video signal generator
        /// 
        /// Notifications : ExecuteOnPixelClock, ExecuteOnPixelClock
        /// </summary>
        public abstract SignalState PixelCLK { get; }

        // --- Video signal input ---
        
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
        public AnalogInputPin<byte> ColorSignal { get; private set; }

        /// <summary>
        /// Horizontal video synchronization signal : sends the electron beam back to the left of the screen
        /// 
        /// Notifications : HSync_OnEdgeLow, HSync_OnEdgeHigh
        /// </summary>
        public abstract SignalState HSync { get; }

        /// <summary>
        /// Vertical video synchronization signal : returns the electron beam to the top of the screen.
        /// 
        /// Notification : VSync_OnEdgeLow, VSync_OnEdgeHigh
        /// </summary>
        public abstract SignalState VSync { get; }

        // --- Screen Display ---

        /// <summary>
        /// Array of bytes representing the TV display 
        /// - 2 pixels per byte : 4 bits encoding the color of each pixel, like in the input video signal
        /// - byte index incremented from the left to the right and from the top to the bottom
        /// </summary>
        public byte[] Display { get; set; }

        // Optimization to generate the byte array
        private int internalDisplayIndex;

        // --- Electron beam position ---

        public int Line { get; private set; }
        public int Column { get; private set; }

        public bool BlankingPeriod { get; private set; }

        /// <summary>
        /// Power On
        /// </summary>
        public Screen()
        {
            ColorSignal = new AnalogInputPin<byte>();

            // To help unit tests, we pretend that the screen is already
            // synchronized with the ULA during the first frame
            Line = 56;
            Column = 32 /* Offset : because of the time necessary to read the first pixels in video memory, color output begins only 13 cycles after the master counter */ - 13 /* Offset : C0 is directly used to drive CPU clock, but CPU T state starts with High state while ULA counter should start with C0 low */ + 1;
            BlankingPeriod = false;

            // Initialize black screen
            Display = new byte[WIDTH * HEIGHT / 2];
            internalDisplayIndex = (Line * WIDTH + Column) / 2;
        }

        public void ExecuteOnPixelClock()
        {
            if (!BlankingPeriod)
            {
                bool isEvenColumnIndex = (Column % 2) == 0;
                if (isEvenColumnIndex)
                {
                    Display[internalDisplayIndex] = (byte)(ColorSignal.Level << 4);
                }
                else
                {
                    Display[internalDisplayIndex] |= ColorSignal.Level;
                    internalDisplayIndex++;
                }

                Column++;
            }
        }

        public void HSync_OnEdgeLow()
        {
            BlankingPeriod = true;
        }

        public void HSync_OnEdgeHigh()
        {
            Line++;
            Column = 0;
            if (VSync == SignalState.HIGH)
            {
                BlankingPeriod = false;
            }
        }

        public void VSync_OnEdgeLow()
        {
            BlankingPeriod = true;
        }

        public void VSync_OnEdgeHigh()
        {
            Line = 0;
            internalDisplayIndex = 0;
            if (HSync == SignalState.HIGH)
            {
                BlankingPeriod = false;
            }
        }

        // Debug Interface

        public byte[] CaptureDisplay()
        {
            int bufferSize = WIDTH * HEIGHT / 2;
            byte[] screenCapture = new byte[bufferSize];
            Buffer.BlockCopy(Display, 0, screenCapture, 0, bufferSize);
            return screenCapture;
        }
    }
}
