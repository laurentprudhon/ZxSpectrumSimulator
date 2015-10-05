
namespace Z80Simulator.Instructions
{
    public enum MicroInstructionFamily
    {
        DataPath,
        ALU,
        IOPins,
        CPUControl,
        Register
    }

    public class MicroInstructionType
    {
        public MicroInstructionFamily Family { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] ParamNames { get; set; }
        public string[] ParamDescriptions { get; set; }
    }

    public class MicroInstruction
    {
        public MicroInstructionType Type { get; set; }
        public object Parameter1 { get; set; }
        public object Parameter2 { get; set; }
        public object Parameter3 { get; set; }

        public MicroInstruction(MicroInstructionType type)
        {
            Type = type;
        }
        
        public MicroInstruction(MicroInstructionType type, object parameter1)
        {
            Type = type;
            Parameter1 = parameter1;
        }

        public MicroInstruction(MicroInstructionType type, object parameter1, object parameter2)
        {
            Type = type;
            Parameter1 = parameter1;
            Parameter2 = parameter2;
        }

        public MicroInstruction(MicroInstructionType type, object parameter1, object parameter2, object parameter3)
        {
            Type = type;
            Parameter1 = parameter1;
            Parameter2 = parameter2;
            Parameter3 = parameter3;
        }
    }
}
