using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ZXSpectrumUI.PlatformAdapters
{
    public class KeyboardAdapter
    {
        private ZXSpectrum.Keyboard zxKeyboard;

        public KeyboardAdapter(ZXSpectrum.Keyboard zxKeyboard)
        {
            this.zxKeyboard = zxKeyboard;
            InitKeyboardMap();
        }

        private IDictionary<System.Windows.Input.Key, ZXSpectrum.Keyboard.Keys> keyboardMap;

        private void InitKeyboardMap()
        {
            keyboardMap = new Dictionary<System.Windows.Input.Key, ZXSpectrum.Keyboard.Keys>();

            keyboardMap.Add(System.Windows.Input.Key.NumPad1, ZXSpectrum.Keyboard.Keys.Num1);
            keyboardMap.Add(System.Windows.Input.Key.NumPad2, ZXSpectrum.Keyboard.Keys.Num2);
            keyboardMap.Add(System.Windows.Input.Key.NumPad3, ZXSpectrum.Keyboard.Keys.Num3);
            keyboardMap.Add(System.Windows.Input.Key.NumPad4, ZXSpectrum.Keyboard.Keys.Num4);
            keyboardMap.Add(System.Windows.Input.Key.NumPad5, ZXSpectrum.Keyboard.Keys.Num5);
            keyboardMap.Add(System.Windows.Input.Key.NumPad6, ZXSpectrum.Keyboard.Keys.Num6);
            keyboardMap.Add(System.Windows.Input.Key.NumPad7, ZXSpectrum.Keyboard.Keys.Num7);
            keyboardMap.Add(System.Windows.Input.Key.NumPad8, ZXSpectrum.Keyboard.Keys.Num8);
            keyboardMap.Add(System.Windows.Input.Key.NumPad9, ZXSpectrum.Keyboard.Keys.Num9);
            keyboardMap.Add(System.Windows.Input.Key.NumPad0, ZXSpectrum.Keyboard.Keys.Num0);
            keyboardMap.Add(System.Windows.Input.Key.Q, ZXSpectrum.Keyboard.Keys.Q);
            keyboardMap.Add(System.Windows.Input.Key.W, ZXSpectrum.Keyboard.Keys.W);
            keyboardMap.Add(System.Windows.Input.Key.E, ZXSpectrum.Keyboard.Keys.E);
            keyboardMap.Add(System.Windows.Input.Key.R, ZXSpectrum.Keyboard.Keys.R);
            keyboardMap.Add(System.Windows.Input.Key.T, ZXSpectrum.Keyboard.Keys.T);
            keyboardMap.Add(System.Windows.Input.Key.Y, ZXSpectrum.Keyboard.Keys.Y);
            keyboardMap.Add(System.Windows.Input.Key.U, ZXSpectrum.Keyboard.Keys.U);
            keyboardMap.Add(System.Windows.Input.Key.I, ZXSpectrum.Keyboard.Keys.I);
            keyboardMap.Add(System.Windows.Input.Key.O, ZXSpectrum.Keyboard.Keys.O);
            keyboardMap.Add(System.Windows.Input.Key.P, ZXSpectrum.Keyboard.Keys.P);
            keyboardMap.Add(System.Windows.Input.Key.A, ZXSpectrum.Keyboard.Keys.A);
            keyboardMap.Add(System.Windows.Input.Key.S, ZXSpectrum.Keyboard.Keys.S);
            keyboardMap.Add(System.Windows.Input.Key.D, ZXSpectrum.Keyboard.Keys.D);
            keyboardMap.Add(System.Windows.Input.Key.F, ZXSpectrum.Keyboard.Keys.F);
            keyboardMap.Add(System.Windows.Input.Key.G, ZXSpectrum.Keyboard.Keys.G);
            keyboardMap.Add(System.Windows.Input.Key.H, ZXSpectrum.Keyboard.Keys.H);
            keyboardMap.Add(System.Windows.Input.Key.J, ZXSpectrum.Keyboard.Keys.J);
            keyboardMap.Add(System.Windows.Input.Key.K, ZXSpectrum.Keyboard.Keys.K);
            keyboardMap.Add(System.Windows.Input.Key.L, ZXSpectrum.Keyboard.Keys.L);
            keyboardMap.Add(System.Windows.Input.Key.Enter, ZXSpectrum.Keyboard.Keys.Enter);
            keyboardMap.Add(System.Windows.Input.Key.LeftShift, ZXSpectrum.Keyboard.Keys.CapsShift);
            keyboardMap.Add(System.Windows.Input.Key.RightShift, ZXSpectrum.Keyboard.Keys.CapsShift);
            keyboardMap.Add(System.Windows.Input.Key.Z, ZXSpectrum.Keyboard.Keys.Z);
            keyboardMap.Add(System.Windows.Input.Key.X, ZXSpectrum.Keyboard.Keys.X);
            keyboardMap.Add(System.Windows.Input.Key.C, ZXSpectrum.Keyboard.Keys.C);
            keyboardMap.Add(System.Windows.Input.Key.V, ZXSpectrum.Keyboard.Keys.V);
            keyboardMap.Add(System.Windows.Input.Key.B, ZXSpectrum.Keyboard.Keys.B);
            keyboardMap.Add(System.Windows.Input.Key.N, ZXSpectrum.Keyboard.Keys.N);
            keyboardMap.Add(System.Windows.Input.Key.M, ZXSpectrum.Keyboard.Keys.M);
            keyboardMap.Add(System.Windows.Input.Key.LeftCtrl, ZXSpectrum.Keyboard.Keys.SymbolShift);
            keyboardMap.Add(System.Windows.Input.Key.Space, ZXSpectrum.Keyboard.Keys.Space);
        }

        public void OnKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            System.Windows.Input.Key windowsKey = e.Key;
            ZXSpectrum.Keyboard.Keys zxKey;

            // Single keys
            if (keyboardMap.TryGetValue(windowsKey, out zxKey))
            {
                zxKeyboard.OnKeyPress(zxKey);
            }
            // Combined shortcuts
            else
            {
                if (windowsKey == System.Windows.Input.Key.Back)
                {
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.CapsShift);
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Num0);
                }               
                else if (windowsKey == System.Windows.Input.Key.Left)
                {
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.CapsShift);
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Num5);
                }
                else if (windowsKey == System.Windows.Input.Key.Down)
                {
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.CapsShift);
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Num6);
                }
                else if (windowsKey == System.Windows.Input.Key.Up)
                {
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.CapsShift);
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Num7);
                }
                else if (windowsKey == System.Windows.Input.Key.Right)
                {
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.CapsShift);
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Num8);
                }
                else if (windowsKey == System.Windows.Input.Key.CapsLock)
                {
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.CapsShift);
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Num2);
                }
                else if (windowsKey == System.Windows.Input.Key.RightCtrl)
                {
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.CapsShift);
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.SymbolShift);
                }
                else if (windowsKey == System.Windows.Input.Key.Divide)
                {
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.SymbolShift);
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.V);
                }
                else if (windowsKey == System.Windows.Input.Key.Multiply)
                {
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.SymbolShift);
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.B);
                }
                else if (windowsKey == System.Windows.Input.Key.Add)
                {
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.SymbolShift);
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.K);
                }
                else if (windowsKey == System.Windows.Input.Key.Subtract)
                {
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.SymbolShift);
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.J);
                }
                else if (windowsKey == System.Windows.Input.Key.Decimal)
                {
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.SymbolShift);
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.M);
                }
                else if (windowsKey == System.Windows.Input.Key.Escape)
                {
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.CapsShift);
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Space);
                }
                else if (windowsKey == System.Windows.Input.Key.F1)
                {
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.CapsShift);
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Num1);
                }
                else if (windowsKey == System.Windows.Input.Key.F2)
                {
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.CapsShift);
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Num2);
                }
                else if (windowsKey == System.Windows.Input.Key.F3)
                {
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.CapsShift);
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Num3);
                }
                else if (windowsKey == System.Windows.Input.Key.F4)
                {
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.CapsShift);
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Num4);
                }
                else if (windowsKey == System.Windows.Input.Key.F5)
                {
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.CapsShift);
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Num5);
                }
                else if (windowsKey == System.Windows.Input.Key.F6)
                {
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.CapsShift);
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Num6);
                }
                else if (windowsKey == System.Windows.Input.Key.F7)
                {
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.CapsShift);
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Num7);
                }
                else if (windowsKey == System.Windows.Input.Key.F8)
                {
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.CapsShift);
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Num8);
                }
                else if (windowsKey == System.Windows.Input.Key.F9)
                {
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.CapsShift);
                    zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Num9);
                }
                // French Logitech keyboard map
                //  !	Oem8
                //  :	Oem2
                //  ;	OemPeriod
                //  ,	OemComma
                //  <	Oem102
                //   *	Oem5
                //  ù	Oem3
                //  ^	Oem6
                //  $	DeadCharProcessed
                //  F1	F1
                //  )	Oem4
                //  =	OemPlus
                else
                {
                    char c = GetCharFromKey(windowsKey);
                    switch (c)
                    {
                        case '!':
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Num1);
                            break;
                        case '@':
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Num2);
                            break;
                        case '#':
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Num3);
                            break;
                        case '$':
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Num4);
                            break;
                        case '%':
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Num5);
                            break;
                        case '&':
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Num6);
                            break;
                        case '\'':
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Num7);
                            break;
                        case '(':
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Num8);
                            break;
                        case ')':
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Num9);
                            break;
                        case '_':
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Num0);
                            break;
                        case '<':
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.R);
                            break;
                        case '>':
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.T);
                            break;
                        case ';':
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.O);
                            break;
                        case '"':
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.P);
                            break;
                        case '-':
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.J);
                            break;
                        case '+':
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.K);
                            break;
                        case '=':
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.L);
                            break;
                        case ':':
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Z);
                            break;
                        case '£':
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.X);
                            break;
                        case '?':
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.C);
                            break;
                        case '/':
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.V);
                            break;
                        case '*':
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.B);
                            break;
                        case ',':
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.N);
                            break;
                        case '.':
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.M);
                            break;
                        case '1':
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Num1);
                            break;
                        case '2':
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Num2);
                            break;
                        case '3':
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Num3);
                            break;
                        case '4':
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Num4);
                            break;
                        case '5':
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Num5);
                            break;
                        case '6':
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Num6);
                            break;
                        case '7':
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Num7);
                            break;
                        case '8':
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Num8);
                            break;
                        case '9':
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Num9);
                            break;
                        case '0':
                            zxKeyboard.OnKeyPress(ZXSpectrum.Keyboard.Keys.Num0);
                            break;
                    }
                }
            }
        }

        public void OnKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            System.Windows.Input.Key windowsKey = e.Key;
            ZXSpectrum.Keyboard.Keys zxKey;

            // Single keys
            if (keyboardMap.TryGetValue(windowsKey, out zxKey))
            {
                zxKeyboard.OnKeyRelease(zxKey);
            }
            // Combined shortcuts
            else
            {
                if (windowsKey == System.Windows.Input.Key.Back)
                {
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.CapsShift);
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Num0);
                }
                else if (windowsKey == System.Windows.Input.Key.Left)
                {
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.CapsShift);
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Num5);
                }
                else if (windowsKey == System.Windows.Input.Key.Down)
                {
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.CapsShift);
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Num6);
                }
                else if (windowsKey == System.Windows.Input.Key.Up)
                {
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.CapsShift);
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Num7);
                }
                else if (windowsKey == System.Windows.Input.Key.Right)
                {
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.CapsShift);
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Num8);
                }
                else if (windowsKey == System.Windows.Input.Key.CapsLock)
                {
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.CapsShift);
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Num2);
                }
                else if (windowsKey == System.Windows.Input.Key.RightCtrl)
                {
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.CapsShift);
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.SymbolShift);
                }
                else if (windowsKey == System.Windows.Input.Key.Divide)
                {
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.SymbolShift);
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.V);
                }
                else if (windowsKey == System.Windows.Input.Key.Multiply)
                {
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.SymbolShift);
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.B);
                }
                else if (windowsKey == System.Windows.Input.Key.Add)
                {
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.SymbolShift);
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.K);
                }
                else if (windowsKey == System.Windows.Input.Key.Subtract)
                {
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.SymbolShift);
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.J);
                }
                else if (windowsKey == System.Windows.Input.Key.Decimal)
                {
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.SymbolShift);
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.M);
                }
                else if (windowsKey == System.Windows.Input.Key.Escape)
                {
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.CapsShift);
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Space);
                }
                else if (windowsKey == System.Windows.Input.Key.F1)
                {
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.CapsShift);
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Num1);
                }
                else if (windowsKey == System.Windows.Input.Key.F2)
                {
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.CapsShift);
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Num2);
                }
                else if (windowsKey == System.Windows.Input.Key.F3)
                {
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.CapsShift);
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Num3);
                }
                else if (windowsKey == System.Windows.Input.Key.F4)
                {
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.CapsShift);
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Num4);
                }
                else if (windowsKey == System.Windows.Input.Key.F5)
                {
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.CapsShift);
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Num5);
                }
                else if (windowsKey == System.Windows.Input.Key.F6)
                {
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.CapsShift);
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Num6);
                }
                else if (windowsKey == System.Windows.Input.Key.F7)
                {
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.CapsShift);
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Num7);
                }
                else if (windowsKey == System.Windows.Input.Key.F8)
                {
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.CapsShift);
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Num8);
                }
                else if (windowsKey == System.Windows.Input.Key.F9)
                {
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.CapsShift);
                    zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Num9);
                }
                // French Logitech keyboard map
                //  !	Oem8
                //  :	Oem2
                //  ;	OemPeriod
                //  ,	OemComma
                //  <	Oem102
                //   *	Oem5
                //  ù	Oem3
                //  ^	Oem6
                //  $	DeadCharProcessed
                //  F1	F1
                //  )	Oem4
                //  =	OemPlus
                else
                {
                    char c = GetCharFromKey(windowsKey);
                    switch (c)
                    {
                        case '!':
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Num1);
                            break;
                        case '@':
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Num2);
                            break;
                        case '#':
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Num3);
                            break;
                        case '$':
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Num4);
                            break;
                        case '%':
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Num5);
                            break;
                        case '&':
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Num6);
                            break;
                        case '\'':
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Num7);
                            break;
                        case '(':
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Num8);
                            break;
                        case ')':
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Num9);
                            break;
                        case '_':
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Num0);
                            break;
                        case '<':
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.R);
                            break;
                        case '>':
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.T);
                            break;
                        case ';':
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.O);
                            break;
                        case '"':
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.P);
                            break;
                        case '-':
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.J);
                            break;
                        case '+':
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.K);
                            break;
                        case '=':
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.L);
                            break;
                        case ':':
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Z);
                            break;
                        case '£':
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.X);
                            break;
                        case '?':
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.C);
                            break;
                        case '/':
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.V);
                            break;
                        case '*':
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.B);
                            break;
                        case ',':
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.N);
                            break;
                        case '.':
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.SymbolShift);
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.M);
                            break;
                        case '1':
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Num1);
                            break;
                        case '2':
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Num2);
                            break;
                        case '3':
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Num3);
                            break;
                        case '4':
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Num4);
                            break;
                        case '5':
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Num5);
                            break;
                        case '6':
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Num6);
                            break;
                        case '7':
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Num7);
                            break;
                        case '8':
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Num8);
                            break;
                        case '9':
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Num9);
                            break;
                        case '0':
                            zxKeyboard.OnKeyRelease(ZXSpectrum.Keyboard.Keys.Num0);
                            break;
                    }
                }
            }
        }

        // --- Direct calls to Win32 API to get the Unicode char for any Key pressed ---

        public enum MapType : uint
        {
            MAPVK_VK_TO_VSC = 0x0,
            MAPVK_VSC_TO_VK = 0x1,
            MAPVK_VK_TO_CHAR = 0x2,
            MAPVK_VSC_TO_VK_EX = 0x3,
        }

        [DllImport("user32.dll")]
        public static extern int ToUnicode(
            uint wVirtKey,
            uint wScanCode,
            byte[] lpKeyState,
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeParamIndex = 4)] 
            StringBuilder pwszBuff,
            int cchBuff,
            uint wFlags);

        [DllImport("user32.dll")]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, MapType uMapType);

        public static char GetCharFromKey(System.Windows.Input.Key key)
        {
            char ch = ' ';

            int virtualKey = System.Windows.Input.KeyInterop.VirtualKeyFromKey(key);
            byte[] keyboardState = new byte[256];
            GetKeyboardState(keyboardState);

            uint scanCode = MapVirtualKey((uint)virtualKey, MapType.MAPVK_VK_TO_VSC);
            StringBuilder stringBuilder = new StringBuilder(2);

            int result = ToUnicode((uint)virtualKey, scanCode, keyboardState, stringBuilder, stringBuilder.Capacity, 0);
            switch (result)
            {
                case -1:
                    break;
                case 0:
                    break;
                case 1:
                    {
                        ch = stringBuilder[0];
                        break;
                    }
                default:
                    {
                        ch = stringBuilder[0];
                        break;
                    }
            }
            return ch;
        }
    }
}
