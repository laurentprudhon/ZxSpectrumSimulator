using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZXSpectrum.TapeFiles
{
    public class TzxFileReader
    {
        public static TzxFile ReadTzxFile(Stream inputStream, string fileName)
        {
            TzxFile resultFile = new TzxFile(fileName);

            using (BinaryReader fileReader = new BinaryReader(inputStream, Encoding.GetEncoding("ISO-8859-1")))
            {
                string signature = Encoding.GetEncoding("ISO-8859-1").GetString(fileReader.ReadBytes(7));
                if (signature != "ZXTape!")
                {
                    throw new InvalidDataException("The signature of the file does not match the standard TZX file format");
                }
                else
                {
                    resultFile.Signature = signature;
                }

                resultFile.EndOfTextFileMarker = fileReader.ReadByte();
                resultFile.MajorRevisionNumber = fileReader.ReadByte();
                resultFile.MinorRevisionNumber = fileReader.ReadByte();

                TapeSection currentGroupSection = null;

                while (fileReader.PeekChar() >= 0)
                {
                    TzxDataBlock tzxBlock = ReadTzxDataBlock(fileReader);
                    resultFile.DataBlocks.Add(tzxBlock);
                    
                    switch(tzxBlock.Type)
                    {
                        case TzxDataBlockType.StandardSpeedDataBlock:
                        case TzxDataBlockType.TurboSpeedDataBlock:
                        case TzxDataBlockType.PureTone:
                        case TzxDataBlockType.PulseSequence:
                        case TzxDataBlockType.PureDataBlock:
                        case TzxDataBlockType.DirectRecording:
                            TapeSection section = new TapeSection(tzxBlock);
                            if (currentGroupSection == null)
                            {
                                resultFile.Sections.Add(section);
                            }
                            else
                            {
                                foreach (TapeSoundSequence soundSequence in section.SoundSequences)
                                {
                                    currentGroupSection.SoundSequences.Add(soundSequence);
                                }
                            }
                            break;
                        case TzxDataBlockType.PauseOrStopTheTape:
                            PauseOrStopTheTape pauseBlock = (PauseOrStopTheTape)tzxBlock;
                            if (pauseBlock.PauseInMs > 0)
                            {
                                section = new TapeSection(tzxBlock);
                                if (currentGroupSection == null)
                                {
                                    resultFile.Sections.Add(section);
                                }
                                else
                                {
                                    foreach (TapeSoundSequence soundSequence in section.SoundSequences)
                                    {
                                        currentGroupSection.SoundSequences.Add(soundSequence);
                                    }
                                }
                            }
                            break;
                        case TzxDataBlockType.TextDescription:
                            TextDescription descriptionBlock = (TextDescription)tzxBlock;
                            resultFile.Description += descriptionBlock.Text;
                            break;
                        case TzxDataBlockType.ArchiveInfo:
                            ArchiveInfo archiveInfo = (ArchiveInfo)tzxBlock;
                            resultFile.Description += archiveInfo.ToString();
                            break;
                        case TzxDataBlockType.GroupStart:
                            currentGroupSection = new TapeSection(tzxBlock);
                            break;
                        case TzxDataBlockType.GroupEnd:
                            foreach (TapeSoundSequence soundSequence in currentGroupSection.SoundSequences)
                            {
                                currentGroupSection.Duration += soundSequence.Duration;
                            }
                            resultFile.Sections.Add(currentGroupSection);
                            currentGroupSection = null;
                            break;
                    }
                }
            }
            TapeSection lastSection = resultFile.Sections[resultFile.Sections.Count-1];
            TapeSoundSequence lastSoundSequence = lastSection.SoundSequences[lastSection.SoundSequences.Count - 1];
            if(lastSoundSequence.GetType() != typeof(PauseSequence))
            {
                lastSection.SoundSequences.Add(new PauseSequence("End of tape", 1));
            }

            foreach (TapeSection section in resultFile.Sections)
            {
                resultFile.Duration += section.Duration;
            }

            return resultFile;
        }

        private static TzxDataBlock ReadTzxDataBlock(BinaryReader fileReader)
        {
            TzxDataBlock resultDataBlock = null;

            byte id = fileReader.ReadByte();

            TzxDataBlockType dataBlockType = TzxDataBlock.GetTZXDataBlockTypeFromID(id);
            switch (dataBlockType)
            {
                // ID 10 - Standard Speed Data Block
                case TzxDataBlockType.StandardSpeedDataBlock:
                    resultDataBlock = new StandardSpeedDataBlock();
                    ReadStandardSpeedDataBlock(fileReader, (StandardSpeedDataBlock)resultDataBlock);
                    break;
                // ID 11 - Turbo Speed Data Block
                case TzxDataBlockType.TurboSpeedDataBlock:
                    resultDataBlock = new TurboSpeedDataBlock();
                    ReadTurboSpeedDataBlock(fileReader, (TurboSpeedDataBlock)resultDataBlock);
                    break;
                // ID 12 - Pure Tone
                case TzxDataBlockType.PureTone:
                    resultDataBlock = new PureTone();
                    ReadPureTone(fileReader, (PureTone)resultDataBlock);
                    break;
                // ID 13 - Pulse sequence
                case TzxDataBlockType.PulseSequence:
                    resultDataBlock = new PulseSequence();
                    ReadPulseSequence(fileReader, (PulseSequence)resultDataBlock);
                    break;
                // ID 14 - Pure Data Block
                case TzxDataBlockType.PureDataBlock:
                    resultDataBlock = new PureDataBlock();
                    ReadPureDataBlock(fileReader, (PureDataBlock)resultDataBlock);
                    break;
                // ID 15 - Direct Recording
                case TzxDataBlockType.DirectRecording:
                    resultDataBlock = new DirectRecording();
                    ReadDirectRecording(fileReader, (DirectRecording)resultDataBlock);
                    break;
                // ID 20 - Pause (silence) or 'Stop the Tape' command
                case TzxDataBlockType.PauseOrStopTheTape:
                    resultDataBlock = new PauseOrStopTheTape();
                    ReadPauseOrStopTheTape(fileReader, (PauseOrStopTheTape)resultDataBlock);
                    break;
                // ID 21 - Group start
                case TzxDataBlockType.GroupStart:
                    resultDataBlock = new GroupStart();
                    ReadGroupStart(fileReader, (GroupStart)resultDataBlock);
                    break;
                // ID 22 - Group end
                case TzxDataBlockType.GroupEnd:
                    resultDataBlock = new GroupEnd();
                    break;
                // ID 30 - Text description
                case TzxDataBlockType.TextDescription:
                    resultDataBlock = new TextDescription();
                    ReadTextDescription(fileReader, (TextDescription)resultDataBlock);
                    break;
                // ID 32 - Archive info
                case TzxDataBlockType.ArchiveInfo:
                    resultDataBlock = new ArchiveInfo();
                    ReadArchiveInfo(fileReader, (ArchiveInfo)resultDataBlock);
                    break;
                default:
                    throw new NotImplementedException("TZX data block ID "+ id.ToString("X") + " is not yet implemented");
            }

            return resultDataBlock;
        }
            
        private static void ReadStandardSpeedDataBlock(BinaryReader fileReader, StandardSpeedDataBlock standardSpeedDataBlock)
        {
            standardSpeedDataBlock.PauseInMs = fileReader.ReadUInt16();
            standardSpeedDataBlock.TapeDataBlock = TapFileReader.ReadTapDataBlock(fileReader);
        }

        private static void ReadTurboSpeedDataBlock(BinaryReader fileReader, TurboSpeedDataBlock turboSpeedDataBlock)
        {
            turboSpeedDataBlock.PilotTonePulseLength = fileReader.ReadUInt16();
            turboSpeedDataBlock.FirstSyncPulseLength = fileReader.ReadUInt16();
            turboSpeedDataBlock.SecondSyncPulseLength = fileReader.ReadUInt16();
            turboSpeedDataBlock.ZeroBitPulseLength = fileReader.ReadUInt16();
            turboSpeedDataBlock.OneBitPulseLength = fileReader.ReadUInt16();
            turboSpeedDataBlock.PilotTonePulseCount = fileReader.ReadUInt16();
            turboSpeedDataBlock.UsedBitsOfTheLastByte = fileReader.ReadByte();
            turboSpeedDataBlock.PauseInMs = fileReader.ReadUInt16();
            turboSpeedDataBlock.TapeDataBlock = TapFileReader.ReadTapDataBlock(fileReader, 3);
        }

        private static void ReadPureTone(BinaryReader fileReader, PureTone pureTone)
        {
            pureTone.LengthOfOnePulse = fileReader.ReadUInt16();
            pureTone.PulseCount = fileReader.ReadUInt16();
        }

        private static void ReadPulseSequence(BinaryReader fileReader, PulseSequence pulseSequence)
        {
            pulseSequence.PulseCount = fileReader.ReadByte();
            pulseSequence.PulsesLengths = new ushort[pulseSequence.PulseCount];
            for (int i = 0; i < pulseSequence.PulseCount; i++)
            {
                pulseSequence.PulsesLengths[i] = fileReader.ReadUInt16();
            }
        }

        private static void ReadPureDataBlock(BinaryReader fileReader, PureDataBlock pureDataBlock)
        {
            pureDataBlock.ZeroBitPulseLength = fileReader.ReadUInt16();
            pureDataBlock.OneBitPulseLength = fileReader.ReadUInt16();
            pureDataBlock.UsedBitsOfTheLastByte = fileReader.ReadByte();
            pureDataBlock.PauseInMs = fileReader.ReadUInt16();
            pureDataBlock.TapeDataBlock = TapFileReader.ReadTapDataBlock(fileReader, 3);
        }

        private static void ReadDirectRecording(BinaryReader fileReader, DirectRecording directRecording)
        {
            directRecording.TStatesPerSample = fileReader.ReadUInt16();
            directRecording.PauseInMs = fileReader.ReadUInt16();
            directRecording.UsedBitsOfTheLastByte = fileReader.ReadByte();
            directRecording.DataLength = fileReader.ReadUInt16() + (fileReader.ReadByte() * 256 * 256);
            directRecording.BitStream = fileReader.ReadBytes(directRecording.DataLength);
        }

        private static void ReadPauseOrStopTheTape(BinaryReader fileReader, PauseOrStopTheTape pauseOrStopTheTape)
        {
            pauseOrStopTheTape.PauseInMs = fileReader.ReadUInt16();
        }

        private static void ReadGroupStart(BinaryReader fileReader, GroupStart groupStart)
        {
            groupStart.GroupNameLength = fileReader.ReadByte();
            groupStart.GroupName = Encoding.GetEncoding("ISO-8859-1").GetString(fileReader.ReadBytes(groupStart.GroupNameLength));
        }          
        
        private static void ReadTextDescription(BinaryReader fileReader, TextDescription textDescription)
        {
            textDescription.Length = fileReader.ReadByte();
            textDescription.Text = Encoding.GetEncoding("ISO-8859-1").GetString(fileReader.ReadBytes(textDescription.Length));
        }

        private static void ReadArchiveInfo(BinaryReader fileReader, ArchiveInfo archiveInfo)
        {
            archiveInfo.DataLength = fileReader.ReadUInt16();
            archiveInfo.TextStringsCount = fileReader.ReadByte();
            archiveInfo.TextStrings = new ArchiveInfo.ArchiveInfoText[archiveInfo.TextStringsCount];
            for (int i = 0; i < archiveInfo.TextStringsCount; i++)
            {
                ArchiveInfo.ArchiveInfoText text = new ArchiveInfo.ArchiveInfoText();
                text.LabelID = fileReader.ReadByte();
                text.Length = fileReader.ReadByte();
                text.Text = Encoding.GetEncoding("ISO-8859-1").GetString(fileReader.ReadBytes(text.Length));
                archiveInfo.TextStrings[i] = text;
            }
        }
    }
}
