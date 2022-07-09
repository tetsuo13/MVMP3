using System.Collections.Generic;

namespace MVMP3.Models
{
    public class MvMp3Artist
    {
        public string Name { get; private set; }
        public List<MvMp3Album> Albums { get; private set; } = new List<MvMp3Album>();

        public MvMp3Artist(string name)
        {
            Name = name;
        }

        public void AddAlbum(MvMp3Album album)
        {
            Albums.Add(album);
        }
    }
}
