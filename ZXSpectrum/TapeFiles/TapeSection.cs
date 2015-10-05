using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZXSpectrum.TapeFiles
{
    public class TapeSection
    {
        /// <summary>
        /// Label describing the tape section
        /// </summary>
        public string Label { get; protected set; }

        /// <summary>
        /// Duration of this tape section in ms
        /// </summary>
        public int Duration { get; internal set; }

        /// <summary>
        /// Each tape section groups several sound sequences
        /// </summary>
        public IList<TapeSoundSequence> SoundSequences { get { return _soundSequences; } }
        private IList<TapeSoundSequence> _soundSequences = new List<TapeSoundSequence>();

        public TapeSection(string label, params TapeSoundSequence[] soundSequences)
        {
            Label = label;
            foreach (TapeSoundSequence soundSequence in soundSequences)
            {
                SoundSequences.Add(soundSequence);
            }
            ComputeDuration();
        }

        public TapeSection(TzxDataBlock tzxBlock)
        {
            switch (tzxBlock.Type)
            {
                // ID 10 - Standard Speed Data Block 
                // PILOT tone : 
                // - number of pulses : 8063 header (flag<128), 3223 data (flag>=128)
                // - pulse length : 2168
                // First SYNC pulse :
                // - pulse length : 667
                // Second SYNC pulse :
                // - pulse length : 735
                // DATA : 
                // - length of ZERO bit pulse : 855
                // - length of ONE bit pulse : 1710 
                // Pause after this block (ms.) {1000}
                case TzxDataBlockType.StandardSpeedDataBlock:
                    StandardSpeedDataBlock standardDataBlock = (StandardSpeedDataBlock)tzxBlock;
                    // PILOT tone
                    if (standardDataBlock.TapeDataBlock.Flag < 128)
                    {
                        _soundSequences.Add(new ToneSequence("Pilot tone", 8063, 2168));
                    }
                    else
                    {
                        _soundSequences.Add(new ToneSequence("Pilot tone", 3223, 2168));
                    }
                    // First and Second SYNC pulse
                    _soundSequences.Add(new PulsesSequence("Sync pulses", new ushort[] { 667, 735 }));
                    // DATA
                    _soundSequences.Add(new DataSequence("Data", standardDataBlock.TapeDataBlock.TapeData, 855, 1710, 8));
                    // Pause
                    if (standardDataBlock.PauseInMs > 0)
                    {
                        _soundSequences.Add(new PauseSequence("Pause", standardDataBlock.PauseInMs));
                    }
                    break;
                // ID 11 - Turbo Speed Data Block 
                // PILOT tone : 
                // - number of pulses
                // - pulse length
                // First SYNC pulse :
                // - pulse length
                // Second SYNC pulse :
                // - pulse length
                // DATA : 
                // - length of ZERO bit pulse
                // - length of ONE bit pulse
                // Pause after this block (ms.)
                case TzxDataBlockType.TurboSpeedDataBlock:
                    TurboSpeedDataBlock turboDataBlock = (TurboSpeedDataBlock)tzxBlock;
                    // PILOT tone
                    _soundSequences.Add(new ToneSequence("Pilot tone", turboDataBlock.PilotTonePulseCount, turboDataBlock.PilotTonePulseLength));
                    // First and Second SYNC pulse
                    _soundSequences.Add(new PulsesSequence("Sync pulses", new ushort[] { turboDataBlock.FirstSyncPulseLength, turboDataBlock.SecondSyncPulseLength }));
                    // DATA
                    _soundSequences.Add(new DataSequence("Data", turboDataBlock.TapeDataBlock.TapeData, turboDataBlock.ZeroBitPulseLength, turboDataBlock.OneBitPulseLength, turboDataBlock.UsedBitsOfTheLastByte));
                    // Pause
                    if (turboDataBlock.PauseInMs > 0)
                    {
                        _soundSequences.Add(new PauseSequence("Pause", turboDataBlock.PauseInMs));
                    }
                    break;
                // ID 12 - Pure Tone
                // Length of one pulse in T-states
                // Number of pulses
                case TzxDataBlockType.PureTone:
                    PureTone toneBlock = (PureTone)tzxBlock;
                    _soundSequences.Add(new ToneSequence("Pure tone", toneBlock.PulseCount, toneBlock.LengthOfOnePulse));
                    break;
                // ID 13 - Pulse sequence
                // Number of pulses
                // Table of pulses' lengths
                case TzxDataBlockType.PulseSequence:
                    PulseSequence pulseBlock = (PulseSequence)tzxBlock;
                    _soundSequences.Add(new PulsesSequence("Pulse sequence", pulseBlock.PulsesLengths));
                    break;
                // ID 14 - Pure Data Block
                // DATA : Length of ZERO bit pulse / Length of ONE bit pulse
                // Pause after this block (ms.)
                case TzxDataBlockType.PureDataBlock:
                    PureDataBlock dataBlock = (PureDataBlock)tzxBlock;
                    // DATA
                    _soundSequences.Add(new DataSequence("Pure data", dataBlock.TapeDataBlock.TapeData, dataBlock.ZeroBitPulseLength, dataBlock.OneBitPulseLength, dataBlock.UsedBitsOfTheLastByte));
                    // Pause
                    if (dataBlock.PauseInMs > 0)
                    {
                        _soundSequences.Add(new PauseSequence("Pause", dataBlock.PauseInMs));
                    }
                    break;
                // ID 15 - Direct Recording
                // Number of T-states per sample (bit of data). When creating a 'Direct recording' block please stick to the standard sampling frequencies of 22050 or 44100 Hz. This will ensure correct playback when using PC's soundcards. 
                // Pause after this block in milliseconds (ms.)
                case TzxDataBlockType.DirectRecording:
                    DirectRecording samplesBlock = (DirectRecording)tzxBlock;
                    _soundSequences.Add(new SamplesSequence("Samples", samplesBlock.BitStream, samplesBlock.TStatesPerSample, samplesBlock.UsedBitsOfTheLastByte));
                    if (samplesBlock.PauseInMs > 0)
                    {
                        _soundSequences.Add(new PauseSequence("Pause", samplesBlock.PauseInMs));
                    }
                    break;
                // ID 20 - Pause (silence) or 'Stop the Tape' command
                // Pause duration (ms.)
                // This will make a silence (low amplitude level (0)) for a given time in milliseconds. If the value is 0 then the emulator or utility should (in effect) STOP THE TAPE, i.e. should not continue loading until the user or emulator requests it.
                case TzxDataBlockType.PauseOrStopTheTape:
                    PauseOrStopTheTape pauseBlock = (PauseOrStopTheTape)tzxBlock;
                    if (pauseBlock.PauseInMs > 0)
                    {
                        _soundSequences.Add(new PauseSequence("Pause", pauseBlock.PauseInMs));
                    }
                    break;
                case TzxDataBlockType.GroupStart:
                case TzxDataBlockType.GroupEnd:
                case TzxDataBlockType.TextDescription:
                case TzxDataBlockType.ArchiveInfo:
                    // Documentation only, ignore these blocks at runtime
                    break;
                default:
                    throw new NotSupportedException("Tzx file block type " + tzxBlock.Type.ToString() + " is not yet supported");
            }

            ComputeDuration();

            Label = GetSectionLabel(tzxBlock) + " [" + Math.Round(Duration / 1000f) + " sec]";
        }              

        private static string GetSectionLabel(TzxDataBlock tzxBlock)
        {
            string sectionLabel = null;
            switch (tzxBlock.Type)
            {
                case TzxDataBlockType.StandardSpeedDataBlock:
                    StandardSpeedDataBlock dataBlock = (StandardSpeedDataBlock)tzxBlock;
                    sectionLabel = GetSectionLabel(dataBlock.TapeDataBlock);
                    break;
                case TzxDataBlockType.TurboSpeedDataBlock:
                    dataBlock = (StandardSpeedDataBlock)tzxBlock;
                    sectionLabel = GetSectionLabel(dataBlock.TapeDataBlock);
                    sectionLabel += " [turbo speed]";
                    break;
                case TzxDataBlockType.PureDataBlock:
                    dataBlock = (StandardSpeedDataBlock)tzxBlock;
                    sectionLabel = GetSectionLabel(dataBlock.TapeDataBlock);
                    sectionLabel += " [no pilot/sync]";
                    break;
                case TzxDataBlockType.PureTone:
                    sectionLabel = "Pure tone";
                    break;
                case TzxDataBlockType.PulseSequence:
                    sectionLabel = "Pulse sequence";
                    break;
                case TzxDataBlockType.DirectRecording:
                    sectionLabel = "Direct recording";
                    break;
                case TzxDataBlockType.PauseOrStopTheTape:
                    sectionLabel = "Pause";
                    break;
                case TzxDataBlockType.GroupStart:
                    GroupStart groupStart = (GroupStart)tzxBlock;
                    sectionLabel = groupStart.GroupName;
                    break;
            }
            return sectionLabel;
        }

        public TapeSection(TapDataBlock tapBlock)
        {
            // PILOT tone : 
            // - number of pulses : 8063 header (flag<128), 3223 data (flag>=128)
            // - pulse length : 2168
            // First SYNC pulse :
            // - pulse length : 667
            // Second SYNC pulse :
            // - pulse length : 735
            // DATA : 
            // - length of ZERO bit pulse : 855
            // - length of ONE bit pulse : 1710 
            // The 'current pulse level' after playing the block is the opposite of the last pulse level played, so that a subsequent pulse will produce an edge. 
            
            if (tapBlock.Flag < 128)
            {
                _soundSequences.Add(new ToneSequence("Pilot tone", 8063, 2168));
            }
            else
            {
                _soundSequences.Add(new ToneSequence("Pilot tone", 3223, 2168));
            }
            // First and Second SYNC pulse
            _soundSequences.Add(new PulsesSequence("Sync pulses", new ushort[] { 667, 735 }));
            // DATA
            _soundSequences.Add(new DataSequence("Data", tapBlock.TapeData, 855, 1710, 8));

            ComputeDuration();

            Label = GetSectionLabel(tapBlock) + " [" + Math.Round(Duration / 1000f) + " sec]";
        }

        private static string GetSectionLabel(TapDataBlock dataBlock)
        {
            string sectionLabel = null;
            switch (dataBlock.Type)
            {
                case TapDataBlockType.StandardHeader:
                    TapHeader header = (TapHeader)dataBlock;
                    switch (header.HeaderType)
                    {
                        case TapHeaderType.ProgramHeader:
                            ProgramHeader programHeader = (ProgramHeader)header;
                            sectionLabel = "Program header : " + programHeader.GetProperties();
                            break;
                        case TapHeaderType.ByteArrayHeader:
                            ByteArrayHeader byteArrayHeader = (ByteArrayHeader)header;
                            sectionLabel = "Byte array header : " + byteArrayHeader.GetProperties();
                            break;
                        case TapHeaderType.StringArrayHeader:
                            AlphanumArrayHeader stringArrayHeader = (AlphanumArrayHeader)header;
                            sectionLabel = "String array header : " + stringArrayHeader.GetProperties();
                            break;
                        case TapHeaderType.NumberArrayHeader:
                            AlphanumArrayHeader numberArrayHeader = (AlphanumArrayHeader)header;
                            sectionLabel = "Number array header : " + numberArrayHeader.GetProperties();
                            break;
                    }
                    break;
                case TapDataBlockType.StandardDataBlock:
                    sectionLabel = "Standard data block : " + dataBlock.DataLength + " bytes";
                    break;
                case TapDataBlockType.CustomDataBlock:
                    sectionLabel = "Custom data block : " + dataBlock.DataLength + " bytes";
                    break;
                case TapDataBlockType.FragmentedDataBlock:
                    sectionLabel = "Fragmented data block : " + dataBlock.DataLength + " bytes";
                    break;
            }
            if (dataBlock.HasInvalidChecksum)
            {
                sectionLabel += " [invalid checksum]";
            }
            return sectionLabel;
        }

        private void ComputeDuration()
        {
            // Section duration
            Duration = 0;
            foreach (TapeSoundSequence soundSequence in _soundSequences)
            {
                Duration += soundSequence.Duration;
            }
        }
    }
}
