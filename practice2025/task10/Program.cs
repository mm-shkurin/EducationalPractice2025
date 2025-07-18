using System;

namespace task10
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Plugin Loader Example");
            Console.WriteLine("=====================");
            try
            {
                PluginLoader.LoadAndExecutePlugins(".");
                Console.WriteLine("All plugins loaded and executed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading plugins: {ex.Message}");
            }
        }
    }
} 
