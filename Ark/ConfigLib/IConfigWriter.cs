using System;

namespace ArkLib.ConfigLib
{
    public interface IConfigWriter
    {
        string WriteConfig(Type configType, object configObject = null);
    }
}