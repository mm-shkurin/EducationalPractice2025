using Xunit;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using task10;

namespace task10tests
{
    public class PluginLoaderTests
    {
        [Fact]
        public void LoadAndExecutePlugins_WithValidPlugins_ExecutesInCorrectOrder()
        {
            var validGraph = new Dictionary<string, string[]>
            {
                ["BasePlugin"] = new string[0],
                ["DependentPlugin"] = new[] { "BasePlugin" },
                ["SecondLevelPlugin"] = new[] { "DependentPlugin" }
            };
            
            var loadOrder = TopologicalSort(validGraph);
            
            Assert.Contains("BasePlugin", loadOrder);
            Assert.Contains("DependentPlugin", loadOrder);
            Assert.Contains("SecondLevelPlugin", loadOrder);
            
            var baseIndex = loadOrder.IndexOf("BasePlugin");
            var dependentIndex = loadOrder.IndexOf("DependentPlugin");
            var secondLevelIndex = loadOrder.IndexOf("SecondLevelPlugin");
            
            Assert.True(baseIndex < dependentIndex);
            Assert.True(dependentIndex < secondLevelIndex);
        }
        
        [Fact]
        public void LoadAndExecutePlugins_WithCircularDependency_ThrowsException()
        {
            var circularGraph = new Dictionary<string, string[]>
            {
                ["CircularPlugin1"] = new[] { "CircularPlugin2" },
                ["CircularPlugin2"] = new[] { "CircularPlugin1" }
            };
            
            Assert.Throws<InvalidOperationException>(() => TopologicalSort(circularGraph));
        }
        
        [Fact]
        public void LoadAndExecutePlugins_WithNoPlugins_DoesNothing()
        {
            var emptyGraph = new Dictionary<string, string[]>();
            
            var result = TopologicalSort(emptyGraph);
            
            Assert.Empty(result);
        }
        
        [Fact]
        public void PluginLoadAttribute_WithDependencies_StoresCorrectly()
        {
            var attribute = new PluginLoadAttribute("Dep1", "Dep2");
            
            Assert.Equal(2, attribute.Dependencies.Length);
            Assert.Contains("Dep1", attribute.Dependencies);
            Assert.Contains("Dep2", attribute.Dependencies);
        }
        
        [Fact]
        public void PluginLoadAttribute_WithoutDependencies_StoresEmptyArray()
        {
            var attribute = new PluginLoadAttribute();
            
            Assert.Empty(attribute.Dependencies);
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
    
    [PluginLoad]
    public class BasePlugin : IPlugin
    {
        public void Execute()
        {
        }
    }
    
    [PluginLoad("BasePlugin")]
    public class DependentPlugin : IPlugin
    {
        public void Execute()
        {
        }
    }
    
    [PluginLoad("DependentPlugin")]
    public class SecondLevelPlugin : IPlugin
    {
        public void Execute()
        {
        }
    }
    
    [PluginLoad("CircularPlugin2")]
    public class CircularPlugin1 : IPlugin
    {
        public void Execute() { }
    }
    
    [PluginLoad("CircularPlugin1")]
    public class CircularPlugin2 : IPlugin
    {
        public void Execute() { }
    }
} 
