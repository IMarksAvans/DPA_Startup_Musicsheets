using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.CoR
{
    public class ChainOfResponsibility
    {
        protected ChainOfResponsibility successor;

        public virtual void Execute(string data)
        {
            if (successor != null)
            {
                successor.Execute(data);
            }
        }

        public void SetSuccessor(ChainOfResponsibility successor)
        {
            this.successor = successor;
        }

        public virtual void Execute(List<Key> keyDownList)
        {
            if (successor != null)
            {
                successor.Execute(keyDownList);
            }
        }
    }
}
