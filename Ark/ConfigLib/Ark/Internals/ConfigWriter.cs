using System;
using System.Linq;
using System.Collections.Generic;

using ArKLib.Utils;

namespace ArKLib.ConfigLib.Ark.Internals
{
    public class ConfigWriter
    {
        private List<string> _curLines;

        public void Write(int emptySpaces, string line)
        {
            if (_curLines == null)
                _curLines = new List<string>();

            ArkLog.Verbose($"ConfigWriter >> Writing: {line} with {emptySpaces} empty space(s)");

            if (emptySpaces > 0)
            {
                _curLines.Add($"{EmptySpaces(emptySpaces)}{line}");
            }
            else
            {
                _curLines.Add(line);
            }
        }

        public List<string> ReadLines()
        {
            return _curLines;
        }

        public string Read()
        {
            return string.Join(Environment.NewLine, _curLines);
        }

        public string ReadFirst()
        {
            return _curLines.FirstOrDefault();
        }

        public void WriteToFirst(string str)
        {
            if (_curLines == null || _curLines.Count <= 0)
                Write(0, str);
            else
                _curLines[0] = $"{_curLines[0]} {str}";
        }

        private string EmptySpaces(int length)
        {
            string str = "";
            int curLoop = 0;

            while (curLoop != length)
            {
                str += " ";
                curLoop++;
            }

            return str;
        }
    }
}
