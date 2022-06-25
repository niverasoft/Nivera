using System;

using Nivera.Utils;
using Nivera.IO;

namespace Nivera.ConfigLib.Binary
{
    public class BinaryConfigEngine : IConfigEngine
    {
        private IConfigWriter configWriter = null;
        private IConfigReader configReader = null;
        private string configPath;
        private Type configType;
        private object configInstance;

        public BinaryConfigEngine(Type configType, string configPath, object configInstance = null)
        {
            Assert.NotNull(configInstance);

            this.configPath = configPath;
            this.configType = configType;
            this.configInstance = configInstance;
        }

        public string ConfigEngineType => "CONFIG_ENGINE_BINARY";
        public string ConfigPath => configPath;

        public Type ConfigType => configType;
        public object ConfigInstance => configInstance;

        public IConfigWriter Writer => configWriter;
        public IConfigReader Reader => configReader;

        public object Load()
        {
            var obj = BinaryFile.ReadFrom(ConfigPath).DeserializeFile<object>("config");

            CopyTo(obj);

            return obj;
        }

        public void Save()
        {
            var file = new BinaryFile();

            file.SerializeFile(ConfigInstance, "config");
            file.WriteTo(ConfigPath);
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