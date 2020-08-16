namespace Lox
{
    class PrintStatement : SyntaxNode
    {
        public SyntaxNode Expression { get; }

        public SyntaxKind Kind => SyntaxKind.PrintStatement;

        public PrintStatement(SyntaxNode expression)
        {
            Expression = expression;
        }
    }
}
