using System;
using System.IO;

using ArKLib.Utils;

namespace ArKLib.ConfigLib.Yaml
{
    public class YamlConfigEngine : IConfigEngine
    {
        private IConfigWriter configWriter = new YamlConfigWriter();
        private IConfigReader configReader = new YamlConfigReader();
        private string configPath;
        private Type configType;
        private object configInstance;

        public YamlConfigEngine(Type configType, string configPath, object configInstance = null)
        {
            Assert.NotNull(configInstance);

            this.configPath = configPath;
            this.configType = configType;
            this.configInstance = configInstance;
        }

        public string ConfigEngineType => "CONFIG_ENGINE_YAML";
        public string ConfigPath => configPath;

        public Type ConfigType => configType;
        public object ConfigInstance => configInstance;

        public IConfigWriter Writer => configWriter;
        public IConfigReader Reader => configReader;

        public object Load()
        {
            return configReader.ReadConfig(ConfigType, ConfigPath);
        }

        public void Save()
        {
            File.WriteAllText(configPath, configWriter.WriteConfig(configType, configInstance));
        }
    }
}
