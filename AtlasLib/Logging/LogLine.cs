using System.Collections.Generic;

using AtlasLib.Utils;

namespace AtlasLib.Logging
{
    public struct LogLine
    {
        public Dictionary<int, LogColor> LineColors;
        public Dictionary<int, string> Lines;
        public int Total;

        public void Finish()
        {
            Assert.Equals(LineColors.Count, Lines.Count);

            Total = LineColors.Count;
        }
    }
}