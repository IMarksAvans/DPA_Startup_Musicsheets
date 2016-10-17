using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets
{
    public class Song
    {
        public int Octave
        {
            get;
            set;
        }

        public char Relative
        {
            get;
            set;
        }

        public List<OurTrack> Tracks
        {
            get;
            set;
        }

        public int Tempo
        {
            get;
            set;
        }

        public double Time
        {
            get;
            set;
        }

        public string Pitch
        {
            get;
            set;
        }

        public int Metronome
        {
            get;
            set;
        }

        public bool InRepeat
        {
            get;
            set;
        }

        public bool InAlternative
        {
            get;
            set;
        }

        public Song()
        {
            this.Octave = 5;
            this.Relative = ' ';
            this.Metronome = 0;
            this.Pitch = "";
            this.Tempo = 0;
            this.Time = 0;
            this.Tracks = new List<OurTrack>();
        }
    }
}
