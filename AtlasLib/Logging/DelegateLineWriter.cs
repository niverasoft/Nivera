using System;

namespace AtlasLib.Logging
{
    internal class DelegateLineWriter : ILogWriter
    {
        internal bool isValid;

        internal Action<string> _writingDelegate;
        internal Action<LogColor> _colorDelegate;
        internal Action _restartDelegate;

        internal DelegateLineWriter(Action<string> writingDelegate, Action<LogColor> colorDelegate = null, Action restartDelegate = null)
        {
            _writingDelegate = writingDelegate;
            _colorDelegate = colorDelegate;
            _restartDelegate = restartDelegate;

            isValid = writingDelegate != null;
        }


        public void WriteLine(LogLine logLine)
        {
            if (!isValid)
                throw new InvalidOperationException($"You have to set the _writingDelegate before executing this method.");

            for (int i = 0; i < logLine.Total; i++)
            {
                WriteString(logLine.LineColors[i], logLine.Lines[i]);
            }

            WriteString(LogColor.Invisible, Environment.NewLine);

            _restartDelegate?.Invoke();
        }

        public void WriteString(LogColor color, string str)
        {
            if (isValid)
            {
                if (_colorDelegate != null)
                {
                    _colorDelegate.Invoke(color);
                }

                _writingDelegate.Invoke(str);
            }
        }
    }
}
