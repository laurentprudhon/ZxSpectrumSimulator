using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z80Simulator.Assembly
{
    public enum MemoryDescriptionType { ProgramLine, Data, SystemVariable }

    public class MemoryCellDescription
    {
        public MemoryCellDescription(MemoryDescriptionType descriptionType, object description)
        {
            Type = descriptionType;
            Description = description;
        }

        public MemoryDescriptionType Type { get; set; }
        public object Description { get; set; }
    }

    public class MemoryMap
    {
        public byte[]                   MemoryCells             { get; private set; }
        public MemoryCellDescription[]  MemoryCellDescriptions  { get; private set; }

        public IList<Program> Programs { get; private set; }
        public IList<int> EntryPoints { get; private set; }

        public MemoryMap(int size)
        {
            MemoryCells = new byte[size];
            MemoryCellDescriptions = new MemoryCellDescription[size];
            Programs = new List<Program>();
        }
    }
}
