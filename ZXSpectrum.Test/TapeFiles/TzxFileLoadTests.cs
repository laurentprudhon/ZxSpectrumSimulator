using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXSpectrum.TapeFiles;

namespace ZXSpectrum.Test.TapeFiles
{
    public static class TzxFileLoadTests
    {
        public static void CheckTzxFileStructure(string fileName)
        {
            TzxFile tzxFile = TzxFileReader.ReadTzxFile(PlatformSpecific.GetStreamForProjectFile("TapeFiles/SampleTzxFiles/" + fileName + ".tzx"), fileName);
            string testResult = tzxFile.ToString();

            string expectedResult = null;
            using (StreamReader reader = new StreamReader(PlatformSpecific.GetStreamForProjectFile("TapeFiles/SampleTzxFiles/" + fileName + ".txt")))
            {
                expectedResult = reader.ReadToEnd();
            }

            if (testResult != expectedResult)
            {
                throw new Exception("Tzx file load test failed for file " + fileName);
            }
        }
    }
}
