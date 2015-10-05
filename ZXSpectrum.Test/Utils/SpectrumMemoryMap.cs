using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Z80Simulator.Assembly;

namespace ZXSpectrum.Test.Utils
{
    public class SpectrumMemoryMap : MemoryMap
    {
        public Program ROM { get; private set; }

        public SpectrumMemoryMap() : base(65536)
        {
            // Load ROM assembly source code
            string sourcePath = "zxspectrum.asm";
            string romProgramText = LoadROMAssemblySource(sourcePath);

            // Parse ROM program
            using (Stream romProgramStream = new MemoryStream(Encoding.UTF8.GetBytes(romProgramText)))
            {
                ROM = Assembler.ParseProgram(sourcePath, romProgramStream, Encoding.UTF8, false);
            }
            
            // Compile ROM program and store it in memeory
            Assembler.CompileProgram(ROM, 0, this);
            
            // Analyze ROM ProgramFlow
            Assembler.AnalyzeProgramFlow(ROM, this);

            // Register system variables descriptions
            foreach (SpectrumSystemVariable variable in SystemVariables.Values)
            {
                for (int i = variable.Address; i < (variable.Address + variable.Length); i++)
                {
                    MemoryCellDescriptions[i] = new MemoryCellDescription(MemoryDescriptionType.SystemVariable, variable);
                }
            }
        }

        private static string LoadROMAssemblySource(string sourcePath)
        {
            // Modify original program to show useful labels
            string romProgramText = null;
            using (StreamReader romProgramFileReader = new StreamReader(ZXSpectrum.PlatformSpecific.GetStreamForProjectFile(sourcePath)))
            {
                romProgramText = romProgramFileReader.ReadToEnd();
                romProgramFileReader.BaseStream.Seek(0, SeekOrigin.Begin);
                string line = null;
                bool bChangeNextLine = false;
                string targetLabel = null;
                while ((line = romProgramFileReader.ReadLine()) != null)
                {

                    if (bChangeNextLine)
                    {
                        bChangeNextLine = false;
                        string sourceLabel = line.Split(':')[0].Trim();
                        if (sourceLabel.StartsWith("L"))
                        {
                            romProgramText = romProgramText.Replace(sourceLabel, targetLabel);
                        }
                    }
                    if (line.Trim().StartsWith(";;"))
                    {
                        targetLabel = line.Substring(2).Trim().Split(' ')[0];
                        if (targetLabel.Length > 0)
                        {
                            if (AsmLexer.DIRECTIVES.Contains<string>(targetLabel, StringComparer.InvariantCultureIgnoreCase) ||
                                AsmLexer.OPCODES.Contains<string>(targetLabel, StringComparer.InvariantCultureIgnoreCase)) targetLabel += "_";
                            targetLabel = targetLabel.Replace('-', '_').Replace('/', '_').Replace("@", "at").Replace("&", "amp").Replace("+", "plus").Replace("*", "mult").Replace("$", "S").Replace('?', '_').Replace(',', '_').Replace('.', '_').Replace('(', '_').Replace(')', '_').Replace("^", "pow");
                            bChangeNextLine = true;
                        }
                    }
                }
            }
            return romProgramText;
        }

        public static IDictionary<int,SpectrumSystemVariable> SystemVariables { get; private set; }

