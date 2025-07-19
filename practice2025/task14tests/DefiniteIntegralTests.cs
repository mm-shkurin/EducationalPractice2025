using Xunit;
using task14;

namespace task14tests
{
    public class DefiniteIntegralTests
    {
        [Fact]
        public void TestLinearFunction_ZeroResult()
        {
            var X = (double x) => x;
            double result = DefiniteIntegral.Solve(-1, 1, X, 1e-4, 2);
            Assert.Equal(0, result, 1e-4);
        }

        [Fact]
        public void TestSinFunction_ZeroResult()
        {
            var SIN = (double x) => Math.Sin(x);
            double result = DefiniteIntegral.Solve(-1, 1, SIN, 1e-5, 8);
            Assert.Equal(0, result, 1e-4);
        }

        [Fact]
        public void TestLinearFunction_PositiveResult()
        {
            var X = (double x) => x;
            double result = DefiniteIntegral.Solve(0, 5, X, 1e-6, 8);
            Assert.Equal(12.5, result, 1e-5);
        }

        [Fact]
        public void TestConstantFunction()
        {
            var CONST = (double x) => 2.0;
            double result = DefiniteIntegral.Solve(0, 3, CONST, 1e-4, 4);
            Assert.Equal(6, result, 1e-4);
        }

        [Fact]
        public void TestQuadraticFunction()
        {
            var QUAD = (double x) => x * x;
            double result = DefiniteIntegral.Solve(0, 2, QUAD, 1e-6, 6);
            Assert.Equal(8.0 / 3.0, result, 1e-3);
        }

        [Fact]
        public void TestDifferentThreadCounts()
        {
            var X = (double x) => x;
            double result1 = DefiniteIntegral.Solve(0, 1, X, 1e-4, 1);
            double result2 = DefiniteIntegral.Solve(0, 1, X, 1e-4, 2);
            double result4 = DefiniteIntegral.Solve(0, 1, X, 1e-4, 4);
            double result8 = DefiniteIntegral.Solve(0, 1, X, 1e-4, 8);

            Assert.Equal(0.5, result1, 1e-4);
            Assert.Equal(0.5, result2, 1e-4);
            Assert.Equal(0.5, result4, 1e-4);
            Assert.Equal(0.5, result8, 1e-4);
        }

        [Fact]
        public void TestSmallStep()
        {
            var SIN = (double x) => Math.Sin(x);
            double result = DefiniteIntegral.Solve(0, Math.PI, SIN, 1e-6, 4);
            Assert.Equal(2, result, 1e-5);
        }
    }
}
