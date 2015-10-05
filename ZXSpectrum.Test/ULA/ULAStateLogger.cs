using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Z80Simulator.CPU;
using Z80Simulator.System;
using ZXSpectrum;

namespace ZXSpectrum.Test.ULAChip
{
    public class ULAStateLogger
    {
        private ULA ula;
        private StringBuilder log;

        public ULAStateLogger(ULA ula)
        {
            this.ula = ula;
            log = new StringBuilder();
        }

        public string Log { get { return log.ToString(); } }

        public void Clear()
        {
            log.Clear();
        }

        [Flags]
        public enum ULAStateElements
        {
            MasterCounters = 1,
            CPUClock = 2,
            DataIO = 4,
            VideoOutput = 8,
            VideoRegisters = 16
        }

        private ULAStateElements stateElementsToLog;
        private ULA.LifecycleEventType[] eventTypesToLog;

        public void StartLogging(ULAStateElements stateElements, ULA.LifecycleEventType[] eventTypes)
        {
            stateElementsToLog = stateElements;
            // Write header line : title for each column
            log.Append("event type");            
            log.Append(';');
            // Pixel clock and master counters
            if(stateElementsToLog.HasFlag(ULAStateElements.MasterCounters))
            {
                log.Append("pixel clock");
                log.Append(';');
                log.Append("line");
                log.Append(';');
                log.Append("column");
                log.Append(';');
            }
            // CPU clock and interrupt
            if (stateElementsToLog.HasFlag(ULAStateElements.CPUClock))
            {
                log.Append("CPU clock");
                log.Append(';');
                log.Append("HaltCpuClock");
                log.Append(';');
                log.Append("CPU interrupt");
                log.Append(';');
            }
            // ULA data input / output
            if (stateElementsToLog.HasFlag(ULAStateElements.DataIO))
            {
                log.Append("Address bus");
                log.Append(';');
                log.Append("Data bus");
                log.Append(';');

                log.Append("VideoMemAccessTimeFrame");
                log.Append(';');
                log.Append("CpuIORQ");
                log.Append(';');
                log.Append("CpuWR");
                log.Append(';');
                log.Append("CpuRD");
                log.Append(';');

                log.Append("VidMemREQ");
                log.Append(';');
                log.Append("VidMemRD");
                log.Append(';');
            }
            // Video output
            if (stateElementsToLog.HasFlag(ULAStateElements.VideoOutput))
            {
                log.Append("ColorSignal");
                log.Append(';');
                log.Append("HSync");
                log.Append(';');                
                log.Append("VSync");
                log.Append(';');
            }
            // Internal video generator
            if (stateElementsToLog.HasFlag(ULAStateElements.VideoRegisters))
            {
                log.Append("GenerateBorder");
                log.Append(';');
                log.Append("BorderColorRegister");
                log.Append(';');

                log.Append("DisplayAddress");
                log.Append(';');
                log.Append("DisplayLatch");
                log.Append(';');
                log.Append("DisplayRegister");
                log.Append(';');
                log.Append("DisplayPixels");
                log.Append(';');

                log.Append("AttributeAddress");
                log.Append(';');
                log.Append("AttributeLatch");
                log.Append(';');
                log.Append("AttributeRegister");
                log.Append(';');

                log.Append("Frame");
                log.Append(';');
                log.Append("FlashClock");
                log.Append(';');
            }            
            log.AppendLine();

            eventTypesToLog = eventTypes;
            foreach (ULA.LifecycleEventType eventType in eventTypesToLog)
            {
                switch (eventType)
                {
                    case ULA.LifecycleEventType.ClockCycle:
                        ula.ClockCycle += LogULAState;
                        break;
                }
            }
        }

        public void StopLogging()
        {
            foreach (ULA.LifecycleEventType eventType in eventTypesToLog)
            {
                switch (eventType)
                {
                    case ULA.LifecycleEventType.ClockCycle:
                        ula.ClockCycle -= LogULAState;
                        break;
                }
            }
        }

        private void LogULAState(ULA ula, ULA.InternalState internalState, ULA.LifecycleEventType eventType)
        {
            // First column : event type
            switch (eventType)
            {
                case ULA.LifecycleEventType.ClockCycle:
                    log.Append("CL");
                    log.Append((internalState.Column%16).ToString("D2"));
                    break;
            }
            log.Append(';');
            // Pixel clock and master counters
            if (stateElementsToLog.HasFlag(ULAStateElements.MasterCounters))
            {
                log.Append(ula.PixelCLK == SignalState.LOW ? 0 : 1);
                log.Append(';');
                log.Append(internalState.Line);
                log.Append(';');
                log.Append(internalState.Column);
                log.Append(';');
            }
            // CPU clock and interrupt
            if (stateElementsToLog.HasFlag(ULAStateElements.CPUClock))
            {
                log.Append(ula.CpuCLK == SignalState.LOW ? 0 : 1);
                log.Append(';');
                log.Append(internalState.HaltCpuClock);
                log.Append(';');
                log.Append(ula.CpuINT == SignalState.LOW ? 0 : 1);
                log.Append(';');
            }
            // ULA data input / output
            if (stateElementsToLog.HasFlag(ULAStateElements.DataIO))
            {
                log.Append(ula.VideoAddress.SampleValue().ToString("X4"));
                log.Append(';');
                log.Append(ula.VideoData.SampleValue().ToString("X2"));
                log.Append(';');

                log.Append(internalState.VideoMemAccessTimeFrame);
                log.Append(';');
                log.Append(ula.CpuIORQ == SignalState.LOW ? 0 : 1);
                log.Append(';');
                log.Append(ula.CpuWR == SignalState.LOW ? 0 : 1);
                log.Append(';');
                log.Append(ula.CpuRD == SignalState.LOW ? 0 : 1);
                log.Append(';');

                log.Append(ula.VideoMREQ == SignalState.LOW ? 0 : 1);
                log.Append(';');
                log.Append(ula.VideoRD == SignalState.LOW ? 0 : 1);
                log.Append(';');
            }
            // Video output
            if (stateElementsToLog.HasFlag(ULAStateElements.VideoOutput))
            {
                log.Append(ula.ColorSignal.Level);
                log.Append(';');
                log.Append(ula.HSync == SignalState.LOW ? 0 : 1);
                log.Append(';');
                log.Append(ula.VSync == SignalState.LOW ? 0 : 1);
                log.Append(';');
            }
            // Internal video generator
            if (stateElementsToLog.HasFlag(ULAStateElements.VideoRegisters))
            {
                log.Append(internalState.GenerateBorder);
                log.Append(';');
                log.Append(internalState.BorderColorRegister);
                log.Append(';');

                log.Append(internalState.DisplayAddress.ToString("X4"));
                log.Append(';');
                log.Append(internalState.DisplayLatch);
                log.Append(';');
                log.Append(internalState.DisplayRegister);
                log.Append(';');
                log.Append(internalState.DisplayPixels);
                log.Append(';');

                log.Append(internalState.AttributeAddress.ToString("X4"));
                log.Append(';');
                log.Append(internalState.AttributeLatch);
                log.Append(';');
                log.Append(internalState.AttributeRegister);
                log.Append(';');

                log.Append(internalState.Frame);
                log.Append(';');
                log.Append(internalState.FlashClock);
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
