using System;
using System.Collections.Generic;
using System.IO;
using Z80Simulator.Instructions;

namespace Z80Simulator.GenerateTables
{
    public class TableGenerator
    {
        public void GenerateInstructionTypesTable()
        {
            string tableName = "InstructionTables/z80_instructions.csv";

            StringWriter writer = new StringWriter();

            writer.WriteLine("using System;");
            writer.WriteLine("");
            writer.WriteLine("namespace Z80Simulator.Instructions");
            writer.WriteLine("{");
            writer.WriteLine("    public static class Z80InstructionTypes");
            writer.WriteLine("    {");
            writer.WriteLine("        /// <summary>");
            writer.WriteLine("        /// Z80 Instruction Types tables.");
            writer.WriteLine("        /// To be user friendly in the documentation, the first instruction type is found at index 1.");
            writer.WriteLine("        /// </summary>");
            writer.WriteLine("        public static InstructionType[] Table = new InstructionType[163];");
            writer.WriteLine("");
            writer.WriteLine("        static Z80InstructionTypes()");
            writer.WriteLine("        {");

            IList<InstructionParamVariantLine> paramVariants = new List<InstructionParamVariantLine>();
            InstructionParamVariantLine previousVariant = null;

            Stream stream = PlatformSpecific.GetStreamForProjectFile(tableName);
             using (StreamReader reader = new StreamReader(stream))
             {
                 reader.ReadLine();
                 string line = null;
                 string nextLine = null;
                 int lineCount = 0;
                 while(true)
                 {
                     if (lineCount == 0)
                     {
                         line = reader.ReadLine();
                         lineCount++;
                     }
                     else
                     {
                         line = nextLine;
                     }
                     if (line == null)
                     {
                         break;
                     }
                     while (true)
                     {
                         nextLine = reader.ReadLine();
                         lineCount++;
                         if (nextLine == null)
                         {
                             break;
                         }
                         else if (nextLine.StartsWith("instr;") || nextLine.StartsWith("execvar;"))
                         {
                             break;
                         }
                         else
                         {
                             line += "\\n" + nextLine;
                         }
                     }
                                          
                     InstructionParamVariantLine instrLine = new InstructionParamVariantLine();

                     string[] columns = line.Split(';');
                     instrLine.lineType = columns[0];
                     instrLine.instructionIndex = columns[1];
                     instrLine.paramVariant = columns[2];
                     instrLine.numberOfInstructionCodes = columns[3];
                     instrLine.isInternal = columns[4] == "1" ? "true" : "false";
                     instrLine.mnemonic = columns[5];
                     instrLine.param1List = columns[6];
                     instrLine.param2List = columns[7];
                     instrLine.param3List = columns[8];
                     instrLine.param1AddressingMode = columns[10];
                     instrLine.param2AddressingMode = columns[11];
                     instrLine.param3AddressingMode = columns[12];
                     instrLine.instructionCodeSizeInBytes = columns[15];
                     instrLine.execVariant = columns[16];
                     instrLine.MCycles = columns[17];
                     instrLine.TStates = columns[18];
                     instrLine.execCondition = columns[19];
                     instrLine.M1 = columns[20].Trim();
                     instrLine.M2 = columns[21].Trim();
                     instrLine.M3 = columns[22].Trim();
                     instrLine.M4 = columns[23].Trim();
                     instrLine.M5 = columns[24].Trim();
                     instrLine.M6 = columns[25].Trim();
                     instrLine.stackComment = columns[26];
                     instrLine.M1comment = columns[27];
                     instrLine.instructionGroup = columns[29];
                     instrLine.undocumented = columns[30] == "1" ? "true" : "false";
                     instrLine.userManualPage = columns[31];
                     instrLine.operationSchema = columns[32];
                     instrLine.description = columns[33];
                     instrLine.conditionBitsAffected = columns[34];
                     instrLine.example = columns[35];

                     // Instruction / param variant
                     if (instrLine.lineType == "instr")
                     {
                         if (previousVariant != null && instrLine.instructionIndex != previousVariant.instructionIndex)
                         {
                             GenerateInstructionType(writer, paramVariants);
                             paramVariants = new List<InstructionParamVariantLine>();
                         }
                         paramVariants.Add(instrLine);
                     }
                     // Exec variant
                     else if (instrLine.lineType == "execvar")
                     {
                         previousVariant.ExecutionVariant = instrLine;
                     }

                     previousVariant = instrLine;
                  }
             }
             GenerateInstructionType(writer, paramVariants);

             writer.WriteLine("        }");
             writer.WriteLine("    }");
             writer.WriteLine("}");

             string Z80InstructionTypes_cs = writer.ToString();
         }

