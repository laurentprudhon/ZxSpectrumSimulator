using System;
using System.Collections.Generic;
using Z80Simulator.Instructions;
using Z80Simulator.System;

namespace Z80Simulator.CPU
{
    /// <summary>
    /// ZiLOG Z80 CPU
    /// 
    /// Debug Interface
    /// 
    /// 
    /// </summary>
    public partial class Z80CPU
    {
        #region Notification events : views on coherent states of the internals of the CPU

        // These events are fired during a half TState in the following order :
        // (instruction start) (machine cycle start) executeMachineCycle (half tstate) executeMachineCycle[last] (machine cycle end) (instruction end)

        public enum LifecycleEventType
        {
            HalfTState,
            MachineCycleStart,
            MachineCycleEnd,
            InstructionStart,
            InstructionEnd
        }

        public delegate void LifecycleNotification(InternalState currentState, LifecycleEventType eventType);

        public class InternalState
        {
            public InternalState(Z80CPU cpu)
            {
                TStateCounter = cpu.tStateCounter;
                MachineCycleCounter = cpu.machineCycleCounter;
                InstructionCounter = cpu.instructionCounter;                
                InstructionOrigin = cpu.instructionOrigin;
                Instruction = cpu.currentInstruction;
                HalfTStateIndex = cpu.halfTStateIndex;
                MachineCycleIndex = cpu.machineCycleIndex;
                MachineCycle = cpu.currentMachineCycle;
            }

            // Absolute counters since power on        
            public long InstructionCounter { get; private set; }
            public long MachineCycleCounter { get; private set; }
            public long TStateCounter { get; private set; }

            // Current instruction
            public InstructionOrigin InstructionOrigin { get; private set; }
            public Instruction Instruction { get; private set; }

            // Relative counters within one instruction
            public byte MachineCycleIndex { get; private set; }
            public byte HalfTStateIndex { get; private set; }

            // Current machine cycle
            public MachineCycle MachineCycle { get; private set; }

            public override string ToString()
            {
                return String.Format("I{0}-{7:X}:[{1}]-M{2}:[{3}]-H{4}/T{5}C{6}", InstructionCounter, InstructionOrigin.OpCode != null ? InstructionOrigin.OpCode.InstructionText : Instruction.Type.OpCodeName, MachineCycleIndex, MachineCycle != null ? MachineCycle.Type.ToString() : "?", HalfTStateIndex, TStateCounter, MachineCycleCounter, InstructionOrigin.Address);
            }
        }

        public event LifecycleNotification HalfTState;
        public event LifecycleNotification MachineCycleStart;
        public event LifecycleNotification MachineCycleEnd;
        public event LifecycleNotification InstructionStart;
        public event LifecycleNotification InstructionEnd;

        #endregion

        #region Exit conditions : breakpoints and tests

        public interface IExitCondition
        {
            LifecycleEventType[] EventsSubscription { get; }
            bool CheckExitCondition(Z80CPU cpu, InternalState internalState, LifecycleEventType eventType);
        }

        private IList<IExitCondition> exitConditions = new List<IExitCondition>();
        private int[] conditionCountByEvent = new int[5];

        public int AddExitCondition(IExitCondition condition)
        {
            foreach (LifecycleEventType eventType in condition.EventsSubscription)
            {
                conditionCountByEvent[(int)eventType]++;
                if (conditionCountByEvent[(int)eventType] == 1)
                {
                    switch (eventType)
                    {
                        case LifecycleEventType.HalfTState:
                            HalfTState += CheckExitCondition;
                            break;
                        case LifecycleEventType.InstructionEnd:
                            InstructionEnd += CheckExitCondition;
                            break;
                        case LifecycleEventType.InstructionStart:
                            InstructionStart += CheckExitCondition;
                            break;
                        case LifecycleEventType.MachineCycleEnd:
                            MachineCycleEnd += CheckExitCondition;
                            break;
                        case LifecycleEventType.MachineCycleStart:
                            MachineCycleStart += CheckExitCondition;
                            break;
                    }
                }
            }
            exitConditions.Add(condition);
            return exitConditions.Count - 1; // return index of exit condition to be able to remove it later
        }

        public void RemoveExitCondition(int index)
        {
            IExitCondition condition = exitConditions[index];
            foreach (LifecycleEventType eventType in condition.EventsSubscription)
            {
                conditionCountByEvent[(int)eventType]--;
                if (conditionCountByEvent[(int)eventType] == 0)
                {
                    switch (eventType)
                    {
                        case LifecycleEventType.HalfTState:
                            HalfTState -= CheckExitCondition;
                            break;
                        case LifecycleEventType.InstructionEnd:
                            InstructionEnd -= CheckExitCondition;
                            break;
                        case LifecycleEventType.InstructionStart:
                            InstructionStart -= CheckExitCondition;
                            break;
                        case LifecycleEventType.MachineCycleEnd:
                            MachineCycleEnd -= CheckExitCondition;
                            break;
                        case LifecycleEventType.MachineCycleStart:
                            MachineCycleStart -= CheckExitCondition;
                            break;
                    }
                }
            }
            exitConditions.RemoveAt(index);
        }

