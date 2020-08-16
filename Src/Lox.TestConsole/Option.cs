using System;

namespace Lox
{
    public static class Functional
    {
        public static Nothing None { get; } = Nothing.Instance;
    }
    abstract class Option<T>
    {
      
        public static implicit operator Option<T>(T value)
        {
            return new Some<T>(value);
        }
        public static implicit operator Option<T>(Nothing none)
        {
            return new None<T>();
        }

        public abstract Option<TResult> Map<TResult>(Func<T, TResult> map);
        public abstract T Or(T whenNone);

    }

    public class Nothing
    {
        public static Nothing Instance { get; } = new Nothing();

        private Nothing() { }

        public override string ToString()
        {
            return "nil";
        }

    }

    class Some<T> : Option<T>
    {
        public T Value { get; }

        public Some(T value)
        {
            Value = value;
        }

        public override Option<TResult> Map<TResult>(Func<T, TResult> map)
        {
            return map(Value);
        }
        public override T Or(T whenNone)
        {
            return Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

    }

    class None<T> : Option<T>
    {
        public override Option<TResult> Map<TResult>(Func<T, TResult> map)
        {
            return Functional.None;
        }
        public override T Or(T whenNone)
        {
            return whenNone;
        }

        public override string ToString()
        {
            return "nil";
        }
    }

}
