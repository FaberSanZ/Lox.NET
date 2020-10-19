using System.Collections.Generic;
using static Lox.Functional;

namespace Lox
{
    sealed class LiteralExpression : SyntaxNode
    {
        public Option<object> Value { get; }

        public LiteralExpression(object? value)
        {
            Value = value ?? None;
        }

        public override SyntaxKind Kind => SyntaxKind.LiteralExpression;

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            throw new System.NotImplementedException();
        }
    }
}
