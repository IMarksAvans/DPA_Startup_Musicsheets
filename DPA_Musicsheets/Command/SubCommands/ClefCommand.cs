using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Command.SubCommands
{
    class ClefCommand : Command
    {
        public ClefCommand(MainWindow receiver) : base(receiver)
        {
        }

        public override void Execute()
        {
            this.receiver.InsertClef();
            //this.receiver.Execute();
        }
    }
}
