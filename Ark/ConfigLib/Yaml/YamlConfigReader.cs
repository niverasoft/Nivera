using System;
using System.IO;

using ArKLib.Utils;

namespace ArKLib.ConfigLib.Yaml
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