        private void GenerateInstructionType(StringWriter writer, IList<InstructionParamVariantLine> paramVariants)
        {   
            InstructionParamVariantLine instrLine = paramVariants[0];
            
            writer.WriteLine(String.Format("            Table[{0}] = new InstructionType()", instrLine.instructionIndex));
            writer.WriteLine("            {");
            writer.WriteLine(String.Format("                Index = {0},", instrLine.instructionIndex));
            writer.WriteLine(String.Format("                IsUndocumented = {0},", instrLine.undocumented));
            writer.WriteLine(String.Format("                IsInternalZ80Operation = {0},", instrLine.isInternal));
            writer.WriteLine("");
            writer.WriteLine(String.Format("                InstructionGroup = \"{0}\",", instrLine.instructionGroup));
            writer.WriteLine(String.Format("                OpCodeName = \"{0}\",", instrLine.mnemonic));
            if (instrLine.param1AddressingMode.Length > 0)
            {
                writer.WriteLine(String.Format("                Param1Type = AddressingMode.{0},", ReadAddressingMode(instrLine.param1AddressingMode)));
            }
            if (instrLine.param2AddressingMode.Length > 0)
            {
                writer.WriteLine(String.Format("                Param2Type = AddressingMode.{0},", ReadAddressingMode(instrLine.param2AddressingMode)));
            }
            if (instrLine.param3AddressingMode.Length > 0)
            {
                writer.WriteLine(String.Format("                Param3Type = AddressingMode.{0},", ReadAddressingMode(instrLine.param3AddressingMode)));
            }
            if (instrLine.operationSchema.Length > 0)
            {
                writer.WriteLine(String.Format("                Equation = \"{0}\",", instrLine.operationSchema.Replace("\"","")));
            }
            writer.WriteLine("");
            writer.WriteLine("                ParametersVariants = new InstructionParametersVariant[] {");

            for (int instrVariantIndex = 0; instrVariantIndex < paramVariants.Count ; instrVariantIndex++ )
            {
                InstructionParamVariantLine instrVariant = paramVariants[instrVariantIndex];

                writer.WriteLine("                new InstructionParametersVariant() {");
                writer.WriteLine(String.Format("                    Index = {0},", instrVariant.paramVariant));
                writer.WriteLine(String.Format("                    IsUndocumented = {0},", instrVariant.undocumented));
                writer.WriteLine(String.Format("                    InstructionCodeCount = {0},", instrVariant.numberOfInstructionCodes));
                if (instrVariant.param1List.Length > 0)
                {
                    writer.Write("                    Param1List = new string[] { ");
                    bool isFirstParam = true;
                    foreach (string param in instrVariant.param1List.Split(','))
                    {
                        if (!isFirstParam)
                        {
                            writer.Write(",");
                        }
                        writer.Write(String.Format("\"{0}\"", param));
                        isFirstParam = false;
                    }
                    writer.WriteLine(" },");
                }
                if (instrVariant.param2List.Length > 0)
                {
                    writer.Write("                    Param2List = new string[] { ");
                    bool isFirstParam = true;
                    foreach (string param in instrVariant.param2List.Split(','))
                    {
                        if (!isFirstParam)
                        {
                            writer.Write(",");
                        }
                        writer.Write(String.Format("\"{0}\"", param));
                        isFirstParam = false;
                    }
                    writer.WriteLine(" },");
                }
                if (instrVariant.param3List.Length > 0)
                {
                    writer.Write("                    Param3List = new string[] { ");
                    bool isFirstParam = true;
                    foreach (string param in instrVariant.param3List.Split(','))
                    {
                        if (!isFirstParam)
                        {
                            writer.Write(",");
                        }
                        writer.Write(String.Format("\"{0}\"", param));
                        isFirstParam = false;
                    }
                    writer.WriteLine(" },");
                }
                writer.WriteLine(String.Format("                    InstructionCodeSizeInBytes = {0},", instrVariant.instructionCodeSizeInBytes));
                                
                writer.WriteLine("                    ExecutionTimings = new InstructionExecutionVariant() {");
                GenerateExecutionTimings(writer, instrVariant);
                                
                if (instrVariant.ExecutionVariant != null)
                {
                    writer.WriteLine("                    },");
                    writer.WriteLine("                    AlternateExecutionTimings = new InstructionExecutionVariant() {");
                    GenerateExecutionTimings(writer, instrVariant.ExecutionVariant);
                }
                writer.WriteLine("                    }");

                writer.Write("                }");
                if (instrVariantIndex < paramVariants.Count - 1)
                {
                    writer.WriteLine(",");
                }
                else
                {
                    writer.WriteLine("");
                }
            }

            writer.WriteLine("                },");
            writer.WriteLine("");
            if (instrLine.userManualPage.Length > 0)
            {
                writer.WriteLine(String.Format("                UserManualPage = \"{0}\",", instrLine.userManualPage.Replace("\"", "")));
            }
            writer.Write(String.Format("                Description = \"{0}\"", instrLine.description.Replace("\"", "")));
            if (instrLine.conditionBitsAffected.Length > 0)
            {
                writer.WriteLine(",");
                writer.Write(String.Format("                ConditionBitsAffected = \"{0}\"", instrLine.conditionBitsAffected.Replace("\"", "")));
            }
            if (instrLine.example.Length > 0)
            {
                writer.WriteLine(",");
                writer.Write(String.Format("                Example = \"{0}\"", instrLine.example.Replace("\"", "")));
            }
            writer.WriteLine("");
            writer.WriteLine("            };");
            writer.WriteLine("");
        }

