using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.States
{
    public abstract class BaseState
    {
        protected MainWindow main;

        public BaseState(MainWindow main)
        {
            this.main = main;
        }

        internal abstract void Handle();
        
            
        
    }
}
