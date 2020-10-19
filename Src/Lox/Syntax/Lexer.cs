using System;
using System.Collections.Generic;
using static Lox.Functional;

namespace Lox
{
    public class Lexer
    {
        private string _source;
        internal List<Token> _tokens = new List<Token>();
        internal List<Error> _errors = new List<Error>();

        private int _start = 0;
        private int _current = 0;
        private int _line = 1;

        internal readonly Dictionary<string, SyntaxKind> Keywords = new Dictionary<string, SyntaxKind>
        {
            ["if"] = SyntaxKind.If,
            ["else"] = SyntaxKind.Else,
            ["true"] = SyntaxKind.True,
            ["false"] = SyntaxKind.False,
            ["and"] = SyntaxKind.AndAnd,
            ["or"] = SyntaxKind.OrOr,
            ["for"] = SyntaxKind.For,
            ["while"] = SyntaxKind.While,
            ["funtion"] = SyntaxKind.Fun,
            ["null"] = SyntaxKind.Nil,
            ["return"] = SyntaxKind.Return,
            ["class"] = SyntaxKind.Class,
            ["this"] = SyntaxKind.This,
            ["super"] = SyntaxKind.Super,
            ["let"] = SyntaxKind.Var,
            ["var"] = SyntaxKind.Var,
            ["print"] = SyntaxKind.Print,
        };

        internal IEnumerable<Token> GetTokens()
        {
            return _tokens;
        }

        internal IEnumerable<Error> GetErrors()
        {
            return _errors;
        }

        public Lexer(string source)
        {
            _source = source;

        }

        public void ScanTokens()
        {

            while (!IsAtEnd)
            {
                _start = _current;

                ScanToken();
            }

            _tokens.Add(new Token(SyntaxKind.Eof, "", None, _line));
        }

        private bool IsAtEnd => _current >= _source.Length;

        private void ScanToken()
        {
            
            char c = AdvanceChar();
            Next();
            switch (c)
            {
                case '(':
                   // Advance();
                    AddToken(SyntaxKind.LeftParen); 
                    break;

                case ')':
                    //Advance();
                    AddToken(SyntaxKind.RightParen); 
                    break;

                case '{': 
                    AddToken(SyntaxKind.LeftBrace); 
                    break;

                case '}': 
                    AddToken(SyntaxKind.RightBrace); 
                    break;

                case ',': AddToken(SyntaxKind.Comma); 
                    break;

                case '.': AddToken(SyntaxKind.Dot); 
                    break;

                case '-': AddToken(SyntaxKind.Minus); 
                    break;

                case '+': AddToken(SyntaxKind.Plus); 
                    break;

                case ';': AddToken(SyntaxKind.Semicolon); 
                    break;

                case '*': AddToken(SyntaxKind.Star); 
                    break;


                case '!': AddToken(Match('=') ? SyntaxKind.BangEqual : SyntaxKind.Bang); 
                    break;

                case '=': AddToken(Match('=') ? SyntaxKind.EqualEqual : SyntaxKind.Equal); 
                    break;

                case '<': AddToken(Match('=') ? SyntaxKind.LessEqual : SyntaxKind.Less); 
                    break;

                case '>': AddToken(Match('=') ? SyntaxKind.GreaterEqual : SyntaxKind.Greater); 
                    break;

                case '&': AddToken(SyntaxKind.And); 
                    break;

                case '|': AddToken(SyntaxKind.Or); 
                    break;


                case '/':
                    if (Match('/')) //comment
                    {
                        while (Peek() != '\n' && !IsAtEnd)
                        {
                            Next();
                        }
                    }
                    else
                    {
                        AddToken(SyntaxKind.Slash);
                    }
                    break;

                // ignore whitespace
                case ' ':
                case '\r':
                case '\t':
                    break;

                case '\n':
                    _line++;
                    break;

                case '"': String(); 
                    break;

                default:
                    if (IsDigit(c))
                    {
                        Number();
                    }
                    else if (IsAlpha(c))
                    {
                        Identifier();
                    }
                    else
                    {
                        Error(_line, "Unexpected character.");
                    }
                    break;
            }

        }

        private char AdvanceChar()
        {
            return _source[_current];
        }

        private void Next()
        {
            _current++;
        }

        private bool Match(char expected)
        {
            if (IsAtEnd)
            {
                return false;
            }

            if (_source[_current] != expected)
            {
                return false;
            }

            // consume character in case it matches
            Next();
            return true;
        }

        private char Peek(int num = 0)
        {
            switch (num)
            {
                case 0:
                    if (IsAtEnd)
                    {
                        return '\0';
                    }

                    return _source[_current];
                case 1:
                    if (_current + 1 >= _source.Length)
                    {
                        return '\0';
                    }

                    return _source[_current + num];
                default:
                    throw new NotSupportedException($"Look Ahead of {num} is not supported.");
            }

        }

        private void AddToken(SyntaxKind type)
        {
            AddToken(type, None);
        }

        private void AddToken(SyntaxKind type, object literal)
        {
            string text = _source.Substring(_start, _current - _start);
            _tokens.Add(new Token(type, text, literal, _line));
        }

        private void Error(int line, string message)
        {
            _errors.Add(new Error(ErrorType.SyntaxError, line, message));
        }

        private void String()
        {
            while (Peek() != '"' && !IsAtEnd)
            {
                if (Peek() == '\n')
                {
                    _line++;
                }

                Next();
            }

            // unterminated string
            if (IsAtEnd)
            {
                Error(_line, "Unterminated string.");
                return;
            }

            //the closing "
            Next();

            string value = _source.Substring(_start + 1, _current - _start - 2 /*remove quotes*/);
            AddToken(SyntaxKind.String, value);
        }

        private void Number()
        {
            while (IsDigit(Peek()))
            {
                Next();
            }

            if (Peek() == '.' && IsDigit(Peek(1)))
            {
                Next();
                while (IsDigit(Peek()))
                {
                    Next();
                }
            }

            AddToken(SyntaxKind.Number, double.Parse(_source.Substring(_start, _current - _start)));
        }

        private void Identifier()
        {
            while (IsAlphaNumeric(Peek()))
            {
                Next();
            }

            string? text = _source.Substring(_start, _current - _start);

            if (Keywords.TryGetValue(text, out SyntaxKind tokenType))
            {
                AddToken(tokenType);
            }
            else
            {
                AddToken(SyntaxKind.Identifier);
            }
        }

        private bool IsAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_';
        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private bool IsAlphaNumeric(char c)
        {
            return IsAlpha(c) || IsDigit(c);
        }
    }
}
