using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z80Simulator.Assembly;
using Z80Simulator.CPU;

namespace Z80Simulator.System
{
    public abstract class Z80SystemBase
    {
        protected Clock clock;
        public Clock Clock { get { return clock; } }

        protected Z80CPU cpu;
        public Z80CPU CPU { get { return cpu; } }

        protected Bus<ushort> addressBus = new Bus<ushort>();
        public Bus<ushort> AddressBus { get { return addressBus; } }

        protected Bus<byte> dataBus = new Bus<byte>();
        public Bus<byte> DataBus { get { return dataBus; } }
        
        public static Memory memory;
        public Memory Memory { get { return memory; } }
        
        private Program programInMemory;
        public Program ProgramInMemory { get { return programInMemory; } }
                             
        public Program LoadProgramInMemory(string program)
        {
            byte[] byteArray = Encoding.Unicode.GetBytes(program);
            MemoryStream stream = new MemoryStream(byteArray);
            return LoadProgramInMemory(String.Empty, stream, Encoding.Unicode, false);
        }

        public Program LoadProgramInMemory(string sourcePath, Stream inputStream, Encoding encoding, bool ignoreCase)
        {
            Program program = Assembler.ParseProgram(sourcePath, inputStream, encoding, ignoreCase);
            MemoryMap programBytes = new MemoryMap(memory.Size);
            Assembler.CompileProgram(program, 0, programBytes);
            if (program.ErrorCount > 0)
            {
                throw new Exception("Program error at line " + program.LinesWithErrors[0].LineNumber + " : " + program.LinesWithErrors[0].Text);
            }
            memory.LoadBytes(programBytes.MemoryCells);
            programInMemory = program;
            return program;
        }

        public int AddBreakpointOnProgramLine(int lineNumber)
        {
            ProgramLine programLine = programInMemory.Lines[lineNumber - 1];
            InstructionAddressCondition instructionAddressCondition = new InstructionAddressCondition(programLine.LineAddress);
            int breakpointIndex = cpu.AddExitCondition(instructionAddressCondition);
            return breakpointIndex;
        }

        public void RemoveBreakpoint(int breakPointIndex)
        {
            cpu.RemoveExitCondition(breakPointIndex);
        }

        public void ExecuteUntilNextBreakpoint()
        {
            clock.TickUntilExitCondition();
        }

        public void ExecuteInstructionCount(int instructionCount)
        {
            InstructionCountCondition instructionCountCondition = new InstructionCountCondition(instructionCount);
            int exitConditionIndex = cpu.AddExitCondition(instructionCountCondition);
            clock.TickUntilExitCondition();
            cpu.RemoveExitCondition(exitConditionIndex);
        }

        public void ExecuteFor20Milliseconds()
        {
            clock.Tick(70000);
        }
    }

    public class Z80System : Z80SystemBase
    {
        public Z80System()
        {
            // Initialize components
            clock = new _Clock(3500000L);
            cpu = new _Z80CPU();
            memory = new _Memory(65536);

            // Connect buses
            cpu.Address.ConnectTo(addressBus);
            cpu.Data.ConnectTo(dataBus);
            memory.Address.ConnectTo(addressBus);
            memory.Data.ConnectTo(dataBus);

            // Connect individual PINs
            ((_Clock)clock).ConnectTo(cpu);
            ((_Z80CPU)cpu).ConnectTo(clock, memory);
            ((_Memory)memory).ConnectTo(cpu);
        }

        protected class _Clock : Clock
        {
            public _Clock(long frequencyHerz) : base(frequencyHerz)
            { }

            protected Z80CPU cpu;
            public void ConnectTo(Z80CPU cpu)
            {
                this.cpu = cpu;
            }

            // Output PINs with notifications

            public override SignalState CLK
            {
                protected set
                {
                    base.CLK = value;
                    cpu.CLK_OnEdge();
                }
            }
        }

        protected class _Z80CPU : Z80CPU
        {
            protected Clock clock;
            protected Memory memory;
            public void ConnectTo(Clock clock, Memory memory)
            {
                this.clock = clock;
                this.memory = memory;
            }

            // Input PINs sources

            public override SignalState BUSREQ
            {
                get { return SignalState.HIGH; }
            }

            public override SignalState CLK
            {
                get { return clock.CLK; }
            }

            public override SignalState INT
            {
                get { return SignalState.HIGH; }
            }

            public override SignalState NMI
            {
                get { return SignalState.HIGH; }
            }

            public override SignalState RESET
            {
                get { return SignalState.HIGH; }
            }

            public override SignalState WAIT
            {
                get { return SignalState.HIGH; }
            }

            // Output PINs with notifications
       
            public override SignalState MREQ
            {
                protected set
                {
                    base.MREQ = value;
                    if (value == SignalState.LOW)
                    {
                        memory.ChipSelect_OnEdgeLow();
                    }
                }
            }

            public override SignalState RD
            {
                protected set
                {
                    base.RD = value;
                    if (value == SignalState.LOW)
                    {
                        memory.ReadEnable_OnEdgeLow();
                    }
                    else
                    {
                        memory.ReadEnable_OnEdgeHigh();
                    }
                }
            }

            public override SignalState WR
            {
                protected set
                {
                    base.WR = value;
                    if (value == SignalState.LOW)
                    {
                        memory.WriteEnable_OnEdgeLow();
                    }
                }
            }
        }

        protected class _Memory : Memory
        {
            public _Memory(int capacityBytes) : base(capacityBytes)
            { }

            protected Z80CPU cpu;
            internal void ConnectTo(Z80CPU cpu)
            {
                this.cpu = cpu;
            }

            // Input PINs sources

            public override SignalState ChipSelect
            {
                get { return cpu.MREQ; }
            }

            public override SignalState ReadEnable
            {
                get { return cpu.RD; }
            }

            public override SignalState Refresh
            {
                get { return cpu.RFSH; }
            }

            public override SignalState WriteEnable
            {
                get { return cpu.WR; }
            }
        }
    }
}
