using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets
{
    public class OurTrack
    {
        public List<Notes.Note> Notes
        {
            get;
            set;
        }

        public OurTrack()
        {
            this.Octave = 5;
            this.Relative = ' ';
            this.Metronome = 0;
            this.Pitch = "";
            this.Tempo = 0;
            this.Time = 0;

        }

        public bool IsRepeat
        {
            get;
            set;
        }

        public bool IsAlternative
        {
            get;
            set;
        }

        public int BPM
        {
            get;
            set;    
        }

        public double Time
        {
            get;
            set;
        }

        public int Metronome
        {
            get;
            set;
        }
        public int Octave { get; internal set; }
        public char Relative { get; internal set; }
        public string Pitch { get; internal set; }
        public int Tempo { get; internal set; }
        public bool InRepeat { get; internal set; }
        public bool InAlternative { get; internal set; }
    }
}
