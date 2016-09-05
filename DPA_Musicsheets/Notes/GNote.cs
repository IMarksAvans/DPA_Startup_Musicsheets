using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Notes
{
    class GNote : INote
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

        public override void show()
        {
            throw new NotImplementedException();
        }
    }
}
