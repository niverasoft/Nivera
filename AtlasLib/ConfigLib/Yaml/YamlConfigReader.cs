using System;
using System.IO;

using AtlasLib.Utils;

namespace AtlasLib.ConfigLib.Yaml
{
    public class YamlConfigReader : IConfigReader
    {
        public object ReadConfig(Type type, string path)
        {
            Assert.FileExists(path);

            return YamlHouse.Deserializer.Deserialize(File.ReadAllText(path), type);
        }
    }
}
