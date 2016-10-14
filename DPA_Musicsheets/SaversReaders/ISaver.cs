using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets
{
    interface ISaver
    {
        void SetTracks(List<OurTrack> Tracks);

        void SetFilename(string Filename);

        int Save(string filename);
    }
}
