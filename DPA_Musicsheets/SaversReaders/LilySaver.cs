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
            List<OurTrack> tracks = this.s.Tracks;
            List<string> Lines = new List<string>();
            string l = "";
            for (int i = this.s.Octave; i < 5; i++)
            {
                l += ",";
            }
            for (int i = this.s.Octave; i > 5; i--)
            {
                l += "'";
            }
            if(this.s.Relative.ToString() != " ")
                Lines.Add("\\relative " + this.s.Relative.ToString() + l);
            if(this.s.Pitch != "")
                Lines.Add("\\clef " + this.s.Pitch);
            if (this.s.Time != 0)
            {


                string time = this.s.Tracks[0].Time.ToString();

                var times = time.Split('.');

                //int time = (int)this.s.Time;
                //string d = this.s.Time.ToString();
                //d = d.Replace(time + ".", "");
                if (times.Count() >= 2)
                {
                    Lines.Add("\\time " + times[0].ToString() + "/" + times[1]);
                }
                
            }
            if (this.s.Metronome != 0 && this.s.Tempo != 0)
                Lines.Add("\\tempo " + s.Metronome.ToString() + "=" + s.Tempo.ToString());

            OurTrack placeholder = null;
            foreach (OurTrack t in tracks)
            {
                // tijd veranderd
                if (placeholder != null && placeholder.Time != t.Time)
                {
                    string time = t.Time.ToString();

                    var times = time.Split('.');
                    if (times.Count() >= 2)
                    Lines.Add("\\time " + times[0].ToString() + "/" + times[1]);
                }

                placeholder = t;

                // voeg note data toe.
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
            for(int i = n.Octave; i > 5;i--)
                whole += "'";
            for(int i = n.Octave; i < 5; i++)
                whole += ",";
            if (n.IsSharp)
                whole += "is";
            if (n.IsFlat)
                whole += "es";
            if(k != "~")
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
