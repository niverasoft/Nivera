using System;

namespace AtlasLib.Logging
{
    internal class LogColorParser
    {
        public static bool UsePrevColor = false;

        internal LogColor? _prevColor;

        internal string ParseText(string col)
        {
            int startIndex = 0;
            int endIndex = col.Length;
            int curIndex = 0;

            bool move = true;

            while (move)
            {
                char c = col[curIndex];

                if (c == ':')
                {
                    endIndex = curIndex;
                    move = false;
                }

                curIndex++;
            }

            string colorString = col.Substring(startIndex, endIndex);

            return colorString.Replace("]", "");
        }

        internal LogColor ParseColor(string col)
        {
            int startIndex = 0;
            int endIndex = col.Length;
            int curIndex = 0;

            bool move = true;

            while (move)
            {
                char c = col[curIndex];

                if (c == ':')
                {
                    endIndex = curIndex;
                    move = false;
                }

                curIndex++;
            }

            string colorString = col.Substring(startIndex, endIndex);

            colorString = colorString.Replace("[", "").Replace(":", "");

            if (!Enum.TryParse(colorString, out LogColor lColor))
            {
                lColor = UsePrevColor && _prevColor.HasValue ? _prevColor.Value : LogColor.White;
            }

            _prevColor = lColor;

            return lColor;
        }

        internal string InsertColor(LogColor color, string str)
        {
            return $"[{color}: {str}]";
        }
    }
}
