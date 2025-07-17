using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace task09
{
    public static class AssemblyBrowser
    {
        public static void AnalyzeAssembly(string path)
        {
            var assembly = Assembly.LoadFrom(path);

            assembly.GetTypes()
                .GroupBy(t => t.Namespace)
                .OrderBy(g => g.Key)
                .ToList()
                .ForEach(nsGroup =>
                {
                    Console.WriteLine($"\n[NAMESPACE: {nsGroup.Key ?? "<global>"}]");

                    nsGroup.OrderBy(t => t.Name)
                        .ToList()
                        .ForEach(type => PrintTypeInfo(type));
                });
        }

        private static void PrintTypeInfo(Type type)
        {
            Console.WriteLine($"\n  CLASS: {type.Name}");

            type.GetCustomAttributes()
                .Select(attr => attr.GetType().Name)
                .ToList()
                .ForEach(attr => Console.WriteLine($"    Attribute: {attr}"));

            type.GetConstructors()
                .Select(ctor => new
                {
                    Name = ctor.Name,
                    Parameters = ctor.GetParameters()
                        .Select(p => $"{p.ParameterType.Name} {p.Name}")
                        .ToList()
                })
                .ToList()
                .ForEach(ctor =>
                    Console.WriteLine($"    Constructor: {type.Name}({string.Join(", ", ctor.Parameters)})"));

            type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Where(m => !m.IsSpecialName)
                .Select(m => new
                {
                    m.Name,
                    ReturnType = m.ReturnType.Name,
                    Parameters = m.GetParameters()
                        .Select(p => $"{p.ParameterType.Name} {p.Name}")
                        .ToList()
                })
                .ToList()
                .ForEach(method =>
                    Console.WriteLine($"    Method: {method.ReturnType} {method.Name}({string.Join(", ", method.Parameters)})"));

            type.GetProperties()
                .SelectMany(prop => new[]
                {
                    prop.GetMethod,
                    prop.SetMethod
                })
                .Where(method => method != null)
                .Select(method => new
                {
                    method!.Name,
                    ReturnType = method.ReturnType.Name,
                    Parameters = method.GetParameters()
                        .Select(p => $"{p.ParameterType.Name} {p.Name}")
                        .ToList()
                })
                .ToList()
                .ForEach(method =>
                    Console.WriteLine($"    Method: {method.ReturnType} {method.Name}({string.Join(", ", method.Parameters)})"));
        }
    }
}
