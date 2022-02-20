using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using AssetsTools;
using AtlasLib.Utils;

namespace AtlasLib.Unity.Assets
{
    public static class AssetManager
    {
        public static void ReplaceAsset(AssetsFile sourceFile, string assetName, string containerFile)
        {
            Assert.NotNull(sourceFile);
            Assert.NotNull(assetName);
            Assert.NotNull(containerFile);
        }
    }
}
