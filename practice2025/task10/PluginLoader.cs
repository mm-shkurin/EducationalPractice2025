using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace task10
{
    public static class PluginLoader
    {
        public static void LoadAndExecutePlugins(string directoryPath)
        {
            var assemblyFiles = Directory.GetFiles(directoryPath, "*.dll");
            
            var assemblies = assemblyFiles
                .Select(file => Assembly.LoadFrom(file))
                .ToList();
            var pluginTypes = assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.GetCustomAttribute<PluginLoadAttribute>() != null)
                .ToList();
            var dependencyGraph = pluginTypes.ToDictionary(
                type => type.Name,
                type => type.GetCustomAttribute<PluginLoadAttribute>()?.Dependencies ?? Array.Empty<string>()
            );
            
            var loadOrder = TopologicalSort(dependencyGraph);
            
            loadOrder
                .Select(pluginName => pluginTypes.First(type => type.Name == pluginName))
                .Where(type => typeof(IPlugin).IsAssignableFrom(type))
                .Select(type => Activator.CreateInstance(type) as IPlugin)
                .Where(plugin => plugin != null)
                .ToList()
                .ForEach(plugin => plugin!.Execute());
        }
        
        private static List<string> TopologicalSort(Dictionary<string, string[]> graph)
        {
            var result = new List<string>();
            var visited = new HashSet<string>();
            var visiting = new HashSet<string>();
            
            void Visit(string node)
            {
                if (visiting.Contains(node))
                    throw new InvalidOperationException($"Circular dependency detected: {node}");
                
                if (visited.Contains(node))
                    return;
                
                visiting.Add(node);
                
                graph.GetValueOrDefault(node, Array.Empty<string>())
                    .ToList()
                    .ForEach(dependency => Visit(dependency));
                
                visiting.Remove(node);
                visited.Add(node);
                result.Add(node);
            }
            graph.Keys.ToList().ForEach(node => Visit(node));
            
            return result;
        }
    }
} 
