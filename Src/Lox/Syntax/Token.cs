using System;
using System.Collections.Generic;

namespace Lox
{
    public sealed class Token : SyntaxNode
    {
        public string Lexeme { get; }
        public object Literal { get; }
        public int Line { get; }

        public override SyntaxKind Kind { get; }

        public Token(SyntaxKind type, string lexeme, object literal, int line)
        {
            Kind = type;
            this.Lexeme = lexeme;
            this.Literal = literal;
            this.Line = line;
        }

        public override string ToString()
        {
            return Kind + " " + Lexeme + " " + Literal;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            return Array.Empty<SyntaxNode>();
        }
    }
}
