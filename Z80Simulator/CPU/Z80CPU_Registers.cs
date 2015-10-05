using System;
using Z80Simulator.Instructions;

namespace Z80Simulator.CPU
{
    /// <summary>
    /// ZiLOG Z80 CPU
    /// 
    /// Registers
    /// 
    /// The Z80 CPU contains 208 bits of R/W memory that are available to the
    /// programmer. This memory is configured to eighteen 8-bit registers 
    /// and four 16-bit registers. All Z80 registers are implemented
    /// using static RAM. The registers include two sets of six general-purpose
    /// registers that may be used individually as 8-bit registers or in pairs as 16-bit
    /// registers. There are also two sets of accumulator and flag registers and six
    /// special-purpose registers.
    /// </summary>
    public partial class Z80CPU
    {
        #region General Purpose Registers

        // Two matched sets of general-purpose registers, each set containing six 8-bit
        // registers, may be used individually as 8-bit registers or as 16-bit register
        // pairs. One set is called BC, DE, and HL while the complementary set is
        // called BC', DE', and HL'. At any one time, the programmer can select either
        // set of registers to work through a single exchange command for the entire
        // set. In systems that require fast interrupt response, one set of general-purpose
        // registers and an ACCUMULATOR/FLAG register may be reserved for
        // handling this fast routine. One exchange command is executed to switch
        // routines. This greatly reduces interrupt service time by eliminating the
        // requirement for saving and retrieving register contents in the external stack
        // during interrupt or subroutine processing. These general-purpose registers
        // are used for a wide range of applications. They also simplify programing,
        // specifically in ROM-based systems where little external read/write memory
        // is available.

        // Registers memory storage
        private byte[,] gpRegisters = new byte[6, 2];

        // Registers multiplexer
        private int activeRegistersSet = 0;

        // Main Register Set

        /// <summary>
        /// 8-bit General Purpose Register (B - Main Register Set)
        /// </summary>
        public byte B
        {
            get { return gpRegisters[0, activeRegistersSet]; }
            private set { gpRegisters[0, activeRegistersSet] = value; }
        }

        /// <summary>
        /// 8-bit General Purpose Register (C - Main Register Set)
        /// </summary>
        public byte C
        {
            get { return gpRegisters[1, activeRegistersSet]; }
            private set { gpRegisters[1, activeRegistersSet] = value; }
        }

        /// <summary>
        /// 16-bit General Purpose Register (BC - Main Register Set)
        /// </summary>
        public ushort BC
        {
            get { return (ushort)((B << 8) + C); }
            private set { B = (byte)(value >> 8); C = (byte)(value & 0xFF); }
        }

        /// <summary>
        /// 8-bit General Purpose Register (D - Main Register Set)
        /// </summary>
        public byte D
        {
            get { return gpRegisters[2, activeRegistersSet]; }
            private set { gpRegisters[2, activeRegistersSet] = value; }
        }

        /// <summary>
        /// 8-bit General Purpose Register (E - Main Register Set)
        /// </summary>
        public byte E
        {
            get { return gpRegisters[3, activeRegistersSet]; }
            private set { gpRegisters[3, activeRegistersSet] = value; }
        }

        /// <summary>
        /// 16-bit General Purpose Register (DE - Main Register Set)
        /// </summary>
        public ushort DE
        {
            get { return (ushort)((D << 8) + E); }
            private set { D = (byte)(value >> 8); E = (byte)(value & 0xFF); }
        }

        /// <summary>
        /// 8-bit General Purpose Register (H - Main Register Set)
        /// </summary>
        public byte H
        {
            get { return gpRegisters[4, activeRegistersSet]; }
            private set { gpRegisters[4, activeRegistersSet] = value; }
        }

        /// <summary>
        /// 8-bit General Purpose Register (L - Main Register Set)
        /// </summary>
        public byte L
        {
            get { return gpRegisters[5, activeRegistersSet]; }
            private set { gpRegisters[5, activeRegistersSet] = value; }
        }

        /// <summary>
        /// 16-bit General Purpose Register (HL - Main Register Set)
        /// </summary>
        public ushort HL
        {
            get { return (ushort)((H << 8) + L); }
            private set { H = (byte)(value >> 8); L = (byte)(value & 0xFF); }
        }

        // Alternate Register Set

