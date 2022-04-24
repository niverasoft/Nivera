using System;
using System.Reflection;

namespace ArkLib.Unity.Assets
{
    public static class AssetModInstaller
    {
        public static readonly string AssetModBackupPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/ArkMods/";

        public static void Install(AssetMod assetMod)
        {
            if (assetMod.ScriptFiles.Count > 0)
            {
                AssetModPatcher.CompileAndPatch(assetMod);
            }


        }

        public static void Uninstall(AssetMod assetMod)
        {
            AssetModPatcher.UnpatchAll(assetMod);
        }

        private static void CallModInit(AssetMod assetMod)
        {
            if (assetMod.CompiledScripts != null)
            {
                Type type = assetMod.CompiledScripts.GetType("AssetPlugin");

                if (type != null)
                {
                    MethodInfo method = type.GetMethod("OnInstalled");

                    if (method != null)
                    {
                        method.Invoke(null, null);
                    }
                }
            }    
        }
    }
}