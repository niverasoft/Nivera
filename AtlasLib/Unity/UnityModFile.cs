using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

namespace AtlasLib.Unity
{
    public class UnityModFile
    {
        public string Directory;
        public string Author;
        public string Name;
        public string Version;

        public List<Object> LoadedAssets;
        public Assembly LoadedScripts;
    }
}