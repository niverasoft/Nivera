using System;
using System.Collections.Generic;

namespace AtlasLib.Logging
{
    internal class ConsoleLineWriter : ILogWriter
    {
        internal static Dictionary<LogColor, ConsoleColor> ConsoleColors = new Dictionary<LogColor, ConsoleColor>
        {
            [LogColor.Blue] = ConsoleColor.Blue,
            [LogColor.Cyan] = ConsoleColor.Cyan,
            [LogColor.DarkRed] = ConsoleColor.DarkRed,
            [LogColor.Invisible] = ConsoleColor.White,
            [LogColor.Yellow] = ConsoleColor.Yellow,
            [LogColor.White] = ConsoleColor.White,
            [LogColor.Orange] = ConsoleColor.DarkYellow
        };

        public void WriteLine(LogLine logLine)
        {
            for (int i = 0; i < logLine.Total; i++)
            {
                WriteString(logLine.LineColors[i], logLine.Lines[i]);
            }

            WriteString(LogColor.Invisible, Environment.NewLine);
        }

        public void WriteString(LogColor color, string str)
        {
            ConsoleColor consoleColor = ConsoleColors[color];

            if (color != LogColor.Invisible)
            {
                Console.ForegroundColor = consoleColor;
                Console.Write(str);
            }    
            else
            {
                Console.Write(str);
            }

            Console.ResetColor();
        }
    }
}