        /// <summary>
        /// 8-bit General Purpose Register (B2 - Alternate Register Set)
        /// </summary>
        public byte B2
        {
            get { return gpRegisters[0, 1 - activeRegistersSet]; }
            private set { gpRegisters[0, 1 - activeRegistersSet] = value; }
        }

        /// <summary>
        /// 8-bit General Purpose Register (C2 - Alternate Register Set)
        /// </summary>
        public byte C2
        {
            get { return gpRegisters[1, 1 - activeRegistersSet]; }
            private set { gpRegisters[1, 1 - activeRegistersSet] = value; }
        }

        /// <summary>
        /// 16-bit General Purpose Register (BC2 - Alternate Register Set)
        /// </summary>
        public ushort BC2
        {
            get { return (ushort)((B2 << 8) + C2); }
            private set { B2 = (byte)(value >> 8); C2 = (byte)(value & 0xFF); }
        }

        /// <summary>
        /// 8-bit General Purpose Register (D2 - Alternate Register Set)
        /// </summary>
        public byte D2
        {
            get { return gpRegisters[2, 1 - activeRegistersSet]; }
            private set { gpRegisters[2, 1 - activeRegistersSet] = value; }
        }

        /// <summary>
        /// 8-bit General Purpose Register (E2 - Alternate Register Set)
        /// </summary>
        public byte E2
        {
            get { return gpRegisters[3, 1 - activeRegistersSet]; }
            private set { gpRegisters[3, 1 - activeRegistersSet] = value; }
        }

        /// <summary>
        /// 16-bit General Purpose Register (DE2 - Alternate Register Set)
        /// </summary>
        public ushort DE2
        {
            get { return (ushort)((D2 << 8) + E2); }
            private set { D2 = (byte)(value >> 8); E2 = (byte)(value & 0xFF); }
        }

        /// <summary>
        /// 8-bit General Purpose Register (H2 - Alternate Register Set)
        /// </summary>
        public byte H2
        {
            get { return gpRegisters[4, 1 - activeRegistersSet]; }
            private set { gpRegisters[4, 1 - activeRegistersSet] = value; }
        }

        /// <summary>
        /// 8-bit General Purpose Register (L2 - Alternate Register Set)
        /// </summary>
        public byte L2
        {
            get { return gpRegisters[5, 1 - activeRegistersSet]; }
            private set { gpRegisters[5, 1 - activeRegistersSet] = value; }
        }

        /// <summary>
        /// 16-bit General Purpose Register (HL2 - Alternate Register Set)
        /// </summary>
        public ushort HL2
        {
            get { return (ushort)((H2 << 8) + L2); }
            private set { H2 = (byte)(value >> 8); L2 = (byte)(value & 0xFF); }
        }

        #endregion

        #region Special-Purpose Registers

        /// <summary>
        /// Program Counter (PC)
        /// 
        /// The program counter holds the 16-bit address of the current instruction
        /// being fetched from memory. The PC is automatically incremented after its
        /// contents have been transferred to the address lines. When a program jump
        /// occurs, the new value is automatically placed in the PC, overriding the
        /// incrementer.
        /// </summary>
        public ushort PC { get; private set; }

        // Lower 8 bit part of PC register (internal use only, useful to move data from 8 bit data bus)
        private byte PCh
        {
            get { return (byte)(PC >> 8); }
            set { PC = (ushort)((value << 8) + PCl); }
        }

        // Lower 8 bit part of PC register (internal use only, useful to move data from 8 bit data bus)
        private byte PCl
        {
            get { return (byte)(PC & 0xFF); }
            set { PC = (ushort)((PCh << 8) + value); }
        }

        /// <summary>
        /// Stack Pointer (SP)
        /// 
        /// The stack pointer holds the 16-bit address of the current top of a stack
        /// located anywhere in external system RAM memory. The external stack
        /// memory is organized as a last-in first-out (LIFO) file. Data can be pushed
        /// onto the stack from specific CPU registers or popped off of the stack to
        /// specific CPU registers through the execution of PUSH and POP
        /// instructions. The data popped from the stack is always the last data pushed
        /// onto it. The stack allows simple implementation of multiple level interrupts,
        /// unlimited subroutine nesting and simplification of many types of data
        /// manipulation.
        /// </summary>
        public ushort SP { get; private set; }

        // Lower 8 bit part of SP register (internal use only, useful to move data from 8 bit data bus)
        private byte SPh
        {
            get { return (byte)(SP >> 8); }
            set { SP = (ushort)((value << 8) + SPl); }
        }

