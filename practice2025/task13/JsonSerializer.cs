using System.Text.Json;
using System.Text.Json.Serialization;

namespace task13
{
    public static class JsonSerializer
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static string SerializeToJson<T>(T obj)
        {
            return System.Text.Json.JsonSerializer.Serialize(obj, Options);
        }

        public static T? DeserializeFromJson<T>(string json)
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(json, Options);
        }

        public static void SaveToFile<T>(T obj, string filePath)
        {
            var json = SerializeToJson(obj);
            File.WriteAllText(filePath, json);
        }

        public static T? LoadFromFile<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }

            var json = File.ReadAllText(filePath);
            return DeserializeFromJson<T>(json);
        }

        public static bool ValidateStudent(Student student)
        {
            if (string.IsNullOrWhiteSpace(student.FirstName))
                return false;

            if (string.IsNullOrWhiteSpace(student.LastName))
                return false;

            if (student.BirthDate > DateTime.Now)
                return false;

            if (student.Grades == null)
                return false;

            return student.Grades.All(grade => 
                !string.IsNullOrWhiteSpace(grade.Name) && 
                grade.Grade >= 0 && 
                grade.Grade <= 100);
        }
    }
} 
