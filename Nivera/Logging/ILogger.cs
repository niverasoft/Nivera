namespace Nivera.Logging
{
    public interface ILogger
    {
        LogBuilder Builder { get; }

        void WriteLine(LogLine logLine);
        void WriteBuilder(LogBuilder logBuilder);
    }
}
