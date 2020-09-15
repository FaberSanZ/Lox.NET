using System.Collections.Generic;

namespace Lox
{
    sealed class UnaryExpression : SyntaxNode
    {
        public SyntaxNode Right { get; }
        public Token Operator { get; }

        public UnaryExpression(Token oper, SyntaxNode right)
        {
            Operator = oper;
            Right = right;
        }

        public SyntaxKind Kind => SyntaxKind.UnaryExpression;


        //public IEnumerable<SyntaxNode> GetNodes()
        //{
        //    yield return Right;
        //    yield return this;
        //}
    }
}
