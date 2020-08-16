namespace Lox
{
    class ReturnStatement : SyntaxNode 
    {
        public Token Keyword {get;}
        public SyntaxNode Value {get;}
        public SyntaxKind Kind => SyntaxKind.ReturnStatement;

        public ReturnStatement(Token keyword, SyntaxNode value)
        {
            Keyword = keyword;
            Value = value;
        }
    }
}
