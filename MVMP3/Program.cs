using McMaster.Extensions.CommandLineUtils;
using MVMP3.Mappers;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace MVMP3
{
    public class Program
    {

        public string SourcePath { get; }

        public bool Verbose { get; }

        public void Start(string src_dir, string dest_dir)
        {
            var destinationPath = Directory.GetCurrentDirectory();

            //Console.WriteLine($"Source: {SourcePath}");
            //Console.WriteLine($"Searching '{SourcePath}' for music...");

            var musicMapper = new MusicMapper(src_dir);
            musicMapper.Map();

            musicMapper.DisplaySummary();

            //Console.WriteLine($"Saving files under '{destinationPath}'");

            var fileMapper = new FileMapper(dest_dir, musicMapper.Artists);
            fileMapper.Verbose = Verbose;
            fileMapper.Map();
        }
    }
}
