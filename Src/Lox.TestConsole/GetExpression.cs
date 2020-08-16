namespace Lox
{
    class GetExpression : SyntaxNode
    {
        public Token Name {get;}
        public SyntaxNode Object {get;}

        public SyntaxKind Kind => SyntaxKind.GetExpression;

        public GetExpression(SyntaxNode expression, Token name)
        {
            Name = name;
            Object = expression;
        }
    }
}
