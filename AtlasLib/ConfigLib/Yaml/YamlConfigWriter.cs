using System;

using AtlasLib.Utils;

namespace AtlasLib.ConfigLib.Yaml
{
    public class YamlConfigWriter : IConfigWriter
    {
        public string WriteConfig(Type configType, object configObject = null)
        {
            Assert.NotNull(configObject);

            return YamlHouse.Serializer.Serialize(configObject);
        }
    }
}
