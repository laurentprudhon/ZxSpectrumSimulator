using System;
using ZXSpectrum;

namespace ZXSpectrumUI.PlatformAdapters
{
    public class WPFSpectrum : ZXSpectrumComputer
    {
        public KeyboardAdapter KeyboardAdapter { get; private set; }
        public JoystickAdapter JoystickAdapter { get; private set; }
        public ScreenAdapter ScreenAdapter { get; private set; }
        public SpeakerAdapter SpeakerAdapter { get; private set; }

        public WPFSpectrum()
        {
            KeyboardAdapter = new KeyboardAdapter(this.Keyboard);
            JoystickAdapter = new JoystickAdapter(this.Joystick1, this.Joystick2);
            ScreenAdapter = new ScreenAdapter(this.Screen);
            SpeakerAdapter = new SpeakerAdapter(this.Speaker);
        }
    }
}
