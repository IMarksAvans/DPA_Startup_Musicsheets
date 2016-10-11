using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Notes
{
    class ENote : Note
    {
        public ENote()
        {

        }

        public override string getKey()
        {
            return "E";
        }

        public override object Clone()
        {
            return new ENote();
        }
    }
}
