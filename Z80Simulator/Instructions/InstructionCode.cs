
namespace Z80Simulator.Instructions
{
    public class InstructionCode
    {
        public byte  FetchMoreBytes { get; set; }
        public byte? SwitchToTableNumber { get; set; }

        public byte[] Prefix { get; set; }
        public byte   OpCode { get; set; }

        public byte InstructionTypeIndex { get; set; }
        public byte InstructionTypeParamVariant { get; set; }

        public InstructionType              InstructionType              { get { return Z80InstructionTypes.Table[InstructionTypeIndex]; } }
        public InstructionParametersVariant InstructionParametersVariant { get { return InstructionType.ParametersVariants[InstructionTypeParamVariant]; } }

        public string InstructionText { get; set; }
        public object Param1 { get; set; }
        public object Param2 { get; set; }
        public object Param3 { get; set; }

        public byte OpCodeByteCount { get; set; }
        public byte OperandsByteCount { get; set; }
        
        public bool IsUndocumented { get; set; }
        public bool IsDuplicate { get; set; }
    }
}
