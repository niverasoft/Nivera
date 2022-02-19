using System.Collections.Generic;
using System.Linq;

namespace AtlasLib.Logging
{
    internal class Logger : ILogger
    {
        private ILogWriter _writer;
        private LogQueue _curQueue;
        private List<ILogger> _loggers;

        internal Logger(ILogWriter writer)
        {
            _writer = writer;
            _loggers = new List<ILogger>();
        }

        public void Write(string message)
        {
            string str = message.ToString();

            LogLine line = new LogLine();
            LogColorParser parser = new LogColorParser();
            int curIndex = 0;

            foreach (string arg in str.Split(' '))
            {
                line.LineColors.Add(curIndex, parser.ParseColor(arg));
                line.Lines.Add(curIndex, parser.ParseText(arg));

                curIndex++;
            }

            line.Finish();

            _writer.WriteLine(line);

            if (_loggers.Any())
                _loggers.ForEach(x => x.Write(message));
        }

        public void FinishQueue()
        {
            LogLine line = new LogLine();
            LogColorParser parser = new LogColorParser();
            int curIndex = 0;

            while (_curQueue.TryGet(out string log))
            {
                line.LineColors.Add(curIndex, parser.ParseColor(log));
                line.Lines.Add(curIndex, parser.ParseText(log));

                curIndex++;
            }

            line.Finish();

            _writer.WriteLine(line);

            _curQueue.FinishQueue();
            _curQueue = null;
        }

        public Logger Emit(LogColor color, string text)
        {
            if (_curQueue == null)
                _curQueue = new LogQueue();

            LogColorParser parser = new LogColorParser();

            text = parser.InsertColor(color, text);

            _curQueue.Insert(text);

            return this;
        }

        public Logger EmitLine(string line)
        {
            if (_curQueue == null)
                _curQueue = new LogQueue();

            _curQueue.Insert(line);

            return this;
        }

        public Logger WithCustomLogger(ILogger logger)
        {
            if (logger == null)
                return this;

            _loggers.Add(logger);

            return this;
        }

        public string WithColor(LogColor color, string text)
        { 
            return new LogColorParser().InsertColor(color, text);
        }
    }
}
