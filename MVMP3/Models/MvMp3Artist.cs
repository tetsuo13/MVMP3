using System.Collections.Generic;

namespace MVMP3.Models;

public class MvMp3Artist
{
    public MvMp3Artist(string name)
    {
        Name = name;
    }

    public string Name { get; }
    public List<MvMp3Album> Albums { get; } = new();

    public void AddAlbum(MvMp3Album album)
    {
        Albums.Add(album);
    }
}