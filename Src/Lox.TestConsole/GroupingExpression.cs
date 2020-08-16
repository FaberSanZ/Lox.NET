namespace Lox
{
    sealed class GroupingExpression : SyntaxNode
    {
        public SyntaxNode Expression { get; }

        public GroupingExpression(SyntaxNode expression)
        {
            Expression = expression;
        }

        public SyntaxKind Kind => SyntaxKind.GroupingExpression;
    }
}
