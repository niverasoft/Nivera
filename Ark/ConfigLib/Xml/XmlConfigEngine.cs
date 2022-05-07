using System;
using System.IO;

using ArKLib.Utils;

namespace ArKLib.ConfigLib.Xml
{
    public class XmlConfigEngine : IConfigEngine
    {
        private IConfigWriter configWriter = new XmlConfigWriter();
        private IConfigReader configReader = new XmlConfigReader();
        private string configPath;
        private Type configType;
        private object configInstance;

        public XmlConfigEngine(Type configType, string configPath, object configInstance = null)
        {
            Assert.NotNull(configInstance);

            this.configPath = configPath;
            this.configType = configType;
            this.configInstance = configInstance;
        }

        public string ConfigEngineType => "CONFIG_ENGINE_XML";
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
