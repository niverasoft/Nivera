using System;

namespace Nivera.ConfigLib
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

        public static IConfigEngine GetNiveraConfig(Type configType, string configPath, object configInstance = null)
        {
            return new Nivera.NiveraConfigEngine(configType, configPath, configInstance);
        }

        public static IConfigEngine GetBinaryConfig(Type configType, string configPath, object configInstance = null)
        {
            return new Binary.BinaryConfigEngine(configType, configPath, configInstance);
        }

        public static DummyConfig LoadDummy(Type type, string filePath)
            => DummyConfig.LoadFrom(type, filePath);

        public static DummyConfig CreateDummy(Type type)
            => DummyConfig.CreateFrom(type);
    }
}
