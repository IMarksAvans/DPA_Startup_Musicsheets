using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DPA_Musicsheets.SaversReaders
{
    class LilySaver : ISaver
    {
        protected List<OurTrack> tracks;

        protected string filename = "";

        public int Save()
        {
            if (filename == "")
                return 0;

            List<string> Lines = new List<string>();

            foreach (OurTrack t in tracks)
            {
                string line = "";

                Lines.Add(line);
            }

            File.WriteAllLines(filename,Lines.ToArray());

            return 1;
        }

        public void SetFilename(string Filename)
        {
            filename = Filename;
        }

        public void SetTracks(List<OurTrack> Tracks)
        {
            this.tracks = Tracks;
        }
    }
}
