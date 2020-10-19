using System;
using System.Collections.Generic;
using System.Linq;

namespace Lox
{
    class VariableExpression : SyntaxNode
    {
        public Token Name { get; }

        public override SyntaxKind Kind => SyntaxKind.VariableExpression;

        public VariableExpression(Token name)
        {
            Name = name;
        }


        public override IEnumerable<SyntaxNode> GetChildren()
        {
            return Array.Empty<SyntaxNode>().ToArray().AsEnumerable();
        }
    }
}
