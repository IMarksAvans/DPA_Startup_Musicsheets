﻿using DPA_Musicsheets;
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

        public Song Load(string Filename)
        {
            List<Interpreter.Expression> Expressions = new List<Interpreter.Expression>();
            Interpreter.Expression exp = new Interpreter.TerminalExpression("}");
            Expressions.Add(exp);
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
            exp = new Interpreter.TerminalExpression("{");
            Expressions.Add(exp);
            Interpreter.Expression or = new Interpreter.OrExpression(Expressions);

            Song s = new Song();

            string[] lines = System.IO.File.ReadAllLines(@"" + Filename);

            for (int i = 0; i < lines.Length; i++)
            {
                String line = lines[i];
                OurTrack track = new OurTrack();
                track.Notes = new List<Note>();

                if (String.IsNullOrEmpty(line))
                    continue;

                if (or.Interpret(line))
                {
                    AddToSong(line);
                    continue;
                }
                else
                {
                    track.Notes.AddRange(readNoteLine(line));
                    Tracks.Add(track);
                }
               
            }

            s.Tracks = Tracks;

            return s;
        }

        private void AddToSong(string line)
        {
            if (line.Contains("}"))
            {
                //throw new Exception("Document is not right threated");
            }
            else if (line.Contains("relative"))
            {
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
                /*
                List<string> repeatList = new List<string>();
                List<string> altList = new List<string>();

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
                var l = readRepeat(repeatList, altList, repeatCount);
                Tracks.AddRange(l);
                continue;
                //Tracks.Add();
                //track.Notes.AddRange(readRepeat(repeatList, altList, repeatCount));
                */
            }
            else if (line.Contains("{"))
            {
                //something something dark
            }
            else //(line.Contains('|')
            {
                
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
                    while(i < line.Length && !char.IsWhiteSpace(line[i]))
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
                            else if (c.Equals('e'))
                                n.IsFlat = true;
                            else if (c.Equals('i'))
                                n.IsSharp = true;
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
                //t.Notes = 
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
