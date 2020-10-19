using System.Collections.Generic;

namespace Lox
{
    class FunctionStatement : SyntaxNode 
    {
        public Token Name {get;}
        public List<Token> Parameters {get;}
        public List<SyntaxNode> Body {get;}

        public override SyntaxKind Kind => SyntaxKind.FunctionStatement;

        public FunctionStatement(Token name, List<Token> parameters, List<SyntaxNode> body)
        {
            Name = name;
            Parameters = parameters;
            Body = body;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            throw new System.NotImplementedException();
        }
    }
}
