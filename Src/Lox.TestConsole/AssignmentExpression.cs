namespace Lox
{
    class AssignmentExpression : SyntaxNode
    {
        public Token Name { get; }
        public SyntaxNode Value { get; }

        public SyntaxKind Kind => SyntaxKind.AssignmentExpression;

        public AssignmentExpression(Token name, SyntaxNode value)
        {
            Name = name;
            Value = value;
        }
    }
}
