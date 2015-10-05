using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXSpectrum.TapeFiles;

namespace ZXSpectrum.Test.TapeFiles
{
    public static class TapeRecorderTests
    {
        public static void CheckTapeRecorderStates()
        {
            StringBuilder testStates = new StringBuilder();

            TapeRecorder tapeRecorder = new _TapeRecorder();
            testStates.AppendLine(tapeRecorder.ToString());

            // From initial state
            tapeRecorder.Eject();
            testStates.AppendLine(tapeRecorder.ToString());
            tapeRecorder.RewindOrForward(10);
            testStates.AppendLine(tapeRecorder.ToString());
            tapeRecorder.RewindToStart();
            testStates.AppendLine(tapeRecorder.ToString());
            tapeRecorder.Start();
            testStates.AppendLine(tapeRecorder.ToString());
            for (int i = 1; i < 5000; i++ )
            {
                tapeRecorder.Tick();
            }
            testStates.AppendLine(tapeRecorder.ToString());
            tapeRecorder.Stop();
            testStates.AppendLine(tapeRecorder.ToString());

            // Tape inserted
            string fileName = "BuggyBoy.tzx";
            using (Stream stream = PlatformSpecific.GetStreamForProjectFile("TapeFiles/SampleTzxFiles/" + fileName))
            {
                tapeRecorder.InsertTape(stream, fileName);
            }
            testStates.AppendLine(tapeRecorder.ToString());
            tapeRecorder.Stop();
            testStates.AppendLine(tapeRecorder.ToString());
            tapeRecorder.RewindToStart();
            testStates.AppendLine(tapeRecorder.ToString());
            tapeRecorder.Start();
            testStates.AppendLine(tapeRecorder.ToString());
            for (int i = 1; i <= 3000; i++)
            {
                tapeRecorder.Tick();
            }
            testStates.AppendLine(tapeRecorder.ToString());
            tapeRecorder.Stop();
            testStates.AppendLine(tapeRecorder.ToString());
            tapeRecorder.Start();
            testStates.AppendLine(tapeRecorder.ToString());
            for (int i = 1; i <= 2000; i++)
            {
                tapeRecorder.Tick();
            }
            testStates.AppendLine(tapeRecorder.ToString());
            for (int i = 1; i <= 2000; i++)
            {
                tapeRecorder.Tick();
            }
            testStates.AppendLine(tapeRecorder.ToString());
            for (int i = 1; i <= 17500000; i++)
            {
                tapeRecorder.Tick();
            }
            testStates.AppendLine(tapeRecorder.ToString());
            tapeRecorder.RewindToStart();
            testStates.AppendLine(tapeRecorder.ToString());
            tapeRecorder.Start();
            testStates.AppendLine(tapeRecorder.ToString());
            for (int i = 1; i <= 4000; i++)
            {
                tapeRecorder.Tick();
            }
            testStates.AppendLine(tapeRecorder.ToString());
            tapeRecorder.RewindOrForward(5);
            testStates.AppendLine(tapeRecorder.ToString());
            tapeRecorder.Start();
            testStates.AppendLine(tapeRecorder.ToString());
            for (int i = 1; i <= 3000; i++)
            {
                tapeRecorder.Tick();
            }
            testStates.AppendLine(tapeRecorder.ToString());
            tapeRecorder.Stop();
            testStates.AppendLine(tapeRecorder.ToString());
            tapeRecorder.Start();
            testStates.AppendLine(tapeRecorder.ToString());
            for (int i = 1; i <= 3000; i++)
            {
                tapeRecorder.Tick();
            }
            testStates.AppendLine(tapeRecorder.ToString());
            tapeRecorder.Eject();
            testStates.AppendLine(tapeRecorder.ToString());
            tapeRecorder.Start();
            testStates.AppendLine(tapeRecorder.ToString());
            for (int i = 1; i <= 5000; i++)
            {
                tapeRecorder.Tick();
            }
            testStates.AppendLine(tapeRecorder.ToString());

            string testResult = testStates.ToString();

            string expectedResult = null;
            using (StreamReader reader = new StreamReader(PlatformSpecific.GetStreamForProjectFile("TapeFiles/Logs/CheckTapeRecorderStates.txt")))
            {
                expectedResult = reader.ReadToEnd();
            }

            if (testResult != expectedResult)
            {
                throw new Exception("TapeRecorder states test failed");
            }
        }

        public static void CheckTapeRecorderPlayback()
        {
            TapeRecorder tapeRecorder = new _TapeRecorder();
            
            Tape testTape = new Tape("TestTape",
                new TapeSection("section 1",
                    new ToneSequence("tone", 4, 5),                                 // 20 TStates
                    new PulsesSequence("pulses", new ushort[] {1,2,3,4}),           // 10 TStates
                    new DataSequence("data", new byte[] {15,175}, 1, 2, 4)),        // 36 TStates
                new TapeSection("section 2",
                    new PauseSequence("pause", 1)),                                 // 3500 TStates
                new TapeSection("section 3",
                    new SamplesSequence("samples", new byte[] { 15, 175 }, 2, 7))); // 30 TStates

            tapeRecorder.InsertTape(testTape);

            StringBuilder sbResult = new StringBuilder();

            tapeRecorder.Start();
            sbResult.AppendLine(tapeRecorder.PlayingTime + "," + tapeRecorder.SoundSignal.Level + "," + tapeRecorder.GetPosition());

            for (int i = 1; i < 70; i++)
            {
                tapeRecorder.Tick();
                sbResult.AppendLine(tapeRecorder.PlayingTime + "," + tapeRecorder.SoundSignal.Level + "," + tapeRecorder.GetPosition());
            }

            for (int i = 1; i <= 3492; i++)
            {
                tapeRecorder.Tick();
            }

            for (int i = 1; i <= 35; i++)
            {
                tapeRecorder.Tick();
                sbResult.AppendLine(tapeRecorder.PlayingTime + "," + tapeRecorder.SoundSignal.Level + "," + tapeRecorder.GetPosition());
            }

            string testResult = sbResult.ToString();
            
            string expectedResult = null;
            using (StreamReader reader = new StreamReader(PlatformSpecific.GetStreamForProjectFile("TapeFiles/Logs/CheckTapeRecorderPlayback.csv")))
            {
                expectedResult = reader.ReadToEnd();
            }

            if (testResult != expectedResult)
            {
                throw new Exception("TapeRecorder playback test failed");
            }
        }

        private class _TapeRecorder : TapeRecorder
        {
            public override Z80Simulator.System.SignalState TimeCLK
            {
                get { return Z80Simulator.System.SignalState.LOW; }
            }
        }
    }
}
