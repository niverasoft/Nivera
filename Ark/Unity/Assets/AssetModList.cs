using System.Collections.Generic;

using ArkLib.IO;

namespace ArkLib.Unity.Assets
{
    public static class AssetModList
    {
        public static List<string> ReadFrom(string path)
        {
            return DataFile.ReadFrom<List<string>>($"{path}/modlist.Ark");
        }

        public static void WriteTo(string path, List<string> mods)
        {
            DataFile.WriteTo($"{path}/modlist.Ark", mods);
        }
    }
}
