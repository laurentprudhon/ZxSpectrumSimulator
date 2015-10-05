using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZXSpectrum.TapeFiles
{
    public enum TapDataBlockType
    {
        StandardHeader,
        StandardDataBlock,
        CustomDataBlock,
        FragmentedDataBlock
    }
    
    /// <summary>
    /// a TAP file is simply one datablock or a group of 2 or more datablocks, one followed after the other. 
    /// The TAP file may be empty. Then it has a size of 0 bytes. 
    /// There's no real file size limit, like real tapes, TAP files can also contain huge amounts of data blocks. 
    /// </summary>
    public class TapDataBlock
    {
        /// <summary>
        /// length of the following datablock in bytes (0..65535) 
        /// </summary>
        public int DataLength { get; set; }

        /// <summary>
        /// data as it is stored on tape, may be a header or any data from ZX-Spectrum 
        /// (or may be empty) 
        /// </summary>
        public byte[] TapeData { get; set; }

        /// <summary>
        /// Fragmented data blocks cannot be produced by ROM saving routines - they must be produced with machine code programs. 
        /// They have less than 2 bytes. 
        /// In some games you really find zero length fragment data blocks
        /// </summary>
        public bool IsFragmented { get { return DataLength < 2; } }

        /// <summary>
        /// 0: Byte indicating a standard ROM loading header
        /// 255: indicating a standard ROM loading data block
        /// or any other value to build a custom data block 
        /// </summary>
        public byte Flag { get; set; }

        public TapDataBlockType Type
        {
            get
            {
                if (IsFragmented)
                {
                    return TapDataBlockType.FragmentedDataBlock;
                }
                else if(Flag == 0x00 && TapeData.Length == 19)
                {
                    return TapDataBlockType.StandardHeader;
                }
                else if (Flag == 0xFF)
                {
                    return TapDataBlockType.StandardDataBlock;
                }
                else 
                {
                    return TapDataBlockType.CustomDataBlock;
                }
            }
        }

        /// <summary>
        /// simply all bytes (including flag byte) XORed
        /// </summary>
        public byte CheckSum { get; set; }

        /// <summary>
        /// Broken standard data blocks may occur in Sinclair Basic, when saving a data block (program, bytes etc.) fails and produces a bad checksum byte. 
        /// Some games also use broken data blocks as a loading protection. 
        /// </summary>
        public bool HasInvalidChecksum { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(Type.ToString() + (HasInvalidChecksum?" (invalid checksum)":"") + " : ");
            if (Type != TapDataBlockType.StandardHeader)
            {
                int essentialDataLength = 0;
                if (TapeData != null)
                {
                    if (TapeData.Length >= 2)
                    {
                        essentialDataLength = TapeData.Length - 2;
                    }
                    else
                    {
                        essentialDataLength = TapeData.Length;
                    }
                }
                sb.Append("TapeDataLength=" + essentialDataLength);
                if (essentialDataLength > 0)
                {
                    sb.Append(", TapeData=|");
                    if (essentialDataLength > 4)
                    {
                        sb.Append(TapeData[1].ToString("X") + " ");
                        sb.Append(TapeData[2].ToString("X") + " ... ");
                        sb.Append(TapeData[TapeData.Length - 3].ToString("X") + " ");
                        sb.Append(TapeData[TapeData.Length - 2].ToString("X"));
                    }
                    else if (TapeData.Length >= 2)
                    {
                        foreach (byte b in TapeData.Skip(1).Take(essentialDataLength))
                        {
                            sb.Append(b.ToString("X") + " ");
                        }
                    }
                    else
                    {
                        foreach (byte b in TapeData)
                        {
                            sb.Append(b.ToString("X") + " ");
                        }
                    }
                    sb.Append("|");
                }
            }
            return sb.ToString();
        }
    }

    public enum TapHeaderType
    {
        ProgramHeader,
        NumberArrayHeader,
        StringArrayHeader,
        ByteArrayHeader
    }

    public abstract class TapHeader : TapDataBlock
    {
        /// <summary>
        /// 0: Byte indicating a program header
        /// 1: Byte indicating a numeric array 
        /// 2: Byte indicating an alphanumeric array 
        /// 3: Byte indicating a byte header
        /// </summary>
        public byte DataType { get; set; }

        public TapHeaderType HeaderType
        {
            get
            {
                return GetTapHeaderTypeFromDataType(DataType);
            }
        }

        public static TapHeaderType GetTapHeaderTypeFromDataType(byte dataType)
        {
            if (dataType == 0x00)
            {
                return TapeFiles.TapHeaderType.ProgramHeader;
            }
            else if (dataType == 0x01)
            {
                return TapeFiles.TapHeaderType.NumberArrayHeader;
            }
            else if (dataType == 0x02)
            {
                return TapeFiles.TapHeaderType.StringArrayHeader;
            }
            else if (dataType == 0x03)
            {
                return TapeFiles.TapHeaderType.ByteArrayHeader;
            }
            else
            {
                throw new ArgumentException("Data type "+ dataType +" is not valid in a standard header");
            }
        }

        /// <summary>
        /// loading name of the program. filled with spaces (CHR$(32))
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// program header : length of BASIC program + variables 
        /// numeric array : length of number array * 5 + 3
        /// alphanumeric array : length of string array + 3 
        /// byte header : length of the following data (after the header)
        /// </summary>
        public ushort FollowingDataLength { get; set; }

        public override string ToString()
        {
            return base.ToString() + "HeaderType=" + HeaderType.ToString() + ", FileName=" + FileName.Replace("\0","0").Trim() + ", FollowingDataLength=" + FollowingDataLength;
        }

        public virtual string GetProperties()
        {
            return "FileName=" + FileName.Replace("\0","0").Trim();
        }
    }

    public class ProgramHeader : TapHeader
    {
        public ProgramHeader()
        {
            Flag     = 0x00;
            DataType = 0x00;
        }

        /// <summary>
        /// LINE parameter of SAVE command. 
        /// Value 32768 means "no auto-loading"; 0..9999 are valid line numbers
        /// </summary>
        public ushort AutostartLine { get; set; }

        /// <summary>
        /// length of BASIC program; 
        /// remaining bytes ([data length] - [program length]) = offset of variables 
        /// </summary>
        public ushort ProgramLength { get; set; }

        public override string ToString()
        {
            return base.ToString() + ", AutoStartLine=" + AutostartLine + ", ProgramLength=" + ProgramLength;
        }

        public override string GetProperties()
        {
            return base.GetProperties() + ", ProgramLength=" + ProgramLength;
        }
    }

    public class AlphanumArrayHeader : TapHeader
    {
        public AlphanumArrayHeader(byte dataType)
        {
            Flag     = 0x00;
            DataType = dataType; // 0x01 or 0x02
        }

        /// <summary>
        /// numeric array : (1..26 meaning A..Z) + 128 
        /// alphanumeric array : (1..26 meaning A$..Z$) + 192
        /// </summary>
        public byte VariableIndex { get; set; }

        public string VariableName 
        {
            get
            {
                if(HeaderType == TapeFiles.TapHeaderType.NumberArrayHeader)
                {
                    return ((char)(VariableIndex - 128 + 64)).ToString();
                }
                else
                {
                    return (char)(VariableIndex - 192 + 64) + "$";
                }
            }
        }

        public override string ToString()
        {
            return base.ToString() + ", VariableName=" + VariableName;
        }

        public override string GetProperties()
        {
            return base.GetProperties() + ", VariableName=" + VariableName;
        }
    }

    public class ByteArrayHeader : TapHeader
    {
        public ByteArrayHeader()
        {
            Flag     = 0x00;
            DataType = 0x03;
        }

        /// <summary>
        ///  in case of a  SCREEN$ header = 16384  
        /// </summary>
        public ushort StartAddress { get; set; }

        public override string ToString()
        {
            return base.ToString() + ", StartAddress=" + StartAddress;
        }

        public override string GetProperties()
        {
            return base.GetProperties() + ", StartAddress=" + StartAddress;
        }
    }
}
