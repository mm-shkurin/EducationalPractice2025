using System;
using System.Threading;

namespace task17
{
    public class TestCommand : ICommand
    {
        private readonly string _name;
        private readonly int _delayMs;
        private readonly bool _shouldThrow;

        public TestCommand(string name, int delayMs = 0, bool shouldThrow = false)
        {
            _name = name;
            _delayMs = delayMs;
            _shouldThrow = shouldThrow;
        }

        public void Execute()
        {
            if (_delayMs > 0)
            {
                Thread.Sleep(_delayMs);
            }

            if (_shouldThrow)
            {
                throw new InvalidOperationException($"Test command {_name} threw an exception");
            }
        }

        public override string ToString()
        {
            return $"TestCommand({_name})";
        }
    }
} 