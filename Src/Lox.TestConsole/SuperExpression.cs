namespace Lox
{
    class SuperExpression : SyntaxNode
    {
        public Token Keyword {get;}
        public Token Method {get;}

        public SyntaxKind Kind => SyntaxKind.SuperExpression;

        public SuperExpression(Token keyword, Token method)
        {
            Keyword = keyword;
            Method = method;
        }
    }
}
