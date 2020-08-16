namespace Lox
{
    class VariableDeclarationStatement : SyntaxNode
    {
        public Token Name { get; }
        public SyntaxNode Initializer { get; }

        public SyntaxKind Kind => SyntaxKind.VariableDeclarationStatement;

        public VariableDeclarationStatement(Token name, SyntaxNode initializer)
        {
            Name = name;
            Initializer = initializer;
        }
    }
}
