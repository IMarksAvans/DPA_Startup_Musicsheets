using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Notes
{
    class FNote : INote
    {
        public FNote()
        {

        }

        public override string getKey()
        {
            return "F";
        }

        public override object Clone()
        {
            return new FNote();
        }
    }
}
