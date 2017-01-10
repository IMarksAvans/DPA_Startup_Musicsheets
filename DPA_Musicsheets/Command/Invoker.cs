using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Command
{
    class Invoker
    {
        protected Command command;


        public void Execute()
        {
            command.Execute();
        }
    }
}
