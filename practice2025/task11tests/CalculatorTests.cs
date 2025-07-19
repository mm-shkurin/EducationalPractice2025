using Xunit;
using System;
using task11;

namespace task11tests
{
    public class CalculatorTests
    {
        [Fact]
        public void CreateCalculator_WithValidCode_ReturnsWorkingCalculator()
        {
            var calculator = CalculatorFactory.CreateCalculator();

            Assert.Equal(5, calculator.Add(2, 3));
            Assert.Equal(-1, calculator.Minus(2, 3));
            Assert.Equal(6, calculator.Mul(2, 3));
            Assert.Equal(2, calculator.Div(6, 3));
        }

        [Fact]
        public void CreateCalculator_WithCustomCode_ReturnsWorkingCalculator()
        {
            var customCode = @"
using System;

public class CustomCalculator : task11.ICalculator
{
    public int Add(int a, int b) => a + b + 1;
    public int Minus(int a, int b) => a - b - 1;
    public int Mul(int a, int b) => a * b * 2;
    public int Div(int a, int b) => a / b / 2;
}";

            var calculator = CalculatorFactory.CreateCalculatorFromCode(customCode);
            Assert.Equal(6, calculator.Add(2, 3));  // 2 + 3 + 1
            Assert.Equal(-2, calculator.Minus(2, 3)); // 2 - 3 - 1
            Assert.Equal(12, calculator.Mul(2, 3));   // 2 * 3 * 2
            Assert.Equal(1, calculator.Div(6, 3));    // 6 / 3 / 2
        }

        [Fact]
        public void CreateCalculator_WithInvalidCode_ThrowsException()
        {
            var invalidCode = @"
public class InvalidCalculator : task11.ICalculator
{
    public int Add(int a, int b) => a + b;
    // Missing other methods
}";

            Assert.Throws<InvalidOperationException>(() =>
                CalculatorFactory.CreateCalculatorFromCode(invalidCode));
        }

        [Fact]
        public void CreateCalculator_WithSyntaxError_ThrowsException()
        {
            var invalidCode = @"
public class Calculator : task11.ICalculator
{
    public int Add(int a, int b) => a + b;
    public int Minus(int a, int b) => a - b;
    public int Mul(int a, int b) => a * b;
    public int Div(int a, int b) => a / b;
    // Missing closing brace
";

            Assert.Throws<InvalidOperationException>(() =>
                CalculatorFactory.CreateCalculatorFromCode(invalidCode));
        }

        [Fact]
        public void CreateCalculator_WithDivisionByZero_ThrowsException()
        {
            var calculator = CalculatorFactory.CreateCalculator();

            Assert.Throws<DivideByZeroException>(() => calculator.Div(5, 0));
        }

        [Fact]
        public void CreateCalculator_MethodsCanBeCalledWithoutReflection()
        {
            var calculator = CalculatorFactory.CreateCalculator();

            var addResult = calculator.Add(10, 5);
            var minusResult = calculator.Minus(10, 5);
            var mulResult = calculator.Mul(10, 5);
            var divResult = calculator.Div(10, 5);

            Assert.Equal(15, addResult);
            Assert.Equal(5, minusResult);
            Assert.Equal(50, mulResult);
            Assert.Equal(2, divResult);
        }
    }
}
