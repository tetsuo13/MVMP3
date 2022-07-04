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

        public void Start(string src_dir, string dest_dir, NLog.Logger log)
        {
            var destinationPath = Directory.GetCurrentDirectory();

            log.Info ($"Source: {SourcePath}");
            log.Info ($"Searching '{SourcePath}' for music...");

            var musicMapper = new MusicMapper(src_dir,log);
            musicMapper.Map();

            musicMapper.DisplaySummary();

            log.Info ($"Saving files under '{destinationPath}'");

            var fileMapper = new FileMapper(dest_dir, musicMapper.Artists,log);
            fileMapper.Verbose = Verbose;
            fileMapper.Map();
        }
    }
}
