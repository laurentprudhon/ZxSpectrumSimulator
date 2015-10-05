using System;
using System.Collections.Concurrent;
using Z80Simulator.System;

namespace ZXSpectrum
{
    public abstract class Speaker
    {
        /// <summary>
        /// Sound Level : 0 or 1
        /// </summary>
        public AnalogInputPin<byte> SoundSignal { get; private set; }

        /// <summary>
        /// 16 Khz sample clock
        /// 
        /// Notifications : SampleSoundLevel, SampleSoundLevel
        /// </summary>
        public abstract SignalState SoundSampleCLK { get; }

        public Speaker()
        {
            SoundSignal = new AnalogInputPin<byte>();
        }

        // Buffered Sound Output

        private ConcurrentQueue<byte> soundBuffer = new ConcurrentQueue<byte>();

        public void SampleSoundLevel()
        {
            soundBuffer.Enqueue(SoundSignal.Level);
        }

        // Fixed volume level
        private float volume = 0.25f;

        // RC circuit simulation (R= 4.1K, C = 10nF, Interval = 1/15600 sec) 
        private const float timeConstFactor = 0.79f;
        private float currentSoundLevel = 0;

        public void PlaySoundSamples(float[] buffer, int offset, int sampleCount)
        {            
            int availableSamples = soundBuffer.Count;
            if (availableSamples < sampleCount)
            {
                for (int i = 0; i < sampleCount - availableSamples; i++)
                {
                    buffer[offset + i] = currentSoundLevel * volume;
                }
                for (int i = sampleCount - availableSamples; i < sampleCount; i++)
                {
                    byte soundLevel = 0;
                    if (!soundBuffer.TryDequeue(out soundLevel))
                    {
                        break;
                    }
                    if (soundLevel == 0)
                    {
                        buffer[offset + i] = ComputeOutputLevel(0);
                    }
                    else
                    {
                        buffer[offset + i] = ComputeOutputLevel(1);
                    }
                }
            }
            else
            {
                if ((availableSamples - sampleCount) > 500)
                {
                    for (int i = 0; i < (availableSamples - sampleCount - 500); i++)
                    {
                        byte trash = 0;
                        soundBuffer.TryDequeue(out trash);
                    }
                }
                for (int i = 0; i < sampleCount; i++)
                {
                    byte soundLevel = 0;
                    if (!soundBuffer.TryDequeue(out soundLevel))
                    {
                        break;
                    }
                    if (soundLevel == 0)
                    {
                        buffer[offset + i] = ComputeOutputLevel(0);
                    }
                    else
                    {
                        buffer[offset + i] = ComputeOutputLevel(1);
                    }
                }
            }
        }

        private float ComputeOutputLevel(int targetSoundLevel)
        {
            if (targetSoundLevel == 0 && currentSoundLevel > 0)
            {
                currentSoundLevel -= currentSoundLevel * timeConstFactor;
            }
            else if(targetSoundLevel == 1 && currentSoundLevel < 1)
            {
                currentSoundLevel += (1 - currentSoundLevel) * timeConstFactor;
            }
            return currentSoundLevel * volume;
        }
    }
}
