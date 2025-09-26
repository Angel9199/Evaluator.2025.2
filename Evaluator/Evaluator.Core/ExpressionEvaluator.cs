using System;
using System.Collections.Generic;
using System.Globalization;

namespace Evaluator.Core
{
    public class ExpressionEvaluator
    {
        public static double Evaluate(string infix)
        {
            var tokens = Tokenize(infix);
            var postfix = InfixToPostfix(tokens);
            return Calculate(postfix);
        }

        private static List<string> Tokenize(string expression)
        {
            var tokens = new List<string>();
            var number = "";

            foreach (char c in expression)
            {
                if (char.IsDigit(c) || c == '.')
                {
                    number += c;
                }
                else
                {
                    if (number.Length > 0)
                    {
                        tokens.Add(number);
                        number = "";
                    }

                    if (!char.IsWhiteSpace(c))
                        tokens.Add(c.ToString());
                }
            }

            if (number.Length > 0)
                tokens.Add(number);

            return tokens;
        }

        private static List<string> InfixToPostfix(List<string> tokens)
        {
            var stack = new Stack<string>();
            var postfix = new List<string>();

            foreach (var token in tokens)
            {
                if (IsOperator(token))
                {
                    if (token == ")")
                    {
                        while (stack.Peek() != "(")
                            postfix.Add(stack.Pop());
                        stack.Pop(); // descarta "("
                    }
                    else
                    {
                        while (stack.Count > 0
                               && PriorityInfix(token) <= PriorityStack(stack.Peek()))
                        {
                            postfix.Add(stack.Pop());
                        }
                        stack.Push(token);
                    }
                }
                else
                {
                    postfix.Add(token);
                }
            }

            while (stack.Count > 0)
                postfix.Add(stack.Pop());

            return postfix;
        }

        private static bool IsOperator(string token) =>
            token == "^"
         || token == "/"
         || token == "*"
         || token == "+"
         || token == "-"
         || token == "("
         || token == ")";

        private static int PriorityInfix(string op) => op switch
        {
            "^" => 4,
            "*" or "/" => 2,
            "+" or "-" => 1,
            "(" => 5,
            _ => throw new Exception($"Operador inválido: {op}"),
        };

        private static int PriorityStack(string op) => op switch
        {
            "^" => 3,
            "*" or "/" => 2,
            "+" or "-" => 1,
            "(" => 0,
            _ => throw new Exception($"Operador inválido: {op}"),
        };

        private static double Calculate(List<string> postfix)
        {
            var stack = new Stack<double>();

            foreach (var token in postfix)
            {
                if (IsOperator(token))
                {
                    var right = stack.Pop();
                    var left = stack.Pop();
                    stack.Push(ApplyOperator(left, token, right));
                }
                else
                {
                    // Parse usando InvariantCulture para aceptar siempre el punto decimal
                    stack.Push(double.Parse(token, CultureInfo.InvariantCulture));
                }
            }

            return stack.Peek();
        }

        private static double ApplyOperator(double left, string op, double right) => op switch
        {
            "*" => left * right,
            "/" => left / right,
            "^" => Math.Pow(left, right),
            "+" => left + right,
            "-" => left - right,
            _ => throw new Exception($"Operador no soportado: {op}"),
        };
    }
}
