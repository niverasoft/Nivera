using System.IO;
using System.Collections.Generic;

using AtlasLib.Utils;

namespace AtlasLib.Unity
{
    public class UnityModFileWriter
    {
        // AUTHOR
        // NAME
        // VERSION
        // ASSET FILES

        public const int MinimalFileLines = 4;

        public void Write(UnityModFile unityModFile, string directory)
        {
            Assert.NotNull(unityModFile);
            Assert.NotNull(directory);
            Assert.DirectoryExists(directory);

            List<string> lines = new List<string>(4)
            {
                unityModFile.Author,
                unityModFile.Name,
                unityModFile.Version,
            };

            File.WriteAllLines($"{directory}/modInfo.cfg", lines);
        }
    }
}
