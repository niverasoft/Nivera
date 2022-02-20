using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using AtlasLib.Utils;
using AtlasLib.CodeGen;

using AssetsTools;
using AssetsTools.Dynamic;

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

        public const int MinimalFileLines = 5;

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
            string[] files = lines[3].Replace("ASSET FILES = ", "").Split(',');

            foreach (string file in files)
                Assert.FileExists($"{directory}/{file}");

            return new UnityModFile
            {
                Author = author,
                Name = name,
                Version = version,
                Directory = directory,
                AssetFiles = files.ToList(),
            };
        }

        public UnityModFile LoadAssets(UnityModFile unityModFile)
        {
            Assert.NotNull(unityModFile);

            unityModFile.LoadedAssets = new List<DynamicAsset>();

            foreach (string file in unityModFile.AssetFiles)
            {
                Assert.FileExists(file);

                AssetBundleFile assetBundleFile = AssetBundleFile.LoadFromFile($"{unityModFile.Directory}/{file}");

                Assert.NotNull(assetBundleFile);

                foreach (AssetBundleFile.FileType fileType in assetBundleFile.Files)
                {
                    AssetsFile assetsFile = fileType.ToAssetsFile();

                    Assert.NotNull(assetsFile);

                    foreach (AssetsFile.ObjectType objectType in assetsFile.Objects)
                    {
                        DynamicAsset dynamicAsset = objectType.ToDynamicAsset();

                        Assert.NotNull(dynamicAsset);

                        unityModFile.LoadedAssets.Add(dynamicAsset);
                    }
                }
            }

            if (Directory.Exists($"{unityModFile.Directory}/AssemblySource"))
            {
                LoadScripts(unityModFile);
            }
            
            return unityModFile;
        }

        public UnityModFile LoadScripts(UnityModFile unityModFile)
        {
            Assert.NotNull(unityModFile);
            Assert.DirectoryExists($"{unityModFile.Directory}/AssemblySource");

            unityModFile.LoadedScripts = CodeCompiler.Compile($"{unityModFile.Directory}/AssemblySource");

            Assert.NotNull(unityModFile.LoadedScripts);

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

                        MethodInfo targetMethod = targetType.GetMethod(method.Name, method.GetParameters().Select(x => x.ParameterType).ToArray());

                        if (targetMethod != null)
                        {
                            harmony.Patch(targetMethod, new HarmonyMethod(method));

                            AtlasHelper.Verbose($"UnityModFileReader >> Patched method {targetType.FullName}::{targetMethod.Name}({string.Join(",", method.GetParameters().Select(x => x.ParameterType.FullName))})");
                        }
                    }
                }
            }
        }
    }
}