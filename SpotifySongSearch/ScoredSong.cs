using SpotifyExplode.Tracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicHandbook
{
    public class ScoredSong : Track
    {
        public int Score { get; set; }
        public string TrackId { get; set; }
        public string YouTubeUrl { get; set; }
    }
}
