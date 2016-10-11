using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Notes
{
    class CNote : Note
    {
        public CNote()
        {

        }

        public override string getKey()
        {
            return "C";
        }

        public override object Clone()
        {
            return new CNote();
        }
    }
}
