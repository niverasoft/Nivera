using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Text;

using AtlasLib.IO;
using AtlasLib.Utils;

namespace AtlasLib.Unity.Assets
{
    public class AssetMod
    {
        public string Name { get; private set; }
        public string Author { get; private set; }
        public string Version { get; private set; }
        public string Description { get; private set; }

        public Assembly CompiledScripts { get; internal set; }
        public List<Assembly> LoadedDependencies { get; private set; } = new List<Assembly>();

        public Dictionary<MethodBase, MethodInfo> Patches { get; private set; } = new Dictionary<MethodBase, MethodInfo>();

        public List<byte[]> AssetFiles { get; private set; }
        public List<byte[]> ScriptFiles { get; private set; }
        public List<byte[]> DependencyFiles { get; private set; }

        public static AssetMod ReadFrom(string path)
        {
            Assert.True(path.EndsWith(".atlasmod"), "Only atlasmod files are supported by this method."); 

            AssetMod assetMod = new AssetMod();

            AssetModFile assetModFile = DataFile.ReadFrom<AssetModFile>(path);

            assetMod.Author = assetModFile.Author;
            assetMod.Description = assetModFile.Description;
            assetMod.Name = assetModFile.Name;
            assetMod.Version = assetModFile.Version;
            assetMod.DependencyFiles = assetModFile.Dependencies;
            assetMod.ScriptFiles = assetModFile.AssemblyFiles;
            assetMod.AssetFiles = assetModFile.Assets;

            return assetMod;
        }

        public static void WriteTo(string path, AssetMod assetMod)
        {
            AssetModFile assetModFile = new AssetModFile
            {
                Author = assetMod.Author,
                Description = assetMod.Description,
                Name = assetMod.Name,
                Version = assetMod.Version,
                AssemblyFiles = Directory.Exists($"{path}/Scripts/") ? ReadAllFilesText($"{path}/Scripts/", "cs") : new List<byte[]>(),
                Assets = Directory.Exists($"{path}/Files/") ? ReadAllFilesBinary(path) : new List<byte[]>(),
                Dependencies = Directory.Exists($"{path}/Dependencies/") ? ReadAllFilesBinary($"{path}/Dependencies/", "dll") : new List<byte[]>()
            };

            DataFile.WriteTo($"{path}/{assetMod.Name}.atlasmod", assetModFile);
        }

        private static List<byte[]> ReadAllFilesText(string path, string byExtension = null)
        {
            List<byte[]> files = new List<byte[]>();

            foreach (string file in Directory.GetFiles(path, string.IsNullOrEmpty(byExtension) ? "*" : $"*.{byExtension}", SearchOption.AllDirectories))
            {
                files.Add(Encoding.ASCII.GetBytes(File.ReadAllText(file)));
            }

            return files;
        }

        private static List<byte[]> ReadAllFilesBinary(string path, string byExtension = null)
        {
            List<byte[]> files = new List<byte[]>();

            foreach (string file in Directory.GetFiles(path, string.IsNullOrEmpty(byExtension) ? "*" : $"*.{byExtension}", SearchOption.AllDirectories))
            {
                files.Add(File.ReadAllBytes(file));
            }

            return files;
        }
    }

    internal class AssetModFile
    {
        internal string Name { get; set; }
        internal string Author { get; set; }
        internal string Version { get; set; }
        internal string Description { get; set; }

        internal List<byte[]> Assets { get; set; } = new List<byte[]>();
        internal List<byte[]> AssemblyFiles { get; set; } = new List<byte[]>();
        internal List<byte[]> Dependencies { get; set; } = new List<byte[]>();
    }
}
