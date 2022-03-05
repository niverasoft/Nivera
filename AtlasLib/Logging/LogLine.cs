using System;
using System.Collections.Generic;

namespace AtlasLib.Logging
{
    public struct LogLine
    {
        public const ConsoleColor DefaultColor = ConsoleColor.White;

        private List<Tuple<string, ConsoleColor>> line;

        public void WriteSingle(ConsoleColor color, string text)
        {
            if (line == null)
                line = new List<Tuple<string, ConsoleColor>>();

            line.Add(new Tuple<string, ConsoleColor>(text, color));
        }

        public void Finish()
        {
            line.Clear();
            line = null;
        }

        public List<Tuple<string, ConsoleColor>> ReadAllLines()
        {
            return line;
        }
    }
}