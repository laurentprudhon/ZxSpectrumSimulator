using System.Collections.Generic;
using ZXSpectrum.Test.Basic;
using ZXSpectrum.Test.Computer;
using ZXSpectrum.Test.TapeFiles;
using ZXSpectrum.Test.ULAChip;

namespace ZXSpectrum.Test
{
    public class TestCollection
    {
        public static void RunBasicTests()
        {
            BasicTests.CheckSpectrumNumbers();
            BasicTests.CheckBasicText();
        }

        public static void RunTapeFilesLoadTests()
        {
            TapFileLoadTests.CheckTapFileStructure("Arkanoid");
            TapFileLoadTests.CheckTapFileStructure("BuggyBoy");
            TapFileLoadTests.CheckTapFileStructure("OhMummy");
            TapFileLoadTests.CheckTapFileStructure("TreasureIsland");
            TapFileLoadTests.CheckTapFileStructure("Wizzball");

            TzxFileLoadTests.CheckTzxFileStructure("1942");
            TzxFileLoadTests.CheckTzxFileStructure("Airwolf");
            TzxFileLoadTests.CheckTzxFileStructure("AlienDestroyer");
            TzxFileLoadTests.CheckTzxFileStructure("BarbarianII-TheDungeonOfDrax");
            TzxFileLoadTests.CheckTzxFileStructure("BattleShips");
            TzxFileLoadTests.CheckTzxFileStructure("Batty");
            TzxFileLoadTests.CheckTzxFileStructure("BeyondTheIcePalace");
            TzxFileLoadTests.CheckTzxFileStructure("BombJack");
            TzxFileLoadTests.CheckTzxFileStructure("BuggyBoy");
            TzxFileLoadTests.CheckTzxFileStructure("Commando");
            TzxFileLoadTests.CheckTzxFileStructure("CrazyGolf");
            TzxFileLoadTests.CheckTzxFileStructure("DiscoDan");
            TzxFileLoadTests.CheckTzxFileStructure("GhostsNGoblins");
            TzxFileLoadTests.CheckTzxFileStructure("OhMummy");
            TzxFileLoadTests.CheckTzxFileStructure("Punchy");
            TzxFileLoadTests.CheckTzxFileStructure("Saboteur");
            TzxFileLoadTests.CheckTzxFileStructure("Scooby-Doo");
            TzxFileLoadTests.CheckTzxFileStructure("Thundercats");
            TzxFileLoadTests.CheckTzxFileStructure("TreasureIsland");
            TzxFileLoadTests.CheckTzxFileStructure("Wizball");
        }


        internal static void RunTapeRecorderTests()
        {
            TapeRecorderTests.CheckTapeRecorderStates();
            TapeRecorderTests.CheckTapeRecorderPlayback();
        }

        public static void RunULAVideoTests()
        {
            ULAVideoTests.Check_CompleteScreenCycle();
            ULAVideoTests.Check_FlashClock();
        }

        public static void RunComputerTests()
        {
            ComputerTests.Check_CPUMemoryAccess();
            ComputerTests.Check_CPUTapeRecorderAccess();
        }
    }
}