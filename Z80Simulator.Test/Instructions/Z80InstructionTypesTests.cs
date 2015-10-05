using System;
using System.Collections.Generic;
using System.Linq;
using Z80Simulator.Instructions;

namespace Z80Simulator.Test.Instructions
{
    public static class Z80InstructionTypesTests
    {
        /// <summary>
        /// Check that each instruction parameter list is associated with a parameter type (and reciprocally).
        /// Check that the number of parameters in the parameter lists matches the number of instruction codes.
        /// </summary>
        public static void CheckInstructionParametersList()
        {
            foreach (InstructionType instrType in Z80InstructionTypes.Table)
            {
                if (instrType == null) continue;
                foreach (InstructionParametersVariant instrVariant in instrType.ParametersVariants)
                {
                    if ((instrType.Index == 86 && instrVariant.Index == 0) || instrVariant.InstructionCodeSizeInBytes == 0) continue;
                    if ((instrType.Param1Type == null && instrVariant.Param1List != null) ||
                        (instrType.Param1Type != null && instrVariant.Param1List == null))
                    {
                        throw new Exception("Param type 1 vs param list 1 problem with instruction " + instrType.Index + "," + instrVariant.Index);
                    }
                    if ((instrType.Param2Type == null && instrVariant.Param2List != null) ||
                        (instrType.Param2Type != null && instrVariant.Param2List == null))
                    {
                        throw new Exception("Param type 2 vs param list 2 problem with instruction " + instrType.Index + "," + instrVariant.Index);
                    }
                    if ((instrType.Param3Type == null && instrVariant.Param3List != null) ||
                        (instrType.Param3Type != null && instrVariant.Param3List == null))
                    {
                        throw new Exception("Param type 3 vs param list 3 problem with instruction " + instrType.Index + "," + instrVariant.Index);
                    }
                    int nbOpcodes = 1;
                    if (instrType.Param1Type != null)
                    {
                        nbOpcodes = instrVariant.Param1List.Length;
                    }
                    if (instrType.Param2Type != null)
                    {
                        nbOpcodes *= instrVariant.Param2List.Length;
                    }
                    if (instrType.Param3Type != null)
                    {
                        nbOpcodes *= instrVariant.Param3List.Length;
                    }
                    if (nbOpcodes != instrVariant.InstructionCodeCount)
                    {
                        throw new Exception("Number of opcodes vs param lists problem with instruction " + instrType.Index + "," + instrVariant.Index);
                    }
                }
            }
        }

        /// <summary>
        /// Check instruction number of Mcycles and number of Tstates vs the table of the type and duration of the Mcycles.
        /// Check MCycles type, order and minimum duration.
        /// </summary>
        public static void CheckInstructionMachineCycles()
        {
            foreach (InstructionType instrType in Z80InstructionTypes.Table)
            {
                if (instrType == null || instrType.IsInternalZ80Operation) continue;
                foreach (InstructionParametersVariant instrVariant in instrType.ParametersVariants)
                {
                    CheckTimings(instrType.Index + "," + instrVariant.Index, instrVariant.ExecutionTimings);
                    if (instrVariant.AlternateExecutionTimings != null) CheckTimings(instrType.Index + "," + instrVariant.Index, instrVariant.AlternateExecutionTimings);
                }
            }
        }

