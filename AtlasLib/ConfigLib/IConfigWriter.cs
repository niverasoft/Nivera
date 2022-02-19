using System;

namespace AtlasLib.ConfigLib
{
    public interface IConfigWriter
    {
        string WriteConfig(Type configType, object configObject = null);
    }
}