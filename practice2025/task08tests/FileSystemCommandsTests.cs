using System;
using System.IO;
using System.Linq;
using Xunit;
using FileSystemCommands;
using CommandLib;

public class FileSystemCommandsTests
{
    [Fact]
    public void DirectorySizeCommand_ShouldCalculateSize()
    {
        string testDir = CreateTestDirectory();
        try
        {
            File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Test content");
            File.WriteAllText(Path.Combine(testDir, "file2.txt"), "Another test");

            var originalOut = Console.Out;
            try
            {
                using var sw = new StringWriter();
                Console.SetOut(sw);

                var command = new DirectorySizeCommand(testDir);
                command.Execute();

                string output = sw.ToString();
                Assert.Contains("Directory size", output);
                Assert.Contains("bytes", output);
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }
        finally
        {
            Directory.Delete(testDir, true);
        }
    }

    [Fact]
    public void FindFilesCommand_ShouldFindMatchingFiles()
    {
        string testDir = CreateTestDirectory();
        try
        {
            File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
            File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");

            var originalOut = Console.Out;
            try
            {
                using var sw = new StringWriter();
                Console.SetOut(sw);

                var command = new FindFilesCommand(testDir, "*.txt");
                command.Execute();

                string output = sw.ToString();
                Assert.Contains("Found 1 files", output);
                Assert.Contains("file1.txt", output);
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }
        finally
        {
            Directory.Delete(testDir, true);
        }
    }

    [Fact]
    public void DirectorySizeCommand_ShouldHandleMissingDirectory()
    {
        string invalidPath = Path.Combine(Path.GetTempPath(), "NonExistentDir");

        var originalOut = Console.Out;
        try
        {
            using var sw = new StringWriter();
            Console.SetOut(sw);

            var command = new DirectorySizeCommand(invalidPath);
            command.Execute();

            string output = sw.ToString();
            Assert.Contains($"Directory not found: {invalidPath}", output);
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    private string CreateTestDirectory()
    {
        string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(path);
        return path;
    }
}
