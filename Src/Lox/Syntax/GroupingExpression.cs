using System.Collections.Generic;

namespace Lox
{
    sealed class GroupingExpression : SyntaxNode
    {
        public SyntaxNode Expression { get; }

        public GroupingExpression(SyntaxNode expression)
        {
            Expression = expression;
        }

        public override SyntaxKind Kind => SyntaxKind.GroupingExpression;

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            throw new System.NotImplementedException();
        }
    }
}
