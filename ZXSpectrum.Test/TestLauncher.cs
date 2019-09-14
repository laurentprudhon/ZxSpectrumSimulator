using Xunit;

namespace ZXSpectrum.Test
{
    public class TestLauncher
    {
        /*[Fact]
        public void GenerateHtmlForROM()
        {
            ZXSpectrum.Test.Utils.AsmHtmlFormatter.GenerateHtmlForROM();
        }*/

        [Fact]
        public void RunBasicTests()
        {
            TestCollection.RunBasicTests();
        }

        [Fact]
        public void RunTapeFilesLoadTests()
        {
            TestCollection.RunTapeFilesLoadTests();
        }

        [Fact]
        public void RunTapeRecorderTests()
        {
            TestCollection.RunTapeRecorderTests();
        }

        [Fact]
        public void RunULAVideoTests()
        {
            TestCollection.RunULAVideoTests();
        }

        [Fact]
        public void RunComputerTests()
        {
            TestCollection.RunComputerTests();
        }
    }
}
