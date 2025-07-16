using Xunit;
using task02;
using System.Collections.Generic;
using System.Linq;

public class StudentServiceTests
{
    private readonly List<Student> _testStudents;
    private readonly StudentService _service;

    public StudentServiceTests()
    {
        _testStudents = new List<Student>
        {
            new() { Name = "Иван", Faculty = "ФИТ", Grades = new List<int> { 5, 4, 5 } },
            new() { Name = "Анна", Faculty = "ФИТ", Grades = new List<int> { 3, 4, 3 } },
            new() { Name = "Петр", Faculty = "Экономика", Grades = new List<int> { 5, 5, 5 } },
            new() { Name = "Мария", Faculty = "Экономика", Grades = new List<int> { 4, 5, 3 } }
        };
        _service = new StudentService(_testStudents);
    }

    [Fact]
    public void GetStudentsByFaculty_ReturnsCorrectStudents()
    {
        var result = _service.GetStudentsByFaculty("ФИТ").ToList();
        Assert.Equal(2, result.Count);
        Assert.True(result.All(s => s.Faculty == "ФИТ"));
    }

    [Fact]
    public void GetFacultyWithHighestAverageGrade_ReturnsCorrectFaculty()
    {
        var result = _service.GetFacultyWithHighestAverageGrade();
        Assert.Equal("Экономика", result);
    }

    [Fact]
    public void GetStudentsWithMinAverageGrade_ReturnsCorrectStudents()
    {
        var result = _service.GetStudentsWithMinAverageGrade(4.0).ToList();
        Assert.Equal(3, result.Count);
        Assert.Contains(result, s => s.Name == "Иван");
        Assert.Contains(result, s => s.Name == "Петр");
        Assert.Contains(result, s => s.Name == "Мария");
        Assert.DoesNotContain(result, s => s.Name == "Анна");
    }

    [Fact]
    public void GetStudentsOrderedByName_ReturnsSortedList()
    {
        var result = _service.GetStudentsOrderedByName().ToList();
        var expectedOrder = new[] { "Анна", "Иван", "Мария", "Петр" };
        Assert.Equal(expectedOrder, result.Select(s => s.Name));
    }

    [Fact]
    public void GroupStudentsByFaculty_ReturnsCorrectGroups()
    {
        var lookup = _service.GroupStudentsByFaculty();

        var fitGroup = lookup["ФИТ"].ToList();
        Assert.Equal(2, fitGroup.Count);
        Assert.Contains(fitGroup, s => s.Name == "Иван");
        Assert.Contains(fitGroup, s => s.Name == "Анна");

        var ecoGroup = lookup["Экономика"].ToList();
        Assert.Equal(2, ecoGroup.Count);
        Assert.Contains(ecoGroup, s => s.Name == "Петр");
        Assert.Contains(ecoGroup, s => s.Name == "Мария");
    }

    [Fact]
    public void GetFacultyWithHighestAverageGrade_WithEqualGrades_ReturnsFirstFaculty()
    {
        var students = new List<Student>
        {
            new() { Faculty = "A", Grades = new List<int> { 5, 5 } },
            new() { Faculty = "B", Grades = new List<int> { 5, 5 } }
        };

        var service = new StudentService(students);
        var result = service.GetFacultyWithHighestAverageGrade();
        Assert.Equal("A", result);
    }
}
