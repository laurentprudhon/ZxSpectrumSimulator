using System;
using Z80Simulator.System;

namespace ZXSpectrum.TapeFiles
{
    /// <summary>
    /// Model of a sound portion of the tape.
    /// 
    /// Tzx tape specification :
    /// 
    /// The timings are given in Z80 clock ticks (T states) unless otherwise stated. 1 T state = (1/3500000)s 
    /// You might interpret 'full-period' as ----____ or ____----, and 'half-period' as ---- or ____. One 'half-period' will also be referred to as a 'pulse'. 
    /// The values in curly brackets {} are the default values that are used in the Spectrum ROM saving routines. These values are in decimal. 
    /// If there is no pause between two data blocks then the second one should follow immediately; not even so much as one T state between them. 
    /// 
    /// This document refers to 'high' and 'low' pulse levels. Whether this is implemented as ear=1 and ear=0 respectively or the other way around is not important, as long as it is done consistently. 
    /// An emulator should put the 'current pulse level' to 'low' when starting to play a TZX file, either from the start or from a certain position. 
    /// 
    /// 1.A block starts with a pilot signal. This 'tells' the loading routine that a new block is starting. The Spectrum ROM needs this pilot tone to be more than 1 second in duration. 
    /// 2.After the pilot come 2 sync pulses, which are significantly smaller than a pilot pulse. This marks the end of the pilot. 
    /// 3.Then comes the datablock. 1 bit of data is composed of 2 equal pulses. The length of the pulses determines whether it's a '0' or a '1' bit. Usually, a '1' bit pulse will be twice as long as a '0' bit pulse. The amplitude of each pulse is not important, as the loading routines are 'edge-triggered' rather than 'level-triggered'. 
    /// The datablock itself is composed of the following parts: 
    /// The first byte is the flag byte. The ROM uses this to make the difference between a 'header' block and a 'data' block. 
    /// The last byte is the parity byte. This is an 8-bit XOR of all previous bytes, including the flag byte. While loading the data, this parity is calculated again and matched with this last byte. If they match, loading was successful, otherwise you get the dreaded 'Tape Loading Error'. 
    /// The rest is the actual data, which is stored in memory. 
    /// 4.A block is always ended in one of two ways: a pause, or the next pilot.
    /// </summary>
    public abstract class TapeSoundSequence
    {
        /// <summary>
        /// Explanation of the role of this sound sequence (pilot tone, sync pulse, data, pause ...)
        /// </summary>
        public string Label { get; protected set; }

        protected void InitLabel(string label)
        {
            Label = label + " [" + Math.Round(Duration/1000f) + " sec]";
        }

        /// <summary>
        /// Duration of the sound sequence in ms
        /// </summary>
        public int Duration { get; protected set; }

        // State machine used to play the sound sequence

        public abstract bool GetNextState(ref byte signalLevel, out int tStatesBeforeNextStateChange);

        public abstract void ResetPosition();

        // Utility methods

        protected void Invert(ref byte signalLevel)
        {
            signalLevel = (byte)(1 - signalLevel);
        }
    }

    /// <summary>
    /// A 'Pause' block of zero duration is completely ignored, so the 'current pulse level' will NOT change in this case. 
    /// This also applies to 'Data' blocks that have some pause duration included in them. 
    /// A 'Pause' block consists of a 'low' pulse level of some duration. To ensure that the last edge produced is properly finished there should be at least 1 ms. (3500 T States) pause of the opposite level and only after that the pulse should go to 'low'. 
    /// At the end of a 'Pause' block the 'current pulse level' is low (note that the first pulse will therefore not immediately produce an edge).
    /// </summary>
    public class PauseSequence : TapeSoundSequence
    {
        /// <summary>
        /// Pause duration in TStates
        /// </summary>
        public int PauseTime { get; private set; }

        private const byte STATE_FINISHLASTEDGE = 0;
        private const byte STATE_PAUSE = 1;
        private const byte STATE_END = 2;

        private byte internalState;

        public PauseSequence(string label, ushort pauseTimeInMs)
        {
            PauseTime = pauseTimeInMs * 3500;
            Duration = pauseTimeInMs;
            InitLabel(label);
        }

        public override bool GetNextState(ref byte signalLevel, out int tStatesBeforeNextStateChange)
        {
            if (internalState == STATE_FINISHLASTEDGE)
            {
                if (signalLevel == 1)
                {
                    tStatesBeforeNextStateChange = 3500;
                    internalState = STATE_PAUSE;
                    return true;
                }
                else
                {
                    internalState = STATE_PAUSE;
                }
            }

            if (internalState == STATE_PAUSE)
            {
                signalLevel = 0;
                tStatesBeforeNextStateChange = PauseTime;
                internalState = STATE_END;
                return true;
            }
            else //if (internalState == STATE_END)
            {
                signalLevel = 0;
                tStatesBeforeNextStateChange = 0;
                return false;
            }
        }

