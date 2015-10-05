#if NETFX_CORE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace ZXSpectrum.Test
{
    [TestClass]
    public class TestLauncher
    {
        [TestMethod]
        public void RunBasicTests()
        {
            TestCollection.RunBasicTests();
        }

        [TestMethod]
        public void RunTapeFilesLoadTests()
        {
            TestCollection.RunTapeFilesLoadTests();
        }

        [TestMethod]
        public void RunTapeRecorderTests()
        {
            TestCollection.RunTapeRecorderTests();
        }

        [TestMethod]
        public void RunULAVideoTests()
        {
            TestCollection.RunULAVideoTests();
        }

        [TestMethod]
        public void RunComputerTests()
        {
            TestCollection.RunComputerTests();
        }

        [TestMethod]
        public void GenerateHtmlForROM()
        {
            ZXSpectrum.Test.Utils.AsmHtmlFormatter.GenerateHtmlForROM();
        }
    }
}