        // Lower 8 bit part of SP register (internal use only, useful to move data from 8 bit data bus)
        private byte SPl
        {
            get { return (byte)(SP & 0xFF); }
            set { SP = (ushort)((SPh << 8) + value); }
        }

        /// <summary>
        /// Index Register 1 (IX)
        /// 
        /// The two independent index registers hold a 16-bit base address that is used
        /// in indexed addressing modes. In this mode, an index register is used as a
        /// base to point to a region in memory from which data is to be stored or
        /// retrieved. An additional byte is included in indexed instructions to specify a
        /// displacement from this base. This displacement is specified as a two's
        /// complement signed integer. This mode of addressing greatly simplifies
        /// many types of programs, especially where tables of data are used.
        /// </summary>
        public ushort IX { get; private set; }

        /// <summary>
        /// Higher 8 bit part of IX register (used by undocumented instructions)
        /// </summary>
        public byte IXh
        {
            get { return (byte)(IX >> 8); }
            private set { IX = (ushort)((value << 8) + IXl); }
        }

        /// <summary>
        /// Lower 8 bit part of IX register (used by undocumented instructions)
        /// </summary>
        public byte IXl
        {
            get { return (byte)(IX & 0xFF); }
            private set { IX = (ushort)((IXh << 8) + value); }
        }

        /// <summary>
        /// Index Register 2 (IY)
        /// 
        /// The two independent index registers hold a 16-bit base address that is used
        /// in indexed addressing modes. In this mode, an index register is used as a
        /// base to point to a region in memory from which data is to be stored or
        /// retrieved. An additional byte is included in indexed instructions to specify a
        /// displacement from this base. This displacement is specified as a two's
        /// complement signed integer. This mode of addressing greatly simplifies
        /// many types of programs, especially where tables of data are used.
        /// </summary>
        public ushort IY { get; private set; }

        /// <summary>
        /// Higher 8 bit part of IY register (used by undocumented instructions)
        /// </summary>
        public byte IYh
        {
            get { return (byte)(IY >> 8); }
            private set { IY = (ushort)((value << 8) + IYl); }
        }

        /// <summary>
        /// Lower 8 bit part of IY register (used by undocumented instructions)
        /// </summary>
        public byte IYl
        {
            get { return (byte)(IY & 0xFF); }
            private set { IY = (ushort)((IYh << 8) + value); }
        }       

        #endregion
        
        #region Temporary Storage Registers

        // MEMPTR behavior is documented here :
        // http://zx.pk.ru/attachment.php?attachmentid=2989

        // Additional information from :
        // http://www.worldofspectrum.org/forums
        // 14-04-2006 #33 boo_boo 
        // EXX was deeply tested, as well as EX AF,AF' -- these doesn't affect MEMPTR value. 
        // it seems to me, that "WZ" regpair (as described in Rodnay Zaks' book) doesn't exist at all, 
        // and was used by Zaks just to illustrate common principles of 8bit cpu design.  
        // 20-04-2008 #34 Woody 
        // Yep, checked that a few days ago myself on an NEC chip. There is no alternate copy of WZ (MEMPTR). 

        /// <summary>
        /// Temporary storage register 1 (W)
        /// 
        /// Special register available to the control unit within the Z80 (but not to the programmer).
        /// Required as a temporary storage location to implement some instructions.
        /// </summary>
        private byte W { get; set; }

        /// <summary>
        /// Temporary storage register 2 (Z)
        /// 
        /// Special register available to the control unit within the Z80 (but not to the programmer).
        /// Required as a temporary storage location to implement some instructions.
        /// </summary>
        private byte Z { get; set; }

        /// <summary>
        /// Access the temporary storage registers W and Z as a single 16 bit value
        /// </summary>
        private ushort WZ
        {
            get { return (ushort)((W << 8) + Z); }
            set { W = (byte)(value >> 8); Z = (byte)(value & 0xFF); }
        }

        #endregion

        #region Micro-instructions

