using MVMP3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MVMP3.Mappers
{
    public class MusicMapper
    {
        public List<Artist> Artists { get; private set; } = new List<Artist>();

        private readonly string BasePath;

        public MusicMapper(string basePath)
        {
            BasePath = basePath;
        }

        public void Map()
        {
            foreach (var filePath in Directory.EnumerateFiles(BasePath, "*.mp3", SearchOption.AllDirectories))
            {
                try
                {
                    var mp3 = TagLib.File.Create(filePath);
              

                if (mp3.Tag == null)
                {
                    Console.WriteLine($"{filePath} missing tag. Skipping.");
                    continue;
                }

                var artistName = mp3.Tag.FirstPerformer?.Trim();

                if (string.IsNullOrEmpty(artistName))
                {
                    Console.WriteLine($"{filePath} missing artist. Skipping");
                    continue;
                }

                var artist = Artists.SingleOrDefault(x => x.Name == artistName);

                if (artist == null)
                {
                    artist = new Artist(artistName);
                    Artists.Add(artist);
                }

                if (mp3.Tag == null || mp3.Tag.Album == null || mp3.Tag.Title == null)
                {
                    continue;
                }

                var albumName = mp3.Tag.Album.Trim();
                var album = artist.Albums.SingleOrDefault(x => x.Name == albumName);

                if (album == null)
                {
                    album = new Album(albumName);
                    artist.AddAlbum(album);
                }

                var song = mp3.Tag.Title.Trim();

                if (!album.Songs.Any(x => x.Name == song))
                {
                    album.AddSong(new Song(song, mp3.Tag.Track, filePath));
                }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
        }

        public void DisplaySummary()
        {
            var totalAlbums = Artists.Sum(x => x.Albums.Count);
            var totalSongs = Artists
                .SelectMany(x => x.Albums)
                .Sum(x => x.Songs.Count);

            Console.WriteLine($"Found {Artists.Count:n0} artists, {totalAlbums:n0} albums, and {totalSongs:n0} songs.");
        }
    }
}
