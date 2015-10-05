using System;
using NAudio.Wave;

namespace ZXSpectrumUI.PlatformAdapters
{
    public class SpeakerAdapter : WaveProvider32
    {
        private ZXSpectrum.Speaker zxSpeaker;

        public SpeakerAdapter(ZXSpectrum.Speaker zxSpeaker)
        {
            this.zxSpeaker = zxSpeaker;
            
            SetWaveFormat(15600, 1); // 16kHz mono    
        }
       
        public override int Read(float[] buffer, int offset, int sampleCount) 
        { 
            zxSpeaker.PlaySoundSamples(buffer, offset, sampleCount);
            return sampleCount;
        }
    }
}
