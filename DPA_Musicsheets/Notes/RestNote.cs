using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Notes
{
    class RestNote : Note
    {
        public RestNote()
        {

        }

        public override string getKey()
        {
            return "r";
        }

        public override object Clone()
        {
            return new RestNote();
        }

        public void show()
        {
            throw new NotImplementedException();
        }

        public int GetDuration()
        {
            throw new NotImplementedException();
        }

        public string GetMaatsoort()
        {
            throw new NotImplementedException();
        }

        public bool IsMuted()
        {
            throw new NotImplementedException();
        }

        public bool IsPunt()
        {
            throw new NotImplementedException();
        }

        public string HogerLager()
        {
            throw new NotImplementedException();
        }
    }
}
