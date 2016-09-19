using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Notes
{
    class BNote : AbstractNote
    {
        public BNote()
        {

        }

        public override string getKey()
        {
            return "B";
        }

        public override object Clone()
        {
            return new BNote();
        }
    }
}
