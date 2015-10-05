using System;

namespace Z80Simulator.Instructions
{
    public static class Z80InstructionTypes
    {
        /// <summary>
        /// Z80 Instruction Types tables.
        /// To be user friendly in the documentation, the first instruction type is found at index 1.
        /// </summary>
        public static InstructionType[] Table = new InstructionType[163];

        static Z80InstructionTypes()
        {
            Table[1] = new InstructionType()
            {
                Index = 1,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "ADC",
                Param1Type = AddressingMode.Register,
                Param2Type = AddressingMode.Immediate,
                Equation = "A <- A + s + CY",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "A" },
                    Param2List = new string[] { "n" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 7,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 } },
                        TStatesByMachineCycle = "7 (4, 3)"
                    }
                }
                },

                UserManualPage = "146",
                Description = "This s operand is any of r, n, (HL), (IX+d), or (lY+d) as defined for the\nanalogous ADD instruction.\nThe s operand, along with the Carry Flag (C in the F register) is added to the\ncontents of the Accumulator, and the result is stored in the Accumulator.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if carry from bit 3, reset otherwise\nP/V is set if overflow, reset otherwise\nN is reset\nC is set if carry from bit 7: reset otherwise",
                Example = "If the Accumulator contents are 16H, the Carry Flag is set, the HL register\npair contains 6666H, and address 6666H contains 10H, at execution of\nADC A, (HL) the Accumulator contains 27H."
            };

            Table[2] = new InstructionType()
            {
                Index = 2,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "ADC",
                Param1Type = AddressingMode.Register,
                Param2Type = AddressingMode.Register,
                Equation = "A <- A + s + CY",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 7,
                    Param1List = new string[] { "A" },
                    Param2List = new string[] { "A","B","C","D","E","H","L" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 1,
                        TStatesCount = 4,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "1 (4)"
                    }
                },
                new InstructionParametersVariant() {
                    Index = 1,
                    IsUndocumented = true,
                    InstructionCodeCount = 4,
                    Param1List = new string[] { "A" },
                    Param2List = new string[] { "IXh","IXl","IYh","IYl" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 8,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "8 (4, 4)"
                    }
                }
                },

                UserManualPage = "146",
                Description = "This s operand is any of r, n, (HL), (IX+d), or (lY+d) as defined for the\nanalogous ADD instruction.\nThe s operand, along with the Carry Flag (C in the F register) is added to the\ncontents of the Accumulator, and the result is stored in the Accumulator.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if carry from bit 3, reset otherwise\nP/V is set if overflow, reset otherwise\nN is reset\nC is set if carry from bit 7: reset otherwise",
                Example = "If the Accumulator contents are 16H, the Carry Flag is set, the HL register\npair contains 6666H, and address 6666H contains 10H, at execution of\nADC A, (HL) the Accumulator contains 27H."
            };

            Table[3] = new InstructionType()
            {
                Index = 3,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "ADC",
                Param1Type = AddressingMode.Register,
                Param2Type = AddressingMode.RegisterIndirect,
                Equation = "A <- A + s + CY",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "A" },
                    Param2List = new string[] { "(HL)" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 7,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 } },
                        TStatesByMachineCycle = "7 (4, 3)"
                    }
                }
                },

                UserManualPage = "146",
                Description = "This s operand is any of r, n, (HL), (IX+d), or (lY+d) as defined for the\nanalogous ADD instruction.\nThe s operand, along with the Carry Flag (C in the F register) is added to the\ncontents of the Accumulator, and the result is stored in the Accumulator.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if carry from bit 3, reset otherwise\nP/V is set if overflow, reset otherwise\nN is reset\nC is set if carry from bit 7: reset otherwise",
                Example = "If the Accumulator contents are 16H, the Carry Flag is set, the HL register\npair contains 6666H, and address 6666H contains 10H, at execution of\nADC A, (HL) the Accumulator contains 27H."
            };

            Table[4] = new InstructionType()
            {
                Index = 4,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "ADC",
                Param1Type = AddressingMode.Register,
                Param2Type = AddressingMode.Indexed,
                Equation = "A <- A + s + CY",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 2,
                    Param1List = new string[] { "A" },
                    Param2List = new string[] { "(IX+d)","(IY+d)" },
                    InstructionCodeSizeInBytes = 3,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 5,
                        TStatesCount = 19,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 } },
                        TStatesByMachineCycle = "19 (4, 4, 3, 5, 3)"
                    }
                }
                },

                UserManualPage = "146",
                Description = "This s operand is any of r, n, (HL), (IX+d), or (lY+d) as defined for the\nanalogous ADD instruction.\nThe s operand, along with the Carry Flag (C in the F register) is added to the\ncontents of the Accumulator, and the result is stored in the Accumulator.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if carry from bit 3, reset otherwise\nP/V is set if overflow, reset otherwise\nN is reset\nC is set if carry from bit 7: reset otherwise",
                Example = "If the Accumulator contents are 16H, the Carry Flag is set, the HL register\npair contains 6666H, and address 6666H contains 10H, at execution of\nADC A, (HL) the Accumulator contains 27H."
            };

            Table[5] = new InstructionType()
            {
                Index = 5,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "16-Bit Arithmetic",
                OpCodeName = "ADC",
                Param1Type = AddressingMode.Register16,
                Param2Type = AddressingMode.Register16,
                Equation = "HL <- HL + ss + CY",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 4,
                    Param1List = new string[] { "HL" },
                    Param2List = new string[] { "BC","DE","HL","SP" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 4,
                        TStatesCount = 15,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 3 } },
                        TStatesByMachineCycle = "15 (4, 4, 4, 3)"
                    }
                }
                },

                UserManualPage = "180",
                Description = "The contents of register pair ss (any of register pairs BC, DE, HL, or SP)\nare added with the Carry flag (C flag in the F register) to the contents of\nregister pair HL, and the result is stored in HL.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nR is set if carry out of bit 11,. reset otherwise\nP/V is set if overflow, reset otherwise\nN is reset\nC is set if carry from bit 15, reset otherwise",
                Example = "If the register pair BC contains 2222H, register pair HL contains 5437H,\nand the Carry Flag is set, at execution of ADC HL, BC the contents of\nHL are 765AH."
            };

            Table[6] = new InstructionType()
            {
                Index = 6,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "ADD",
                Param1Type = AddressingMode.Register,
                Param2Type = AddressingMode.Immediate,
                Equation = "A <- A + n",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "A" },
                    Param2List = new string[] { "n" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 7,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 } },
                        TStatesByMachineCycle = "7 (4, 3)"
                    }
                }
                },

                UserManualPage = "142",
                Description = "The integer n is added to the contents of the Accumulator, and the results\nare stored in the Accumulator.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if carry from bit 3, reset otherwise\nP/V is set if overflow, reset otherwise\nN is reset\nC is set if carry from bit 7, reset otherwise",
                Example = "If the contents of the Accumulator are 23H, at execution of ADD A, 33H\nthe contents of the Accumulator are 56H."
            };

            Table[7] = new InstructionType()
            {
                Index = 7,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "ADD",
                Param1Type = AddressingMode.Register,
                Param2Type = AddressingMode.Register,
                Equation = "A <- A + r",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 7,
                    Param1List = new string[] { "A" },
                    Param2List = new string[] { "A","B","C","D","E","H","L" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 1,
                        TStatesCount = 4,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "1 (4)"
                    }
                },
                new InstructionParametersVariant() {
                    Index = 1,
                    IsUndocumented = true,
                    InstructionCodeCount = 4,
                    Param1List = new string[] { "A" },
                    Param2List = new string[] { "IXh","IXl","IYh","IYl" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 8,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "8 (4, 4)"
                    }
                }
                },

                UserManualPage = "140",
                Description = "The contents of register r are added to the contents of the Accumulator, and\nthe result is stored in the Accumulator. The symbol r identifies the registers\nA, B, C, D, E, H, or L.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if carry from bit 3, reset otherwise\nP/V is set if overflow, reset otherwise\nN is reset\nC is set if carry from bit 7, reset otherwise",
                Example = "If the contents of the Accumulator are 44H, and the contents of register C\nare 11H, at execution of ADD A,C the contents of the Accumulator are\n55H."
            };

            Table[8] = new InstructionType()
            {
                Index = 8,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "ADD",
                Param1Type = AddressingMode.Register,
                Param2Type = AddressingMode.RegisterIndirect,
                Equation = "A <- A + (HL)",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "A" },
                    Param2List = new string[] { "(HL)" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 7,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 } },
                        TStatesByMachineCycle = "7 (4, 3)"
                    }
                }
                },

                UserManualPage = "143",
                Description = "The byte at the memory address specified by the contents of the HL register\npair is added to the contents of the Accumulator, and the result is stored in\nthe Accumulator.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if carry from bit 3, reset otherwise\nP/V is set if overflow, reset otherwise\nN is reset\nC is set if carry from bit 7, reset otherwise",
                Example = "If the contents of the Accumulator are A0H, and the content of the register\npair HL is 2323H, and memory location 2323H contains byte 08H, at\nexecution of ADD A, (HL) the Accumulator contains A8H."
            };

            Table[9] = new InstructionType()
            {
                Index = 9,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "ADD",
                Param1Type = AddressingMode.Register,
                Param2Type = AddressingMode.Indexed,
                Equation = "A <- A + (IX+d)",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 2,
                    Param1List = new string[] { "A" },
                    Param2List = new string[] { "(IX+d)","(IY+d)" },
                    InstructionCodeSizeInBytes = 3,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 5,
                        TStatesCount = 19,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 } },
                        TStatesByMachineCycle = "19 (4, 4, 3, 5, 3)"
                    }
                }
                },

                UserManualPage = "144\n145",
                Description = "The contents of the Index Register (register pair IX) is added to a two�s\ncomplement displacement d to point to an address in memory. The contents\nof this address is then added to the contents of the Accumulator and the\nresult is stored in the Accumulator.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if carry from bit 3, reset otherwise\nP/V is set if overflow, reset otherwise\nN is reset\nC is set if carry from bit 7, reset otherwise",
                Example = "If the Accumulator contents are 11H, the Index Register IX contains\n1000H, and if the contents of memory location 1005H is 22H, at execution\nof ADD A, (IX + 5H) the contents of the Accumulator are 33H."
            };

            Table[10] = new InstructionType()
            {
                Index = 10,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "16-Bit Arithmetic",
                OpCodeName = "ADD",
                Param1Type = AddressingMode.Register16,
                Param2Type = AddressingMode.Register16,
                Equation = "HL <- HL + ss",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 4,
                    Param1List = new string[] { "HL" },
                    Param2List = new string[] { "BC","DE","HL","SP" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 3,
                        TStatesCount = 11,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 3 } },
                        TStatesByMachineCycle = "11 (4, 4, 3)"
                    }
                },
                new InstructionParametersVariant() {
                    Index = 1,
                    IsUndocumented = false,
                    InstructionCodeCount = 4,
                    Param1List = new string[] { "IX" },
                    Param2List = new string[] { "BC","DE","IX","SP" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 4,
                        TStatesCount = 15,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 3 } },
                        TStatesByMachineCycle = "15 (4, 4, 4, 3)"
                    }
                },
                new InstructionParametersVariant() {
                    Index = 2,
                    IsUndocumented = false,
                    InstructionCodeCount = 4,
                    Param1List = new string[] { "IY" },
                    Param2List = new string[] { "BC","DE","IY","SP" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 4,
                        TStatesCount = 15,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 3 } },
                        TStatesByMachineCycle = "15 (4, 4, 4, 3)"
                    }
                }
                },

                UserManualPage = "179",
                Description = "The contents of register pair ss (any of register pairs BC, DE, HL, or SP)\nare added to the contents of register pair HL and the result is stored in HL.",
                ConditionBitsAffected = "S is not affected\nZ is not affected\nH is set if carry out of bit 11, reset otherwise\nP/V is not affected\nN is reset\nC is set if carry from bit 15, reset otherwise",
                Example = "If register pair HL contains the integer 4242H, and register pair DE contains\n1111H, at execution of ADD HL, DE the HL register pair contains 5353H."
            };

            Table[11] = new InstructionType()
            {
                Index = 11,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "AND",
                Param1Type = AddressingMode.Immediate,
                Equation = "A <- A & s",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "n" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 7,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 } },
                        TStatesByMachineCycle = "7 (4, 3)"
                    }
                }
                },

                UserManualPage = "152",
                Description = "A logical AND operation is performed between the byte specified by the s\noperand and the byte contained in the Accumulator, the result is stored in\nthe Accumulator.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set\nP/V is reset if overflow, reset otherwise\nN is reset\nC is reset",
                Example = "If the B register contains 7BH (0111 1011), and the Accumulator contains\nC3H (1100 0011), at execution of AND B the Accumulator contains 43H\n(0100 0011)."
            };

            Table[12] = new InstructionType()
            {
                Index = 12,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "AND",
                Param1Type = AddressingMode.Register,
                Equation = "A <- A & s",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 7,
                    Param1List = new string[] { "A","B","C","D","E","H","L" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 1,
                        TStatesCount = 4,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "1 (4)"
                    }
                },
                new InstructionParametersVariant() {
                    Index = 1,
                    IsUndocumented = true,
                    InstructionCodeCount = 4,
                    Param1List = new string[] { "IXh","IXl","IYh","IYl" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 8,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "8 (4, 4)"
                    }
                }
                },

                UserManualPage = "152",
                Description = "A logical AND operation is performed between the byte specified by the s\noperand and the byte contained in the Accumulator, the result is stored in\nthe Accumulator.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set\nP/V is reset if overflow, reset otherwise\nN is reset\nC is reset",
                Example = "If the B register contains 7BH (0111 1011), and the Accumulator contains\nC3H (1100 0011), at execution of AND B the Accumulator contains 43H\n(0100 0011)."
            };

            Table[13] = new InstructionType()
            {
                Index = 13,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "AND",
                Param1Type = AddressingMode.RegisterIndirect,
                Equation = "A <- A & s",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "(HL)" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 7,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 } },
                        TStatesByMachineCycle = "7 (4, 3)"
                    }
                }
                },

                UserManualPage = "152",
                Description = "A logical AND operation is performed between the byte specified by the s\noperand and the byte contained in the Accumulator, the result is stored in\nthe Accumulator.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set\nP/V is reset if overflow, reset otherwise\nN is reset\nC is reset",
                Example = "If the B register contains 7BH (0111 1011), and the Accumulator contains\nC3H (1100 0011), at execution of AND B the Accumulator contains 43H\n(0100 0011)."
            };

            Table[14] = new InstructionType()
            {
                Index = 14,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "AND",
                Param1Type = AddressingMode.Indexed,
                Equation = "A <- A & s",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 2,
                    Param1List = new string[] { "(IX+d)","(IY+d)" },
                    InstructionCodeSizeInBytes = 3,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 5,
                        TStatesCount = 19,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 } },
                        TStatesByMachineCycle = "19 (4, 4, 3, 5, 3)"
                    }
                }
                },

                UserManualPage = "152",
                Description = "A logical AND operation is performed between the byte specified by the s\noperand and the byte contained in the Accumulator, the result is stored in\nthe Accumulator.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set\nP/V is reset if overflow, reset otherwise\nN is reset\nC is reset",
                Example = "If the B register contains 7BH (0111 1011), and the Accumulator contains\nC3H (1100 0011), at execution of AND B the Accumulator contains 43H\n(0100 0011)."
            };

            Table[15] = new InstructionType()
            {
                Index = 15,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Bit Set, Reset, and Test",
                OpCodeName = "BIT",
                Param1Type = AddressingMode.Bit,
                Param2Type = AddressingMode.Register,
                Equation = "Z <- rb",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 56,
                    Param1List = new string[] { "0","1","2","3","4","5","6","7" },
                    Param2List = new string[] { "A","B","C","D","E","H","L" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 8,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "8 (4, 4)"
                    }
                }
                },

                UserManualPage = "224",
                Description = "This instruction tests bit b in register r and sets the Z flag accordingly.",
                ConditionBitsAffected = "S is unknown\nZ is set if specified bit is 0, reset otherwise\nH is set\nP/V is unknown\nN is reset\nC is not affected",
                Example = "If bit 2 in register B contains 0, at execution of BIT 2, B the Z flag in the\nF register contains 1, and bit 2 in register B remains 0. Bit 0 in register B is\nthe least-significant bit."
            };

            Table[16] = new InstructionType()
            {
                Index = 16,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Bit Set, Reset, and Test",
                OpCodeName = "BIT",
                Param1Type = AddressingMode.Bit,
                Param2Type = AddressingMode.RegisterIndirect,
                Equation = "Z <- (HL)b",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 8,
                    Param1List = new string[] { "0","1","2","3","4","5","6","7" },
                    Param2List = new string[] { "(HL)" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 3,
                        TStatesCount = 12,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 } },
                        TStatesByMachineCycle = "12 (4, 4, 4)"
                    }
                }
                },

                UserManualPage = "226",
                Description = "This instruction tests bit b in the memory location specified by the contents\nof the HL register pair and sets the Z flag accordingly.",
                ConditionBitsAffected = "S is unknown\nZ is set if specified Bit is 0, reset otherwise\nH is set\nP/V is unknown\nH is reset\nC is not affected",
                Example = "If the HL register pair contains 4444H, and bit 4 in the memory location\n444H contains 1, at execution of BIT 4, (HL) the Z flag in the F register\ncontains 0, and bit 4 in memory location 4444H still contains 1. Bit 0 in\nmemory location 4444H is the least-significant bit."
            };

            Table[17] = new InstructionType()
            {
                Index = 17,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Bit Set, Reset, and Test",
                OpCodeName = "BIT",
                Param1Type = AddressingMode.Bit,
                Param2Type = AddressingMode.Indexed,
                Equation = "Z <- (IX+d)b",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 16,
                    Param1List = new string[] { "0","1","2","3","4","5","6","7" },
                    Param2List = new string[] { "(IX+d)","(IY+d)" },
                    InstructionCodeSizeInBytes = 4,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 5,
                        TStatesCount = 20,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.OC4, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 } },
                        TStatesByMachineCycle = "20 (4, 4, 3, 5, 4)"
                    }
                }
                },

                UserManualPage = "228\n230",
                Description = "This instruction tests bit b in the memory location specified by the contents\nof register pair IX combined with the two�s complement displacement d\nand sets the Z flag accordingly. Operand b is specified as follows in the\nassembled object code.",
                ConditionBitsAffected = "S is unknown\nZ is set if specified Bit is 0, reset otherwise\nH is set\nP/V is unknown\nN is reset\nC is not affected",
                Example = "If the contents of Index Register IX are 2000H, and bit 6 in memory\nlocation 2004H contains 1, at execution of BIT 6, (IX+4H) the Z flag in\nthe F register contains 0, and bit 6 in memory location 2004H still contains\n1. Bit 0 in memory location 2004H is the least-significant bit."
            };

            Table[18] = new InstructionType()
            {
                Index = 18,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Call And Return",
                OpCodeName = "CALL",
                Param1Type = AddressingMode.Extended,
                Equation = "(SP-1) <- PCH, (SP-2) <- PCL, PC <- nn",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "nn" },
                    InstructionCodeSizeInBytes = 3,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 5,
                        TStatesCount = 17,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.ODL, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.ODH, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.SWH, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.SWL, TStates = 3 } },
                        TStatesByMachineCycle = "17 (4, 3, 4, 3, 3)",
                        StackOperations = "M3 -1, M4 -1"
                    }
                }
                },

                UserManualPage = "255",
                Description = "The current contents of the Program Counter (PC) are pushed onto the top\nof the external memory stack. The operands nn are then loaded to the PC to\npoint to the address in memory where the first Op Code of a subroutine is to\nbe fetched. At the end of the subroutine, a RETurn instruction can be used\nto return to the original program flow by popping the top of the stack back\nto the PC. The push is accomplished by first decrementing the current\ncontents of the Stack Pointer (register pair SP), loading the high-order byte\nof the PC contents to the memory address now pointed to by the SP, then\ndecrementing SP again, and loading the low order byte of the PC contents\nto the top of stack.\nBecause this is a 3-byte instruction, the Program Counter was incremented\nby three before the push is executed.",
                ConditionBitsAffected = "None",
                Example = "If the contents of the Program Counter are 1A47H, the contents of the Stack\nPointer are 3002H, and memory locations have the contents:\n1A47H contains CDH\nIA48H contains 35H\n1A49H contains 21H\nIf an instruction fetch sequence begins, the 3-byte instruction CD3521H is\nfetched to the CPU for execution. The mnemonic equivalent of this is CALL\n2135H. At execution of this instruction, the contents of memory address\n3001H is 1AH, the contents of address 3000H is 4AH, the contents of the\nStack Pointer is 3000H, and the contents of the Program Counter is 2135H,\npointing to the address of the first Op Code of the subroutine now to be\nexecuted."
            };

            Table[19] = new InstructionType()
            {
                Index = 19,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Call And Return",
                OpCodeName = "CALL",
                Param1Type = AddressingMode.FlagCondition,
                Param2Type = AddressingMode.Extended,
                Equation = "IF cc true: (sp-1) <- PCH\n(sp-2) <- PCL, pc <- nn",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 8,
                    Param1List = new string[] { "C","M","NC","NZ","P","PE","PO","Z" },
                    Param2List = new string[] { "nn" },
                    InstructionCodeSizeInBytes = 3,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        Condition = "cc == 1",
                        MachineCyclesCount = 5,
                        TStatesCount = 17,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.ODL, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.ODH, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.SWH, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.SWL, TStates = 3 } },
                        TStatesByMachineCycle = "17 (4, 3, 4, 3, 3)",
                        StackOperations = "M3 -1, M4 -1"
                    },
                    AlternateExecutionTimings = new InstructionExecutionVariant() {
                        Condition = "cc == 0",
                        MachineCyclesCount = 3,
                        TStatesCount = 10,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.ODL, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.ODH, TStates = 3 } },
                        TStatesByMachineCycle = "10 (4, 3, 3)"
                    }
                }
                },

                UserManualPage = "257",
                Description = "If condition cc is true, this instruction pushes the current contents of the\nProgram Counter (PC) onto the top of the external memory stack, then\nloads the operands nn to PC to point to the address in memory where the\nfirst Op Code of a subroutine is to be fetched. At the end of the subroutine,\na RETurn instruction can be used to return to the original program flow by\npopping the top of the stack back to PC. If condition cc is false, the\nProgram Counter is incremented as usual, and the program continues with\nthe next sequential instruction. The stack push is accomplished by first\ndecrementing the current contents of the Stack Pointer (SP), loading the\nhigh-order byte of the PC contents to the memory address now pointed to\nby SP, then decrementing SP again, and loading the low order byte of the\nPC contents to the top of the stack.\nBecause this is a 3-byte instruction, the Program Counter was incremented\nby three before the push is executed.\nCondition cc is programmed as one of eight status that corresponds to\ncondition bits in the Flag Register (register F). These eight status are\ndefined in the table below :\nCondition Flag\nNZ non zero Z\nZ zero Z\nNC non carry C\nC carry Z\nPO parity odd P/V\nPE parity even P/V\nP sign positive S\nM sign negative S",
                ConditionBitsAffected = "None",
                Example = "are 1A47H, the contents of the Stack Pointer are 3002H, and memory\nlocations have the contents:\nLocation Contents\n1A47H D4H\n1448H 35H\n1A49H 21H\nthen if an instruction fetch sequence begins, the 3-byte instruction\nD43521H is fetched to the CPU for execution. The mnemonic equivalent of\nthis is CALL NC, 2135H. At execution of this instruction, the contents of\nmemory address 3001H is 1AH, the contents of address 3000H is 4AH, the\ncontents of the Stack Pointer is 3000H, and the contents of the Program\nCounter is 2135H, pointing to the address of the first Op Code of the\nsubroutine now to be executed."
            };

            Table[20] = new InstructionType()
            {
                Index = 20,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "General-Purpose Arithmetic",
                OpCodeName = "CCF",
                Equation = "CY <- ~CY",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 1,
                        TStatesCount = 4,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "1 (4)"
                    }
                }
                },

                UserManualPage = "170",
                Description = "The Carry flag in the F register is inverted.",
                ConditionBitsAffected = "S is not affected\nZ is not affected\nH, previous carry is copied\nP/V is not affected\nN is reset\nC is set if CY was 0 before operation, reset otherwise"
            };

            Table[21] = new InstructionType()
            {
                Index = 21,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "CP",
                Param1Type = AddressingMode.Immediate,
                Equation = "A - s",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "n" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 7,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 } },
                        TStatesByMachineCycle = "7 (4, 3)"
                    }
                }
                },

                UserManualPage = "158",
                Description = "The contents of the s operand are compared with the contents of the\nAccumulator. If there is a true compare, the Z flag is set. The execution of\nthis instruction does not affect the contents of the Accumulator.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is set if overflow, reset otherwise\nN is set\nC is set if borrow, reset otherwise",
                Example = "If the Accumulator contains 63H, the HL register pair contains 6000H, and\nmemory location 6000H contains 60H, the instruction CP (HL) results in\nthe PN flag in the F register resetting."
            };

            Table[22] = new InstructionType()
            {
                Index = 22,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "CP",
                Param1Type = AddressingMode.Register,
                Equation = "A - s",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 7,
                    Param1List = new string[] { "A","B","C","D","E","H","L" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 1,
                        TStatesCount = 4,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "1 (4)"
                    }
                },
                new InstructionParametersVariant() {
                    Index = 1,
                    IsUndocumented = true,
                    InstructionCodeCount = 4,
                    Param1List = new string[] { "IXh","IXl","IYh","IYl" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 8,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "8 (4, 4)"
                    }
                }
                },

                UserManualPage = "158",
                Description = "The contents of the s operand are compared with the contents of the\nAccumulator. If there is a true compare, the Z flag is set. The execution of\nthis instruction does not affect the contents of the Accumulator.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is set if overflow, reset otherwise\nN is set\nC is set if borrow, reset otherwise",
                Example = "If the Accumulator contains 63H, the HL register pair contains 6000H, and\nmemory location 6000H contains 60H, the instruction CP (HL) results in\nthe PN flag in the F register resetting."
            };

            Table[23] = new InstructionType()
            {
                Index = 23,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "CP",
                Param1Type = AddressingMode.RegisterIndirect,
                Equation = "A - s",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "(HL)" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 7,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 } },
                        TStatesByMachineCycle = "7 (4, 3)"
                    }
                }
                },

                UserManualPage = "158",
                Description = "The contents of the s operand are compared with the contents of the\nAccumulator. If there is a true compare, the Z flag is set. The execution of\nthis instruction does not affect the contents of the Accumulator.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is set if overflow, reset otherwise\nN is set\nC is set if borrow, reset otherwise",
                Example = "If the Accumulator contains 63H, the HL register pair contains 6000H, and\nmemory location 6000H contains 60H, the instruction CP (HL) results in\nthe PN flag in the F register resetting."
            };

            Table[24] = new InstructionType()
            {
                Index = 24,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "CP",
                Param1Type = AddressingMode.Indexed,
                Equation = "A - s",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 2,
                    Param1List = new string[] { "(IX+d)","(IY+d)" },
                    InstructionCodeSizeInBytes = 3,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 5,
                        TStatesCount = 19,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 } },
                        TStatesByMachineCycle = "19 (4, 4, 3, 5, 3)"
                    }
                }
                },

                UserManualPage = "158",
                Description = "The contents of the s operand are compared with the contents of the\nAccumulator. If there is a true compare, the Z flag is set. The execution of\nthis instruction does not affect the contents of the Accumulator.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is set if overflow, reset otherwise\nN is set\nC is set if borrow, reset otherwise",
                Example = "If the Accumulator contains 63H, the HL register pair contains 6000H, and\nmemory location 6000H contains 60H, the instruction CP (HL) results in\nthe PN flag in the F register resetting."
            };

            Table[25] = new InstructionType()
            {
                Index = 25,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Exchange, Block Transfer, and Search",
                OpCodeName = "CPD",
                Equation = "A - (HL), HL <- HL -1, BC <- BC -1",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 4,
                        TStatesCount = 16,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 5 } },
                        TStatesByMachineCycle = "16 (4, 4, 3, 5)"
                    }
                }
                },

                UserManualPage = "137",
                Description = "The contents of the memory location addressed by the HL register pair is\ncompared with the contents of the Accumulator. In case of a true\ncompare, a condition bit is set. The HL and Byte Counter (register pair\nBC) are decremented.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if A equals (HL), reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is set if BC -1 x 0, reset otherwise\nN is set\nC is not affected",
                Example = "If the HL register pair contains 1111H, memory location 1111H contains\n3BH, the Accumulator contains 3BH, and the Byte Counter contains 0001H.\nAt execution of CPD the Byte Counter contains 0000H, the HL register\npair contains 1110H, the flag in the F register sets, and the P/V flag in the F\nregister resets. There is no effect on the contents of the Accumulator or\naddress 1111H."
            };

            Table[26] = new InstructionType()
            {
                Index = 26,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Exchange, Block Transfer, and Search",
                OpCodeName = "CPDR",
                Equation = "A - (HL), HL <- HL -1, BC <- BC -1",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        Condition = "BC!=0",
                        MachineCyclesCount = 5,
                        TStatesCount = 21,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 5 } },
                        TStatesByMachineCycle = "21 (4, 4, 3, 5, 5)"
                    },
                    AlternateExecutionTimings = new InstructionExecutionVariant() {
                        Condition = "BC==0",
                        MachineCyclesCount = 4,
                        TStatesCount = 16,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 5 } },
                        TStatesByMachineCycle = "16 (4, 4, 3, 5)"
                    }
                }
                },

                UserManualPage = "138",
                Description = "The contents of the memory location addressed by the HL register pair is\ncompared with the contents of the Accumulator. In case of a true compare,\na condition bit is set. The HL and BC (Byte Counter) register pairs are\ndecremented. If decrementing causes the BC to go to zero or if A = (HL),\nthe instruction is terminated. If BC is not zero and A = (HL), the program\ncounter is decremented by two and the instruction is repeated. Interrupts are\nrecognized and two refresh cycles execute after each data transfer. When\nBC is set to zero, prior to instruction execution, the instruction loops\nthrough 64 Kbytes if no match is found.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if A = (HL), reset otherwise\nH is set if borrow form bit 4, reset otherwise\nP/V is set if BC -1 ? 0, reset otherwise\nN is set\nC is not affected",
                Example = "If the HL register pair contains 1118H, the Accumulator contains F3H, the\nByte Counter contains 0007H, and memory locations have these contents.\n(1118H) contains 52H\n(1117H) contains 00H\n(1116H) contains F3H\nThen, at execution of CPDR the contents of register pair HL are 1115H,\nthe contents of the Byte Counter are 0004H, the P/V flag in the F register\nsets, and the Z flag in the F register sets."
            };

            Table[27] = new InstructionType()
            {
                Index = 27,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Exchange, Block Transfer, and Search",
                OpCodeName = "CPI",
                Equation = "A - (HL), HL <- HL +1, BC <- BC -1",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 4,
                        TStatesCount = 16,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 5 } },
                        TStatesByMachineCycle = "16 (4, 4, 3, 5)"
                    }
                }
                },

                UserManualPage = "134",
                Description = "The contents of the memory location addressed by the HL register is\ncompared with the contents of the Accumulator. In case of a true compare,\na condition bit is set. Then HL is incremented and the Byte Counter\n(register pair BC) is decremented.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if A is (HL), reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is set if BC -1 is not 0, reset otherwise\nN is set\nC is not affected",
                Example = "If the HL register pair contains 1111H, memory location 1111H contains\n3BH, the Accumulator contains 3BH, and the Byte Counter contains 0001H.\nAt execution of CPI the Byte Counter contains 0000H, the HL register\npair contains 1112H, the Z flag in the F register sets, and the P/V flag in the\nF register resets. There is no effect on the contents of the Accumulator or\naddress 1111H."
            };

            Table[28] = new InstructionType()
            {
                Index = 28,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Exchange, Block Transfer, and Search",
                OpCodeName = "CPIR",
                Equation = "A - (HL), HL <- HL +1, BC <- BC -1",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        Condition = "BC!=0",
                        MachineCyclesCount = 5,
                        TStatesCount = 21,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 5 } },
                        TStatesByMachineCycle = "21 (4, 4, 3, 5, 5)"
                    },
                    AlternateExecutionTimings = new InstructionExecutionVariant() {
                        Condition = "BC==0",
                        MachineCyclesCount = 4,
                        TStatesCount = 16,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 5 } },
                        TStatesByMachineCycle = "16 (4, 4, 3, 5)"
                    }
                }
                },

                UserManualPage = "135",
                Description = "The contents of the memory location addressed by the HL register pair is\ncompared with the contents of the Accumulator. In case of a true compare, a\ncondition bit is set. HL is incremented and the Byte Counter (register pair\nBC) is decremented. If decrementing causes BC to go to zero or if A = (HL),\nthe instruction is terminated. If BC is not zero and A ? (HL), the program\ncounter is decremented by two and the instruction is repeated. Interrupts are\nrecognized and two refresh cycles are executed after each data transfer.\nIf BC is set to zero before instruction execution, the instruction loops\nthrough 64 Kbytes if no match is found.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if A equals (HL), reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is set if BC -1 does not equal 0, reset otherwise\nN is set\nC is not affected",
                Example = "If the HL register pair contains 1111H, the Accumulator contains F3H, the\nByte Counter contains 0007H, and memory locations have these contents:\n(1111H) contains 52H\n(1112H) contains 00H\n(1113H) contains F3H\nThen, at execution of CPIR the contents of register pair HL is 1114H, the\ncontents of the Byte Counter is 0004H, the P/V flag in the F register sets,\nand the Z flag in the F register sets."
            };

            Table[29] = new InstructionType()
            {
                Index = 29,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "General-Purpose Arithmetic",
                OpCodeName = "CPL",
                Equation = "A <- ~A",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 1,
                        TStatesCount = 4,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "1 (4)"
                    }
                }
                },

                UserManualPage = "168",
                Description = "The contents of the Accumulator (register A) are inverted (one�s\ncomplement).",
                ConditionBitsAffected = "S is not affected\nZ is not affected\nH is set\nP/V is not affected\nN is set\nC is not affected",
                Example = "If the contents of the Accumulator are 1011 0100, at execution of CPL\nthe Accumulator contents are 0100 1011."
            };

            Table[30] = new InstructionType()
            {
                Index = 30,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "General-Purpose Arithmetic",
                OpCodeName = "DAA",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 1,
                        TStatesCount = 4,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "1 (4)"
                    }
                }
                },

                UserManualPage = "166",
                Description = "This instruction conditionally adjusts the Accumulator for BCD addition and\nsubtraction operations. For addition (ADD, ADC, INC) or subtraction (SUB,\nSBC, DEC, NEG), the following table indicates the operation performed:\nOperation\nC Before\nDAA\nHex Value In\nUpper Digit\n(bit 7-4)\nH Before\nDAA\nHex Value\nIn Lower\nDigit\n(bit 3-0)\nNumber\nAdded To\nByte\nC After\nDAA\n0 9-0 0 0-9 00 0\n0 0-8 0 A-F 06 0\n0 0-9 1 0-3 06 0\nADD 0 A-F 0 0-9 60 1\nADC 0 9-F 0 A-F 66 1\nINC 0 A-F 1 0-3 66 1\n1 0-2 0 0-9 60 1\n1 0-2 0 A-F 66 1\n1 0-3 1 0-3 66 1\nSUB 0 0-9 0 0-9 00 0\nSBC 0 0-8 1 6-F FA 0\nDEC 1 7-F 0 0-9 A0 1\nNEG 1 6-7 1 6-F 9A 1",
                ConditionBitsAffected = "S is set if most-significant bit of Accumulator is 1 after operation, reset\notherwise\nZ is set if Accumulator is zero after operation, reset otherwise\nH, see instruction\nP/V is set if Accumulator is even parity after operation, reset otherwise\nN is not affected\nC, see instruction",
                Example = "If an addition operation is performed between 15 (BCD) and 27 (BCD),\nsimple decimal arithmetic gives this result:\n15\n+27\n42\nBut when the binary representations are added in the Accumulator\naccording to standard binary arithmetic.\n0001 0101\n+ 0010 0111\n0011 1100 = 3C\nthe sum is ambiguous. The DAA instruction adjusts this result so that the\ncorrect BCD representation is obtained:\n0011 1100\n+ 0000 0110\n0100 0010 = 42"
            };

            Table[31] = new InstructionType()
            {
                Index = 31,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "DEC",
                Param1Type = AddressingMode.Register,
                Equation = "m <- m- 1",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 7,
                    Param1List = new string[] { "A","B","C","D","E","H","L" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 1,
                        TStatesCount = 4,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "1 (4)"
                    }
                },
                new InstructionParametersVariant() {
                    Index = 1,
                    IsUndocumented = true,
                    InstructionCodeCount = 4,
                    Param1List = new string[] { "IXh","IXl","IYh","IYl" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 8,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "8 (4, 4)"
                    }
                }
                },

                UserManualPage = "164",
                Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous INC instructions.\nThe byte specified by the m operand is decremented.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is set if m was 80H before operation, reset otherwise\nN is set\nC is not affected",
                Example = "If the D register contains byte 2AH, at execution of DEC D register D\ncontains 29H."
            };

            Table[32] = new InstructionType()
            {
                Index = 32,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "DEC",
                Param1Type = AddressingMode.RegisterIndirect,
                Equation = "m <- m- 1",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "(HL)" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 3,
                        TStatesCount = 11,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "11 (4, 4, 3)"
                    }
                }
                },

                UserManualPage = "164",
                Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous INC instructions.\nThe byte specified by the m operand is decremented.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is set if m was 80H before operation, reset otherwise\nN is set\nC is not affected",
                Example = "If the D register contains byte 2AH, at execution of DEC D register D\ncontains 29H."
            };

            Table[33] = new InstructionType()
            {
                Index = 33,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "DEC",
                Param1Type = AddressingMode.Indexed,
                Equation = "m <- m- 1",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 2,
                    Param1List = new string[] { "(IX+d)","(IY+d)" },
                    InstructionCodeSizeInBytes = 3,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 6,
                        TStatesCount = 23,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "23 (4, 4, 3, 5, 4, 3)"
                    }
                }
                },

                UserManualPage = "164",
                Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous INC instructions.\nThe byte specified by the m operand is decremented.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is set if m was 80H before operation, reset otherwise\nN is set\nC is not affected",
                Example = "If the D register contains byte 2AH, at execution of DEC D register D\ncontains 29H."
            };

            Table[34] = new InstructionType()
            {
                Index = 34,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "16-Bit Arithmetic",
                OpCodeName = "DEC",
                Param1Type = AddressingMode.Register16,
                Equation = "ss <- ss - 1",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 4,
                    Param1List = new string[] { "BC","DE","HL","SP" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 1,
                        TStatesCount = 6,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 6 } },
                        TStatesByMachineCycle = "1 (6)"
                    }
                },
                new InstructionParametersVariant() {
                    Index = 1,
                    IsUndocumented = false,
                    InstructionCodeCount = 2,
                    Param1List = new string[] { "IX","IY" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 10,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 6 } },
                        TStatesByMachineCycle = "10 (4, 6)"
                    }
                }
                },

                UserManualPage = "187",
                Description = "The contents of register pair ss (any of the register pairs BC, DE, HL, or\nSP) are decremented.",
                ConditionBitsAffected = "None",
                Example = "If register pair HL contains 1001H, at execution of DEC HL the contents\nof HL are 1000H."
            };

            Table[35] = new InstructionType()
            {
                Index = 35,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "CPU Control",
                OpCodeName = "DI",
                Equation = "IFF <- 0",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 1,
                        TStatesCount = 4,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "1 (4)"
                    }
                }
                },

                UserManualPage = "174",
                Description = "DI disables the maskable interrupt by resetting the interrupt enable flipflops\n(IFF1 and IFF2). Note that this instruction disables the maskable\ninterrupt during its execution",
                ConditionBitsAffected = "None",
                Example = "When the CPU executes the instruction DI the maskable interrupt is\ndisabled until it is subsequently re-enabled by an EI instruction. The CPU\ndoes not respond to an Interrupt Request (INT) signal."
            };

            Table[36] = new InstructionType()
            {
                Index = 36,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Jump",
                OpCodeName = "DJNZ",
                Param1Type = AddressingMode.Relative,

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "e" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        Condition = "B != 0",
                        MachineCyclesCount = 3,
                        TStatesCount = 13,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 5 } },
                        TStatesByMachineCycle = "13 (5,3, 5)"
                    },
                    AlternateExecutionTimings = new InstructionExecutionVariant() {
                        Condition = "B == 0",
                        MachineCyclesCount = 2,
                        TStatesCount = 8,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 } },
                        TStatesByMachineCycle = "8 (5, 3)"
                    }
                }
                },

                UserManualPage = "253",
                Description = "This instruction is similar to the conditional jump instructions except that\na register value is used to determine branching. The B register is\ndecremented, and if a non zero value remains, the value of the\ndisplacement e is added to the Program Counter (PC). The next\ninstruction is fetched from the location designated by the new contents of\nthe PC. The jump is measured from the address of the instruction Op\nCode and has a range of -126 to +129 bytes. The assembler automatically\nadjusts for the twice incremented PC.\nIf the result of decrementing leaves B with a zero value, the next instruction\nexecuted is taken from the location following this instruction.",
                ConditionBitsAffected = "None",
                Example = "A typical software routine is used to demonstrate the use of the DJNZ\ninstruction. This routine moves a line from an input buffer (INBUF) to an\noutput buffer (OUTBUF). It moves the bytes until it finds a CR, or until it\nhas moved 80 bytes, whichever occurs first.\nLD B, 80 ,Set up counter\nLD HL, Inbuf ,Set up pointers\nLD DE, Outbuf\nLOOP: LD A, (HL) ,Get next byte from\n,input buffer\nLD (DE), A ,Store in output buffer\nCP ODH ,Is it a CR?\nJR Z, DONE ,Yes finished\nINC HL ,Increment pointers\nINC DE\nDJNZ LOOP ,Loop back if 80\n,bytes have not\n,been moved\nDONE:"
            };

            Table[37] = new InstructionType()
            {
                Index = 37,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "CPU Control",
                OpCodeName = "EI",
                Equation = "IFF <- 1",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 1,
                        TStatesCount = 4,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "1 (4)"
                    }
                }
                },

                UserManualPage = "175",
                Description = "The enable interrupt instruction sets both interrupt enable flip flops (IFFI\nand IFF2) to a logic 1, allowing recognition of any maskable interrupt. Note\nthat during the execution of this instruction and the following instruction,\nmaskable interrupts are disabled.",
                ConditionBitsAffected = "None",
                Example = "When the CPU executes instruction EI the maskable interrupt is\nenabled."
            };

            Table[38] = new InstructionType()
            {
                Index = 38,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Exchange, Block Transfer, and Search",
                OpCodeName = "EX",
                Param1Type = AddressingMode.Register16,
                Param2Type = AddressingMode.Register16,
                Equation = "DE <-> HL",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "AF" },
                    Param2List = new string[] { "AF'" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 1,
                        TStatesCount = 4,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "1 (4)"
                    }
                },
                new InstructionParametersVariant() {
                    Index = 1,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "DE" },
                    Param2List = new string[] { "HL" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 1,
                        TStatesCount = 4,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "1 (4)"
                    }
                }
                },

                UserManualPage = "123",
                Description = "The 2-byte contents of register pairs DE and HL are exchanged.",
                ConditionBitsAffected = "None",
                Example = "If the content of register pair DE is the number 2822H, and the content of\nthe register pair HL is number 499AH, at instruction EX DE, HL the content\nof register pair DE is 499AH, and the content of register pair HL is 2822H."
            };

            Table[39] = new InstructionType()
            {
                Index = 39,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Exchange, Block Transfer, and Search",
                OpCodeName = "EX",
                Param1Type = AddressingMode.RegisterIndirect,
                Param2Type = AddressingMode.Register16,
                Equation = "H <-> (SP+1), L <-> (SP)",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "(SP)" },
                    Param2List = new string[] { "HL" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 5,
                        TStatesCount = 19,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.SRL, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.SRH, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.SWH, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.SWL, TStates = 5 } },
                        TStatesByMachineCycle = "19 (4, 3, 4, 3, 5)",
                        StackOperations = "M2 -1, M4 -1"
                    }
                },
                new InstructionParametersVariant() {
                    Index = 1,
                    IsUndocumented = false,
                    InstructionCodeCount = 2,
                    Param1List = new string[] { "(SP)" },
                    Param2List = new string[] { "IX","IY" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 6,
                        TStatesCount = 23,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.SRL, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.SRH, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.SWH, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.SWL, TStates = 5 } },
                        TStatesByMachineCycle = "23 (4, 4, 3, 4, 3, 5)",
                        StackOperations = "M3 +1, M5 +1"
                    }
                }
                },

                UserManualPage = "125",
                Description = "The low order byte contained in register pair HL is exchanged with the\ncontents of the memory address specified by the contents of register pair SP\n(Stack Pointer), and the high order byte of HL is exchanged with the next\nhighest memory address (SP+1).",
                ConditionBitsAffected = "None",
                Example = "If the HL register pair contains 7012H, the SP register pair contains 8856H,\nthe memory location 8856H contains byte 11H, and memory location\n8857H contains byte 22H, then the instruction EX (SP), HL results in the\nHL register pair containing number 2211H, memory location 8856H\ncontaining byte 12H, memory location 8857H containing byte 70H and\nStack Pointer containing 8856H."
            };

            Table[40] = new InstructionType()
            {
                Index = 40,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Exchange, Block Transfer, and Search",
                OpCodeName = "EXX",
                Equation = "BC <-> BC', DE <-> DE', HL <-> HL'",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 1,
                        TStatesCount = 4,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "1 (4)"
                    }
                }
                },

                UserManualPage = "124",
                Description = "Each 2-byte value in register pairs BC, DE, and HL is exchanged with the\n2-byte value in BC', DE', and HL', respectively.",
                ConditionBitsAffected = "None",
                Example = "If the contents of register pairs BC, DE, and HL are the numbers 445AH,\n3DA2H, and 8859H, respectively, and the contents of register pairs BC',\nDE', and HL' are 0988H, 9300H, and 00E7H, respectively, at instruction\nEXX the contents of the register pairs are as follows: BC' contains 0988H,\nDE' contains 9300H, HL contains 00E7H, BC' contains 445AH, DE'\ncontains 3DA2H, and HL' contains 8859H."
            };

            Table[41] = new InstructionType()
            {
                Index = 41,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "CPU Control",
                OpCodeName = "HALT",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 1,
                        TStatesCount = 4,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "1 (4)"
                    }
                }
                },

                UserManualPage = "173",
                Description = "The HALT instruction suspends CPU operation until a subsequent interrupt\nor reset is received. While in the HALT state, the processor executes NOPs\nto maintain memory refresh logic.",
                ConditionBitsAffected = "None"
            };

            Table[42] = new InstructionType()
            {
                Index = 42,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "CPU Control",
                OpCodeName = "IM",
                Param1Type = AddressingMode.InterruptMode,

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 3,
                    Param1List = new string[] { "0","1","2" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 8,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "8 (4, 4)"
                    }
                }
                },

                UserManualPage = "176\n177\n178",
                Description = "The IM 0 instruction sets interrupt mode 0. In this mode, the interrupting\ndevice can insert any instruction on the data bus for execution by the\nCPU. The first byte of a multi-byte instruction is read during the interrupt\nacknowledge cycle. Subsequent bytes are read in by a normal memory\nread sequence.\nThe IM 1 instruction sets interrupt mode 1. In this mode, the processor\nresponds to an interrupt by executing a restart to location 0038H.\nThe IM 2 instruction sets the vectored interrupt mode 2. This mode allows\nan indirect call to any memory location by an 8-bit vector supplied from the\nperipheral device. This vector then becomes the least-significant eight bits\nof the indirect pointer, while the I register in the CPU provides the most-significant\neight bits. This address points to an address in a vector table that\nis the starting address for the interrupt service routine.",
                ConditionBitsAffected = "None"
            };

            Table[43] = new InstructionType()
            {
                Index = 43,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Input and Output",
                OpCodeName = "IN",
                Param1Type = AddressingMode.Register,
                Param2Type = AddressingMode.IOPortImmediate,
                Equation = "A <- (n)",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "A" },
                    Param2List = new string[] { "(n)" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 3,
                        TStatesCount = 11,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.PR, TStates = 4 } },
                        TStatesByMachineCycle = "11 (4, 3, 4)"
                    }
                }
                },

                UserManualPage = "269",
                Description = "The operand n is placed on the bottom half (A0 through A7) of the address\nbus to select the I/O device at one of 256 possible ports. The contents of the\nAccumulator also appear on the top half (A8 through A15) of the address\nbus at this time. Then one byte from the selected port is placed on the data\nbus and written to the Accumulator (register A) in the CPU.",
                ConditionBitsAffected = "None",
                Example = "If the contents of the Accumulator are 23H, and byte 7BH is available at the\nperipheral device mapped to I/O port address 01H. At execution of INA,\n(01H) the Accumulator contains 7BH."
            };

            Table[44] = new InstructionType()
            {
                Index = 44,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Input and Output",
                OpCodeName = "IN",
                Param1Type = AddressingMode.Register,
                Param2Type = AddressingMode.IOPortRegister,
                Equation = "r <- (C)",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 7,
                    Param1List = new string[] { "A","B","C","D","E","H","L" },
                    Param2List = new string[] { "(C)" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 3,
                        TStatesCount = 12,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.PR, TStates = 4 } },
                        TStatesByMachineCycle = "12 (4, 4, 4)"
                    }
                }
                },

                UserManualPage = "270",
                Description = "The contents of register C are placed on the bottom half (A0 through A7) of\nthe address bus to select the I/O device at one of 256 possible ports. The\ncontents of Register B are placed on the top half (A8 through A15) of the\naddress bus at this time. Then one byte from the selected port is placed on\nthe data bus and written to register r in the CPU. Register r identifies any of\nthe CPU registers shown in the following table, which also indicates the\ncorresponding 3-bit r field for each. The flags are affected, checking the\ninput data.",
                ConditionBitsAffected = "S is set if input data is negative, reset otherwise\nZ is set if input data is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is not affected",
                Example = "If the contents of register C are 07H, the contents of register B are 10H,\nand byte 7BH is available at the peripheral device mapped to I/O port\naddress 07H. After execution of IN D, (C) register D contains 7BH."
            };

            Table[45] = new InstructionType()
            {
                Index = 45,
                IsUndocumented = true,
                IsInternalZ80Operation = false,

                InstructionGroup = "Input and Output",
                OpCodeName = "IN",
                Param1Type = AddressingMode.Flags,
                Param2Type = AddressingMode.IOPortRegister,

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = true,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "F" },
                    Param2List = new string[] { "(C)" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 3,
                        TStatesCount = 12,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.PR, TStates = 4 } },
                        TStatesByMachineCycle = "12 (4, 4, 4)"
                    }
                }
                },

                Description = "The ED70 instruction reads from I/O port C, but does not store the result. It just affects the flags like the other IN x,(C) instructions.",
                ConditionBitsAffected = "S is set if input data is negative, reset otherwise\nZ is set if input data is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is not affected",
                Example = "If the contents of register C are 07H, the contents of register B are 10H,\nand byte 7BH is available at the peripheral device mapped to I/O port\naddress 07H. After execution of IN F, (C) value 7BH is not stored in any register, only flag F is affected."
            };

            Table[46] = new InstructionType()
            {
                Index = 46,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "INC",
                Param1Type = AddressingMode.Register,
                Equation = "r <- r + 1",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 7,
                    Param1List = new string[] { "A","B","C","D","E","H","L" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 1,
                        TStatesCount = 4,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "1 (4)"
                    }
                },
                new InstructionParametersVariant() {
                    Index = 1,
                    IsUndocumented = true,
                    InstructionCodeCount = 4,
                    Param1List = new string[] { "IXh","IXl","IYh","IYl" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 8,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "8 (4, 4)"
                    }
                }
                },

                UserManualPage = "160",
                Description = "Register r is incremented and register r identifies any of the registers A, B,\nC, D, E, H, or L.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if carry from bit 3, reset otherwise\nP/V is set if r was 7FH before operation, reset otherwise\nN is reset\nC is not affected",
                Example = "If the contents of register D are 28H, at execution of INC D the contents of\nregister D are 29H."
            };

            Table[47] = new InstructionType()
            {
                Index = 47,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "INC",
                Param1Type = AddressingMode.RegisterIndirect,
                Equation = "(HL) <- (HL) + 1",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "(HL)" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 3,
                        TStatesCount = 11,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "11 (4, 4, 3)"
                    }
                }
                },

                UserManualPage = "161",
                Description = "The byte contained in the address specified by the contents of the HL\nregister pair is incremented.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if carry from bit 3, reset otherwise\nP/V is set if (HL) was 7FH before operation, reset otherwise\nN is reset\nC is not affected",
                Example = "If the contents of the HL register pair are 3434H, and the contents of\naddress 3434H are 82H, at execution of INC (HL) memory location\n3434H contains 83H."
            };

            Table[48] = new InstructionType()
            {
                Index = 48,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "INC",
                Param1Type = AddressingMode.Indexed,
                Equation = "(IX+d) <- (IX+d) + 1",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 2,
                    Param1List = new string[] { "(IX+d)","(IY+d)" },
                    InstructionCodeSizeInBytes = 3,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 6,
                        TStatesCount = 23,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "23 (4, 4, 3, 5, 4, 3)"
                    }
                }
                },

                UserManualPage = "162\n163",
                Description = "The contents of the Index Register IX (register pair IX) are added to a two�s\ncomplement displacement integer d to point to an address in memory. The\ncontents of this address are then incremented.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if carry from bit 3, reset otherwise\nP/V is set if (IX+d) was 7FH before operation, reset otherwise\nN is reset\nC is not affected",
                Example = "If the contents of the Index Register pair IX are 2020H, and the memory\nlocation 2030H contains byte 34H, at execution of INC (IX+10H) the\ncontents of memory location 2030H is 35H."
            };

            Table[49] = new InstructionType()
            {
                Index = 49,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "16-Bit Arithmetic",
                OpCodeName = "INC",
                Param1Type = AddressingMode.Register16,
                Equation = "ss <- ss + 1",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 4,
                    Param1List = new string[] { "BC","DE","HL","SP" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 1,
                        TStatesCount = 6,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 6 } },
                        TStatesByMachineCycle = "1 (6)"
                    }
                },
                new InstructionParametersVariant() {
                    Index = 1,
                    IsUndocumented = false,
                    InstructionCodeCount = 2,
                    Param1List = new string[] { "IX","IY" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 10,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 6 } },
                        TStatesByMachineCycle = "10 (4, 6)"
                    }
                }
                },

                UserManualPage = "184",
                Description = "The contents of register pair ss (any of register pairs BC, DE, HL, or SP)\nare incremented.",
                ConditionBitsAffected = "None",
                Example = "If the register pair contains 1000H, after the execution of INC HL, HL\ncontains 1001H."
            };

            Table[50] = new InstructionType()
            {
                Index = 50,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Input and Output",
                OpCodeName = "IND",
                Equation = "(HL) <- (C), B <- B -1, HL <- HL -1",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 4,
                        TStatesCount = 16,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.PR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "16 (4, 5, 4, 3)"
                    }
                }
                },

                UserManualPage = "275",
                Description = "The contents of register C are placed on the bottom half (A0 through A7) of\nthe address bus to select the I/O device at one of 256 possible ports.\nRegister B may be used as a byte counter, and its contents are placed on the\ntop half (A8 through A15) of the address bus at this time. Then one byte\nfrom the selected port is placed on the data bus and written to the CPU. The\ncontents of the HL register pair are placed on the address bus and the input\nbyte is written to the corresponding location of memory. Finally, the byte\ncounter and register pair HL are decremented.",
                ConditionBitsAffected = "S is unknown\nZ is set if B�1 = 0, reset otherwise\nH is unknown\nP/V is unknown\nN is set\nC is not affected",
                Example = "If the contents of register C are 07H, the contents of register B are 10H, the\ncontents of the HL register pair are 1000H, and byte 7BH is available at the\nperipheral device mapped to I/O port address 07H. At execution of IND\nmemory location 1000H contains 7BH, the HL register pair contains\n0FFFH, and register B contains 0FH."
            };

            Table[51] = new InstructionType()
            {
                Index = 51,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Input and Output",
                OpCodeName = "INDR",
                Equation = "(HL) <- (C), B <- B -1, HL <- HL -1",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        Condition = "B!=0",
                        MachineCyclesCount = 5,
                        TStatesCount = 21,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.PR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 5 } },
                        TStatesByMachineCycle = "21 (4, 5, 4, 3, 5)"
                    },
                    AlternateExecutionTimings = new InstructionExecutionVariant() {
                        Condition = "B=0",
                        MachineCyclesCount = 4,
                        TStatesCount = 16,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.PR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "16 (4, 5, 4, 3)"
                    }
                }
                },

                UserManualPage = "277",
                Description = "The contents of register C are placed on the bottom half (A0 through A7)\nof the address bus to select the I/O device at one of 256 possible ports.\nRegister B is used as a byte counter, and its contents are placed on the top\nhalf (A8 through A15) of the address bus at this time. Then one byte from\nthe selected port is placed on the data bus and written to the CPU. The\ncontents of the HL register pair are placed on the address bus and the\ninput byte is written to the corresponding location of memory. Then HL\nand the byte counter are decremented. If decrementing causes B to go to\nzero, the instruction is terminated. If B is not zero, the PC is decremented\nby two and the instruction repeated. Interrupts are recognized and two\nrefresh cycles are executed after each data transfer.\nWhen B is set to zero prior to instruction execution, 256 bytes of data are\ninput.",
                ConditionBitsAffected = "S is unknown\nZ is set\nH is unknown\nP/V is unknown\nN is set\nC is not affected",
                Example = "If the contents of register C are 07H, the contents of register B are 03H, the\ncontents of the HL register pair are 1000H, and the following sequence of\nbytes are available at the peripheral device mapped to I/O port address 07H:\n51H\nA9H\n03H\nthen at execution of INDR the HL register pair contains 0FFDH, register B\ncontains zero, and memory locations contain the following:\n0FFEH contains 03H\n0FFFH contains A9H\n1000H contains 51H"
            };

            Table[52] = new InstructionType()
            {
                Index = 52,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Input and Output",
                OpCodeName = "INI",
                Equation = "(HL) <- (C), B <- B -1, HL <- HL + 1",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 4,
                        TStatesCount = 16,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.PR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "16 (4, 5, 4, 3)"
                    }
                }
                },

                UserManualPage = "272",
                Description = "The contents of register C are placed on the bottom half (A0 through A7) of\nthe address bus to select the I/O device at one of 256 possible ports.\nRegister B may be used as a byte counter, and its contents are placed on the\ntop half (A8 through A15) of the address bus at this time. Then one byte\nfrom the selected port is placed on the data bus and written to the CPU. The\ncontents of the HL register pair are then placed on the address bus and the\ninput byte is written to the corresponding location of memory. Finally, the\nbyte counter is decremented and register pair HL is incremented.",
                ConditionBitsAffected = "S is unknown\nZ is set if B�1 = 0, reset otherwise\nH is unknown\nP/V is unknown\nN is set\nC is not affected",
                Example = "If the contents of register C are 07H, the contents of register B are 10H, the\ncontents of the HL register pair are 1000H, and byte 7BH is available at the\nperipheral device mapped to I /O port address 07H. At execution of INI\nmemory location 1000H contains 7BH, the HL register pair contains\n1001H, and register B contains 0FH."
            };

            Table[53] = new InstructionType()
            {
                Index = 53,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Input and Output",
                OpCodeName = "INIR",
                Equation = "(HL) <- (C), B <- B -1, HL <- HL + 1",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        Condition = "B!=0",
                        MachineCyclesCount = 5,
                        TStatesCount = 21,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.PR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 5 } },
                        TStatesByMachineCycle = "21 (4, 5, 4, 3, 5)"
                    },
                    AlternateExecutionTimings = new InstructionExecutionVariant() {
                        Condition = "B=0",
                        MachineCyclesCount = 4,
                        TStatesCount = 16,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.PR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "16 (4, 5, 4, 3)"
                    }
                }
                },

                UserManualPage = "273",
                Description = "The contents of register C are placed on the bottom half (A0 through A7) of\nthe address bus to select the I/O device at one of 256 possible ports.\nRegister B is used as a byte counter, and its contents are placed on the top\nhalf (A8 through A15) of the address bus at this time. Then one byte from\nthe selected port is placed on the data bus and written to the CPU. The\ncontents of the HL register pair are placed on the address bus and the input\nbyte is written to the corresponding location of memory. Then register pair\nHL is incremented, the byte counter is decremented. If decrementing causes\nB to go to zero, the instruction is terminated. If B is not zero, the PC is\ndecremented by two and the instruction repeated. Interrupts are recognized\nand two refresh cycles execute after each data transfer.\nNote: if B is set to zero prior to instruction execution, 256 bytes of data\nare input.",
                ConditionBitsAffected = "S is unknown\nZ is set\nH is unknown\nP/V is unknown\nN is set\nC is not affected",
                Example = "If the contents of register C are 07H, the contents of register B are 03H,\nthe contents of the HL register pair are 1000H, and the following\nsequence of bytes are available at the peripheral device mapped to I/O\nport of address 07H:\n51H\nA9H\n03H\nthen at execution of INIR the HL register pair contains 1003H, register B\ncontains zero, and memory locations contain the following:\n1000H contains 51H\n1001H contains A9H\n1002H contains 03H"
            };

            Table[54] = new InstructionType()
            {
                Index = 54,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Jump",
                OpCodeName = "JP",
                Param1Type = AddressingMode.Extended,
                Equation = "PC <- nn",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "nn" },
                    InstructionCodeSizeInBytes = 3,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 3,
                        TStatesCount = 10,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.ODL, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.ODH, TStates = 3 } },
                        TStatesByMachineCycle = "10 (4, 3, 3)"
                    }
                }
                },

                UserManualPage = "238",
                Description = "Operand nn is loaded to register pair PC (Program Counter). The next\ninstruction is fetched from the location designated by the new contents of\nthe PC.",
                ConditionBitsAffected = "None"
            };

            Table[55] = new InstructionType()
            {
                Index = 55,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Jump",
                OpCodeName = "JP",
                Param1Type = AddressingMode.RegisterIndirect,
                Equation = "pc <- hL",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "(HL)" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 1,
                        TStatesCount = 4,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "1 (4)"
                    }
                },
                new InstructionParametersVariant() {
                    Index = 1,
                    IsUndocumented = false,
                    InstructionCodeCount = 2,
                    Param1List = new string[] { "(IX)","(IY)" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 8,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "8 (4, 4)"
                    }
                }
                },

                UserManualPage = "250",
                Description = "The Program Counter (register pair PC) is loaded with the contents of the\nHL register pair. The next instruction is fetched from the location\ndesignated by the new contents of the PC.",
                ConditionBitsAffected = "None",
                Example = "If the contents of the Program Counter are 1000H, and the contents of the\nHL register pair are 4800H, at execution of JP (HL) the contents of the\nProgram Counter are 4800H."
            };

            Table[56] = new InstructionType()
            {
                Index = 56,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Jump",
                OpCodeName = "JP",
                Param1Type = AddressingMode.FlagCondition,
                Param2Type = AddressingMode.Extended,
                Equation = "IF cc true, PC <- nn",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 8,
                    Param1List = new string[] { "C","M","NC","NZ","P","PE","PO","Z" },
                    Param2List = new string[] { "nn" },
                    InstructionCodeSizeInBytes = 3,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 3,
                        TStatesCount = 10,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.ODL, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.ODH, TStates = 3 } },
                        TStatesByMachineCycle = "10 (4, 3, 3)"
                    }
                }
                },

                UserManualPage = "239",
                Description = "If condition cc is true, the instruction loads operand nn to register pair PC\n(Program Counter), and the program continues with the instruction\nbeginning at address nn. If condition cc is false, the Program Counter is\nincremented as usual, and the program continues with the next sequential\ninstruction. Condition cc is programmed as one of eight status that\ncorresponds to condition bits in the Flag Register (register F). These eight\nstatus are defined in the table below :\nCondition Flag\nNZ non zero Z\nZ zero Z\nNC no carry C\nC carry C\nPO parity odd P/V\nPE parity even P/V\nP sign positive S\nM sign negative S",
                ConditionBitsAffected = "None",
                Example = "If the Carry flag (C flag in the F register) is set and the contents of address\n1520 are 03H, at execution of JP C, 1520H the Program Counter contains\n1520H, and on the next machine cycle the CPD fetches byte 03H from\naddress 1520H."
            };

            Table[57] = new InstructionType()
            {
                Index = 57,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Jump",
                OpCodeName = "JR",
                Param1Type = AddressingMode.Relative,
                Equation = "PC <- PC + e",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "e" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 3,
                        TStatesCount = 12,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 5 } },
                        TStatesByMachineCycle = "12 (4, 3, 5)"
                    }
                }
                },

                UserManualPage = "241",
                Description = "This instruction provides for unconditional branching to other segments of\na program. The value of the displacement e is added to the Program\nCounter (PC) and the next instruction is fetched from the location\ndesignated by the new contents of the PC. This jump is measured from the\naddress of the instruction Op Code and has a range of-126 to +129 bytes.\nThe assembler automatically adjusts for the twice incremented PC.",
                ConditionBitsAffected = "None",
                Example = "To jump forward five locations from address 480, the following assembly\nlanguage statement is used JR $+5\nThe resulting object code and final PC value is shown below:\nLocation Instruction\n480 18\n481 03\n482 -\n483 -\n484 -\n485 <- PC after jump"
            };

            Table[58] = new InstructionType()
            {
                Index = 58,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Jump",
                OpCodeName = "JR",
                Param1Type = AddressingMode.FlagCondition,
                Param2Type = AddressingMode.Relative,
                Equation = "If C = 0, continue\nIf C = 1, PC <- PC+ e",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 4,
                    Param1List = new string[] { "C","NC","NZ","Z" },
                    Param2List = new string[] { "e" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        Condition = "cc == 1",
                        MachineCyclesCount = 3,
                        TStatesCount = 12,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 5 } },
                        TStatesByMachineCycle = "12 (4, 3, 5)"
                    },
                    AlternateExecutionTimings = new InstructionExecutionVariant() {
                        Condition = "cc == 0",
                        MachineCyclesCount = 2,
                        TStatesCount = 7,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 } },
                        TStatesByMachineCycle = "7 (4,3)"
                    }
                }
                },

                UserManualPage = "242\n244\n246\n248",
                Description = "This instruction provides for conditional branching to other segments of a\nprogram depending on the results of a test on the Carry Flag. If the flag is\nequal to a 1, the value of the displacement e is added to the Program\nCounter (PC) and the next instruction is fetched from the location\ndesignated by the new contents of the PC. The jump is measured from the\naddress of the instruction Op Code and has a range of -126 to +129 bytes.\nThe assembler automatically adjusts for the twice incremented PC.\nIf the flag is equal to a 0, the next instruction executed is taken from the\nlocation following this instruction.",
                ConditionBitsAffected = "None",
                Example = "The Carry flag is set and it is required to jump back four locations from\n480. The assembly language statement is JR C, $ - 4\nThe resulting object code and final PC value is shown below:\nLocation Instruction\n47C <- PC after jump\n47D -\n47E -\n47F -\n480 38\n481 FA (two�s complement - 6)"
            };

            Table[59] = new InstructionType()
            {
                Index = 59,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Load",
                OpCodeName = "LD",
                Param1Type = AddressingMode.Register,
                Param2Type = AddressingMode.Immediate,
                Equation = "r <- n",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 7,
                    Param1List = new string[] { "A","B","C","D","E","H","L" },
                    Param2List = new string[] { "n" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 7,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 } },
                        TStatesByMachineCycle = "7 (4,3)"
                    }
                },
                new InstructionParametersVariant() {
                    Index = 1,
                    IsUndocumented = true,
                    InstructionCodeCount = 4,
                    Param1List = new string[] { "IXh","IXl","IYh","IYl" },
                    Param2List = new string[] { "n" },
                    InstructionCodeSizeInBytes = 3,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 3,
                        TStatesCount = 11,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 } },
                        TStatesByMachineCycle = "11 (4,4,3)"
                    }
                }
                },

                UserManualPage = "82",
                Description = "The 8-bit integer n is loaded to any register r, where r identifies register A,B, C, D, E, H, or L.",
                ConditionBitsAffected = "None",
                Example = "At execution of LD E, A5H the contents of register E are A5H."
            };

            Table[60] = new InstructionType()
            {
                Index = 60,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Load",
                OpCodeName = "LD",
                Param1Type = AddressingMode.Register,
                Param2Type = AddressingMode.Register,
                Equation = "r <- r'",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 49,
                    Param1List = new string[] { "A","B","C","D","E","H","L" },
                    Param2List = new string[] { "A","B","C","D","E","H","L" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 1,
                        TStatesCount = 4,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "1 (4)"
                    }
                },
                new InstructionParametersVariant() {
                    Index = 1,
                    IsUndocumented = true,
                    InstructionCodeCount = 20,
                    Param1List = new string[] { "A","B","C","D","E" },
                    Param2List = new string[] { "IXh","IXl","IYh","IYl" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 8,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "8 (4,4)"
                    }
                },
                new InstructionParametersVariant() {
                    Index = 2,
                    IsUndocumented = true,
                    InstructionCodeCount = 20,
                    Param1List = new string[] { "IXh","IXl","IYh","IYl" },
                    Param2List = new string[] { "A","B","C","D","E" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 8,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "8 (4,4)"
                    }
                },
                new InstructionParametersVariant() {
                    Index = 3,
                    IsUndocumented = true,
                    InstructionCodeCount = 4,
                    Param1List = new string[] { "IXh","IXl" },
                    Param2List = new string[] { "IXh","IXl" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 8,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "8 (4,4)"
                    }
                },
                new InstructionParametersVariant() {
                    Index = 4,
                    IsUndocumented = true,
                    InstructionCodeCount = 4,
                    Param1List = new string[] { "IYh","IYl" },
                    Param2List = new string[] { "IYh","IYl" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 8,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "8 (4,4)"
                    }
                },
                new InstructionParametersVariant() {
                    Index = 5,
                    IsUndocumented = false,
                    InstructionCodeCount = 2,
                    Param1List = new string[] { "A" },
                    Param2List = new string[] { "R","I" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 9,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 5 } },
                        TStatesByMachineCycle = "9 (4, 5)"
                    }
                },
                new InstructionParametersVariant() {
                    Index = 6,
                    IsUndocumented = false,
                    InstructionCodeCount = 2,
                    Param1List = new string[] { "R","I" },
                    Param2List = new string[] { "A" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 9,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 5 } },
                        TStatesByMachineCycle = "9 (4, 5)"
                    }
                }
                },

                UserManualPage = "81",
                Description = "The contents of any register r' are loaded to any other register r. r, r'\nidentifies any of the registers A, B, C, D, E, H, or L.",
                ConditionBitsAffected = "None",
                Example = "If the H register contains the number 8AH, and the E register contains 10H, the instruction LD H, E results in both registers containing 10H."
            };

            Table[61] = new InstructionType()
            {
                Index = 61,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Load",
                OpCodeName = "LD",
                Param1Type = AddressingMode.Register,
                Param2Type = AddressingMode.Extended,
                Equation = "A <- (nn)",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "A" },
                    Param2List = new string[] { "(nn)" },
                    InstructionCodeSizeInBytes = 3,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 4,
                        TStatesCount = 13,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.ODL, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.ODH, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 } },
                        TStatesByMachineCycle = "13 (4, 3, 3, 3)"
                    }
                }
                },

                UserManualPage = "94",
                Description = "The contents of the memory location specified by the operands nn are\nloaded to the Accumulator. The first n operand after the Op Code is the low\norder byte of a 2-byte memory address.",
                ConditionBitsAffected = "None",
                Example = "If the contents of nn is number 8832H, and the content of memory address\n8832H is byte 04H, at instruction LD A, (nn) byte 04H is in the\nAccumulator."
            };

            Table[62] = new InstructionType()
            {
                Index = 62,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Load",
                OpCodeName = "LD",
                Param1Type = AddressingMode.Register,
                Param2Type = AddressingMode.RegisterIndirect,
                Equation = "r <- (HL)",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 7,
                    Param1List = new string[] { "A","B","C","D","E","H","L" },
                    Param2List = new string[] { "(HL)" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 7,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 } },
                        TStatesByMachineCycle = "7 (4,3)"
                    }
                },
                new InstructionParametersVariant() {
                    Index = 1,
                    IsUndocumented = false,
                    InstructionCodeCount = 2,
                    Param1List = new string[] { "A" },
                    Param2List = new string[] { "(BC)","(DE)" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 7,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 } },
                        TStatesByMachineCycle = "7 (4, 3)"
                    }
                }
                },

                UserManualPage = "83",
                Description = "The 8-bit contents of memory location (HL) are loaded to register r,\nwhere r identifies register A, B, C, D, E, H, or L.",
                ConditionBitsAffected = "None",
                Example = "If register pair HL contains the number 75A1H, and memory address\n75A1H contains byte 58H, the execution of LD C, (HL) results in 58H in\nregister C."
            };

            Table[63] = new InstructionType()
            {
                Index = 63,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Load",
                OpCodeName = "LD",
                Param1Type = AddressingMode.Register,
                Param2Type = AddressingMode.Indexed,
                Equation = "r <- (IX+d)",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 14,
                    Param1List = new string[] { "A","B","C","D","E","H","L" },
                    Param2List = new string[] { "(IX+d)","(IY+d)" },
                    InstructionCodeSizeInBytes = 3,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 5,
                        TStatesCount = 19,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 } },
                        TStatesByMachineCycle = "19 (4, 4, 3, 5, 3)"
                    }
                }
                },

                UserManualPage = "84\n85",
                Description = "The operand (IX+d), (the contents of the Index Register IX summed with\na two�s complement displacement integer d) is loaded to register r, where r identifies register A, B, C, D, E, H, or L.",
                ConditionBitsAffected = "None",
                Example = "If the Index Register IX contains the number 25AFH, the instruction LD B,\n(IX+19H) causes the calculation of the sum 25AFH + 19H, which points\nto memory location 25C8H. If this address contains byte 39H, the\ninstruction results in register B also containing 39H."
            };

            Table[64] = new InstructionType()
            {
                Index = 64,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "16-Bit Load",
                OpCodeName = "LD",
                Param1Type = AddressingMode.Register16,
                Param2Type = AddressingMode.Immediate16,
                Equation = "dd <- nn",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 4,
                    Param1List = new string[] { "BC","DE","HL","SP" },
                    Param2List = new string[] { "nn" },
                    InstructionCodeSizeInBytes = 3,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 3,
                        TStatesCount = 10,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.ODL, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.ODH, TStates = 3 } },
                        TStatesByMachineCycle = "10 (4, 3, 3)"
                    }
                },
                new InstructionParametersVariant() {
                    Index = 1,
                    IsUndocumented = false,
                    InstructionCodeCount = 2,
                    Param1List = new string[] { "IX","IY" },
                    Param2List = new string[] { "nn" },
                    InstructionCodeSizeInBytes = 4,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 4,
                        TStatesCount = 14,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.ODL, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.ODH, TStates = 3 } },
                        TStatesByMachineCycle = "14 (4, 4, 3, 3)"
                    }
                }
                },

                UserManualPage = "102",
                Description = "The 2-byte integer nn is loaded to the dd register pair, where dd defines the\nBC, DE, HL, or SP register pairs.",
                ConditionBitsAffected = "None",
                Example = "At execution of LD HL, 5000H the contents of the HL register pair is 5000H."
            };

            Table[65] = new InstructionType()
            {
                Index = 65,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "16-Bit Load",
                OpCodeName = "LD",
                Param1Type = AddressingMode.Register16,
                Param2Type = AddressingMode.Register16,
                Equation = "SP <- HL",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "SP" },
                    Param2List = new string[] { "HL" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 1,
                        TStatesCount = 6,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 6 } },
                        TStatesByMachineCycle = "1 (6)"
                    }
                },
                new InstructionParametersVariant() {
                    Index = 1,
                    IsUndocumented = false,
                    InstructionCodeCount = 2,
                    Param1List = new string[] { "SP" },
                    Param2List = new string[] { "IX","IY" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 10,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 6 } },
                        TStatesByMachineCycle = "10 (4, 6)"
                    }
                }
                },

                UserManualPage = "113",
                Description = "The contents of the register pair HL are loaded to the Stack Pointer (SP)",
                ConditionBitsAffected = "None",
                Example = "If the register pair HL contains 442EH, at instruction LD SP, HL the Stack\nPointer also contains 442EH."
            };

            Table[66] = new InstructionType()
            {
                Index = 66,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "16-Bit Load",
                OpCodeName = "LD",
                Param1Type = AddressingMode.Register16,
                Param2Type = AddressingMode.Extended,
                Equation = "H <- (nn+1), L <- (nn)",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "HL" },
                    Param2List = new string[] { "(nn)" },
                    InstructionCodeSizeInBytes = 3,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 5,
                        TStatesCount = 16,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.ODL, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.ODH, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.MRL, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.MRH, TStates = 3 } },
                        TStatesByMachineCycle = "16 (4, 3, 3, 3, 3)"
                    }
                },
                new InstructionParametersVariant() {
                    Index = 1,
                    IsUndocumented = false,
                    InstructionCodeCount = 5,
                    Param1List = new string[] { "BC","DE","IX","IY","SP" },
                    Param2List = new string[] { "(nn)" },
                    InstructionCodeSizeInBytes = 4,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 6,
                        TStatesCount = 20,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.ODL, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.ODH, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.MRL, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.MRH, TStates = 3 } },
                        TStatesByMachineCycle = "20 (4, 4, 3, 3, 3, 3)"
                    }
                },
                new InstructionParametersVariant() {
                    Index = 2,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "HL" },
                    Param2List = new string[] { "(nn)" },
                    InstructionCodeSizeInBytes = 4,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 6,
                        TStatesCount = 20,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.ODL, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.ODH, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.MRL, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.MRH, TStates = 3 } },
                        TStatesByMachineCycle = "20 (4, 4, 3, 3, 3, 3)"
                    }
                }
                },

                UserManualPage = "105",
                Description = "The contents of memory address (nn) are loaded to the low order portion of\nregister pair HL (register L), and the contents of the next highest memory\naddress (nn+1) are loaded to the high order portion of HL (register H). The\nfirst n operand after the Op Code is the low order byte of nn.",
                ConditionBitsAffected = "None",
                Example = "If address 4545H contains 37H, and address 4546H contains A1H, at\ninstruction LD HL, (4545H) the HL register pair contains A137H."
            };

            Table[67] = new InstructionType()
            {
                Index = 67,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Load",
                OpCodeName = "LD",
                Param1Type = AddressingMode.Extended,
                Param2Type = AddressingMode.Register,
                Equation = "(nn) <- A",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "(nn)" },
                    Param2List = new string[] { "A" },
                    InstructionCodeSizeInBytes = 3,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 4,
                        TStatesCount = 13,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.ODL, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.ODH, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "13 (4, 3, 3, 3)"
                    }
                }
                },

                UserManualPage = "97",
                Description = "The contents of the Accumulator are loaded to the memory address\nspecified by the operand nn. The first n operand after the Op Code is the\nlow order byte of nn.",
                ConditionBitsAffected = "None",
                Example = "If the contents of the Accumulator are byte D7H, at execution of\nLD (3141 H), AD7H results in memory location 3141H."
            };

            Table[68] = new InstructionType()
            {
                Index = 68,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "16-Bit Load",
                OpCodeName = "LD",
                Param1Type = AddressingMode.Extended,
                Param2Type = AddressingMode.Register16,
                Equation = "(nn+1) <- H, (nn) <- L",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "(nn)" },
                    Param2List = new string[] { "HL" },
                    InstructionCodeSizeInBytes = 3,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 5,
                        TStatesCount = 16,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.ODL, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.ODH, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.MWL, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.MWH, TStates = 3 } },
                        TStatesByMachineCycle = "16 (4, 3, 3, 3, 3)"
                    }
                },
                new InstructionParametersVariant() {
                    Index = 1,
                    IsUndocumented = false,
                    InstructionCodeCount = 5,
                    Param1List = new string[] { "(nn)" },
                    Param2List = new string[] { "BC","DE","IX","IY","SP" },
                    InstructionCodeSizeInBytes = 4,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 6,
                        TStatesCount = 20,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.ODL, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.ODH, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.MWL, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.MWH, TStates = 3 } },
                        TStatesByMachineCycle = "20 (4, 4, 3, 3, 3, 3)"
                    }
                },
                new InstructionParametersVariant() {
                    Index = 2,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "(nn)" },
                    Param2List = new string[] { "HL" },
                    InstructionCodeSizeInBytes = 4,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 6,
                        TStatesCount = 20,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.ODL, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.ODH, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.MWL, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.MWH, TStates = 3 } },
                        TStatesByMachineCycle = "20 (4, 4, 3, 3, 3, 3)"
                    }
                }
                },

                UserManualPage = "109",
                Description = "The contents of the low order portion of register pair HL (register L) are\nloaded to memory address (nn), and the contents of the high order portion\nof HL (register H) are loaded to the next highest memory address (nn+1).\nThe first n operand after the Op Code is the low order byte of nn.",
                ConditionBitsAffected = "None",
                Example = "If the content of register pair HL is 483AH, at instruction\nLD (B2291-1), HL address B229H contains 3AH, and address B22AH\ncontains 48H."
            };

            Table[69] = new InstructionType()
            {
                Index = 69,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Load",
                OpCodeName = "LD",
                Param1Type = AddressingMode.RegisterIndirect,
                Param2Type = AddressingMode.Immediate,
                Equation = "(HL) <- n",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "(HL)" },
                    Param2List = new string[] { "n" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 3,
                        TStatesCount = 10,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "10 (4, 3, 3)"
                    }
                }
                },

                UserManualPage = "89",
                Description = "Integer n is loaded to the memory address specified by the contents of the\nHL register pair.",
                ConditionBitsAffected = "None",
                Example = "If the HL register pair contains 4444H, the instruction LD (HL), 28H\nresults in the memory location 4444H containing byte 28H."
            };

            Table[70] = new InstructionType()
            {
                Index = 70,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Load",
                OpCodeName = "LD",
                Param1Type = AddressingMode.RegisterIndirect,
                Param2Type = AddressingMode.Register,
                Equation = "(HL) <- r",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 7,
                    Param1List = new string[] { "(HL)" },
                    Param2List = new string[] { "A","B","C","D","E","H","L" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 7,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "7 (4, 3)"
                    }
                },
                new InstructionParametersVariant() {
                    Index = 1,
                    IsUndocumented = false,
                    InstructionCodeCount = 2,
                    Param1List = new string[] { "(BC)","(DE)" },
                    Param2List = new string[] { "A" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 7,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "7 (4, 3)"
                    }
                }
                },

                UserManualPage = "86",
                Description = "The contents of register r are loaded to the memory location specified by\nthe contents of the HL register pair. The symbol r identifies register A, B,\nC, D, E, H, or L.",
                ConditionBitsAffected = "None",
                Example = "If the contents of register pair HL specifies memory location 2146H, and\nthe B register contains byte 29H, at execution of LD (HL), B memory\naddress 2146H also contains 29H."
            };

            Table[71] = new InstructionType()
            {
                Index = 71,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Load",
                OpCodeName = "LD",
                Param1Type = AddressingMode.Indexed,
                Param2Type = AddressingMode.Immediate,
                Equation = "(IX+d) <- n",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 2,
                    Param1List = new string[] { "(IX+d)","(IY+d)" },
                    Param2List = new string[] { "n" },
                    InstructionCodeSizeInBytes = 4,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 5,
                        TStatesCount = 19,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "19 (4, 4, 3,5,3)"
                    }
                }
                },

                UserManualPage = "90\n91",
                Description = "The n operand is loaded to the memory address specified by the sum of the\nIndex Register IX and the two�s complement displacement operand d.",
                ConditionBitsAffected = "None",
                Example = "If the Index Register IX contains the number 219AH, the instruction\nLD (IX+5H), 5AH results in byte 5AH in the memory address 219FH."
            };

            Table[72] = new InstructionType()
            {
                Index = 72,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Load",
                OpCodeName = "LD",
                Param1Type = AddressingMode.Indexed,
                Param2Type = AddressingMode.Register,
                Equation = "(IX+d) <- r",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 14,
                    Param1List = new string[] { "(IX+d)","(IY+d)" },
                    Param2List = new string[] { "A","B","C","D","E","H","L" },
                    InstructionCodeSizeInBytes = 3,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 5,
                        TStatesCount = 19,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "19 (4, 4, 3, 5, 3)"
                    }
                }
                },

                UserManualPage = "87\n88",
                Description = "The contents of register r are loaded to the memory address specified by the\ncontents of Index Register IX summed with d, a two�s complement\ndisplacement integer. The symbol r identifies register A, B, C, D, E, H, or\nL.",
                ConditionBitsAffected = "None",
                Example = "If the C register contains byte 1CH, and the Index Register IX contains\n3100H, then the instruction LID (IX+6H), C performs the sum 3100H +\n6H and loads 1CH to memory location 3106H."
            };

            Table[73] = new InstructionType()
            {
                Index = 73,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Exchange, Block Transfer, and Search",
                OpCodeName = "LDD",
                Equation = "(DE) <- (HL), DE <- DE -1, HL <- HL-1, BC <- BC-1",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 4,
                        TStatesCount = 16,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 5 } },
                        TStatesByMachineCycle = "16 (4, 4, 3, 5)"
                    }
                }
                },

                UserManualPage = "131",
                Description = "This 2-byte instruction transfers a byte of data from the memory location\naddressed by the contents of the HL register pair to the memory location\naddressed by the contents of the DE register pair. Then both of these register\npairs including the BC (Byte Counter) register pair are decremented.",
                ConditionBitsAffected = "S is not affected\nZ is not affected\nH is reset\nP/V is set if BC -1 ? 0, reset otherwise\nN is reset\nC is not affected",
                Example = "If the HL register pair contains 1111H, memory location 1111H contains\nbyte 88H, the DE register pair contains 2222H, memory location 2222H\ncontains byte 66H, and the BC register pair contains 7H, then instruction\nLDD results in the following contents in register pairs and memory\naddresses:\nHL contains 1110H\n(1111H) contains 88H\nDE contains 2221H\n(2222H) contains 88H\nBC contains 6H"
            };

            Table[74] = new InstructionType()
            {
                Index = 74,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Exchange, Block Transfer, and Search",
                OpCodeName = "LDDR",
                Equation = "(DE) <- (HL), DE <- DE -1, HL <- HL-1, BC <- BC-1",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        Condition = "BC!=0",
                        MachineCyclesCount = 5,
                        TStatesCount = 21,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 5 } },
                        TStatesByMachineCycle = "21 (4, 4, 3, 5, 5)"
                    },
                    AlternateExecutionTimings = new InstructionExecutionVariant() {
                        Condition = "BC==0",
                        MachineCyclesCount = 4,
                        TStatesCount = 16,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 5 } },
                        TStatesByMachineCycle = "16 (4, 4, 3, 5)"
                    }
                }
                },

                UserManualPage = "132",
                Description = "This 2-byte instruction transfers a byte of data from the memory\nlocation addressed by the contents of the HL register pair to the memory\nlocation addressed by the contents of the DE register pair. Then both of\nthese registers, as well as the BC (Byte Counter), are decremented. If\ndecrementing causes BC to go to zero, the instruction is terminated. If\nBC is not zero, the program counter is decremented by two and the\ninstruction is repeated. Interrupts are recognized and two refresh cycles\nexecute after each data transfer.\nWhen BC is set to zero, prior to instruction execution, the instruction loops\nthrough 64 Kbytes.",
                ConditionBitsAffected = "S is not affected\nZ is not affected\nH is reset\nP/V is reset\nN is reset\nC is not affected",
                Example = "If the HL register pair contains 1114H, the DE register pair contains\n2225H, the BC register pair contains 0003H, and memory locations have\nthese contents:\n(1114H) contains A5H (2225H) contains C5H\n(1113H) contains 36H (2224H) contains 59H\n(1112H) contains 88H (2223H) contains 66H\nThen at execution of LDDR the contents of register pairs and memory\nlocations are:\nHL contains 1111H\nDE contains 2222H\nDC contains 0000H\n(1114H) contains A5H (2225H) contains A5H\n(1113H) contains 36H (2224H) contains 36H\n(1112H) contains 88H (2223H) contains 88H"
            };

            Table[75] = new InstructionType()
            {
                Index = 75,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Exchange, Block Transfer, and Search",
                OpCodeName = "LDI",
                Equation = "(DE) <- (HL), DE <- DE + 1, HL <- HL + 1, BC <- BC -1",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 4,
                        TStatesCount = 16,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 5 } },
                        TStatesByMachineCycle = "16 (4, 4, 3, 5)"
                    }
                }
                },

                UserManualPage = "128",
                Description = "A byte of data is transferred from the memory location addressed, by the\ncontents of the HL register pair to the memory location addressed by the\ncontents of the DE register pair. Then both these register pairs are\nincremented and the BC (Byte Counter) register pair is decremented.",
                ConditionBitsAffected = "S is not affected\nZ is not affected\nH is reset\nP/V is set if BC -1 ? 0, reset otherwise\nN is reset\nC is not affected",
                Example = "If the HL register pair contains 1111H, memory location 1111H contains\nbyte 88H, the DE register pair contains 2222H, the memory location 2222H\ncontains byte 66H, and the BC register pair contains 7H, then the instruction\nLDI results in the following contents in register pairs and memory addresses:\nHL contains 1112H\n(1111H) contains 88H\nDE contains 2223H\n(2222H) contains 88H\nBC contains 6H"
            };

            Table[76] = new InstructionType()
            {
                Index = 76,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Exchange, Block Transfer, and Search",
                OpCodeName = "LDIR",
                Equation = "(DE) <- (HL), DE <- DE + 1, HL <- HL + 1, BC <- BC -1",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        Condition = "BC!=0",
                        MachineCyclesCount = 5,
                        TStatesCount = 21,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 5 } },
                        TStatesByMachineCycle = "21 (4, 4, 3, 5, 5)"
                    },
                    AlternateExecutionTimings = new InstructionExecutionVariant() {
                        Condition = "BC==0",
                        MachineCyclesCount = 4,
                        TStatesCount = 16,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 5 } },
                        TStatesByMachineCycle = "16 (4, 4, 3, 5)"
                    }
                }
                },

                UserManualPage = "129",
                Description = "This 2-byte instruction transfers a byte of data from the memory location\naddressed by the contents of the HL register pair to the memory location\naddressed by the DE register pair. Both these register pairs are incremented\nand the BC (Byte Counter) register pair is decremented. If decrementing\ncauses the BC to go to zero, the instruction is terminated. If BC is not zero,\nthe program counter is decremented by two and the instruction is repeated.\nInterrupts are recognized and two refresh cycles are executed after each\ndata transfer. When BC is set to zero prior to instruction execution, the\ninstruction loops through 64 Kbytes.",
                ConditionBitsAffected = "S is not affected\nZ is not affected\nH is reset\nP/V is reset\nN is reset\nC is not affected",
                Example = "2222H, the BC register pair contains 0003H, and memory locations have\nthese contents:\n(1111H) contains 88H (2222H) contains 66H\n(1112H) contains 36H (2223H) contains 59H\n(1113H) contains A5H (2224H) contains C5H\nthen at execution of LDIR the contents of register pairs and memory\nlocations are:\nHL contains 1114H\nDE contains 2225H\nBC contains 0000H\n(1111H) contains 88H (2222H) contains 88H\n(1112H) contains 36H (2223H) contains 36H\n(1113H) contains A5H (2224H) contains A5H"
            };

            Table[77] = new InstructionType()
            {
                Index = 77,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "General-Purpose Arithmetic",
                OpCodeName = "NEG",
                Equation = "A <- 0 - A",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 8,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "8 (4, 4)"
                    }
                }
                },

                UserManualPage = "169",
                Description = "The contents of the Accumulator are negated (two�s complement). This is\nthe same as subtracting the contents of the Accumulator from zero. Note\nthat 80H is left unchanged.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is 0, reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is set if Accumulator was 80H before operation, reset otherwise\nN is set\nC is set if Accumulator was not 00H before operation, reset otherwise",
                Example = "If the contents of the Accumulator are\n1 0 0 1 1 0 0 0\nat execution of NEG the Accumulator contents are\n0 1 1 0 1 0 0 0"
            };

            Table[78] = new InstructionType()
            {
                Index = 78,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "CPU Control",
                OpCodeName = "NOP",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 1,
                        TStatesCount = 4,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "1 (4)"
                    }
                },
                new InstructionParametersVariant() {
                    Index = 1,
                    IsUndocumented = true,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 8,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "8 (4,4)"
                    }
                }
                },

                UserManualPage = "172",
                Description = "The CPU performs no operation during this machine cycle.",
                ConditionBitsAffected = "None"
            };

            Table[79] = new InstructionType()
            {
                Index = 79,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "OR",
                Param1Type = AddressingMode.Immediate,
                Equation = "A <- A | s",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "n" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 7,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 } },
                        TStatesByMachineCycle = "7 (4, 3)"
                    }
                }
                },

                UserManualPage = "154",
                Description = "A logical OR operation is performed between the byte specified by the s\noperand and the byte contained in the Accumulator, the result is stored in\nthe Accumulator.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if overflow, reset otherwise\nN is reset\nC is reset",
                Example = "If the H register contains 48H (0100 0100), and the Accumulator contains\n12H (0001 0010), at execution of OR H the Accumulator contains 5AH\n(0101 1010)."
            };

            Table[80] = new InstructionType()
            {
                Index = 80,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "OR",
                Param1Type = AddressingMode.Register,
                Equation = "A <- A | s",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 7,
                    Param1List = new string[] { "A","B","C","D","E","H","L" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 1,
                        TStatesCount = 4,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "1 (4)"
                    }
                },
                new InstructionParametersVariant() {
                    Index = 1,
                    IsUndocumented = true,
                    InstructionCodeCount = 4,
                    Param1List = new string[] { "IXh","IXl","IYh","IYl" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 8,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "8 (4, 4)"
                    }
                }
                },

                UserManualPage = "154",
                Description = "A logical OR operation is performed between the byte specified by the s\noperand and the byte contained in the Accumulator, the result is stored in\nthe Accumulator.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if overflow, reset otherwise\nN is reset\nC is reset",
                Example = "If the H register contains 48H (0100 0100), and the Accumulator contains\n12H (0001 0010), at execution of OR H the Accumulator contains 5AH\n(0101 1010)."
            };

            Table[81] = new InstructionType()
            {
                Index = 81,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "OR",
                Param1Type = AddressingMode.RegisterIndirect,
                Equation = "A <- A | s",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "(HL)" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 7,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 } },
                        TStatesByMachineCycle = "7 (4, 3)"
                    }
                }
                },

                UserManualPage = "154",
                Description = "A logical OR operation is performed between the byte specified by the s\noperand and the byte contained in the Accumulator, the result is stored in\nthe Accumulator.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if overflow, reset otherwise\nN is reset\nC is reset",
                Example = "If the H register contains 48H (0100 0100), and the Accumulator contains\n12H (0001 0010), at execution of OR H the Accumulator contains 5AH\n(0101 1010)."
            };

            Table[82] = new InstructionType()
            {
                Index = 82,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "OR",
                Param1Type = AddressingMode.Indexed,
                Equation = "A <- A | s",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 2,
                    Param1List = new string[] { "(IX+d)","(IY+d)" },
                    InstructionCodeSizeInBytes = 3,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 5,
                        TStatesCount = 19,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 } },
                        TStatesByMachineCycle = "19 (4, 4, 3, 5, 3)"
                    }
                }
                },

                UserManualPage = "154",
                Description = "A logical OR operation is performed between the byte specified by the s\noperand and the byte contained in the Accumulator, the result is stored in\nthe Accumulator.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if overflow, reset otherwise\nN is reset\nC is reset",
                Example = "If the H register contains 48H (0100 0100), and the Accumulator contains\n12H (0001 0010), at execution of OR H the Accumulator contains 5AH\n(0101 1010)."
            };

            Table[83] = new InstructionType()
            {
                Index = 83,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Input and Output",
                OpCodeName = "OTDR",
                Equation = "(C) <- (HL), B <- B -1, HL <- HL - 1",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        Condition = "B!=0 ",
                        MachineCyclesCount = 5,
                        TStatesCount = 21,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.PW, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 5 } },
                        TStatesByMachineCycle = "21 (4, 5, 3, 4, 5)"
                    },
                    AlternateExecutionTimings = new InstructionExecutionVariant() {
                        Condition = "B=0",
                        MachineCyclesCount = 4,
                        TStatesCount = 16,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.PW, TStates = 4 } },
                        TStatesByMachineCycle = "16 (4, 5, 3, 4)"
                    }
                }
                },

                UserManualPage = "286",
                Description = "The contents of the HL register pair are placed on the address bus to select a\nlocation in memory. The byte contained in this memory location is temporarily\nstored in the CPU. Then, after the byte counter (B) is decremented,\nthe contents of register C are placed on the bottom half (A0 through A7) of\nthe address bus to select the I/O device at one of 256 possible ports. Register\nB may be used as a byte counter, and its decremented value is placed on\nthe top half (A8 through A15) of the address bus at this time. Next, the byte\nto be output is placed on the data bus and written to the selected peripheral\ndevice. Then, register pair HL is decremented and if the decremented B\nregister is not zero, the Program Counter (PC) is decremented by two and\nthe instruction is repeated. If B has gone to zero, the instruction is terminated.\nInterrupts are recognized and two refresh cycles are executed after\neach data transfer.\nNote: When B is set to zero prior to instruction execution, the instruction\noutputs 256 bytes of data.",
                ConditionBitsAffected = "Z is set\nH is unknown\nP/V is unknown\nN is set\nC is not affected",
                Example = "If the contents of register C are 07H, the contents of register B are 03H, the\ncontents of the HL register pair are 1000H, and memory locations have the\nfollowing contents:\n0FFEH contains 51H\n0FFFH contains A9H\n1000H contains 03H\nthen at execution of OTDR the HL register pair contain 0FFDH, register B\ncontains zero, and a group of bytes is written to the peripheral device\nmapped to I/O port address 07H in the following sequence:\n03H\nA9H\n51H"
            };

            Table[84] = new InstructionType()
            {
                Index = 84,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Input and Output",
                OpCodeName = "OTIR",
                Equation = "(C) <- (HL), B <- B -1, HL <- HL + 1",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        Condition = "B!=0",
                        MachineCyclesCount = 5,
                        TStatesCount = 21,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.PW, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 5 } },
                        TStatesByMachineCycle = "21 (4, 5, 3, 4, 5)"
                    },
                    AlternateExecutionTimings = new InstructionExecutionVariant() {
                        Condition = "B=0",
                        MachineCyclesCount = 4,
                        TStatesCount = 16,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.PW, TStates = 4 } },
                        TStatesByMachineCycle = "16 (4, 5, 3, 4)"
                    }
                }
                },

                UserManualPage = "283",
                Description = "The contents of the HL register pair are placed on the address bus to select\na location in memory. The byte contained in this memory location is temporarily\nstored in the CPU. Then, after the byte counter (B) is decremented, the\ncontents of register C are placed on the bottom half (A0 through A7) of the\naddress bus to select the I/O device at one of 256 possible ports. Register B\nmay be used as a byte counter, and its decremented value is placed on the top\nhalf (A8 through A15) of the address bus at this time. Next, the byte to be\noutput is placed on the data bus and written to the selected peripheral device.\nThen register pair HL is incremented. If the decremented B register is not\nzero, the Program Counter (PC) is decremented by two and the instruction is\nrepeated. If B has gone to zero, the instruction is terminated. Interrupts are\nrecognized and two refresh cycles are executed after each data transfer.\nNote: When B is set to zero prior to instruction execution, the instruction\noutputs 256 bytes of data.",
                ConditionBitsAffected = "S is unknown\nZ is set\nH is unknown\nP/V is unknown\nN is set\nC is not affected",
                Example = "If the contents of register C are 07H, the contents of register B are 03H, the\ncontents of the HL register pair are 1000H, and memory locations have the\nfollowing contents:\n1000H contains 51H\n1001H contains A9H\n1002H contains 03H\nthen at execution of OTIR the HL register pair contains 1003H, register B\ncontains zero, and a group of bytes is written to the peripheral device\nmapped to I/O port address 07H in the following sequence:\n51H\nA9H\n03H"
            };

            Table[85] = new InstructionType()
            {
                Index = 85,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Input and Output",
                OpCodeName = "OUT",
                Param1Type = AddressingMode.IOPortImmediate,
                Param2Type = AddressingMode.Register,
                Equation = "(n) <- A",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "(n)" },
                    Param2List = new string[] { "A" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 3,
                        TStatesCount = 11,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.PW, TStates = 4 } },
                        TStatesByMachineCycle = "11 (4, 3, 4)"
                    }
                }
                },

                UserManualPage = "279",
                Description = "The operand n is placed on the bottom half (A0 through A7) of the address\nbus to select the I/O device at one of 256 possible ports. The contents of the\nAccumulator (register A) also appear on the top half (A8 through A15) of\nthe address bus at this time. Then the byte contained in the Accumulator is\nplaced on the data bus and written to the selected peripheral device.",
                ConditionBitsAffected = "None",
                Example = "If the contents of the Accumulator are 23H, at execution of OUT (01H),\nbyte 23H is written to the peripheral device mapped to I/O port address 01H."
            };

            Table[86] = new InstructionType()
            {
                Index = 86,
                IsUndocumented = true,
                IsInternalZ80Operation = false,

                InstructionGroup = "Input and Output",
                OpCodeName = "OUT",
                Param1Type = AddressingMode.IOPortRegister,
                Equation = "(C) <- 0",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = true,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "(C)" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 3,
                        TStatesCount = 12,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.PW, TStates = 4 } },
                        TStatesByMachineCycle = "12 (4, 4, 4)"
                    }
                }
                },

                Description = "ED71 simply outs the value 0 to I/O port C.",
                ConditionBitsAffected = "None",
                Example = "If the contents of register C are 01H, at execution of OUT (C), byte 0 is written to the peripheral device\nmapped to I/O port address 01H."
            };

            Table[87] = new InstructionType()
            {
                Index = 87,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Input and Output",
                OpCodeName = "OUT",
                Param1Type = AddressingMode.IOPortRegister,
                Param2Type = AddressingMode.Register,
                Equation = "(C) <- r",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 7,
                    Param1List = new string[] { "(C)" },
                    Param2List = new string[] { "A","B","C","D","E","H","L" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 3,
                        TStatesCount = 12,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.PW, TStates = 4 } },
                        TStatesByMachineCycle = "12 (4, 4, 4)"
                    }
                }
                },

                UserManualPage = "280",
                Description = "The contents of register C are placed on the bottom half (A0 through A7) of\nthe address bus to select the I/O device at one of 256 possible ports. The\ncontents of Register B are placed on the top half (A8 through A15) of the\naddress bus at this time. Then the byte contained in register r is placed on\nthe data bus and written to the selected peripheral device. Register r\nidentifies any of the CPU registers shown in the following table :\nB, C, D, E, H, L, A",
                ConditionBitsAffected = "None",
                Example = "If the contents of register C are 01H, and the contents of register D are 5AH,\nat execution of OUT (C),D byte 5AH is written to the peripheral device\nmapped to I/O port address 01H."
            };

            Table[88] = new InstructionType()
            {
                Index = 88,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Input and Output",
                OpCodeName = "OUTD",
                Equation = "(C) <- (HL), B <- B -1, HL <- HL - 1",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 4,
                        TStatesCount = 16,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.PW, TStates = 4 } },
                        TStatesByMachineCycle = "16 (4, 5, 3, 4)"
                    }
                }
                },

                UserManualPage = "285",
                Description = "The contents of the HL register pair are placed on the address bus to select a\nlocation in memory. The byte contained in this memory location is\ntemporarily stored in the CPU. Then, after the byte counter (B) is\ndecremented, the contents of register C are placed on the bottom half (A0\nthrough A7) of the address bus to select the I/O device at one of 256\npossible ports. Register B may be used as a byte counter, and its\ndecremented value is placed on the top half (A8 through A15) of the\naddress bus at this time. Next, the byte to be output is placed on the data bus\nand written to the selected peripheral device. Finally, the register pair HL is\ndecremented.",
                ConditionBitsAffected = "S is unknown\nZ is set if B�1 = 0, reset otherwise\nH is unknown\nP/V is unknown\nN is set\nC is not affected",
                Example = "If the contents of register C are 07H, the contents of register B are 10H, the\ncontents of the HL register pair are 1000H, and the contents of memory\nlocation 1000H are 59H, at execution of OUTD register B contains 0FH, the\nHL register pair contains 0FFFH, and byte 59H is written to the peripheral\ndevice mapped to I/O port address 07H."
            };

            Table[89] = new InstructionType()
            {
                Index = 89,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Input and Output",
                OpCodeName = "OUTI",
                Equation = "(C) <- (HL), B <- B -1, HL <- HL + 1",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 4,
                        TStatesCount = 16,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.PW, TStates = 4 } },
                        TStatesByMachineCycle = "16 (4, 5, 3, 4)"
                    }
                }
                },

                UserManualPage = "282",
                Description = "The contents of the HL register pair are placed on the address bus to select a\nlocation in memory. The byte contained in this memory location is\ntemporarily stored in the CPU. Then, after the byte counter (B) is\ndecremented, the contents of register C are placed on the bottom half (A0\nthrough A7) of the address bus to select the I/O device at one of 256\npossible ports. Register B may be used as a byte counter, and its\ndecremented value is placed on the top half (A8 through A15) of the\naddress bus. The byte to be output is placed on the data bus and written to a\nselected peripheral device. Finally, the register pair HL is incremented.",
                ConditionBitsAffected = "S is unknown\nZ is set if B�1 = 0, reset otherwise\nH is unknown\nP/V is unknown\nN is set\nC is not affected",
                Example = "If the contents of register C are 07H, the contents of register B are 10H, the\ncontents of the HL register pair are 100014, and the contents of memory\naddress 1000H are 5914, then after thee execution of OUTI register B\ncontains 0FH, the HL register pair contains 1001H, and byte 59H is written\nto the peripheral device mapped to I/O port address 07H."
            };

            Table[90] = new InstructionType()
            {
                Index = 90,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "16-Bit Load",
                OpCodeName = "POP",
                Param1Type = AddressingMode.Register16,
                Equation = "qqH <- (SP+1), qqL <- (SP)",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 4,
                    Param1List = new string[] { "AF","BC","DE","HL" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 3,
                        TStatesCount = 10,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.SRL, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.SRH, TStates = 3 } },
                        TStatesByMachineCycle = "10 (4, 3, 3)",
                        StackOperations = "M2 +1, M3 +1"
                    }
                },
                new InstructionParametersVariant() {
                    Index = 1,
                    IsUndocumented = false,
                    InstructionCodeCount = 2,
                    Param1List = new string[] { "IX","IY" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 4,
                        TStatesCount = 14,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.SRL, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.SRH, TStates = 3 } },
                        TStatesByMachineCycle = "14 (4, 4, 3, 3)",
                        StackOperations = "M3 +1, M4 +1"
                    }
                }
                },

                UserManualPage = "119",
                Description = "The top two bytes of the external memory LIFO (last-in, first-out) Stack\nare popped to register pair qq. The Stack Pointer (SP) register pair holds\nthe 16-bit address of the current top of the Stack. This instruction first\nloads to the low order portion of qq, the byte at memory location\ncorresponding to the contents of SP, then SP is incriminated and the\ncontents of the corresponding adjacent memory location are loaded to the\nhigh order portion of qq and the SP is now incriminated again. The\noperand qq identifies register pair BC, DE, HL, or AF.",
                ConditionBitsAffected = "None",
                Example = "If the Stack Pointer contains 1000H, memory location 1000H contains 55H,\nand location 1001H contains 33H, the instruction POP HL results in register\npair HL containing 3355H, and the Stack Pointer containing 1002H."
            };

            Table[91] = new InstructionType()
            {
                Index = 91,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "16-Bit Load",
                OpCodeName = "PUSH",
                Param1Type = AddressingMode.Register16,
                Equation = "(SP-2) <- qqL, (SP-1) <- qqH",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 4,
                    Param1List = new string[] { "AF","BC","DE","HL" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 3,
                        TStatesCount = 11,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.SWH, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.SWL, TStates = 3 } },
                        TStatesByMachineCycle = "11 (5, 3, 3)",
                        StackOperations = "M1 -1, M2 -1"
                    }
                },
                new InstructionParametersVariant() {
                    Index = 1,
                    IsUndocumented = false,
                    InstructionCodeCount = 2,
                    Param1List = new string[] { "IX","IY" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 4,
                        TStatesCount = 15,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.SWH, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.SWL, TStates = 3 } },
                        TStatesByMachineCycle = "15 (4, 5, 3, 3)",
                        StackOperations = "M2 -1, M3 -1"
                    }
                }
                },

                UserManualPage = "116",
                Description = "The contents of the register pair qq are pushed to the external memory\nLIFO (last-in, first-out) Stack. The Stack Pointer (SP) register pair holds the\n16-bit address of the current top of the Stack. This instruction first\ndecrements SP and loads the high order byte of register pair qq to the\nmemory address specified by the SP. The SP is decremented again and\nloads the low order byte of qq to the memory location corresponding to this\nnew address in the SP. The operand qq identifies register pair BC, DE, HL,\nor AF.",
                ConditionBitsAffected = "None",
                Example = "If the AF register pair contains 2233H and the Stack Pointer contains\n1007H, at instruction PUSH AF memory address 1006H contains 22H,\nmemory address 1005H contains 33H, and the Stack Pointer contains\n1005H."
            };

            Table[92] = new InstructionType()
            {
                Index = 92,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Bit Set, Reset, and Test",
                OpCodeName = "RES",
                Param1Type = AddressingMode.Bit,
                Param2Type = AddressingMode.Register,
                Equation = "sb <- 0",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 56,
                    Param1List = new string[] { "0","1","2","3","4","5","6","7" },
                    Param2List = new string[] { "A","B","C","D","E","H","L" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 8,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "8 (4, 4)"
                    }
                }
                },

                UserManualPage = "236",
                Description = "Operand b is any bit (7 through 0) of the contents of the m operand, (any of\nr, (HL), (IX+d), or (lY+d)) as defined for the analogous SET instructions.\nBit b in operand m is reset.",
                ConditionBitsAffected = "None",
                Example = "At execution of RES 6, D, bit 6 in register 0 resets. Bit 0 in register D is the\nleast-significant bit."
            };

            Table[93] = new InstructionType()
            {
                Index = 93,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Bit Set, Reset, and Test",
                OpCodeName = "RES",
                Param1Type = AddressingMode.Bit,
                Param2Type = AddressingMode.RegisterIndirect,
                Equation = "sb <- 0",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 8,
                    Param1List = new string[] { "0","1","2","3","4","5","6","7" },
                    Param2List = new string[] { "(HL)" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 4,
                        TStatesCount = 15,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "15 (4, 4, 4, 3)"
                    }
                }
                },

                UserManualPage = "236",
                Description = "Operand b is any bit (7 through 0) of the contents of the m operand, (any of\nr, (HL), (IX+d), or (lY+d)) as defined for the analogous SET instructions.\nBit b in operand m is reset.",
                ConditionBitsAffected = "None",
                Example = "At execution of RES 6, D, bit 6 in register 0 resets. Bit 0 in register D is the\nleast-significant bit."
            };

            Table[94] = new InstructionType()
            {
                Index = 94,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Bit Set, Reset, and Test",
                OpCodeName = "RES",
                Param1Type = AddressingMode.Bit,
                Param2Type = AddressingMode.Indexed,
                Equation = "sb <- 0",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 16,
                    Param1List = new string[] { "0","1","2","3","4","5","6","7" },
                    Param2List = new string[] { "(IX+d)","(IY+d)" },
                    InstructionCodeSizeInBytes = 4,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 6,
                        TStatesCount = 23,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.OC4, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "23 (4, 4, 3, 5, 4, 3)"
                    }
                }
                },

                UserManualPage = "236",
                Description = "Operand b is any bit (7 through 0) of the contents of the m operand, (any of\nr, (HL), (IX+d), or (lY+d)) as defined for the analogous SET instructions.\nBit b in operand m is reset.",
                ConditionBitsAffected = "None",
                Example = "At execution of RES 6, D, bit 6 in register 0 resets. Bit 0 in register D is the\nleast-significant bit."
            };

            Table[95] = new InstructionType()
            {
                Index = 95,
                IsUndocumented = true,
                IsInternalZ80Operation = false,

                InstructionGroup = "Bit Set, Reset, and Test",
                OpCodeName = "RES",
                Param1Type = AddressingMode.Bit,
                Param2Type = AddressingMode.Indexed,
                Param3Type = AddressingMode.Register,
                Equation = "sb <- 0\nr <- value",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = true,
                    InstructionCodeCount = 112,
                    Param1List = new string[] { "0","1","2","3","4","5","6","7" },
                    Param2List = new string[] { "(IX+d)","(IY+d)" },
                    Param3List = new string[] { "A","B","C","D","E","H","L" },
                    InstructionCodeSizeInBytes = 4,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 6,
                        TStatesCount = 23,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.OC4, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "23 (4, 4, 3, 5, 4, 3)"
                    }
                }
                },

                Description = "Operand b is any bit (7 through 0) of the contents of the m operand, (any of\nr, (HL), (IX+d), or (lY+d)) as defined for the analogous SET instructions.\nBit b in operand m is reset.\nThe undocumented DDCB instructions store the result (if any) of the operation in one of the\nseven all-purpose registers.",
                ConditionBitsAffected = "None",
                Example = "At execution of RES 6, D, bit 6 in register 0 resets. Bit 0 in register D is the\nleast-significant bit."
            };

            Table[96] = new InstructionType()
            {
                Index = 96,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Call And Return",
                OpCodeName = "RET",
                Equation = "pCL <- (sp), pCH <- (sp+1)",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 3,
                        TStatesCount = 10,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.SRL, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.SRH, TStates = 3 } },
                        TStatesByMachineCycle = "10 (4, 3, 3)",
                        StackOperations = "M2 +1, M3 +1"
                    }
                }
                },

                UserManualPage = "260",
                Description = "The byte at the memory location specified by the contents of the Stack\nPointer (SP) register pair is moved to the low order eight bits of the\nProgram Counter (PC). The SP is now incremented and the byte at the\nmemory location specified by the new contents of this instruction is fetched\nfrom the memory location specified by the PC. This instruction is normally\nused to return to the main line program at the completion of a routine\nentered by a CALL instruction.",
                ConditionBitsAffected = "None",
                Example = "If the contents of the Program Counter are 3535H, the contents of the Stack\nPointer are 2000H, the contents of memory location 2000H are B5H, and\nthe contents of memory location of memory location 2001H are 18H. At\nexecution of RET the contents of the Stack Pointer is 2002H, and the\ncontents of the Program Counter is 18B5H, pointing to the address of the\nnext program Op Code to be fetched."
            };

            Table[97] = new InstructionType()
            {
                Index = 97,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Call And Return",
                OpCodeName = "RET",
                Param1Type = AddressingMode.FlagCondition,
                Equation = "If cc true: PCL <- (sp), pCH <- (sp+1)",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 8,
                    Param1List = new string[] { "C","M","NC","NZ","P","PE","PO","Z" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        Condition = "cc == 1",
                        MachineCyclesCount = 3,
                        TStatesCount = 11,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.SRL, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.SRH, TStates = 3 } },
                        TStatesByMachineCycle = "11 (5, 3, 3)",
                        StackOperations = "M2 +1, M3 +1"
                    },
                    AlternateExecutionTimings = new InstructionExecutionVariant() {
                        Condition = "cc == 0",
                        MachineCyclesCount = 1,
                        TStatesCount = 5,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 5 } },
                        TStatesByMachineCycle = "1 (5)"
                    }
                }
                },

                UserManualPage = "261",
                Description = "If condition cc is true, the byte at the memory location specified by the\ncontents of the Stack Pointer (SP) register pair is moved to the low order\neight bits of the Program Counter (PC). The SP is incremented and the byte\nat the memory location specified by the new contents of the SP are moved\nto the high order eight bits of the PC. The SP is incremented again. The\nnext Op Code following this instruction is fetched from the memory\nlocation specified by the PC. This instruction is normally used to return to\nthe main line program at the completion of a routine entered by a CALL\ninstruction. If condition cc is false, the PC is simply incremented as usual,\nand the program continues with the next sequential instruction. Condition\ncc is programmed as one of eight status that correspond to condition bits in\nthe Flag Register (register F). These eight status are defined in the table below :\nCondition Flag\nNZ non zero Z\nZ zero Z\nNC no carry C\nC carry C\nPO parity odd P/V\nPE parity even P/V\nP sign positive S\nM sign negative S",
                ConditionBitsAffected = "None",
                Example = "If the S flag in the F register is set, the contents of the Program Counter are\n3535H, the contents of the Stack Pointer are 2000H, the contents of\nmemory location 2000H are B5H, and the contents of memory location\n2001H are 18H. At execution of RET M the contents of the Stack Pointer\nis 2002H, and the contents of the Program Counter is 18B5H, pointing to\nthe address of the next program Op Code to be fetched."
            };

            Table[98] = new InstructionType()
            {
                Index = 98,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Call And Return",
                OpCodeName = "RETI",
                Equation = "Return from Interrupt",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 4,
                        TStatesCount = 14,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.SRL, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.SRH, TStates = 3 } },
                        TStatesByMachineCycle = "14 (4, 4, 3, 3)",
                        StackOperations = "M3 +1, M4 +1"
                    }
                }
                },

                UserManualPage = "263",
                Description = "This instruction is used at the end of a maskable interrupt service routine to:\n- Restore the contents of the Program Counter (PC) (analogous to the\nRET instruction)\n- Signal an I/O device that the interrupt routine is completed. The RETI\ninstruction also facilitates the nesting of interrupts, allowing higher\npriority devices to temporarily suspend service of lower priority\nservice routines. However, this instruction does not enable interrupts\nthat were disabled when the interrupt routine was entered. Before\ndoing the RETI instruction, the enable interrupt instruction (EI)\nshould be executed to allow recognition of interrupts after completion\nof the current service routine.",
                ConditionBitsAffected = "None",
                Example = "Given: Two interrupting devices, with A and B connected in a daisy-chain\nconfiguration and A having a higher priority than B.\nB generates an interrupt and is acknowledged. The interrupt\nenable out, IEO, of B goes Low, blocking any lower priority\ndevices from interrupting while B is being serviced. Then A generates\nan interrupt, suspending service of B. The IEO of A goes\nLow, indicating that a higher priority device is being serviced.\nThe A routine is completed and a RETI is issued resetting the IEO\nof A, allowing the B routine to continue. A second RETI is issued\non completion of the B routine and the IE0 of B is reset (high)\nallowing lower priority devices interrupt access."
            };

            Table[99] = new InstructionType()
            {
                Index = 99,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Call And Return",
                OpCodeName = "RETN",
                Equation = "Return from non maskable interrupt",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 4,
                        TStatesCount = 14,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.SRL, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.SRH, TStates = 3 } },
                        TStatesByMachineCycle = "14 (4, 4, 3, 3)",
                        StackOperations = "M3 +1, M4 +1"
                    }
                }
                },

                UserManualPage = "265",
                Description = "This instruction is used at the end of a non-maskable interrupts service\nroutine to restore the contents of the Program Counter (PC) (analogous to\nthe RET instruction). The state of IFF2 is copied back to IFF1 so that\nmaskable interrupts are enabled immediately following the RETN if they\nwere enabled before the nonmaskable interrupt.",
                ConditionBitsAffected = "None",
                Example = "If the contents of the Stack Pointer are 1000H, and the contents of the\nProgram Counter are 1A45H, when a non maskable interrupt (NMI) signal\nis received, the CPU ignores the next instruction and instead restarts to\nmemory address 0066H. The current Program Counter contents of 1A45H\nis pushed onto the external stack address of 0FFFH and 0FFEH, high orderbyte\nfirst, and 0066H is loaded onto the Program Counter. That address\nbegins an interrupt service routine that ends with a RETN instruction.\nUpon the execution of RETN the former Program Counter contents are\npopped off the external memory stack, low order first, resulting in a Stack\nPointer contents again of 1000H. The program flow continues where it\nleft off with an Op Code fetch to address 1A45H, order-byte first, and\n0066H is loaded onto the Program Counter. That address begins an\ninterrupt service routine that ends with a RETN instruction. At execution of\nRETN the former Program Counter contents are popped off the external\nmemory stack, low order first, resulting in a Stack Pointer contents again of\n1000H. The program flow continues where it left off with an Op Code fetch\nto address 1A45H."
            };

            Table[100] = new InstructionType()
            {
                Index = 100,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "RL",
                Param1Type = AddressingMode.Register,

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 7,
                    Param1List = new string[] { "A","B","C","D","E","H","L" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 8,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "8 (4, 4)"
                    }
                }
                },

                UserManualPage = "202",
                Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nThe contents of the m operand are rotated left 1-bit position. The content of\nbit 7 is copied to the Carry flag and the previous content of the Carry flag is\ncopied to bit 0.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise\nN is reset\nC is data from bit 7 of source register",
                Example = "If the contents of register D and the Carry flag are\n0 - 1 0 0 0 1 1 1 1\nat execution of RL D the contents of register D and the Carry flag are\n1 - 0 0 0 1 1 1 1 0"
            };

            Table[101] = new InstructionType()
            {
                Index = 101,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "RL",
                Param1Type = AddressingMode.RegisterIndirect,

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "(HL)" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 4,
                        TStatesCount = 15,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "15 (4, 4, 4, 3)"
                    }
                }
                },

                UserManualPage = "202",
                Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nThe contents of the m operand are rotated left 1-bit position. The content of\nbit 7 is copied to the Carry flag and the previous content of the Carry flag is\ncopied to bit 0.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise\nN is reset\nC is data from bit 7 of source register",
                Example = "If the contents of register D and the Carry flag are\n0 - 1 0 0 0 1 1 1 1\nat execution of RL D the contents of register D and the Carry flag are\n1 - 0 0 0 1 1 1 1 0"
            };

            Table[102] = new InstructionType()
            {
                Index = 102,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "RL",
                Param1Type = AddressingMode.Indexed,

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 2,
                    Param1List = new string[] { "(IX+d)","(IY+d)" },
                    InstructionCodeSizeInBytes = 4,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 6,
                        TStatesCount = 23,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.OC4, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "23 (4, 4, 3, 5, 4, 3)"
                    }
                }
                },

                UserManualPage = "202",
                Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nThe contents of the m operand are rotated left 1-bit position. The content of\nbit 7 is copied to the Carry flag and the previous content of the Carry flag is\ncopied to bit 0.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise\nN is reset\nC is data from bit 7 of source register",
                Example = "If the contents of register D and the Carry flag are\n0 - 1 0 0 0 1 1 1 1\nat execution of RL D the contents of register D and the Carry flag are\n1 - 0 0 0 1 1 1 1 0"
            };

            Table[103] = new InstructionType()
            {
                Index = 103,
                IsUndocumented = true,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "RL",
                Param1Type = AddressingMode.Indexed,
                Param2Type = AddressingMode.Register,

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = true,
                    InstructionCodeCount = 14,
                    Param1List = new string[] { "(IX+d)","(IY+d)" },
                    Param2List = new string[] { "A","B","C","D","E","H","L" },
                    InstructionCodeSizeInBytes = 4,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 6,
                        TStatesCount = 23,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.OC4, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "23 (4, 4, 3, 5, 4, 3)"
                    }
                }
                },

                Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nThe contents of the m operand are rotated left 1-bit position. The content of\nbit 7 is copied to the Carry flag and the previous content of the Carry flag is\ncopied to bit 0.\nThe undocumented DDCB instructions store the result (if any) of the operation in one of the\nseven all-purpose registers.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise\nN is reset\nC is data from bit 7 of source register",
                Example = "If the contents of register D and the Carry flag are\n0 - 1 0 0 0 1 1 1 1\nat execution of RL D the contents of register D and the Carry flag are\n1 - 0 0 0 1 1 1 1 0"
            };

            Table[104] = new InstructionType()
            {
                Index = 104,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "RLA",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 1,
                        TStatesCount = 4,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "1 (4)"
                    }
                }
                },

                UserManualPage = "191",
                Description = "The contents of the Accumulator (register A) are rotated left 1-bit position\nthrough the Carry flag. The previous content of the Carry flag is copied to\nbit 0. Bit 0 is the least-significant bit.",
                ConditionBitsAffected = "S is not affected\nZ is not affected\nH is reset\nP/V is not affected\nN is reset\nC is data from bit 7 of Accumulator",
                Example = "If the contents of the Accumulator and the Carry flag are\n0 - 0 1 1 1 0 1 1 0\nat execution of RLA the contents of the Accumulator and the Carry flag are\n0 - 1 1 1 0 1 1 0 1"
            };

            Table[105] = new InstructionType()
            {
                Index = 105,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "RLC",
                Param1Type = AddressingMode.Register,

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 7,
                    Param1List = new string[] { "A","B","C","D","E","H","L" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 8,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "8 (4, 4)"
                    }
                }
                },

                UserManualPage = "194",
                Description = "The contents of register r are rotated left 1-bit position. The content of bit 7\nis copied to the Carry flag and also to bit 0.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise\nN is reset\nC is data from bit 7 of source register",
                Example = "If the contents of register r are\n1 0 0 0 1 0 0 0\nat execution of RLC r the contents of register r and the Carry flag are\n1 - 0 0 0 1 0 0 0 1"
            };

            Table[106] = new InstructionType()
            {
                Index = 106,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "RLC",
                Param1Type = AddressingMode.RegisterIndirect,

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "(HL)" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 4,
                        TStatesCount = 15,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "15 (4, 4, 4, 3)"
                    }
                }
                },

                UserManualPage = "196",
                Description = "The contents of the memory address specified by the contents of register\npair HL are rotated left 1-bit position. The content of bit 7 is copied to the\nCarry flag and also to bit 0. Bit 0 is the least-significant bit.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise\nN is reset\nC is data from bit 7 of source register",
                Example = "If the contents of the HL register pair are 2828H, and the contents of\nmemory location 2828H are\n1 0 0 0 1 0 0 0\nat execution of RLC(HL) the contents of memory location 2828H and the\nCarry flag are\n1 - 0 0 0 1 0 0 0 1"
            };

            Table[107] = new InstructionType()
            {
                Index = 107,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "RLC",
                Param1Type = AddressingMode.Indexed,

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 2,
                    Param1List = new string[] { "(IX+d)","(IY+d)" },
                    InstructionCodeSizeInBytes = 4,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 6,
                        TStatesCount = 23,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.OC4, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "23 (4, 4, 3, 5, 4, 3)"
                    }
                }
                },

                UserManualPage = "198\n200",
                Description = "The contents of the memory address specified by the sum of the contents of\nthe Index Register IX and a two�s complement displacement integer d, are\nrotated left 1-bit position. The content of bit 7 is copied to the Carry flag\nand also to bit 0. Bit 0 is the least-significant bit.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise\nN is reset\nC is data from bit 7 of source register",
                Example = "If the contents of the Index Register IX are 1000H, and the contents of\nmemory location 1022H are\n1 0 0 0 1 0 0 0\nat execution of RLC (IX+2H) the contents of memory location 1002H\nand the Carry flag are\n1 - 0 0 0 1 0 0 0 1"
            };

            Table[108] = new InstructionType()
            {
                Index = 108,
                IsUndocumented = true,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "RLC",
                Param1Type = AddressingMode.Indexed,
                Param2Type = AddressingMode.Register,

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = true,
                    InstructionCodeCount = 14,
                    Param1List = new string[] { "(IX+d)","(IY+d)" },
                    Param2List = new string[] { "A","B","C","D","E","H","L" },
                    InstructionCodeSizeInBytes = 4,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 6,
                        TStatesCount = 23,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.OC4, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "23 (4, 4, 3, 5, 4, 3)"
                    }
                }
                },

                Description = "The contents of the memory address specified by the sum of the contents of\nthe Index Register IX and a two�s complement displacement integer d, are\nrotated left 1-bit position. The content of bit 7 is copied to the Carry flag\nand also to bit 0. Bit 0 is the least-significant bit.\nThe undocumented DDCB instructions store the result (if any) of the operation in one of the\nseven all-purpose registers",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise\nN is reset\nC is data from bit 7 of source register",
                Example = "If the contents of the Index Register IX are 1000H, and the contents of\nmemory location 1022H are\n1 0 0 0 1 0 0 0\nat execution of RLC (IX+2H) the contents of memory location 1002H\nand the Carry flag are\n1 - 0 0 0 1 0 0 0 1"
            };

            Table[109] = new InstructionType()
            {
                Index = 109,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "RLCA",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 1,
                        TStatesCount = 4,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "1 (4)"
                    }
                }
                },

                UserManualPage = "190",
                Description = "The contents of the Accumulator (register A) are rotated left 1-bit position.\nThe sign bit (bit 7) is copied to the Carry flag and also to bit 0. Bit 0 is the\nleast-significant bit.",
                ConditionBitsAffected = "S is not affected\nZ is not affected\nH is reset\nP/V is not affected\nN is reset\nC is data from bit 7 of Accumulator",
                Example = "If the contents of the Accumulator are\n1 0 0 0 1 0 0 0\nat execution of RLCA the contents of the Accumulator and Carry flag are\n1 - 0 0 0 1 0 0 0 1"
            };

            Table[110] = new InstructionType()
            {
                Index = 110,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "RLD",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 5,
                        TStatesCount = 18,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "18 (4, 4, 3, 4, 3)"
                    }
                }
                },

                UserManualPage = "220",
                Description = "The contents of the low order four bits (bits 3, 2, 1, and 0) of the memory\nlocation (HL) are copied to the high order four bits (7, 6, 5, and 4) of that\nsame memory location, the previous contents of those high order four bits\nare copied to the low order four bits of the Accumulator (register A), and\nthe previous contents of the low order four bits of the Accumulator are\ncopied to the low order four bits of memory location (HL). The contents of\nthe high order bits of the Accumulator are unaffected.",
                ConditionBitsAffected = "S is set if Accumulator is negative after operation, reset otherwise\nZ is set if Accumulator is zero after operation, reset otherwise\nH is reset\nP/V is set if parity of Accumulator is even after operation, reset otherwise\nN is reset\nC is not affected",
                Example = "If the contents of the HL register pair are 5000H, and the contents of the\nAccumulator and memory location 5000H are\n0 1 1 1 1 0 1 0 Accumulator\n0 0 1 1 0 0 0 1 (5000H)\nat execution of RLD the contents of the Accumulator and memory location\n5000H are\n0 1 1 1 0 0 1 1 Accumulator\n0 0 0 1 1 0 1 0 (5000H)"
            };

            Table[111] = new InstructionType()
            {
                Index = 111,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "RR",
                Param1Type = AddressingMode.Register,

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 7,
                    Param1List = new string[] { "A","B","C","D","E","H","L" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 8,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "8 (4, 4)"
                    }
                }
                },

                UserManualPage = "208",
                Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nThe contents of operand m are rotated right 1-bit position through the Carry\nflag. The content of bit 0 is copied to the Carry flag and the previous\ncontent of the Carry flag is copied to bit 7. Bit 0 is the least-significant bit.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise,\nN is reset\nC is data from bit 0 of source register",
                Example = "If the contents of register D and the Carry flag are\n1 1 0 1 1 1 0 1 - 0\nat execution of RR D the contents of register D and the Carry flag are\n0 1 1 0 1 1 1 0 - 1"
            };

            Table[112] = new InstructionType()
            {
                Index = 112,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "RR",
                Param1Type = AddressingMode.RegisterIndirect,

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "(HL)" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 4,
                        TStatesCount = 15,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "15 (4, 4, 4, 3)"
                    }
                }
                },

                UserManualPage = "208",
                Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nThe contents of operand m are rotated right 1-bit position through the Carry\nflag. The content of bit 0 is copied to the Carry flag and the previous\ncontent of the Carry flag is copied to bit 7. Bit 0 is the least-significant bit.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise,\nN is reset\nC is data from bit 0 of source register",
                Example = "If the contents of register D and the Carry flag are\n1 1 0 1 1 1 0 1 - 0\nat execution of RR D the contents of register D and the Carry flag are\n0 1 1 0 1 1 1 0 - 1"
            };

            Table[113] = new InstructionType()
            {
                Index = 113,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "RR",
                Param1Type = AddressingMode.Indexed,

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 2,
                    Param1List = new string[] { "(IX+d)","(IY+d)" },
                    InstructionCodeSizeInBytes = 4,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 6,
                        TStatesCount = 23,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.OC4, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "23 (4, 4, 3, 5, 4, 3)"
                    }
                }
                },

                UserManualPage = "208",
                Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nThe contents of operand m are rotated right 1-bit position through the Carry\nflag. The content of bit 0 is copied to the Carry flag and the previous\ncontent of the Carry flag is copied to bit 7. Bit 0 is the least-significant bit.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise,\nN is reset\nC is data from bit 0 of source register",
                Example = "If the contents of register D and the Carry flag are\n1 1 0 1 1 1 0 1 - 0\nat execution of RR D the contents of register D and the Carry flag are\n0 1 1 0 1 1 1 0 - 1"
            };

            Table[114] = new InstructionType()
            {
                Index = 114,
                IsUndocumented = true,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "RR",
                Param1Type = AddressingMode.Indexed,
                Param2Type = AddressingMode.Register,

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = true,
                    InstructionCodeCount = 14,
                    Param1List = new string[] { "(IX+d)","(IY+d)" },
                    Param2List = new string[] { "A","B","C","D","E","H","L" },
                    InstructionCodeSizeInBytes = 4,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 6,
                        TStatesCount = 23,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.OC4, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "23 (4, 4, 3, 5, 4, 3)"
                    }
                }
                },

                Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nThe contents of operand m are rotated right 1-bit position through the Carry\nflag. The content of bit 0 is copied to the Carry flag and the previous\ncontent of the Carry flag is copied to bit 7. Bit 0 is the least-significant bit.\nThe undocumented DDCB instructions store the result (if any) of the operation in one of the\nseven all-purpose registers.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise,\nN is reset\nC is data from bit 0 of source register",
                Example = "If the contents of register D and the Carry flag are\n1 1 0 1 1 1 0 1 - 0\nat execution of RR D the contents of register D and the Carry flag are\n0 1 1 0 1 1 1 0 - 1"
            };

            Table[115] = new InstructionType()
            {
                Index = 115,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "RRA",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 1,
                        TStatesCount = 4,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "1 (4)"
                    }
                }
                },

                UserManualPage = "193",
                Description = "The contents of the Accumulator (register A) are rotated right 1-bit position\nthrough the Carry flag. The previous content of the Carry flag is copied to\nbit 7. Bit 0 is the least-significant bit.",
                ConditionBitsAffected = "S is not affected\nZ is not affected\nH is reset\nP/V is not affected\nN is reset\nC is data from bit 0 of Accumulator",
                Example = "If the contents of the Accumulator are\n1 1 1 0 0 0 0 1 - 0\nat execution of RRA the contents of the Accumulator and the Carry flag are\n0 1 1 1 0 0 0 0 - 1\n\n"
            };

            Table[116] = new InstructionType()
            {
                Index = 116,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "RRC",
                Param1Type = AddressingMode.Register,

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 7,
                    Param1List = new string[] { "A","B","C","D","E","H","L" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 8,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "8 (4, 4)"
                    }
                }
                },

                UserManualPage = "205",
                Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nThe contents of the m operand are rotated right 1-bit position. The content\nof bit 0 is copied to the Carry flag and also to bit 7. Bit 0 is the leastsignificant\nbit.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise,\nN is reset\nC is data from bit 0 of source register",
                Example = "If the contents of register D and the Carry flag are\n0 0 1 1 0 0 0 1\nat execution of RRC D the contents of register D and the Carry flag are\n1 0 0 1 1 0 0 0 - 1"
            };

            Table[117] = new InstructionType()
            {
                Index = 117,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "RRC",
                Param1Type = AddressingMode.RegisterIndirect,

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "(HL)" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 4,
                        TStatesCount = 15,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "15 (4, 4, 4, 3)"
                    }
                }
                },

                UserManualPage = "205",
                Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nThe contents of the m operand are rotated right 1-bit position. The content\nof bit 0 is copied to the Carry flag and also to bit 7. Bit 0 is the leastsignificant\nbit.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise,\nN is reset\nC is data from bit 0 of source register",
                Example = "If the contents of register D and the Carry flag are\n0 0 1 1 0 0 0 1\nat execution of RRC D the contents of register D and the Carry flag are\n1 0 0 1 1 0 0 0 - 1"
            };

            Table[118] = new InstructionType()
            {
                Index = 118,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "RRC",
                Param1Type = AddressingMode.Indexed,

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 2,
                    Param1List = new string[] { "(IX+d)","(IY+d)" },
                    InstructionCodeSizeInBytes = 4,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 6,
                        TStatesCount = 23,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.OC4, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "23 (4, 4, 3, 5, 4, 3)"
                    }
                }
                },

                UserManualPage = "205",
                Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nThe contents of the m operand are rotated right 1-bit position. The content\nof bit 0 is copied to the Carry flag and also to bit 7. Bit 0 is the leastsignificant\nbit.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise,\nN is reset\nC is data from bit 0 of source register",
                Example = "If the contents of register D and the Carry flag are\n0 0 1 1 0 0 0 1\nat execution of RRC D the contents of register D and the Carry flag are\n1 0 0 1 1 0 0 0 - 1"
            };

            Table[119] = new InstructionType()
            {
                Index = 119,
                IsUndocumented = true,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "RRC",
                Param1Type = AddressingMode.Indexed,
                Param2Type = AddressingMode.Register,

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = true,
                    InstructionCodeCount = 14,
                    Param1List = new string[] { "(IX+d)","(IY+d)" },
                    Param2List = new string[] { "A","B","C","D","E","H","L" },
                    InstructionCodeSizeInBytes = 4,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 6,
                        TStatesCount = 23,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.OC4, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "23 (4, 4, 3, 5, 4, 3)"
                    }
                }
                },

                Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nThe contents of the m operand are rotated right 1-bit position. The content\nof bit 0 is copied to the Carry flag and also to bit 7. Bit 0 is the leastsignificant\nbit.\nThe undocumented DDCB instructions store the result (if any) of the operation in one of the\nseven all-purpose registers.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise,\nN is reset\nC is data from bit 0 of source register",
                Example = "If the contents of register D and the Carry flag are\n0 0 1 1 0 0 0 1\nat execution of RRC D the contents of register D and the Carry flag are\n1 0 0 1 1 0 0 0 - 1"
            };

            Table[120] = new InstructionType()
            {
                Index = 120,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "RRCA",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 1,
                        TStatesCount = 4,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "1 (4)"
                    }
                }
                },

                UserManualPage = "192",
                Description = "The contents of the Accumulator (register A) are rotated right 1-bit\nposition. Bit 0 is copied to the Carry flag and also to bit 7. Bit 0 is the leastsignificant\nbit.",
                ConditionBitsAffected = "S is not affected\nZ is not affected\nH is reset\nP/V is not affected\nN is reset\nC is data from bit 0 of Accumulator",
                Example = "If the contents of the Accumulator are\n0 0 1 0 0 0 1 - 0\nat execution of RRCA the contents of the Accumulator and the Carry flag are\n1 0 0 1 0 0 0 - 1\n\n"
            };

            Table[121] = new InstructionType()
            {
                Index = 121,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "RRD",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 5,
                        TStatesCount = 18,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "18 (4, 4, 3, 4, 3)"
                    }
                }
                },

                UserManualPage = "222",
                Description = "The contents of the low order four bits (bits 3, 2, 1, and 0) of memory\nlocation (HL) are copied to the low order four bits of the Accumulator\n(register A). The previous contents of the low order four bits of the\nAccumulator are copied to the high order four bits (7, 6, 5, and 4) of\nlocation (HL), and the previous contents of the high order four bits of (HL)\nare copied to the low order four bits of (HL). The contents of the high order\nbits of the Accumulator are unaffected.",
                ConditionBitsAffected = "S is set if Accumulator is negative after operation, reset otherwise\nZ is set if Accumulator is zero after operation, reset otherwise\nH is reset\nP/V is set if parity of Accumulator is even after operation, reset otherwise\nN is reset\nC is not affected",
                Example = "If the contents of the HL register pair are 5000H, and the contents of the\nAccumulator and memory location 5000H are\n1 0 0 0 0 1 0 0 Accumulator\n0 0 1 0 0 0 0 0 (5000H)\nat execution of RRD the contents of the Accumulator and memory location\n5000H are\n1 0 0 0 0 0 0 0 Accumulator\n0 1 0 0 0 0 1 0 (5000H)"
            };

            Table[122] = new InstructionType()
            {
                Index = 122,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Call And Return",
                OpCodeName = "RST",
                Param1Type = AddressingMode.ModifiedPageZero,
                Equation = "(SP-1) <- PCH, (SP-2) <- PCL, PCH <- 0, PCL <- P",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 8,
                    Param1List = new string[] { "0H","8H","10H","18H","20H","28H","30H","38H" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 3,
                        TStatesCount = 11,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.SWH, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.SWL, TStates = 3 } },
                        TStatesByMachineCycle = "11 (5, 3, 3)",
                        StackOperations = "M1 -1, M2 -1"
                    }
                }
                },

                UserManualPage = "267",
                Description = "The current Program Counter (PC) contents are pushed onto the external\nmemory stack, and the page zero memory location given by operand p is\nloaded to the PC. Program execution then begins with the Op Code in the\naddress now pointed to by PC. The push is performed by first decrementing\nthe contents of the Stack Pointer (SP), loading the high-order byte of PC to\nthe memory address now pointed to by SP, decrementing SP again, and\nloading the low order byte of PC to the address now pointed to by SP. The\nRestart instruction allows for a jump to one of eight addresses indicated in\nthe table below. The operand p is assembled to the object code using the\ncorresponding T state.\nBecause all addresses are in page zero of memory, the high order byte of\nPC is loaded with 00H. The number selected from the p column of the table\nis loaded to the low order byte of PC.\np\n00H\n08H\n10H\n18H\n20H\n28H\n30H\n38H",
                ConditionBitsAffected = "None",
                Example = "If the contents of the Program Counter are 15B3H, at execution of\nRST 18H (Object code 1101111) the PC contains 0018H, as the address\nof the next Op Code fetched."
            };

            Table[123] = new InstructionType()
            {
                Index = 123,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "SBC",
                Param1Type = AddressingMode.Register,
                Param2Type = AddressingMode.Immediate,
                Equation = "A <- A - s - CY",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "A" },
                    Param2List = new string[] { "n" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 7,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 } },
                        TStatesByMachineCycle = "7 (4, 3)"
                    }
                }
                },

                UserManualPage = "150",
                Description = "The s operand, along with the Carry flag (C in the F register) is subtracted\nfrom the contents of the Accumulator, and the result is stored in the\nAccumulator.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is reset if overflow, reset otherwise\nN is set\nC is set if borrow, reset otherwise",
                Example = "If the Accumulator contains 16H, the carry flag is set, the HL register pair\ncontains 3433H, and address 3433H contains 05H, at execution of\nSBC A, (HL) the Accumulator contains 10H."
            };

            Table[124] = new InstructionType()
            {
                Index = 124,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "SBC",
                Param1Type = AddressingMode.Register,
                Param2Type = AddressingMode.Register,
                Equation = "A <- A - s - CY",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 7,
                    Param1List = new string[] { "A" },
                    Param2List = new string[] { "A","B","C","D","E","H","L" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 1,
                        TStatesCount = 4,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "1 (4)"
                    }
                },
                new InstructionParametersVariant() {
                    Index = 1,
                    IsUndocumented = true,
                    InstructionCodeCount = 4,
                    Param1List = new string[] { "A" },
                    Param2List = new string[] { "IXh","IXl","IYh","IYl" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 8,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "8 (4, 4)"
                    }
                }
                },

                UserManualPage = "150",
                Description = "The s operand, along with the Carry flag (C in the F register) is subtracted\nfrom the contents of the Accumulator, and the result is stored in the\nAccumulator.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is reset if overflow, reset otherwise\nN is set\nC is set if borrow, reset otherwise",
                Example = "If the Accumulator contains 16H, the carry flag is set, the HL register pair\ncontains 3433H, and address 3433H contains 05H, at execution of\nSBC A, (HL) the Accumulator contains 10H."
            };

            Table[125] = new InstructionType()
            {
                Index = 125,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "SBC",
                Param1Type = AddressingMode.Register,
                Param2Type = AddressingMode.RegisterIndirect,
                Equation = "A <- A - s - CY",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "A" },
                    Param2List = new string[] { "(HL)" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 7,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 } },
                        TStatesByMachineCycle = "7 (4, 3)"
                    }
                }
                },

                UserManualPage = "150",
                Description = "The s operand, along with the Carry flag (C in the F register) is subtracted\nfrom the contents of the Accumulator, and the result is stored in the\nAccumulator.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is reset if overflow, reset otherwise\nN is set\nC is set if borrow, reset otherwise",
                Example = "If the Accumulator contains 16H, the carry flag is set, the HL register pair\ncontains 3433H, and address 3433H contains 05H, at execution of\nSBC A, (HL) the Accumulator contains 10H."
            };

            Table[126] = new InstructionType()
            {
                Index = 126,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "SBC",
                Param1Type = AddressingMode.Register,
                Param2Type = AddressingMode.Indexed,
                Equation = "A <- A - s - CY",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 2,
                    Param1List = new string[] { "A" },
                    Param2List = new string[] { "(IX+d)","(IY+d)" },
                    InstructionCodeSizeInBytes = 3,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 5,
                        TStatesCount = 19,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 } },
                        TStatesByMachineCycle = "19 (4, 4, 3, 5, 3)"
                    }
                }
                },

                UserManualPage = "150",
                Description = "The s operand, along with the Carry flag (C in the F register) is subtracted\nfrom the contents of the Accumulator, and the result is stored in the\nAccumulator.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is reset if overflow, reset otherwise\nN is set\nC is set if borrow, reset otherwise",
                Example = "If the Accumulator contains 16H, the carry flag is set, the HL register pair\ncontains 3433H, and address 3433H contains 05H, at execution of\nSBC A, (HL) the Accumulator contains 10H."
            };

            Table[127] = new InstructionType()
            {
                Index = 127,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "16-Bit Arithmetic",
                OpCodeName = "SBC",
                Param1Type = AddressingMode.Register16,
                Param2Type = AddressingMode.Register16,
                Equation = "HL <- HL - ss - CY",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 4,
                    Param1List = new string[] { "HL" },
                    Param2List = new string[] { "BC","DE","HL","SP" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 4,
                        TStatesCount = 15,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 3 } },
                        TStatesByMachineCycle = "15 (4, 4, 4, 3)"
                    }
                }
                },

                UserManualPage = "181",
                Description = "The contents of the register pair ss (any of register pairs BC, DE, HL, or\nSP) and the Carry Flag (C flag in the F register) are subtracted from the\ncontents of register pair HL, and the result is stored in HL.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if a borrow from bit 12, reset otherwise\nP/V is set if overflow, reset otherwise\nN is set\nC is set if borrow, reset otherwise",
                Example = "If the contents of the HL, register pair are 9999H, the contents of register\npair DE are 1111H, and the Carry flag is set. At execution of SBC HL, DE\nthe contents of HL are 8887H."
            };

            Table[128] = new InstructionType()
            {
                Index = 128,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "General-Purpose Arithmetic",
                OpCodeName = "SCF",
                Equation = "CY <- 1",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 1,
                        TStatesCount = 4,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "1 (4)"
                    }
                }
                },

                UserManualPage = "171",
                Description = "The Carry flag in the F register is set.",
                ConditionBitsAffected = "S is not affected\nZ is not affected\nH is reset\nP/V is not affected\nN is reset\nC is set"
            };

            Table[129] = new InstructionType()
            {
                Index = 129,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Bit Set, Reset, and Test",
                OpCodeName = "SET",
                Param1Type = AddressingMode.Bit,
                Param2Type = AddressingMode.Register,
                Equation = "rb <- 1",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 56,
                    Param1List = new string[] { "0","1","2","3","4","5","6","7" },
                    Param2List = new string[] { "A","B","C","D","E","H","L" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 8,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "8 (4, 4)"
                    }
                }
                },

                UserManualPage = "232",
                Description = "Bit b in register r (any of registers B, C, D, E, H, L, or A) is set.",
                ConditionBitsAffected = "None",
                Example = "At execution of SET 4, A bit 4 in register A sets. Bit 0 is the leastsignificant\nbit."
            };

            Table[130] = new InstructionType()
            {
                Index = 130,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Bit Set, Reset, and Test",
                OpCodeName = "SET",
                Param1Type = AddressingMode.Bit,
                Param2Type = AddressingMode.RegisterIndirect,
                Equation = "(HL)b <- 1",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 8,
                    Param1List = new string[] { "0","1","2","3","4","5","6","7" },
                    Param2List = new string[] { "(HL)" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 4,
                        TStatesCount = 15,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "15 (4, 4, 4, 3)"
                    }
                }
                },

                UserManualPage = "233",
                Description = "Bit b in the memory location addressed by the contents of register pair HL\nis set.",
                ConditionBitsAffected = "None",
                Example = "If the contents of the HL register pair are 3000H, at execution of\nSET 4, (HL) bit 4 in memory location 3000H is 1. Bit 0 in memory\nlocation 3000H is the least-significant bit."
            };

            Table[131] = new InstructionType()
            {
                Index = 131,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Bit Set, Reset, and Test",
                OpCodeName = "SET",
                Param1Type = AddressingMode.Bit,
                Param2Type = AddressingMode.Indexed,
                Equation = "(IX+d)b <- 1",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 16,
                    Param1List = new string[] { "0","1","2","3","4","5","6","7" },
                    Param2List = new string[] { "(IX+d)","(IY+d)" },
                    InstructionCodeSizeInBytes = 4,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 6,
                        TStatesCount = 23,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.OC4, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "23 (4, 4, 3, 5, 4, 3)"
                    }
                }
                },

                UserManualPage = "234\n235",
                Description = "Bit b in the memory location addressed by the sum of the contents of the IX\nregister pair and the two�s complement integer d is set.",
                ConditionBitsAffected = "None",
                Example = "If the contents of Index Register are 2000H, at execution of\nSET 0, (IX + 3H) bit 0 in memory location 2003H is 1.\nBit 0 in memory location 2003H is the least-significant bit."
            };

            Table[132] = new InstructionType()
            {
                Index = 132,
                IsUndocumented = true,
                IsInternalZ80Operation = false,

                InstructionGroup = "Bit Set, Reset, and Test",
                OpCodeName = "SET",
                Param1Type = AddressingMode.Bit,
                Param2Type = AddressingMode.Indexed,
                Param3Type = AddressingMode.Register,
                Equation = "(IX+d)b <- 1",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = true,
                    InstructionCodeCount = 112,
                    Param1List = new string[] { "0","1","2","3","4","5","6","7" },
                    Param2List = new string[] { "(IX+d)","(IY+d)" },
                    Param3List = new string[] { "A","B","C","D","E","H","L" },
                    InstructionCodeSizeInBytes = 4,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 6,
                        TStatesCount = 23,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.OC4, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "23 (4, 4, 3, 5, 4, 3)"
                    }
                }
                },

                Description = "Bit b in the memory location addressed by the sum of the contents of the IX\nregister pair and the two�s complement integer d is set.\nThe undocumented DDCB instructions store the result (if any) of the operation in one of the\nseven all-purpose registers.",
                ConditionBitsAffected = "None",
                Example = "If the contents of Index Register are 2000H, at execution of\nSET 0, (IX + 3H) bit 0 in memory location 2003H is 1.\nBit 0 in memory location 2003H is the least-significant bit."
            };

            Table[133] = new InstructionType()
            {
                Index = 133,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "SLA",
                Param1Type = AddressingMode.Register,

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 7,
                    Param1List = new string[] { "A","B","C","D","E","H","L" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 8,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "8 (4, 4)"
                    }
                }
                },

                UserManualPage = "211",
                Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nAn arithmetic shift left 1-bit position is performed on the contents of\noperand m. The content of bit 7 is copied to the Carry flag. Bit 0 is the\nleast-significant bit.",
                ConditionBitsAffected = "Z is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is data from bit 7",
                Example = "If the contents of register L are\n1 0 1 1 0 0 0 1\nat execution of SLA L the contents of register L and the Carry flag are\n1 - 0 1 1 0 0 0 1 0"
            };

            Table[134] = new InstructionType()
            {
                Index = 134,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "SLA",
                Param1Type = AddressingMode.RegisterIndirect,

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "(HL)" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 4,
                        TStatesCount = 15,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "15 (4, 4, 4, 3)"
                    }
                }
                },

                UserManualPage = "211",
                Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nAn arithmetic shift left 1-bit position is performed on the contents of\noperand m. The content of bit 7 is copied to the Carry flag. Bit 0 is the\nleast-significant bit.",
                ConditionBitsAffected = "Z is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is data from bit 7",
                Example = "If the contents of register L are\n1 0 1 1 0 0 0 1\nat execution of SLA L the contents of register L and the Carry flag are\n1 - 0 1 1 0 0 0 1 0"
            };

            Table[135] = new InstructionType()
            {
                Index = 135,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "SLA",
                Param1Type = AddressingMode.Indexed,

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 2,
                    Param1List = new string[] { "(IX+d)","(IY+d)" },
                    InstructionCodeSizeInBytes = 4,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 6,
                        TStatesCount = 23,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.OC4, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "23 (4, 4, 3, 5, 4, 3)"
                    }
                }
                },

                UserManualPage = "211",
                Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nAn arithmetic shift left 1-bit position is performed on the contents of\noperand m. The content of bit 7 is copied to the Carry flag. Bit 0 is the\nleast-significant bit.",
                ConditionBitsAffected = "Z is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is data from bit 7",
                Example = "If the contents of register L are\n1 0 1 1 0 0 0 1\nat execution of SLA L the contents of register L and the Carry flag are\n1 - 0 1 1 0 0 0 1 0"
            };

            Table[136] = new InstructionType()
            {
                Index = 136,
                IsUndocumented = true,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "SLA",
                Param1Type = AddressingMode.Indexed,
                Param2Type = AddressingMode.Register,

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = true,
                    InstructionCodeCount = 14,
                    Param1List = new string[] { "(IX+d)","(IY+d)" },
                    Param2List = new string[] { "A","B","C","D","E","H","L" },
                    InstructionCodeSizeInBytes = 4,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 6,
                        TStatesCount = 23,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.OC4, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "23 (4, 4, 3, 5, 4, 3)"
                    }
                }
                },

                Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nAn arithmetic shift left 1-bit position is performed on the contents of\noperand m. The content of bit 7 is copied to the Carry flag. Bit 0 is the\nleast-significant bit.\nThe undocumented DDCB instructions store the result (if any) of the operation in one of the\nseven all-purpose registers.",
                ConditionBitsAffected = "Z is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is data from bit 7",
                Example = "If the contents of register L are\n1 0 1 1 0 0 0 1\nat execution of SLA L the contents of register L and the Carry flag are\n1 - 0 1 1 0 0 0 1 0"
            };

            Table[137] = new InstructionType()
            {
                Index = 137,
                IsUndocumented = true,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "SLL",
                Param1Type = AddressingMode.Register,

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = true,
                    InstructionCodeCount = 7,
                    Param1List = new string[] { "A","B","C","D","E","H","L" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 8,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "8 (4, 4)"
                    }
                }
                },

                Description = "There are a few\ninstructions missing from the o�cial list, which are usually denoted with SLL (Shift Logical Left).\nIt works like SLA, for one exception: it sets bit 0 (SLA resets it).",
                ConditionBitsAffected = "Z is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is data from bit 7",
                Example = "If the contents of register L are\n1 0 1 1 0 0 0 1\nat execution of SLL L the contents of register L and the Carry flag are\n1 - 0 1 1 0 0 0 1 1"
            };

            Table[138] = new InstructionType()
            {
                Index = 138,
                IsUndocumented = true,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "SLL",
                Param1Type = AddressingMode.RegisterIndirect,

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = true,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "(HL)" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 4,
                        TStatesCount = 15,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "15 (4, 4, 4, 3)"
                    }
                }
                },

                Description = "There are a few\ninstructions missing from the o�cial list, which are usually denoted with SLL (Shift Logical Left).\nIt works like SLA, for one exception: it sets bit 0 (SLA resets it).",
                ConditionBitsAffected = "Z is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is data from bit 7",
                Example = "If the contents of register L are\n1 0 1 1 0 0 0 1\nat execution of SLL L the contents of register L and the Carry flag are\n1 - 0 1 1 0 0 0 1 1"
            };

            Table[139] = new InstructionType()
            {
                Index = 139,
                IsUndocumented = true,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "SLL",
                Param1Type = AddressingMode.Indexed,

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = true,
                    InstructionCodeCount = 2,
                    Param1List = new string[] { "(IX+d)","(IY+d)" },
                    InstructionCodeSizeInBytes = 4,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 6,
                        TStatesCount = 23,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.OC4, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "23 (4, 4, 3, 5, 4, 3)"
                    }
                }
                },

                Description = "There are a few\ninstructions missing from the o�cial list, which are usually denoted with SLL (Shift Logical Left).\nIt works like SLA, for one exception: it sets bit 0 (SLA resets it).",
                ConditionBitsAffected = "Z is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is data from bit 7",
                Example = "If the contents of register L are\n1 0 1 1 0 0 0 1\nat execution of SLL L the contents of register L and the Carry flag are\n1 - 0 1 1 0 0 0 1 1"
            };

            Table[140] = new InstructionType()
            {
                Index = 140,
                IsUndocumented = true,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "SLL",
                Param1Type = AddressingMode.Indexed,
                Param2Type = AddressingMode.Register,

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = true,
                    InstructionCodeCount = 14,
                    Param1List = new string[] { "(IX+d)","(IY+d)" },
                    Param2List = new string[] { "A","B","C","D","E","H","L" },
                    InstructionCodeSizeInBytes = 4,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 6,
                        TStatesCount = 23,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.OC4, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "23 (4, 4, 3, 5, 4, 3)"
                    }
                }
                },

                Description = "There are a few\ninstructions missing from the o�cial list, which are usually denoted with SLL (Shift Logical Left).\nIt works like SLA, for one exception: it sets bit 0 (SLA resets it).\nThe undocumented DDCB instructions store the result (if any) of the operation in one of the\nseven all-purpose registers.",
                ConditionBitsAffected = "Z is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is data from bit 7",
                Example = "If the contents of register L are\n1 0 1 1 0 0 0 1\nat execution of SLL L the contents of register L and the Carry flag are\n1 - 0 1 1 0 0 0 1 1"
            };

            Table[141] = new InstructionType()
            {
                Index = 141,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "SRA",
                Param1Type = AddressingMode.Register,

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 7,
                    Param1List = new string[] { "A","B","C","D","E","H","L" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 8,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "8 (4, 4)"
                    }
                }
                },

                UserManualPage = "214",
                Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous PLC instructions.\nAn arithmetic shift right 1-bit position is performed on the contents of\noperand m. The content of bit 0 is copied to the Carry flag and the previous\ncontent of bit 7 is unchanged. Bit 0 is the least-significant bit.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is data from bit 0 of source register",
                Example = "If the contents of the Index Register IX are 1000H, and the contents of\nmemory location 1003H are\n1 0 1 1 1 0 0 0\nat execution of SRA (IX+3H) the contents of memory location 1003H\nand the Carry flag are\n1 1 0 1 1 1 0 0 - 0"
            };

            Table[142] = new InstructionType()
            {
                Index = 142,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "SRA",
                Param1Type = AddressingMode.RegisterIndirect,

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "(HL)" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 4,
                        TStatesCount = 15,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "15 (4, 4, 4, 3)"
                    }
                }
                },

                UserManualPage = "214",
                Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous PLC instructions.\nAn arithmetic shift right 1-bit position is performed on the contents of\noperand m. The content of bit 0 is copied to the Carry flag and the previous\ncontent of bit 7 is unchanged. Bit 0 is the least-significant bit.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is data from bit 0 of source register",
                Example = "If the contents of the Index Register IX are 1000H, and the contents of\nmemory location 1003H are\n1 0 1 1 1 0 0 0\nat execution of SRA (IX+3H) the contents of memory location 1003H\nand the Carry flag are\n1 1 0 1 1 1 0 0 - 0"
            };

            Table[143] = new InstructionType()
            {
                Index = 143,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "SRA",
                Param1Type = AddressingMode.Indexed,

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 2,
                    Param1List = new string[] { "(IX+d)","(IY+d)" },
                    InstructionCodeSizeInBytes = 4,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 6,
                        TStatesCount = 23,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.OC4, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "23 (4, 4, 3, 5, 4, 3)"
                    }
                }
                },

                UserManualPage = "214",
                Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous PLC instructions.\nAn arithmetic shift right 1-bit position is performed on the contents of\noperand m. The content of bit 0 is copied to the Carry flag and the previous\ncontent of bit 7 is unchanged. Bit 0 is the least-significant bit.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is data from bit 0 of source register",
                Example = "If the contents of the Index Register IX are 1000H, and the contents of\nmemory location 1003H are\n1 0 1 1 1 0 0 0\nat execution of SRA (IX+3H) the contents of memory location 1003H\nand the Carry flag are\n1 1 0 1 1 1 0 0 - 0"
            };

            Table[144] = new InstructionType()
            {
                Index = 144,
                IsUndocumented = true,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "SRA",
                Param1Type = AddressingMode.Indexed,
                Param2Type = AddressingMode.Register,

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = true,
                    InstructionCodeCount = 14,
                    Param1List = new string[] { "(IX+d)","(IY+d)" },
                    Param2List = new string[] { "A","B","C","D","E","H","L" },
                    InstructionCodeSizeInBytes = 4,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 6,
                        TStatesCount = 23,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.OC4, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "23 (4, 4, 3, 5, 4, 3)"
                    }
                }
                },

                Description = "The m operand is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous PLC instructions.\nAn arithmetic shift right 1-bit position is performed on the contents of\noperand m. The content of bit 0 is copied to the Carry flag and the previous\ncontent of bit 7 is unchanged. Bit 0 is the least-significant bit.\nThe undocumented DDCB instructions store the result (if any) of the operation in one of the\nseven all-purpose registers.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is data from bit 0 of source register",
                Example = "If the contents of the Index Register IX are 1000H, and the contents of\nmemory location 1003H are\n1 0 1 1 1 0 0 0\nat execution of SRA (IX+3H) the contents of memory location 1003H\nand the Carry flag are\n1 1 0 1 1 1 0 0 - 0"
            };

            Table[145] = new InstructionType()
            {
                Index = 145,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "SRL",
                Param1Type = AddressingMode.Register,

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 7,
                    Param1List = new string[] { "A","B","C","D","E","H","L" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 8,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "8 (4, 4)"
                    }
                }
                },

                UserManualPage = "217",
                Description = "The operand m is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nThe contents of operand m are shifted right 1-bit position. The content of\nbit 0 is copied to the Carry flag, and bit 7 is reset. Bit 0 is the leastsignificant\nbit.",
                ConditionBitsAffected = "S is reset\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is data from bit 0 of source register",
                Example = "If the contents of register B are\n1 0 0 0 1 1 1 1\nat execution of SRL B the contents of register B and the Carry flag are\n0 1 0 0 0 1 1 1 - 1"
            };

            Table[146] = new InstructionType()
            {
                Index = 146,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "SRL",
                Param1Type = AddressingMode.RegisterIndirect,

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "(HL)" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 4,
                        TStatesCount = 15,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "15 (4, 4, 4, 3)"
                    }
                }
                },

                UserManualPage = "217",
                Description = "The operand m is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nThe contents of operand m are shifted right 1-bit position. The content of\nbit 0 is copied to the Carry flag, and bit 7 is reset. Bit 0 is the leastsignificant\nbit.",
                ConditionBitsAffected = "S is reset\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is data from bit 0 of source register",
                Example = "If the contents of register B are\n1 0 0 0 1 1 1 1\nat execution of SRL B the contents of register B and the Carry flag are\n0 1 0 0 0 1 1 1 - 1"
            };

            Table[147] = new InstructionType()
            {
                Index = 147,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "SRL",
                Param1Type = AddressingMode.Indexed,

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 2,
                    Param1List = new string[] { "(IX+d)","(IY+d)" },
                    InstructionCodeSizeInBytes = 4,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 6,
                        TStatesCount = 23,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.OC4, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "23 (4, 4, 3, 5, 4, 3)"
                    }
                }
                },

                UserManualPage = "217",
                Description = "The operand m is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nThe contents of operand m are shifted right 1-bit position. The content of\nbit 0 is copied to the Carry flag, and bit 7 is reset. Bit 0 is the leastsignificant\nbit.",
                ConditionBitsAffected = "S is reset\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is data from bit 0 of source register",
                Example = "If the contents of register B are\n1 0 0 0 1 1 1 1\nat execution of SRL B the contents of register B and the Carry flag are\n0 1 0 0 0 1 1 1 - 1"
            };

            Table[148] = new InstructionType()
            {
                Index = 148,
                IsUndocumented = true,
                IsInternalZ80Operation = false,

                InstructionGroup = "Rotate and Shift",
                OpCodeName = "SRL",
                Param1Type = AddressingMode.Indexed,
                Param2Type = AddressingMode.Register,

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = true,
                    InstructionCodeCount = 14,
                    Param1List = new string[] { "(IX+d)","(IY+d)" },
                    Param2List = new string[] { "A","B","C","D","E","H","L" },
                    InstructionCodeSizeInBytes = 4,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 6,
                        TStatesCount = 23,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.OC4, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MW, TStates = 3 } },
                        TStatesByMachineCycle = "23 (4, 4, 3, 5, 4, 3)"
                    }
                }
                },

                Description = "The operand m is any of r, (HL), (IX+d), or (lY+d), as defined for the\nanalogous RLC instructions.\nThe contents of operand m are shifted right 1-bit position. The content of\nbit 0 is copied to the Carry flag, and bit 7 is reset. Bit 0 is the leastsignificant\nbit.\nThe undocumented DDCB instructions store the result (if any) of the operation in one of the\nseven all-purpose registers.",
                ConditionBitsAffected = "S is reset\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity is even, reset otherwise\nN is reset\nC is data from bit 0 of source register",
                Example = "If the contents of register B are\n1 0 0 0 1 1 1 1\nat execution of SRL B the contents of register B and the Carry flag are\n0 1 0 0 0 1 1 1 - 1"
            };

            Table[149] = new InstructionType()
            {
                Index = 149,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "SUB",
                Param1Type = AddressingMode.Immediate,
                Equation = "A <- A - s",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "n" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 7,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 } },
                        TStatesByMachineCycle = "7 (4, 3)"
                    }
                }
                },

                UserManualPage = "148",
                Description = "The s operand is subtracted from the contents of the Accumulator, and the\nresult is stored in the Accumulator.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is set if overflow, reset otherwise\nN is set\nC is set if borrow, reset otherwise",
                Example = "If the Accumulator contents are 29H, and register D contains 11H, at\nexecution of SUB D the Accumulator contains 18H."
            };

            Table[150] = new InstructionType()
            {
                Index = 150,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "SUB",
                Param1Type = AddressingMode.Register,
                Equation = "A <- A - s",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 7,
                    Param1List = new string[] { "A","B","C","D","E","H","L" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 1,
                        TStatesCount = 4,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "1 (4)"
                    }
                },
                new InstructionParametersVariant() {
                    Index = 1,
                    IsUndocumented = true,
                    InstructionCodeCount = 4,
                    Param1List = new string[] { "IXh","IXl","IYh","IYl" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 8,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "8 (4, 4)"
                    }
                }
                },

                UserManualPage = "148",
                Description = "The s operand is subtracted from the contents of the Accumulator, and the\nresult is stored in the Accumulator.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is set if overflow, reset otherwise\nN is set\nC is set if borrow, reset otherwise",
                Example = "If the Accumulator contents are 29H, and register D contains 11H, at\nexecution of SUB D the Accumulator contains 18H."
            };

            Table[151] = new InstructionType()
            {
                Index = 151,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "SUB",
                Param1Type = AddressingMode.RegisterIndirect,
                Equation = "A <- A - s",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "(HL)" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 7,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 } },
                        TStatesByMachineCycle = "7 (4, 3)"
                    }
                }
                },

                UserManualPage = "148",
                Description = "The s operand is subtracted from the contents of the Accumulator, and the\nresult is stored in the Accumulator.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is set if overflow, reset otherwise\nN is set\nC is set if borrow, reset otherwise",
                Example = "If the Accumulator contents are 29H, and register D contains 11H, at\nexecution of SUB D the Accumulator contains 18H."
            };

            Table[152] = new InstructionType()
            {
                Index = 152,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "SUB",
                Param1Type = AddressingMode.Indexed,
                Equation = "A <- A - s",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 2,
                    Param1List = new string[] { "(IX+d)","(IY+d)" },
                    InstructionCodeSizeInBytes = 3,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 5,
                        TStatesCount = 19,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 } },
                        TStatesByMachineCycle = "19 (4, 4, 3, 5, 3)"
                    }
                }
                },

                UserManualPage = "148",
                Description = "The s operand is subtracted from the contents of the Accumulator, and the\nresult is stored in the Accumulator.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is set if borrow from bit 4, reset otherwise\nP/V is set if overflow, reset otherwise\nN is set\nC is set if borrow, reset otherwise",
                Example = "If the Accumulator contents are 29H, and register D contains 11H, at\nexecution of SUB D the Accumulator contains 18H."
            };

            Table[153] = new InstructionType()
            {
                Index = 153,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "XOR",
                Param1Type = AddressingMode.Immediate,
                Equation = "A <- A ^ s",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "n" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 7,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 } },
                        TStatesByMachineCycle = "7 (4, 3)"
                    }
                }
                },

                UserManualPage = "156",
                Description = "The logical exclusive-OR operation is performed between the byte\nspecified by the s operand and the byte contained in the Accumulator, the\nresult is stored in the Accumulator.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise\nN is reset\nC is reset"
            };

            Table[154] = new InstructionType()
            {
                Index = 154,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "XOR",
                Param1Type = AddressingMode.Register,
                Equation = "A <- A ^ s",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 7,
                    Param1List = new string[] { "A","B","C","D","E","H","L" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 1,
                        TStatesCount = 4,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "1 (4)"
                    }
                },
                new InstructionParametersVariant() {
                    Index = 1,
                    IsUndocumented = true,
                    InstructionCodeCount = 4,
                    Param1List = new string[] { "IXh","IXl","IYh","IYl" },
                    InstructionCodeSizeInBytes = 2,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 8,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 } },
                        TStatesByMachineCycle = "8 (4, 4)"
                    }
                }
                },

                UserManualPage = "156",
                Description = "The logical exclusive-OR operation is performed between the byte\nspecified by the s operand and the byte contained in the Accumulator, the\nresult is stored in the Accumulator.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise\nN is reset\nC is reset"
            };

            Table[155] = new InstructionType()
            {
                Index = 155,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "XOR",
                Param1Type = AddressingMode.RegisterIndirect,
                Equation = "A <- A ^ s",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 1,
                    Param1List = new string[] { "(HL)" },
                    InstructionCodeSizeInBytes = 1,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 2,
                        TStatesCount = 7,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 } },
                        TStatesByMachineCycle = "7 (4, 3)"
                    }
                }
                },

                UserManualPage = "156",
                Description = "The logical exclusive-OR operation is performed between the byte\nspecified by the s operand and the byte contained in the Accumulator, the\nresult is stored in the Accumulator.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise\nN is reset\nC is reset"
            };

            Table[156] = new InstructionType()
            {
                Index = 156,
                IsUndocumented = false,
                IsInternalZ80Operation = false,

                InstructionGroup = "8-Bit Arithmetic",
                OpCodeName = "XOR",
                Param1Type = AddressingMode.Indexed,
                Equation = "A <- A ^ s",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 2,
                    Param1List = new string[] { "(IX+d)","(IY+d)" },
                    InstructionCodeSizeInBytes = 3,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 5,
                        TStatesCount = 19,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.MR, TStates = 3 } },
                        TStatesByMachineCycle = "19 (4, 4, 3, 5, 3)"
                    }
                }
                },

                UserManualPage = "156",
                Description = "The logical exclusive-OR operation is performed between the byte\nspecified by the s operand and the byte contained in the Accumulator, the\nresult is stored in the Accumulator.",
                ConditionBitsAffected = "S is set if result is negative, reset otherwise\nZ is set if result is zero, reset otherwise\nH is reset\nP/V is set if parity even, reset otherwise\nN is reset\nC is reset"
            };

            Table[157] = new InstructionType()
            {
                Index = 157,
                IsUndocumented = false,
                IsInternalZ80Operation = true,

                InstructionGroup = "",
                OpCodeName = "NMI",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 0,
                    InstructionCodeSizeInBytes = 0,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 3,
                        TStatesCount = 11,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 5 },
                            new MachineCycle() { Type = MachineCycleType.SWH, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.SWL, TStates = 3 } },
                        TStatesByMachineCycle = "11 (5,3,3)",
                        StackOperations = "M1 -1, M2 -1",
                        M1Comment = "Op Code Ignored"
                    }
                }
                },

                Description = "It takes 11 clock cycles to get to #0066: \nM1 cycle: 5 T states to do an opcode read and decrement SP \nM2 cycle: 3 T states write high byte of PC to the stack and decrement SP \nM3 cycle: 3 T states write the low byte of PC and jump to #0066. ",
                Example = "If the Accumulator contains 96H (1001 0110), at execution of XOR 5DH\n(5DH = 0101 1101) the Accumulator contains CBH (1100 1011)."
            };

            Table[158] = new InstructionType()
            {
                Index = 158,
                IsUndocumented = false,
                IsInternalZ80Operation = true,

                InstructionGroup = "",
                OpCodeName = "INT 0",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 0,
                    InstructionCodeSizeInBytes = 0,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        Condition = "if call",
                        MachineCyclesCount = 5,
                        TStatesCount = 19,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.INTA, TStates = 7 },
                            new MachineCycle() { Type = MachineCycleType.ODL, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.ODH, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.SWH, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.SWL, TStates = 3 } },
                        TStatesByMachineCycle = "19 (7,3,3,3,3)",
                        StackOperations = "M3 -1, M4 -1",
                        M1Comment = "(CALL INSERTED)"
                    },
                    AlternateExecutionTimings = new InstructionExecutionVariant() {
                        Condition = "if rst",
                        MachineCyclesCount = 3,
                        TStatesCount = 13,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.INTA, TStates = 7 },
                            new MachineCycle() { Type = MachineCycleType.SWH, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.SWL, TStates = 3 } },
                        TStatesByMachineCycle = "13 (7,3,3)",
                        StackOperations = "M1 -1, M2 -1",
                        M1Comment = "(RST INSERTED)"
                    }
                }
                },

                Description = "In this mode, timing depends on the instruction put on the bus. The interrupt processing last 2 clock cycles more than this instruction usually needs.\nTwo typical examples follow:\n- a RST n on the data bus, it takes 13 cycles to get to 'n': \nM1 cycle: 7 ticks\nacknowledge interrupt and decrement SP \nM2 cycle: 3 ticks\nwrite high byte and decrement SP \nM3 cycle: 3 ticks\nwrite low byte and jump to 'n' \n- With a CALL nnnn on the data bus, it takes 19 cycles: \nM1 cycle: 7 ticks\nacknowledge interrupt \nM2 cycle: 3 ticks\nread low byte of 'nnnn' from data bus \nM3 cycle: 3 ticks\nread high byte of 'nnnn' and decrement SP \nM4 cycle: 3 ticks\nwrite high byte of PC to the stack and decrement SP \nM5 cycle: 3 ticks\nwrite low byte of PC and jump to 'nnnn'. "
            };

            Table[159] = new InstructionType()
            {
                Index = 159,
                IsUndocumented = false,
                IsInternalZ80Operation = true,

                InstructionGroup = "",
                OpCodeName = "INT 1",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 0,
                    InstructionCodeSizeInBytes = 0,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 3,
                        TStatesCount = 13,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.INTA, TStates = 7 },
                            new MachineCycle() { Type = MachineCycleType.SWH, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.SWL, TStates = 3 } },
                        TStatesByMachineCycle = "13 (7,3,3)",
                        StackOperations = "M1 -1, M2 -1",
                        M1Comment = "(RST 38H INTERNAL)"
                    }
                }
                },

                Description = "It takes 13 clock cycles to reach #0038: \nM1 cycle: 7 ticks\nacknowledge interrupt and decrement SP \nM2 cycle: 3 ticks\nwrite high byte of PC onto the stack and decrement SP \nM3 cycle: 3 ticks\nwrite low byte onto the stack and to set PC to #0038. "
            };

            Table[160] = new InstructionType()
            {
                Index = 160,
                IsUndocumented = false,
                IsInternalZ80Operation = true,

                InstructionGroup = "",
                OpCodeName = "INT 2",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 0,
                    InstructionCodeSizeInBytes = 0,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 5,
                        TStatesCount = 19,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.INTA, TStates = 7 },
                            new MachineCycle() { Type = MachineCycleType.SWH, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.SWL, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.MRL, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.MRH, TStates = 3 } },
                        TStatesByMachineCycle = "19 (7,3,3,3,3)",
                        StackOperations = "M1 -1, M2 -1",
                        M1Comment = "(VECTOR SUPPLIED)"
                    }
                }
                },

                Description = "It takes 19 clock cycles to get to the interrupt routine: \nM1 cycle: 7 ticks\nacknowledge interrupt and decrement SP \nM2 cycle: 3 ticks\nwrite high byte of PC onto stack and decrement SP \nM3 cycle: 3 ticks\nwrite low byte onto the stack \nM4 cycle: 3 ticks\nread low byte from the interrupt vector \nM5 cycle: 3 ticks\nread high byte from bus and jump to interrupt routine "
            };

            Table[161] = new InstructionType()
            {
                Index = 161,
                IsUndocumented = false,
                IsInternalZ80Operation = true,

                InstructionGroup = "",
                OpCodeName = "RESET",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 0,
                    InstructionCodeSizeInBytes = 0,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 1,
                        TStatesCount = 3,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.CPU, TStates = 3 } },
                        TStatesByMachineCycle = "1 (3)"
                    }
                }
                },

                Description = "It takes 3 clock cycles: \nIFF1 and IFF2 as well as interrupt mode is set to 0 \nPC is set to 0, I and R registers are reset also. \nSP is set to 0xffff as well as the A and the F register is set to 0xff. "
            };

            Table[162] = new InstructionType()
            {
                Index = 162,
                IsUndocumented = false,
                IsInternalZ80Operation = true,

                InstructionGroup = "",
                OpCodeName = "?",

                ParametersVariants = new InstructionParametersVariant[] {
                new InstructionParametersVariant() {
                    Index = 0,
                    IsUndocumented = false,
                    InstructionCodeCount = 0,
                    InstructionCodeSizeInBytes = 0,
                    ExecutionTimings = new InstructionExecutionVariant() {
                        MachineCyclesCount = 4,
                        TStatesCount = 16,
                        MachineCycles = new MachineCycle[] {
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OCF, TStates = 4 },
                            new MachineCycle() { Type = MachineCycleType.OD, TStates = 3 },
                            new MachineCycle() { Type = MachineCycleType.OC4, TStates = 5 } },
                        TStatesByMachineCycle = "16 (4, 4, 3, 5)"
                    }
                }
                },

                Description = "This generic instruction is executed by the CPU during the opcode fetch phase.\nIt is then replaced by the instruction that was decoded from the fetched opcode during the last OCF machine cycle."
            };

        }
    }
}
