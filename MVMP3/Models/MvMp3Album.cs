using System.Collections.Generic;

namespace MVMP3.Models;

public class MvMp3Album
{
    public MvMp3Album(string name)
    {
        Name = name;
    }

    public string Name { get; }
    public List<Song> Songs { get; } = new();

    public void AddSong(Song song)
    {
        Songs.Add(song);
    }
}