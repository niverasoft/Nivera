using System;

using AtlasLib.Utils;

using Newtonsoft.Json;

namespace AtlasLib.ConfigLib.Json
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
