using System.Collections.Generic;

namespace Lox
{
    interface LoxCallable
    {
        object Call(Evaluator evaluator, List<object> arguments);
        int Arity {get;}
    }
}
