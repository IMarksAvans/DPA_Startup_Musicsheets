using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.States
{
    class SetBookmarksState : BaseState
    {
        public SetBookmarksState(MainWindow main) : base(main)
        {
            
        }

        internal override void Handle()
        {
            main.Bookmark();

            main.SetState(new TypeState(main));
        }
    }
}
