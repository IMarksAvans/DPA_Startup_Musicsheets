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
        public bool Muted { get; set; } = false;
        public bool Punt { get; set; } = false;
        //protected string _hogerlager; // * Not sure what this should intents to do.

        public bool IsSharp
        {
            get;
            set;
        } = false;

        public bool IsFlat
        {
            get;
            set;
        } = false;

        public int TicksPosition
        {
            get;
            set;
        } = 0;

        public int NotePos
        {
            get;
            set;
        } = 0;

        public Note()
        {
        }

        public static Note create(String name)
        {
            return FactoryMethod<String, Note>.create(name);
        }

        public abstract String getKey();

        public abstract object Clone();

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
