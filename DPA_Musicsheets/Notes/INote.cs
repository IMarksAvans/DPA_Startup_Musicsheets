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
    }
}
