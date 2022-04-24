using System;
using System.Collections;

using ArkLib.Reflection;

namespace ArkLib.ConfigLib.Ark.Internals
{
    public class ArraySerializer
    {
        public const int MinOffset = 3;
        public const char ListPreset = '>';

        private Array _array;
        private string _key;

        public ArraySerializer(string key, Array array)
        {
            _array = array;
            _key = key;
        }

        public void Serialize(ConfigWriter writer)
        {
            int offset = _key.Length + MinOffset;
            int index = 0;

            IEnumerator enumerator = _array.GetEnumerator();

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
