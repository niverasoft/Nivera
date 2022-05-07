namespace ArKLib.Reflection.Compiler
{
    public interface ICompilerLogger
    {
        void LogCompiler(CodeLanguageType codeLanguageType, CompilerLogType compilerLogType, object message);
    }
}
