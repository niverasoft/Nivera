using System;

using ArKLib.Utils;

using Newtonsoft.Json;

namespace ArKLib.ConfigLib.Json
{
    public class JsonConfigWriter : IConfigWriter
    {
        public string WriteConfig(Type configType, object configObject = null)
        {
            Assert.NotNull(configObject);

            return JsonConvert.SerializeObject(configObject, Formatting.Indented);
        }
    }
}
