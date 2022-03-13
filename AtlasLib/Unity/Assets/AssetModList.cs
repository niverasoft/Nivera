using System.Collections.Generic;

using AtlasLib.IO;

namespace AtlasLib.Unity.Assets
{
    public static class AssetModList
    {
        public static List<string> ReadFrom(string path)
        {
            return DataFile.ReadFrom<List<string>>($"{path}/modlist.atlas");
        }

        public static void WriteTo(string path, List<string> mods)
        {
            DataFile.WriteTo($"{path}/modlist.atlas", mods);
        }
    }
}
