﻿using MVMP3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MVMP3.Mappers;

public class FileMapper
{
    public bool Verbose { get; set; }

    private readonly string DestinationPath;
    private readonly IEnumerable<Artist> Artists;

    /// <summary>
    /// File name characters that should never appear. Cross-platform
    /// differences are normalized, i.e., double quote isn't allowed in
    /// Windows but is allowed in Linux however it's included in this list.
    /// </summary>
    private readonly char[] InvalidFileNameChars = (
        // Path separator in Linux
        "/" +

        // Unprintable character in Linux
        Convert.ToChar(0) +

        // Unprintable characters in Windows
        @"><:""/\|?*" +

        // Unprintable characters in Windows ASCII/Unicode characters 1 through 31
        new string(Enumerable.Range(0, 32).Select(Convert.ToChar).ToArray())
        ).ToArray();

    public FileMapper(string destinationPath, IEnumerable<Artist> artists)
    {
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
        s = string.Concat(s.Split(InvalidFileNameChars));

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
        if (Verbose)
        {
            Console.WriteLine(s);
        }
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
