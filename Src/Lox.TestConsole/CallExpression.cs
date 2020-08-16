using System.Collections.Generic;

namespace Lox
{
    class CallExpression : SyntaxNode 
    {
        public SyntaxNode Callee {get;}
        public Token Paren {get;}

        public List<SyntaxNode> Arguments {get;}

        public SyntaxKind Kind => SyntaxKind.CallExpression;

        public CallExpression(SyntaxNode callee, Token paren, List<SyntaxNode> arguments)
        {
            Callee  = callee;
            Paren = paren;
            Arguments = arguments;
        }

    }
}
