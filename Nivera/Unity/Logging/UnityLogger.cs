using System.Threading.Tasks;
using System.Collections.Concurrent;

using Nivera.Logging;
using Nivera.Unity.CompatLayer;

using UnityEngine;

namespace Nivera.Unity.Logging
{
    public class UnityLogger : Nivera.Logging.ILogger
    {
        private Task _updateTask;
        private ConcurrentQueue<string> _waitingLines = new ConcurrentQueue<string>();

        public UnityLogger()
        {
            if (LibUnityCompatModule.IsActive)
            {
                LibUnityCompatModule.RegisterUpdateMethod(UpdateQueue);
            }
            else
            {
                _updateTask = Task.Run( async () =>
                {
                    while (true)
                    {
                        await Task.Delay(200);

                        UpdateQueue();
                    }
                });
            }
        }

        public LogBuilder Builder => new LogBuilder().SetParent(this);

        public void WriteBuilder(LogBuilder logBuilder)
        {
            WriteLine(logBuilder.GetLine());
        }

        public void WriteLine(LogLine logLine)
        {
            string line = "";

            foreach (var tuple in logLine.ReadAllLines())
            {
                line += $"{tuple.Item1} ";
            }

            _waitingLines.Enqueue(line);
        }

        private void UpdateQueue()
        {
            if (_waitingLines.TryDequeue(out string line))
            {
                if (line.ToLower().Contains("error"))
                {
                    Debug.LogError(line);
                }
                else if (line.ToLower().Contains("warn"))
                {
                    Debug.LogWarning(line);
                }
                else
                {
                    Debug.Log(line);
                }
            }
        }
    }
}