        private void GenerateExecutionTimings(StringWriter writer, InstructionParamVariantLine instrVariant)
        {
            if (instrVariant.execCondition.Length > 0)
            {
                writer.WriteLine(String.Format("                        Condition = \"{0}\",", instrVariant.execCondition));
            }

            int nbMCycles = Int32.Parse(instrVariant.MCycles);
            writer.WriteLine(String.Format("                        MachineCyclesCount = {0},", instrVariant.MCycles));

            string TStatesCount = null;
            int startParentIndex = instrVariant.TStates.IndexOf('(');
            if (startParentIndex >= 0)
            {
                TStatesCount = instrVariant.TStates.Substring(0, startParentIndex - 1).Trim();
            }
            else
            {
                TStatesCount = instrVariant.TStates;
                instrVariant.TStates = "1 (" + instrVariant.TStates + ")";
            }
            writer.WriteLine(String.Format("                        TStatesCount = {0},", TStatesCount));

            writer.WriteLine("                        MachineCycles = new MachineCycle[] {");
            for (int McycleIndex = 1; McycleIndex <= nbMCycles; McycleIndex++)
            {
                string currentMcycle = null;
                switch (McycleIndex)
                {
                    case 1:
                        currentMcycle = instrVariant.M1;
                        break;
                    case 2:
                        currentMcycle = instrVariant.M2;
                        break;
                    case 3:
                        currentMcycle = instrVariant.M3;
                        break;
                    case 4:
                        currentMcycle = instrVariant.M4;
                        break;
                    case 5:
                        currentMcycle = instrVariant.M5;
                        break;
                    case 6:
                        currentMcycle = instrVariant.M6;
                        break;
                }
                startParentIndex = currentMcycle.IndexOf('(');
                string mcycleType = currentMcycle.Substring(0, startParentIndex).Trim();
                string mcycleTStates = currentMcycle.Substring(startParentIndex + 1, 1);

                writer.Write(String.Format("                            new MachineCycle() {{ Type = MachineCycleType.{0}, TStates = {1} }}", mcycleType, mcycleTStates));
                if (McycleIndex < nbMCycles)
                {
                    writer.WriteLine(",");
                }
                else
                {
                    writer.WriteLine(" },");
                }
            }
            writer.Write(String.Format("                        TStatesByMachineCycle = \"{0}\"", instrVariant.TStates));
            if (instrVariant.stackComment.Length > 0)
            {
                writer.WriteLine(",");
                writer.Write(String.Format("                        StackOperations = \"{0}\"", instrVariant.stackComment));
            }
            if (instrVariant.M1comment.Length > 0)
            {
                writer.WriteLine(",");
                writer.Write(String.Format("                        M1Comment = \"{0}\"", instrVariant.M1comment));
            }
            writer.WriteLine("");
        }

