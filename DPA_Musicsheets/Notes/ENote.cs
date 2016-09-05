using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Notes
{
    class ENote : INote
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

        public override void show()
        {
            throw new NotImplementedException();
        }
    }
}
