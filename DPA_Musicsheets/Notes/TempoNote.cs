using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Notes
{
    class TempoNote : Note
    {
        public TempoNote()
        {

        }

        public override string getKey()
        {
            return "tempo";
        }

        public override object Clone()
        {
            return new TempoNote();
        }
    }
}
