using System.IO;

using Utf8Json;

namespace AtlasLib.IO
{
    public static class DataFile
    {
        public static T ReadFrom<T>(string path)
        {
            using (var stream = File.OpenRead(path))
            using (var reader = new BinaryReader(stream))
            {
                byte[] jsonData = reader.ReadBytes(reader.ReadInt32());

                return JsonSerializer.Deserialize<T>(jsonData);
            }
        }

        public static void WriteTo(string path, object obj)
        {
            byte[] jsonData = JsonSerializer.Serialize(obj);

            using (var fileStream = File.OpenWrite(path))
            using (var writer = new BinaryWriter(fileStream))
            {
                writer.Write(jsonData.Length);
                writer.Write(jsonData);
            }
        }
    }
}
