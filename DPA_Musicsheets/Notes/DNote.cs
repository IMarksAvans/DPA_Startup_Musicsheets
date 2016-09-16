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

        public override int GetDuration()
        {
            throw new NotImplementedException();
        }

        public override string GetMaatsoort()
        {
            throw new NotImplementedException();
        }

        public override bool IsMuted()
        {
            throw new NotImplementedException();
        }

        public override bool IsPunt()
        {
            throw new NotImplementedException();
        }

        public override string HogerLager()
        {
            throw new NotImplementedException();
        }
    }
}
