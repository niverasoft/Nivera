using Nivera.Utils;

namespace Nivera.Reflection.Compiler
{
    internal class DefaultRerouteCompilerLogger : ICompilerLogger
    {
        public void LogCompiler(CodeLanguageType codeLanguageType, CompilerLogType compilerLogType, object message)
        {
            if (compilerLogType == CompilerLogType.CompilerError)
            {
                NiveraLog.Error($"[Error - {codeLanguageType} Compiler] >> {message}");
            }

            if (compilerLogType == CompilerLogType.CompilerWarning)
            {
                NiveraLog.Warn($"[Warning - {codeLanguageType} Compiler] >> {message}");
            }

            if (compilerLogType == CompilerLogType.CompilerMessage)
            {
                NiveraLog.Info($"[Message - {codeLanguageType} Compiler] >> {message}");
            }
        }
    }
}
