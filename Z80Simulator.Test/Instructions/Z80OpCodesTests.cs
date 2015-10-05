using System;
using System.Collections.Generic;
using System.Linq;
using Z80Simulator.Assembly;
using Z80Simulator.Instructions;

namespace Z80Simulator.Test.Instructions
{
    public static class Z80OpCodesTests
    {
        /// <summary>
        /// Check instruction InstructionCodeSizeInBytes vs instruction codes : opcode + operands bytes.
        /// </summary>          
        public static IList<InstructionCode> CheckInstructionCodeSizeInBytes()
        {
            IList<InstructionCode> opcodesList = new List<InstructionCode>();
            for (int table = 0; table < 7; table++ )
            {
                for (int opcode = 0; opcode < 256; opcode++)
                {
                    InstructionCode instrCode = Z80OpCodes.Tables[table, opcode];
                    if (instrCode.FetchMoreBytes == 0 && instrCode.SwitchToTableNumber == null)
                    {
                        InstructionParametersVariant instrVariant = Z80InstructionTypes.Table[instrCode.InstructionTypeIndex].ParametersVariants[instrCode.InstructionTypeParamVariant];
                        if (instrCode.OpCodeByteCount + instrCode.OperandsByteCount != instrVariant.InstructionCodeSizeInBytes)
                        {
                            throw new Exception("Problem with opcode " + table + "," + opcode + " : " + instrCode.InstructionText + " vs instruction type " + instrCode.InstructionTypeIndex + "," + instrVariant.Index + " : " + (instrCode.OpCodeByteCount + instrCode.OperandsByteCount) + " vs " + instrVariant.InstructionCodeSizeInBytes);
                        }
                        opcodesList.Add(instrCode);
                    }
                }
            }
            return opcodesList;
        }

        /// <summary>
        /// Check instruction InstructionCodeCount vs actual number of non duplicate instruction codes linked to the instruction.
        /// </summary>
        public static void CheckInstructionCodeCount(IList<InstructionCode> opcodesList)
        {
            foreach(InstructionType instrType in Z80InstructionTypes.Table)
            {
                if (instrType == null) continue;
                foreach(InstructionParametersVariant instrVariant in instrType.ParametersVariants)
                {
                    if ((instrType.Index == 66 && instrVariant.Index == 2) || (instrType.Index == 68 && instrVariant.Index == 2) || (instrType.Index == 78 && instrVariant.Index == 1) || (instrType.Index == 162 || instrType.Index == 163)) continue;
                    int nbCodes = (from instrCode in opcodesList
                                   where instrCode.InstructionTypeIndex == instrType.Index && instrCode.InstructionTypeParamVariant == instrVariant.Index
                                         && !instrCode.IsDuplicate
                                   select instrCode).Count();
                    if (instrVariant.InstructionCodeCount != nbCodes)
                    {
                        throw new Exception("Problem with instruction "+instrType.Index+","+instrVariant.Index+" : "+instrVariant.InstructionCodeCount+" opcodes expected, "+nbCodes+" found");
                    }
                }
            }
        }

        /// <summary>
        /// Check instruction code text vs instruction code parameters.
        /// Check instruction code opcode name vs instruction opcode name.
        /// Check instruction code parameters vs instruction parameters list.
        /// </summary>
        public static void CheckInstructionCodeNameAndParameters(IList<InstructionCode> opcodesList)
        {
            foreach (InstructionCode instrCode in opcodesList)
            {
                InstructionType instrType =  Z80InstructionTypes.Table[instrCode.InstructionTypeIndex];
                InstructionParametersVariant instrParams = instrType.ParametersVariants[instrCode.InstructionTypeParamVariant];

                int endOfOpcodeName = instrCode.InstructionText.IndexOf('[');
                if (endOfOpcodeName < 0) endOfOpcodeName = instrCode.InstructionText.IndexOf(' ');

                int startOfParamsText = instrCode.InstructionText.IndexOf(' ', instrCode.InstructionText.IndexOf(']') + 1);
                if (startOfParamsText > 0 || instrType.Param1Type != null)
                {
                    if (instrCode.InstructionText.Substring(0, endOfOpcodeName) != instrType.OpCodeName)
                    {
                        throw new Exception("Opcode name problem problem between instruction " + instrType.Index + " and opcode " + instrCode.OpCode);
                    }
                    string paramsFromInstructionText = instrCode.InstructionText.Substring(startOfParamsText + 1);
                    string paramsText = "";
                    if (instrCode.Param1 != null)
                    {
                        string param1 = Disassembler.GetParamTextFromEnumStringAndOperand(instrType, instrType.Param1Type.Value, instrCode.Param1.ToString(), true, 0, 0);
                        paramsText = param1;
                        if (!instrParams.Param1List.Contains<string>(param1))
                        {
                            throw new Exception("Parameter 1 of opcode " + instrCode.InstructionText + " is not in parameters list of instruction variant " + instrType.Index + "," + instrParams.Index);
                        }
                    }
                    if (instrCode.Param2 != null)
                    {
                        string param2 = Disassembler.GetParamTextFromEnumStringAndOperand(instrType, instrType.Param2Type.Value, instrCode.Param2.ToString(), true, 0, 0);
                        paramsText += ","+param2;
                        if (!instrParams.Param2List.Contains<string>(param2))
                        {
                            throw new Exception("Parameter 2 of opcode " + instrCode.InstructionText + " is not in parameters list of instruction variant " + instrType.Index + "," + instrParams.Index);
                        }
                    }
                    if (instrCode.Param3 != null)
                    {
                        string param3 = Disassembler.GetParamTextFromEnumStringAndOperand(instrType, instrType.Param3Type.Value, instrCode.Param3.ToString(), true, 0, 0); ;
                        paramsText += ","+param3;
                        if (!instrParams.Param3List.Contains<string>(param3))
                        {
                            throw new Exception("Parameter 3 of opcode " + instrCode.InstructionText + " is not in parameters list of instruction variant " + instrType.Index + "," + instrParams.Index);
                        }
                    }
                    if (paramsText != paramsFromInstructionText)
                    {
                        throw new Exception("Opcode instruction text " + instrCode.InstructionText + " does not match parameters of opcode " + instrCode.OpCode);
                    }
                }
                else
                {
                    string opcodeName = instrCode.InstructionText;
                    if (endOfOpcodeName > 0)
                    {
                        opcodeName = instrCode.InstructionText.Substring(0, endOfOpcodeName);
                    }
                    if (opcodeName != instrType.OpCodeName)
                    {
                        throw new Exception("Opcode name problem problem between instruction " + instrType.Index + " and opcode " + instrCode.OpCode);
                    }
                }                
            }
        }        
    }
}
