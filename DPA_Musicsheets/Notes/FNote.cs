using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Notes
{
    class FNote : Note
    {
        public FNote()
        {

        }

        public override string getKey()
        {
            return "f";
        }

        public override object Clone()
        {
            return new FNote();
        }
    }
}
