using System;
using System.Runtime.CompilerServices;

namespace Lox
{
    sealed class Error
    {
        public int Line { get; }
        public string Message { get; }

        public string Where { get; }

        public ErrorType Type { get; }

        public Error(ErrorType type, int line, string message) : this(type, line, "", message)
        {
           
        }

        public Error(ErrorType type, int line, string where, string message)
        {
            this.Type = type;
            this.Line = Line;
            this.Message = message;
            this.Where = where;
        }
    }
}
