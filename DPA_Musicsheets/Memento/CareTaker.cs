using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Memento
{
    class Caretaker
    {
        private Memento _memento;

        public Caretaker()
        {
            this.Mementos = new List<Memento>();
        }

        public List<Memento> Mementos
        {
            get;
            set;
        }

        // Gets or sets memento
        public Memento Memento
        {
            set { _memento = value; }
            get { return _memento; }
        }
    }
}
