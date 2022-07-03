using McMaster.Extensions.CommandLineUtils;
using MVMP3.Mappers;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace MVMP3
{
    [Command(Description = "")]
    internal class Program
    {
        [Option("--source", "Base directory for MP3 files to search under.", CommandOptionType.SingleValue)]
        public string SourcePath { get; }

        [Option]
        public bool Verbose { get; }

        private static void Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        public void OnExecute()
        {
            var destinationPath = Directory.GetCurrentDirectory();

            Console.WriteLine($"Source: {SourcePath}");
            Console.WriteLine($"Searching '{SourcePath}' for music...");

            var musicMapper = new MusicMapper(@"d:\OneDrive\music");
            musicMapper.Map();

            musicMapper.DisplaySummary();

            Console.WriteLine($"Saving files under '{destinationPath}'");

            var fileMapper = new FileMapper(@"d:\tmp", musicMapper.Artists);
            fileMapper.Verbose = Verbose;
            fileMapper.Map();
        }
    }
}
