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
