using ArKLib.Utils;

using Newtonsoft.Json;

using System;
using System.IO;

namespace ArKLib.ConfigLib.Json
{
    public class JsonConfigReader : IConfigReader
    {
        public object ReadConfig(Type type, string path)
        {
            Assert.FileExists(path);

            return JsonConvert.DeserializeObject(File.ReadAllText(path));
        }
    }
}
