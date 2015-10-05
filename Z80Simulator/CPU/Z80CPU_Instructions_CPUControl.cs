using Z80Simulator.Instructions;

namespace Z80Simulator.CPU
{
    /// <summary>
    /// ZiLOG Z80 CPU
    /// 
    /// Instructions - CPU Control Group
    /// </summary>
    public partial class Z80CPU
    {
        /// <summary>
        /// OpCodeName = "DI",
        /// Equation = "IFF <- 0",
        /// UserManualPage = "174",
        /// Description = "DI disables the maskable interrupt by resetting the interrupt enable flipflops\n(IFF1 and IFF2). Note that this instruction disables the maskable\ninterrupt during its execution",
        /// ConditionBitsAffected = "None",
        /// Example = "When the CPU executes the instruction DI the maskable interrupt is\ndisabled until it is subsequently re-enabled by an EI instruction. The CPU\ndoes not respond to an Interrupt Request (INT) signal."
        /// </summary>
        private void Instruction_35_DI(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                CPUControl_ResetIFFs();
            }
        }

        /// <summary>
        /// OpCodeName = "EI",
        /// Equation = "IFF <- 1",
        /// UserManualPage = "175",
        /// Description = "The enable interrupt instruction sets both interrupt enable flip flops (IFFI\nand IFF2) to a logic 1, allowing recognition of any maskable interrupt. Note\nthat during the execution of this instruction and the following instruction,\nmaskable interrupts are disabled.",
        /// ConditionBitsAffected = "None",
        /// Example = "When the CPU executes instruction EI the maskable interrupt is\nenabled."
        /// </summary>
        private void Instruction_37_EI(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        {
            // MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                CPUControl_SetIFFs();
            }
        }

