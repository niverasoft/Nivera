using System;
using System.IO;

namespace Nivera.ConfigLib.Nivera
{
    public class NiveraConfigEngine : IConfigEngine
    {
        private IConfigWriter configWriter = new NiveraConfigWriter();
        private string configPath;
        private Type configType;
        private object configInstance;

        public NiveraConfigEngine(Type configType, string configPath, object configInstance = null)
        {
            this.configPath = configPath;
            this.configType = configType;
            this.configInstance = configInstance;
        }

        public string ConfigEngineType => "CONFIG_ENGINE_Nivera";
        public string ConfigPath => configPath;

        public Type ConfigType => configType;
        public object ConfigInstance => configInstance;

        public IConfigWriter Writer => configWriter;
        public IConfigReader Reader => throw new NotImplementedException();

        public object Load()
        {
            object obj = null;

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