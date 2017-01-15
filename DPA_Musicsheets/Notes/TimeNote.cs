using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Notes
{
    class TimeNote : Note
    {
        public TimeNote()
        {

        }

        public override string getKey()
        {
            return "time";
        }

        public override object Clone()
        {
            return new TimeNote();
        }
    }
}
