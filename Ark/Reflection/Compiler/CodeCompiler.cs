using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.CodeDom.Compiler;
using System.Collections.Generic;

using SharpCompress.Archives;

namespace ArkLib.Reflection.Compiler
{
    public class CodeCompiler
    {
        private ICompilerLogger compilationLogger;
        private bool logWarnings;
        private CodeLanguageType codeLanguageType;
        private CodeSourceType codeSourceType;
        private string path;
        private CompilerParameters compilerParameters;

        public CodeCompiler(CodeLanguageType codeLanguageType, CodeSourceType codeSourceType, string path, bool logWarnings = true, ICompilerLogger compilerLogger = null, IEnumerable<string> dependencies = null)
        {
            this.codeLanguageType = codeLanguageType;
            this.codeSourceType = codeSourceType;
            this.path = path;
            this.logWarnings = logWarnings;

            if (compilerLogger != null)
                this.compilationLogger = compilerLogger;
            else
                this.compilationLogger = new DefaultRerouteCompilerLogger();

            compilerParameters = new CompilerParameters
            {
                GenerateExecutable = false,
                GenerateInMemory = true
            };

            compilerParameters.ReferencedAssemblies?.AddRange(dependencies != null ? dependencies.ToArray() : AppDomain.CurrentDomain?.GetAssemblies()?.Select(x => $"{x.GetName().Name}.dll").ToArray());

            compilationLogger.LogCompiler(codeLanguageType, CompilerLogType.CompilerMessage, $"Initialized Compiler.");
        }

        public Assembly Compile(IEnumerable<string> sourceCode = null)
        {
            VerifySourcePathInternal();

            List<string> files = new List<string>();

            if (codeSourceType != CodeSourceType.SourceCode)
            {
                if (IsSourceAnArchiveInternal())
                {
                    IArchive archive = GetArchiveInternal();

                    string tempDirPath = $"{Path.GetTempPath()}/Decompiled_{DateTime.Now.Ticks}";

                    Directory.CreateDirectory(tempDirPath);

                    archive.WriteToDirectory(tempDirPath, new SharpCompress.Common.ExtractionOptions
                    {
                        ExtractFullPath = true,
                        Overwrite = true
                    });

                    archive.Dispose();

                    foreach (string file in Directory.GetFiles(tempDirPath, "*", SearchOption.AllDirectories))
                    {
                        if (IsValidFileInternal(file))
                        {
                            files.Add(file);
                        }
                    }
                }
                else
                {
                    foreach (string file in Directory.GetFiles(path))
                    {
                        files.Add(file);
                    }
                }
            }
            else
            {
                files.AddRange(sourceCode);
            }

            Assembly assembly = null;

            if (codeLanguageType == CodeLanguageType.Cpp)
                assembly = CompileCppInternal(files, compilerParameters);
            else if (codeLanguageType == CodeLanguageType.CSharp)
                assembly = CompileCSharpInternal(files, compilerParameters);
            else if (codeLanguageType == CodeLanguageType.JavaScript)
                assembly = CompileJavaScriptInternal(files, compilerParameters);
            else
                assembly = CompileVisualBasicInternal(files, compilerParameters);

            return assembly;
        }

        private bool IsValidFileInternal(string filePath)
        {
            string fileExtension = ".cs";

            if (codeLanguageType == CodeLanguageType.Cpp)
                fileExtension = ".h";

            if (codeLanguageType == CodeLanguageType.JavaScript)
                fileExtension = ".js";

            if (codeLanguageType == CodeLanguageType.VisualBasic)
                fileExtension = ".vb";

            if (!filePath.EndsWith(fileExtension))
                return false;

            return File.Exists(filePath);
        }

        private IArchive GetArchiveInternal()
        {
            if (codeSourceType == CodeSourceType.RarArchive)
            {
                return SharpCompress.Archives.Rar.RarArchive.Open(path);
            }

            if (codeSourceType == CodeSourceType.TarArchive)
            {
                return SharpCompress.Archives.Tar.TarArchive.Open(path);
            }

            if (codeSourceType == CodeSourceType.ZipArchive)
            {
                return SharpCompress.Archives.Zip.ZipArchive.Open(path);
            }

            ThrowPathException();

            return null;
        }

        private bool IsSourceAnArchiveInternal()
        {
            return codeSourceType == CodeSourceType.RarArchive || codeSourceType == CodeSourceType.TarArchive || codeSourceType == CodeSourceType.ZipArchive;
        }

        private void VerifySourcePathInternal()
        {
            if (codeSourceType == CodeSourceType.Folder)
            {
                if (!Directory.Exists(path))
                    ThrowPathException();
            }
            else
            {
                if (!File.Exists(path))
                    ThrowPathException();
            }
        }

        private void ThrowPathException()
        {
            throw new Exception($"The specified path is invalid: {path}");
        }

        private Assembly CompileCSharpInternal(IEnumerable<string> sourceFiles, CompilerParameters compilerParameters)
        {
            CodeDomProvider codeDomProvider = GetProviderInternal(CodeLanguageType.CSharp);

            CompilerResults compilerResults = codeSourceType != CodeSourceType.SourceCode ? codeDomProvider.CompileAssemblyFromFile(compilerParameters, sourceFiles.ToArray()) : codeDomProvider.CompileAssemblyFromSource(compilerParameters, sourceFiles.ToArray());

            if (compilerResults.Errors.HasWarnings)
                LogWarningsInternal(compilerResults.Errors);

            if (compilerResults.Errors.HasErrors)
            {
                LogErrorsInternal(compilerResults.Errors, out string errorCollection);

                throw new Exception($"Several errors were encountered by the targeted compiler.{Environment.NewLine}{errorCollection}");
            }

            return compilerResults.CompiledAssembly;
        }

