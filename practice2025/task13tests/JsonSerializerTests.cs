using Xunit;
using System;
using System.IO;
using System.Linq;
using task13;
using System.Collections.Generic;

namespace task13tests
{
    public class JsonSerializerTests
    {
        [Fact]
        public void SerializeToJson_WithValidStudent_ReturnsValidJson()
        {
            var student = CreateTestStudent();
            
            var json = JsonSerializer.SerializeToJson(student);
            
            Assert.NotNull(json);
            Assert.Contains("firstName", json);
            Assert.Contains("lastName", json);
            Assert.Contains("birthDate", json);
            Assert.Contains("grades", json);
        }
        
        [Fact]
        public void DeserializeFromJson_WithValidJson_ReturnsValidStudent()
        {
            var originalStudent = CreateTestStudent();
            var json = JsonSerializer.SerializeToJson(originalStudent);
            
            var deserializedStudent = JsonSerializer.DeserializeFromJson<Student>(json);
            
            Assert.NotNull(deserializedStudent);
            Assert.Equal(originalStudent.FirstName, deserializedStudent!.FirstName);
            Assert.Equal(originalStudent.LastName, deserializedStudent.LastName);
            Assert.Equal(originalStudent.BirthDate.Date, deserializedStudent.BirthDate.Date);
            Assert.Equal(originalStudent.Grades.Count, deserializedStudent.Grades.Count);
        }
        
        [Fact]
        public void SaveToFile_WithValidStudent_SavesToFile()
        {
            var student = CreateTestStudent();
            var filePath = Path.GetTempFileName();
            
            try
            {
                JsonSerializer.SaveToFile(student, filePath);
                
                Assert.True(File.Exists(filePath));
                var fileContent = File.ReadAllText(filePath);
                Assert.Contains("firstName", fileContent);
            }
            finally
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }
        
        [Fact]
        public void LoadFromFile_WithValidFile_ReturnsStudent()
        {
            var student = CreateTestStudent();
            var filePath = Path.GetTempFileName();
            
            try
            {
                JsonSerializer.SaveToFile(student, filePath);
                
                var loadedStudent = JsonSerializer.LoadFromFile<Student>(filePath);
                
                Assert.NotNull(loadedStudent);
                Assert.Equal(student.FirstName, loadedStudent!.FirstName);
            }
            finally
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }
        
        [Fact]
        public void LoadFromFile_WithNonExistentFile_ThrowsException()
        {
            var nonExistentPath = "non_existent_file.json";
            
            Assert.Throws<FileNotFoundException>(() => 
                JsonSerializer.LoadFromFile<Student>(nonExistentPath));
        }
        
        [Fact]
        public void ValidateStudent_WithValidStudent_ReturnsTrue()
        {
            var student = CreateTestStudent();
            
            var isValid = JsonSerializer.ValidateStudent(student);
            
            Assert.True(isValid);
        }
        
        [Fact]
        public void ValidateStudent_WithInvalidStudent_ReturnsFalse()
        {
            var invalidStudent = new Student
            {
                FirstName = "",
                LastName = "Doe",
                BirthDate = DateTime.Now.AddDays(1),
                Grades = new List<Subject>()
            };
            
            var isValid = JsonSerializer.ValidateStudent(invalidStudent);
            
            Assert.False(isValid);
        }
        
        [Fact]
        public void ValidateStudent_WithInvalidGrades_ReturnsFalse()
        {
            var student = CreateTestStudent();
            student.Grades.Add(new Subject { Name = "", Grade = 150 });
            
            var isValid = JsonSerializer.ValidateStudent(student);
            
            Assert.False(isValid);
        }
        
        [Fact]
        public void DateTimeConverter_FormatsDateCorrectly()
        {
            var student = CreateTestStudent();
            var json = JsonSerializer.SerializeToJson(student);
            
            Assert.Contains("2000-01-15", json);
        }
        
        private static Student CreateTestStudent()
        {
            return new Student
            {
                FirstName = "John",
                LastName = "Doe",
                BirthDate = new DateTime(2000, 1, 15),
                Grades = new List<Subject>
                {
                    new Subject { Name = "Math", Grade = 85 },
                    new Subject { Name = "Physics", Grade = 92 }
                }
            };
        }
    }
} 
