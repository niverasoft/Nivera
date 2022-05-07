using System;

namespace ArKLib.ConfigLib
{
    public interface IConfigWriter
    {
        string WriteConfig(Type configType, object configObject = null);
    }
}