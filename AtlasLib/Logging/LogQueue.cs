using System.Collections.Generic;
using System.Linq;

namespace AtlasLib.Logging
{
    internal class LogQueue
    {
        private List<string> queue;

        internal void Insert(string log)
        {
            queue.Add(log);
        }

        internal bool TryGet(out string log)
        {
            log = null;

            if (queue.Count == 0)
                return false;

            log = queue.ElementAt(0);

            queue.RemoveAt(0);

            return true;
        }

        internal void FinishQueue()
        {
            queue.Clear();
            queue = null;
        }

        internal void StartQueue()
        {
            queue = new List<string>();
        }
    }
}
