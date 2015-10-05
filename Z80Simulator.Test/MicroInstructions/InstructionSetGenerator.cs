using System;
using System.Text;
using Z80Simulator.Instructions;

namespace Z80Simulator.Test.MicroInstructions
{
    public class InstructionSetGenerator
    {        
        public static void GenerateProgramToExecuteAllInstructions()
        {
            StringBuilder program = new StringBuilder();

            // 1. Execute all opcodes 
            int nbTables = Z80OpCodes.Tables.GetLength(0);
            int nbOpCodesPerTable = Z80OpCodes.Tables.GetLength(1);
            int opCodesCount = 0;
            int programSize = 0;
            for (int tableIndex = 0; tableIndex < nbTables; tableIndex++)
            {
                for (int opCodeIndex = 0; opCodeIndex < nbOpCodesPerTable; opCodeIndex++)
                {
                    InstructionCode instrCode = Z80OpCodes.Tables[tableIndex, opCodeIndex];
                    if (instrCode.FetchMoreBytes == 0 && instrCode.SwitchToTableNumber == null)
                    {
                        program.AppendLine(instrCode.InstructionText);
                        opCodesCount++;
                        programSize += instrCode.OpCodeByteCount + instrCode.OperandsByteCount;
                    }
                }
            }

            // 2. Execute all internal instructions
            int nbInstructionTypes = Z80InstructionTypes.Table.Length;
            for (int instrTypeIndex = 1; instrTypeIndex < nbInstructionTypes; instrTypeIndex++)
            {
                InstructionType instructionType = Z80InstructionTypes.Table[instrTypeIndex];
                if (instructionType.IsInternalZ80Operation)
                {
                    switch (instructionType.OpCodeName)
                    {
                        case "NMI":
                            break;
                        case "INT 0":
                            break;
                        case "INT 1":
                            break;
                        case "INT 2":
                            break;
                        case "RESET":
                            break;
                        case "?":
                            break;
                        default:
                            throw new NotImplementedException("No test implemented for internal instruction " + instructionType.OpCodeName);
                    }
                }
            }
        }
    }
}
