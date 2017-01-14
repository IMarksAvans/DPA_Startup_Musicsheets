using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Command
{
    public abstract class Command
    {
        protected MainWindow receiver;

        public Command(MainWindow receiver)
        {
            this.receiver = receiver;
        }

        /*public void SetReceiver(Receiver receiver)
        {
            this.receiver = receiver;
        }*/

        public abstract void Execute();
    }
}
