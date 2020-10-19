namespace Lox
{

    public static class TokenTypeHelper
    {
        public static int GetUnaryOperatorPrecendence(this SyntaxKind type)
        {
            switch (type)
            {
                case SyntaxKind.Minus:
                case SyntaxKind.Bang:
                    return 6;

                default:
                    return 0;
            }
        }

        public static int GetBinaryOperatorPrecendence(this SyntaxKind type)
        {
            switch (type)
            {
                case SyntaxKind.Star:
                case SyntaxKind.Slash:
                    return 5;

                case SyntaxKind.Minus:
                case SyntaxKind.Plus:
                    return 4;

                case SyntaxKind.EqualEqual:
                case SyntaxKind.BangEqual:
                case SyntaxKind.Less:
                case SyntaxKind.LessEqual:
                case SyntaxKind.Greater:
                case SyntaxKind.GreaterEqual:
                    return 3;

                case SyntaxKind.AndAnd:
                    return 2;
                
                case SyntaxKind.OrOr:
                    return 1;

                default:
                    return 0;
            }
        }

    }
}
