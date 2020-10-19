using System.Collections.Generic;

namespace Lox
{
    class AssignmentExpression : SyntaxNode
    {
        public Token Name { get; }
        public SyntaxNode Value { get; }

        public override SyntaxKind Kind => SyntaxKind.AssignmentExpression;

        public AssignmentExpression(Token name, SyntaxNode value)
        {
            Name = name;
            Value = value;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            throw new System.NotImplementedException();
        }
    }
}
