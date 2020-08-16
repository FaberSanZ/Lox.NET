namespace Lox
{
    sealed class BinaryExpression : SyntaxNode
    {
        public SyntaxNode Left { get; }
        public SyntaxNode Right { get; }
        public Token Operator { get; }

        public BinaryExpression(SyntaxNode left, Token oper, SyntaxNode right)
        {
            Left = left;
            Operator = oper;
            Right = right;
        }

        public SyntaxKind Kind => SyntaxKind.BinaryExpression;
    }
}
