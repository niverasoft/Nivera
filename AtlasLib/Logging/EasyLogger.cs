using System;

namespace AtlasLib.Logging
{
    public class EasyLogger : ILogger
    {
        private Action<string> write;
        private Action<ConsoleColor> setColor;
        private Action resetColor;

        public EasyLogger(Action<string> write, Action<ConsoleColor> setColor = null, Action resetColor = null)
        {
            this.write = write;
            this.setColor = setColor;
            this.resetColor = resetColor;
        }

        public LogBuilder Builder
            => new LogBuilder().SetParent(this);

        public void WriteBuilder(LogBuilder logBuilder)
        {
            LogLine logLine = logBuilder.GetLine();

            foreach (var tuple in logLine.ReadAllLines())
            {
                SetColor(tuple.Item2);
                Write($"{tuple.Item1} ");
                ResetColor();
            }

            Write(Environment.NewLine);
        }

        public void WriteLine(LogLine logLine)
        {
            WriteBuilder(new LogBuilder().SetLine(logLine));
        }

        private void Write(string text)
        {
            write?.Invoke(text);
        }

        private void SetColor(ConsoleColor color)
        {
            setColor?.Invoke(color);
        }

        private void ResetColor()
        {
            resetColor?.Invoke();
        }
    }
}
