using System;
using System.IO;

using ArkLib.Utils;

namespace ArkLib.ConfigLib.Yaml
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
