using System.Collections.Generic;

namespace Lox
{
    class ThisExpression : SyntaxNode 
    {
        public Token Keyword {get;}
        public override SyntaxKind Kind => SyntaxKind.ThisExpression;

        public ThisExpression(Token keyword)
        {
            Keyword = keyword;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            throw new System.NotImplementedException();
        }
    }
}
