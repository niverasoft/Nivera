using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

using Nivera.Utils;

using Newtonsoft.Json;

namespace Nivera.ConfigLib
{
    public class DummyConfig
    {
        private Type type;
        private Dictionary<string, object> Config;

        internal DummyConfig(Type type)
        {
            this.type = type;

            Config = new Dictionary<string, object>();

            AddTypeToConfig();
        }

        internal DummyConfig(Type type, string filePath)
        {
            this.type = type;

            Deserialize(filePath);
        }

        private void AddTypeToConfig()
        {
            Config.Add($"DUMMYCONFIG_TYPE", type.FullName);
        }

        public void LoadProperties()
        {
            Config.Clear();

            AddTypeToConfig();

            foreach (var prop in type.GetProperties())
            {
                if (!prop.CanRead || !prop.CanWrite || !(prop.GetMethod?.IsPublic ?? false) || !(prop.SetMethod?.IsPublic ?? false))
                    continue;

                Config.Add(prop.Name, prop.GetValue(null));
            }
        }

        public byte[] Serialize()
        {
            LoadProperties();

            return Encoding.UTF32.GetBytes(Convert.ToBase64String(Encoding.UTF32.GetBytes(JsonConvert.SerializeObject(Config))));
        }

        public void Serialize(string filePath)
        {
            File.WriteAllBytes(filePath, Utility.CompressByteArray(Serialize()));
        }

        public void Deserialize(string filePath)
        {
            Deserialize(Utility.DecompressByteArray(File.ReadAllBytes(filePath)));
        }

        public void Deserialize(byte[] bytesRead)
        {
            LoadProperties();

            Dictionary<string, object> cfg = JsonConvert.DeserializeObject<Dictionary<string, object>>(Encoding.UTF32.GetString(Convert.FromBase64String(Encoding.UTF32.GetString(bytesRead))));

            foreach (var pair in cfg)
            {
                Config[pair.Key] = pair.Value;
            }

            SetValuesToProperties();
        }

        public void SetValuesToProperties()
        {
            if (type.FullName != Config["DUMMYCONFIG_TYPE"].ToString())
            {
                ThrowHelper.LogAndThrow("The selected type does not match the type saved in the config.");

                return;
            }

            foreach (var prop in type.GetProperties())
            {
                if (!prop.CanRead || !prop.CanWrite || !(prop.GetMethod?.IsPublic ?? false) || !(prop.SetMethod?.IsPublic ?? false))
                    continue;

                if (Config.TryGetValue(prop.Name, out object value))
                {
                    prop.SetValue(null, value);
                }
            }
        }

        public static DummyConfig LoadFrom(Type type, string filePath)
        {
            return new DummyConfig(type, filePath);
        }

        public static DummyConfig CreateFrom(Type type)
        {
            return new DummyConfig(type);
        }
    }
}
