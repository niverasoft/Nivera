using System.Text;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using AtlasLib.Reflection.Compiler;
using AtlasLib.Attributes;
using AtlasLib.Utils;

using HarmonyLib;

namespace AtlasLib.Unity.Assets
{
    public static class AssetModPatcher
    {
        private static Harmony harmony = new Harmony(RandomGen.RandomString());

        public static void CompileModDependencies(AssetMod assetMod)
        {
            foreach (var rawBytes in assetMod.DependencyFiles)
            {
                assetMod.LoadedDependencies.Add(Assembly.Load(rawBytes));
            }
        }

        public static void CompileModScripts(AssetMod assetMod)
        {
            List<string> pureSource = new List<string>();

            foreach (var rawBytes in assetMod.ScriptFiles)
            {
                pureSource.Add(Encoding.ASCII.GetString(rawBytes));
            }

            CodeCompiler codeCompiler = new CodeCompiler(CodeLanguageType.CSharp, CodeSourceType.SourceCode, null, true, null, assetMod.LoadedDependencies.Select(x => $"{x.GetName().Name}.dll"));

            assetMod.CompiledScripts = codeCompiler.Compile(pureSource);
        }

        public static void PatchModScripts(AssetMod assetMod)
        {
            Assert.NotNull(assetMod);
            Assert.NotNull(assetMod.CompiledScripts);
            
            foreach (var type in assetMod.CompiledScripts.GetTypes())
            {
                if (type.GetCustomAttribute<PatchTypeAttribute>() == null)
                    continue;

                foreach (var method in type.GetMethods())
                {
                    PatchMethodAttribute patchMethodAttribute = method.GetCustomAttribute<PatchMethodAttribute>();

                    if (patchMethodAttribute == null)
                        continue;

                    assetMod.Patches.Add(patchMethodAttribute.TargetMethod, harmony.Patch(patchMethodAttribute.TargetMethod, method.Name == "Prefix" ? new HarmonyMethod(method) : null,
                                                                     method.Name == "Postfix" ? new HarmonyMethod(method) : null,
                                                                     method.Name == "Transpiler" ? new HarmonyMethod(method) : null,
                                                                     method.Name == "Finalizer" ? new HarmonyMethod(method) : null));
                }
            }
        }

        public static void CompileAndPatch(AssetMod assetMod)
        {
            CompileModDependencies(assetMod);
            CompileModScripts(assetMod);
            PatchModScripts(assetMod);
        }

        public static void UnpatchAll()
        {
            harmony.UnpatchAll();
        }

        public static void UnpatchAll(AssetMod assetMod)
        {
            foreach (var patchPair in assetMod.Patches)
            {
                harmony.Unpatch(patchPair.Key, patchPair.Value);
            }
        }
    }
}