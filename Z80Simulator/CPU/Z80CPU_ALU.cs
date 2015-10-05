using System.Text;
using Z80Simulator.Instructions;

namespace Z80Simulator.CPU
{
    /// <summary>
    /// ZiLOG Z80 CPU
    /// 
    /// Arithmetic Logic Unit (ALU)
    /// 
    /// The 8-bit arithmetic and logical instructions of the CPU are executed in the
    /// ALU. Internally, the ALU communicates with the registers and the external
    /// data bus by using the internal data bus. Functions performed by the ALU
    /// include:
    /// - Add
    /// - Subtract
    /// - Logical AND
    /// - Logical OR
    /// - Logical Exclusive OR
    /// - Compare
    /// - Left or Right Shifts or Rotates (Arithmetic and Logical)
    /// - Increment
    /// - Decrement
    /// - Set Bit
    /// - Reset Bit
    /// - Test bit/
    /// </summary>
    public partial class Z80CPU
    {
        #region Accumulator and Flag Registers

        // The CPU includes two independent 8-bit accumulators and associated 8-bit
        // flag registers. The accumulator holds the results of 8-bit arithmetic or logical
        // operations while the FLAG register indicates specific conditions for 8-bit or
        // 16-bit operations, such as indicating whether or not the result of an
        // operation is equal to zero. The programmer selects the accumulator and flag
        // pair with a single exchange instruction so that it is possible to work with
        // either pair.

        // Registers memory storage
        private byte[,] aluRegisters = new byte[2, 2];

        // Registers multiplexer
        private int activeALURegistersSet = 0;        
        
        // Main Register Set

        /// <summary>
        /// Accumulator (A - Main Register Set)
        /// 
        /// The accumulator holds the results of 8-bit arithmetic or logical
        /// operations.
        /// </summary>
        public byte A
        {
            get { return aluRegisters[0, activeALURegistersSet]; }
            private set { aluRegisters[0, activeALURegistersSet] = value; }
        }

        /// <summary>
        /// Flags  (F - Main Register Set)
        /// 
        /// The FLAG register indicates specific conditions for 8-bit or
        /// 16-bit operations, such as indicating whether or not the result of an
        /// operation is equal to zero. 
        /// </summary>
        public byte F
        {
            get { return aluRegisters[1, activeALURegistersSet]; }
            private set { aluRegisters[1, activeALURegistersSet] = value; }
        }

        // Individuals flags stored in the F register
         
        /// <summary>
        /// SF flag
        /// Set if the 2-complement value is negative. It's simply a copy of the most significant bit.
        /// </summary>
        public bool SF
        {
            get { return (F & 0x80) != 0; }
            private set { if(value) { F |= 0x80;  } else { F &= 0x7F;  } }
        }

        /// <summary>
        /// ZF flag
        /// Set if the result is zero.
        /// </summary>
        public bool ZF
        {
            get { return (F & 0x40) != 0; }
            private set { if(value) { F |= 0x40;  } else { F &= 0xBF;  } }
        }

        /// <summary>
        /// YF flag 
        /// A copy of bit 5 of the result.
        /// </summary>
        public bool YF
        {
            get { return (F & 0x20) != 0; }
            private set { if(value) { F |= 0x20;  } else { F &= 0xDF;  } }
        }

        /// <summary>
        /// HF flag 
        /// The half-carry of an addition/subtraction (from bit 3 to 4). Needed for BCD correction with DAA.
        /// </summary>
        public bool HF
        {
            get { return (F & 0x10) != 0; }
            private set { if(value) { F |= 0x10;  } else { F &= 0xEF;  } }
        }

        /// <summary>
        /// XF flag 
        /// A copy of bit 3 of the result.
        /// </summary>
        public bool XF
        {
            get { return (F & 0x08) != 0; }
            private set { if(value) { F |= 0x08;  } else { F &= 0xF7;  } }
        }

        /// <summary>
        /// PF flag 
        /// This flag can either be the parity of the result (PF), or the 2-compliment signed overflow (VF): set if 2-compliment value doesn't fit in the register.
        /// </summary>
        public bool PF
        {
            get { return (F & 0x04) != 0; }
            private set { if(value) { F |= 0x04;  } else { F &= 0xFB;  } }
        }

        /// <summary>
        /// NF flag 
        /// Shows whether the last operation was an addition (0) or an subtraction (1). This information is needed for DAA.
        /// </summary>
        public bool NF
        {
            get { return (F & 0x02) != 0; }
            private set { if(value) { F |= 0x02;  } else { F &= 0xFD;  } }
        }

        /// <summary>
        /// CF flag 
        /// The carry flag, set if there was a carry after the most significant bit.
        /// </summary>
        public bool CF
        {
            get { return (F & 0x01) != 0; }
            private set { if(value) { F |= 0x01;  } else { F &= 0xFE;  } }
        }
        
        /// <summary>
        /// 16-bit ALU Register (AF - Main Register Set)
        /// </summary>
        public ushort AF
        {
            get { return (ushort)((A << 8) + F); }
            private set { A = (byte)(value >> 8); F = (byte)(value & 0xFF); }
        }

