using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Notes
{
    class TieNote: Note
    {
        public TieNote()
        {

        }

        public override string getKey()
        {
            return "~";
        }

        public override object Clone()
        {
            return new TieNote();
        }
    }
}
