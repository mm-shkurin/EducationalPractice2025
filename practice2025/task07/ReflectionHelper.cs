#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace task07;

public static class ReflectionHelper
{
    public static bool ClassHasAttribute<TAttribute>(Type type)
        where TAttribute : Attribute
        => type.GetCustomAttributes(typeof(TAttribute), false).Any();

    public static bool MemberHasAttribute<TAttribute>(MemberInfo member)
        where TAttribute : Attribute
        => member.GetCustomAttributes(typeof(TAttribute), false).Any();

    public static IEnumerable<MemberInfo> GetMembersWithAttribute<TAttribute>(Type type)
        where TAttribute : Attribute
    {
        var members = new List<MemberInfo>();
        
        if (type.GetCustomAttributes(typeof(TAttribute), false).Any())
        {
            members.Add(type);
        }
        
        members.AddRange(type.GetMembers().Where(m => m.GetCustomAttributes(typeof(TAttribute), false).Any()));
        
        return members;
    }

    public static TAttribute? GetAttribute<TAttribute>(MemberInfo member)
        where TAttribute : Attribute
        => member.GetCustomAttributes(typeof(TAttribute), false)
                 .OfType<TAttribute>()
                 .FirstOrDefault();
}