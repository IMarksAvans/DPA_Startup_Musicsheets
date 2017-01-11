using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets
{
    interface IWriter
    {
        void SetSong(Song s);

        string[] GetContent();

        int Save(string Filename);
    }
}
