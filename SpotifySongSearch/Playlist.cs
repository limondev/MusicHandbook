using System;
using System.Collections.Generic;
using SpotifyExplode.Tracks;

namespace MusicHandbook
{
    public class Playlist
    {
        public string Name { get; set; }
        public List<ScoredSong> Songs { get; set; }

        public Playlist(string name)
        {
            Name = name;
            Songs = new List<ScoredSong>();
        }
    }
}
