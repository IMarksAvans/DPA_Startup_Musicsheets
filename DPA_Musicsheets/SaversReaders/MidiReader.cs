using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.SaversReaders
{
    class MidiReader : IReader
    {
        protected string _fileName;
        protected List<OurTrack> _tracks;

        MidiReader()
        {
            _tracks = new List<OurTrack>();
            _fileName = "";
        }

        public List<OurTrack> GetTracks()
        {
            return _tracks;
        }

        public int Load()
        {
            return 1;
            //throw new NotImplementedException();
        }

        public void SetFilename(string Filename)
        {
            _fileName = Filename;
            //throw new NotImplementedException();
        }
    }
}
