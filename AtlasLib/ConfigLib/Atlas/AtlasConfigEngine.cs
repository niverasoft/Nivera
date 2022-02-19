using System;
using System.IO;

namespace AtlasLib.ConfigLib.Atlas
{
    public class AtlasConfigEngine : IConfigEngine
    {
        private IConfigWriter configWriter = new AtlasConfigWriter();
        private string configPath;
        private Type configType;
        private object configInstance;

        public AtlasConfigEngine(Type configType, string configPath, object configInstance = null)
        {
            this.configPath = configPath;
            this.configType = configType;
            this.configInstance = configInstance;
        }

        public string ConfigEngineType => "CONFIG_ENGINE_ATLAS";
        public string ConfigPath => configPath;

        public Type ConfigType => configType;
        public object ConfigInstance => configInstance;

        public IConfigWriter Writer => configWriter;
        public IConfigReader Reader => throw new NotImplementedException();

        public object Load()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            File.WriteAllText(configPath, configWriter.WriteConfig(configType, configInstance));
        }
    }
}