        // Alternate Register Set

        /// <summary>
        /// Accumulator (A' - Alternate Register Set)
        /// 
        /// The accumulator holds the results of 8-bit arithmetic or logical
        /// operations.
        /// </summary>
        public byte A2
        {
            get { return aluRegisters[0, 1 - activeALURegistersSet]; }
            private set { aluRegisters[0, 1 - activeALURegistersSet] = value; }
        }

        /// <summary>
        /// Flags  (F' - Alternate Register Set)
        /// 
        /// The FLAG register indicates specific conditions for 8-bit or
        /// 16-bit operations, such as indicating whether or not the result of an
        /// operation is equal to zero. 
        /// </summary>
        public byte F2
        {
            get { return aluRegisters[1, 1 - activeALURegistersSet]; }
            private set { aluRegisters[1, 1 - activeALURegistersSet] = value; }
        }

        /// <summary>
        /// 16-bit ALU Register (AF' - Alternate Register Set)
        /// </summary>
        public ushort AF2
        {
            get { return (ushort)((A2 << 8) + F2); }
            private set { A2 = (byte)(value >> 8); F2 = (byte)(value & 0xFF); }
        }

        #endregion

        #region ALU input buffer Registers

        /// <summary>
        /// This internal register is used to buffer the left input of the ALU.
        /// It is connected to the Accumulator (A) register.
        /// </summary>
        private byte ALULeftBuffer { get; set; }

        /// <summary>
        /// This internal register is used to buffer the right input of the ALU.
        /// It is connected to the internal data bus of the CPU.
        /// </summary>
        private byte ALURightBuffer { get; set; }

        #endregion

        #region Micro-instructions           
        
        // Binary constants used to improve code readability
        private const int B00000001 = 0x01;
        private const int B00000010 = 0x02;
        private const int B00000100 = 0x04;
        private const int B00001000 = 0x08;
        private const int B00001111 = 0x0F;
        private const int B00010000 = 0x10;
        private const int B00010010 = 0x12;
        private const int B00101000 = 0x28;
        private const int B00111010 = 0x3A;
        private const int B01000000 = 0x40;
        private const int B10000000 = 0x80;
        private const int B10000001 = 0x81;
        private const int B10101000 = 0xA8;
        private const int B11000001 = 0xC1;
        private const int B11000100 = 0xC4;
        private const int B11000101 = 0xC5;        
        private const int B11010010 = 0xD2;
        private const int B11010111 = 0xD7;
        private const int B11110000 = 0xF0;
        private const int B11111101 = 0xFD;
        private const int B11111110 = 0xFE;

        #region Arithmetic operations

        private enum ArithmeticOperationType
        {
            Addition,
            LeftMinusRight,
            RightMinusLeft
        }

