using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Notes
{
    class DNote : INote
    {
        public DNote()
        {

        }

        public override string getKey()
        {
            return "D";
        }

        public override object Clone()
        {
            return new DNote();
        }

        public override void show()
        {
            throw new NotImplementedException();
        }
    }
}
