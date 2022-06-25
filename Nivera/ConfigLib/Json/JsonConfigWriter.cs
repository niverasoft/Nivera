using System;

using Nivera.Utils;

using Newtonsoft.Json;

namespace Nivera.ConfigLib.Json
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
