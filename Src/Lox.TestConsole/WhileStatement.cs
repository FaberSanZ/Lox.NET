namespace Lox
{
    class WhileStatement : SyntaxNode
    {
        
        public SyntaxNode Condition {get;}
        public SyntaxNode Body {get;}
        public SyntaxKind Kind => SyntaxKind.WhileStatement;

        public WhileStatement(SyntaxNode condition, SyntaxNode body)
        {
            Condition = condition;
            Body = body;
        }
    }
}
