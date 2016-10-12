using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Notes
{
    class DNote : Note
    {
        public DNote()
        {

        }

        public override string getKey()
        {
            return "d";
        }

        public override object Clone()
        {
            return new DNote();
        }
    }
}
