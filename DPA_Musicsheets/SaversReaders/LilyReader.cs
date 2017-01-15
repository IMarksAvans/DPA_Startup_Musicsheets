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
            //_tracks = new List<OurTrack>();
            Filename = "";
        }

        public Song Load(string[] lines)
        {
            Song s = new Song();
            List<OurTrack> Tracks = new List<OurTrack>();

            List<Interpreter.Expression> Expressions = new List<Interpreter.Expression>();
            Interpreter.Expression exp;
            //exp = new Interpreter.TerminalExpression("}");
            //Expressions.Add(exp);
            exp = new Interpreter.TerminalExpression("relative");
            Expressions.Add(exp);
            exp = new Interpreter.TerminalExpression("clef");
            Expressions.Add(exp);
            exp = new Interpreter.TerminalExpression("time");
            Expressions.Add(exp);
            exp = new Interpreter.TerminalExpression("tempo");
            Expressions.Add(exp);
            exp = new Interpreter.TerminalExpression("repeat");
            Expressions.Add(exp);
            exp = new Interpreter.TerminalExpression("alternative");
            Expressions.Add(exp);
            //exp = new Interpreter.TerminalExpression("{");
            //Expressions.Add(exp);
            Interpreter.Expression or = new Interpreter.OrExpression(Expressions);
            OurTrack track = null;// = new OurTrack();
            for (int i = 0; i < lines.Length; i++)
            {
                if (track == null)
                    track = new OurTrack();
                String line = lines[i];
                
                track.Notes = new List<Note>();

                if (String.IsNullOrEmpty(line))
                    continue;

                if (or.Interpret(line))
                {
                    AddToTrack(line,track);
                    //AddToSong(line, s);
                    continue;
                }
                else
                {
                    track.Notes.AddRange(readNoteLine(line));
                    //track.BPM = s.Tempo;
                    //track.Time = s.Time;
                    //track.Metronome = s.Metronome;
                    //track.IsRepeat = s.InRepeat;
                    //track.IsAlternative = s.InAlternative;

                    Tracks.Add(track);
                    track = null;
                }

            }

            s.Tracks = Tracks;

            return s;
        }

        private void AddToTrack(string line, OurTrack track)
        {
            if (line.Contains("relative"))
            {
                int oh = line.Count(x => x == '\'');
                int ol = line.Count(x => x == ',');
                track.Octave += (oh - ol);
                track.Relative = line.Substring(line.IndexOf("relative") + 9, 1)[0];
            }
            else if (line.Contains("clef"))
            {
                track.Pitch = line.Substring(line.IndexOf("clef") + 5);
            }
            else if (line.Contains("time"))
            {
                string time = line.Substring(line.IndexOf("time") + 5);

                var times = time.Split('/');

                track.Time = Convert.ToDouble(times[0] + ',' + times[1]);
                //times[0];
                //times[1];

            }
            else if (line.Contains("tempo"))
            {
                string M = line.Substring(line.IndexOf("tempo") + 6, 1);
                track.Metronome = Convert.ToInt32(M);

                string BPM = line.Substring(line.IndexOf("=") + 1);
                track.Tempo = Convert.ToInt32(BPM);
            }
            else if (line.Contains("repeat"))
            {
                track.InRepeat = true;
            }
            else if (line.Contains("alternative"))
            {
                track.InAlternative = true;
                track.InRepeat = false;
            }

        }

        public Song Load(string Filename)
        {
            return Load(System.IO.File.ReadAllLines(@"" + Filename));
        }

        private void AddToSong(string line,Song s)
        {
            if (line.Contains("relative"))
            {
                int oh = line.Count(x => x == '\'');
                int ol = line.Count(x => x == ',');
                s.Octave += (oh - ol);
                s.Relative = line.Substring(line.IndexOf("relative") + 9,1)[0];
            }
            else if (line.Contains("clef"))
            {
                s.Pitch = line.Substring(line.IndexOf("clef") + 5);
            }
            else if (line.Contains("time"))
            {
                string time = line.Substring(line.IndexOf("time") + 5);

                var times = time.Split('/');

                s.Time = Convert.ToDouble(times[0] + ',' + times[1]);
                //times[0];
                //times[1];

            }
            else if (line.Contains("tempo"))
            {
                string M = line.Substring(line.IndexOf("tempo") + 6,1);
                s.Metronome = Convert.ToInt32(M);

                string BPM = line.Substring(line.IndexOf("=") + 1);
                s.Tempo = Convert.ToInt32(BPM);
            }
            else if (line.Contains("repeat"))
            {
                s.InRepeat = true;
            }
            else if (line.Contains("alternative"))
            {
                s.InAlternative = true;
                s.InRepeat = false;
            }

            //Tracks.Add(track);
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
                    while(i < line.Length && (!char.IsWhiteSpace(line[i])))
                    {
                        c = line[i];
                        if (Char.IsNumber(c))
                            n.Duration = (n.Duration == 0) ? Int32.Parse(c.ToString()) : int.Parse(n.Duration.ToString() + c.ToString());
                        else
                        {
                            if (c.Equals('\''))
                                n.IncreaseOctave();
                            else if (c.Equals(','))
                                n.DecreaseOctave();
                            else if (c.Equals('.'))
                                n.Punt = true;
                            else if (c.Equals('e') && line[i + 1].Equals('s'))
                                n.IsFlat = true;
                            else if (c.Equals('i') && line[i + 1].Equals('s'))
                                n.IsSharp = true;
                            else if (char.IsLetter(c) && !c.Equals('s'))
                            {
                                i--;
                                break;
                            }
                            
                        }

                        i++;
                    }
                    noteList.Add(n);
                }
            }

            // ~    = door tot in de volgende maat

            return noteList;
        }

        private List<OurTrack> readRepeat(List<string> repeat, List<string> alt, int repeatCount)
        {
            List<Note> noteList     = new List<Note>();
            List<Note> repeatList   = new List<Note>();
            List<List<Note>> altList      = new List<List<Note>>();

            List<OurTrack> tracks = new List<OurTrack>();

            foreach (string line in repeat)
            {
                var l = readNoteLine(line);
                repeatList.AddRange(l);
                OurTrack t = new OurTrack();
                t.Notes = l;
                tracks.Add(t);
            }

            foreach (string line in alt)
            {
                var l = readNoteLine(line);
                altList.Add(l);
                OurTrack t = new OurTrack();
                t.Notes = l;
                tracks.Add(t);
            }

            int currentAlt = 0;

            for (int currentRepeat = 0; currentRepeat < repeatCount; currentRepeat++) 
            {
                List<Note> toAdd = repeatList.ToList(); // Clone the list, to destroy the reference
                
                if (altList.Count() > 0)
                {
                    toAdd.AddRange(altList[currentAlt]);
                    if (currentRepeat >= repeatCount - altList.Count())
                    { 
                        currentAlt++;
                    }
                }
               

                noteList.AddRange(toAdd);

                repeatCount--;
            }

            return tracks;
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
