using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Notes
{
    class GNote : Note
    {
        public GNote()
        {

        }

        public override string getKey()
        {
            return "g";
        }

        public override object Clone()
        {
            return new GNote();
        }
    }
}
