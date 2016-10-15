using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Interpreter
{
    class OrExpression : Expression
    {
        protected List<Expression> Expressions;

        public OrExpression(List<Expression> Expressions)
        {
            this.Expressions = Expressions;
        }

        public bool Interpret(string context)
        {
            foreach (Expression exp in Expressions)
            {
                if (exp.Interpret(context))
                    return true;
            }

            return false;
        }
    }
}
