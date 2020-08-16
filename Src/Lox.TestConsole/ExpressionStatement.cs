namespace Lox
{
    class ExpressionStatement : SyntaxNode
    {
        public SyntaxNode Expression { get; }
        public SyntaxKind Kind => SyntaxKind.ExpressionStatement;

        public ExpressionStatement(SyntaxNode expression)
        {
            Expression = expression;
        }
    }
}
