using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Command.SubCommands
{
    class InsertTime6Command : Command
    {
        public InsertTime6Command(MainWindow receiver) : base(receiver)
        {
        }

        public override void Execute()
        {
            this.receiver.InsertTime6();
        }
    }
}