        private static void CheckTimings(string instrIds, InstructionExecutionVariant instructionExecutionVariant)
        {
            int tstates = 0;
            for (int mcycle = 1; mcycle <= instructionExecutionVariant.MachineCyclesCount; mcycle++)
            {
                tstates += instructionExecutionVariant.MachineCycles[mcycle - 1].TStates;
                // Check machine cycle types
                // 1. OCF or IN is mandatory for cycle 1
                if (mcycle == 1 && instructionExecutionVariant.MachineCycles[mcycle - 1].Type != MachineCycleType.OCF) throw new Exception("Instruction does not start with OCF " + instrIds);
                // 2. OCF is forbidden after cycle 2 unless in case of four bytes opcodes
                if (mcycle > 2 && instructionExecutionVariant.MachineCycles[mcycle - 1].Type == MachineCycleType.OCF && (mcycle != 4 || instructionExecutionVariant.MachineCycles[mcycle - 2].Type != MachineCycleType.OD)) throw new Exception("Too many OCF in instruction " + instrIds);
                // 3. ODL is always followed by ODH
                if (instructionExecutionVariant.MachineCycles[mcycle - 1].Type == MachineCycleType.ODL && instructionExecutionVariant.MachineCycles[mcycle].Type != MachineCycleType.ODH) throw new Exception("ODL is always followed by ODH " + instrIds);
                // 4. MWL is always followed by MWH
                if (instructionExecutionVariant.MachineCycles[mcycle - 1].Type == MachineCycleType.MWL && instructionExecutionVariant.MachineCycles[mcycle].Type != MachineCycleType.MWH) throw new Exception("MWL is always followed by MWH " + instrIds);
                // 5. MRL is always followed by MRH
                if (instructionExecutionVariant.MachineCycles[mcycle - 1].Type == MachineCycleType.MRL && instructionExecutionVariant.MachineCycles[mcycle].Type != MachineCycleType.MRH) throw new Exception("MRL is always followed by MRH " + instrIds);
                // 6. SWH is always followed by SWL
                if (instructionExecutionVariant.MachineCycles[mcycle - 1].Type == MachineCycleType.SWH && instructionExecutionVariant.MachineCycles[mcycle].Type != MachineCycleType.SWL) throw new Exception("SWH is always followed by SWL " + instrIds);
                // 7. SRL is always followed by SRH
                if (instructionExecutionVariant.MachineCycles[mcycle - 1].Type == MachineCycleType.SRL && instructionExecutionVariant.MachineCycles[mcycle].Type != MachineCycleType.SRH) throw new Exception("SRL is always followed by SRH " + instrIds);
                // 8. Check minimum duration
                if ((instructionExecutionVariant.MachineCycles[mcycle - 1].Type == MachineCycleType.OCF ||
                    instructionExecutionVariant.MachineCycles[mcycle - 1].Type == MachineCycleType.PR ||
                    instructionExecutionVariant.MachineCycles[mcycle - 1].Type == MachineCycleType.PW) &&
                    instructionExecutionVariant.MachineCycles[mcycle - 1].TStates < 4)
                {
                    throw new Exception("OCF,PR,PW must last at least 4 Tstates " + instrIds);
                }
                else if (instructionExecutionVariant.MachineCycles[mcycle - 1].TStates < 3)
                {
                    throw new Exception("Machine cycles must last at least 4 Tstates " + instrIds);
                }
                else if (instructionExecutionVariant.MachineCycles[mcycle - 1].TStates > 6)
                {
                    throw new Exception("Machine cycles can last at most 6 Tstates " + instrIds);
                }
            }
            if (tstates != instructionExecutionVariant.TStatesCount)
            {
                throw new Exception("Problem of timings with instruction " + instrIds);
            }
        }


        /// <summary>
        /// Check undocumented status coherence between instructions, instruction variants and opcodes.
        /// </summary>
        public static void CheckInstructionUndocumentedStatus(IList<InstructionCode> opcodesList)
        {
            foreach (InstructionType instrType in Z80InstructionTypes.Table)
            {
                if (instrType == null) continue;

                if (instrType.IsUndocumented && instrType.UserManualPage != null)
                {
                    throw new Exception("Instruction type " + instrType.Index + " is undocumented but is associated to a user manal page");
                }
                if (instrType.ParametersVariants != null && instrType.ParametersVariants.Length > 0)
                {
                    int nbDocumentedVariants = (from instrVariant in instrType.ParametersVariants
                                                where !instrVariant.IsUndocumented
                                                select instrVariant).Count();
                    if (instrType.IsUndocumented && nbDocumentedVariants > 0)
                    {
                        throw new Exception("Instruction type " + instrType.Index + " is undocumented but at least one of its variants is documented");
                    }
                    else if (!instrType.IsUndocumented && nbDocumentedVariants == 0)
                    {
                        throw new Exception("Instruction type " + instrType.Index + " is documented but none of its variants is documented");
                    }
                }
                if (instrType.ParametersVariants != null && instrType.ParametersVariants.Length > 1)
                {
                    foreach (InstructionParametersVariant instrVariant in instrType.ParametersVariants)
                    {
                        int nbDocumentedCodes = (from instrCode in opcodesList
                                                 where instrCode.InstructionTypeIndex == instrType.Index && instrCode.InstructionTypeParamVariant == instrVariant.Index
                                                     && !instrCode.IsUndocumented
                                                 select instrCode).Count();
                        if (instrVariant.IsUndocumented && nbDocumentedCodes > 0)
                        {
                            throw new Exception("Instruction variant " + instrType.Index + "," + instrVariant.Index + " is undocumented but at least one of its opcodes is documented");
                        }
                        else if (!instrVariant.IsUndocumented && nbDocumentedCodes == 0)
                        {
                            throw new Exception("Instruction variant " + instrType.Index + "," + instrVariant.Index + " is documented but none of its opcodes is documented");
                        }
                    }
                }
            }
        }
    }
}
