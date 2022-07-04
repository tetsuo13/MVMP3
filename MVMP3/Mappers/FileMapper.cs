using MVMP3.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace MVMP3.Mappers
{
    public class FileMapper
    {
        public bool Verbose { get; set; }

        private readonly string DestinationPath;
        private readonly IEnumerable<Artist> Artists;

        private NLog.Logger log;
        public FileMapper(string destinationPath, IEnumerable<Artist> artists, NLog.Logger log)
        {
            this.log = log;
            DestinationPath = destinationPath;
            Artists = artists;
        }

        public void Map()
        {
            var destinationDirectory = $"MVMP3-{DateTime.Now:yyyyMMdd-HHmmss}";
            var basePath = Path.Join(DestinationPath, destinationDirectory);

            Directory.CreateDirectory(basePath);

            WriteArtists(Artists, basePath);
        }

        public string RemoveInvalidChars(string s)
        {
            // System-defined invalid characters.
            s = string.Concat(s.Split(Path.GetInvalidFileNameChars()));

            // Directories can't have leading or trailing periods.
            if (s.StartsWith("."))
            {
                s = s.Trim('.');
            }

            if (s.EndsWith("."))
            {
                s = s.TrimEnd('.');
            }

            return s;
        }

        private void Output(string s)
        {
            //check into levels
             log.Info (s);
        }

        private void WriteArtists(IEnumerable<Artist> artists, string path)
        {
            foreach (var artist in artists)
            {
                Output(artist.Name);

                var artistName = RemoveInvalidChars(artist.Name);
                var artistDirectory = Path.Combine(path, artistName);

                Directory.CreateDirectory(artistDirectory);

                WriteAlbumsFromArtist(artist, artistDirectory);
            }
        }

        private void WriteAlbumsFromArtist(Artist artist, string path)
        {
            foreach (var album in artist.Albums)
            {
                Output($"\t{album.Name}");

                var albumName = RemoveInvalidChars(album.Name);
                var albumDirectory = Path.Combine(path, albumName);

                Directory.CreateDirectory(albumDirectory);

                WriteSongsFromAlbum(album, albumDirectory);
            }
        }

        private void WriteSongsFromAlbum(Album album, string path)
        {
            foreach (var song in album.Songs)
            {
                Output($"\t\t{song.Name}");

                var songName = string.Format("{0:D2} - {1}{2}",
                    song.Track,
                    RemoveInvalidChars(song.Name),
                    Path.GetExtension(song.FilePath));

                var songDirectory = Path.Combine(path, songName);

                Output($"\t\tFrom {song.FilePath}");
                Output($"\t\tTo   {songDirectory}");

                File.Copy(song.FilePath, songDirectory);
            }
        }
    }
}
