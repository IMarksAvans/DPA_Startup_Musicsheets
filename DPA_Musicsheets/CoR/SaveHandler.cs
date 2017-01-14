using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.CoR
{
    class SaveHandler : ChainOfResponsibility
    {

        public override void Execute(string data)
        {
            base.Execute(data);
        }

        public override void Execute(List<Key> keyDownList)
        {
            if ((keyDownList.Contains(Key.LeftCtrl) || keyDownList.Contains(Key.RightCtrl)) && keyDownList.Contains(Key.S))
            {
                this.ExecuteCommand();
            }
            else
                base.Execute(keyDownList);
        }
    }
}
