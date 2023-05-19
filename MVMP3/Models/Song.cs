namespace MVMP3.Models;

public class Song
{
    public string Name { get; private set; }
    public uint Track { get; private set; }
    public string FilePath { get; private set; }

    public Song(string name, uint track, string filePath)
    {
        Name = name;
        Track = track;
        FilePath = filePath;
    }
}
