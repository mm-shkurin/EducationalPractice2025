using Xunit;
using System.Reflection;

public class AttributeReflectionTests
{
    [Fact]
    public void Class_HasDisplayNameAttribute()
    {
        var type = typeof(SampleClass);
        Assert.True(ReflectionHelper.ClassHasAttribute<DisplayNameAttribute>(type));

        var attribute = ReflectionHelper.GetAttribute<DisplayNameAttribute>(type);
        Assert.NotNull(attribute);
        Assert.Equal("Пример класса", attribute.DisplayName);
    }

    [Fact]
    public void Method_HasDisplayNameAttribute()
    {
        var method = typeof(SampleClass).GetMethod("TestMethod");
        Assert.True(ReflectionHelper.MemberHasAttribute<DisplayNameAttribute>(method));

        var attribute = ReflectionHelper.GetAttribute<DisplayNameAttribute>(method);
        Assert.NotNull(attribute);
        Assert.Equal("Тестовый метод", attribute.DisplayName);
    }

    [Fact]
    public void Property_HasDisplayNameAttribute()
    {
        var prop = typeof(SampleClass).GetProperty("Number");
        Assert.True(ReflectionHelper.MemberHasAttribute<DisplayNameAttribute>(prop));

        var attribute = ReflectionHelper.GetAttribute<DisplayNameAttribute>(prop);
        Assert.NotNull(attribute);
        Assert.Equal("Числовое свойство", attribute.DisplayName);
    }

    [Fact]
    public void Class_HasVersionAttribute()
    {
        var type = typeof(SampleClass);
        Assert.True(ReflectionHelper.ClassHasAttribute<VersionAttribute>(type));

        var attribute = ReflectionHelper.GetAttribute<VersionAttribute>(type);
        Assert.NotNull(attribute);
        Assert.Equal(1, attribute.Major);
        Assert.Equal(0, attribute.Minor);
    }

    [Fact]
    public void GetMembersWithAttribute_ReturnsDecoratedMembers()
    {
        var members = ReflectionHelper.GetMembersWithAttribute<DisplayNameAttribute>(typeof(SampleClass))
            .Select(m => m.Name)
            .ToList();

        Assert.Contains("Number", members);
        Assert.Contains("TestMethod", members);
        Assert.Equal(2, members.Count);
    }
}
