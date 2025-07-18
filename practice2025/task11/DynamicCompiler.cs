using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System.Reflection;
using System.IO;

namespace task11
{
    public static class DynamicCompiler
    {
        public static T CompileAndCreateInstance<T>(string sourceCode) where T : class
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

            var references = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(T).Assembly.Location)
            };

            var compilation = CSharpCompilation.Create(
                assemblyName: "DynamicAssembly",
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            );

            using var ms = new MemoryStream();
            var result = compilation.Emit(ms);

            if (!result.Success)
            {
                var errors = result.Diagnostics
                    .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
                    .Select(diagnostic => diagnostic.GetMessage())
                    .ToList();

                throw new InvalidOperationException($"Compilation failed: {string.Join(", ", errors)}");
            }

            ms.Position = 0;
            var assembly = Assembly.Load(ms.ToArray());

            var type = assembly.GetTypes()
                .FirstOrDefault(t => typeof(T).IsAssignableFrom(t));

            if (type == null)
            {
                throw new InvalidOperationException($"No type implementing {typeof(T).Name} found in compiled code");
            }
            return Activator.CreateInstance(type) as T
                ?? throw new InvalidOperationException($"Failed to create instance of {type.Name}");
        }
    }
}