        private string ReadAddressingMode(string strAddressingMode)
        {
            switch (strAddressingMode)
            {        
                case "Bit":
                    return "Bit";
                case "Extended":
                    return "Extended";
                case "Flag Cond.":
                    return "FlagCondition";
                case "Flags":
                    return "Flags";
                case "Immediate":
                    return "Immediate";
                case "Immediate Extended":
                    return "Immediate16";
                case "Immediate I/O Port":
                    return "IOPortImmediate";
                case "Indexed":
                    return "Indexed";
                case "Interrupt Mode":
                    return "InterruptMode";
                case "Modified Page Zero":
                    return "ModifiedPageZero";
                case "Register":
                    return "Register";
                case "Register Ext":
                    return "Register16";
                case "Register I/O Port":
                    return "IOPortRegister";
                case "Register Indirect":
                    return "RegisterIndirect";
                case "Relative":
                    return "Relative";
                default:
                    throw new Exception("Adressing mode not supported : "+strAddressingMode);
            }
        }

        public void GenerateOpCodesTable()
        {
            string[] tableNames = new string[] { 
                "InstructionTables/z80_opcodes_table_1byte.csv",
                "InstructionTables/z80_opcodes_table_2bytes_CB.csv",
                "InstructionTables/z80_opcodes_table_2bytes_DD.csv",
                "InstructionTables/z80_opcodes_table_2bytes_ED.csv",
                "InstructionTables/z80_opcodes_table_2bytes_FD.csv",
                "InstructionTables/z80_opcodes_table_4bytes_DDCB.csv",
                "InstructionTables/z80_opcodes_table_4bytes_FDCB.csv" };

            StringWriter writer = new StringWriter();

            writer.WriteLine("using System;");
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine("");
            writer.WriteLine("namespace Z80Simulator.Instructions");
            writer.WriteLine("{");
            writer.WriteLine("    public static class Z80OpCodes");
            writer.WriteLine("    {");
            writer.WriteLine("        /// <summary>");
            writer.WriteLine("        /// Z80 OpCodes tables.");
            writer.WriteLine("        /// ");
            writer.WriteLine("        /// [0,] : 1 byte opcodes table.");
            writer.WriteLine("        /// [1,] : 2 bytes CB prefix opcodes table.");
            writer.WriteLine("        /// [2,] : 2 bytes DD prefix opcodes table.");
            writer.WriteLine("        /// [3,] : 2 bytes ED prefix opcodes table.");
            writer.WriteLine("        /// [4,] : 2 bytes FB prefix opcodes table.");
            writer.WriteLine("        /// [5,] : 4 bytes DDCB prefix opcodes table.");
            writer.WriteLine("        /// [6,] : 4 bytes FDCB prefix opcodes table.");
            writer.WriteLine("        /// </summary>");
            writer.WriteLine("        public static InstructionCode[,] Tables = new InstructionCode[7, 256];");
            writer.WriteLine("");
            writer.WriteLine("        /// <summary>");
            writer.WriteLine("        /// Lookup table used to find an InstructionCode by its InstructionText");
            writer.WriteLine("        /// </summary>");
            writer.WriteLine("        public static IDictionary<string, InstructionCode> Dictionary = new Dictionary<string, InstructionCode>(1792);");
            writer.WriteLine("");
            writer.WriteLine("        static Z80OpCodes()");
            writer.WriteLine("        {");

            for (int tableIndex = 0; tableIndex < 7; tableIndex++)
            {
                Stream stream = PlatformSpecific.GetStreamForProjectFile(tableNames[tableIndex]);
                using (StreamReader reader = new StreamReader(stream))
                {
                    string line = reader.ReadLine();
                    int opcodeIndex = 0;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] columns = line.Split(';');
                        //param 1;param 2;param 3;duplicate

                        string prefix = columns[0];
                        string opcode = columns[1];
                        string opCodeBytes = columns[2].Length == 0 ? "0" : columns[2];
                        string operandBytes = columns[3].Length == 0 ? "0" : columns[3];
                        string undocumented = columns[4] == "1" ? "true" : "false";
                        string fetchMoreBytes = columns[5];
                        string switchToTable = columns[6].Length == 0 ? "null" : columns[6];
                        string text = columns[7];
                        string instrTypeIndex = columns[8].Length == 0 ? "0" : columns[8];
                        string instrParamVariant = columns[9].Length == 0 ? "0" : columns[9];

                        int instructionIndex = Int32.Parse(instrTypeIndex);
                        Z80Simulator.Instructions.InstructionType instruction = Z80Simulator.Instructions.Z80InstructionTypes.Table[instructionIndex];

                        string param1 = columns[10].Length == 0 ? null : GetEnumFromParam(columns[10], instruction.Param1Type, instruction.Index);
                        string param2 = columns[11].Length == 0 ? null : GetEnumFromParam(columns[11], instruction.Param2Type, instruction.Index);
                        string param3 = columns[12].Length == 0 ? null : GetEnumFromParam(columns[12], instruction.Param3Type, instruction.Index);                        
                        
                        string duplicate = columns[13] == "1" ? "true" : "false";

                        writer.WriteLine(String.Format("            Tables[{0}, {1}] = new InstructionCode()", tableIndex, opcodeIndex));
                        writer.WriteLine("            {");
                        byte[] prefixBytes = null;
                        if(prefix.Length == 4)
                        {
                            prefixBytes = new byte[] { Convert.ToByte(prefix.Substring(2, 2), 16) };
                            writer.WriteLine(String.Format("                Prefix = new byte[] {{ {0} }},", prefix));
                        }
                        else if(prefix.Length == 6)
                        {
                            prefixBytes = new byte[] { Convert.ToByte(prefix.Substring(2, 2), 16), Convert.ToByte(prefix.Substring(4, 2), 16) };
                            writer.WriteLine(String.Format("                Prefix = new byte[] {{ {0}, 0x{1} }},", prefix.Substring(0,4), prefix.Substring(4)));
                        }                             
                        writer.WriteLine(String.Format("                OpCode = {0},", opcode));

                        if (duplicate == "true")
                        {
                            string instrCodeBytes = "[";
                            foreach (byte b in prefixBytes)
                            {
                                instrCodeBytes += b.ToString("X2") + "H ";
                            }
                            instrCodeBytes += opcode.Substring(2,2) + "H]";

                            int insertIndex = text.IndexOf(' ');
                            if(insertIndex<0) 
                            {
                                text += instrCodeBytes;
                            }
                            else
                            {
                                text = text.Insert(insertIndex, instrCodeBytes);
                            }
                        }
                        writer.WriteLine(String.Format("                InstructionText = \"{0}\",", text));
                                                
                        if (param1 != null)
                        {
                            writer.WriteLine(String.Format("                Param1 = {0},", param1));
                        }
                        if (param2 != null)
                        {
                            writer.WriteLine(String.Format("                Param2 = {0},", param2));
                        }
                        if (param3 != null)
                        {
                            writer.WriteLine(String.Format("                Param3 = {0},", param3));
                        }
                        writer.WriteLine(String.Format("                InstructionTypeIndex = {0},", instrTypeIndex));
                        writer.WriteLine(String.Format("                InstructionTypeParamVariant = {0},", instrParamVariant));
                        writer.WriteLine(String.Format("                IsUndocumented = {0},", undocumented));
                        writer.WriteLine(String.Format("                IsDuplicate = {0},", duplicate));
                        writer.WriteLine(String.Format("                OpCodeByteCount = {0},", opCodeBytes));
                        writer.WriteLine(String.Format("                OperandsByteCount = {0},", operandBytes));
                        writer.WriteLine(String.Format("                FetchMoreBytes = {0},", fetchMoreBytes));
                        writer.WriteLine(String.Format("                SwitchToTableNumber = {0}", switchToTable));
                        writer.WriteLine("            };");
                        writer.WriteLine("            ");

                        opcodeIndex++;
                        if (opcodeIndex == 256) break;
                    }
                }
            }

