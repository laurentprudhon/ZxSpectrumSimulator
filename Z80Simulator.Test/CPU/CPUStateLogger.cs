using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Z80Simulator.CPU;
using Z80Simulator.Instructions;
using Z80Simulator.System;

namespace Z80Simulator.Test.CPU
{
    public class CPUStateLogger
    {
        private Z80CPU cpu;
        private StringBuilder log;

        public CPUStateLogger(Z80CPU cpu)
        {
            this.cpu = cpu;
            log = new StringBuilder();
        }

        public string Log { get { return log.ToString(); } }

        public void Clear()
        {
            log.Clear();
        }

        [Flags]
        public enum CPUStateElements
        {
            Registers = 1,
            Buses = 2,
            ControlPins = 4,
            InternalState = 8,
            MicroInstructions = 16
        }

        private CPUStateElements stateElementsToLog;
        private Z80CPU.LifecycleEventType[] eventTypesToLog;

        public void StartLogging(CPUStateElements stateElements, Z80CPU.LifecycleEventType[] eventTypes)
        {
            stateElementsToLog = stateElements;
            // Write header line : title for each column
            log.Append("event type");            
            log.Append(';');
            // Internal state : instruction, machine cycle, TStates
            if(stateElementsToLog.HasFlag(CPUStateElements.InternalState))
            {
                log.Append("instruction count");
                log.Append(';');
                log.Append("instruction address");
                log.Append(';');
                log.Append("instruction text");
                log.Append(';');
                log.Append("machine cycle index");
                log.Append(';');
                log.Append("machine cycle type");
                log.Append(';');
                log.Append("half Tstate index");
                log.Append(';');
                log.Append("Tstate count");
                log.Append(';');
            }
            // Micro instructions
            if (stateElementsToLog.HasFlag(CPUStateElements.MicroInstructions))
            {
                log.Append("micro instructions");
                log.Append(';');
            }
            // CPU registers
            if (stateElementsToLog.HasFlag(CPUStateElements.Registers))
            {
                log.Append("A");
                log.Append(';');
                log.Append("SZYHXPNC");
                log.Append(';');
                log.Append("B");
                log.Append(';');
                log.Append("C");
                log.Append(';');
                log.Append("D");
                log.Append(';');
                log.Append("E");
                log.Append(';');
                log.Append("H");
                log.Append(';');
                log.Append("L");
                log.Append(';');
                log.Append("PC");
                log.Append(';');
                log.Append("SP");
                log.Append(';');
                log.Append("IX");
                log.Append(';');
                log.Append("IY");
                log.Append(';');
                log.Append("IFF1");
                log.Append(';');
                log.Append("IFF2");
                log.Append(';');
                log.Append("IM");
                log.Append(';');
                log.Append("I");
                log.Append(';');
                log.Append("R");
                log.Append(';');
            }
            // Address and Data bus
            if (stateElementsToLog.HasFlag(CPUStateElements.Buses))
            {
                log.Append("address bus");
                log.Append(';');
                log.Append("data bus");
                log.Append(';');
            }
            // Control pins
            if (stateElementsToLog.HasFlag(CPUStateElements.ControlPins))
            {
                log.Append("M1");
                log.Append(';');
                log.Append("MREQ");
                log.Append(';');
                log.Append("IORQ");
                log.Append(';');
                log.Append("RD");
                log.Append(';');
                log.Append("WR");
                log.Append(';');
                log.Append("RFSH");
                log.Append(';');

                log.Append("HALT");
                log.Append(';');
                log.Append("WAIT");
                log.Append(';');
                log.Append("INT");
                log.Append(';');
                log.Append("NMI");
                log.Append(';');
                log.Append("RESET");
                log.Append(';');

                log.Append("BUSACK");
                log.Append(';');
                log.Append("BUSREQ");
                log.Append(';');

                log.Append("CLK");
                log.Append(';');
            }            
            log.AppendLine();

            eventTypesToLog = eventTypes;
            foreach (Z80CPU.LifecycleEventType eventType in eventTypesToLog)
            {
                switch (eventType)
                {
                    case Z80CPU.LifecycleEventType.HalfTState:
                        cpu.HalfTState += LogCPUState;
                        break;
                    case Z80CPU.LifecycleEventType.InstructionEnd:
                        cpu.InstructionEnd += LogCPUState;
                        break;
                    case Z80CPU.LifecycleEventType.InstructionStart:
                        cpu.InstructionStart += LogCPUState;
                        break;
                    case Z80CPU.LifecycleEventType.MachineCycleEnd:
                        cpu.MachineCycleEnd += LogCPUState;
                        break;
                    case Z80CPU.LifecycleEventType.MachineCycleStart:
                        cpu.MachineCycleStart += LogCPUState;
                        break;
                }
            }
            if (stateElementsToLog.HasFlag(CPUStateElements.MicroInstructions))
            {
                cpu.TraceMicroInstructions = true;
                cpu.MicroInstructions = new List<Z80Simulator.Instructions.MicroInstruction>();
            }
        }

