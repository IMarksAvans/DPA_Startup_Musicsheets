using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Notes
{
    class GNote : AbstractNote
    {
        public GNote()
        {

        }

        public override string getKey()
        {
            return "G";
        }

        public override object Clone()
        {
            return new GNote();
        }
    }
}
