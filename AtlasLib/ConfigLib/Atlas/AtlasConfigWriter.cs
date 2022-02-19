using System;

using AtlasLib.ConfigLib.Atlas.Internals;

namespace AtlasLib.ConfigLib.Atlas
{
    public class AtlasConfigWriter : IConfigWriter
    {
        public string WriteConfig(Type configType, object configObject = null)
        {
            if (!(configType.IsSealed && configType.IsAbstract) && configObject == null)
                throw new ArgumentNullException("configObject");

            return new TypeSerializer(configType, configObject).Serialize();
        }
    }
}
