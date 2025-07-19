using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
        => type.GetMembers().Where(m => m.GetCustomAttributes(typeof(TAttribute), false).Any());
    public static TAttribute GetAttribute<TAttribute>(MemberInfo member)
        where TAttribute : Attribute
        => member.GetCustomAttributes(typeof(TAttribute), false).FirstOrDefault() as TAttribute;
}
