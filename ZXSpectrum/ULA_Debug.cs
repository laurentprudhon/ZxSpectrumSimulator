using System;
using System.Collections.Generic;
using Z80Simulator.System;

namespace ZXSpectrum
{
    /// <summary>
    /// The ZX Spectrum ULA
    /// 
    /// Debug interface
    /// 
    /// </summary>
    public partial class ULA
    {
        #region Notification events : views on coherent states of the internals of the ULA

        // These events are fired in the following order :
        // (clock cycle)

        public enum LifecycleEventType
        {
            ClockCycle
        }

        public class InternalState
        {
            public InternalState(ULA ula)
            {
                Column = ula.column;
                Line = ula.line;
        
                GenerateBorder = ula.generateBorder;
                DisplayPixels = ula.displayPixels; 
        
                Frame = ula.frame;
                FlashClock = ula.flashClock;
        
                VideoMemAccessTimeFrame = ula.videoMemAccessTimeFrame;
                HaltCpuClock = ula.haltCpuClock;

                DisplayAddress = ula.displayAddress;
                AttributeAddress = ula.attributeAddress;

                DisplayLatch = ula.displayLatch;
                DisplayRegister = ula.displayRegister;
                AttributeLatch = ula.attributeLatch;
                AttributeRegister = ula.attributeRegister;
                BorderColorRegister = ula.borderColorRegister;
            }

            public int Column { get; private set; } 
            public int Line { get; private set; }
        
            public bool GenerateBorder { get; private set; }
            public bool DisplayPixels { get; private set; } 
        
            public int Frame { get; private set; }
            public bool FlashClock { get; private set; }
        
            public bool VideoMemAccessTimeFrame { get; private set; }
            public bool HaltCpuClock { get; private set; }

            public ushort DisplayAddress { get; private set; }
            public ushort AttributeAddress { get; private set; }

            public byte DisplayLatch { get; private set; }
            public byte DisplayRegister { get; private set; }
            public byte AttributeLatch { get; private set; }
            public byte AttributeRegister { get; private set; }
            public byte BorderColorRegister { get; private set; }

            public override string ToString()
            {
                return String.Format("L{0}C{1}-B?{2}D?{3}-Display{4:X},Attribute{5:X},Border{6:X}", Line, Column, GenerateBorder, DisplayPixels, DisplayRegister, AttributeRegister, BorderColorRegister);
            }
        }

        public delegate void LifecycleNotification(ULA ula, InternalState currentState, LifecycleEventType eventType);

        public event LifecycleNotification ClockCycle;

        #endregion

        #region Exit conditions : breakpoints and tests

        public interface IExitCondition
        {
            LifecycleEventType[] EventsSubscription { get; }
            // Throws ExitConditionException
            void CheckExitCondition(ULA ula, InternalState internalState, LifecycleEventType eventType);
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

        private IList<IExitCondition> exitConditions = new List<IExitCondition>();

        public int AddExitCondition(IExitCondition condition)
        {
            foreach (LifecycleEventType eventType in condition.EventsSubscription)
            {
                switch (eventType)
                {
                    case LifecycleEventType.ClockCycle:
                        ClockCycle += condition.CheckExitCondition;
                        break;
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
                switch (eventType)
                {
                    case LifecycleEventType.ClockCycle:
                        ClockCycle -= condition.CheckExitCondition;
                        break;
                }
            }
            exitConditions.RemoveAt(index);
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
                    case LifecycleEventType.ClockCycle:
                        if (ClockCycle != null)
                        {
                            ClockCycle(this, new InternalState(this), eventType);
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

        #region

        public void Debug_SetBorderColor(byte borderColor)
        {
            borderColorRegister = borderColor;
        }

        public void Debug_SetFrameCounter(int frameCount)
        {
            frame = frameCount;
        }

        #endregion
    }
}

