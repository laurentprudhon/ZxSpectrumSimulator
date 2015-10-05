using Z80Simulator.Instructions;

namespace Z80Simulator.CPU
{
    /// <summary>
    /// ZiLOG Z80 CPU
    /// 
    /// Instructions - Call And Return Group
    /// </summary>
    public partial class Z80CPU
    {
        /// <summary>
        /// OpCodeName = "CALL",
        /// Param1Type = AddressingMode.Extended,
        /// Equation = "(SP-1) <- PCH, (SP-2) <- PCL, PC <- nn",
        /// Param1List = "nn"
        /// UserManualPage = "255",
        /// Description = "The current contents of the Program Counter (PC) are pushed onto the top\nof the external memory stack. The operands nn are then loaded to the PC to\npoint to the address in memory where the first Op Code of a subroutine is to\nbe fetched. At the end of the subroutine, a RETurn instruction can be used\nto return to the original program flow by popping the top of the stack back\nto the PC. The push is accomplished by first decrementing the current\ncontents of the Stack Pointer (register pair SP), loading the high-order byte\nof the PC contents to the memory address now pointed to by the SP, then\ndecrementing SP again, and loading the low order byte of the PC contents\nto the top of stack.\nBecause this is a 3-byte instruction, the Program Counter was incremented\nby three before the push is executed.",
        /// ConditionBitsAffected = "None",
        /// Example = "If the contents of the Program Counter are 1A47H, the contents of the Stack\nPointer are 3002H, and memory locations have the contents:\n1A47H contains CDH\nIA48H contains 35H\n1A49H contains 21H\nIf an instruction fetch sequence begins, the 3-byte instruction CD3521H is\nfetched to the CPU for execution. The mnemonic equivalent of this is CALL\n2135H. At execution of this instruction, the contents of memory address\n3001H is 1AH, the contents of address 3000H is 4AH, the contents of the\nStack Pointer is 3000H, and the contents of the Program Counter is 2135H,\npointing to the address of the first Op Code of the subroutine now to be\nexecuted."
        /// </summary>
        private void Instruction_18_CALL_Address(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.ODL, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.Z);
            }
            // MachineCycleType.ODH, TStates = 4
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 7)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.W);
            }
            // MachineCycleType.SWH, TStates = 3
            if (machineCycleCountAfterInstruction == 4 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom(InternalDataBusConnection.PCh);
            }
            // MachineCycleType.SWL, TStates = 3
            if (machineCycleCountAfterInstruction == 5 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom(InternalDataBusConnection.PCl);
            }
            if (machineCycleCountAfterInstruction == 5 && halfTStateIndex == 5)            
            {
                InternalAddressBus_SampleFrom(InternalAddressBusConnection.WZ);
                InternalAddressBus_SendTo(InternalAddressBusConnection.PC);
            }
        }

        /// <summary>
        /// OpCodeName = "CALL",
        /// Param1Type = AddressingMode.FlagCondition,
        /// Param2Type = AddressingMode.Extended,
        /// Equation = "IF cc true: (sp-1) <- PCH\n(sp-2) <- PCL, pc <- nn",
        /// Param1List = "C","M","NC","NZ","P","PE","PO","Z"                   
        /// Param2List = "nn"
        /// UserManualPage = "257",
        /// Description = "If condition cc is true, this instruction pushes the current contents of the\nProgram Counter (PC) onto the top of the external memory stack, then\nloads the operands nn to PC to point to the address in memory where the\nfirst Op Code of a subroutine is to be fetched. At the end of the subroutine,\na RETurn instruction can be used to return to the original program flow by\npopping the top of the stack back to PC. If condition cc is false, the\nProgram Counter is incremented as usual, and the program continues with\nthe next sequential instruction. The stack push is accomplished by first\ndecrementing the current contents of the Stack Pointer (SP), loading the\nhigh-order byte of the PC contents to the memory address now pointed to\nby SP, then decrementing SP again, and loading the low order byte of the\nPC contents to the top of the stack.\nBecause this is a 3-byte instruction, the Program Counter was incremented\nby three before the push is executed.\nCondition cc is programmed as one of eight status that corresponds to\ncondition bits in the Flag Register (register F). These eight status are\ndefined in the table below :\nCondition Flag\nNZ non zero Z\nZ zero Z\nNC non carry C\nC carry Z\nPO parity odd P/V\nPE parity even P/V\nP sign positive S\nM sign negative S",
        /// ConditionBitsAffected = "None",
        /// Example = "are 1A47H, the contents of the Stack Pointer are 3002H, and memory\nlocations have the contents:\nLocation Contents\n1A47H D4H\n1448H 35H\n1A49H 21H\nthen if an instruction fetch sequence begins, the 3-byte instruction\nD43521H is fetched to the CPU for execution. The mnemonic equivalent of\nthis is CALL NC, 2135H. At execution of this instruction, the contents of\nmemory address 3001H is 1AH, the contents of address 3000H is 4AH, the\ncontents of the Stack Pointer is 3000H, and the contents of the Program\nCounter is 2135H, pointing to the address of the first Op Code of the\nsubroutine now to be executed."    
        /// </summary>
        private void Instruction_19_CALL_FlagCondition_Address(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        {             
            // MachineCycleType.OCF, TStates = 4
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 7)
            {
                SelectAlternateExecutionTimings_IfFlagIsNotSet((FlagCondition)instructionOrigin.OpCode.Param1);
            }
            // MachineCycleType.ODL, TStates = 3 
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.Z);
            }
            // -- Condition = "cc == 0",
            // MachineCycleType.ODH, TStates = 3
            // -- Condition = "cc == 1",            
            // MachineCycleType.ODH, TStates = 4
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.W);
            }
            // MachineCycleType.SWH, TStates = 3
            if (machineCycleCountAfterInstruction == 4 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom(InternalDataBusConnection.PCh);
            }
            // MachineCycleType.SWL, TStates = 3
            if (machineCycleCountAfterInstruction == 5 && halfTStateIndex == 0)
            {
                InternalDataBus_SampleFrom(InternalDataBusConnection.PCl);
            }
            if (machineCycleCountAfterInstruction == 5 && halfTStateIndex == 5)
            {
                InternalAddressBus_SampleFrom(InternalAddressBusConnection.WZ);
                InternalAddressBus_SendTo(InternalAddressBusConnection.PC);
            }
        }        

        /// <summary>
        /// OpCodeName = "RET",
        /// Equation = "pCL <- (sp), pCH <- (sp+1)",
        /// UserManualPage = "260",
        /// Description = "The byte at the memory location specified by the contents of the Stack\nPointer (SP) register pair is moved to the low order eight bits of the\nProgram Counter (PC). The SP is now incremented and the byte at the\nmemory location specified by the new contents of this instruction is fetched\nfrom the memory location specified by the PC. This instruction is normally\nused to return to the main line program at the completion of a routine\nentered by a CALL instruction.",
        /// ConditionBitsAffected = "None",
        /// Example = "If the contents of the Program Counter are 3535H, the contents of the Stack\nPointer are 2000H, the contents of memory location 2000H are B5H, and\nthe contents of memory location of memory location 2001H are 18H. At\nexecution of RET the contents of the Stack Pointer is 2002H, and the\ncontents of the Program Counter is 18B5H, pointing to the address of the\nnext program Op Code to be fetched."     
        /// </summary>
        private void Instruction_96_RET(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.SRL, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.Z);
            }
            // MachineCycleType.SRH, TStates = 3
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.W);
                InternalAddressBus_SampleFrom(InternalAddressBusConnection.WZ);
                InternalAddressBus_SendTo(InternalAddressBusConnection.PC);
            }
        }
        
        /// <summary>
        /// OpCodeName = "RET",
        /// Param1Type = AddressingMode.FlagCondition,
        /// Equation = "If cc true: PCL <- (sp), pCH <- (sp+1)",
        /// Param1List = "C","M","NC","NZ","P","PE","PO","Z",
        /// UserManualPage = "261",
        /// Description = "If condition cc is true, the byte at the memory location specified by the\ncontents of the Stack Pointer (SP) register pair is moved to the low order\neight bits of the Program Counter (PC). The SP is incremented and the byte\nat the memory location specified by the new contents of the SP are moved\nto the high order eight bits of the PC. The SP is incremented again. The\nnext Op Code following this instruction is fetched from the memory\nlocation specified by the PC. This instruction is normally used to return to\nthe main line program at the completion of a routine entered by a CALL\ninstruction. If condition cc is false, the PC is simply incremented as usual,\nand the program continues with the next sequential instruction. Condition\ncc is programmed as one of eight status that correspond to condition bits in\nthe Flag Register (register F). These eight status are defined in the table below :\nCondition Flag\nNZ non zero Z\nZ zero Z\nNC no carry C\nC carry C\nPO parity odd P/V\nPE parity even P/V\nP sign positive S\nM sign negative S",
        /// ConditionBitsAffected = "None",
        /// Example = "If the S flag in the F register is set, the contents of the Program Counter are\n3535H, the contents of the Stack Pointer are 2000H, the contents of\nmemory location 2000H are B5H, and the contents of memory location\n2001H are 18H. At execution of RET M the contents of the Stack Pointer\nis 2002H, and the contents of the Program Counter is 18B5H, pointing to\nthe address of the next program Op Code to be fetched."           
        /// </summary>
        private void Instruction_97_RET_FlagCondition(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 

            // MachineCycleType.OCF, TStates = 5
            if (machineCycleCountAfterInstruction == 1 && halfTStateIndex == 9)
            {
                SelectAlternateExecutionTimings_IfFlagIsNotSet((FlagCondition)instructionOrigin.OpCode.Param1);
            }
            // -- Condition = "cc == 1",
            // MachineCycleType.SRL, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.Z);
            }
            // MachineCycleType.SRH, TStates = 3
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.W);
                InternalAddressBus_SampleFrom(InternalAddressBusConnection.WZ);
                InternalAddressBus_SendTo(InternalAddressBusConnection.PC);
            }
        }

        /// <summary>
        /// OpCodeName = "RETI",
        /// UserManualPage = "263",
        /// Description = "This instruction is used at the end of a maskable interrupt service routine to:\n- Restore the contents of the Program Counter (PC) (analogous to the\nRET instruction)\n- Signal an I/O device that the interrupt routine is completed. The RETI\ninstruction also facilitates the nesting of interrupts, allowing higher\npriority devices to temporarily suspend service of lower priority\nservice routines. However, this instruction does not enable interrupts\nthat were disabled when the interrupt routine was entered. Before\ndoing the RETI instruction, the enable interrupt instruction (EI)\nshould be executed to allow recognition of interrupts after completion\nof the current service routine.",
        /// ConditionBitsAffected = "None",
        /// Example = "Given: Two interrupting devices, with A and B connected in a daisy-chain\nconfiguration and A having a higher priority than B.\nB generates an interrupt and is acknowledged. The interrupt\nenable out, IEO, of B goes Low, blocking any lower priority\ndevices from interrupting while B is being serviced. Then A generates\nan interrupt, suspending service of B. The IEO of A goes\nLow, indicating that a higher priority device is being serviced.\nThe A routine is completed and a RETI is issued resetting the IEO\nof A, allowing the B routine to continue. A second RETI is issued\non completion of the B routine and the IE0 of B is reset (high)\nallowing lower priority devices interrupt access."
        /// </summary>
        private void Instruction_98_RETI(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.SRL, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.Z);
            }
            // MachineCycleType.SRH, TStates = 3
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.W);
                InternalAddressBus_SampleFrom(InternalAddressBusConnection.WZ);
                InternalAddressBus_SendTo(InternalAddressBusConnection.PC);
            }
        }

        /// <summary>
        /// OpCodeName = "RETN",
        /// UserManualPage = "265",
        /// Description = "This instruction is used at the end of a non-maskable interrupts service\nroutine to restore the contents of the Program Counter (PC) (analogous to\nthe RET instruction). The state of IFF2 is copied back to IFF1 so that\nmaskable interrupts are enabled immediately following the RETN if they\nwere enabled before the nonmaskable interrupt.",
        /// ConditionBitsAffected = "None",
        /// Example = "If the contents of the Stack Pointer are 1000H, and the contents of the\nProgram Counter are 1A45H, when a non maskable interrupt (NMI) signal\nis received, the CPU ignores the next instruction and instead restarts to\nmemory address 0066H. The current Program Counter contents of 1A45H\nis pushed onto the external stack address of 0FFFH and 0FFEH, high orderbyte\nfirst, and 0066H is loaded onto the Program Counter. That address\nbegins an interrupt service routine that ends with a RETN instruction.\nUpon the execution of RETN the former Program Counter contents are\npopped off the external memory stack, low order first, resulting in a Stack\nPointer contents again of 1000H. The program flow continues where it\nleft off with an Op Code fetch to address 1A45H, order-byte first, and\n0066H is loaded onto the Program Counter. That address begins an\ninterrupt service routine that ends with a RETN instruction. At execution of\nRETN the former Program Counter contents are popped off the external\nmemory stack, low order first, resulting in a Stack Pointer contents again of\n1000H. The program flow continues where it left off with an Op Code fetch\nto address 1A45H."
        /// </summary>
        private void Instruction_99_RETN(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        { 
            // MachineCycleType.OCF, TStates = 4
            // MachineCycleType.SRL, TStates = 3
            if (machineCycleCountAfterInstruction == 2 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.Z);
            }
            // MachineCycleType.SRH, TStates = 3
            if (machineCycleCountAfterInstruction == 3 && halfTStateIndex == 5)
            {
                InternalDataBus_SendTo(InternalDataBusConnection.W);
                InternalAddressBus_SampleFrom(InternalAddressBusConnection.WZ);
                InternalAddressBus_SendTo(InternalAddressBusConnection.PC);
                CPUControl_CopyIFF2ToIFF1();
            }
        }
        
        /// <summary>
        /// OpCodeName = "RST",
        /// Param1Type = AddressingMode.ModifiedPageZero,
        /// Equation = "(SP-1) <- PCH, (SP-2) <- PCL, PCH <- 0, PCL <- P",
        /// Param1List = "0H","8H","10H","18H","20H","28H","30H","38H"
        /// UserManualPage = "267",
        /// Description = "The current Program Counter (PC) contents are pushed onto the external\nmemory stack, and the page zero memory location given by operand p is\nloaded to the PC. Program execution then begins with the Op Code in the\naddress now pointed to by PC. The push is performed by first decrementing\nthe contents of the Stack Pointer (SP), loading the high-order byte of PC to\nthe memory address now pointed to by SP, decrementing SP again, and\nloading the low order byte of PC to the address now pointed to by SP. The\nRestart instruction allows for a jump to one of eight addresses indicated in\nthe table below. The operand p is assembled to the object code using the\ncorresponding T state.\nBecause all addresses are in page zero of memory, the high order byte of\nPC is loaded with 00H. The number selected from the p column of the table\nis loaded to the low order byte of PC.\np\n00H\n08H\n10H\n18H\n20H\n28H\n30H\n38H",
        /// ConditionBitsAffected = "None",
        /// Example = "If the contents of the Program Counter are 15B3H, at execution of\nRST 18H (Object code 1101111) the PC contains 0018H, as the address\nof the next Op Code fetched." 
        /// </summary>
        private void Instruction_122_RST_ResetAddress(byte machineCycleCountAfterInstruction, byte halfTStateIndex) 
        {
            // MachineCycleType.OCF, TStates = 5
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
                RegisterWZ_Reset((AddressModifiedPageZero)instructionOrigin.OpCode.Param1);
                InternalAddressBus_SampleFrom(InternalAddressBusConnection.WZ);
                InternalAddressBus_SendTo(InternalAddressBusConnection.PC);
            }
        }
    }
}