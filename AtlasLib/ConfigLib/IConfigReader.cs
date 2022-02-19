using System;

namespace AtlasLib.ConfigLib
{
    public interface IConfigReader
    {
        object ReadConfig(Type type, string path);
    }
}
