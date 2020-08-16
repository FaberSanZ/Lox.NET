namespace Lox
{
    public enum TokenType
    {
        // single character tokens
        LeftParen,
        RightParen,
        LeftBrace,
        RightBrace,
        Comma,
        Dot, 
        Minus,
        Plus,
        Semicolon,
        Slash,
        Star,

        // one or two character tokens
        Bang, 
        BangEqual,
        Equal,
        EqualEqual,
        Greater,
        GreaterEqual,
        Less,
        LessEqual,
        And,
        AndAnd,
        Or,
        OrOr,

        // literals
        Identifier,
        String,
        Number,

        // keywords
        False,
        True,
        Else,
        If,
        Fun,
        Return,
        For,
        While,
        Nil,
        Class,
        This,
        Super,
        Var,
        Print,

        // end of file
        Eof
    }

    public static class TokenTypeHelper
    {
        public static int GetUnaryOperatorPrecendence(this TokenType type)
        {
            switch (type)
            {
                case TokenType.Minus:
                case TokenType.Bang:
                    return 6;

                default:
                    return 0;
            }
        }

        public static int GetBinaryOperatorPrecendence(this TokenType type)
        {
            switch (type)
            {
                case TokenType.Star:
                case TokenType.Slash:
                    return 5;

                case TokenType.Minus:
                case TokenType.Plus:
                    return 4;

                case TokenType.EqualEqual:
                case TokenType.BangEqual:
                case TokenType.Less:
                case TokenType.LessEqual:
                case TokenType.Greater:
                case TokenType.GreaterEqual:
                    return 3;

                case TokenType.AndAnd:
                    return 2;
                
                case TokenType.OrOr:
                    return 1;

                default:
                    return 0;
            }
        }

    }
}
