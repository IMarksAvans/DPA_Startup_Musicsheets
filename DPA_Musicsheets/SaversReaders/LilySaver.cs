using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DPA_Musicsheets.Notes;

namespace DPA_Musicsheets.SaversReaders
{
    class LilySaver : ISaver
    {
        //protected List<OurTrack> tracks;

        protected Song s;

        protected string filename = "";

        public int Save(string filename)
        {
            this.filename = filename;
            if (this.s == null)
                return 0;
            if (filename == "")
                return 0;
            List<OurTrack> tracks = s.Tracks;
            List<string> Lines = new List<string>();

            foreach (OurTrack t in tracks)
            {
                if (t.Notes.Count == 0)
                    continue;
                string line = "";
                foreach (Notes.Note n in t.Notes)
                {
                    line += TextFromNote(n);
                    line += " ";
                }

                Lines.Add(line);
            }

            File.WriteAllLines(filename,Lines.ToArray());

            return 1;
        }

        protected string TextFromNote(Note n)
        {
            string whole = "";
            string k = n.getKey();
            string d = Convert.ToString(n.Duration);
            whole += k;
            if (n.Octave > 5)
                whole += "'";
            else if (n.Octave < 5)
                whole += ",";
            whole += d;

            if (n.Punt)
                whole += ".";

            

            return whole;
        }

        public void SetFilename(string Filename)
        {
            filename = Filename;
        }

        public void SetSong(Song s)
        {
            this.s = s;
            //this.tracks = Tracks;
        }
    }
}
