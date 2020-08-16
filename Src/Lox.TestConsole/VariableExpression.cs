namespace Lox
{
    class VariableExpression : SyntaxNode
    {
        public Token Name { get; }

        public SyntaxKind Kind => SyntaxKind.VariableExpression;

        public VariableExpression(Token name)
        {
            Name = name;
        }
    }
}
