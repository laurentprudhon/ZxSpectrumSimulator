using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZXSpectrum.TapeFiles
{
    /// <summary>
    /// http://www.zxmodules.de/fileformats/tapformat.html
    /// 
    /// The TAP- (and BLK-) format is nearly a direct copy of the data that is stored 
    /// in real tapes, as it is 'written' by the ROM save routine of the ZX-Spectrum. 
    /// It is supported by many Spectrum emulators.
    /// </summary>
    public class TapFile : Tape
    {
        public TapFile(string fileName) : base(fileName)
        {
            FileName = fileName;
        }

        /// <summary>
        /// A TAP file is simply one datablock or a group of 2 or more datablocks, one followed after the other. 
        /// The TAP file may be empty. Then it has a size of 0 bytes. 
        /// There's no real file size limit, like real tapes, TAP files can also contain huge amounts of data blocks. 
        /// </summary>
        public IList<TapDataBlock> DataBlocks { get { return _dataBlocks; } }
        private IList<TapDataBlock> _dataBlocks = new List<TapDataBlock>();

        public override string ToString()
        {
            StringBuilder sbResult = new StringBuilder("[TAP file : " + FileName + "]");
            sbResult.AppendLine();
            foreach(TapDataBlock dataBlock in DataBlocks)
            {
                sbResult.AppendLine(" + " + dataBlock.ToString());
            }
            return sbResult.ToString();
        }
    }
}
