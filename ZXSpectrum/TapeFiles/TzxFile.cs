using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZXSpectrum.TapeFiles
{
    /// <summary>
    /// http://www.worldofspectrum.org/TZXformat.html
    /// 
    ///  TZX is a file format designed to preserve all (hopefully) of the tapes with turbo or custom loading routines. 
    ///  The format was first started off by Tomaz Kac who was maintainer until revision 1.13, and then passed to Martijn v.d. Heide. 
    ///  After that, Ramsoft were the maintainers for a brief period during which the v1.20 revision was put together.
    ///
    ///  Implemented following TZX format revision 1.20.
    /// </summary>
    public class TzxFile : Tape
    {
        public TzxFile(string fileName) : base(fileName)
        {
            FileName = fileName;
        }

        /// <summary>
        /// The file is identified with the first 8 bytes being 'ZXTape!'
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// The 'end of file' byte is 26 (1A hex)
        /// </summary>
        public byte EndOfTextFileMarker { get; set; } 

        /// <summary>
        /// Major version number.
        /// To be able to use a TZX file, your program (emulator, utility, or whatever) must be able to handle files of at least its major version number. 
        /// </summary>
        public byte MajorRevisionNumber { get; set; }

        /// <summary>
        /// Minor version number.
        /// If your program can handle (say) version 1.05 and you encounter a file with version number 1.06, your program must be able to handle it, even if it cannot handle all the data in the file.
        /// </summary>
        public byte MinorRevisionNumber { get; set; }

        /// <summary>
        /// Then the main body of the file consists of a mixture of blocks, each identified by an ID byte.
        /// </summary>
        public IList<TzxDataBlock> DataBlocks { get { return _dataBlocks; } }
        private IList<TzxDataBlock> _dataBlocks = new List<TzxDataBlock>();
        
        public override string ToString()
        {
            StringBuilder sbResult = new StringBuilder("[TZX file v"+MajorRevisionNumber+"."+MinorRevisionNumber+" : " + FileName + "]");
            sbResult.AppendLine();
            foreach (TzxDataBlock dataBlock in DataBlocks)
            {
                sbResult.AppendLine(" + " + dataBlock.ToString());
            }
            return sbResult.ToString();
        }
    }
}
