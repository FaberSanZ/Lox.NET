namespace Lox
{
    class ThisExpression : SyntaxNode 
    {
        public Token Keyword {get;}
        public SyntaxKind Kind => SyntaxKind.ThisExpression;

        public ThisExpression(Token keyword)
        {
            Keyword = keyword;
        }
    }
}
