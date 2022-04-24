using System;

using ArkLib.ConfigLib.Ark.Internals;

namespace ArkLib.ConfigLib.Ark
{
    public class ArkConfigWriter : IConfigWriter
    {
        public string WriteConfig(Type configType, object configObject = null)
        {
            if (!(configType.IsSealed && configType.IsAbstract) && configObject == null)
                throw new ArgumentNullException("configObject");

            return new TypeSerializer(configType, configObject).Serialize();
        }
    }
}
