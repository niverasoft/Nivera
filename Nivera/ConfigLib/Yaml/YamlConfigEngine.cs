using System;
using System.IO;

using Nivera.Utils;

namespace Nivera.ConfigLib.Yaml
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
            var obj = configReader.ReadConfig(ConfigType, ConfigPath);

            CopyTo(obj);

            return obj;
        }

        public void Save()
        {
            File.WriteAllText(configPath, configWriter.WriteConfig(configType, configInstance));
        }

        public void CopyTo(object source)
        {
            if (configInstance == null)
                return;

            foreach (var property in configInstance.GetType().GetProperties())
            {
                property.SetValue(configInstance, property.GetValue(source));
            }
        }
    }
}
