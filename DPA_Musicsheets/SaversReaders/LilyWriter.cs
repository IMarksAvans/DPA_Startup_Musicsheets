using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DPA_Musicsheets.Notes;

namespace DPA_Musicsheets.SaversReaders
{
    class LilyWriter : IWriter
    {
        //protected List<OurTrack> tracks;

        protected Song s;
        
        string[] lines;

        public string[] GetContent()
        {
            this.lines = null;

            if (this.s == null)
                return null;
            List<OurTrack> tracks = this.s.Tracks;
            List<string> Lines = new List<string>();
           

            OurTrack placeholder = null;
            foreach (OurTrack t in tracks)
            {
                string l = "";
                for (int i = t.Octave; i < 5; i++)
                {
                    l += ",";
                }
                for (int i = t.Octave; i > 5; i--)
                {
                    l += "'";
                }
                if (t.Relative.ToString() != " ")
                    Lines.Add("\\relative " + t.Relative.ToString() + l);
                if (t.Pitch != "")
                    Lines.Add("\\clef " + t.Pitch);
                if (t.Time != 0)
                {


                    string time = t.Time.ToString();

                    //var times = time.Split(',');

                    //int time = (int)this.s.Time;
                    //string d = this.s.Time.ToString();
                    //d = d.Replace(time + ".", "");
                    
                    Lines.Add("\\time " + time[0].ToString() + "/" + time[1]);
                   

                }
                if (t.Metronome != 0 && t.Tempo != 0)
                    Lines.Add("\\tempo " + t.Metronome.ToString() + "=" + t.Tempo.ToString());


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
            this.lines = Lines.ToArray();
            return Lines.ToArray();
        }

        public int Save(string Filename)
        {
            if (Filename == "")
                return 0;
            if (this.s == null)
                return 0;

            this.GetContent();
            File.WriteAllLines(Filename, lines);

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

        public void SetSong(Song s)
        {
            this.s = s;
            //this.tracks = Tracks;
        }
    }
}
