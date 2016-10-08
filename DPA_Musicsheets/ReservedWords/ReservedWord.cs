using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.ReservedWords
{
    class ReservedWord : IExpression
    {
        public string Word
        {
            get;
            set;
        }

        public ReservedWord()
        {

        }

        public bool interpret(string context)
        {
            return context.Contains(Word);     
        }
    }
}
