using System.IO;
using System.Text;

using ArKLib.Reflection;

using Newtonsoft.Json;

namespace ArKLib.IO
{
    public static class DataFile
    {
        public static byte[] Compress(byte[] data)
        {
            return SevenZip.Compression.LZMA.SevenZipHelper.Compress(data);
        }

        public static byte[] Decompress(byte[] data)
        {
            return SevenZip.Compression.LZMA.SevenZipHelper.Decompress(data);
        }

        public static T ReadFrom<T>(string path)
        {
            using (var stream = File.OpenRead(path))
            using (var reader = new BinaryReader(stream))
            {
                byte[] rawData = reader.ReadBytes(reader.ReadInt32());
                byte[] jsonData = Decompress(rawData);

                return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(jsonData)).ConvertTo<T>();
            }
        }

        public static void WriteTo(string path, object obj)
        {
            byte[] jsonData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));

            using (var fileStream = File.OpenWrite(path))
            using (var writer = new BinaryWriter(fileStream))
            {
                byte[] rawData = Compress(jsonData);

                writer.Write(rawData.Length);
                writer.Write(rawData);
            }
        }
    }
}
