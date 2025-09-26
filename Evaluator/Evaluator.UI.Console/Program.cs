using Evaluator.Core;

Console.WriteLine("Hello, Evaluator");
var infix = "123.89^(1.6/2.789)";
var result = ExpressionEvaluator.Evaluate(infix);
Console.WriteLine($"{infix} = {result}");
