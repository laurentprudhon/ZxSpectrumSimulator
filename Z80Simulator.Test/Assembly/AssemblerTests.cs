using System;
using System.IO;
using System.Text;
using Z80Simulator.Assembly;

namespace Z80Simulator.Test.Assembly
{
    public class AssemblerTests
    {
        public static void CheckProgram(string filename)
        {
            string objFilePath = String.Format("Assembly/Samples/{0}.obj", filename);
            Stream objStream = PlatformSpecific.GetStreamForProjectFile(objFilePath);

            string asmFilePath = String.Format("Assembly/Samples/{0}.asm", filename);
            Stream asmStream = PlatformSpecific.GetStreamForProjectFile(asmFilePath);

            byte[] referenceMemory = ReadToEnd(objStream);
                        
            Program program = Assembler.ParseProgram(filename, asmStream, Encoding.UTF8, false);
            MemoryMap compiledMemory = new MemoryMap(referenceMemory.Length);
            Assembler.CompileProgram(program, 0, compiledMemory);

            for (int i = 0; i < referenceMemory.Length; i++)
            {
                if (compiledMemory.MemoryCells[i] != referenceMemory[i])
                {
                    ProgramLine errorLine = null;
                    foreach (ProgramLine prgLine in program.Lines)
                    {
                        if (prgLine.LineAddress > i)
                            break;
                        else if (prgLine.Type != ProgramLineType.CommentOrWhitespace)
                            errorLine = prgLine;
                    }
                    if (errorLine.Type == ProgramLineType.OpCodeInstruction)
                    {
                        throw new Exception(String.Format("Compilation result different from reference at address {0} - compiled {1:X} / expected {2:X} - line {3} starting at address {4}, {5}+{6} bytes : {7}", i, compiledMemory.MemoryCells[i], referenceMemory[i], errorLine.LineNumber, errorLine.LineAddress, errorLine.InstructionCode.OpCodeByteCount, errorLine.InstructionCode.OperandsByteCount, errorLine.Text));
                    }
                    else
                    {
                        throw new Exception(String.Format("Compilation result different from reference at address {0} - compiled {1:X} / expected {2:X} - line {3} starting at address {4}, {5}+{6} bytes : {7}", i, compiledMemory.MemoryCells[i], referenceMemory[i], errorLine.LineNumber, errorLine.LineAddress, errorLine.Text));
                    }
                }
            }
        }

        private static byte[] ReadToEnd(Stream input)
        {
            byte[] buffer = new byte[8 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        } 

    }
}
