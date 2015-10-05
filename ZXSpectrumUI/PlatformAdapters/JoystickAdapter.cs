using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZXSpectrumUI.PlatformAdapters
{
    public class JoystickAdapter
    {
        private ZXSpectrum.Joystick zxJoystick1;
        private ZXSpectrum.Joystick zxJoystick2;

        public JoystickAdapter(ZXSpectrum.Joystick zxJoystick1, ZXSpectrum.Joystick zxJoystick2)
        {
            this.zxJoystick1 = zxJoystick1;
            this.zxJoystick2 = zxJoystick2;
        }

        private Joystick joystick1;
        public string Joystick1DeviceName { get; private set; }

        private Joystick joystick2;
        public string Joystick2DeviceName { get; private set; }

        public void CaptureJoysticks()
        {
            // Initialize DirectInput
            var directInput = new DirectInput();
            
            // Find all joysticks connected to the system
            IList<DeviceInstance> connectedJoysticks = new List<DeviceInstance>();
            // - look for gamepads
            foreach (var deviceInstance in directInput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices))
            {
                connectedJoysticks.Add(deviceInstance);
            }
            // - look for joysticks
            foreach (var deviceInstance in directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices))
            {
                connectedJoysticks.Add(deviceInstance);
            }
            
            // Use the two first joysticks found
            if (connectedJoysticks.Count >= 1)
            {
                joystick1 = new Joystick(directInput, connectedJoysticks[0].InstanceGuid);
                Joystick1DeviceName = connectedJoysticks[0].InstanceName;
                joystick1.Acquire();
            }
            if (connectedJoysticks.Count >= 2)
            {
                joystick2 = new Joystick(directInput, connectedJoysticks[1].InstanceGuid);
                Joystick2DeviceName = connectedJoysticks[1].InstanceName;
                joystick2.Acquire();
            }
        }

        public void RefreshPosition()
        {
            if (joystick1 != null)
            {
                RefreshJoystickState(joystick1, zxJoystick1);
            }
            if (joystick2 != null)
            {
                RefreshJoystickState(joystick2, zxJoystick2);
            }
        }

        private void RefreshJoystickState(Joystick joystick, ZXSpectrum.Joystick zxJoystick)
        {
            // Set BufferSize in order to use buffered data.
            // joystick.Properties.BufferSize = 128;
            // Poll events from joystick
            // joystick.Poll();
            // var datas = joystick.GetBufferedData();

            joystick.Poll();
            JoystickState currentState = joystick.GetCurrentState();

            ZXSpectrum.JoystickHorizontalPosition newHorizontalPosition = ZXSpectrum.JoystickHorizontalPosition.Center;
            if (currentState.X < 16383)
            {
                newHorizontalPosition = ZXSpectrum.JoystickHorizontalPosition.Left;
            }
            else if (currentState.X > 49150)
            {
                newHorizontalPosition = ZXSpectrum.JoystickHorizontalPosition.Right;
            }
            ZXSpectrum.JoystickVerticalPosition newVerticalPosition = ZXSpectrum.JoystickVerticalPosition.Center;
            if (currentState.Y < 16383)
            {
                newVerticalPosition = ZXSpectrum.JoystickVerticalPosition.Up;
            }
            else if (currentState.Y > 49150)
            {
                newVerticalPosition = ZXSpectrum.JoystickVerticalPosition.Down;
            }
            bool newButtonPressedState = currentState.Buttons[0];

            zxJoystick.RefreshCurrentPosition(newVerticalPosition, newHorizontalPosition, newButtonPressedState);
        }

        public void ReleaseJoysticks()
        {
            if (joystick1 != null)
            {
                Joystick1DeviceName = null;
                joystick1.Unacquire();
                joystick1.Dispose();
                joystick1 = null;
            }
            if (joystick2 != null)
            {
                Joystick2DeviceName = null;
                joystick2.Unacquire();
                joystick2.Dispose();
                joystick2 = null;
            }
        }
    }
}
