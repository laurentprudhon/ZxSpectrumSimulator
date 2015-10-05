using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXSpectrum.TapeFiles;

namespace ZXSpectrum.Test.TapeFiles
{
    public static class TapFileLoadTests
    {
        public static void CheckTapFileStructure(string fileName)
        {
            TapFile tapFile = TapFileReader.ReadTapFile(PlatformSpecific.GetStreamForProjectFile("TapeFiles/SampleTapFiles/" + fileName + ".tap"), fileName);
            string testResult = tapFile.ToString();

            string expectedResult = null;
            using (StreamReader reader = new StreamReader(PlatformSpecific.GetStreamForProjectFile("TapeFiles/SampleTapFiles/" + fileName + ".txt")))
            {
                expectedResult = reader.ReadToEnd();
            }

            if (testResult != expectedResult)
            {
                throw new Exception("Tap file load test failed for file "+fileName);
            }

            /*string testOverview = tapFile.GetContentsOverview();

            string expectedOverview = null;
            using (StreamReader reader = new StreamReader(PlatformSpecific.GetStreamForProjectFile("TapeFiles/SampleTapFiles/" + fileName + ".overview.txt")))
            {
                expectedOverview = reader.ReadToEnd();
            }

            if (testOverview != expectedOverview)
            {
                throw new Exception("Tap file overview test failed for file " + fileName);
            }*/
        }
    }
}
