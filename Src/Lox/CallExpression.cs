using System.Collections.Generic;

namespace Lox
{
    class CallExpression : SyntaxNode 
    {
        public SyntaxNode Callee {get;}
        public Token Paren {get;}

        public List<SyntaxNode> Arguments {get;}

        public override SyntaxKind Kind => SyntaxKind.CallExpression;

        public CallExpression(SyntaxNode callee, Token paren, List<SyntaxNode> arguments)
        {
            Callee  = callee;
            Paren = paren;
            Arguments = arguments;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            throw new System.NotImplementedException();
        }
    }
}
