using System;

using ArkLib.Utils;

using Newtonsoft.Json;

namespace ArkLib.ConfigLib.Json
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