        public void StopLogging()
        {
            foreach (Z80CPU.LifecycleEventType eventType in eventTypesToLog)
            {
                switch (eventType)
                {
                    case Z80CPU.LifecycleEventType.HalfTState:
                        cpu.HalfTState -= LogCPUState;
                        break;
                    case Z80CPU.LifecycleEventType.InstructionEnd:
                        cpu.InstructionEnd -= LogCPUState;
                        break;
                    case Z80CPU.LifecycleEventType.InstructionStart:
                        cpu.InstructionStart -= LogCPUState;
                        break;
                    case Z80CPU.LifecycleEventType.MachineCycleEnd:
                        cpu.MachineCycleEnd -= LogCPUState;
                        break;
                    case Z80CPU.LifecycleEventType.MachineCycleStart:
                        cpu.MachineCycleStart -= LogCPUState;
                        break;
                }
            }
            if (stateElementsToLog.HasFlag(CPUStateElements.MicroInstructions))
            {
                cpu.TraceMicroInstructions = false;
                cpu.MicroInstructions = null;
            }
        }

        private void LogCPUState(Z80CPU.InternalState internalState, Z80CPU.LifecycleEventType eventType)
        {
            // First column : event type
            switch (eventType)
            {
                case Z80CPU.LifecycleEventType.HalfTState:
                    log.Append("HT");
                    break;
                case Z80CPU.LifecycleEventType.InstructionEnd:
                    log.Append("IE");
                    break;
                case Z80CPU.LifecycleEventType.InstructionStart:
                    log.Append("IS");
                    break;
                case Z80CPU.LifecycleEventType.MachineCycleEnd:
                    log.Append("ME");
                    break;
                case Z80CPU.LifecycleEventType.MachineCycleStart:
                    log.Append("MS");
                    break;
            }
            log.Append(';');
            
            // Internal state : instruction, machine cycle, TStates
            if(stateElementsToLog.HasFlag(CPUStateElements.InternalState))
            {
                log.Append(internalState.InstructionCounter);
                log.Append(';');
                if (internalState.InstructionOrigin.Source == InstructionSource.Memory)
                {
                    log.Append(internalState.InstructionOrigin.Address);
                    log.Append(';');
                    log.Append(internalState.InstructionOrigin.OpCode != null ? internalState.InstructionOrigin.OpCode.InstructionText : "?");
                    log.Append(';');
                }
                else if (internalState.InstructionOrigin.Source == InstructionSource.Internal)
                {
                    log.Append(""); // No address - internal
                    log.Append(';');
                    log.Append(internalState.Instruction.Type.OpCodeName);
                    log.Append(';');
                }
                else if (internalState.InstructionOrigin.Source == InstructionSource.InterruptingDevice)
                {
                    log.Append(""); // No address - device
                    log.Append(';');
                    log.Append(internalState.InstructionOrigin.OpCode != null ? internalState.InstructionOrigin.OpCode.InstructionText : "?");
                    log.Append(';');
                }
                log.Append(internalState.MachineCycleIndex);
                log.Append(';');
                log.Append(internalState.MachineCycle != null ? internalState.MachineCycle.Type.ToString() : "?");
                log.Append(';');
                log.Append(internalState.HalfTStateIndex);
                log.Append(';');
                log.Append(internalState.TStateCounter);
                log.Append(';');
            }

            // Micro instructions
            if (stateElementsToLog.HasFlag(CPUStateElements.MicroInstructions))
            {
                for (int i = 0; i < cpu.MicroInstructions.Count; i++)
                {
                    if (i > 0)
                    {
                        log.Append("|");  
                    }
                    Z80Simulator.Instructions.MicroInstruction mi = cpu.MicroInstructions[i];
                    log.Append(mi.Type.Name);
                    if (mi.Parameter1 != null) log.Append("("+mi.Parameter1.ToString()+")");
                }
                cpu.MicroInstructions.Clear();
                log.Append(';');
            }

            // CPU registers
            if (stateElementsToLog.HasFlag(CPUStateElements.Registers))
            {
                log.Append(String.Format("{0:X}", cpu.A));
                log.Append(';');
                log.Append(Convert.ToString(cpu.F, 2).PadLeft(8, '0'));
                log.Append(';');
                log.Append(String.Format("{0:X}", cpu.B));
                log.Append(';');
                log.Append(String.Format("{0:X}", cpu.C));
                log.Append(';');
                log.Append(String.Format("{0:X}", cpu.D));
                log.Append(';');
                log.Append(String.Format("{0:X}", cpu.E));
                log.Append(';');
                log.Append(String.Format("{0:X}", cpu.H));
                log.Append(';');
                log.Append(String.Format("{0:X}", cpu.L));
                log.Append(';');
                log.Append(String.Format("{0:X}", cpu.PC));
                log.Append(';');
                log.Append(String.Format("{0:X}", cpu.SP));
                log.Append(';');
                log.Append(String.Format("{0:X}", cpu.IX));
                log.Append(';');
                log.Append(String.Format("{0:X}", cpu.IY));
                log.Append(';');
                log.Append(String.Format("{0}", cpu.IFF1 ? 1 : 0));
                log.Append(';');
                log.Append(String.Format("{0}", cpu.IFF2 ? 1 : 0));
                log.Append(';');
                log.Append(String.Format("{0}", cpu.IM));
                log.Append(';');
                log.Append(String.Format("{0:X}", cpu.I));
                log.Append(';');
                log.Append(String.Format("{0:X}", cpu.R));
                log.Append(';');
            }

            // Address and Data bus
            if (stateElementsToLog.HasFlag(CPUStateElements.Buses))
            {
                log.Append(String.Format("{0:X}", cpu.Address.SampleValue()));
                log.Append(';');
                log.Append(String.Format("{0:X}", cpu.Data.SampleValue()));
                log.Append(';');
            }

            // Control pins
            if (stateElementsToLog.HasFlag(CPUStateElements.ControlPins))
            {
                log.Append(cpu.M1 == SignalState.LOW ? 0 : 1);
                log.Append(';');
                log.Append(cpu.MREQ == SignalState.LOW ? 0 : 1);
                log.Append(';');
                log.Append(cpu.IORQ == SignalState.LOW ? 0 : 1);
                log.Append(';');
                log.Append(cpu.RD == SignalState.LOW ? 0 : 1);
                log.Append(';');
                log.Append(cpu.WR == SignalState.LOW ? 0 : 1);
                log.Append(';');
                log.Append(cpu.RFSH == SignalState.LOW ? 0 : 1);
                log.Append(';');

                log.Append(cpu.HALT == SignalState.LOW ? 0 : 1);
                log.Append(';');
                log.Append(cpu.WAIT == SignalState.LOW ? 0 : 1);
                log.Append(';');
                log.Append(cpu.INT == SignalState.LOW ? 0 : 1);
                log.Append(';');
                log.Append(cpu.NMI == SignalState.LOW ? 0 : 1);
                log.Append(';');
                log.Append(cpu.RESET == SignalState.LOW ? 0 : 1);
                log.Append(';');

                log.Append(cpu.BUSACK == SignalState.LOW ? 0 : 1);
                log.Append(';');
                log.Append(cpu.BUSREQ == SignalState.LOW ? 0 : 1);
                log.Append(';');

                log.Append(cpu.CLK == SignalState.LOW ? 0 : 1);
                log.Append(';');
            }

            log.AppendLine();
        }
        
        public bool CompareWithSavedLog(string filename)
        {
            string filePath = String.Format("{0}.csv", filename);
            Stream stream = PlatformSpecific.GetStreamForProjectFile(filePath);
            string referenceLog = null;
            using (StreamReader reader = new StreamReader(stream))
            {
                referenceLog = reader.ReadToEnd();
            }
            return Log == referenceLog;
        }
    }
}
