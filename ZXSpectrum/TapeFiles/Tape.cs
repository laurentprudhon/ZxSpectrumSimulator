using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZXSpectrum.TapeFiles
{
    public class Tape
    {
        /// <summary>
        /// Name of the file from which the tape image was loaded
        /// </summary>
        public string FileName { get; protected set; }

        /// <summary>
        /// Duration of the tape in ms
        /// </summary>
        public int Duration { get; internal set; }

        /// <summary>
        /// Optional description of the contents of the tape
        /// </summary>
        public string Description { get; internal set; }

        /// <summary>
        /// The tape is divided into logical sections
        /// </summary>
        public IList<TapeSection> Sections { get { return _sections; } }
        private IList<TapeSection> _sections = new List<TapeSection>();

        public Tape(string fileName)
        {
            FileName = fileName;
        }

        public Tape(string fileName, params TapeSection[] sections)
        {
            FileName = fileName;
            foreach (TapeSection section in sections)
            {
                Sections.Add(section);
            }
            ComputeDuration();
        }

        public string GetContentsOverview()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Tape file : " + FileName);
            TimeSpan totalDuration = TimeSpan.FromMilliseconds(Duration);
            sb.AppendLine("Total duration : " + totalDuration.Minutes + " min " + totalDuration.Seconds + " sec");
            if(!String.IsNullOrEmpty(Description))
            {
                sb.AppendLine("Description : \"" + Description + "\"");
            }
            foreach(TapeSection section in Sections)
            {
                sb.AppendLine("* " + section.Label);
                foreach (TapeSoundSequence soundSequence in section.SoundSequences)
                {
                    sb.AppendLine("  > " + soundSequence.Label);
                }
            }
            return sb.ToString();
        }

        private void ComputeDuration()
        {
            // Section duration
            Duration = 0;
            foreach (TapeSection section in Sections)
            {
                Duration += section.Duration;
            }
        }
    }
}