        static SpectrumMemoryMap()
        {
            SystemVariables = new Dictionary<int,SpectrumSystemVariable>();
            SystemVariables.Add(23552, new SpectrumSystemVariable(23552, 8, "KSTATE", "Used in reading the keyboard."));
            SystemVariables.Add(23560, new SpectrumSystemVariable(23560, 1, "LAST_K", "Stores newly pressed key."));
            SystemVariables.Add(23561, new SpectrumSystemVariable(23561, 1, "REPDEL", "Time (in 50ths of a second in 60ths of a second in N. America) that a key must be held down before it repeats. This starts off at 35, but you can POKE in other values."));
            SystemVariables.Add(23562, new SpectrumSystemVariable(23562, 1, "REPPER", "Delay (in 50ths of a second in 60ths of a second in N. America) between successive repeats of a key held down: initially 5."));
            SystemVariables.Add(23563, new SpectrumSystemVariable(23563, 2, "DEFADD", "Address of arguments of user defined function if one is being evaluated; otherwise 0."));
            SystemVariables.Add(23565, new SpectrumSystemVariable(23565, 1, "K_DATA", "Stores 2nd byte of colour controls entered from keyboard ."));
            SystemVariables.Add(23566, new SpectrumSystemVariable(23566, 2, "TVDATA", "Stores bytes of coiour, AT and TAB controls going to television."));
            SystemVariables.Add(23568, new SpectrumSystemVariable(23568, 38, "STRMS", "Addresses of channels attached to streams."));
            SystemVariables.Add(23606, new SpectrumSystemVariable(23606, 2, "CHARS", "256 less than address of character set (which starts with space and carries on to the copyright symbol). Normally in ROM, but you can set up your own in RAM and make CHARS point to it."));
            SystemVariables.Add(23608, new SpectrumSystemVariable(23608, 1, "RASP", "Length of warning buzz."));
            SystemVariables.Add(23609, new SpectrumSystemVariable(23609, 1, "PIP", "Length of keyboard click."));
            SystemVariables.Add(23610, new SpectrumSystemVariable(23610, 1, "ERR_NR", "1 less than the report code. Starts off at 255 (for 1) so PEEK 23610 gives 255."));
            SystemVariables.Add(23611, new SpectrumSystemVariable(23611, 1, "FLAGS", "Various flags to control the BASIC system."));
            SystemVariables.Add(23612, new SpectrumSystemVariable(23612, 1, "TV_FLAG", "Flags associated with the television."));
            SystemVariables.Add(23613, new SpectrumSystemVariable(23613, 2, "ERR_SP", "Address of item on machine stack to be used as error return."));
            SystemVariables.Add(23615, new SpectrumSystemVariable(23615, 2, "LIST_SP", "Address of return address from automatic listing."));
            SystemVariables.Add(23617, new SpectrumSystemVariable(23617, 1, "MODE", "Specifies K, L, C. E or G cursor."));
            SystemVariables.Add(23618, new SpectrumSystemVariable(23618, 2, "NEWPPC", "Line to be jumped to."));
            SystemVariables.Add(23620, new SpectrumSystemVariable(23620, 1, "NSPPC", "Statement number in line to be jumped to. Poking first NEWPPC and then NSPPC forces a jump to a specified statement in a line."));
            SystemVariables.Add(23621, new SpectrumSystemVariable(23621, 2, "PPC", "Line number of statement currently being executed."));
            SystemVariables.Add(23623, new SpectrumSystemVariable(23623, 1, "SUBPPC", "Number within line of statement being executed."));
            SystemVariables.Add(23624, new SpectrumSystemVariable(23624, 1, "BORDCR", "Border colour * 8; also contains the attributes normally used for the lower half of the screen."));
            SystemVariables.Add(23625, new SpectrumSystemVariable(23625, 2, "E_PPC", "Number of current line (with program cursor)."));
            SystemVariables.Add(23627, new SpectrumSystemVariable(23627, 2, "VARS", "Address of variables."));
            SystemVariables.Add(23629, new SpectrumSystemVariable(23629, 2, "DEST", "Address of variable in assignment."));
            SystemVariables.Add(23631, new SpectrumSystemVariable(23631, 2, "CHANS", "Address of channel data."));
            SystemVariables.Add(23633, new SpectrumSystemVariable(23633, 2, "CURCHL", "Address of information currently being used for input and output."));
            SystemVariables.Add(23635, new SpectrumSystemVariable(23635, 2, "PROG", "Address of BASIC program."));
            SystemVariables.Add(23637, new SpectrumSystemVariable(23637, 2, "NXTLIN", "Address of next line in program."));
            SystemVariables.Add(23639, new SpectrumSystemVariable(23639, 2, "DATADD", "Address of terminator of last DATA item."));
            SystemVariables.Add(23641, new SpectrumSystemVariable(23641, 2, "E_LINE", "Address of command being typed in."));
            SystemVariables.Add(23643, new SpectrumSystemVariable(23643, 2, "K_CUR", "Address of cursor."));
            SystemVariables.Add(23645, new SpectrumSystemVariable(23645, 2, "CH_ADD", "Address of the next character to be interpreted: the character after the argument of PEEK, or the NEWLINE at the end of a POKE statement."));
            SystemVariables.Add(23647, new SpectrumSystemVariable(23647, 2, "X_PTR", "Address of the character after the ? marker."));
            SystemVariables.Add(23649, new SpectrumSystemVariable(23649, 2, "WORKSP", "Address of temporary work space."));
            SystemVariables.Add(23651, new SpectrumSystemVariable(23651, 2, "STKBOT", "Address of bottom of calculator stack."));
            SystemVariables.Add(23653, new SpectrumSystemVariable(23653, 2, "STKEND", "Address of start of spare space."));
            SystemVariables.Add(23655, new SpectrumSystemVariable(23655, 1, "BREG", "Calculator's b register."));
            SystemVariables.Add(23656, new SpectrumSystemVariable(23656, 2, "MEM", "Address of area used for calculator's memory. (Usually MEMBOT, but not always.)"));
            SystemVariables.Add(23658, new SpectrumSystemVariable(23658, 1, "FLAGS2", "More flags."));
            SystemVariables.Add(23659, new SpectrumSystemVariable(23659, 1, "DF_SZ", "The number of lines (including one blank line) in the lower part of the screen."));
            SystemVariables.Add(23660, new SpectrumSystemVariable(23660, 2, "S_TOP", "The number of the top program line in automatic listings."));
            SystemVariables.Add(23662, new SpectrumSystemVariable(23662, 2, "OLDPPC", "Line number to which CONTINUE jumps."));
            SystemVariables.Add(23664, new SpectrumSystemVariable(23664, 1, "OSPCC", "Number within line of statement to which CONTINUE jumps."));
            SystemVariables.Add(23665, new SpectrumSystemVariable(23665, 1, "FLAGX", "Various flags."));
            SystemVariables.Add(23666, new SpectrumSystemVariable(23666, 2, "STRLEN", "Length of string type destination in assignment."));
            SystemVariables.Add(23668, new SpectrumSystemVariable(23668, 2, "T_ADDR", "Address of next item in syntax table (very unlikely to be useful)."));
            SystemVariables.Add(23670, new SpectrumSystemVariable(23670, 2, "SEED", "The seed for RND. This is the variable that is set by RANDOMIZE."));
            SystemVariables.Add(23672, new SpectrumSystemVariable(23672, 3, "FRAMES", "3 byte (least significant first), frame counter. Incremented every 20ms. See Chapter 18."));
            SystemVariables.Add(23675, new SpectrumSystemVariable(23675, 2, "UDG", "Address of 1st user defined graphic You can change this for instance to save space by having fewer user defined graphics."));
            SystemVariables.Add(23677, new SpectrumSystemVariable(23677, 1, "COORDS_X", "x-coordinate of last point plotted."));
            SystemVariables.Add(23678, new SpectrumSystemVariable(23678, 1, "COORDS_Y", "y-coordinate of last point plotted."));
            SystemVariables.Add(23679, new SpectrumSystemVariable(23679, 1, "P_POSN", "33 column number of printer position"));
            SystemVariables.Add(23680, new SpectrumSystemVariable(23680, 2, "PR_CC", "Full address of next position for LPRINT to print at (in ZX printer buffer). Legal values 5B00 - 5B1F. [Not used in 128K mode or when certain peripherals are attached]"));
            SystemVariables.Add(23682, new SpectrumSystemVariable(23682, 2, "ECHO_E", "33 column number and 24 line number (in lower half) of end of input buffer."));
            SystemVariables.Add(23684, new SpectrumSystemVariable(23684, 2, "DF_CC", "Address in display file of PRINT position."));
            SystemVariables.Add(23686, new SpectrumSystemVariable(23686, 2, "DFCCL", "Like DF CC for lower part of screen.")); 
            SystemVariables.Add(23688, new SpectrumSystemVariable(23688, 1, "S_POSN_X", "33 column number for PRINT position"));
            SystemVariables.Add(23689, new SpectrumSystemVariable(23689, 1, "S_POSN_Y", "24 line number for PRINT position."));
            SystemVariables.Add(23690, new SpectrumSystemVariable(23690, 1, "SPOSNL_X", "Like S POSN for lower part : 33 column number for PRINT position"));
            SystemVariables.Add(23691, new SpectrumSystemVariable(23691, 1, "SPOSNL_Y", "Like S POSN for lower part : 24 line number for PRINT position."));
            SystemVariables.Add(23692, new SpectrumSystemVariable(23692, 1, "SCR_CT", "Counts scrolls: it is always 1 more than the number of scrolls that will be done before stopping with scroll? If you keep poking this with a number bigger than 1 (say 255), the screen will scroll on and on without asking you."));
            SystemVariables.Add(23693, new SpectrumSystemVariable(23693, 1, "ATTR_P", "Permanent current colours, etc (as set up by colour statements)."));
            SystemVariables.Add(23694, new SpectrumSystemVariable(23694, 1, "MASK_P", "Used for transparent colours, etc. Any bit that is 1 shows that the corresponding attribute bit is taken not from ATTR P, but from what is already on the screen."));
            SystemVariables.Add(23695, new SpectrumSystemVariable(23695, 1, "ATTR_T", "Temporary current colours, etc (as set up by colour items)."));
            SystemVariables.Add(23696, new SpectrumSystemVariable(23696, 1, "MASK_T", "Like MASK P, but temporary."));
            SystemVariables.Add(23697, new SpectrumSystemVariable(23697, 1, "P_FLAG", "More flags."));
            SystemVariables.Add(23698, new SpectrumSystemVariable(23698, 30, "MEMBOT", "Calculator's memory area; used to store numbers that cannot conveniently be put on the calculator stack."));
            SystemVariables.Add(23730, new SpectrumSystemVariable(23730, 2, "RAMTOP", "Address of last byte of BASIC system area."));
            SystemVariables.Add(23732, new SpectrumSystemVariable(23732, 2, "P_RAMT", "Address of last byte of physical RAM."));
        }
    }

    public class SpectrumSystemVariable
    {
        public SpectrumSystemVariable(int address, int length, string name, string description)
        {
            Address = address;
            Length = length;
            Name = name;
            Description = description;
        }

        public int Address { get; private set; }
        public int Length { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
    }
}
