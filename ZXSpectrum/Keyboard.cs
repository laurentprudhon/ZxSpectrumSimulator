using System;
using System.Collections.Generic;
using Z80Simulator.System;

namespace ZXSpectrum
{
    /// <summary>
    /// The ZX Spectrum keyboard is implemented as an array of 40 switches.
    /// 
    /// ---------------------------------------------------------------------------
    /// 
    ///          0     1     2     3     4 -Bits-  4     3     2     1     0
    ///  PORT                                                                    PORT
    /// 
    ///  F7FE  [ 1 ] [ 2 ] [ 3 ] [ 4 ] [ 5 ]  |  [ 6 ] [ 7 ] [ 8 ] [ 9 ] [ 0 ]   EFFE
    ///   ^                                   |                                   v
    ///  FBFE  [ Q ] [ W ] [ E ] [ R ] [ T ]  |  [ Y ] [ U ] [ I ] [ O ] [ P ]   DFFE
    ///   ^                                   |                                   v
    ///  FDFE  [ A ] [ S ] [ D ] [ F ] [ G ]  |  [ H ] [ J ] [ K ] [ L ] [ ENT ] BFFE
    ///   ^                                   |                                   v
    ///  FEFE  [SHI] [ Z ] [ X ] [ C ] [ V ]  |  [ B ] [ N ] [ M ] [sym] [ SPC ] 7FFE
    ///   ^                                                                       v
    ///  Start                                                                   End
    /// 
    /// ---------------------------------------------------------------------------
    /// 
    /// </summary>
    public abstract class Keyboard
    {
        /// <summary>
        /// Physical keyboard layout
        /// </summary>
        public enum Keys
        {
            Num1, Num2, Num3, Num4, Num5, Num6, Num7, Num8, Num9, Num0,
            Q   , W   , E   , R   , T   , Y   , U   , I   , O   , P   ,
            A   , S   , D   , F   , G   , H   , J   , K   , L   , Enter ,
       CapsShift, Z   , X   , C   , V   , B   , N   , M   , SymbolShift, Space   
        }

        /// <summary>
        /// Electrical switches position in the grid of :
        /// - address bus lines : A8 - A15
        /// - keyboard output columns : K0 - K4
        /// </summary>
        private static IDictionary<Keys, SwitchPosition> KeySwitchesPositions = new Dictionary<Keys, SwitchPosition>();
        static Keyboard()
        {
            KeySwitchesPositions[Keys.CapsShift] = new SwitchPosition(0, 0); KeySwitchesPositions[Keys.Z]    = new SwitchPosition(0, 1); KeySwitchesPositions[Keys.X]    = new SwitchPosition(0, 2); KeySwitchesPositions[Keys.C]           = new SwitchPosition(0, 3); KeySwitchesPositions[Keys.V]     = new SwitchPosition(0, 4); 
            KeySwitchesPositions[Keys.A]         = new SwitchPosition(1, 0); KeySwitchesPositions[Keys.S]    = new SwitchPosition(1, 1); KeySwitchesPositions[Keys.D]    = new SwitchPosition(1, 2); KeySwitchesPositions[Keys.F]           = new SwitchPosition(1, 3); KeySwitchesPositions[Keys.G]     = new SwitchPosition(1, 4); 
            KeySwitchesPositions[Keys.Q]         = new SwitchPosition(2, 0); KeySwitchesPositions[Keys.W ]   = new SwitchPosition(2, 1); KeySwitchesPositions[Keys.E]    = new SwitchPosition(2, 2); KeySwitchesPositions[Keys.R]           = new SwitchPosition(2, 3); KeySwitchesPositions[Keys.T]     = new SwitchPosition(2, 4); 
            KeySwitchesPositions[Keys.Num1]      = new SwitchPosition(3, 0); KeySwitchesPositions[Keys.Num2] = new SwitchPosition(3, 1); KeySwitchesPositions[Keys.Num3] = new SwitchPosition(3, 2); KeySwitchesPositions[Keys.Num4]        = new SwitchPosition(3, 3); KeySwitchesPositions[Keys.Num5]  = new SwitchPosition(3, 4); 
           
            KeySwitchesPositions[Keys.Num6]      = new SwitchPosition(4, 4); KeySwitchesPositions[Keys.Num7] = new SwitchPosition(4, 3); KeySwitchesPositions[Keys.Num8] = new SwitchPosition(4, 2); KeySwitchesPositions[Keys.Num9]        = new SwitchPosition(4, 1); KeySwitchesPositions[Keys.Num0]  = new SwitchPosition(4, 0); 
            KeySwitchesPositions[Keys.Y]         = new SwitchPosition(5, 4); KeySwitchesPositions[Keys.U]    = new SwitchPosition(5, 3); KeySwitchesPositions[Keys.I]    = new SwitchPosition(5, 2); KeySwitchesPositions[Keys.O]           = new SwitchPosition(5, 1); KeySwitchesPositions[Keys.P]     = new SwitchPosition(5, 0); 
            KeySwitchesPositions[Keys.H]         = new SwitchPosition(6, 4); KeySwitchesPositions[Keys.J]    = new SwitchPosition(6, 3); KeySwitchesPositions[Keys.K]    = new SwitchPosition(6, 2); KeySwitchesPositions[Keys.L]           = new SwitchPosition(6, 1); KeySwitchesPositions[Keys.Enter] = new SwitchPosition(6, 0); 
            KeySwitchesPositions[Keys.B]         = new SwitchPosition(7, 4); KeySwitchesPositions[Keys.N]    = new SwitchPosition(7, 3); KeySwitchesPositions[Keys.M]    = new SwitchPosition(7, 2); KeySwitchesPositions[Keys.SymbolShift] = new SwitchPosition(7, 1); KeySwitchesPositions[Keys.Space] = new SwitchPosition(7, 0); 
        }

