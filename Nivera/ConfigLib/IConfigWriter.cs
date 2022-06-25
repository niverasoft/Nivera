using System;

namespace Nivera.ConfigLib
{
    public interface IConfigWriter
    {
        string WriteConfig(Type configType, object configObject = null);
    }
}