
namespace Z80Simulator.Instructions
{
    public class InstructionType
    {
        public byte Index { get; set; }

        public string InstructionGroup { get; set; }
        
        public string OpCodeName { get; set; }            
        
        public AddressingMode? Param1Type { get; set; } 
        public AddressingMode? Param2Type { get; set; }
        public AddressingMode? Param3Type { get; set; }
        
        public bool IsUndocumented { get; set; }
        public bool IsInternalZ80Operation { get; set; }
                
        public string Equation { get; set; }
        public string Description  { get; set; }
        public string ConditionBitsAffected  { get; set; }
        public string Example  { get; set; }
        public string UserManualPage { get; set; }

        public InstructionParametersVariant[] ParametersVariants { get; set; }
    }

    public class InstructionParametersVariant
    {
        public byte Index { get; set; }

        public string[] Param1List { get; set; }
        public string[] Param2List { get; set; }
        public string[] Param3List { get; set; }

        public byte InstructionCodeSizeInBytes { get; set; }
        public byte InstructionCodeCount { get; set; }

        public InstructionExecutionVariant ExecutionTimings { get; set; }
        public InstructionExecutionVariant AlternateExecutionTimings { get; set; }

        public bool IsUndocumented { get; set; }
    }

    public class InstructionExecutionVariant
    {
        public string Condition { get; set; }

        public byte MachineCyclesCount { get; set; }
        public byte TStatesCount { get; set; }
              
        public MachineCycle[] MachineCycles { get; set; }

        public string TStatesByMachineCycle { get; set; }
        public string StackOperations { get; set; }
        public string M1Comment { get; set; }

    }

    public class Instruction
    {
        public InstructionType Type { get; set; }
        public InstructionParametersVariant ParametersVariant { get; set; }

        private InstructionExecutionVariant _executionTimings;
        public InstructionExecutionVariant ExecutionTimings 
        { 
            get { return _executionTimings; } 
            set 
            {
                _executionTimings = value;
                LastMachineCycleIndex = (byte)(_executionTimings.MachineCyclesCount - 1);
            }
        }        
        public byte LastMachineCycleIndex;

        public Instruction(int instructionTypeIndex, int instructionTypeParamVariant)
        {
            Type = Z80InstructionTypes.Table[instructionTypeIndex];
            ParametersVariant = Type.ParametersVariants[instructionTypeParamVariant];
            ExecutionTimings = ParametersVariant.ExecutionTimings;
        }

        public void SelectAlternateExecutionTimings()
        {
            ExecutionTimings = ParametersVariant.AlternateExecutionTimings;
        }

        public static Instruction NMI = new Instruction(157, 0);
        public static Instruction INT0 = new Instruction(158, 0);
        public static Instruction INT1 = new Instruction(159, 0);
        public static Instruction INT2 = new Instruction(160, 0);
        public static Instruction RESET = new Instruction(161, 0);
        public static Instruction FetchNewInstruction = new Instruction(162, 0);
    }

    /// <summary>
    /// An instruction can be decoded from three different sources
    /// </summary>
    public enum InstructionSource
    {
        /// <summary>
        /// Opcode read from memory and stored in IR register
        /// </summary>
        Memory,
        /// <summary>
        /// Internal instruction triggered by a CPU input pin (reset and interrupts)
        /// </summary>
        Internal,
        /// <summary>
        /// Instruction inserted on the data bus by an interrupting device (interrupt mode 0)
        /// </summary>
        InterruptingDevice
    }

    public class InstructionOrigin
    {
        public InstructionSource Source { get; set; }
        // If Source =  Memory or Source = InterruptingDevice
        public InstructionCode OpCode { get; set; }
        // Only if Source = Memory
        public ushort Address { get; set; }

        public InstructionOrigin(InstructionSource source, InstructionCode opCode, ushort memoryAddress)
        {
            Source = source;
            OpCode = opCode;
            Address = memoryAddress;
        }

        public static InstructionOrigin Internal = new InstructionOrigin(InstructionSource.Internal, null, 0);
    }

    public class MachineCycle
    {
        public MachineCycleType Type { get; set; }
        
        private byte _TStates;
        public byte TStates 
        { 
            get { return _TStates; }
            set 
            { 
                _TStates = value;
                PenultimateHalfTStateIndex = (byte)(2 * _TStates - 2);
                LastHalfTStateIndex = (byte)(2 * _TStates - 1);
            }
        }

        public byte PenultimateHalfTStateIndex;
        public byte LastHalfTStateIndex;

        public static MachineCycle OpCodeFetch = new MachineCycle { Type = MachineCycleType.OCF, TStates = 4 };
        public static MachineCycle BusRequestAcknowledge = new MachineCycle { Type = MachineCycleType.BRQA, TStates = 2 };
    }

    public enum MachineCycleType
    {
        CPU,    //	Internal CPU Operation
        MR,     //	Memory Read
        MRH,    //	Memory Read of High Byte
        MRL,    //	Memory Read of Low Byte
        MW,     //	Memory Write
        MWH,    //	Memory Write of High Byte
        MWL,    //	Memory Write of Low Byte
        OCF,    //	Op Code Fetch
        OC4,    //  Last opcode fetch in a 4 bytes opcode
        OD,     //	Operand Data Read
        ODH,    //	Operand Data Read of High Byte
        ODL,    //	Operand Data Read of Low Byte
        PR,     //	Port Read
        PW,     // 	Port Write
        SRH,    //	Stack Read of High Byte
        SRL,    //	Stack Read of Low Byte
        SWH,    //	Stack Write of High Byte
        SWL,    //	Stack Write of Low byte
        INTA,   //  Interrupt request acknowledge cycle
        BRQA    //  Bus request acknowledge cycle
    }
}
