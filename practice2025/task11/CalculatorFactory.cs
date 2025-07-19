using System;
using System.Linq;

namespace task11
{
    public static class CalculatorFactory
    {
        private const string CalculatorTemplate = @"
using System;

public class Calculator : task11.ICalculator
{
    public int Add(int a, int b) => a + b;
    public int Minus(int a, int b) => a - b;
    public int Mul(int a, int b) => a * b;
    public int Div(int a, int b) => a / b;
}";

        public static ICalculator CreateCalculator()
        {
            return DynamicCompiler.CompileAndCreateInstance<ICalculator>(CalculatorTemplate);
        }

        public static ICalculator CreateCalculatorFromCode(string sourceCode)
        {
            return DynamicCompiler.CompileAndCreateInstance<ICalculator>(sourceCode);
        }
    }
}