        public override void ResetPosition()
        {
            internalState = STATE_FINISHLASTEDGE;
        }

        public override string ToString()
        {
            return "Pause : silence during " + Duration + " ms";
        }
    }

    /// <summary>
    /// The 'current pulse level' after playing the block is the opposite of the last pulse level played, so that a subsequent pulse will produce an edge.            
    /// </summary>
    public class ToneSequence : TapeSoundSequence
    {
        /// <summary>
        /// Number of pulses to generate
        /// </summary>
        public int PulsesCount { get; private set; }
        /// <summary>
        /// Length of each pulse in TStates
        /// </summary>
        public int PulseLength { get; private set; }

        private int pulseIndex = 0;

        public ToneSequence(string label, ushort pulsesCount, ushort pulseLength)
        {
            PulsesCount = pulsesCount;
            PulseLength = pulseLength;
            Duration = (pulsesCount * pulseLength) / 3500;
            InitLabel(label);
        }

        public override bool GetNextState(ref byte signalLevel, out int tStatesBeforeNextStateChange)
        {
            if (pulseIndex == 0)
            {
                tStatesBeforeNextStateChange = PulseLength;
                pulseIndex++;
                return true;
            }
            else if (pulseIndex < PulsesCount)
            {
                Invert(ref signalLevel);
                tStatesBeforeNextStateChange = PulseLength;
                pulseIndex++;
                return true;
            }
            else
            {
                Invert(ref signalLevel);
                tStatesBeforeNextStateChange = 0;
                return false;
            }
        }

        public override void ResetPosition()
        {
            pulseIndex = 0;
        }

        public override string ToString()
        {
            return "Tone : " + PulsesCount/2 + " periods, frequency = " + 3500 / PulseLength + " kHz";
        }
    }

    /// <summary>
    /// The 'current pulse level' after playing the block is the opposite of the last pulse level played, so that a subsequent pulse will produce an edge.             
    /// </summary>
    public class PulsesSequence : TapeSoundSequence
    {
        /// <summary>
        /// Pulses lengths in TStates
        /// </summary>
        public int[] PulsesLengths { get; private set; }

        private int pulseIndex = 0;

        public PulsesSequence(string label, ushort[] pulsesLengths)
        {
            PulsesLengths = new int[pulsesLengths.Length];

            int durationInTStates = 0;
            for (int i = 0; i < pulsesLengths.Length; i++)
            {
                PulsesLengths[i] = pulsesLengths[i];
                durationInTStates += pulsesLengths[i];
            }

            Duration = durationInTStates / 3500;
            InitLabel(label);
        }

        public override bool GetNextState(ref byte signalLevel, out int tStatesBeforeNextStateChange)
        {
            if (pulseIndex == 0)
            {
                tStatesBeforeNextStateChange = PulsesLengths[pulseIndex];
                pulseIndex++;
                return true;
            }
            else if (pulseIndex < PulsesLengths.Length)
            {
                Invert(ref signalLevel);
                tStatesBeforeNextStateChange = PulsesLengths[pulseIndex];
                pulseIndex++;
                return true;
            }
            else
            {
                Invert(ref signalLevel);
                tStatesBeforeNextStateChange = 0;
                return false;
            }
        }

        public override void ResetPosition()
        {
            pulseIndex = 0;
        }

        public override string ToString()
        {
            return "Pulses : " + PulsesLengths.Length + " irregular pulses, total length = " + Duration + " ms";
        }
    }

    /// <summary>
    /// The 'current pulse level' after playing the block is the opposite of the last pulse level played, so that a subsequent pulse will produce an edge.             
    /// </summary>
    public class DataSequence : TapeSoundSequence
    {
        public byte[] Data { get; private set; }
        public int ZeroPulseLength { get; private set; }
        public int OnePulseLength { get; private set; }
        public byte UsedBitsOfTheLastByte { get; private set; }

        private int byteIndex = 0;
        private int bitOfTheByteIndex = 0;
        private bool isFirstPulseOfBit = true;

