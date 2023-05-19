using System.Collections.Generic;

namespace MVMP3.Models;

public class Artist
{
    public string Name { get; private set; }
    public List<Album> Albums { get; private set; } = new();

    public Artist(string name)
    {
        Name = name;
    }

    public void AddAlbum(Album album)
    {
        Albums.Add(album);
    }
}
