using MVMP3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MVMP3.Mappers
{
    public class MusicMapper
    {
        public List<MvMp3Artist> Artists { get; private set; } = new List<MvMp3Artist>();

        private readonly string BasePath;

        private NLog.Logger log;
        public MusicMapper(string basePath, NLog.Logger log)
        {
            this.log = log;
            BasePath = basePath;
        }

        static Object artistlock = new Object();
        static int artistcount = 0;

        public static string CountUnknownArtist
        {
            get
            {
                lock (artistlock)
                {
                    artistcount++;
                    return "Unknown Artist " + artistcount;
                }
            }
        }

        class UnknownTag : TagLib.Tag
        {
            public override TagLib.TagTypes TagTypes => TagLib.TagTypes.Id3v2;

            public UnknownTag(string fileName)
            {
                string artist = CountUnknownArtist;
                this.Performers = new string[] { artist };
                this.Album = "Unknown Album";
                this.Track = 1;
                this.Title = Path.GetFileName(fileName);
            }

       

            public override void Clear()
            {
                this.Performers = new string[] {  };
                this.Album = "";
                this.Track = 1;
                this.Title = "";
            }
        }

        public void Map()
        {
            foreach (var filePath in Directory.EnumerateFiles(BasePath, "*.mp3", SearchOption.AllDirectories))
            {
                try
                {
                    var mp3tag = TagLib.File.Create(filePath).Tag;

                    if (mp3tag == null)
                    {
                        log.Warn($"{filePath} missing tag. Placing in Unknown category.");
                        mp3tag = new UnknownTag(filePath);
                    }

                    var artistName = mp3tag.FirstPerformer?.Trim();

                    if (string.IsNullOrEmpty(artistName))
                    {
                        log.Warn($"{filePath} missing artist. Placing in Unknown category.");
                        mp3tag.Performers = new string[] { CountUnknownArtist };
                        artistName = mp3tag.FirstPerformer?.Trim();
                    }

                    MvMp3Artist artist = AddArtist(artistName);

                    if (mp3tag.Album == null)
                    {
                        mp3tag.Album = "Unknown Album";
                    }

                    if (mp3tag.Title == null)
                    {
                        mp3tag.Title = Path.GetFileName(filePath);
                    }

                    MvMp3Album album = AddAlbum(mp3tag, artist);

                    var song = mp3tag.Title.Trim();

                    if (!album.Songs.Any(x => x.Name == song))
                    {
                        album.AddSong(new Song(song, mp3tag.Track, filePath));
                    }
                }
                catch (Exception ex)
                {
                    //what to do depends on the error
                    log.Error(ex.Message);
                    continue;
                }
            }
        }

        private static MvMp3Album AddAlbum(TagLib.Tag mp3tag, MvMp3Artist artist)
        {
            var albumName = mp3tag.Album.Trim();
            var album = artist.Albums.SingleOrDefault(x => x.Name == albumName);

            if (album == null)
            {
                album = new MvMp3Album(albumName);
                artist.AddAlbum(album);
            }

            return album;
        }

        private MvMp3Artist AddArtist(string artistName)
        {
            var artist = Artists.SingleOrDefault(x => x.Name == artistName);

            if (artist == null)
            {
                artist = new MvMp3Artist(artistName);
                Artists.Add(artist);
            }

            return artist;
        }

        public void DisplaySummary()
        {
            var totalAlbums = Artists.Sum(x => x.Albums.Count);
            var totalSongs = Artists
                .SelectMany(x => x.Albums)
                .Sum(x => x.Songs.Count);

            //Console.WriteLine($"Found {Artists.Count:n0} artists, {totalAlbums:n0} albums, and {totalSongs:n0} songs.");
        }
    }
}
