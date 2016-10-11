﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets
{
    public class OurTrack
    {
        protected List<Notes.AbstractNote> _notes;

        public OurTrack()
        {

        }

        public int BPM
        {
            get;
            set;    
        }

        public string Time
        {
            get;
            set;
        }

        public string Tempo
        {
            get;
            set;
        }
    }
}
