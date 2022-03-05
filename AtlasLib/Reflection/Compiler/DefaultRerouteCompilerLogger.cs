using AtlasLib.Utils;

namespace AtlasLib.Reflection.Compiler
{
    internal class DefaultRerouteCompilerLogger : ICompilerLogger
    {
        public void LogCompiler(CodeLanguageType codeLanguageType, CompilerLogType compilerLogType, object message)
        {
            if (compilerLogType == CompilerLogType.CompilerError)
            {
                AtlasLogger.Error($"[Error - {codeLanguageType} Compiler] >> {message}");
            }

            if (compilerLogType == CompilerLogType.CompilerWarning)
            {
                AtlasLogger.Warn($"[Warning - {codeLanguageType} Compiler] >> {message}");
            }

            if (compilerLogType == CompilerLogType.CompilerMessage)
            {
                AtlasLogger.Info($"[Message - {codeLanguageType} Compiler] >> {message}");
            }
        }
    }
}
