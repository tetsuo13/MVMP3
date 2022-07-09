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

        public int CountArtist
        {
            get
            {
                lock (artistlock)
                {
                    artistcount++;
                    return artistcount;
                }
            }
        }

        class UnknownTag : TagLib.Tag
        {
            static Object alock = new Object();
            static int count = 0;

            public int Count 
            {
                get {
                    lock(alock)
                    {
                        count++;
                        return count;
                    }
                }
            }

            public override TagLib.TagTypes TagTypes => TagLib.TagTypes.Id3v2;

            public UnknownTag(string fileName)
            {
                string artist = "Unknown : " + fileName;
                this.Performers = new string[] { artist };
                this.Album = "Unknown Album";
                this.Track = 1;
                this.Title = Count + Path.GetFileName(fileName);
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
                        log.Warn  ($"{filePath} missing tag. Placing in Unknown category.");
                        mp3tag = new UnknownTag(filePath);
                    }

                    var artistName = mp3tag.FirstPerformer?.Trim();

                    if (string.IsNullOrEmpty(artistName))
                    {
                        log.Warn  ($"{filePath} missing artist. Placing in Unknown category.");
                        mp3tag.Performers = new string[] { "Unknown Artist " + CountArtist };
                        artistName = mp3tag.FirstPerformer?.Trim();
                    }

                    var artist = Artists.SingleOrDefault(x => x.Name == artistName);

                    if (artist == null)
                    {
                        artist = new MvMp3Artist(artistName);
                        Artists.Add(artist);
                    }

                    if (mp3tag == null || mp3tag.Album == null || mp3tag.Title == null)
                    {
                        continue;
                    }

                    var albumName = mp3tag.Album.Trim();
                    var album = artist.Albums.SingleOrDefault(x => x.Name == albumName);

                    if (album == null)
                    {
                        album = new MvMp3Album(albumName);
                        artist.AddAlbum(album);
                    }

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
