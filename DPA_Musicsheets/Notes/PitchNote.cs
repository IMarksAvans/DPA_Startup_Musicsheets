using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Notes
{
    class PitchNote : Note
    {
        public PitchNote()
        {

        }

        public override string getKey()
        {
            return "pitch";
        }

        public override object Clone()
        {
            return new PitchNote();
        }
    }
}
