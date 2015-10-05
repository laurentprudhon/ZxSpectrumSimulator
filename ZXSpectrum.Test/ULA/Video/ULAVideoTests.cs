using System;
using System.IO;
using System.Text;
using Z80Simulator.System;
using ZXSpectrum.Test.System;

namespace ZXSpectrum.Test.ULAChip
{
    public class ULAVideoTests
    {
        public static byte[] Check_CompleteScreenCycle()
        {
            ULATestSystem testSystem = new ULATestSystem();
            testSystem.LoadScreenInVideoMemory("ULA/Video/ScreenSamples/BuggyBoy.scr");
            testSystem.ULA.Debug_SetBorderColor(4);

            ULAStateLogger logger = new ULAStateLogger(testSystem.ULA);
            logger.StartLogging(
                ULAStateLogger.ULAStateElements.MasterCounters | ULAStateLogger.ULAStateElements.DataIO | ULAStateLogger.ULAStateElements.VideoOutput | ULAStateLogger.ULAStateElements.VideoRegisters,
                new ULA.LifecycleEventType[] { ULA.LifecycleEventType.ClockCycle });
            for (int i = 0; i < ULA.ONE_FRAME_TICKS_COUNT + 7; i++)
            {
                testSystem.PixelClock.Tick();
            }
            logger.StopLogging();

            if (!logger.CompareWithSavedLog("ULA/Video/Logs/Check_CompleteScreenCycle"))
            {
                throw new Exception("Log compare failed");
            }

            ZXSpectrumComputer computer = new ZXSpectrumComputer();
            // Load a program that does nothing in computer ROM
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("JP 0");
            computer.LoadProgramInROM(testProgram.ToString());
            // Load sample picture in video memory and set green border color
            using (Stream stream = PlatformSpecific.GetStreamForProjectFile("ULA/Video/ScreenSamples/BuggyBoy.scr"))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    reader.Read(computer.RAM16K.Cells, 0, 6912);
                }
            }
            computer.ULA.Debug_SetBorderColor(4);
            
            // Render the same screen with the complete computer
            for (int i = 0; i < ULA.ONE_FRAME_TICKS_COUNT + 7; i++)
            {
                computer.Clock.Tick();
            }
            string testScreenValues = GetDisplayAsString(testSystem.Screen.Display);
            string computerScreenValues = GetDisplayAsString(computer.Screen.Display);

            if (!testScreenValues.Equals(computerScreenValues))
            {
                throw new Exception("Screen compare failed for complete computer");
            }

            return testSystem.Screen.Display;
        }

        private static string GetDisplayAsString(byte[] display)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < display.Length; i++)
            {
                sb.AppendLine(i + " : " + display[i]);
            }
            return sb.ToString();
        }

        public static byte[] Check_FlashClock()
        {
            ULATestSystem testSystem = new ULATestSystem();
            testSystem.LoadScreenInVideoMemory("ULA/Video/ScreenSamples/CrazyGolf.scr");
            testSystem.ULA.Debug_SetBorderColor(15);
            testSystem.ULA.Debug_SetFrameCounter(15);

            for (int i = 0; i < ULA.ONE_FRAME_TICKS_COUNT; i++)
            {
                testSystem.PixelClock.Tick();
            }

            byte[] firstScreen = testSystem.Screen.CaptureDisplay();

            for (int i = 0; i < ULA.ONE_FRAME_TICKS_COUNT; i++)
            {
                testSystem.PixelClock.Tick();
            }

            byte[] secondScreen = testSystem.Screen.CaptureDisplay();

            int differenceCount = 0;
            for (int i = 0; i < firstScreen.Length; i++)
            {
                if (firstScreen[i] != secondScreen[i]) differenceCount++;
            }
            if (differenceCount != 322)
            {
                throw new Exception("Screen comparisons failed");
            }

            return secondScreen;
        }
    }
}