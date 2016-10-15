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


    }
}
