using System.Collections.Generic;

namespace MVMP3.Models;

public class Album
{
    public string Name { get; private set; }
    public List<Song> Songs { get; private set; } = new();

    public Album(string name)
    {
        Name = name;
    }

    public void AddSong(Song song)
    {
        Songs.Add(song);
    }
}
