using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Interpreter
{
    class TerminalExpression : Expression
    {
        public string Data
        {
            get;
            set;
        }

        public bool Interpret(string context)
        {
            if (context.Contains(Data))
                return true;

            return false;
        }
    }
}
