using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Notes
{
    class BNote : INote
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

        public override void show()
        {
            throw new NotImplementedException();
        }
    }
}
