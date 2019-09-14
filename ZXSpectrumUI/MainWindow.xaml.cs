using ICSharpCode.AvalonEdit.Document;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using ZXSpectrum;
using ZXSpectrumUI.PlatformAdapters;
using ZXSpectrumUI.TextEditor;

namespace ZXSpectrumUI
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Wave out device used to play sound
        private WaveOut waveOut;   

        // WPF specific version of the ZX Spectrum simulator
        private WPFSpectrum zxSpectrum;

        public MainWindow()
        {
            InitializeComponent();
            this.Closing += MainWindow_Closing;

            // Initialize ZX Spectrum simulator
            zxSpectrum = new WPFSpectrum();

            // Display ROM program
            TextEditor.SyntaxHighlighting = TextEditorConfig.LoadAsmHighlightingDefinition();
            TextEditor.Text = zxSpectrum.ROM.Program.ToString();
            
            // Forward keystrokes to ZX Spectrum keyboard
            this.KeyDown += zxSpectrum.KeyboardAdapter.OnKeyDown;
            this.KeyUp += zxSpectrum.KeyboardAdapter.OnKeyUp;
        
            // Capture joystick moves
            zxSpectrum.JoystickAdapter.CaptureJoysticks();

            // Display ZX Spectrum screen through an Image UI element
            this.Screen.Source = zxSpectrum.ScreenAdapter.ImageSource;

            // Play ZX Spectrum sound through a wave out device
            waveOut = new WaveOut();         
            waveOut.Init(zxSpectrum.SpeakerAdapter);
            waveOut.Play();

            frameWatch.Start();
            lastDisplayTime = frameWatch.ElapsedMilliseconds;

            tapeState.Text = zxSpectrum.TapeRecorder.GetState();

            // Activate debug mode from a certain location
            // startDebugAddress = 0x0605; //(ushort)zxSpectrum.ROM.Program.GetAddressFromLine(1825);// 0x0552;
            // zxSpectrum.CPU.InstructionEnd += CPU_InstructionEnd;
            // zxSpectrum.TapeRecorder.InsertTape(File.OpenRead(@"C:\Users\Laurent\SkyDrive\Dev\Visual Studio 2012\Projects\Z80VisualSimulator\ZXSPectrum.Test\TapeFiles\SampleTzxFiles\BuggyBoy.tzx"), "BuggyBoy.tzx");

            Task.Factory.StartNew(() =>
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                while (true)
                {
                    if (framesCount == 500)
                    {
                        watch.Restart();
                        framesCount = 0;
                    }
                    long timeBefore = watch.ElapsedMilliseconds;
                    for (int i = 0; i < 5; i++)
                    {
                        zxSpectrum.ExecuteFor20Milliseconds();
                        framesCount++;
                        Dispatcher.Invoke(this.RefreshScreen);
                    }
                    zxSpectrum.JoystickAdapter.RefreshPosition();
                    long timeAfter = watch.ElapsedMilliseconds;
                    fiveFramesDurationMs = timeAfter - timeBefore;
                    long deviation = (framesCount*20) - timeAfter;

                    // Adjust to real speed if it runs too fast
                    if (deviation > 0)
                    {
                        Thread.Sleep((int)(deviation));
                    }
                }

            });
        }

        Stopwatch frameWatch = new Stopwatch();
        long lastDisplayTime = 0;
        int lastFrameCount = 0;
        
        public int framesCount = 0;
        long fiveFramesDurationMs = 200;

        public void RefreshScreen()
        {    
            zxSpectrum.ScreenAdapter.RefreshScreen();

            long currentTime = frameWatch.ElapsedMilliseconds;
            if (currentTime - lastDisplayTime >= 1000)
            {
                if (framesCount > lastFrameCount)
                {
                    frameRate.Text = (framesCount - lastFrameCount) + " frames/sec (max speed : " + (int)(100.0 / fiveFramesDurationMs * 100) + "%)";
                }
                lastFrameCount = framesCount;
                lastDisplayTime = currentTime;

                tapePosition.Text = zxSpectrum.TapeRecorder.GetPosition();
            }
        }

        private ushort startDebugAddress;
        private bool isDebugging = false;

        //long lastTStateCounter;
        //int lastPlayingTime;

        private void CPU_InstructionEnd(Z80Simulator.CPU.Z80CPU.InternalState currentState, Z80Simulator.CPU.Z80CPU.LifecycleEventType eventType)
        {
            if (!isDebugging)
            {
                isDebugging = (currentState.InstructionOrigin.Address == startDebugAddress);
                //isDebugging = currentState.Instruction.Type.Index == 43 && zxSpectrum.CPU.A == 95;
            }
            else
            {
                /*if(currentState.Instruction.Type.Index == 43)
                {
                    string debugMessage = (zxSpectrum.TapeRecorder.PlayingTime - lastPlayingTime) + ":" + (currentState.TStateCounter - lastTStateCounter) + "/" + zxSpectrum.TapeRecorder.SoundSignal.Level + " : A = " + zxSpectrum.CPU.A;
                    lastPlayingTime = zxSpectrum.TapeRecorder.PlayingTime;
                    lastTStateCounter = currentState.TStateCounter;
                    Console.WriteLine(debugMessage);
                    //Dispatcher.Invoke(() => debugOutput.Text = debugMessage);                    
                }
                if (currentState.Instruction.Type.Index == 21)
                {
                    string debugMessage = "CP : " + zxSpectrum.CPU.A + " / " + currentState.InstructionOrigin.OpCode.Param1;
                    Console.WriteLine(debugMessage);
                    //Dispatcher.Invoke(() => debugOutput.Text = debugMessage);
                }
                if (currentState.Instruction.Type.Index == 22)
                {
                    string debugMessage = zxSpectrum.CPU.H + " - CP : " + zxSpectrum.CPU.A + " / " + zxSpectrum.CPU.B;
                    Console.WriteLine(debugMessage);
                    /*if (zxSpectrum.CPU.A > zxSpectrum.CPU.B)
                    {
                        Thread.Sleep(30000);
                    }*/
                    //Dispatcher.Invoke(() => debugOutput.Text = debugMessage);
                //}
                if (currentState.InstructionOrigin.Address == 1454)
                {
                    // LD      (IX+$00),L      ; place loaded byte at memory location.
                    Console.WriteLine("LD      ("+zxSpectrum.CPU.IX+"),"+zxSpectrum.CPU.L);
                    //Thread.Sleep(10000);
                }
                if (currentState.InstructionOrigin.Address == 1488)
                {
                    Console.WriteLine("B=" + zxSpectrum.CPU.B + " => " + ((zxSpectrum.CPU.B<=203)?"0":"1"));
                }
                if (currentState.InstructionOrigin.Address == 1503)
                {
                    // LD      A,H             ; fetch parity byte => should be 0
                    Console.WriteLine("LD      A," + zxSpectrum.CPU.H);
                }

               /* if ((currentState.InstructionOrigin.Address >= 1503 && currentState.InstructionOrigin.Address < 1506) ||
                    
                    (currentState.InstructionOrigin.Address >= 0x0760 && currentState.InstructionOrigin.Address < 0x09F4))
                {
                    currentROMLine = zxSpectrum.ROM.Program.GetLineFromAddress(currentState.InstructionOrigin.Address).LineNumber;
                    Dispatcher.Invoke(this.DisplayROMLine);
                    Thread.Sleep(2000);
                }*/
            }
        }

        public int currentROMLine = 0;

        public void DisplayROMLine()
        {
            TextEditor.ScrollToLine(currentROMLine);
            DocumentLine line = TextEditor.Document.Lines[currentROMLine - 1];
            TextEditor.Select(line.Offset, line.Length);
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Stop playing sound
            waveOut.Stop();

            // Free wave out device
            waveOut.Dispose();         
            waveOut = null;

            // Release captured joysticks
            zxSpectrum.JoystickAdapter.ReleaseJoysticks();
        }

        private void KeyboardImage_Click(object sender, RoutedEventArgs e)
        {
            this.Focus();
        }

        private void TapeRecorder_InsertTape(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            DirectoryInfo initialDir = new DirectoryInfo(@"..\..\Games");
            if(!initialDir.Exists)
            {
                initialDir = new DirectoryInfo(".");
            }
            dlg.InitialDirectory = initialDir.FullName;
            dlg.DefaultExt = ".tzx";
            dlg.Filter = "TZX Files (*.tzx)|*.tzx|TAP Files (*.tap)|*.tap";

            // Display OpenFileDialog by calling ShowDialog method 
            bool? fileSelected = dlg.ShowDialog();
            if (fileSelected.HasValue && fileSelected.Value)
            {
                FileInfo tapeFileInfo = new FileInfo(dlg.FileName);
                using(Stream stream = File.Open(dlg.FileName, FileMode.Open))
                {
                    zxSpectrum.TapeRecorder.InsertTape(stream, tapeFileInfo.Name);
                }
                tapeState.Text = zxSpectrum.TapeRecorder.GetState();
            }
        }

        private void TapeRecorder_Start(object sender, RoutedEventArgs e)
        {
            zxSpectrum.TapeRecorder.Start();
            tapeState.Text = zxSpectrum.TapeRecorder.GetState();
        }

        private void TapeRecorder_RewindToStart(object sender, RoutedEventArgs e)
        {
            zxSpectrum.TapeRecorder.RewindToStart();
            tapeState.Text = zxSpectrum.TapeRecorder.GetState();
        }

        private void TapeRecorder_Stop(object sender, RoutedEventArgs e)
        {
            zxSpectrum.TapeRecorder.Stop();
            tapeState.Text = zxSpectrum.TapeRecorder.GetState();
        }

        private void Reset(object sender, RoutedEventArgs e)
        {
            zxSpectrum.ResetButton.Push();
            Thread.Sleep(10);
            zxSpectrum.ResetButton.Release();
        }
    }
}
