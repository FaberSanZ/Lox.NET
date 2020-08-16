using System.Collections.Generic;

namespace Lox
{
    class LoxClass : LoxCallable
    {
        public string Name {get;}
        public Dictionary<string, LoxFunction> Methods {get;}

        public LoxClass SuperClass {get;}

        public int Arity {
            get {
            LoxFunction initializer = FindMethod("init");
            if (initializer == null) return 0;
            return initializer.Arity;
            }
        }

        public LoxClass(string name, LoxClass superclass, Dictionary<string, LoxFunction> methods)
        {
            Name = name;
            Methods = methods;
            SuperClass = superclass;
        }

        public LoxFunction FindMethod(string name)
        {
            if (Methods.ContainsKey(name))
                return Methods[name];
            
            if (SuperClass != null)
                return SuperClass.FindMethod(name);
            
            return null;
        }

        public override string ToString() 
        {
            return Name;
        }

        public object Call(Evaluator evaluator, List<object> arguments)
        {
            LoxInstance instance = new LoxInstance(this);
            LoxFunction initializer = FindMethod("init");
            if (initializer != null)
                initializer.Bind(instance).Call(evaluator, arguments);
           
           return instance;
        }
    }
}
