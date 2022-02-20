using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Utf8Json;

namespace AtlasLib.Database
{
    public class Database<T> where T : class
    {
        public static readonly string Directory = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}/AtlasDatabases";

        private string _fileName;
        private List<T> _collection = new List<T>();

        public Database(string fileName)
        {
            _fileName = fileName;
        }

        public void Load()
        {
            if (!System.IO.Directory.Exists(Directory))
                System.IO.Directory.CreateDirectory(Directory);

            if (!File.Exists($"{Directory}/{_fileName}"))
            {
                File.WriteAllBytes($"{Directory}/{_fileName}", JsonSerializer.Serialize(_collection));
            }
            else
            {
                _collection.AddRange(JsonSerializer.Deserialize<List<T>>(File.ReadAllBytes($"{Directory}/{_fileName}")));
            }
        }

        public void Save()
        {
            if (!System.IO.Directory.Exists(Directory))
                System.IO.Directory.CreateDirectory(Directory);

            File.WriteAllBytes($"{Directory}/{_fileName}", JsonSerializer.Serialize(_collection));
        }

        public T Get(Func<T, bool> predicate)
        {
            return _collection.FirstOrDefault(predicate);
        }

        public bool TryGet(Func<T, bool> predicate, out T t)
        {
            t = Get(predicate);

            return t != default;
        }

        public bool Add(T t)
        {
            _collection.Add(t);

            Save();

            return true;
        }
    }
}