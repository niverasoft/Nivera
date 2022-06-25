using System;
using System.IO;

using Nivera.Utils;

namespace Nivera.ConfigLib.Yaml
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
