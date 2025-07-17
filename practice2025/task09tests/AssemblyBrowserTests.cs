using System;
using System.IO;
using Xunit;
using task09;
using task07; 

namespace task09tests
{
    public class AssemblyBrowserTests
    {
        [Fact]
        public void AnalyzeAssembly_ShouldReturnCorrectMetadata()
        {
            var testAssembly = typeof(SampleClass).Assembly.Location;
            var consoleOut = new StringWriter();
            Console.SetOut(consoleOut);

            AssemblyBrowser.AnalyzeAssembly(testAssembly);
            var output = consoleOut.ToString();

            Assert.Contains("[NAMESPACE: task07]", output);
            Assert.Contains("CLASS: DisplayNameAttribute", output);
            Assert.Contains("CLASS: ReflectionHelper", output);
            Assert.Contains("CLASS: VersionAttribute", output);
            Assert.Contains("Constructor: VersionAttribute(Int32 major, Int32 minor)", output);
            Assert.Contains("Method: Void TestMethod()", output);
            Assert.Contains("Method: Int32 get_Number()", output);
            Assert.Contains("Method: Void set_Number(Int32 value)", output);
        }

        [Fact]
        public void AnalyzeAssembly_InvalidPath_ShouldThrowException()
        {
            var invalidPath = "invalid_assembly.dll";

            var ex = Assert.Throws<FileNotFoundException>(() => AssemblyBrowser.AnalyzeAssembly(invalidPath));
            Assert.Contains("Could not load file or assembly", ex.Message);
        }
    }

    [DisplayName("Test Class")] 
    [Version(2, 0)]            
    public class TestClass
    {
        [DisplayName("Test Property")] 
        public int TestProperty { get; set; }

        [DisplayName("Test Method")] 
        public void TestMethod() { }
    }
}