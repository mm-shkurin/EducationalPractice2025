using System.Text.Json.Serialization;

namespace task13
{
    public class Student
    {
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; } = string.Empty;
        
        [JsonPropertyName("lastName")]
        public string LastName { get; set; } = string.Empty;
        
        [JsonPropertyName("birthDate")]
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime BirthDate { get; set; }
        
        [JsonPropertyName("grades")]
        public List<Subject> Grades { get; set; } = new List<Subject>();
    }
} 
