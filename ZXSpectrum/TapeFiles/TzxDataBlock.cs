using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZXSpectrum.TapeFiles
{
    public enum TzxDataBlockType
    { 
        /**/StandardSpeedDataBlock,
        /**/TurboSpeedDataBlock,
        /**/PureTone,
        /**/PulseSequence,
        /**/PureDataBlock,
        /**/DirectRecording,
        C64ROMBlock,            // (deprecated)
        C64TurboBlock,          // (deprecated)
        CSWRecordingBlock,      // v1.20
        GeneralizedDataBlock,   // v1.20
        /**/PauseOrStopTheTape,
        /**/GroupStart,
        /**/GroupEnd,  
        JumpToBlock,
        LoopStart,
        LoopEnd,
        CallSequence,
        ReturnFromSequence,
        SelectBlock,
        StopTapeIn48kMode,
        SetSignalLevel,         // v1.20
        /**/TextDescription,
        Message,
        /**/ArchiveInfo,
        HardwareType,
        EmulationInfo,          // (deprecated)
        CustomInfo,
        SCREENSBlock,           // (deprecated)
        SkipBlock
    }

    public abstract class TzxDataBlock
    {
        /// <summary>
        /// selector for the type of following data block
        /// </summary>
        public byte ID { get; set; }

        public TzxDataBlockType Type
        {
            get
            {
                return GetTZXDataBlockTypeFromID(ID);
            }
        }

        public static TzxDataBlockType GetTZXDataBlockTypeFromID(byte id)
        {
            switch(id)
            {
                case 0x10:
                    return TzxDataBlockType.StandardSpeedDataBlock;
                case 0x11:
                    return TzxDataBlockType.TurboSpeedDataBlock;
                case 0x12:
                    return TzxDataBlockType.PureTone;
                case 0x13:
                    return TzxDataBlockType.PulseSequence;
                case 0x14:
                    return TzxDataBlockType.PureDataBlock;
                case 0x15:
                    return TzxDataBlockType.DirectRecording;
                case 0x16:
                    return TzxDataBlockType.C64ROMBlock;
                case 0x17:
                    return TzxDataBlockType.C64TurboBlock;
                case 0x18:
                    return TzxDataBlockType.CSWRecordingBlock;
                case 0x19:
                    return TzxDataBlockType.GeneralizedDataBlock;
                case 0x20:
                    return TzxDataBlockType.PauseOrStopTheTape;
                case 0x21:
                    return TzxDataBlockType.GroupStart;
                case 0x22:
                    return TzxDataBlockType.GroupEnd;
                case 0x23:
                    return TzxDataBlockType.JumpToBlock;
                case 0x24:
                    return TzxDataBlockType.LoopStart;
                case 0x25:
                    return TzxDataBlockType.LoopEnd;
                case 0x26:
                    return TzxDataBlockType.CallSequence;
                case 0x27:
                    return TzxDataBlockType.ReturnFromSequence;
                case 0x28:
                    return TzxDataBlockType.SelectBlock;
                case 0x2A:
                    return TzxDataBlockType.StopTapeIn48kMode;
                case 0x2B:
                    return TzxDataBlockType.SetSignalLevel;
                case 0x30:
                    return TzxDataBlockType.TextDescription;
                case 0x31:
                    return TzxDataBlockType.Message;
                case 0x32:
                    return TzxDataBlockType.ArchiveInfo;
                case 0x33:
                    return TzxDataBlockType.HardwareType;
                case 0x34:
                    return TzxDataBlockType.EmulationInfo;
                case 0x35:
                    return TzxDataBlockType.CustomInfo;
                case 0x40:
                    return TzxDataBlockType.SCREENSBlock;
                //case 0x5A:
                default:
                    return TzxDataBlockType.SkipBlock;
            }
        }

        public override string ToString()
        {
            return "ID " + ID.ToString("X") + " - " + Type.ToString() + " : ";
        }
    }

    /// <summary>
    /// ID 10 - Standard Speed Data Block
    /// The standard speed data block has to be played with the standard Spectrum ROM timing values - see the default values in block ID 11.
    /// The pilot tone consists in 8063 pulses if the first data byte (flag byte) is < 128, 3223 otherwise. 
    /// This block can be used for the ROM loading routines AND for custom loading routines that use the same timings as ROM ones do.
    /// </summary>
    public class StandardSpeedDataBlock : TzxDataBlock
    {
        public StandardSpeedDataBlock()
        {
            ID = 0x10;
        }

        /// <summary>
        ///  Pause after this block (ms.)  
        /// (default value = 1000 ms) 
        /// </summary>
        public ushort PauseInMs { get; set; }

        /// <summary>
        /// Data as in .TAP files
        /// WARNING
        /// - StandardSpeedDataBlock : the length of data that follow is encoded in 2 bytes
        /// - TurboSpeedDataBlock : the length of data that follow is encoded in _3_ bytes !
        /// </summary>
        public TapDataBlock TapeDataBlock { get; set; }

        public override string ToString()
        {
            return base.ToString() + "TapeDataBlock={ " + TapeDataBlock.ToString() + " }, PauseInMs=" + PauseInMs;
        }
    }

    /// <summary>
    /// ID 14 - Pure Data Block
    /// This is the same as in the turbo loading data block, except that it has no pilot or sync pulses.
    /// </summary>
    public class PureDataBlock : StandardSpeedDataBlock
    {
        public PureDataBlock()
        {
            ID = 0x14;
        }

        /// <summary>
        /// 0-bit pulse length
        /// (default value = 855)  
        /// </summary>
        public ushort ZeroBitPulseLength { get; set; }

        /// <summary>
        /// 1-bit pulse length
        /// (default value = 1710)
        /// </summary>
        public ushort OneBitPulseLength { get; set; }

        /// <summary>
        /// used bits of the last byte 
        /// 1..8
        /// Used bits, where the most significant bit is the leftmost bit; 
        /// e.g. 6 means ++++++00 (+ means a used bit, 0 means unused; default value = 8) 
        /// </summary>
        public byte UsedBitsOfTheLastByte { get; set; }

        public override string ToString()
        {
            return base.ToString() + ", ZeroBitPulseLength=" + ZeroBitPulseLength + ", OneBitPulseLength=" + OneBitPulseLength + ", UsedBitsOfTheLastByte=" + UsedBitsOfTheLastByte;
        }
    }

    /// <summary>
    /// ID 11 - Turbo Speed Data Block
    /// The turbo speed data block allows different timings for special loaders.
    /// This block is very similar to the normal TAP block but with some additional info on the timings and other important differences.
    /// The same tape encoding is used as for the standard speed data block. 
    /// If a block should use some non-standard sync or pilot tones (i.e. all sorts of protection schemes) then use the next three blocks to describe it. 
    /// </summary>
    public class TurboSpeedDataBlock : PureDataBlock
    {
        public TurboSpeedDataBlock()
        {
            ID = 0x11;
        }

        /// <summary>
        /// pilot tone pulse length 
        /// (default value = 2168) 
        /// </summary>
        public ushort PilotTonePulseLength { get; set; }

        /// <summary>
        /// 1st sync pulse length 
        /// (default value = 667) 
        /// </summary>
        public ushort FirstSyncPulseLength { get; set; }
 
        /// <summary>
        /// 2nd sync pulse length 
        /// (default value = 735) 
        /// </summary>
        public ushort SecondSyncPulseLength { get; set; }
 
        /// <summary>
        /// pilot tone pulse count 
        /// (default values = 8063 for headers,
        ///  3223 for data blocks) 
        /// </summary>
        public ushort PilotTonePulseCount { get; set; }

        public override string ToString()
        {
            return base.ToString() + ", PilotTonePulseLength=" + PilotTonePulseLength + ", FirstSyncPulseLength=" + FirstSyncPulseLength + ", SecondSyncPulseLength=" + SecondSyncPulseLength + ", PilotTonePulseCount=" + PilotTonePulseCount;
        }
    }

    /// <summary>
    /// ID 12 - Pure Tone
    /// This will produce a tone which is basically the same as the pilot tone in the ID 10, ID 11 blocks. 
    /// You can define how long the pulse is and how many pulses are in the tone
    /// </summary>
    public class PureTone : TzxDataBlock
    {
        public PureTone()
        {
            ID = 0x12;
        }

        /// <summary>
        /// Length of one pulse in T-states 
        /// </summary>
        public ushort LengthOfOnePulse { get; set; }

        /// <summary>
        /// Number of pulses for the tone
        /// </summary>
        public ushort PulseCount { get; set; }

        public override string ToString()
        {
            return base.ToString() + "LengthOfOnePulse=" + LengthOfOnePulse + ", PulseCount=" + PulseCount;
        }
    }

    /// <summary>
    /// ID 13 - Pulse sequence
    /// This will produce N pulses, each having its own timing. 
    /// Up to 255 pulses can be stored in this block; 
    /// this is useful for non-standard sync tones used by some protection schemes.
    /// </summary>
    public class PulseSequence : TzxDataBlock
    {
        public PulseSequence()
        {
            ID = 0x13;
        }

        /// <summary>
        /// Number of pulses
        /// </summary>
        public byte PulseCount { get; set; }

        /// <summary>
        ///Pulses' lengths
        /// </summary>
        public ushort[] PulsesLengths { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(base.ToString() + "PulseCount=" + PulseCount + ", PulsesLengths={");
            if (PulsesLengths.Length > 4)
            {
                sb.Append(PulsesLengths[0] + ", ");
                sb.Append(PulsesLengths[1] + " ... ");
                sb.Append(PulsesLengths[PulsesLengths.Length - 2] + ", ");
                sb.Append(PulsesLengths[PulsesLengths.Length - 1]);
            }
            else
            {
                foreach (ushort n in PulsesLengths)
                {
                    sb.Append(n + " ");
                }
            }
            sb.Append("}");
            return sb.ToString();
        }
    }

    /// <summary>
    /// ID 15 - Direct Recording
    /// This block is used for tapes which have some parts in a format such that the turbo loader block cannot be used. 
    /// This is not like a VOC file, since the information is much more compact. 
    /// Each sample value is represented by one bit only (0 for low, 1 for high) which means that the block will be at most 1/8 the size of the equivalent VOC.
    /// The preferred sampling frequencies are 22050 or 44100 Hz (158 or 79 T-states/sample). 
    /// Please, if you can, don't use other sampling frequencies.
    /// Please use this block only if you cannot use any other block.
    /// </summary>
    public class DirectRecording : TzxDataBlock
    {
        public DirectRecording()
        {
            ID = 0x15;
        }

        /// <summary>
        /// Number of T-States per sample. 
        /// One T-State is 1 / 3,500,000 s.
        /// 158 means 22050 Hz
        /// 79 means 44100 Hz
        /// other values are not recommended!  
        /// </summary>
        public ushort TStatesPerSample { get; set; }

        /// <summary>
        /// Pause after this block (ms.)
        /// </summary>
        public ushort PauseInMs { get; set; }

        /// <summary>
        /// used bits of the last byte 
        /// 1..8
        /// Used bits, where the most significant bit is the leftmost bit; 
        /// e.g. 6 means ++++++00 (+ means a used bit, 0 means unused; default value = 8) 
        /// </summary>
        public byte UsedBitsOfTheLastByte { get; set; }

        /// <summary>
        /// Length of the samples data (BitStream)
        /// WARNING : number coded in 3 bytes
        /// </summary>
        public int DataLength { get; set; }

        /// <summary>
        /// The bits that represent the samples to be played
        /// Each bit represents a state on the EAR port (i.e. one sample).
        /// 0 means low, 1 means high signal of the EAR port. 
        /// The most significant bit is played first.
        /// </summary>
        public byte[] BitStream { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(base.ToString());
            sb.Append("TStatesPerSample=" + TStatesPerSample);
            sb.Append(", PauseInMs=" + PauseInMs);
            sb.Append(", UsedBitsOfTheLastByte=" + UsedBitsOfTheLastByte);
            sb.Append(", DataLength=" + DataLength);
            sb.Append(", BitStream=| ");
            if (BitStream.Length > 4)
            {
                sb.Append(BitStream[0].ToString("X") + " ");
                sb.Append(BitStream[1].ToString("X") + " ... ");
                sb.Append(BitStream[BitStream.Length - 2].ToString("X") + " ");
                sb.Append(BitStream[BitStream.Length - 1].ToString("X"));
            }
            else
            {
                foreach (byte b in BitStream)
                {
                    sb.Append(b.ToString("X") + " ");
                }
            }
            sb.Append("|");
            return sb.ToString();
        }
    }

    /// <summary>
    /// ID 20 - Pause (silence) or 'Stop the Tape' command
    /// This will make a silence (low amplitude level (0)) for a given time in milliseconds. 
    /// If the value is 0 then the emulator or utility should (in effect) STOP THE TAPE, i.e. should not continue loading until the user or emulator requests it.
    /// </summary>
    public class PauseOrStopTheTape : TzxDataBlock
    {
        public PauseOrStopTheTape()
        {
            ID = 0x20;
        }

        /// <summary>
        /// Pause duration (ms.)
        /// </summary>
        public ushort PauseInMs { get; set; }

        public override string ToString()
        {
            return base.ToString() + "PauseInMs=" + PauseInMs;
        }
    }

    /// <summary>
    /// ID 21 - Group start
    /// This block marks the start of a group of blocks which are to be treated as one single (composite) block. 
    /// This is very handy for tapes that use lots of subblocks like Bleepload (which may well have over 160 custom loading blocks). 
    /// You can also give the group a name (example 'Bleepload Block 1').
    /// For each group start block, there must be a group end block. 
    /// Nesting of groups is not allowed.
    /// </summary>
    public class GroupStart : TzxDataBlock
    {
        public GroupStart()
        {
            ID = 0x21;
        }

        /// <summary>
        /// Length of the group name string
        /// </summary>
        public byte GroupNameLength { get; set; }

        /// <summary>
        /// Group name in ASCII format (please keep it under 30 characters long)
        /// </summary>
        public string GroupName { get; set; }

        public override string ToString()
        {
            return base.ToString() + "GroupName=" + GroupName;
        }
    }

    /// <summary>
    /// ID 22 - Group end
    /// This indicates the end of a group. This block has no body.
    /// </summary>
    public class GroupEnd : TzxDataBlock
    {
        public GroupEnd()
        {
            ID = 0x22;
        }
    }

    /// <summary>
    /// ID 30 - Text description
    /// This is meant to identify parts of the tape, so you know where level 1 starts, where to rewind to when the game ends, etc. 
    /// This description is not guaranteed to be shown while the tape is playing, but can be read while browsing the tape or changing the tape pointer.
    /// The description can be up to 255 characters long but please keep it down to about 30 so the programs can show it in one line (where this is appropriate).
    /// Please use 'Archive Info' block for title, authors, publisher, etc.
    /// </summary>
    public class TextDescription : TzxDataBlock
    {
        public TextDescription()
        {
            ID = 0x30;
        }

        /// <summary>
        /// Length of the text description
        /// </summary>
        public byte Length { get; set; }

        /// <summary>
        /// Text description in ASCII format
        /// </summary>
        public string Text { get; set; }

        public override string ToString()
        {
            return base.ToString() + "Length=" + Length + ", Text=" + Text;
        }
    }

    /// <summary>
    /// ID 32 - Archive info
    /// Use this block at the beginning of the tape to identify the title of the game, author, publisher, year of publication, 
    /// price (including the currency), type of software (arcade adventure, puzzle, word processor, ...), protection scheme it uses 
    /// (Speedlock 1, Alkatraz, ...) and its origin (Original, Budget re-release, ...), etc. 
    /// This block is built in a way that allows easy future expansion. 
    /// The block consists of a series of text strings. 
    /// Each text has its identification number (which tells us what the text means) and then the ASCII text. 
    /// To make it possible to skip this block, if needed, the length of the whole block is at the beginning of it.
    /// If all texts on the tape are in English language then you don't have to supply the 'Language' field.
    /// The information about what hardware the tape uses is in the 'Hardware Type' block, so no need for it here.
    /// </summary>
    public class ArchiveInfo : TzxDataBlock
    {
        public ArchiveInfo()
        {
            ID = 0x32;
        }

        /// <summary>
        /// Length of the whole block (without these two bytes) 
        /// </summary>
        public ushort DataLength { get; set; }

        /// <summary>
        /// Number of text strings
        /// </summary>
        public byte TextStringsCount { get; set; }

        /// <summary>
        /// List of text strings 
        /// </summary>
        public ArchiveInfoText[] TextStrings { get; set; }

        public class ArchiveInfoText
        {
            /// <summary>
            /// Text identification byte:
            /// 00 - Full title
            /// 01 - Software house/publisher
            /// 02 - Author(s)
            /// 03 - Year of publication
            /// 04 - Language
            /// 05 - Game/utility type
            /// 06 - Price
            /// 07 - Protection scheme/loader
            /// 08 - Origin
            /// FF - Comment(s)
            /// </summary>
            public byte LabelID { get; set; }

            public string Label
            {
                get 
                {
                    switch(LabelID)
                    {
                        case 0x00:
                            return "Full title";
                        case 0x01:
                            return "Software house/publisher";
                        case 0x02:
                            return "Author(s)";
                        case 0x03:
                            return "Year of publication";
                        case 0x04:
                            return "Language";
                        case 0x05:
                            return "Game/utility type";
                        case 0x06:
                            return "Price";
                        case 0x07:
                            return "Protection scheme/loader";
                        case 0x08:
                            return "Origin";
                        case 0xFF: 
                            return "Comment(s)";
                        default:
                            return "?";
                    }
                }
            }

            /// <summary>
            /// Length of text string 
            /// </summary>
            public byte Length { get; set; }

            /// <summary>
            /// Text string in ASCII format 
            /// </summary>
            public string Text { get; set; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(base.ToString());
            foreach (ArchiveInfoText text in TextStrings)
            {
                sb.Append(text.Label + "=\"" + text.Text + "\" ");
            }
            return sb.ToString();
        }
    }


}
