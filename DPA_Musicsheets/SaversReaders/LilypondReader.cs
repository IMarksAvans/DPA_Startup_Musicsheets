using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.SaversReaders
{
    class LilypondReader : IReader
    {

        protected List<ReservedWords.IExpression> _reservedWords;

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

        LilypondReader()
        {
            Tracks = new List<OurTrack>();
            //_tracks = new List<OurTrack>();
            Filename = "";
            this.LoadReserved();
        }



        public int Load()
        {
            string text = System.IO.File.ReadAllText(this.Filename);
            string []lat = text.Split(' ');

            if (lat.Length <= 0)
                return 0;

            List<string> llt = new List<string>(lat);

            

            return 1;
        }

        public void LoadReserved()
        {
            this._reservedWords = new List<ReservedWords.IExpression>();

            this._reservedWords.Add(new ReservedWords.ReservedWord());
        }
    }
}
