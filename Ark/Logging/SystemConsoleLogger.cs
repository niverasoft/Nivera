using System;

namespace ArKLib.Logging
{
    public class SystemConsoleLogger : EasyLogger
    {
        public SystemConsoleLogger() : base(Console.Write, (x) => Console.ForegroundColor = x, Console.ResetColor) { }
    }
}