using System;

namespace ArKLib.ConfigLib
{
    public interface IConfigReader
    {
        object ReadConfig(Type type, string path);
    }
}
