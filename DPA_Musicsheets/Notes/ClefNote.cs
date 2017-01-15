using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Notes
{
    class ClefNote : Note
    {
        public ClefNote()
        {

        }

        public override string getKey()
        {
            return "clef";
        }

        public override object Clone()
        {
            return new ClefNote();
        }
    }
}
