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

        public SyntaxKind Kind => SyntaxKind.LiteralExpression;
    }
}
