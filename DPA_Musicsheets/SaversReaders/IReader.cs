using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets
{
    interface IReader
    {
        List<OurTrack> GetTracks();

        void SetFilename(string Filename);

        int Load();
    }
}
