using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpotifyExplode;
using SpotifyExplode.Search;
using SpotifyExplode.Tracks;

namespace MusicHandbook
{
    public class SongsClass
    {
        private readonly SpotifyClient _spotify;
        private List<Track> _tracks;
        private readonly SongRepository _songRepository;
        public List<ScoredSong> Songs { get; private set; }

        public SongsClass()
        {
            _spotify = new SpotifyClient();
            _tracks = new List<Track>();
            _songRepository = new SongRepository();
            Songs = new List<ScoredSong>();
        }

        public async Task<List<Track>> SearchTracksAsync(string query)
        {
            var tracks = new List<Track>();
            foreach (var result in await _spotify.Search.GetResultsAsync(query))
            {
                if (result is TrackSearchResult track)
                {
                    tracks.Add(track);
                }
            }
            _tracks = tracks;
            return tracks;
        }

        public async Task<string> GetYouTubeLinkAsync(Track track)
        {
            var youtubeId = await _spotify.Tracks.GetYoutubeIdAsync(track.Id);
            return $"https://www.youtube.com/watch?v={youtubeId}";
        }

        public void AddSongToRepository(DataGridViewRow row, int rowIndex)
        {
            var song = new ScoredSong()
            {
                Album = _tracks[rowIndex].Album,
                Title = _tracks[rowIndex].Title,
                Artists = _tracks[rowIndex].Artists,
                TrackId = _tracks[rowIndex].Id.Value,
                YouTubeUrl = (string)row.Cells["YouTube URL"].Value,
            };

            var scoreCellValue = (string)row.Cells["Score (1-10)"].Value;
            song.Score = string.IsNullOrWhiteSpace(scoreCellValue) ? 0 : int.Parse(scoreCellValue);

            _songRepository.Save(song);
        }

        public List<ScoredSong> LoadSongs(string filter = "")
        {
            Songs = _songRepository.Load();
            if (string.IsNullOrEmpty(filter))
            {
                return Songs;
            }

            return Songs.Where(song =>
                song.Title.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                song.Artists.Any(artist => artist.Name.Contains(filter, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        }

        public void DeleteSong(ScoredSong song)
        {
            _songRepository.Delete(song);
        }

        public void ClearAllSongs()
        {
            Songs.Clear();
            _songRepository.ClearAll();
        }
    }
}