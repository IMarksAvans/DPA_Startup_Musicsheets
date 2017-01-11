using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets
{
    interface IReader
    {
        void LoadReserved();

        Song Load(string Filename);
        Song Load(string[] lines);
    }
}
