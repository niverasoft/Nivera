using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using AtlasLib.Utils;
using AtlasLib.Properties;
using AtlasLib.Reflection.Compiler;

using AssetsTools;
using AssetsTools.NET;
using AssetsTools.NET.Extra;

using HarmonyLib;

namespace AtlasLib.Unity
{
    public class UnityModFileReader
    {
        private static HarmonyLib.Harmony harmony = new HarmonyLib.Harmony(RandomGen.NextString());

        // AUTHOR
        // NAME
        // VERSION
        // ASSET FILES

        public const int MinimalFileLines = 3;

        public UnityModFile ReadFile(string directory)
        {
            Assert.NotNull(directory);
            Assert.DirectoryExists(directory);
            Assert.FileExists($"{directory}/modInfo.cfg");

            string[] lines = File.ReadAllLines($"{directory}/modInfo.cfg");

            Assert.Equals(lines.Length, MinimalFileLines);

            string author = lines[0].Replace("AUTHOR = ", "");
            string name = lines[1].Replace("NAME = ", "");
            string version = lines[2].Replace("VERSION = ", "");

            return new UnityModFile
            {
                Author = author,
                Name = name,
                Version = version,
                Directory = directory,
            };
        }

        public UnityModFile LoadAssets(UnityModFile unityModFile)
        {
            Assert.NotNull(unityModFile);

            AssetsManager assetsManager = new AssetsManager();

            MemoryStream memoryStream = new MemoryStream(Resources.classdata);

            ClassDatabaseFile classDatabase = assetsManager.LoadClassDatabase(memoryStream);
            
            if (Directory.Exists($"{unityModFile.Directory}/AssemblySource"))
                LoadScripts(unityModFile);

            AssetsFileInstance assetsFile = null;

            if (Directory.Exists($"{unityModFile.Directory}/Assets"))
                assetsFile = LoadAssetBundle(unityModFile, assetsManager);
            else
            {
                BundleFileInstance bundleFileInstance = new BundleFileInstance(File.OpenRead($"{unityModFile.Directory}/assetBundle.unity3d"), null, true);
                assetsFile = assetsManager.LoadAssetsFileFromBundle(bundleFileInstance, 0, true);
            }

            return unityModFile;
        }

        public AssetsFileInstance LoadAssetBundle(UnityModFile unityModFile, AssetsManager assetsManager)
        {
            Assert.NotNull(unityModFile);
            Assert.DirectoryExists($"{unityModFile.Directory}/Assets");

            string tempPath = $"{Path.GetTempPath()}/AtlasLib/Mods/LoadedAssets/{unityModFile.Name}/assetBundle.unity3d";

            Dictionary<string, string> bundles = new Dictionary<string, string>();

            string[] files = Directory.GetFiles($"{unityModFile.Directory}/Assets");

            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string pureFileName = Path.GetFileNameWithoutExtension(file);

                bundles.Add(pureFileName, null);
            }

            foreach (KeyValuePair<string, string> pair in bundles)
            {
                string pairFile = files.FirstOrDefault(x => !x.EndsWith(".cfg") && Path.GetFileName(x) == pair.Key);

                if (!string.IsNullOrEmpty(pairFile))
                    bundles[pair.Key] = pairFile;
                else
                    bundles.Remove(pair.Key);
            }

            foreach (KeyValuePair<string, string> pair in bundles)
            {
                UnityAssetConfig unityAssetConfig = ReadConfig(pair.Key);

                AssetTypeTemplateField assetTypeTemplateField = new AssetTypeTemplateField();

                ClassDatabaseType classDatabaseType = AssetHelper.FindAssetClassByName(assetsManager.classFile, unityAssetConfig.AssetType);

                assetTypeTemplateField.FromClassDatabase(assetsManager.classFile, classDatabaseType, 0);

                AssetTypeValueField valueBuilder = ValueBuilder.DefaultValueFieldFromTemplate(assetTypeTemplateField);

                valueBuilder.Get("m_Name").GetValue().Set(unityAssetConfig.AssetName);
            }

            return null;
        }

        public UnityAssetConfig ReadConfig(string configPath)
        {
            string[] lines = File.ReadAllLines(configPath);

            return new UnityAssetConfig
            {
                AssetType = lines[0].Replace("ASSET TYPE NAME = ", ""),
                AssetName = lines[1].Replace("ASSET NAME = ", "")
            };
        }

        public UnityModFile LoadScripts(UnityModFile unityModFile)
        {
            Assert.NotNull(unityModFile);
            Assert.DirectoryExists($"{unityModFile.Directory}/AssemblySource");

            CodeCompiler codeCompiler = new CodeCompiler(CodeLanguageType.CSharp, CodeSourceType.Folder, $"{unityModFile.Directory}/AssemblySource");

            unityModFile.LoadedScripts = codeCompiler.Compile();

            Assert.NotNull(unityModFile.LoadedScripts);

            if (LibProperties.UseUnityCompatModule)
                PatchInAssembly(unityModFile, LibUnityCompatModule.AssemblyCSharpAssembly);

            return unityModFile;
        }

        public void PatchInAssembly(UnityModFile unityModFile, Assembly targetAssembly)
        {
            Assert.NotNull(targetAssembly);
            Assert.NotNull(unityModFile);
            Assert.NotNull(unityModFile.LoadedScripts);

            foreach (Type type in unityModFile.LoadedScripts.GetTypes())
            {
                Type targetType = targetAssembly.GetType(type.FullName);

                if (targetType != null)
                {
                    foreach (MethodInfo method in type.GetMethods())
                    {
                        if (method.ReturnType != Constants.BoolType)
                            continue;

                        MethodInfo targetMethod = FindTargetMethodInternal(targetType, method);

                        if (targetMethod != null)
                        {
                            harmony.Patch(targetMethod, new HarmonyMethod(method));

                            AtlasHelper.Verbose($"Patched method {targetType.FullName}::{targetMethod.Name}({string.Join(",", method.GetParameters().Select(x => x.ParameterType.FullName))})");
                        }
                    }
                }
            }
        }

        internal MethodInfo FindTargetMethodInternal(Type targetType, MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();

            parameters = parameters.Where(x => x.Name != "__instance" && x.Name != "__result" && x.Name != "__state" && x.Name != "__exception").ToArray();

            return targetType.GetMethod(method.Name, parameters.Select(x => x.ParameterType).ToArray());
        }
    }
}