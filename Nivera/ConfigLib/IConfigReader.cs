using System;

namespace Nivera.ConfigLib
{
    public interface IConfigReader
    {
        object ReadConfig(Type type, string path);
    }
}
