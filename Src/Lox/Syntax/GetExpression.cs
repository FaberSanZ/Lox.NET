using System.Collections.Generic;

namespace Lox
{
    class GetExpression : SyntaxNode
    {
        public Token Name {get;}
        public SyntaxNode Object {get;}

        public override SyntaxKind Kind => SyntaxKind.GetExpression;

        public GetExpression(SyntaxNode expression, Token name)
        {
            Name = name;
            Object = expression;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            throw new System.NotImplementedException();
        }
    }
}
