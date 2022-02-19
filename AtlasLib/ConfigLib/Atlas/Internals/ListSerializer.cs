using System;
using System.Collections;

using AtlasLib.Reflection;

namespace AtlasLib.ConfigLib.Atlas.Internals
{
    public class ListSerializer
    {
        public const int MinOffset = 3;
        public const char ListPreset = '>';

        private IEnumerable _enum;
        private string _key;

        public ListSerializer(string key, IEnumerable enume)
        {
            _enum = enume;
            _key = key;
        }

        public void Serialize(ConfigWriter writer)
        {
            int offset = _key.Length + MinOffset;
            int index = 0;

            IEnumerator enumerator = _enum.GetEnumerator();

            enumerator.Reset();

            while (enumerator.MoveNext())
            {
                object curr = enumerator.Current;

                if (ReflectUtils.IsNull(curr))
                    continue;
                else if (!ReflectUtils.IsSimple(curr.GetType()))
                    continue;
                else
                {
                    string str = "";

                    if (curr is DateTime dateTime)
                    {
                        str = dateTime.ToString("G");
                    }
                    else
                    {
                        str = curr.ToString();
                    }

                    if (!string.IsNullOrWhiteSpace(str))
                    {
                        if (index == 0)
                            writer.WriteToFirst(str);
                        else
                            writer.Write(offset, str);
                    }
                }
            }
        }
    }
}
