using System;
using System.IO;
using System.Text;
using Z80Simulator.Test.System;

namespace Z80Simulator.Test.ALU
{
    public partial class ALUInstructionsTests
    {
        /// <summary>
        /// Utility method used to compare execution log with arithmetic operations results (8 bits arithmetic)
        /// </summary>
        private static void CompareExecutionResultsWithSample(string sampleProgramFileName, string sampleResultFileName, int instructionsPerLine, int accColumn)
        {
            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(sampleProgramFileName, Encoding.UTF8, false);

            Stream stream = PlatformSpecific.GetStreamForProjectFile(sampleResultFileName);
            using (StreamReader reader = new StreamReader(stream))
            {
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    testSystem.ExecuteInstructionCount(instructionsPerLine);

                    string[] columns = line.Split(';');
                    string AHexValue = columns[accColumn];
                    byte AByteValue = Convert.ToByte(AHexValue, 16);
                    string FBinValue = columns[accColumn + 1];
                    byte FByteValue = Convert.ToByte(FBinValue, 2);

                    if (testSystem.CPU.A != AByteValue)
                    {
                        throw new Exception(String.Format("Error line {0}, A = {1}", line, String.Format("{0:X}", testSystem.CPU.A)));
                    }
                    else if ((testSystem.CPU.F) != FByteValue)
                    {
                        throw new Exception(String.Format("Error line {0}, F = {1}", line, Convert.ToString(testSystem.CPU.F, 2)));
                    }
                }
            }
        }

        /// <summary>
        /// Utility method used to compare execution log with arithmetic operations results (16 bits arithmetic)
        /// </summary>
        private static void Compare16bitsExecutionResultsWithSample(string sampleProgramFileName, string sampleResultFileName, int instructionsPerLine)
        {
            TestSystem testSystem = new TestSystem();
            testSystem.LoadProgramInMemory(sampleProgramFileName, Encoding.UTF8, false);

            Stream stream = PlatformSpecific.GetStreamForProjectFile(sampleResultFileName);
            using (StreamReader reader = new StreamReader(stream))
            {
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    testSystem.ExecuteInstructionCount(instructionsPerLine);

                    string[] columns = line.Split(';');
                    string HHexValue = columns[3];
                    byte HByteValue = Convert.ToByte(HHexValue, 16);
                    string LHexValue = columns[4];
                    byte LByteValue = Convert.ToByte(LHexValue, 16);
                    string FBinValue = columns[5];
                    byte FByteValue = Convert.ToByte(FBinValue, 2);

                    if (testSystem.CPU.L != LByteValue)
                    {
                        throw new Exception(String.Format("Error line {0}, L = {1}", line, String.Format("{0:X}", testSystem.CPU.L)));
                    }
                    else if (testSystem.CPU.H != HByteValue)
                    {
                        throw new Exception(String.Format("Error line {0}, H = {1}", line, String.Format("{0:X}", testSystem.CPU.H)));
                    }
                    // Ignore undocumented flags 3 and 5 : 11010111 = 215
                    else if ((testSystem.CPU.F & 215) != (FByteValue & 215))
                    {
                        throw new Exception(String.Format("Error line {0}, F = {1}", line, Convert.ToString(testSystem.CPU.F, 2)));
                    }
                }
            }
        }
    }
}
