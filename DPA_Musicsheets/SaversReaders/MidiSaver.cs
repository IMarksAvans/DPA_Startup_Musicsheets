using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.SaversReaders
{
    class MidiSaver : ISaver
    {
        protected string _fileName;
        protected List<OurTrack> _tracks;

        MidiSaver()
        {
            _tracks = new List<OurTrack>();
            _fileName = "";
        }

        public void SetTracks(List<OurTrack> Tracks)
        {
            _tracks = Tracks;
        }

        public void SetFilename(string Filename)
        {
            this._fileName = Filename;
        }

        public int Save(string filename)
        {
            return 1;
        }
    }
}
