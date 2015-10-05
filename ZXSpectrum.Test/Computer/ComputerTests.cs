using System;
using System.Text;
using ZXSpectrum.TapeFiles;

namespace ZXSpectrum.Test.Computer
{
    public class ComputerTests
    {
        public static void Check_CPUMemoryAccess()
        {
            // Read ROM value
            StringBuilder testProgram = new StringBuilder();
            testProgram.AppendLine("LD A,(100)");
            testProgram.AppendLine("LD B,A");
            testProgram.AppendLine("LD A,(16383)");
            testProgram.AppendLine("LD C,A");
            testProgram.AppendLine("JP 0");
            testProgram.AppendLine("ORG 100");
            testProgram.AppendLine("DEFB 33");
            testProgram.AppendLine("ORG 16383");
            testProgram.AppendLine("DEFB 66");

            ZXSpectrumComputer computer = new ZXSpectrumComputer();
            computer.LoadProgramInROM(testProgram.ToString());
            computer.ExecuteInstructionCount(5);

            if (computer.CPU.B != 33 || computer.ROM.Cells[100] != 33 ||
                computer.CPU.C != 66 || computer.ROM.Cells[16383] != 66)
            {
                throw new Exception("ROM access error");
            }

            // Write and read RAM16K value
            testProgram = new StringBuilder();
            testProgram.AppendLine("LD A,44");
            testProgram.AppendLine("LD (16384),A");
            testProgram.AppendLine("LD A,88");
            testProgram.AppendLine("LD (32767),A");
            testProgram.AppendLine("LD A,(16384)");
            testProgram.AppendLine("LD B,A");
            testProgram.AppendLine("LD A,(32767)");
            testProgram.AppendLine("LD C,A");
            testProgram.AppendLine("JP 0");
                        
            computer.LoadProgramInROM(testProgram.ToString());
            computer.ExecuteInstructionCount(9);

            if (computer.CPU.B != 44 || computer.RAM16K.Cells[0] != 44 ||
                computer.CPU.C != 88 || computer.RAM16K.Cells[16383] != 88)
            {
                throw new Exception("RAM16K access error");
            }

            // Write and read RAM32K value
            testProgram = new StringBuilder();
            testProgram.AppendLine("LD A,55");
            testProgram.AppendLine("LD (32768),A");
            testProgram.AppendLine("LD A,99");
            testProgram.AppendLine("LD (65535),A");
            testProgram.AppendLine("LD A,(32768)");
            testProgram.AppendLine("LD B,A");
            testProgram.AppendLine("LD A,(65535)");
            testProgram.AppendLine("LD C,A");

            computer.LoadProgramInROM(testProgram.ToString());
            computer.ExecuteInstructionCount(8);

            if (computer.CPU.B != 55 || computer.RAM32K.Cells[0] != 55 ||
                computer.CPU.C != 99 || computer.RAM32K.Cells[32767] != 99)
            { 
                throw new Exception("RAM16K access error");
            }
        }

        public static void Check_CPUTapeRecorderAccess()
        {


            ZXSpectrumComputer computer = new ZXSpectrumComputer();
          
            Tape testTape = new Tape("TestTape",
                                new TapeSection("section 1",
                                    new ToneSequence("tone", 6, 26)));
            computer.TapeRecorder.InsertTape(testTape);

            // From ROM function LD-EDGE-1
            StringBuilder testProgram = new StringBuilder();
            // 7 TStates
            testProgram.AppendLine("LD      A,$7F           ; prepare to read keyboard and EAR port");
            // 11 TSTates
            testProgram.AppendLine("IN      A,($FE)         ; row $7FFE. bit 6 is EAR, bit 0 is SPACE key.");
            // 10 TStates
            testProgram.AppendLine("JP 0");               
            computer.LoadProgramInROM(testProgram.ToString());

            computer.TapeRecorder.Start();
            computer.ExecuteInstructionCount(2);
            if (computer.TapeRecorder.SoundSignal.Level == 0 && (computer.CPU.A & 0x40) != 0)
            {
                throw new Exception("Tape Recorder access error");
            }
            computer.ExecuteInstructionCount(3);
            if (computer.TapeRecorder.SoundSignal.Level == 1 && (computer.CPU.A & 0x40) == 0)
            {
                throw new Exception("Tape Recorder access error");
            }

            computer.Keyboard.OnKeyPress(Keyboard.Keys.Space);
            computer.ExecuteInstructionCount(6);
            if (computer.TapeRecorder.SoundSignal.Level == 0 && (computer.CPU.A & 0x40) != 0)
            {
                throw new Exception("Tape Recorder access error");
            }
            computer.ExecuteInstructionCount(3);
            if (computer.TapeRecorder.SoundSignal.Level == 1 && (computer.CPU.A & 0x40) == 0)
            {
                throw new Exception("Tape Recorder access error");
            }
        }
    }
}
