using System;

namespace ArkLib.ConfigLib
{
    public interface IConfigReader
    {
        object ReadConfig(Type type, string path);
    }
}
