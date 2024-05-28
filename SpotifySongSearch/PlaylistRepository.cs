using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace MusicHandbook
{
    public class PlaylistRepository : JSONRepository<Playlist>
    {
        public PlaylistRepository() : base("playlists.json") { }

        public override void Save(Playlist playlist)
        {
            List<Playlist> playlists = Load();
            playlists.Add(playlist);
            var newjson = JsonSerializer.Serialize(playlists, options);
            File.WriteAllText(filePath, newjson);
        }

        public override void Delete(Playlist playlist)
        {
            List<Playlist> playlists = Load();
            playlists.RemoveAll(p => p.Name == playlist.Name);
            var newjson = JsonSerializer.Serialize(playlists, options);
            File.WriteAllText(filePath, newjson);
        }
    }
}
