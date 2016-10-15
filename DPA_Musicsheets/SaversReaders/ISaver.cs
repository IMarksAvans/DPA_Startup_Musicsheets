using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets
{
    interface ISaver
    {
        void SetSong(Song s);

        void SetFilename(string Filename);

        int Save(string filename);
    }
}
