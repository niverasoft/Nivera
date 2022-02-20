using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.CodeDom.Compiler;

using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using SharpCompress.Archives.Tar;
using SharpCompress.Archives.Rar;

using AtlasLib.Utils;

namespace AtlasLib.CodeGen
{
    public static class CodeCompiler
    {
        private static readonly CompilerParameters _compilerParameters = new CompilerParameters
        {
            GenerateExecutable = false,
            GenerateInMemory = true,
        };

        public static Assembly CompileFromZipArchive(string archiveLocation)
        {
            Assert.True(ZipArchive.IsZipFile(archiveLocation));

            ZipArchive zipArchive = ZipArchive.Open(archiveLocation);

            string path = $"{Path.GetTempPath()}/Extracted_{Path.GetFileNameWithoutExtension(archiveLocation)}";

            zipArchive.WriteToDirectory(path, new SharpCompress.Common.ExtractionOptions
            {
                ExtractFullPath = true,
                Overwrite = true
            });

            zipArchive.Dispose();

            Assembly compiled = Compile(path);

            Directory.Delete(path, true);

            return compiled;
        }

        public static Assembly CompileFromRarArchive(string archiveLocation)
        {
            Assert.True(RarArchive.IsRarFile(archiveLocation));

            RarArchive rarArchive = RarArchive.Open(archiveLocation);

            string path = $"{Path.GetTempPath()}/Extracted_{Path.GetFileNameWithoutExtension(archiveLocation)}";

            rarArchive.WriteToDirectory(path, new SharpCompress.Common.ExtractionOptions
            {
                ExtractFullPath = true,
                Overwrite = true
            });

            rarArchive.Dispose();

            Assembly compiled = Compile(path);

            Directory.Delete(path, true);

            return compiled;
        }

        public static Assembly CompileFromTarArchive(string archiveLocation)
        {
            Assert.True(TarArchive.IsTarFile(archiveLocation));

            TarArchive tarArchive = TarArchive.Open(archiveLocation);

            string path = $"{Path.GetTempPath()}/Extracted_{Path.GetFileNameWithoutExtension(archiveLocation)}";

            tarArchive.WriteToDirectory(path, new SharpCompress.Common.ExtractionOptions
            {
                ExtractFullPath = true,
                Overwrite = true
            });

            tarArchive.Dispose();

            Assembly compiled = Compile(path);

            Directory.Delete(path, true);

            return compiled;
        }

        public static Assembly Compile(string sourceCodeDir)
        {
            CodeDomProvider codeDomProvider = CodeDomProvider.CreateProvider(CodeDomProvider.GetLanguageFromExtension(".cs"));

            List<string> sourceFiles = new List<string>();

            AddFilesToList(sourceCodeDir, sourceFiles);

            CompilerResults compilerResults = codeDomProvider.CompileAssemblyFromFile(_compilerParameters, sourceFiles.ToArray());

            if (compilerResults.Errors.HasErrors)
                return null;
            else
                return compilerResults.CompiledAssembly;
        }

        private static void AddFilesToList(string dir, List<string> sourceFiles)
        {
            foreach (string file in Directory.GetFiles(dir))
            {
                if (!file.EndsWith(".cs"))
                    continue;

                if (IsDirectory(file))
                    AddFilesToList(dir, sourceFiles);
                else
                    sourceFiles.Add(file);
            }
        }

        private static bool IsDirectory(string path)
        {
            FileAttributes fileAttributes = File.GetAttributes(path);

            return fileAttributes.HasFlag(FileAttributes.Directory);
        }
    }
}
