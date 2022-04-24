using System;

namespace ArkLib.ConfigLib
{
    public interface IConfigEngine
    {
        string ConfigEngineType { get; }
        string ConfigPath { get; }

        Type ConfigType { get; }
        object ConfigInstance { get; }

        IConfigWriter Writer { get; }
        IConfigReader Reader { get; }

        object Load();
        void Save();
    }
}