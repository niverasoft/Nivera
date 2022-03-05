using System;

namespace AtlasLib.Logging
{
    public static class StringExtensions
    {
        public static string AttachColor(this string str, ConsoleColor color)
        {
            return $"{str}-={color}=-";
        }
    }
}
