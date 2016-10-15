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
        protected Song s;//List<OurTrack> _tracks;

        MidiSaver()
        {
            s = null;
            //_tracks = new List<OurTrack>();
            _fileName = "";
        }

        public void SetSong(Song S)
        {
            this.s = s;
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
