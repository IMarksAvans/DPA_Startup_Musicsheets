using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.CoR
{
    class InsertTempoHandler : ChainOfResponsibility
    {
        public override void Execute(string data)
        {
            base.Execute(data);
        }

        public override void Execute(List<Key> keyDownList)
        {
            if ((keyDownList.Contains(Key.LeftAlt) || keyDownList.Contains(Key.RightAlt)) && keyDownList.Contains(Key.S))
            {
                this.ExecuteCommand();
            }
            else
                base.Execute(keyDownList);
        }
    }
}
