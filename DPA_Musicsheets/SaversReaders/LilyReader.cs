using DPA_Musicsheets;
using DPA_Musicsheets.Notes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DPA_Musicsheets.SaversReaders
{
    class LilyReader : IReader
    {
        public LilyReader()
        {
            Tracks = new List<OurTrack>();
            //_tracks = new List<OurTrack>();
            Filename = "";
        }

        public int Load(string Filename)
        {
            string[] lines = System.IO.File.ReadAllLines(@"" + Filename);

            for (int i = 0; i < lines.Length; i++)
            {
                String line = lines[i];
                OurTrack track = new OurTrack();
                track.Notes = new List<Note>();

                if (line.Contains("}"))
                {
                    //throw new Exception("Document is not right threated");
                }
                else if (line.Contains("relative") || String.IsNullOrEmpty(line))
                {
                    continue;
                }
                else if (line.Contains("clef"))
                {

                }
                else if (line.Contains("time"))
                {

                }
                else if (line.Contains("tempo"))
                {

                }
                else if (line.Contains("repeat"))
                {
                    List<string> repeatList = new List<string>(); 
                    List<string> altList    = new List<string>();
                    
                    int repeatCount = Int32.Parse(Regex.Match(line, @"\d+").Value);

                    i++;
                    while (!line.Contains("alternative"))
                    {
                        line = lines[i];
                        repeatList.Add(line);
                        i++;
                    }

                    while (!line.Contains("}"))
                    {
                        line = lines[i];
                        altList.Add(line);
                        i++;
                    }

                    track.Notes.AddRange(readRepeat(repeatList, altList, repeatCount));
                }
                else if (line.Contains("{"))
                {
                    //something something dark
                }
                else //(line.Contains('|')
                {
                    track.Notes.AddRange(readNoteLine(line));
                }

                Tracks.Add(track);
            }

            return 1;
        }

        private List<Note> readNoteLine(string line)
        {
            List<Note> noteList = new List<Note>(1000);

            for(int i = 0; i < line.Length; i++)
            {
                Char c = line[i];
                if (char.IsLetter(c))
                {
                    Note n = Note.create(c.ToString());
                    i++;
                    while(i < line.Length && !char.IsWhiteSpace(line[i]))
                    {
                        c = line[i];
                        if (Char.IsNumber(c))
                            n.Duration = Int32.Parse(c.ToString());
                        else
                        {
                            if (c.Equals('\''))
                                n.IncreaseOctave();
                            else if (c.Equals(','))
                                n.DecreaseOctave();
                            else if (c.Equals('.'))
                                n.Punt = true;
                        }

                        i++;
                    }
                    noteList.Add(n);
                }
            }
            // dis  =
            // gis  =
            // ~    = door tot in de volgende maat
            // es   = mol
            // is   = kruis

            return noteList;
        }

        private List<Note> readRepeat(List<string> repeat, List<string> alt, int repeatCount)
        {
            List<Note> noteList     = new List<Note>();
            List<Note> repeatList   = new List<Note>();
            List<List<Note>> altList      = new List<List<Note>>();

            foreach (string line in repeat)
            {
                repeatList.AddRange(readNoteLine(line));
            }

            foreach (string line in alt)
            {
                altList.Add(readNoteLine(line));
            }

            while (repeatCount > 0)
            {
                List<Note> toAdd = repeatList.ToList(); // Clone the list, to destroy the reference
                
                if (altList.Count() != 0)
                {
                    // logic to add the right alt
                }

                noteList.AddRange(toAdd);

                repeatCount--;
            }

            return noteList;
        }

        public void LoadReserved()
        {
            throw new NotImplementedException();
        }
    
        public string Filename
        {
            get;
            set;
        }

        public List<OurTrack> Tracks
        {
            get;
            set;
        }
    }
}
