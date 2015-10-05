
namespace Z80Simulator.Instructions
{
    public static class Z80MicroInstructionTypes
    {
        public static MicroInstructionType InternalDataBusSampleFrom =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.DataPath,
                Name = "InternalDataBus.SampleFrom",
                Description = "Send source register value to the internal data bus",
                ParamNames = new string[] { "Source" },
                ParamDescriptions = new string[] { "Source register name" }
            };

        public static MicroInstructionType InternalDataBusSendTo =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.DataPath,
                Name = "InternalDataBus.SendTo",
                Description = "Send internal data bus value to the target register (or external bus)",
                ParamNames = new string[] { "Target" },
                ParamDescriptions = new string[] { "Target register name (or external bus)" }
            };

        public static MicroInstructionType InternalDataBusReset =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.DataPath,
                Name = "InternalDataBus.Reset",
                Description = "Resets the value of the internal data bus"
            };

        public static MicroInstructionType InternalAddressBusSampleFrom =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.DataPath,
                Name = "InternalAddresBus.SampleFrom",
                Description = "Send source register value to the internal address bus",
                ParamNames = new string[] { "Source" },
                ParamDescriptions = new string[] { "Source register name" }
            };

        public static MicroInstructionType InternalAddressBusSampleFromRegisterPlusDisplacement =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.DataPath,
                Name = "InternalAddressBus.SampleFromRegisterPlusDisplacement",
                Description = "Send source register value + internal data bus value to the internal address bus",
                ParamNames = new string[] { "Source" },
                ParamDescriptions = new string[] { "Source register name" }
            };

        public static MicroInstructionType InternalAddressBusSendTo =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.DataPath,
                Name = "InternalAddressBus.SendTo",
                Description = "Send internal address bus value to the target register (or external bus)",
                ParamNames = new string[] { "Target" },
                ParamDescriptions = new string[] { "Target register name (or external bus)" }
            };

        public static MicroInstructionType SendOperandToPCIfFlagIsSet =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.DataPath,
                Name = "SendOperandToPCIfFlagIsSet",
                Description = "Send internal data bus to PCh and W to PCl if the flag condition is true",
                ParamNames = new string[] { "Flag" },
                ParamDescriptions = new string[] { "Flag condition" }
            };        

        public static MicroInstructionType ALULeftBufferSampleFrom =
             new MicroInstructionType
             {
                 Family = MicroInstructionFamily.DataPath,
                 Name = "ALULeftBuffer.SampleFrom",
                 Description = "Send source register value to the left buffer of the arithmetic unit",
                 ParamNames = new string[] { "Source" },
                 ParamDescriptions = new string[] { "Source register name" }
             };

        public static MicroInstructionType ALULeftBufferSendTo =
             new MicroInstructionType
             {
                 Family = MicroInstructionFamily.DataPath,
                 Name = "ALULeftBuffer.SendTo",
                 Description = "Send the left buffer of the arithmetic unit to the target register value",
                 ParamNames = new string[] { "Target" },
                 ParamDescriptions = new string[] { "Target register name" }
             };

        public static MicroInstructionType ALULeftBufferReset =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.DataPath,
                Name = "ALULeftBuffer.Reset",
                Description = "Reset ALULeftBuffer to value 0"
            };

        public static MicroInstructionType ArithmeticOperationAdd =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.ALU,
                Name = "ArithmeticOperation.Add",
                Description = "Add ALULeftBuffer to ALURightBuffer without input carry / with output carry and send result to the internal data bus",
                ParamNames = new string[] { "WithInputCarry", "16bitsZFlag", "WithSZPFlags" },
                ParamDescriptions = new string[] { "Count input carry ?", "Compute Z flag for a 16 bits result ?", "Update S,Z,P flags ?" }
            };

        public static MicroInstructionType ArithmeticOperationSub =
           new MicroInstructionType
           {
               Family = MicroInstructionFamily.ALU,
               Name = "ArithmeticOperation.Sub",
               Description = "Subtract ALURightBuffer from ALULeftBuffer and send result to the internal data bus",
               ParamNames = new string[] { "WithInputCarry", "16bitsZFlag" },
               ParamDescriptions = new string[] { "Count input carry ?", "Compute Z flag for a 16 bits result ?" }
           };

        public static MicroInstructionType ArithmeticOperationIncrement =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.ALU,
                Name = "ArithmeticOperation.Increment",
                Description = "Set ALULeftBuffer to value 1, then add ALULeftBuffer to ALURightBuffer without output carry and send result to the internal data bus"
            };

        public static MicroInstructionType ArithmeticOperationDecrement =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.ALU,
                Name = "ArithmeticOperation.Decrement",
                Description = "Set ALULeftBuffer to value 1, then subtract ALULeftBuffer from ALURightBuffer without output carry and send result to the internal data bus"
            };

        public static MicroInstructionType ArithmeticOperationCompare =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.ALU,
                Name = "ArithmeticOperation.Compare",
                Description = "Subtract ALURightBuffer from ALULeftBuffer with output carry and send result to the internal data bus"
            };


        public static MicroInstructionType ArithmeticOperationBlockCompare =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.ALU,
                Name = "ArithmeticOperation.BlockCompare",
                Description = "Subtract ALURightBuffer from ALULeftBuffer with output carry and send result to the internal data bus. The CPI/CPIR/CPD/CPDR instructions aﬀect the ﬂags in a complex way : see The Undocumented Z80 Documented section 4.2"
            };

        public static MicroInstructionType ArithmeticOperationDecimalAdjust =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.ALU,
                Name = "ArithmeticOperation.DecimalAdjust",
                Description = "Send accumulator value to ALULeftBuffer, compute the correction needed for a BCD arithmetic operation and send it to ALURightBuffer, then add or subtract ALULeftBuffer and ALURightBuffer with output carry and send result to the internal data bus"
            };

        public static MicroInstructionType FlagsOperationSetCarry =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.ALU,
                Name = "FlagsOperation.SetCarry",
                Description = "Set carry flag"
            };

        public static MicroInstructionType FlagsOperationInvertCarry =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.ALU,
                Name = "FlagsOperation.SetCarry",
                Description = "Invert carry flag"
            };        

        public static MicroInstructionType FlagsOperationInvertAccumulator =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.ALU,
                Name = "FlagsOperation.InvertAccumulator",
                Description = "Invert the accumulator bits, and update the flags accordingly"
            };        

        public static MicroInstructionType FlagsOperationComputeFlagsForLDAIandR =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.ALU,
                Name = "FlagsOperation.ComputeFlagsForLDAIandR",
                Description = "Special setting of the flags after LD A,I or LD A,R : copy IFF2 to P/V flag"
            };

        public static MicroInstructionType FlagsOperationComputeFlagsForMemBlockLoad =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.ALU,
                Name = "FlagsOperation.ComputeFlagsForMemBlockLoad",
                Description = "The LDI/LDIR/LDD/LDDR instructions aﬀect the ﬂags in a complex way : see The Undocumented Z80 Documented section 4.2"
            };

        public static MicroInstructionType FlagsOperationComputeFlagsForInput =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.ALU,
                Name = "FlagsOperation.ComputeFlagsForInput",
                Description = "C is not affected, H is reset, N is reset. Copy bits 3 and 5 from input data. S is set if input data is negative; reset otherwise. Z is set if input data is zero; reset otherwise. P/V is set if parity of input data is even; reset otherwise."
            };

        public static MicroInstructionType FlagsOperationComputeFlagsForIOBlockInstruction =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.ALU,
                Name = "FlagsOperation.ComputeFlagsForIOBlockInstruction",
                Description = "The OUTI/OTIR/OUTD/OTDR, INI/INIR, and IND/INDR instructions aﬀect the ﬂags in a complex way : see The Undocumented Z80 Documented section 4.3",
                ParamNames = new string[] { "InstructionType" },
                ParamDescriptions = new string[] { "IO block instruction type : OUT, INI, or IND" }
            };
        
        public static MicroInstructionType LogicalOperationAnd =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.ALU,
                Name = "LogicalOperation.And",
                Description = "Send accumulator value to ALULeftBuffer, then perform a logical AND operation on ALULeftBuffer / ALURightBuffer, and send result to the internal data bus"
            };

        public static MicroInstructionType LogicalOperationOr =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.ALU,
                Name = "LogicalOperation.Or",
                Description = "Send accumulator value to ALULeftBuffer, then perform a logical OR operation on ALULeftBuffer / ALURightBuffer, and send result to the internal data bus"
            };

        public static MicroInstructionType LogicalOperationXor =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.ALU,
                Name = "LogicalOperation.Xor",
                Description = "Send accumulator value to ALULeftBuffer, then perform a logical XOR operation on ALULeftBuffer / ALURightBuffer, and send result to the internal data bus"
            };

        public static MicroInstructionType OutputPinSetLow =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.IOPins,
                Name = "OutputPin.SetLow",
                Description = "Set output pin to LOW state",
                ParamNames = new string[] { "Pin" },
                ParamDescriptions = new string[] { "Output pin name" }
            };

        public static MicroInstructionType OutputPinSetHigh =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.IOPins,
                Name = "OutputPin.SetHigh",
                Description = "Set output pin to HIGH state",
                ParamNames = new string[] { "Pin" },
                ParamDescriptions = new string[] { "Output pin name" }
            };

        public static MicroInstructionType BusConnectorSetValue =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.IOPins,
                Name = "BusConnector.SetValue",
                Description = "Apply the value of the internal bus to the external bus lines through the bus connector buffer",
                ParamNames = new string[] { "Bus" },
                ParamDescriptions = new string[] { "Internal and External bus name" }
            };

        public static MicroInstructionType BusConnectorReleaseValue =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.IOPins,
                Name = "BusConnector.ReleaseValue",
                Description = "Disconnect from the external bus by setting the bus connector to a high-impedance state",
                ParamNames = new string[] { "Bus" },
                ParamDescriptions = new string[] { "External bus name" }
            };

        public static MicroInstructionType CPUControlAddOneTStateIfSignalLow =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.CPUControl,
                Name = "CPUControl.AddOneTStateIfSignalLow",
                Description = " Add one more TState to the machine cycle by decreasing the halfTState counter if the input pin signal is low",
                ParamNames = new string[] { "Input" },
                ParamDescriptions = new string[] { "Input pin name" }
            };

        public static MicroInstructionType CPUControlSelectAltExecTimingsIfFlagIsNotSet =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.CPUControl,
                Name = "CPUControl.SelectAltExecTimingsIfFlagIsNotSet",
                Description = "If flag condition (C,M,NC,NZ,P,PE,PO,Z) is false, select alternate execution timings for the current instruction",
                ParamNames = new string[] { "Flag" },
                ParamDescriptions = new string[] { "Flag condition" }
            };

        public static MicroInstructionType CPUControlSelectAltExecTimingsIfRegisterIsZero =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.CPUControl,
                Name = "CPUControl.SelectAltExecTimingsIfRegisterIsZero",
                Description = "If the tested register (B, BC, BC&ZF) = 0, select alternate execution timings for the current instruction",
                ParamNames = new string[] { "Register" },
                ParamDescriptions = new string[] { "Tested register (+ flag)" }
            };

        public static MicroInstructionType CPUControlDecodeInstruction =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.CPUControl,
                Name = "CPUControl.DecodeInterruptingDeviceInstruction",
                Description = "Decodes an instruction from an opcode read in memory and stored in the instruction register"
            };   

        public static MicroInstructionType CPUControlDecodeInterruptingDeviceInstruction =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.CPUControl,
                Name = "CPUControl.DecodeInterruptingDeviceInstruction",
                Description = "Decodes an instruction from an opcode written on the data bus by an interrupting device"
            };

        public static MicroInstructionType CPUControlResetRegistersState =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.CPUControl,
                Name = "CPUControl.ResetRegistersState",
                Description = "Resets the interrupt enable flip-flops, clears the PC and registers I and R, sets the interrupt status to Mode 0, AF and SP are set to FFFFh, all other registers are undefined"
            };          

        public static MicroInstructionType CPUControlEnterResetMode =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.CPUControl,
                Name = "CPUControl.EnterResetMode",
                Description = "RESET initializes the CPU as follows : clears the PC and the registers I and R, resets the interrupt enable flip-flops, sets the interrupt status to Mode 0, sets AF and SP to FFFFh"
            };

        public static MicroInstructionType CPUControlExitResetMode =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.CPUControl,
                Name = "CPUControl.ExitResetMode",
                Description = "During reset time, the address and data bus go to a high-impedance state, and all control output signals go to the inactive state"
            };

        public static MicroInstructionType CPUControlEnterOrExitHaltMode =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.CPUControl,
                Name = "CPUControl.EnterOrExitHaltMode",
                Description = "The HALT instruction halts the Z80 : the instruction is re-executed until an interrupt is received. During the HALT state, the HALT line is set."
            };

        public static MicroInstructionType CPUControlEnterDMAMode =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.CPUControl,
                Name = "CPUControl.EnterDMAMode",
                Description = "Sets all address bus, data bus, and tristate control signals to the high-impedance state"
            };

        public static MicroInstructionType CPUControlExitDMAMode =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.CPUControl,
                Name = "CPUControl.ExitDMAMode",
                Description = "Restores all tristate control signals to the inactive state"
            };

        public static MicroInstructionType CPUControlResetIFFs =
        new MicroInstructionType
        {
            Family = MicroInstructionFamily.CPUControl,
            Name = "CPUControl.ResetIFFs",
            Description = "Resets both interrupt flipflops"
        };

        public static MicroInstructionType CPUControlSetIFFs =
        new MicroInstructionType
        {
            Family = MicroInstructionFamily.CPUControl,
            Name = "CPUControl.SetIFFs",
            Description = "Sets both interrupt flipflops"
        };

        public static MicroInstructionType CPUControlResetIFF1 =
        new MicroInstructionType
        {
            Family = MicroInstructionFamily.CPUControl,
            Name = "CPUControl.ResetIFF1",
            Description = "Resets interrupt flipflop 1"
        };
        
        public static MicroInstructionType CPUControlCopyIFF2ToIFF1 =
        new MicroInstructionType
        {
            Family = MicroInstructionFamily.CPUControl,
            Name = "CPUControl.CopyIFF2ToIFF1",
            Description = "Copy interrupt flipflop 2 to interrupt flipflop 1"
        };

        public static MicroInstructionType CPUControlSetInterruptMode =
        new MicroInstructionType
        {
            Family = MicroInstructionFamily.CPUControl,
            Name = "CPUControl.SetInterruptMode",
            Description = "Sets interrupt mode 0, 1 or 2"
        };

        public static MicroInstructionType Register16SwitchTwoRegisters =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.Register,
                Name = "Register16.SwitchTwoRegisters",
                Description = "The 2-byte contents of register pairs AF are exchanged with AF', or DE is exchanged HL",
                ParamNames = new string[] { "Register" },
                ParamDescriptions = new string[] { "First register name which will be swapped with the other" }
            };

        public static MicroInstructionType Register16SwitchAlternateRegistersSet =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.Register,
                Name = "Register16.SwitchAlternateRegisterSet",
                Description = "The 2-byte contents of register pairs BC, DE and HL are exchanged with BC', DE' and HL'"
            };

        public static MicroInstructionType Register16Increment =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.Register,
                Name = "Register16.Increment",
                Description = "Increment the value of a 16 bits register",
                ParamNames = new string[] { "Register" },
                ParamDescriptions = new string[] { "16 bits register name" }
            };

        public static MicroInstructionType Register16Decrement =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.Register,
                Name = "Register16.Decrement",
                Description = "Decrement the value of a 16 bits register",
                ParamNames = new string[] { "Register" },
                ParamDescriptions = new string[] { "16 bits register name" }
            };

        public static MicroInstructionType RegisterRIncrement =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.Register,
                Name = "RegisterR.Increment",
                Description = "The R register is increased at every first machine cycle (M1), but bit 7 of the register is never changed by this"
            };

        public static MicroInstructionType RegisterDecrement =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.Register,
                Name = "Register.Decrement",
                Description = "Decrement the value of a 8 bits register",
                ParamNames = new string[] { "Register" },
                ParamDescriptions = new string[] { "8 bits register name" }
            };

        public static MicroInstructionType RegisterBCheckZero =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.Register,
                Name = "RegisterB.CheckZero",
                Description = "Checks if the value of the B register is zero"
            };

        public static MicroInstructionType RegisterBCCheckZero =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.Register,
                Name = "RegisterBC.CheckZero",
                Description = "Checks if the value of the BC register is zero"
            };

        public static MicroInstructionType RegisterWZReset =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.Register,
                Name = "RegisterWZ.Reset",
                Description = "Reset the value of the WZ register to one of 8 predefined values : 00H,08H,10H,18H,20H,28H,30H,38H",
                ParamNames = new string[] { "Address" },
                ParamDescriptions = new string[] { "WZ reset address" }
            };

        public static MicroInstructionType RegisterPCRepeatInstruction =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.Register,
                Name = "RegisterPC.RepeatInstruction",
                Description = "Decreases the value of the PC register from the bumber bytes of the current opcode to repeat the instruction"
            };

        public static MicroInstructionType BitOperationTest =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.ALU,
                Name = "BitOperation.Test",
                Description = "Test if bit n is set in the byte value stored in ALURightBuffer",
                ParamNames = new string[] { "Position", "Source" },
                ParamDescriptions = new string[] { "Bit index 0 to 7", "Addressing mode used to fetch the tested value" }
            };

        public static MicroInstructionType BitOperationSet =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.ALU,
                Name = "BitOperation.Set",
                Description = "Sets bit n the byte value stored in ALURightBuffer",
                ParamNames = new string[] { "Position" },
                ParamDescriptions = new string[] { "Bit index 0 to 7" }
            };

        public static MicroInstructionType BitOperationReset =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.ALU,
                Name = "BitOperation.Reset",
                Description = "Resets bit n the byte value stored in ALURightBuffer",
                ParamNames = new string[] { "Position" },
                ParamDescriptions = new string[] { "Bit index 0 to 7" }
            };

        public static MicroInstructionType BitOperationRotate =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.ALU,
                Name = "BitOperation.Rotate",
                Description = "Rotates the bits of ALULeftBuffer or ALURightBuffer one position to the left or to the right",
                ParamNames = new string[] { "Direction", "IncludeCarryInRotation", "ForAccumulator" },
                ParamDescriptions = new string[] { "Left or Right direction", "True if flag CF participates in the rotation", "True if the fast implementation for ALULeftBuffer is used," }
            };

        public static MicroInstructionType BitOperationRotateBCDDigit =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.ALU,
                Name = "BitOperation.RotateBCDDigit",
                Description = "Rotates three 4 bits groups : lower nibble of ALULeftBuffer, higher and lower nibbles of ALURightBuffer for BCD arithmetic",
                ParamNames = new string[] { "Direction" },
                ParamDescriptions = new string[] { "Left or Right direction" }
            };

        public static MicroInstructionType BitOperationShift =
            new MicroInstructionType
            {
                Family = MicroInstructionFamily.ALU,
                Name = "BitOperation.Shift",
                Description = "Shifts the bits of ALURightBuffer one position to the left or to the right",
                ParamNames = new string[] { "Direction", "ShiftType" },
                ParamDescriptions = new string[] { "Left or Right direction", "Arithmetic or Logical shift operation" }
            };
    }
}
