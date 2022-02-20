using System.Collections.Generic;
using System.Reflection;

using AssetsTools.Dynamic;

namespace AtlasLib.Unity
{
    public class UnityModFile
    {
        public string Directory;
        public string Author;
        public string Name;
        public string Version;

        public List<string> AssetFiles;

        public List<DynamicAsset> LoadedAssets;
        public Assembly LoadedScripts;
    }
}