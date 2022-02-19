using System;

namespace AtlasLib.Logging
{
    public static class Loggers
    {
        public static ILogWriter GetConsoleWriter()
            => new ConsoleLineWriter();

        public static ILogWriter GetDelegateWriter(Action<string> writingDelegate, Action<LogColor> colorDelegate = null, Action restartDelegate = null)
            => new DelegateLineWriter(writingDelegate, colorDelegate, restartDelegate);

        public static ILogger GetConsoleLogger()
            => new Logger(GetConsoleWriter());

        public static ILogger GetDelegateLogger(Action<string> writingDelegate, Action<LogColor> colorDelegate = null, Action restartDelegate = null)
            => new Logger(GetDelegateWriter(writingDelegate, colorDelegate, restartDelegate));
    }
}