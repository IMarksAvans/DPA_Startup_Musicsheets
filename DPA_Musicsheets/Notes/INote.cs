using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Factory;

namespace DPA_Musicsheets.Notes
{
    public abstract class INote : ICloneable, IGetKey<String>
    {

        protected int _duration;
        protected string _maatsoort;
        protected bool _muted;
        protected bool _punt;
        protected string _hogerlager;

        public INote()
        {
        }

        public static INote create(String name)
        {
            return FactoryMethod<String, INote>.create(name);
        }

        public abstract String getKey();

        public abstract object Clone();

        public abstract void show();

        public abstract int GetDuration();

        public abstract string GetMaatsoort();

        public abstract bool IsMuted();

        public abstract bool IsPunt();

        public abstract string HogerLager();
    }
}
