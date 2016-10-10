using System;
using System.Collections.Generic;
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
            foreach (string line in lines)
            {
                // Use a tab to indent each line of the file.
                Console.WriteLine("\t" + line);
            }

            return 1;
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