        /// <summary>
        /// Add left and right ALU buffers, put the result on the internal data bus, update the Flags accordingly
        /// </summary>
        /// <param name="withInputCarry">If true, add one when the CF flag is set before the operation</param>
        /// <param name="withOutputCarry">If true, update the value of the CF flag after operation</param>
        /// <param name="with16bitsZFlag">If true, update the Z flag according to 16 bits arithmetic semantics</param>
        /// <param name="withSZPFlags">If false, do not update the S, Z, and P/V flags</param>
        /// <param name="withComparisonYXFlags">If true, copy Y and X flags from the right buffer instead of the result</param>
        private void ExecuteArithmeticOperation(ArithmeticOperationType operation, bool withInputCarry, bool withOutputCarry, bool with16bitsZFlag, bool withSZPFlags, bool withComparisonYXFlags)
        {
            // Carry flag (bit 0) is considered only in case of add or sub with carry
            int inputCarry = 0;
            if (withInputCarry)
            {
                inputCarry = F & B00000001;
            }

            // For 16 bits operations performed in 2 stages, check if the previous operation result was Zero
            bool previousOperationResultWasZero = ZF;

            // Perform the operation using our host CPU
            int result = 0;
            switch (operation)
            {
                case ArithmeticOperationType.Addition:
                    result = ALULeftBuffer + ALURightBuffer + inputCarry;
                    break;
                case ArithmeticOperationType.LeftMinusRight:
                    result = ALULeftBuffer - ALURightBuffer - inputCarry;
                    break;
                case ArithmeticOperationType.RightMinusLeft:
                    result = ALURightBuffer - ALULeftBuffer - inputCarry;
                    break;
            }

            // Compute the carries associated with this operation afterwards.
            int carries = (ALULeftBuffer ^ ALURightBuffer ^ result) >> 1;
            // Explanation of the formula above :
            // Consider bit n of the result : if no carry was forwarded from the previous bit, this bit is set if a and b bits are different (0 + 1 = 1 + 0 = 1), reset if a and b bits are the same (0 + 0 = 0, 1 + 1 = 0 + carry for next bit).
            // On the contrary, a carry must have been forwarded from bit n-1 to bit n : if a and b bits are different (a ^ b = 1) and result is reset (result = 0), or if a and b bits are the same (a ^ b = 0) and result is set (result = 1).
            // Formula : carry[n-1 to n] = ((a[n] ^ b[n] == 1 && result[n] == 0) || (a[n] ^ b[n] == 0 & result[n] == 1)).
            // Simplification : carry[n-1 to n] = a[n] ^ b[n] ^ result[n].            
            // In the variable "carries" below, when bit n is set  it means that there was a carry from bit n to bit n+1 during the addition.

            // Truncate the result to 8 bits 
            byte truncResult = (byte)result;
            // and send it to the internal data bus. 
            InternalDataBus = truncResult;

            // Compute flags

            // Prepare a binary mask to preserve the flags which are not affected by the current operation
            byte notAffectedFlagsMask = 0;
            if (!withOutputCarry)
            {
                notAffectedFlagsMask |= B00000001;
            }
            if(!withSZPFlags)
            {
                notAffectedFlagsMask |= B11000100;
            }
            // Preserve all flags which are not affected by the current operation
            F = (byte)(F & notAffectedFlagsMask);

            if (withSZPFlags)
            {
                // Flags bit 7 - SF flag - Set if the 2-complement value is negative. It's simply a copy of the most signifcant bit of the result.
                F |= (byte)(truncResult & B10000000);

                // Flags bit 6 - ZF flag Set if the result is zero.
                if (truncResult == 0 && (!with16bitsZFlag || previousOperationResultWasZero)) F |= B01000000;

                // Flags bit 2 - VF flag 2-compliment signed overflow : set if 2-compliment value doesn't fit in the register.
                F |= (byte)(((carries & B10000000) >> 5) ^ ((carries & B01000000) >> 4));
                // Explanation of the formula above :
                // The idea behind setting the overflow flag is simple. Suppose you sign-extend your 8-bit signed integers to 9 bits, that is, just copy the 7th bit to an extra, 8th bit. An overflow/underflow will occur if the 9-bit sum/difference of these 9-bit signed integers has different values in bits 7 and 8, meaning that the addition/subtraction has lost the result's sign in the 7th bit and used it for the result's magnitude, or, in other words, the 8 bits can't accommodate the sign bit and such a large magnitude.
                // Now, bit 7 of the result can differ from the imaginary sign bit 8 if and only if the carry into bit 7 and the carry into bit 8 (=carry out of bit 7) are different. That's because we start with the addends having bit 7=bit 8 and only different carry-ins into them can affect them in the result in different ways.
                // So overflow flag = carry-out flag XOR carry from bit 6 into bit 7.
            }

            // Undocumented - Flags bits 5 and 3
            if (!withComparisonYXFlags)
            {
                // Flags bit 5 - YF flag - A copy of bit 5 of the result.
                // Flags bit 3 - XF flag - A copy of bit 3 of the result.
                F |= (byte)(truncResult & B00101000);
            }
            else
            {
                // Special case for comparison operation : YF and XF flags are copied from the right operand, not the result.
                F |= (byte)(ALURightBuffer & B00101000);
            }

            // Flags bit 4 - HF flag The half-carry of an addition/subtraction (from bit 3 to 4). Needed for BCD correction with DAA.
            F |= (byte)((carries & B00001000) << 1);

            // Flags bit 1 - NF flag Shows whether the last operation was an addition (0) or a subtraction (1). This information is needed for DAA.            
            if (operation != ArithmeticOperationType.Addition)
            {
                F |= B00000010;
            }
            
            if (withOutputCarry)
            {
                // Flags bit 0 - CF flag The carry flag, set if there was a carry after the most significant bit.
                F |= (byte)((carries & B10000000) >> 7);
            }
        }

