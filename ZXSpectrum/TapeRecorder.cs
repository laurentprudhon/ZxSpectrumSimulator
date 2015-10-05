using System;
using System.IO;
using Z80Simulator.System;
using ZXSpectrum.TapeFiles;

namespace ZXSpectrum
{
    public abstract class TapeRecorder
    {
        /// <summary>
        /// Common time base with the ZX Spectrum
        /// 
        /// Notifications : Tick, null
        /// </summary>
        public abstract SignalState TimeCLK { get; }

        /// <summary>
        /// One byte represents the sound level produced while playing the tape (0 or 1)
        /// </summary>
        public AnalogOutputPin<byte> SoundSignal { get; private set; }

        public TapeRecorder()
        {
            SoundSignal = new AnalogOutputPin<byte>(0);
        }

        private const byte STATE_STOPPED = 0;
        private const byte STATE_PLAYING = 1;
        private const byte STATE_RECORDING = 2;
        private byte recorderState = STATE_STOPPED;

        public Tape Tape { get; private set; }

        public int PlayingTime { get; private set; }
        private int nextStateChangeCountdown = 0;

        private int currentSectionIndex = 0;
        private TapeSection currentSection = null;

        private int currentSoundSequenceIndex = 0;
        private TapeSoundSequence currentSoundSequence = null;

        private int playingTimeBeforeStop = 0;
        private byte soundSignalBeforeStop = 0;

        private void SetCurrentSection(int targetSectionIndex, bool adjustPlayingTime)
        {
            currentSectionIndex = targetSectionIndex;
            currentSection = Tape.Sections[currentSectionIndex];

            if (adjustPlayingTime)
            {
                int duration = 0;
                for (int i = 0; i < currentSectionIndex; i++)
                {
                    duration += Tape.Sections[i].Duration;
                }
                PlayingTime = duration * 3500;
            }

            SetCurrentSoundSequence(0);
        }

        private void SetCurrentSoundSequence(int targetSoundSequenceIndex)
        {
            currentSoundSequenceIndex = targetSoundSequenceIndex;
            currentSoundSequence = currentSection.SoundSequences[currentSoundSequenceIndex];

            currentSoundSequence.ResetPosition();
            currentSoundSequence.GetNextState(ref SoundSignal.Level, out nextStateChangeCountdown);
        }

        public void InsertTape(Tape insertedTape)
        {
            if(Tape != null)
            {
                Eject();
            }

            Tape = insertedTape;

            RewindToStart();
        }

        public void InsertTape(Stream inputStream, string fileName)
        {
            Tape insertedTape = null;

            string extension = fileName.Substring(fileName.LastIndexOf('.') + 1);
            if (extension.ToLower() == "tap")
            {
                insertedTape = TapFileReader.ReadTapFile(inputStream, fileName);
            }
            else if (extension.ToLower() == "tzx")
            {
                insertedTape = TzxFileReader.ReadTzxFile(inputStream, fileName);
            }
            else
            {
                throw new NotSupportedException("Only .tap and .tzx file extensions are suported");
            }

            InsertTape(insertedTape);
        }


        public void Eject()
        {
            Stop();

            Tape = null;
            PlayingTime = 0;
            nextStateChangeCountdown = 0;
            currentSectionIndex = 0;
            currentSection = null;
            currentSoundSequenceIndex = 0;
            currentSoundSequence = null;
        }

        public void Start()
        {
            if(Tape != null)
            {
                // Restore sound level in case of Stop/Start
                if (PlayingTime == playingTimeBeforeStop)
                {
                    SoundSignal.Level = soundSignalBeforeStop;
                }

                recorderState = STATE_PLAYING;
            }
        }

        public void Stop()
        {
            // Save sound level before stop
            playingTimeBeforeStop = PlayingTime;
            soundSignalBeforeStop = SoundSignal.Level;

            // Sound level is zero while the tape recorder is stopped
            SoundSignal.Level = 0;
            recorderState = STATE_STOPPED;
        }

        public void RewindOrForward(int targetSectionIndex)
        {
            Stop();

            if(Tape != null)
            {                
                SetCurrentSection(targetSectionIndex, true);                
            }
        }        

        public void RewindToStart()
        {
            Stop();

            if(Tape != null)
            {
                SetCurrentSection(0, true);
            }
        }

        // int lastChangeTime;

        public void Tick()
        {            
            if (recorderState == STATE_PLAYING)
            {
                PlayingTime++;
                nextStateChangeCountdown--;
                if (nextStateChangeCountdown == 0)
                {
                    if (!currentSoundSequence.GetNextState(ref SoundSignal.Level, out nextStateChangeCountdown))
                    {
                        currentSoundSequenceIndex++;
                        if (currentSoundSequenceIndex < currentSection.SoundSequences.Count)
                        {
                            SetCurrentSoundSequence(currentSoundSequenceIndex);
                        }
                        else
                        {
                            currentSectionIndex++;
                            if (currentSectionIndex < Tape.Sections.Count)
                            {
                                SetCurrentSection(currentSectionIndex, false);
                            }
                            else
                            {
                                Stop();
                            }
                        }
                    }
                    //Console.WriteLine((PlayingTime - lastChangeTime) + " : " + SoundSignal.Level);
                    //lastChangeTime = PlayingTime;
                }
            }
        }

        public string GetState()
        {
            if (Tape == null)
            {
                return "No tape inserted";
            }
            else
            {
                string status = Tape.FileName;
                TimeSpan totalDuration = TimeSpan.FromMilliseconds(Tape.Duration);
                status += " (" + totalDuration.Minutes + " min " + totalDuration.Seconds + " sec) : ";
                switch (recorderState)
                {
                    case STATE_PLAYING:
                        status += "playing.";
                        break;
                    case STATE_RECORDING:
                        status += "recording.";
                        break;
                    default:
                        status += "stopped.";
                        break;
                }
                return status;
            }
        }

        public string GetPosition()
        {
            string position = "Position : ";
            position += (PlayingTime / 3500000) + " sec";
            if (recorderState == STATE_PLAYING)
            {
                if (currentSection != null)
                {
                    position += " => " + currentSection.Label ;
                }
                if (currentSoundSequence != null)
                {
                    position += " / " + currentSoundSequence.Label;
                }
            }
            return position;
        }

        public override string ToString()
        {
            return GetState() + "|" + GetPosition() + "|" + PlayingTime + "|" + SoundSignal.Level;
        }
    }
}
