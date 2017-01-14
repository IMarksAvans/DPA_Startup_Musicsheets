using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Command.SubCommands
{
    class PdfCommand : Command
    {
        public PdfCommand(MainWindow receiver) : base(receiver)
        {
        }

        public override void Execute()
        {
            this.receiver.SaveToPdf();
           // this.receiver.Execute();
        }
    }
}