        private void Add(bool withInputCarry, bool with16bitsZFlag, bool withSZPFlags)
        {
            // Perform an addition without input carry
            ExecuteArithmeticOperation(ArithmeticOperationType.Addition, withInputCarry, true, with16bitsZFlag, withSZPFlags, false);

            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.ArithmeticOperationAdd, withInputCarry, with16bitsZFlag, withSZPFlags));
            }
        }

        private void Increment()
        {
            // Load value 1 into ALULeftBuffer
            ALULeftBuffer = 1;

            // Perform a simple addition without input or output carry
            ExecuteArithmeticOperation(ArithmeticOperationType.Addition, false, false, false, true, false);

            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.ArithmeticOperationIncrement));
            }
        }
        
        private void Sub(bool withInputCarry, bool with16bitsZFlag)
        {
            // Perform a subtraction without input carry
            ExecuteArithmeticOperation(ArithmeticOperationType.LeftMinusRight, withInputCarry, true, with16bitsZFlag, true, false);

            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.ArithmeticOperationSub, withInputCarry, with16bitsZFlag));
            }
        }

        private void Decrement()
        {
            // Load value 1 into ALULeftBuffer
            ALULeftBuffer = 1;
            
            // Perform a simple subtraction without input or output carry
            ExecuteArithmeticOperation(ArithmeticOperationType.RightMinusLeft, false, false, false, true, false);

            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.ArithmeticOperationDecrement));
            }
        }
               
        private void Compare()
        {
            // Perform a comparison = substraction without input carry
            ExecuteArithmeticOperation(ArithmeticOperationType.LeftMinusRight, false, true, false, true, true);

            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.ArithmeticOperationCompare));
            }
        }

        private void BlockCompare()
        {
            // Save previous state of CF ﬂag
            bool CFbefore = CF;

            // For CPI/CPIR/CPD/CPDR. This instruction compares a series of bytes in memory to register A. 
            // Eﬀectively, it can be said it does CP (HL) at every iteration.             
            // The result of that compare sets the HF ﬂag, which is important for the next step.  
            ExecuteArithmeticOperation(ArithmeticOperationType.LeftMinusRight, false, true, false, true, true);

            // SF, ZF, HF ﬂags Set by the hypothetical CP (HL).            
            // NF ﬂag Always set.
            F = (byte)(F & B11010010);

            // CF ﬂag Unchanged.
            CF = CFbefore;
            // PF ﬂag Set if BC is not 0 after the decrement.
            PF = BC != 1;

            // Take the value of register A, subtract the value of the memory address, and ﬁnally subtract the value of HF ﬂag, which is set or reset by the hypothetical CP (HL). So, n = A - (HL) - HF.
            int n = A - ALURightBuffer - (HF ? 1 : 0);

            // YF ﬂag A copy of bit 1 of n.
            YF = (n & B00000010) != 0;
            // XF ﬂag A copy of bit 3 of n.
            XF = (n & B00001000) != 0;
            

            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.ArithmeticOperationBlockCompare));
            }
        }

        private void DecimalAdjust()
        {
            // In BCD representation, each decimal numeral is represented by a four 
            // bit pattern : http://en.wikipedia.org/wiki/Binary-coded_decimal.
            // Numbers between 00 and 99 can thus be encoded in one byte = 2 x 4 bits.
            // Bits :               7654        3210
            // Decimal digits :     highDigit   lowDigit            
            // For example, decimal number 85 is encoded as :
            // Bits :               1000        0101
            // Decimal digits :     8           5
            // It can simply be written as hexadecimal number 0x85.

            // This instruction is executed after an arithmetic operation,
            // addition (if flag NF = 0) or a subtraction (if flag NF = 1).
            ArithmeticOperationType previousOperation = ArithmeticOperationType.Addition;
            if (NF == true)
            {
                previousOperation = ArithmeticOperationType.LeftMinusRight;
            }

            // This instruction computes the value to add or subtract into ALURightBuffer, 
            // to adjust the accumulator after a BCD operation (addition or subtraction).

            // The result of adding idependently each one of the decimal digits 
            // of the two initial operands can be found by taking into account 
            // the half carry (carry between bits 3 and 4) and the full carry
            // (between bit 7 and the next byte) produced by the operation.   
            int resultAfterOperation = ALULeftBuffer;
            bool halfCarrySetDuringOperation = HF;
            bool fullCarrySetDuringOperation = CF;
                       
            // If the addition of the first 4 bits gave a result
            // between 0 and 9, then no correction is necessary.
            // Example 1 : 0x4 + 0x5 = 0x9, the result is correct.

            int lowNibbleAfterOperation = resultAfterOperation & B00001111;
            int correction = 0;

            // But if the addition / substraction of the first 4 bits gave a result
            // at least equal to ten (between A and F or with a half carry), then we 
            // must translate the number from base 16 (4 bits) to base 10, and produce 
            // a half carry to increment the next digit in base 10 if necessary.
            // To do this, we just have to add 6 to the result (2^4 - 10).
            // Example 2 : 0x6 + 0x7 = 0xD, adjust with +6 : 0xD + 6 = 0x13.
            // Example 3 : 0x8 + 0x9 = 0x11, adjust with +6 : 0x11 + 6 = 0x17.
            // After the correction, a new half carry will be propagated to the 
            // higher 4 bits only if the value of the low nibble is between 
            // 0xA and 0xF.
            if (lowNibbleAfterOperation > 0x09 || halfCarrySetDuringOperation)
            {
                correction = 6;
            }

            // Apply the same kind of correction to the higher digit 
            if (resultAfterOperation > 0x99 || fullCarrySetDuringOperation)
            {
                correction += 0x60;
            }

            // Load into ALURightBuffer the correction to add or subtract
            ALURightBuffer = (byte)correction;

            // Execute the operation to adjust the result
            ExecuteArithmeticOperation(previousOperation, false, true, false, true, false);

            // For arithmetic operations, the flag at nit 2 indicates an Overflow condition
            // But with DAA instruction, is is used to indicate the resulting parity is Even
            // The number of 1 bits in a byte are counted. 
            // If the total is Odd, ODD parity is flagged (P = 0). 
            // If the total is Even, EVEN parity is flagged (P = 1).
            PF = numberOfBitsInByteParityTable[InternalDataBus];

            // Adjust the carry flag to take both operation + correction into account
            CF = resultAfterOperation > 0x99 | fullCarrySetDuringOperation;            

            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.ArithmeticOperationDecimalAdjust));
            }            
        }

        #endregion

        #region Flags and Accumulator operations

        private void SwitchALUAlternateRegisterSet()
        {
            activeALURegistersSet = 1 - activeALURegistersSet;
        }

        private void SetCarryFlag()
        {
            F = (byte)(F & B11000100);  // reset all affected bits 
            F |= (byte)(A & B00101000); // copy bits 3 and 5 from A     
            F |= B00000001;             // set CF flag

            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.FlagsOperationSetCarry));
            }
        }

        private void InvertCarryFlag()
        {
            byte previousCF = (byte)(F & B00000001); // save CF state before the instruction

            F = (byte)(F & B11000100);               // reset all affected bits 
            F |= (byte)(A & B00101000);              // copy bits 3 and 5 from A 
            F |= (byte)(previousCF << 4);            // copy CF state before the instruction into HF (bit 4)
            F |= (byte)(1 - previousCF);             // toggle CF flag

            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.FlagsOperationInvertCarry));
            }
        }

        private void InvertAccumulator()
        {
            A = (byte)(~A); // Invert Accumulator content

            F  = (byte)(F & B11000101);  // reset all affected bits 
            F |= (byte)(A & B00101000);  // copy bits 3 and 5 from A 
            F |= B00010010;              // set bits 1 and 4

            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.FlagsOperationInvertAccumulator));
            }
        }

        private bool CheckFlagCondition(FlagCondition flagCondition)
        {
            switch (flagCondition)
            {
                // NZ non zero Z
                case FlagCondition.NZ:
                    return !ZF;
                // Z  zero Z
                case FlagCondition.Z:
                    return ZF;
                // NC no carry C
                case FlagCondition.NC:
                    return !CF;
                // C carry C
                case FlagCondition.C:
                    return CF;
                // PO parity odd P/V
                case FlagCondition.PO:
                    return !PF;
                // PE parity even P/V
                case FlagCondition.PE:
                    return PF;
                // P sign positive S
                case FlagCondition.P:
                    return !SF;
                // M sign negative S
                //case FlagCondition.M:
                default:
                    return SF;
            }
        }

        private void ComputeFlagsForLDAIandR()
        {
            // Special case : LD A,I and LD A,R => impact on the flags
            if (instructionOrigin.OpCode.InstructionTypeParamVariant == 5)
            {
                // C is not affected, H is reset, N is reset                    
                F = (byte)(F & B00000001);  // reset all affected bits 

                // Copy bits 3 and 5 from I/R-Register
                F |= (byte)(InternalDataBus & B00101000);

                // S is set if I/R-Register is negative; reset otherwise
                F |= (byte)(InternalDataBus & B10000000);
                // Z is set if I/R-Register is zero; reset otherwise
                if (InternalDataBus == 0) F |= B01000000;

                // P/V contains contents of IFF2
                // If an interrupt occurs during execution of this instruction, the Parity flag contains a 0. (??)
                if (IFF2) F |= B00000100;

                if (TraceMicroInstructions)
                {
                    TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.FlagsOperationComputeFlagsForLDAIandR));
                }
            }
        }

        private void ComputeFlagsForMemBlockLoad()
        {
            // The LDI/LDIR/LDD/LDDR instructions aﬀect the ﬂags in a strange way. 
            // At every iteration, a byte is copied. 
            // Take that byte and add the value of register A to it. Call that value n. 
            int n = InternalDataBus + A;

            // SF, ZF, CF ﬂags These ﬂags are unchanged.
            // HF, NF ﬂag Always reset.
            F = (byte)(F & B11000001);

            // YF ﬂag A copy of bit 1 of n.
            YF = (n & B00000010) != 0;
            // XF ﬂag A copy of bit 3 of n.
            XF = (n & B00001000) != 0;

            // PF ﬂag Set if BC not 0.
            PF = BC != 0;

            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.FlagsOperationComputeFlagsForMemBlockLoad));
            }
        }
                       
        private void ComputeFlagsForInput()
        {
            // C is not affected, H is reset, N is reset                    
            F = (byte)(F & B00000001);  // reset all affected bits 

            // Copy bits 3 and 5 from input data
            F |= (byte)(InternalDataBus & B00101000);

            // S is set if input data is negative; reset otherwise
            F |= (byte)(InternalDataBus & B10000000);
            // Z is set if input data is zero; reset otherwise
            if (InternalDataBus == 0) F |= B01000000;

            // P/V is set if parity of input data is even; reset otherwise
            if (numberOfBitsInByteParityTable[InternalDataBus]) F |= B00000100;

            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.FlagsOperationComputeFlagsForInput));
            }
        }

        private enum IOBlockInstructionType
        {
            INI,
            IND,
            OUT
        }

        private void ComputeFlagsForIOBlockInstruction(IOBlockInstructionType instructionType)
        {
            // The out instructions behave diﬀerently than the in instructions, which doesn’t make the CPU very symmetrical.
            // First of all, all instructions aﬀect the following ﬂags:
            F = 0;

            // SF, ZF, YF, XF ﬂags Aﬀected by decreasing register B, as in DEC B.
                // For OUT instructions, B has already been decremented when we compute flags
                byte decB = B;
                // For IN instructions, B has not yet been decremented when we compute flags, so we consider B-1
                if (instructionType == IOBlockInstructionType.IND || instructionType == IOBlockInstructionType.INI)
                {
                    decB = (byte)(B - 1);
                }
                // Flags bit 7 - SF flag - Set if the 2-complement value is negative. It's simply a copy of the most signifcant bit of the result.
                F |= (byte)(decB & B10000000);
                // Flags bit 6 - ZF flag Set if the result is zero.
                if (decB == 0) F |= B01000000;
                // Flags bit 5 - YF flag - A copy of bit 5 of the result.
                // Flags bit 3 - XF flag - A copy of bit 3 of the result.
                F |= (byte)(decB & B00101000);
                                    
            // NF ﬂag A copy of bit 7 of the value read from or written to an I/O port.
            NF = (InternalDataBus & B10000000) != 0;

            int k = 0;
            // And now the for OUTI/OTIR/OUTD/OTDR instructions. 
            if (instructionType == IOBlockInstructionType.OUT)
            {
                // Take state of the L after the increment or decrement of HL; add the value written to the I/O port to; call that k for now. 
                // HF and CF Both set if ((HL) + L > 255)
                // PF The parity of ((((HL) + L) & 7) xor B)
                k = InternalDataBus + L;
            }
            else
            {
                // INI/INIR/IND/INDR use the C register in stead of the L register. 
                // There is a catch though, because not the value of C is used, but C + 1 if it’s INI/INIR or C - 1 if it’s IND/INDR.

                // So, ﬁrst of all INI/INIR:
                if (instructionType == IOBlockInstructionType.INI)
                {
                    // HF and CF Both set if ((HL) + ((C + 1) & 255) > 255)
                    // PF The parity of (((HL) + ((C + 1) & 255)) & 7) xor B)
                    k = InternalDataBus + ((C + 1) & 255);
                }
                // And last IND/INDR:
                else if (instructionType == IOBlockInstructionType.IND)
                {
                    // HF and CF Both set if ((HL) + ((C - 1) & 255) > 255)
                    // PF The parity of (((HL) + ((C - 1) & 255)) & 7) xor B)
                    k = InternalDataBus + ((C - 1) & 255);
                }
            }

            // If k > 255, then the CF and HF ﬂags are set. 
            // HF and CF Both set if ((HL) + L > 255)
            CF = k > 255;
            HF = k > 255;

            // The PF ﬂags is set like the parity of k bitwise and’ed with 7, bitwise xor’ed with B.
            // PF The parity of ((((HL) + L) & 7) xor B)
            PF = numberOfBitsInByteParityTable[(byte)((k & 7) ^ decB)];

            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.FlagsOperationComputeFlagsForIOBlockInstruction));
            }
        }

        #endregion

        #region Logical operations

        private enum LogicalOperationType
        {
            And,
            Or,
            Xor
        }

        private void ExecuteLogicalOperation(LogicalOperationType operation)
        {            
            // Perform the logical operation using our host CPU
            int result = 0;
            switch (operation)
            {
                case LogicalOperationType.And:
                    result = ALULeftBuffer & ALURightBuffer;
                    break;
                case LogicalOperationType.Or:
                    result = ALULeftBuffer | ALURightBuffer;
                    break;
                case LogicalOperationType.Xor:
                    result = ALULeftBuffer ^ ALURightBuffer;
                    break;
            }

            // Truncate the result to 8 bits and send it to the internal data bus. 
            InternalDataBus = (byte)result;

            // Compute all flags

            // Flags bit 7 - SF flag - Set if the 2-complement value is negative. It's simply a copy of the most signifcant bit.
            // Flags bit 5 - YF flag - A copy of bit 5 of the result.
            // Flags bit 3 - XF flag - A copy of bit 3 of the result.
            F = (byte)(result & B10101000);

            // Flags bit 6 - ZF flag Set if the result is zero.
            if (result == 0) F |= B01000000;

            if (operation == LogicalOperationType.And)
            {
                // Flags bit 4 - HF flag is set
                F |= B00010000;
            }
            else if (operation == LogicalOperationType.Or || operation == LogicalOperationType.Xor)
            {
                // Flags bit 4 - HF flag is reset
            }
            
            // Flags bit 2 - PF flag - This flag is also used with logical operations and rotate instructions to
            // indicate the resulting parity is Even. The number of 1 bits in a byte are
            // counted. If the total is Odd, ODD parity is flagged (P = 0). If the total is
            // Even, EVEN parity is flagged (P = 1).
            if(numberOfBitsInByteParityTable[result]) F |= B00000100;

            // Flags bit 1 - NF flag is reset
            // Flags bit 0 - CF flag is reset
        }

        private void And()
        {
            // Perform the logical operation
            ExecuteLogicalOperation(LogicalOperationType.And);

            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.LogicalOperationAnd));
            }
        }

        private void Or()
        {
            // Perform the logical operation
            ExecuteLogicalOperation(LogicalOperationType.Or);

            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.LogicalOperationOr));
            }
        }

        private void Xor()
        {
            // Perform the logical operation
            ExecuteLogicalOperation(LogicalOperationType.Xor);

            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.LogicalOperationXor));
            }
        }

        /// <summary>
        /// Lookup table used to compute the parity flag
        /// 
        /// Take the value of one byte                      (val1 = 15 / val2 = 254 )
        /// Write it in binary format                       (val1 = 00001111 / val2 = 11111110)
        /// Count the number of bits set                    (val1 : 4 bits set / val2 : 7 bits set)
        /// Compute the parity of the number of bits set    (val1 : even / val2 : odd)
        /// 
        /// In the table below :
        /// - the boolean value at index 15 is true because the parity of the number of bits set in number 15 is even (last value of first line)
        /// - the boolean value at index 254 is false because the parity of the number of bits set in number 254 is odd (penultimate value of last line)
        /// </summary>
        private static bool[] numberOfBitsInByteParityTable = { 
            true,false,false,true,false,true,true,false,false,true,true,false,true,false,false,true,  
            false,true,true,false,true,false,false,true,true,false,false,true,false,true,true,false,  
            false,true,true,false,true,false,false,true,true,false,false,true,false,true,true,false,  
            true,false,false,true,false,true,true,false,false,true,true,false,true,false,false,true,  
            false,true,true,false,true,false,false,true,true,false,false,true,false,true,true,false,  
            true,false,false,true,false,true,true,false,false,true,true,false,true,false,false,true,  
            true,false,false,true,false,true,true,false,false,true,true,false,true,false,false,true,  
            false,true,true,false,true,false,false,true,true,false,false,true,false,true,true,false,  
            false,true,true,false,true,false,false,true,true,false,false,true,false,true,true,false,  
            true,false,false,true,false,true,true,false,false,true,true,false,true,false,false,true,  
            true,false,false,true,false,true,true,false,false,true,true,false,true,false,false,true,  
            false,true,true,false,true,false,false,true,true,false,false,true,false,true,true,false,  
            true,false,false,true,false,true,true,false,false,true,true,false,true,false,false,true,  
            false,true,true,false,true,false,false,true,true,false,false,true,false,true,true,false,  
            false,true,true,false,true,false,false,true,true,false,false,true,false,true,true,false,  
            true,false,false,true,false,true,true,false,false,true,true,false,true,false,false,true };

        #endregion

        #region Bit operations

        private void BitTest(byte position, AddressingMode source)
        {
            bool bitIsSet = (ALURightBuffer & (1 << position)) != 0;

            // SF ﬂag Set if n = 7 and tested bit is set.
            SF = bitIsSet && position == 7;
            // ZF ﬂag Set if the tested bit is reset.
            ZF = !bitIsSet;
            // PF ﬂag Set just like ZF ﬂag.           
            PF = ZF;
            // HF ﬂag Always set.
            HF = true;
            // NF ﬂag Always reset.          
            NF = false;
            // CF ﬂag Unchanged.

            // Bits 3 and 5 are computed below
            F &= B11010111;
                
            // Contrary to what is said in Z80-Undocumented, 
            // bits 3 and 5 are always copied from the test value
            // for BIT n,r
            if (source == AddressingMode.Register)
            {
                F |= (byte)(ALURightBuffer & B00101000);
            }
            // With the BIT n,(IX+d) instructions, YF and XF are not copied from the result but from something 
            // completely diﬀerent, namely bit 5 and 3 of the high byte of IX+d (so IX plus the displacement). 
            else if (source == AddressingMode.Indexed)
            {
               F |= (byte)((InternalAddressBus >> 8) & B00101000);
            }
            // With the BIT n,(HL) instruction. YF and XF are copied from internal register MEMPTR.
            else if (source == AddressingMode.RegisterIndirect)
            {
               F |= (byte)(W & B00101000);
            }

            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.BitOperationTest, position, source));
            }
        }

        private void BitSet(byte position)
        {
            InternalDataBus = (byte)(ALURightBuffer | (1 << position));

            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.BitOperationSet, position));
            }
        }

        private void BitReset(byte position)
        {
            InternalDataBus = (byte)(ALURightBuffer & ~(1 << position));

            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.BitOperationReset, position));
            }
        }

        #endregion

        #region Rotate and shift

        private enum OperationDirection
        {
            Left,
            Right
        }

        private void Rotate(OperationDirection direction, bool includeCarryInRotation, bool forAccumulator)
        {
            // Select byte to rotate
            byte result = 0;
            if (forAccumulator)
            {
                result = ALULeftBuffer;
            }
            else
            {
                result = ALURightBuffer;
            }

            // Rotate 1 bit to the left or to the right
            if(direction == OperationDirection.Left)
            {
                bool leftmostBitIsSet = (result & B10000000) != 0;
                result <<= 1;                
                if ((includeCarryInRotation && CF) | (!includeCarryInRotation && leftmostBitIsSet)) result |= B00000001;
                CF = leftmostBitIsSet;
            } 
            else if(direction == OperationDirection.Right)
            {
                bool rightmostBitIsSet = (result & B00000001) != 0;
                result >>= 1;
                if ((includeCarryInRotation && CF) | (!includeCarryInRotation && rightmostBitIsSet)) result |= B10000000;
                CF = rightmostBitIsSet;
            }

            // Send result to the data bus
            InternalDataBus = result;

            // Compute flags

            // HF an NF are always reset
            // XF and YF will be updated
            F &= B11000101;

            // Flags bit 5 - YF flag - A copy of bit 5 of the result.
            // Flags bit 3 - XF flag - A copy of bit 3 of the result.
            F |= (byte)(result & B00101000);

            if (!forAccumulator)
            {
                // SF flag - Set if the 2-complement value is negative. It's simply a copy of the most signifcant bit of the result.                
                SF = (result & B10000000) != 0;

                // ZF flag Set if the result is zero.
                ZF = result == 0;

                // PF : parity of the result
                PF = numberOfBitsInByteParityTable[result];
            }

            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.BitOperationRotate, direction, includeCarryInRotation, forAccumulator));
            }
        }

        private void RotateBCDDigit(OperationDirection direction)
        {            
            int result = 0;
            if (direction == OperationDirection.Left)
            {
                // Move lower nibble to higher nibble in right buffer
                result = ALURightBuffer << 4;
                // Move lower nibble of left buffer to lower nibble of right buffer
                result |= ALULeftBuffer & B00001111;
                // Move higher nibble of right buffer to lower nibble of left buffer
                ALULeftBuffer = (byte)((ALULeftBuffer & B11110000) | (ALURightBuffer >> 4));
            }
            else if (direction == OperationDirection.Right)
            {
                // Move higher nibble to lower nibble in right buffer
                result = ALURightBuffer >> 4;
                // Move lower nibble of left buffer to higher nibble of right buffer
                result |= (ALULeftBuffer << 4);
                // Move lower nibble of right buffer to lower nibble of left buffer
                ALULeftBuffer = (byte)((ALULeftBuffer & B11110000) | (ALURightBuffer & B00001111));
            }

            // Send result to the data bus
            byte truncResult = (byte)result;
            InternalDataBus = truncResult;

            // Compute flags

            // HF an NF are always reset
            // SF, ZF, PF, XF and YF will be updated
            F &= B00000001;

            // Flags bit 5 - YF flag - A copy of bit 5 of the result.
            // Flags bit 3 - XF flag - A copy of bit 3 of the result.
            F |= (byte)(ALULeftBuffer & B00101000);

            // Flags bit 7 - SF flag - Set if the 2-complement value is negative. It's simply a copy of the most signifcant bit of the result.                
            F |= (byte)(ALULeftBuffer & B10000000);

            // Flags bit 6 - ZF flag Set if the result is zero.
            if (ALULeftBuffer == 0) F |= B01000000;
            
            // PF : parity of the result
            PF = numberOfBitsInByteParityTable[ALULeftBuffer];

            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.BitOperationRotateBCDDigit, direction));
            }            
        }

        private enum ShiftType
        { 
            Arithmetic,
            Logical
        }

        private void Shift(OperationDirection direction, ShiftType shiftType)
        {
            byte result = ALURightBuffer;
            bool leftmostBitIsSet = (result & B10000000) != 0;

            // Shift 1 bit to the left or to the right
            if (direction == OperationDirection.Left)
            {
                result <<= 1;
                if (shiftType == ShiftType.Logical) result |= B00000001;
                CF = leftmostBitIsSet;
            }
            else if (direction == OperationDirection.Right)
            {
                bool rightmostBitIsSet = (result & B00000001) != 0;
                result >>= 1;
                if (shiftType == ShiftType.Arithmetic && leftmostBitIsSet) result |= B10000000;
                CF = rightmostBitIsSet;
            }

            // Send result to the data bus
            InternalDataBus = result;

            // Compute flags

            // HF an NF are always reset
            // SF, ZF, PF, XF and YF will be updated
            F &= B00000001;

            // Flags bit 5 - YF flag - A copy of bit 5 of the result.
            // Flags bit 3 - XF flag - A copy of bit 3 of the result.
            F |= (byte)(result & B00101000);

            // Flags bit 7 - SF flag - Set if the 2-complement value is negative. It's simply a copy of the most signifcant bit of the result.                
            F |= (byte)(result & B10000000);

            // Flags bit 6 - ZF flag Set if the result is zero.
            if (result == 0) F |= B01000000;

            // PF : parity of the result
            PF = numberOfBitsInByteParityTable[result];

            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.BitOperationShift, direction, shiftType));
            }
        }
        
        #endregion

        #endregion
    }
}