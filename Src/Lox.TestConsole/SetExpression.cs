namespace Lox
{
    class SetExpression : SyntaxNode
    {
        public Token Name {get;}
        public SyntaxNode Object {get;}
        public SyntaxNode Value {get;}

        public SyntaxKind Kind => SyntaxKind.SetExpression;

        public SetExpression(SyntaxNode expr, Token name, SyntaxNode value)
        {
            Name = name;
            Object = expr;
            Value = value;
        }
    }
}
