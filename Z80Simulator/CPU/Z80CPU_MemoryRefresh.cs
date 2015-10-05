using Z80Simulator.Instructions;
using Z80Simulator.System;

namespace Z80Simulator.CPU
{
    /// <summary>
    /// ZiLOG Z80 CPU
    /// 
    /// Memory refresh
    /// 
    /// </summary>
    public partial class Z80CPU
    {
        #region Memory refresh

        /// <summary>
        /// Memory Refresh Register (R)
        /// 
        /// The Z80 CPU contains a memory refresh counter, enabling dynamic
        /// memories to be used with the same ease as static memories. Seven bits of
        /// this 8-bit register are automatically incremented after each instruction fetch.
        /// The eighth bit remains as programmed, resulting from an LD R, A
        /// instruction. The data in the refresh counter is sent out on the lower portion of
        /// the address bus along with a refresh control signal while the CPU is
        /// decoding and executing the fetched instruction. This mode of refresh is
        /// transparent to the programmer and does not slow the CPU operation. The
        /// programmer can load the R register for testing purposes, but this register is
        /// normally not used by the programmer. During refresh, the contents of the I
        /// register are placed on the upper eight bits of the address bus.         
        /// </summary>
        public byte R { get; private set; }

        /// <summary>
        /// The R register is increased at every first machine cycle (M1). Bit 7 of the register is never
        /// changed by this; only the lower 7 bits are included in the addition. So bit 7 stays the same, but
        /// it can be changed using the LD R,A instruction.            
        /// </summary>
        private void RegisterR_Increment()
        {
            byte lower7Bits = (byte)(R & 0x7F);
            byte bit7 = (byte)(R & 0x80);
            R = (byte)(bit7 + (lower7Bits < 127 ? lower7Bits + 1 : 0));
            if (TraceMicroInstructions)
            {
                TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.RegisterRIncrement));
            }
        }

        #endregion
    }
}
