using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Notes
{
    class ANote : INote
    {
        public ANote()
        {

        }

        public override string getKey()
        {
            return "A";
        }

        public override object Clone()
        {
            return new ANote();
        }

        public override void show()
        {
            throw new NotImplementedException();
        }
    }
}
