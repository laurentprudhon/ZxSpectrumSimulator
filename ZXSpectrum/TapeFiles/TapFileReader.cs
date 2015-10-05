using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZXSpectrum.TapeFiles
{
    public class TapFileReader
    {
        public static TapFile ReadTapFile(Stream inputStream, string fileName)
        {
            TapFile resultFile = new TapFile(fileName);

            using (BinaryReader fileReader = new BinaryReader(inputStream, Encoding.GetEncoding("ISO-8859-1")))
            {
                while (fileReader.PeekChar() >= 0)
                {
                    TapDataBlock dataBlock = ReadTapDataBlock(fileReader);
                    resultFile.DataBlocks.Add(dataBlock);

                    TapeSection section = new TapeSection(dataBlock);
                    resultFile.Sections.Add(section);
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
            resultFile.Description = ""; // Tap file do not contain metadata

            return resultFile;
        }        

        internal static TapDataBlock ReadTapDataBlock(BinaryReader fileReader)
        {
            return ReadTapDataBlock(fileReader, 2);
        }

        internal static TapDataBlock ReadTapDataBlock(BinaryReader fileReader, int dataLengthBytesCount)
        {
            // Data block length
            int dataLength = fileReader.ReadUInt16();
            if (dataLengthBytesCount == 3)
            {
                dataLength += (fileReader.ReadByte() * 256 * 256);
            }

            // Read tape data block
            byte[] tapeData = null;
            if (dataLength > 0)
            {
                tapeData = fileReader.ReadBytes(dataLength);
            }

            TapDataBlock tapDataBlock = null;
            if (dataLength > 0)
            {
                byte flag = tapeData[0];

                // Standard header
                if (flag == 0 && dataLength == 19) 
                {
                    using (BinaryReader headerReader = new BinaryReader(new MemoryStream(tapeData)))
                    {
                        headerReader.ReadByte(); // flag

                        byte dataType = headerReader.ReadByte();
                        TapHeaderType headerType = TapHeader.GetTapHeaderTypeFromDataType(dataType);
                        switch (headerType)
                        {
                            case TapHeaderType.ProgramHeader:
                                tapDataBlock = new ProgramHeader();
                                break;
                            case TapHeaderType.NumberArrayHeader:
                            case TapHeaderType.StringArrayHeader:
                                tapDataBlock = new AlphanumArrayHeader(dataType);
                                break;
                            default:
                                //case TapHeaderType.ByteArrayHeader:
                                tapDataBlock = new ByteArrayHeader();
                                break;
                        }

                        TapHeader tapHeader = (TapHeader)tapDataBlock;
                        tapHeader.FileName = Encoding.GetEncoding("ISO-8859-1").GetString(headerReader.ReadBytes(10));
                        tapHeader.FollowingDataLength = headerReader.ReadUInt16();

                        switch (headerType)
                        {
                            case TapHeaderType.ProgramHeader:
                                ReadProgramHeader(headerReader, (ProgramHeader)tapDataBlock);
                                break;
                            case TapHeaderType.NumberArrayHeader:
                            case TapHeaderType.StringArrayHeader:
                                ReadAlphanumArrayHeader(headerReader, (AlphanumArrayHeader)tapDataBlock);
                                break;
                            default:
                                //case TapHeaderType.ByteArrayHeader:
                                ReadByteArrayHeader(headerReader, (ByteArrayHeader)tapDataBlock);
                                break;
                        }
                    }
                }
                // Anonymous data block
                else
                {
                    tapDataBlock = new TapDataBlock();
                    tapDataBlock.Flag = flag;
                }

                tapDataBlock.DataLength = dataLength;
                tapDataBlock.TapeData = tapeData;

                if (dataLength >= 2)
                {
                    tapDataBlock.CheckSum = tapeData[dataLength-1];
                    tapDataBlock.HasInvalidChecksum = !CheckIfChecksumIsValid(tapDataBlock);
                }
            }

            return tapDataBlock;
        }

        private static void ReadProgramHeader(BinaryReader fileReader, ProgramHeader programHeader)
        {            
            programHeader.AutostartLine = fileReader.ReadUInt16();
            programHeader.ProgramLength = fileReader.ReadUInt16();
        }

        private static void ReadAlphanumArrayHeader(BinaryReader fileReader, AlphanumArrayHeader alphanumArrayHeader)
        {
           fileReader.ReadByte(); // unused
            alphanumArrayHeader.VariableIndex = fileReader.ReadByte();
            fileReader.ReadUInt16(); // unused
        }

        private static void ReadByteArrayHeader(BinaryReader fileReader, ByteArrayHeader byteArrayHeader)
        {
            byteArrayHeader.StartAddress = fileReader.ReadUInt16();
            fileReader.ReadUInt16(); // unused
        }

        private static bool CheckIfChecksumIsValid(TapDataBlock tapDataBlock)
        {
            if(tapDataBlock.TapeData != null && tapDataBlock.TapeData.Length > 2)
            {
                byte computedChecksum = tapDataBlock.Flag;
                for (int i = 1; i < (tapDataBlock.TapeData.Length - 1); i++ )
                {
                    computedChecksum ^= tapDataBlock.TapeData[i];
                }
                return computedChecksum == tapDataBlock.CheckSum;
            }
            else
            {
                return false;
            }
        }
    }
}
