using System.Collections.Generic;

namespace Lox
{
    class LoxFunction : LoxCallable
    {
        private FunctionStatement _declaration;
        private Environment _closure;

        private bool _isInitializer;

        public LoxFunction(FunctionStatement declaration, Environment closure, bool isInitializer)
        {
            _declaration = declaration;
            _closure = closure;
            _isInitializer = isInitializer;
        }

        public int Arity => _declaration.Parameters.Count;

        public object Call(Evaluator evaluator, List<object> arguments)
        {
            var env = new Environment(_closure);
            for(int i = 0; i < _declaration.Parameters.Count; i++)
            {
                env.Define(_declaration.Parameters[i].Lexeme, arguments[i]);
            }
            try 
            {
                evaluator.EvaluateBlock(_declaration.Body, env);
            }
            catch (Return ret)
            {
                if (_isInitializer) return _closure.GetAt(0, new Token(TokenType.This, "this", null, 0));
                return ret.Value;
            }

            if (_isInitializer) return _closure.GetAt(0, new Token(TokenType.This, "this", null, 0));
            return null;
        }

        
        public LoxFunction Bind(LoxInstance instance)
        {
            var env = new Environment(_closure);
            env.Define("this", instance);
            return new LoxFunction(_declaration, env, _isInitializer);
        }

        public override string ToString()
        {
            return "<fn " + _declaration.Name.Lexeme + ">";
        }
    }
}
