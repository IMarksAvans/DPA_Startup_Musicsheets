using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Command.SubCommands
{
    class OpenCommand : Command
    {
        public OpenCommand(MainWindow receiver) : base(receiver)
        {
        }

        public override void Execute()
        {
            this.receiver.Open();
        }
    }
}
