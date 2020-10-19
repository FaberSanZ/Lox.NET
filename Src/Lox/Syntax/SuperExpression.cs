using System.Collections.Generic;

namespace Lox
{
    class SuperExpression : SyntaxNode
    {
        public Token Keyword {get;}
        public Token Method {get;}

        public override SyntaxKind Kind => SyntaxKind.SuperExpression;

        public SuperExpression(Token keyword, Token method)
        {
            Keyword = keyword;
            Method = method;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            throw new System.NotImplementedException();
        }
    }
}
