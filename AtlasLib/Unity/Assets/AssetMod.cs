using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using SharpCompress.Archives;
using SharpCompress.Archives.Rar;

using Newtonsoft.Json;

using AtlasLib.Utils;
using AtlasLib.Properties;

using AssetsTools.NET;
using AssetsTools.NET.Extra;

namespace AtlasLib.Unity.Assets
{
    public class AssetMod
    {
        public string ParentPath { get; private set; }

        public string Name { get; private set; }
        public string Author { get; private set; }
        public string Version { get; private set; }
        public string Description { get; private set; }

        public Dictionary<string, string> Files { get; private set; }
        public List<string> AssemblyFiles { get; private set; }

        public Assembly CompiledAssembly { get; private set; }

        public static AssetMod ReadFrom(string directoryOrFile)
        {
            AssetMod assetMod = new AssetMod();

            string directory = null;

            if (File.GetAttributes(directoryOrFile).HasFlag(FileAttributes.Directory))
            {
                directory = directoryOrFile;
            }
            else
            {
                directory = $"{Path.GetTempPath()}/{Path.GetFileNameWithoutExtension(directoryOrFile)}";

                RarArchive rarArchive = RarArchive.Open(directoryOrFile);

                rarArchive.WriteToDirectory(directory, new SharpCompress.Common.ExtractionOptions
                {
                    ExtractFullPath = true,
                    Overwrite = true
                });

                rarArchive.Dispose();
            }

            byte[] infoBytes = File.ReadAllBytes($"{directory}/modInfo.atlasmod");

            string jsonString = Encoding.ASCII.GetString(infoBytes);

            ModInfo modInfo = JsonConvert.DeserializeObject<ModInfo>(jsonString);

            assetMod.ParentPath = directory;
            assetMod.Author = modInfo.Author;
            assetMod.Description = modInfo.Description;
            assetMod.Files = modInfo.FileConfig;
            assetMod.Name = modInfo.Name;
            assetMod.Version = modInfo.Version;

            return assetMod;
        }

        public void Apply()
        {
            Assert.True(LibProperties.System_UseUnityCompatModule);


        }

        public void WriteAssetTo(AssetsFileInstance inst, int fileType, int infoIndex, int fileId, int pathId, string assetName, string pathToGlobal)
        {
            AssetsManager assetsManager = new AssetsManager();

            assetsManager.LoadClassDatabase(new MemoryStream(Resources.classdata_large));

            var ggm = assetsManager.LoadAssetsFile(pathToGlobal, true);

            assetsManager.LoadClassDatabaseFromPackage(ggm.file.typeTree.unityVersion);

            var rsrcMan = ggm.table.GetAssetsOfType((int)AssetClassID.ResourceManager)[0];
            var rsrc = assetsManager.GetTypeInstance(ggm, rsrcMan).GetBaseField();

            var container = rsrc.Get("m_Container").Get("Array");

            List<AssetsReplacer> replacers = new List<AssetsReplacer>();

            foreach (var data in container.children)
            {
                var name = data[0].GetValue().AsString();
               
                if (name == assetName)
                {
                    var pathData = data[1];

                    pathData.Get("m_FileID").GetValue().Set(fileId);
                    pathData.Get("m_PathID").GetValue().Set(pathId);

                    replacers.Add(new AssetsReplacerFromMemory(0, infoIndex, fileType, 0xffff, data.WriteToByteArray()));
                }
            }

            using (var stream = File.OpenWrite("resources-modified.assets"))
            using (var writer = new AssetsFileWriter(stream))
            {
                inst.file.Write(writer, 0, replacers);
            }
        }

        public void Compile()
        {

        }

        public static void WriteToDirectory(string directory, AssetMod assetMod)
        {

        }

        private class ModInfo
        {
            public string Name;
            public string Author;
            public string Version;
            public string Description;

            public Dictionary<string, string> FileConfig;
        }
    }
}
