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

        private static void removeEmptyDir(string startLocation, NLog.Logger log)
        {
            foreach (var directory in Directory.GetDirectories(startLocation))
            {
                removeEmptyDir(directory, log);
                if (Directory.GetFiles(directory).Length == 0 &&
                    Directory.GetDirectories(directory).Length == 0)
                {
                    log.Info("Deleting empty dir " + directory);
                    Directory.Delete(directory, false);
                }
            }
        }


        public void Start(string src_dir, string dest_dir, NLog.Logger log)
        {
            
            //var destinationPath = Directory.GetCurrentDirectory();

            log.Info ($"Source: {SourcePath}");
            log.Info ($"Searching '{SourcePath}' for music...");

            var musicMapper = new MusicMapper(src_dir,log);
            musicMapper.Map();

            musicMapper.DisplaySummary();

            log.Info ($"Saving files under '{dest_dir}'");

            var fileMapper = new FileMapper(dest_dir, musicMapper.Artists,log);
            fileMapper.Verbose = Verbose;
            fileMapper.Map();

            removeEmptyDir(dest_dir, log);
        }
    }
}
