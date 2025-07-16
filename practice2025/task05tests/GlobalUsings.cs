using Xunit;
using System;
using System.Linq;

public class ClassAnalyzerTests
{
    [Fact]
    public void GetPublicMethods_ReturnsCorrectMethods()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var methods = analyzer.GetPublicMethods().ToList();
        
        Assert.Contains("Method", methods);
        Assert.DoesNotContain("get_Property", methods);
        Assert.DoesNotContain("set_Property", methods);
        Assert.Single(methods);
    }

    [Fact]
    public void GetAllFields_IncludesPrivateFields_ExcludesCompilerGenerated()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var fields = analyzer.GetAllFields().ToList();
        
        Assert.Contains("PublicField", fields);
        Assert.Contains("_privateField", fields);
        Assert.DoesNotContain("<Property>k__BackingField", fields);
        Assert.Equal(2, fields.Count);
    }

    [Fact]
    public void GetProperties_ReturnsProperties()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var properties = analyzer.GetProperties().ToList();
        
        Assert.Contains("Property", properties);
        Assert.Single(properties);
    }

    [Fact]
    public void GetClassAttributes_ReturnsAttributes()
    {
        var analyzer = new ClassAnalyzer(typeof(AttributedClass));
        var attributes = analyzer.GetClassAttributes().ToList();
        
        Assert.Contains("SerializableAttribute", attributes);
        Assert.Single(attributes);
    }

    [Fact]
    public void GetClassAttributes_ForClassWithoutAttributes_ReturnsEmpty()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var attributes = analyzer.GetClassAttributes().ToList();
        
        Assert.Empty(attributes);
    }
}

public class TestClass
{
    public int PublicField;
    private string _privateField;
    public int Property { get; set; }

    public void Method() { }
}

[Serializable]
public class AttributedClass { }
