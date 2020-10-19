using System;
using System.Collections.Generic;
using System.Linq;

namespace Lox
{
    class WhileStatement : SyntaxNode
    {
        
        public SyntaxNode Condition {get;}
        public SyntaxNode Body {get;}

        public override SyntaxKind Kind => SyntaxKind.WhileStatement;

        public WhileStatement(SyntaxNode condition, SyntaxNode body)
        {
            Condition = condition;
            Body = body;
        }
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            return Array.Empty<SyntaxNode>().ToArray().AsEnumerable();
        }
    }
}
