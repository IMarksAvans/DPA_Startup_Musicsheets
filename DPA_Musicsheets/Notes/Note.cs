using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Notes
{
    public abstract class Note : ICloneable, IGetKey<String>
    {

        public int Duration { get; set; }
        public int Octave { get; internal set;} = 5;
        //protected string _maatsoort; // * Maatsoort is not related to a note but to a track.
        protected bool _muted;
        protected bool _punt;
        protected string _hogerlager; // * Not sure what this should intents to do.

        public Note()
        {
        }

        public static Note create(String name)
        {
            return FactoryMethod<String, Note>.create(name);
        }

        public abstract String getKey();

        public abstract object Clone();

        public bool IsMuted()
        {
            return false;
        }

        public bool IsPunt()
        {
            return false;
        }

        public void getOctave()
        {

        }

        public void DecreaseOctave()
        {
            Octave--;
        }

        public void IncreaseOctave()
        {
            Octave++;
        }

        //public string HogerLager();
    }
}