        private Assembly CompileCppInternal(IEnumerable<string> sourceFiles, CompilerParameters compilerParameters)
        {
            CodeDomProvider codeDomProvider = GetProviderInternal(CodeLanguageType.Cpp);

            CompilerResults compilerResults = codeSourceType != CodeSourceType.SourceCode ? codeDomProvider.CompileAssemblyFromFile(compilerParameters, sourceFiles.ToArray()) : codeDomProvider.CompileAssemblyFromSource(compilerParameters, sourceFiles.ToArray());

            if (compilerResults.Errors.HasWarnings)
                LogWarningsInternal(compilerResults.Errors);

            if (compilerResults.Errors.HasErrors)
            {
                LogErrorsInternal(compilerResults.Errors, out string errorCollection);

                throw new Exception($"Several errors were encountered by the targeted compiler.{Environment.NewLine}{errorCollection}");
            }

            return compilerResults.CompiledAssembly;
        }

        private Assembly CompileVisualBasicInternal(IEnumerable<string> sourceFiles, CompilerParameters compilerParameters)
        {
            CodeDomProvider codeDomProvider = GetProviderInternal(CodeLanguageType.VisualBasic);

            CompilerResults compilerResults = codeSourceType != CodeSourceType.SourceCode ? codeDomProvider.CompileAssemblyFromFile(compilerParameters, sourceFiles.ToArray()) : codeDomProvider.CompileAssemblyFromSource(compilerParameters, sourceFiles.ToArray());

            if (compilerResults.Errors.HasWarnings)
                LogWarningsInternal(compilerResults.Errors);

            if (compilerResults.Errors.HasErrors)
            {
                LogErrorsInternal(compilerResults.Errors, out string errorCollection);

                throw new Exception($"Several errors were encountered by the targeted compiler.{Environment.NewLine}{errorCollection}");
            }

            return compilerResults.CompiledAssembly;
        }

        private Assembly CompileJavaScriptInternal(IEnumerable<string> sourceFiles, CompilerParameters compilerParameters)
        {
            CodeDomProvider codeDomProvider = GetProviderInternal(CodeLanguageType.JavaScript);

            CompilerResults compilerResults = codeSourceType != CodeSourceType.SourceCode ? codeDomProvider.CompileAssemblyFromFile(compilerParameters, sourceFiles.ToArray()) : codeDomProvider.CompileAssemblyFromSource(compilerParameters, sourceFiles.ToArray());

            if (compilerResults.Errors.HasWarnings)
                LogWarningsInternal(compilerResults.Errors);

            if (compilerResults.Errors.HasErrors)
            {
                LogErrorsInternal(compilerResults.Errors, out string errorCollection);

                throw new Exception($"Several errors were encountered by the targeted compiler.{Environment.NewLine}{errorCollection}");
            }

            return compilerResults.CompiledAssembly;
        }

        private void LogWarningsInternal(CompilerErrorCollection compilerErrorCollection)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine();

            foreach (CompilerError compilerError in compilerErrorCollection)
            {
                if (compilerError.IsWarning && logWarnings)
                {
                    stringBuilder.AppendLine($"Warning {compilerError.ErrorNumber} - File:{compilerError.FileName} Line:{compilerError.Line} Column:{compilerError.Column} > {compilerError.ErrorText}");
                }
            }

            compilationLogger?.LogCompiler(CodeLanguageType.JavaScript, CompilerLogType.CompilerWarning, stringBuilder.ToString());
        }

        private void LogErrorsInternal(CompilerErrorCollection compilerErrorCollection, out string errorCollection)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine();

            foreach (CompilerError compilerError in compilerErrorCollection)
            {
                if (!compilerError.IsWarning)
                {
                    stringBuilder.AppendLine($"Error {compilerError.ErrorNumber} - File:{compilerError.FileName} Line:{compilerError.Line} Column:{compilerError.Column} > {compilerError.ErrorText}");
                }
            }

            errorCollection = stringBuilder.ToString();

            compilationLogger?.LogCompiler(CodeLanguageType.JavaScript, CompilerLogType.CompilerWarning, errorCollection);
        }

        private CodeDomProvider GetProviderInternal(CodeLanguageType codeLanguageType)
        {
            if (!IsSupportedInternal(codeLanguageType))
                throw new NotSupportedException($"{codeLanguageType} compilation is not supported on this platform.");

            return CodeDomProvider.CreateProvider(GetLanguageNameInternal(codeLanguageType), new Dictionary<string, string>
            {
                ["CompilerVersion"] = "v4.0"
            });
        }

        private bool IsSupportedInternal(CodeLanguageType codeLanguageType)
        {
            CompilerInfo[] compilers = CodeDomProvider.GetAllCompilerInfo();

            foreach (CompilerInfo compiler in compilers)
            {
                string languageName = GetLanguageNameInternal(codeLanguageType);

                if (compiler.GetLanguages().Contains(languageName))
                    return true;
            }

            return false;
        }

        private string GetLanguageNameInternal(CodeLanguageType codeLanguageType)
        {
            if (codeLanguageType == CodeLanguageType.Cpp)
                return "c++";

            if (codeLanguageType == CodeLanguageType.JavaScript)
                return "js";

            if (codeLanguageType == CodeLanguageType.VisualBasic)
                return "vb";

            if (codeLanguageType == CodeLanguageType.CSharp)
                return "csharp";

            throw new NotSupportedException($"{codeLanguageType} is not a supported language by ArkLib.");
        }
    }
}
