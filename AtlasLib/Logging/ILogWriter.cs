namespace AtlasLib.Logging
{
    public interface ILogWriter
    {
        void WriteLine(LogLine logLine);
        void WriteString(LogColor color, string str);
    }
}
