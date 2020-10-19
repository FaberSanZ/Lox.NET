using System.Collections.Generic;

namespace Lox
{
    public sealed class BlockStatement : SyntaxNode
    {
        public List<SyntaxNode> Statements { get; }
        public override SyntaxKind Kind => SyntaxKind.BlockStatement;

        public BlockStatement(List<SyntaxNode> statements)
        {
            Statements = statements;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            throw new System.NotImplementedException();
        }
    }
}