        private void Instruction_41_HALT(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        {
            // MachineCycleType.OCF, TStates = 4 
            if (halfTStateIndex == 7)
            {                
                CPUControl_EnterOrExitHaltMode();
            }
        }

        /// <summary>
        /// OpCodeName = "IM",
        /// Param1Type = AddressingMode.InterruptMode,
        /// UserManualPage = "176\n177\n178",
        /// Description = "The IM 0 instruction sets interrupt mode 0. In this mode, the interrupting\ndevice can insert any instruction on the data bus for execution by the\nCPU. The first byte of a multi-byte instruction is read during the interrupt\nacknowledge cycle. Subsequent bytes are read in by a normal memory\nread sequence.\nThe IM 1 instruction sets interrupt mode 1. In this mode, the processor\nresponds to an interrupt by executing a restart to location 0038H.\nThe IM 2 instruction sets the vectored interrupt mode 2. This mode allows\nan indirect call to any memory location by an 8-bit vector supplied from the\nperipheral device. This vector then becomes the least-significant eight bits\nof the indirect pointer, while the I register in the CPU provides the most-significant\neight bits. This address points to an address in a vector table that\nis the starting address for the interrupt service routine.",
        /// ConditionBitsAffected = "None"
        /// </summary>
        private void Instruction_42_IM_InterruptMode(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        {
            // Type = MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                CPUControl_SetInterruptMode((InterruptMode)instructionOrigin.OpCode.Param1);                
            }
        }
        
        /// <summary>
        /// OpCodeName = "NOP",
        /// UserManualPage = "172",
        /// Description = "The CPU performs no operation during this machine cycle.",
        /// ConditionBitsAffected = "None"
        /// </summary>
        private void Instruction_78_NOP(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            // ... do nothing ...
        }

        /// <summary>
        /// OpCodeName = "NMI",
        /// Description = "It takes 11 clock cycles to get to #0066: \nM1 cycle: 5 T states to do an opcode read and decrement SP \nM2 cycle: 3 T states write high byte of PC to the stack and decrement SP \nM3 cycle: 3 T states write the low byte of PC and jump to #0066. ",
        /// Example = "If the Accumulator contains 96H (1001 0110), at execution of XOR 5DH\n(5DH = 0101 1101) the Accumulator contains CBH (1100 1011)."
        /// </summary>
        private void Instruction_157_NMI(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 5
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 9)
            {
                // When a non-maskable interrupt is accepted, IFF1 resets to prevent further interrupts until reenabled by the programmer. 
                // Thus, after a non-maskable interrupt is accepted, maskable interrupts are disabled but the previous state of IFF1 is still
                // present in IFF2, so that the complete state of the CPU just prior to the non-maskable interrupt can be restored at any time. 
                CPUControl_ResetIFF1();
            }
            // MachineCycleType.SWH, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom(InternalDataBusConnection.PCh);
            }
            // MachineCycleType.SWL, TStates = 3
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom(InternalDataBusConnection.PCl);
            }
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 5)
            {
                // NMI automatically forces the CPU to restart at location 0066H.
                RegisterWZ_Reset0066H();
                InternalAddressBus_SampleFrom(InternalAddressBusConnection.WZ);
                InternalAddressBus_SendTo(InternalAddressBusConnection.PC);
            }
        }

        /// <summary>
        /// OpCodeName = "INT 0",
        /// Description = "In this mode, timing depends on the instruction put on the bus. The interrupt processing last 2 clock cycles more than this instruction usually needs.\nTwo typical examples follow:\n- a RST n on the data bus, it takes 13 cycles to get to 'n': \nM1 cycle: 7 ticks\nacknowledge interrupt and decrement SP \nM2 cycle: 3 ticks\nwrite high byte and decrement SP \nM3 cycle: 3 ticks\nwrite low byte and jump to 'n' \n- With a CALL nnnn on the data bus, it takes 19 cycles: \nM1 cycle: 7 ticks\nacknowledge interrupt \nM2 cycle: 3 ticks\nread low byte of 'nnnn' from data bus \nM3 cycle: 3 ticks\nread high byte of 'nnnn' and decrement SP \nM4 cycle: 3 ticks\nwrite high byte of PC to the stack and decrement SP \nM5 cycle: 3 ticks\nwrite low byte of PC and jump to 'nnnn'. "
        /// </summary>
        private void Instruction_158_INT_0(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.INTA, TStates = 7
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 8)
            {
                DecodeInterruptingDeviceInstruction();
            }
            // --- after halfTState 8, decoded instruction replaces INT 0 --
        }

        /// <summary>
        /// OpCodeName = "INT 1",
        /// Description = "It takes 13 clock cycles to reach #0038: \nM1 cycle: 7 ticks\nacknowledge interrupt and decrement SP \nM2 cycle: 3 ticks\nwrite high byte of PC onto the stack and decrement SP \nM3 cycle: 3 ticks\nwrite low byte onto the stack and to set PC to #0038. "
        /// </summary>
        private void Instruction_159_INT_1(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        {
            // MachineCycleType.INTA, TStates = 7
            // MachineCycleType.SWH, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom(InternalDataBusConnection.PCh);
            }
            // MachineCycleType.SWL, TStates = 3
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom(InternalDataBusConnection.PCl);
            }
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 5)
            {
                RegisterWZ_Reset(AddressModifiedPageZero.rst38);
                InternalAddressBus_SampleFrom(InternalAddressBusConnection.WZ);
                InternalAddressBus_SendTo(InternalAddressBusConnection.PC);
            }
        }

        /// <summary>
        /// OpCodeName = "INT 2",
        /// Description = "It takes 19 clock cycles to get to the interrupt routine: \nM1 cycle: 7 ticks\nacknowledge interrupt and decrement SP \nM2 cycle: 3 ticks\nwrite high byte of PC onto stack and decrement SP \nM3 cycle: 3 ticks\nwrite low byte onto the stack \nM4 cycle: 3 ticks\nread low byte from the interrupt vector \nM5 cycle: 3 ticks\nread high byte from bus and jump to interrupt routine "
        /// </summary>
        private void Instruction_160_INT_2(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.INTA, TStates = 7
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 8)
            {
                // The lower eight bits of the pointer must be supplied by the interrupting device on the data bus
                InternalDataBus_SendTo(InternalDataBusConnection.Z);
                // The upper eight bits of this pointer is formed from the contents of the I register
                InternalDataBus_SampleFrom(Register.I);
                InternalDataBus_SendTo(InternalDataBusConnection.W);
            }
            // MachineCycleType.SWH, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom(InternalDataBusConnection.PCh);
            }
            // MachineCycleType.SWL, TStates = 3
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom(InternalDataBusConnection.PCl);
            }
            // MachineCycleType.MRL, TStates = 3
            if (machineCycleCountAfterInstruction == 4 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFromAndIncrement(InternalAddressBusConnection.WZ);
            }
            if (machineCycleCountAfterInstruction == 4 && halfTStateIndex == 5)
            {
                // The first byte in the table is the least-significant (low order portion of the address)
                InternalDataBus_SendTo(InternalDataBusConnection.PCl);
            }
            // MachineCycleType.MRH, TStates = 3
            if (machineCycleCountAfterInstruction == 5 && halfTStateIndex == 0)
            {
                InternalAddressBus_SampleFrom(InternalAddressBusConnection.WZ);
            }
            if (machineCycleCountAfterInstruction == 5 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.PCh);
            }
        }

        /// <summary>
        /// RESET initializes the CPU as follows: it resets
        /// the interrupt enable flip-flop, clears the PC and registers I and R, and sets the
        /// interrupt status to Mode 0. AF and SP are always set to FFFF after a reset, and
        /// all other registers are undefined (different depending on how long the CPU has
        /// been powered off, different for different Z80 chips). During reset time, the address 
        /// and data bus go to a high-impedance state, and all control output signals go to the 
        /// inactive state. Notice that RESET must be active for a minimum of three full clock
        /// cycles before the reset operation is complete.
        /// </summary>
        private void Instruction_161_RESET(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        {
            // MachineCycleType.CPU, TStates = 3
            if (halfTStateIndex == 0)
            {
                CPUControl_EnterResetMode();
            }
            if (halfTStateIndex == 5)
            {
                CPUControl_ExitResetMode();
            }
        }

        /// <summary>
        /// This generic instruction is executed by the CPU during the opcode fetch phase.
        /// It is then replaced by the instruction that was decoded from the fetched opcode 
        /// during the last OCF machine cycle.
        /// </summary>
        private void Instruction_162_FetchNewInstruction(byte machineCycleCountAfterInstruction, byte halfTStateIndex)
        {
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.OD, TStates = 3
            if (machineCycleIndex == 2 && halfTStateIndex == 5)
            {
                // Store the third byte (displacement) into register W while decoding a four bytes opcode
                InternalDataBus_SendTo(InternalDataBusConnection.W);
            }
            // MachineCycleType.OC4, TStates = 5
            // -> here, DecodeInstruction() will be called and the real instruction method will be set
        }
    }
}