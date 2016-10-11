using DPA_Musicsheets;
using DPA_Musicsheets.Notes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

            // Display the file contents by using a foreach loop.
            System.Console.WriteLine("Contents of " + Filename + " = ");
            Debug.Print("something whon");
            Debug.WriteLine(Filename);
            
            foreach (string line in lines)
            {
                if (line.Contains("}"))
                {
                    throw new Exception("Document is not right threated");
                }
                else if (line.Contains("relative") || line.CompareTo("") == 1)
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
                    // get stack up to and including the alternative
                    // convert
                }
                else if (line.Contains("|"))
                {
                    readNoteLine(line);
                }
                else if (line.Contains("{"))
                {
                    //something something dark
                }
            }

            return 1;
        }

        private void readNoteLine(string line)
        {
            for(int i = 0; i < line.Length; i++)
            {
                Char c = line[i];
                if (Char.IsLetter(c))
                {
                    Note n = Note.create(c.ToString());
                    while(line[++i].Equals(' '))
                    {
                        c = line[i];
                        if (Char.IsNumber(c))
                        {
                            n.Duration = Int32.Parse(c.ToString());
                        }
                        else
                        {
                            if (c.Equals("'"))
                            {
                                n.IncreaseOctave();
                            }
                            else if (c.Equals(","))
                            {
                                n.DecreaseOctave();
                            }
                            else if (c.Equals("."))
                            {
                                n.Punt = true;
                            }
                        }
                        // . == x1,5
                        // ~ == door tot in de volgende maat
                    }
                    // add the note to something.
                }
            }
            // r    = rest
            // dis  =
            // gis  =
            // ~    =
            // '    = increase note height
            // .    = decrease note height
            // es   = mol
            // is   = kruis
        }

        private void readRepeat()
        {
            // please note the alternative, this replaced the first line with the second line.
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
