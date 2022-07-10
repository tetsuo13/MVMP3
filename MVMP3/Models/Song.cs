namespace MVMP3.Models;

public class Song
{
    public Song(string name, uint track, string filePath)
    {
        Name = name;
        Track = track;
        FilePath = filePath;
    }

    public string Name { get; }
    public uint Track { get; }
    public string FilePath { get; }
}