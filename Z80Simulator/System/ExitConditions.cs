using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z80Simulator.CPU;

namespace Z80Simulator.System
{
    public class InstructionAddressCondition : Z80CPU.IExitCondition
    {
        public Z80CPU.LifecycleEventType[] EventsSubscription
        {
            get { return new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.InstructionStart }; }
        }

        private ushort instructionAddress;

        public InstructionAddressCondition(int instructionAddress)
        {
            this.instructionAddress = (ushort)instructionAddress;
        }

        public bool CheckExitCondition(Z80CPU cpu, Z80CPU.InternalState internalState, Z80CPU.LifecycleEventType eventType)
        {
            if (eventType == Z80CPU.LifecycleEventType.InstructionStart)
            {
                if (internalState.InstructionOrigin.Address == instructionAddress)
                {
                    return true;
                }
            }
            return false;
        }

        public ushort InstructionAddress { get { return instructionAddress; } }
    }

    public class InstructionCountCondition : Z80CPU.IExitCondition
    {
        public Z80CPU.LifecycleEventType[] EventsSubscription
        {
            get { return new Z80CPU.LifecycleEventType[] { Z80CPU.LifecycleEventType.InstructionEnd }; }
        }

        private int instructionCount;
        private long internalInstructionCounterInitialValue = -1;

        public InstructionCountCondition(int instructionCount)
        {
            this.instructionCount = instructionCount;
        }

        public bool CheckExitCondition(Z80CPU cpu, Z80CPU.InternalState internalState, Z80CPU.LifecycleEventType eventType)
        {
            if (eventType == Z80CPU.LifecycleEventType.InstructionEnd)
            {
                if (internalInstructionCounterInitialValue < 0)
                {
                    internalInstructionCounterInitialValue = internalState.InstructionCounter - 1;
                }
                if (internalState.InstructionCounter == internalInstructionCounterInitialValue + instructionCount)
                {
                    return true;
                }
            }
            return false;
        }

        public int InstructionCount { get { return instructionCount; } }
    }
}
