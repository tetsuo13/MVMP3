using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MVMP3.Models;
using NLog;
using TagLib;
using File = TagLib.File;

namespace MVMP3.Mappers;

public class MusicMapper
{
    private static readonly object artistlock = new();
    private static int artistcount;

    private readonly string BasePath;

    private readonly Logger log;

    public MusicMapper(string basePath, Logger log)
    {
        this.log = log;
        BasePath = basePath;
    }

    public List<MvMp3Artist> Artists { get; } = new();

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

    public void Map()
    {
        foreach (var filePath in Directory.EnumerateFiles(BasePath, "*.mp3", SearchOption.AllDirectories))
            try
            {
                var mp3tag = File.Create(filePath).Tag;

                if (mp3tag == null)
                {
                    log.Warn($"{filePath} missing tag. Placing in Unknown category.");
                    mp3tag = new UnknownTag(filePath);
                }

                var artistName = mp3tag.FirstPerformer?.Trim();

                if (string.IsNullOrEmpty(artistName))
                {
                    log.Warn($"{filePath} missing artist. Placing in Unknown category.");
                    mp3tag.Performers = new[] { CountUnknownArtist };
                    artistName = mp3tag.FirstPerformer?.Trim();
                }

                var artist = AddArtist(artistName);

                if (mp3tag.Album == null) mp3tag.Album = "Unknown Album";

                if (mp3tag.Title == null) mp3tag.Title = Path.GetFileName(filePath);

                var album = AddAlbum(mp3tag, artist);

                var song = mp3tag.Title.Trim();

                if (!album.Songs.Any(x => x.Name == song)) album.AddSong(new Song(song, mp3tag.Track, filePath));
            }
            catch (Exception ex)
            {
                //what to do depends on the error
                log.Error(ex.Message);
            }
    }

    private static MvMp3Album AddAlbum(Tag mp3tag, MvMp3Artist artist)
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

    private class UnknownTag : Tag
    {
        public UnknownTag(string fileName)
        {
            var artist = CountUnknownArtist;
            Performers = new[] { artist };
            Album = "Unknown Album";
            Track = 1;
            Title = Path.GetFileName(fileName);
        }

        public override TagTypes TagTypes => TagTypes.Id3v2;


        public override void Clear()
        {
            Performers = new string[] { };
            Album = "";
            Track = 1;
            Title = "";
        }
    }
}