        public DataSequence(string label, byte[] data, ushort zeroPulseLength, ushort onePulseLength, byte usedBitsOfTheLastByte)
        {
            Data = data;
            ZeroPulseLength = zeroPulseLength;
            OnePulseLength = onePulseLength;
            UsedBitsOfTheLastByte = usedBitsOfTheLastByte;

            int durationInTStates = 0;
            for (int i = 0; i < data.Length ; i++)
            {
                byte bitsCount = 8;
                if (i == data.Length - 1)
                {
                    bitsCount = usedBitsOfTheLastByte;
                }
                for (int j = 0; j < bitsCount; j++)
                {
                    bool dataIsZero = ((Data[i] << j) & 0x80) == 0;
                    if (dataIsZero)
                    {
                        durationInTStates += 2 * zeroPulseLength;
                    }
                    else
                    {
                        durationInTStates += 2 * onePulseLength;
                    }
                }
            }
            Duration = durationInTStates / 3500;
            InitLabel(label);
        }

        public override bool GetNextState(ref byte signalLevel, out int tStatesBeforeNextStateChange)
        {
            if ((byteIndex < (Data.Length - 1)) || (byteIndex == (Data.Length - 1) && bitOfTheByteIndex < UsedBitsOfTheLastByte))
            {
                bool dataIsZero = ((Data[byteIndex] << bitOfTheByteIndex) & 0x80) == 0;
                if (dataIsZero)
                {
                    tStatesBeforeNextStateChange = ZeroPulseLength;
                }
                else
                {
                    tStatesBeforeNextStateChange = OnePulseLength;
                }
                if (isFirstPulseOfBit)
                {
                    if (byteIndex > 0 || bitOfTheByteIndex > 0)
                    {
                        Invert(ref signalLevel);
                    }
                    isFirstPulseOfBit = false;
                }
                else
                {
                    Invert(ref signalLevel);
                    isFirstPulseOfBit = true;
                    bitOfTheByteIndex++;
                    if (bitOfTheByteIndex > 7)
                    {
                        byteIndex++;
                        bitOfTheByteIndex = 0;
                    }
                }
                return true;
            }
            else
            {
                Invert(ref signalLevel);
                tStatesBeforeNextStateChange = 0;
                return false;
            }
        }

        public override void ResetPosition()
        {
            byteIndex = 0;
            bitOfTheByteIndex = 0;
            isFirstPulseOfBit = true;
        }

        public override string ToString()
        {
            return "Data : " + ((Data.Length - 1)*8 + UsedBitsOfTheLastByte) + " bit periods, total length = " + Duration + " ms";
        }
    }

    /// <summary>
    /// Samples data. Each bit represents a state on the EAR port (i.e. one sample). MSb is played first. Zeros and ones in 'Direct recording' blocks mean low and high pulse levels respectively. 
    /// The 'current pulse level' after playing a Direct Recording block of CSW recording block is the last level played.                     
    /// </summary>
    public class SamplesSequence : TapeSoundSequence
    {
        public byte[] BitStream { get; private set; }
        public int TStatesPerSample { get; private set; }
        public byte UsedBitsOfTheLastByte { get; private set; }

        private int byteIndex = 0;
        private int bitOfTheByteIndex = 0;

        public SamplesSequence(string label, byte[] bitStream, ushort tStatesPerSample, byte usedBitsOfTheLastByte)
        {
            BitStream = bitStream;
            TStatesPerSample = tStatesPerSample;
            UsedBitsOfTheLastByte = usedBitsOfTheLastByte;

            Duration = ((bitStream.Length - 1) * 8 + usedBitsOfTheLastByte) * TStatesPerSample / 3500;
            InitLabel(label);
        }

        public override bool GetNextState(ref byte signalLevel, out int tStatesBeforeNextStateChange)
        {
            if ((byteIndex < (BitStream.Length - 1)) || (byteIndex == (BitStream.Length - 1) && bitOfTheByteIndex < UsedBitsOfTheLastByte))
            {
                tStatesBeforeNextStateChange = TStatesPerSample;

                bool zeroBit = ((BitStream[byteIndex] << bitOfTheByteIndex) & 0x80) == 0;
                if (zeroBit)
                {
                    signalLevel = 0;
                }
                else
                {
                    signalLevel = 1;
                }
                bitOfTheByteIndex++;
                if (bitOfTheByteIndex > 7)
                {
                    byteIndex++;
                    bitOfTheByteIndex = 0;
                }
                return true;
            }
            else
            {
                tStatesBeforeNextStateChange = 0;
                return false;
            }
        }

        public override void ResetPosition()
        {
            byteIndex = 0;
            bitOfTheByteIndex = 0;
        }

        public override string ToString()
        {
            return "Samples : " + ((BitStream.Length - 1) * 8 + UsedBitsOfTheLastByte) + " samples, sampling frequency = " + 3500 / TStatesPerSample + " kHz";
        }
    }
}
