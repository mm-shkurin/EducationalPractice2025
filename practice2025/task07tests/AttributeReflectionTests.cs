using Xunit;
using task07;  
using System.Linq;
namespace task07tests
{
    public class AttributeReflectionTests
    {
        [Fact]
        public void ClassHasAttribute_DisplayName_ReturnsTrue()
        {
            var type = typeof(SampleClass);
            
            var hasAttribute = ReflectionHelper.ClassHasAttribute<DisplayNameAttribute>(type);
            
            Assert.True(hasAttribute);
        }

        [Fact]
        public void GetAttribute_DisplayName_ReturnsCorrectName()
        {
            var type = typeof(SampleClass);
            
            var attribute = ReflectionHelper.GetAttribute<DisplayNameAttribute>(type);
            
            Assert.NotNull(attribute);
            Assert.Equal("Sample Class", attribute.Name);
        }

        [Fact]
        public void MemberHasAttribute_PropertyDisplayName_ReturnsTrue()
        {
            var property = typeof(SampleClass).GetProperty("Number");
            
            var hasAttribute = ReflectionHelper.MemberHasAttribute<DisplayNameAttribute>(property);
            
            Assert.True(hasAttribute);
        }

        [Fact]
        public void GetAttribute_PropertyDisplayName_ReturnsCorrectName()
        {
            var property = typeof(SampleClass).GetProperty("Number");
            
            var attribute = ReflectionHelper.GetAttribute<DisplayNameAttribute>(property);
            
            Assert.NotNull(attribute);
            Assert.Equal("Number Property", attribute.Name);
        }

        [Fact]
        public void ClassHasAttribute_Version_ReturnsTrue()
        {
            var type = typeof(SampleClass);
            
            var hasAttribute = ReflectionHelper.ClassHasAttribute<VersionAttribute>(type);
            
            Assert.True(hasAttribute);
        }

        [Fact]
        public void GetMembersWithAttribute_ReturnsCorrectCount()
        {
            var members = ReflectionHelper.GetMembersWithAttribute<DisplayNameAttribute>(typeof(SampleClass));
            
            Assert.Equal(3, members.Count()); 
        }
    }
}