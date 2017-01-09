using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.CoR
{
    class KeyCommand
    {
        protected KeyCommand successor;

        void Execute(string data)
        {
            if (successor != null)
            {
                successor.Execute(data);
            }
        }

        void SetSuccessor(KeyCommand successor)
        {
            this.successor = successor;
        }
    }
}
