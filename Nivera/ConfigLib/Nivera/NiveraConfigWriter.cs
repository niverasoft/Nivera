using System;

using Nivera.ConfigLib.Nivera.Internals;

namespace Nivera.ConfigLib.Nivera
{
    public class NiveraConfigWriter : IConfigWriter
    {
        public string WriteConfig(Type configType, object configObject = null)
        {
            if (!(configType.IsSealed && configType.IsAbstract) && configObject == null)
                throw new ArgumentNullException("configObject");

            return new TypeSerializer(configType, configObject).Serialize();
        }
    }
}
