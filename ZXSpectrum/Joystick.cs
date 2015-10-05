using System;

namespace ZXSpectrum
{
    public enum JoystickVerticalPosition
    {
        Center,       
        Down,
        Up
    }

    public enum JoystickHorizontalPosition
    {
        Center,
        Left,
        Right
    }

    /// <summary>
    /// The 'left' Sinclair joystick maps the joystick directions and the fire button to the 
    /// 1 (left), 2 (right), 3 (down), 4 (up) and 5 (fire) keys on the ZX Spectrum keyboard, 
    /// and can thus be read via port 0xf7fe.
    /// 
    /// The 'right' Sinclair joystick maps to keys 6 (left), 7 (right), 8 (down), 9 (up) and 0 (fire)
    /// and can therefore be read via port 0xeffe.   
    /// 
    /// For any of the joystick interfaces which map to keys, any game offering the appropriate form 
    /// of joystick control can instead be played with the listed keys.
    /// </summary>
    public class Joystick
    {
        private bool isConnectedToLeftPlug;
        private Keyboard computerKeyboard;

        public Joystick(bool isConnectedToLeftPlug, Keyboard computerKeyboard)
        {
            this.isConnectedToLeftPlug = isConnectedToLeftPlug;
            this.computerKeyboard = computerKeyboard;
        }

        private JoystickVerticalPosition   currentVerticalPosition = JoystickVerticalPosition.Center;
        private JoystickHorizontalPosition currentHorizontalPosition = JoystickHorizontalPosition.Center;
        private bool currentButtonPressedState = false;

        public void RefreshCurrentPosition(JoystickVerticalPosition newVerticalPosition, JoystickHorizontalPosition newHorizontalPosition, bool newButtonPressedState)
        {
            if (newVerticalPosition != currentVerticalPosition)
            {
                if (currentVerticalPosition != JoystickVerticalPosition.Center)
                {
                    computerKeyboard.OnKeyRelease(GetKeyForVerticalPosition());
                }
                currentVerticalPosition = newVerticalPosition;
                if (newVerticalPosition != JoystickVerticalPosition.Center)
                {
                    computerKeyboard.OnKeyPress(GetKeyForVerticalPosition());
                }
            }
            if (newHorizontalPosition != currentHorizontalPosition)
            {
                if (currentHorizontalPosition != JoystickHorizontalPosition.Center)
                {
                    computerKeyboard.OnKeyRelease(GetKeyForHorizontalPosition());
                }
                currentHorizontalPosition = newHorizontalPosition;
                if (newHorizontalPosition != JoystickHorizontalPosition.Center)
                {
                    computerKeyboard.OnKeyPress(GetKeyForHorizontalPosition());
                }
            }
            if (newButtonPressedState != currentButtonPressedState)
            {
                if (newButtonPressedState)
                {
                    computerKeyboard.OnKeyPress(GetKeyForButton());
                }
                else 
                {
                    computerKeyboard.OnKeyRelease(GetKeyForButton());
                }
                currentButtonPressedState = newButtonPressedState;
            }
        }

        private Keyboard.Keys GetKeyForVerticalPosition()
        {
            if (isConnectedToLeftPlug)
            {
                switch (currentVerticalPosition)
                {
                    case JoystickVerticalPosition.Down:
                        return Keyboard.Keys.Num3;
                    case JoystickVerticalPosition.Up:
                        return Keyboard.Keys.Num4;
                }
            }
            else
            {
                switch (currentVerticalPosition)
                {
                    case JoystickVerticalPosition.Down:
                        return Keyboard.Keys.Num8;
                    case JoystickVerticalPosition.Up:
                        return Keyboard.Keys.Num9;
                }
            }
            throw new ArgumentOutOfRangeException(currentVerticalPosition.ToString());
        }

        private Keyboard.Keys GetKeyForHorizontalPosition()
        {
            if (isConnectedToLeftPlug)
            {
                switch (currentHorizontalPosition)
                {
                    case JoystickHorizontalPosition.Left:
                        return Keyboard.Keys.Num1;
                    case JoystickHorizontalPosition.Right:
                        return Keyboard.Keys.Num2;
                }
            }
            else
            {
                switch (currentHorizontalPosition)
                {
                    case JoystickHorizontalPosition.Left:
                        return Keyboard.Keys.Num6;
                    case JoystickHorizontalPosition.Right:
                        return Keyboard.Keys.Num7;
                }
            }
            throw new ArgumentOutOfRangeException(currentHorizontalPosition.ToString());
        }

        private Keyboard.Keys GetKeyForButton()
        {
            if (isConnectedToLeftPlug)
            {
                return Keyboard.Keys.Num5;
            }
            else
            {
                return Keyboard.Keys.Num0;
            }
        }
    }
}
