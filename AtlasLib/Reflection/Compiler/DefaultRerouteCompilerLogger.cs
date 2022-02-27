using AtlasLib.Utils;

namespace AtlasLib.Reflection.Compiler
{
    internal class DefaultRerouteCompilerLogger : ICompilerLogger
    {
        public void LogCompiler(CodeLanguageType codeLanguageType, CompilerLogType compilerLogType, object message)
        {
            if (compilerLogType == CompilerLogType.CompilerError)
            {
                AtlasHelper.Error($"[Error - {codeLanguageType} Compiler] >> {message}");
            }

            if (compilerLogType == CompilerLogType.CompilerWarning)
            {
                AtlasHelper.Warn($"[Warning - {codeLanguageType} Compiler] >> {message}");
            }

            if (compilerLogType == CompilerLogType.CompilerMessage)
            {
                AtlasHelper.Info($"[Message - {codeLanguageType} Compiler] >> {message}");
            }
        }
    }
}
