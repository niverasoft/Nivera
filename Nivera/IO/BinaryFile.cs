using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

using Nivera.Utils;

namespace Nivera.IO
{
    public class BinaryFile
    {
        public Dictionary<string, byte[]> SerializedContents { get; set; } = new Dictionary<string, byte[]>();

        public int SerializerVersion { get; set; }

        public void WriteTo(string path)
        {
            SerializerVersion = LibProperties.BinaryVersion;

            byte[] bytesToWrite = Encoding.UTF32.GetBytes(Convert.ToBase64String(Encoding.UTF32.GetBytes(JsonConvert.SerializeObject(this))));

            File.WriteAllBytes(path, Utility.CompressByteArray(bytesToWrite));
        }

        public static BinaryFile ReadFrom(string path)
        {
            byte[] bytesRead = File.ReadAllBytes(path);

            return JsonConvert.DeserializeObject<BinaryFile>(Encoding.UTF32.GetString(Convert.FromBase64String(Encoding.UTF32.GetString(Utility.DecompressByteArray(bytesRead)))));
        }

        public T DeserializeFile<T>(string fileName)
        {
            if (!SerializedContents.TryGetValue(fileName, out byte[] fileData))
                throw new KeyNotFoundException(fileName);

            return JsonConvert.DeserializeObject<T>(Encoding.UTF32.GetString(Utility.DecompressByteArray(fileData)));
        }

        public bool SerializeFile(object file, string fileName)
        {
            try
            {
                SerializedContents[fileName] = Utility.CompressByteArray(Encoding.UTF32.GetBytes(JsonConvert.SerializeObject(file)));

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}