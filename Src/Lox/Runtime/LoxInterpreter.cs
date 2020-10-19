using System;
using System.Collections.Generic;
using System.Linq;

namespace Lox
{
    public class LoxInterpreter
    {
        private bool _hadError = false;
        private readonly Evaluator _evaluator = new Evaluator();
        public bool Run(string source)
        {
            Lexer scanner = new Lexer(source);
            scanner.ScanTokens();

            Parser parser = new Parser(scanner.GetTokens().ToList());
            List<SyntaxNode> expressionTree = parser.Parse();

            foreach (Error error in scanner.GetErrors())
            {
                Report(error.Line, error.Where, error.Message);
            }

            foreach (Error error in parser.GetErrors())
            {
                Report(error.Line, error.Where, error.Message);
            }

            Resolver resolver = new Resolver(_evaluator);
            resolver.Resolve(expressionTree);

            bool runEvaluator = true;
            foreach (Error error in resolver.GetErrors())
            {
                Report(error.Line, error.Where, error.Message);
                runEvaluator = false;
            }

            if (runEvaluator)
            {
                try
                {
                    _evaluator.Evaluate(expressionTree);
                }
                catch (RuntimeError error)
                {
                    Report(error.Token.Line, "", error.Message);
                }
            }

            return _hadError;
        }


        private void Report(int line, string where, string message)
        {
            Console.Error.WriteLine($"[line {line} ] Error {where} : {message}");
            _hadError = true;
        }
    }
}