            writer.WriteLine("            for (int i = 0; i < 7; i++)");
            writer.WriteLine("            {");
            writer.WriteLine("                for (int j = 0; j < 256; j++)");
            writer.WriteLine("                {");
            writer.WriteLine("                    InstructionCode opcode = Tables[i, j];");
            writer.WriteLine("                    if (opcode.FetchMoreBytes == 0 && opcode.SwitchToTableNumber == null && !opcode.IsDuplicate)");
            writer.WriteLine("                    {");
            writer.WriteLine("                        string text = opcode.InstructionText.Replace(\"nn\", \"n\").Replace(\"e\", \"n\");");
            writer.WriteLine("                        if (!Dictionary.ContainsKey(text))");
            writer.WriteLine("                        {");
            writer.WriteLine("                            Dictionary.Add(text, opcode);");
            writer.WriteLine("                        }");
            writer.WriteLine("                    }");
            writer.WriteLine("                }");
            writer.WriteLine("            }");
            writer.WriteLine("        }");
            writer.WriteLine("    }");
            writer.WriteLine("}");

            string Z80OpCodes_cs = writer.ToString();
        }

        private string GetEnumFromParam(string param, AddressingMode? addressingMode, int instrTypeIndex)
        {
            if (addressingMode == null) return null;
            switch (addressingMode.Value)
            {
                case AddressingMode.Bit:
                    return "Bit.b" + param;
                case AddressingMode.Extended:
                    bool isCallJumpInstruction = instrTypeIndex == 18 || instrTypeIndex == 19 || instrTypeIndex == 54 || instrTypeIndex == 56;
                    if (!isCallJumpInstruction)
                    {
                        return "\"(nn)\"";
                    }
                    else
                    {
                        return "\"nn\"";
                    }
                case AddressingMode.FlagCondition:
                    return "FlagCondition." + param;
                case AddressingMode.Flags:
                    return "Flags.F";
                case AddressingMode.Immediate:
                    return "\"n\"";
                case AddressingMode.Immediate16:
                    return "\"nn\"";
                case AddressingMode.IOPortImmediate:
                    return "\"(n)\"";
                case AddressingMode.IOPortRegister:
                    return "IOPortRegister.C";
                case AddressingMode.Indexed:
                    switch (param)
                    {
                        case "(IX+d)":
                            return "AddressIndexBase.IX";
                        case "(IY+d)":
                            return "AddressIndexBase.IY";
                        default:
                            throw new Exception("Value not allowed");
                    }
                case AddressingMode.InterruptMode:
                    return "InterruptMode.IM" + param;
                case AddressingMode.ModifiedPageZero:
                    switch (param)
                    {
                        case "0H":
                            return "AddressModifiedPageZero.rst00";
                        case "8H":
                            return "AddressModifiedPageZero.rst08";
                        case "10H":
                            return "AddressModifiedPageZero.rst10";
                        case "18H":
                            return "AddressModifiedPageZero.rst18";
                        case "20H":
                            return "AddressModifiedPageZero.rst20";
                        case "28H":
                            return "AddressModifiedPageZero.rst28";
                        case "30H":
                            return "AddressModifiedPageZero.rst30";
                        case "38H":
                            return "AddressModifiedPageZero.rst38";
                        default:
                            throw new Exception("Value not allowed");
                    }
                case AddressingMode.Register:
                    return "Register." + param;
                case AddressingMode.Register16:
                    return "Register16." + param.Replace('\'','2');
                case AddressingMode.RegisterIndirect:
                    return "AddressRegisterIndirect." + param.Substring(1,2);
                case AddressingMode.Relative:
                    return "\"e\"";
                default:
                    throw new Exception("Mode not allowed");
            }
        }

        private string GetParamFromEnum(string strenum, AddressingMode? addressingMode)
        {
            if (addressingMode == null) return null;
            switch (addressingMode.Value)
            {
                case AddressingMode.Bit:
                    return strenum.Substring(1);
                case AddressingMode.Extended:
                    return strenum;
                case AddressingMode.FlagCondition:
                    return strenum;
                case AddressingMode.Flags:
                    return strenum;
                case AddressingMode.Immediate:
                    return strenum;
                case AddressingMode.Immediate16:
                    return strenum;
                case AddressingMode.IOPortImmediate:
                    return strenum;
                case AddressingMode.IOPortRegister:
                    return "("+strenum+")";
                case AddressingMode.Indexed:
                    switch (strenum)
                    {
                        case "IX":
                            return "(IX+d)";
                        case "IY":
                            return "(IY+d)";
                        default:
                            throw new Exception("Value not allowed");
                    }
                case AddressingMode.InterruptMode:
                    return strenum.Substring(2);
                case AddressingMode.ModifiedPageZero:
                    switch (strenum)
                    {
                        case "rst00":
                            return "0H";
                        case "rst08":
                            return "8H";
                        case "rst10":
                            return "10H";
                        case "rst18":
                            return "18H";
                        case "rst20":
                            return "20H";
                        case "rst28":
                            return "28H";
                        case "rst30":
                            return "30H";
                        case "rst38":
                            return "38H";
                        default:
                            throw new Exception("Value not allowed");
                    }
                case AddressingMode.Register:
                    return strenum;
                case AddressingMode.Register16:
                    return strenum.Replace( '2','\'');
                case AddressingMode.RegisterIndirect:
                    return "("+strenum+")";
                case AddressingMode.Relative:
                    return strenum;
                default:
                    throw new Exception("Mode not allowed");
            }
        }

        private class InstructionParamVariantLine
        {
            public string lineType;
            public string instructionIndex;
            public string paramVariant;
            public string numberOfInstructionCodes;
            public string isInternal;
            public string mnemonic;
            public string param1List;
            public string param2List;
            public string param3List;
            public string param1AddressingMode;
            public string param2AddressingMode;
            public string param3AddressingMode;
            public string instructionCodeSizeInBytes;
            public string execVariant;
            public string MCycles;
            public string TStates;
            public string execCondition;
            public string M1;
            public string M2;
            public string M3;
            public string M4;
            public string M5;
            public string M6;
            public string stackComment;
            public string M1comment;
            public string instructionGroup;
            public string undocumented;
            public string userManualPage;
            public string operationSchema;
            public string description;
            public string conditionBitsAffected;
            public string example;

            public InstructionParamVariantLine ExecutionVariant;
        }
    }
}