        private void SwitchAlternateRegisterSet()
        {
            activeRegistersSet = 1 - activeRegistersSet;        
            
            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.Register16SwitchAlternateRegistersSet));
            }
        }

        private void SwitchRegister16(Register16 firstRegister)
        {
            if (firstRegister == Register16.AF)
            {
                SwitchALUAlternateRegisterSet();
            }
            else // if (firstRegister == Register16.DE)
            {
                // Here we don't implement a real switch with table index arithmetic for performance reasons
                // -> just swap the two values in memory
                ushort temp = DE;
                DE = HL;
                HL = temp;
            }

            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.Register16SwitchTwoRegisters, firstRegister));
            }
        }

        private void Register16_Increment(Register16 register)
        {
            switch (register)
            {
                case Register16.BC:
                    BC = (ushort)(BC + 1);
                    break;
                case Register16.DE:
                    DE = (ushort)(DE + 1);
                    break;
                case Register16.HL:
                    HL = (ushort)(HL + 1);
                    break;
                case Register16.SP:
                    SP = (ushort)(SP + 1);
                    break;
                case Register16.IX:
                    IX = (ushort)(IX + 1);
                    break;
                case Register16.IY:
                    IY = (ushort)(IY + 1);
                    break;
                default:
                    throw new NotImplementedException();
            }
            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.Register16Increment, register));
            }
        }

        private void Register16_Increment(InternalAddressBusConnection register)
        {
            switch (register)
            {
                case InternalAddressBusConnection.PC:
                    PC = (ushort)(PC + 1);
                    break;
                case InternalAddressBusConnection.SP:
                    SP = (ushort)(SP + 1);
                    break;
                case InternalAddressBusConnection.WZ:
                    WZ = (ushort)(WZ + 1);
                    break;
                default:
                    throw new NotImplementedException();
            }
            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.Register16Increment, register));
            }
        }

        private void Register16_Decrement(Register16 register)
        {
            switch (register)
            {
                case Register16.BC:
                    BC = (ushort)(BC - 1);
                    break;
                case Register16.DE:
                    DE = (ushort)(DE - 1);
                    break;
                case Register16.HL:
                    HL = (ushort)(HL - 1);
                    break;
                case Register16.SP:
                    SP = (ushort)(SP - 1);
                    break;
                case Register16.IX:
                    IX = (ushort)(IX - 1);
                    break;
                case Register16.IY:
                    IY = (ushort)(IY - 1);
                    break;
                default:
                    throw new NotImplementedException();
            }
            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.Register16Decrement, register));
            }
        }

        private void Register16_Decrement(InternalAddressBusConnection register)
        {
            switch (register)
            {
                case InternalAddressBusConnection.PC:
                    PC = (ushort)(PC - 1);
                    break;
                case InternalAddressBusConnection.WZ:
                    WZ = (ushort)(WZ - 1);
                    break;
                default:
                    throw new NotImplementedException();
            }
            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.Register16Decrement, register));
            }
        }

        private void Register_Decrement(Register register)
        {
            switch (register)
            {
                case Register.B:
                    B = (byte)(B - 1);
                    break;
                default:
                    throw new NotImplementedException();
            }
            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.RegisterDecrement, register));
            }
        }

        private bool RegisterB_CheckZero()
        {
            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.RegisterBCheckZero));
            }
            return B == 0;
        }

        private bool RegisterBC_CheckZero()
        {
            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.RegisterBCCheckZero));
            }
            return BC == 0;
        }

        private void RegisterWZ_Reset(AddressModifiedPageZero resetAddress)
        {
            switch(resetAddress)
            {
                case AddressModifiedPageZero.rst00:
                    WZ = 0x00;
                    break;
                case AddressModifiedPageZero.rst08:
                    WZ = 0x08;
                    break;
                case AddressModifiedPageZero.rst10:
                    WZ = 0x10;
                    break;
                case AddressModifiedPageZero.rst18:
                    WZ = 0x18;
                    break;
                case AddressModifiedPageZero.rst20:
                    WZ = 0x20;
                    break;
                case AddressModifiedPageZero.rst28:
                    WZ = 0x28;
                    break;
                case AddressModifiedPageZero.rst30:
                    WZ = 0x30;
                    break;
                case AddressModifiedPageZero.rst38:
                    WZ = 0x38;
                    break;
            }
            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.RegisterWZReset, resetAddress));
            }
        }

        private void RegisterWZ_Reset0066H()
        {
            WZ = 0x66;
            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.RegisterWZReset, "nmi66"));
            }
        }

        private void RegisterPC_RepeatInstruction()
        {
            if (instructionOrigin.Source == InstructionSource.Memory)
            {
                PC -= (ushort)(instructionOrigin.OpCode.OpCodeByteCount + instructionOrigin.OpCode.OperandsByteCount);
            }
            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.RegisterPCRepeatInstruction));
            }
        }

        #endregion
    }
}
