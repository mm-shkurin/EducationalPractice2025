using System;
using System.IO;
using System.Linq;
using CommandLib;

namespace FileSystemCommands
{
    public class FindFilesCommand : ICommand
    {
        private readonly string _path;
        private readonly string _pattern;

        public FindFilesCommand(string path, string pattern)
        {
            _path = path;
            _pattern = pattern;
        }

        public void Execute()
        {
            if (!Directory.Exists(_path))
            {
                Console.WriteLine($"Directory not found: {_path}");
                return;
            }

            var files = Directory.EnumerateFiles(_path, _pattern, SearchOption.AllDirectories)
                                 .Select(Path.GetFileName)
                                 .ToList();

            Console.WriteLine($"Found {files.Count} files:");
            files.ForEach(Console.WriteLine);
        }
    }
}
