using System;

namespace ArkLib.ConfigLib
{
    public static class Config
    {
        public static IConfigEngine GetYamlConfig(Type configType, string configPath, object configInstance = null)
        {
            return new Yaml.YamlConfigEngine(configType, configPath, configInstance);
        }

        public static IConfigEngine GetXmlConfig(Type configType, string configPath, object configInstance = null)
        {
            return new Xml.XmlConfigEngine(configType, configPath, configInstance);
        }

        public static IConfigEngine GetJsonConfig(Type configType, string configPath, object configInstance = null)
        {
            return new Json.JsonConfigEngine(configType, configPath, configInstance);
        }

        public static IConfigEngine GetArkConfig(Type configType, string configPath, object configInstance = null)
        {
            return new Ark.ArkConfigEngine(configType, configPath, configInstance);
        }
    }
}
