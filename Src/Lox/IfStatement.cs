using System.Collections.Generic;

namespace Lox
{
    class IfStatement : SyntaxNode 
    {
        public SyntaxNode Condition {get;}
        public SyntaxNode ThenBranch {get;}
        public SyntaxNode ElseBranch {get;}
        public override SyntaxKind Kind => SyntaxKind.IfStatement;

        public IfStatement(SyntaxNode condition, SyntaxNode thenBranch, SyntaxNode elseBranch)
        {
            Condition = condition;
            ThenBranch = thenBranch;
            ElseBranch = elseBranch;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            throw new System.NotImplementedException();
        }
    }
}
