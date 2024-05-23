using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using SpotifyExplode.Tracks;

namespace MusicHandbook
{
    public class JSONRepository<T>
    {
        protected string filePath;
        protected JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true};

        public JSONRepository(string filePath)
        {
            this.filePath = filePath;
        }
        public virtual void Save(T obj)
        {
            List<T> objects = Load();
            objects.Add(obj);
            var newjson = JsonSerializer.Serialize(objects, options);
            File.WriteAllText(filePath, newjson);
        }
        public virtual List<T> Load()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                var objects = JsonSerializer.Deserialize<List<T>>(json);
                return objects;
            }
            else { return new List<T>(); }

        }
        public virtual void Delete(T obj)
        {
            List<T> objects = Load();
            objects.Remove(obj);
            var newjson = JsonSerializer.Serialize(objects, options);
            File.WriteAllText(filePath, newjson);
        }
    }
}
