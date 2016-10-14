using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets
{
    class ChainOfResponsibility
    {
        public bool Handle(List<Key> keysDown)
        {
            return false;
        }
    }
}
