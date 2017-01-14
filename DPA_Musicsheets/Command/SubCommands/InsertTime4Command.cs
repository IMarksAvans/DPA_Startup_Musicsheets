using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Command.SubCommands
{
    class InsertTime4Command : Command
    {
        public InsertTime4Command(MainWindow receiver) : base(receiver)
        {
        }

        public override void Execute()
        {
            this.receiver.InsertTime4();
        }
    }
}
