using System;
using System.Collections.Generic;

namespace Lox
{
    class LoxInstance
    {
        private LoxClass _class;
        private Dictionary<string, object> _fields = new Dictionary<string, object>();

        public LoxInstance(LoxClass klass)
        {
            _class = klass;
        }

        public override string ToString()
        {
            return _class.Name + " instance";
        }

        public object Get(Token name)
        {
            if (_fields.ContainsKey(name.Lexeme))
                return _fields[name.Lexeme];

            var method = _class.FindMethod(name.Lexeme);
            if (method != null) return method.Bind(this);

            throw new RuntimeError(name, $"Undefined property {name.Lexeme}.");
        }

        public void Set(Token name, Object value)
        {
            _fields[name.Lexeme] = value;
        }
    }
}
