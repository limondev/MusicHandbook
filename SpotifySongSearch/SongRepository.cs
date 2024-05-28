using SpotifyExplode.Tracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MusicHandbook
{
    public class SongRepository : JSONRepository<ScoredSong>
    {
        public SongRepository() : base("songs.json")
        {

        }
        public override void Save(ScoredSong song)
        {
            List<ScoredSong> songs = Load();
            songs.Add(song);
            var newjson = JsonSerializer.Serialize(songs, options);
            File.WriteAllText(filePath, newjson);
        }
        public override void Delete(ScoredSong song)
        {
            List<ScoredSong> songs = Load();
            if (song.TrackId != null)
            { 
                songs.RemoveAll(s => s.Id.Value == song.Id.Value);
            }
            else
            {
                songs.RemoveAll(s => s.Title == song.Title);
            }
            
            var newjson = JsonSerializer.Serialize(songs, options);
            File.WriteAllText(filePath, newjson);
        }
        public override List<ScoredSong> Load()
        {
            List<ScoredSong> songs = base.Load();
            foreach (var song in songs)
            {
                if (song.TrackId != null)
                {
                    song.Id = TrackId.Parse(song.TrackId);
                }
            }
            return songs;

        }
        public void ClearAll()
        {
            File.Delete(filePath);
        }
    }
}