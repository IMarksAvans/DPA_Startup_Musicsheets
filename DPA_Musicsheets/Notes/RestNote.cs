using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Notes
{
    class RestNote : INote
    {
        public RestNote()
        {

        }

        public override string getKey()
        {
            return "R";
        }

        public override object Clone()
        {
            return new RestNote();
        }

        public override void show()
        {
            throw new NotImplementedException();
        }
    }
}
