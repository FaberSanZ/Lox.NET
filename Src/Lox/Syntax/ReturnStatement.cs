using System.Collections.Generic;

namespace Lox
{
    class ReturnStatement : SyntaxNode 
    {
        public Token Keyword {get;}
        public SyntaxNode Value {get;}
        public override SyntaxKind Kind => SyntaxKind.ReturnStatement;

        public ReturnStatement(Token keyword, SyntaxNode value)
        {
            Keyword = keyword;
            Value = value;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            throw new System.NotImplementedException();
        }
    }
}