        private struct SwitchPosition
        {
            public byte AddressLine;
            public byte KeyboardOutputColumn;

            public SwitchPosition(byte addressLine, byte keyboardOutputColumn)
            {
                AddressLine = addressLine;
                KeyboardOutputColumn = keyboardOutputColumn;
            }
        }
        
        /// <summary>
        /// Buffer used to remeber wich keys are pressed at any point in time
        /// </summary>
        private bool[,] keySwitchesPressed = new bool[8,5];

        public void OnKeyPress(Keys key)
        {
            SwitchPosition switchPosition  = KeySwitchesPositions[key];
            keySwitchesPressed[switchPosition.AddressLine, switchPosition.KeyboardOutputColumn] = true;
        }

        public void OnKeyRelease(Keys key)
        {
            SwitchPosition switchPosition = KeySwitchesPositions[key];
            keySwitchesPressed[switchPosition.AddressLine, switchPosition.KeyboardOutputColumn] = false;
        }

        // System interface

        /// <summary>
        /// A8 - A15 : Upper 8 bits of the address bus are used 
        /// to select a keybord half row in the matrix of switches
        /// </summary>
        public BusConnector<ushort> Address { get; private set; }

        /// <summary>
        /// K0 - K4 : Keyboard output colmumns.
        /// One of the first 5 bits is equal to 1 if at least one key 
        /// in the corresponding column (bit 4 -> colmumn 4)
        /// is pressed in a half row selected by the adress bus.
        /// </summary>
        public BusConnector<byte> Data { get; private set; }

        /// <summary>
        /// When ChipSelect is activated, the keyboard output is refreshed
        /// 
        /// Notifications : ChipSelect_OnEdgeLow, ChipSelect_OnEdgeHigh
        /// </summary>
        public abstract SignalState ChipSelect { get; }

        public Keyboard()
        {
            Address = new BusConnector<ushort>();
            Data = new BusConnector<byte>();
        }

        // Simulation of the array of switches
        
        public void ChipSelect_OnEdgeLow()
        {
            // Reset keyboard output
            byte keyboardOutput = 0xFF;

            // Read address bus upper byte
            int addressBusA8A15 = Address.SampleValue() >> 8;

            // List activated address lines
            IList<int> selectedAddressLines = new List<int>();
            for (int addressLineIndex = 0; addressLineIndex < 8; addressLineIndex++)
            {
                if (((addressBusA8A15 >> addressLineIndex) & 1) == 0)
                {
                    selectedAddressLines.Add(addressLineIndex);
                }
            }

            // Compute the value of each keyboard output column
            for (int keyboardOutputColumn = 0; keyboardOutputColumn < 5; keyboardOutputColumn++)
            {
                bool currentColumnPulledDownToZero = false;
                foreach (int addressLine in selectedAddressLines)
                {
                    if (keySwitchesPressed[addressLine, keyboardOutputColumn])
                    {
                        currentColumnPulledDownToZero = true;
                        break;
                    }
                }
                if (currentColumnPulledDownToZero)
                {
                    keyboardOutput = (byte)(keyboardOutput ^ (1 << keyboardOutputColumn));
                }
            }

            // Send keyboard output to data bus
            Data.SetValue(keyboardOutput);
        }

        public void ChipSelect_OnEdgeHigh()
        {
            Data.ReleaseValue();
        }
    }
}
