using System.Collections.Generic;

using ArKLib.IO;

namespace ArKLib.Unity.Assets
{
    public static class AssetModList
    {
        public static List<string> ReadFrom(string path)
        {
            return DataFile.ReadFrom<List<string>>($"{path}/modlist.ark");
        }

        public static void WriteTo(string path, List<string> mods)
        {
            DataFile.WriteTo($"{path}/modlist.ark", mods);
        }
    }
}
