using System;

namespace task11
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Dynamic Calculator Compilation Example");
            Console.WriteLine("=====================================");

            try
            {
                var calculator = CalculatorFactory.CreateCalculator();

                Console.WriteLine("Calculator created successfully!");
                Console.WriteLine();

                Console.WriteLine($"Add(10, 5) = {calculator.Add(10, 5)}");
                Console.WriteLine($"Minus(10, 5) = {calculator.Minus(10, 5)}");
                Console.WriteLine($"Mul(10, 5) = {calculator.Mul(10, 5)}");
                Console.WriteLine($"Div(10, 5) = {calculator.Div(10, 5)}");

                Console.WriteLine();
                Console.WriteLine("All operations completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
