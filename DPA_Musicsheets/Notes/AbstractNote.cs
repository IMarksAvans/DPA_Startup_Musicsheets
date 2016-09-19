using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Factory;

namespace DPA_Musicsheets.Notes
{
    public abstract class AbstractNote : ICloneable, IGetKey<String>
    {

        protected int Duration { get; set; }
        //protected string _maatsoort; // * Maatsoort is not related to a note but to a track.
        protected bool _muted;
        protected bool _punt;
        protected string _hogerlager; // * Not sure what this should intents to do.

        public AbstractNote()
        {
        }

        public static AbstractNote create(String name)
        {
            return FactoryMethod<String, AbstractNote>.create(name);
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

        //public string HogerLager();
    }
}