        public class ExitConditionException : Exception
        {
            public IExitCondition ExitCondition { get; private set; }
            public InternalState InternalState { get; private set; }
            public LifecycleEventType EventType { get; private set; }

            public ExitConditionException(IExitCondition exitCondition, InternalState internalState, LifecycleEventType eventType)
            {
                this.ExitCondition = exitCondition;
                this.InternalState = internalState;
                this.EventType = eventType;
            }
        }

        private void CheckExitCondition(InternalState internalState, LifecycleEventType eventType)
        {
            foreach (IExitCondition exitCondition in exitConditions)
            {
                if (exitCondition.CheckExitCondition(this, internalState, eventType))
                {
                    throw new ExitConditionException(exitCondition, internalState, eventType);
                }
            }
        }

        #endregion

        #region Utility methods to fire debug notifications

        private ExitConditionException NotifyLifecycleEvent(LifecycleEventType eventType)
        {
            ExitConditionException pendingInterruption = null;
            try
            {
                switch (eventType)
                {
                    case LifecycleEventType.HalfTState:
                        if (HalfTState != null)
                        {
                            HalfTState(new InternalState(this), eventType);
                        }
                        break;
                    case LifecycleEventType.InstructionEnd:
                        if (InstructionEnd != null)
                        {
                            InstructionEnd(new InternalState(this), eventType);
                        }
                        break;
                    case LifecycleEventType.InstructionStart:
                        if (InstructionStart != null)
                        {
                            InstructionStart(new InternalState(this), eventType);
                        }
                        break;
                    case LifecycleEventType.MachineCycleEnd:
                        if (MachineCycleEnd != null)
                        {
                            MachineCycleEnd(new InternalState(this), eventType);
                        }
                        break;
                    case LifecycleEventType.MachineCycleStart:
                        if (MachineCycleStart != null)
                        {
                            MachineCycleStart(new InternalState(this), eventType);
                        }
                        break;
                }
            }
            catch (ExitConditionException e)
            {
                pendingInterruption = e;
            }
            return pendingInterruption;
        }

        #endregion
        
        #region Micro instructions log

        public bool TraceMicroInstructions { get; set; }
        public IList<MicroInstruction> MicroInstructions { get; set; }

        private void TraceMicroInstruction(MicroInstruction microInstruction)
        {
            MicroInstructions.Add(microInstruction);
        }

        public class Z80CPUBusConnector<T> : BusConnector<T>
        {
            private Z80CPU cpu;
            private string connectorName;

            public Z80CPUBusConnector(Z80CPU cpu, string connectorName)
            {
                this.cpu = cpu;
                this.connectorName = connectorName;
            }

            public new void ReleaseValue()
            {
                //if (connectedBus != null) -> optimization : no BusConnector should be used before it is connected to a bus
                //{
                connectedBus.ReleaseValue(/*this*/);
                //}

                if (cpu.TraceMicroInstructions)
                {
                    cpu.TraceMicroInstruction(new MicroInstruction(Z80MicroInstructionTypes.BusConnectorReleaseValue, connectorName));
                }
            }
        }

        #endregion

        #region Initialize internal state

        public void InitProgramCounter(ushort debugStartAddress)
        {
            PC = debugStartAddress;
        }

        public void InitRegister(Register register, byte debugInitialValue)
        {
            switch (register)
            {
                case Register.A:
                    A = debugInitialValue;
                    break;
                case Register.B:
                    B = debugInitialValue;
                    break;
                case Register.C:
                    C = debugInitialValue;
                    break;
                case Register.D:
                    D = debugInitialValue;
                    break;
                case Register.E:
                    E = debugInitialValue;
                    break;
                case Register.H:
                    H = debugInitialValue;
                    break;
                case Register.L:
                    L = debugInitialValue;
                    break;
                case Register.IXh:
                    IXh = debugInitialValue;
                    break;
                case Register.IXl:
                    IXl = debugInitialValue;
                    break;
                case Register.IYh:
                    IYh = debugInitialValue;
                    break;
                case Register.IYl:
                    IYl = debugInitialValue;
                    break;
                case Register.I:
                    I = debugInitialValue;
                    break;
                case Register.R:
                    R = debugInitialValue;
                    break;                    
                default:
                    throw new NotImplementedException();
            }
        }

        public void InitRegister16(Register16 register, ushort debugInitialValue)
        {
            switch (register)
            {
                case Register16.BC:
                    BC = debugInitialValue;
                    break;
                case Register16.DE:
                    DE = debugInitialValue;
                    break;
                case Register16.HL:
                    HL = debugInitialValue;
                    break;
                case Register16.SP:
                    SP = debugInitialValue;
                    break;
                case Register16.IX:
                    IX = debugInitialValue;
                    break;
                case Register16.IY:
                    IY = debugInitialValue;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void InitMemptr(ushort debugMemptr)
        {
            WZ = debugMemptr;
        }

        #endregion
    }
}
