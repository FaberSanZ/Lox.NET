using System;
using System.Linq;

namespace Lox
{
    sealed class LoxInterpreter
    {
        private bool _hadError = false;
        private Evaluator _evaluator = new Evaluator();
        public bool Run(string source)
        {
            var scanner = new Scanner(source);
            scanner.ScanTokens();

            var parser = new Parser(scanner.GetTokens().ToList());
            var expressionTree = parser.Parse();

            foreach (var error in scanner.GetErrors())
            {
                Report(error.Line, error.Where , error.Message);
            }

            foreach (var error in parser.GetErrors())
            {
                Report(error.Line, error.Where,  error.Message);
            }

            var resolver = new Resolver(_evaluator);
            resolver.Resolve(expressionTree);

            var runEvaluator = true;
             foreach (var error in resolver.GetErrors())
            {
                Report(error.Line, error.Where,  error.Message);
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
