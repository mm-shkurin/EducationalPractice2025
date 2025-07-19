using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

public class ClassAnalyzer
{
    private readonly Type _type;

    public ClassAnalyzer(Type type) => _type = type;
    public IEnumerable<string> GetPublicMethods() =>
        _type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Where(m => !m.IsSpecialName)
            .Select(m => m.Name);
    public IEnumerable<string> GetAllFields() =>
        _type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
            .Where(f => !f.IsDefined(typeof(CompilerGeneratedAttribute), false))
            .Select(f => f.Name);
    public IEnumerable<string> GetProperties() =>
        _type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => p.Name);
    public IEnumerable<string> GetClassAttributes() =>
        _type.GetCustomAttributes(true)
            .Select(a => a.GetType().Name);
}
