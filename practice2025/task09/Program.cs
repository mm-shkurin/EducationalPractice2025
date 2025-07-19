using System;

namespace task09
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: dotnet run -- <path-to-assembly>");
                return;
            }

            try
            {
                AssemblyBrowser.AnalyzeAssembly(args[0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error analyzing assembly: {ex.Message}");
            }
        }
    }
}
