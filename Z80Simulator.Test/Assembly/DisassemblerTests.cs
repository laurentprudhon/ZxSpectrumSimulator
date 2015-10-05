using System;
using System.IO;
using Z80Simulator.Assembly;

namespace Z80Simulator.Test.Assembly
{
    public class DisassemblerTests
    {
        public static void CheckMemoryDump(string filename)
        {
            string objFilepath = String.Format("Assembly/Samples/{0}.obj", filename);
            Stream objStream = PlatformSpecific.GetStreamForProjectFile(objFilepath);

            string asmFilepath = String.Format("Assembly/DisassemblerResults/{0}.disasm", filename);
            Stream asmStream = PlatformSpecific.GetStreamForProjectFile(asmFilepath);
                        
            string referenceAsmText = null;
            using (StreamReader sr = new StreamReader(asmStream))
            {
                referenceAsmText = sr.ReadToEnd();
            }

            MemoryMap memoryMap = new MemoryMap(65536);
            CallTarget entryPoint = new CallTarget(new CallSource(CallSourceType.Boot, 0, null), 0);
            Program program = Disassembler.GenerateProgram(objFilepath, objStream, 0, new CallTarget[] { entryPoint }, memoryMap);

            if (program.ToString() != referenceAsmText)
            {
                throw new Exception(String.Format("Disassembly of program {0} does not match reference text", filename));
            }
        }
    }
}
