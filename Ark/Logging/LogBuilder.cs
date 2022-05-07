using System;

namespace ArKLib.Logging
{
    public class LogBuilder
    {
        private LogLine logLine;
        private ILogger parentLogger;

        public LogBuilder()
        {
            logLine = new LogLine();
        }

        public LogBuilder AttachDate(ConsoleColor color = ConsoleColor.White)
            => AttachWord(DateTime.Now.ToLocalTime().ToString(LibProperties.Log_DateTimeFormat), color);

        public LogBuilder AttachSplitter(ConsoleColor color = ConsoleColor.White)
            => AttachWord(">>", color);

        public LogBuilder AttachErrorTag()
            => AttachWord("Error", ConsoleColor.Red);

        public LogBuilder AttachFatalErrorTag()
            => AttachWord("Fatal Error", ConsoleColor.DarkRed);

        public LogBuilder AttachWarningTag()
            => AttachWord("Warning", ConsoleColor.Yellow);

        public LogBuilder AttachVerboseTag()
            => AttachWord("Verbose", ConsoleColor.White);

        public LogBuilder AttachDebugTag()
            => AttachWord("Debug", ConsoleColor.Cyan);

        public LogBuilder AttachInfoTag()
            => AttachWord("Information", ConsoleColor.Gray);
        
        public LogBuilder AttachWord(object text, ConsoleColor color = ConsoleColor.White)
        {
            logLine.WriteSingle(color, text.ToString());

            return this;
        }

        public LogBuilder SetLine(LogLine logLine)
        {
            this.logLine = logLine;

            return this;
        }

        public void Finish()
            => Finish(parentLogger);

        public void Finish(ILogger logger)
        {
            logger?.WriteBuilder(this);
            logLine.Finish();
            parentLogger = null;
        }

        public LogLine GetLine()
            => logLine;

        internal LogBuilder SetParent(ILogger logger)
        {
            parentLogger = logger;

            return this;
        }
    }
}