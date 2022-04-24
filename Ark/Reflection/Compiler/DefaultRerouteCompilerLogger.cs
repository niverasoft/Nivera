using ArkLib.Utils;

namespace ArkLib.Reflection.Compiler
{
    internal class DefaultRerouteCompilerLogger : ICompilerLogger
    {
        public void LogCompiler(CodeLanguageType codeLanguageType, CompilerLogType compilerLogType, object message)
        {
            if (compilerLogType == CompilerLogType.CompilerError)
            {
                ArkLog.Error($"[Error - {codeLanguageType} Compiler] >> {message}");
            }

            if (compilerLogType == CompilerLogType.CompilerWarning)
            {
                ArkLog.Warn($"[Warning - {codeLanguageType} Compiler] >> {message}");
            }

            if (compilerLogType == CompilerLogType.CompilerMessage)
            {
                ArkLog.Info($"[Message - {codeLanguageType} Compiler] >> {message}");
            }
        }
    }
}
