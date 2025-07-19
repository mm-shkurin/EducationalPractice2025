using System;
using System.IO;
using System.Linq;
using System.Reflection;
using CommandLib;

namespace CommandRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: CommandRunner <command> [parameters]");
                return;
            }

            string commandName = args[0];
            string[] commandArgs = args.Skip(1).ToArray();
            var commands = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*.dll")
                .Select(Assembly.LoadFrom)
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(ICommand).IsAssignableFrom(t) && !t.IsInterface)
                .ToDictionary(t => t.Name, t => t);

            if (commands.TryGetValue(commandName, out Type commandType))
            {
                ICommand command = (ICommand)Activator.CreateInstance(commandType, commandArgs);
                command.Execute();
            }
            else
            {
                Console.WriteLine($"Unknown command: {commandName}");
                Console.WriteLine("Available commands: " + string.Join(", ", commands.Keys));
            }
        }
    }
}
