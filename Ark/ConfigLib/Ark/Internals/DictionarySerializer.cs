using System;
using System.Collections;

using ArKLib.Reflection;
using ArKLib.Utils;

namespace ArKLib.ConfigLib.Ark.Internals
{
    public class DictionarySerializer
    {
        public const int MinOffset = 3;
        public const string DictPreset = ">>";

        private IDictionary _dict;
        private string _key;

        public DictionarySerializer(string key, IDictionary dict)
        {
            _dict = dict;
            _key = key;

            ArkLog.Verbose($"DictionarySerializer >> Entering: {key}");
        }

        public void Serialize(ConfigWriter writer)
        {
            int offset = _key.Length + MinOffset;
            int index = 0;

            ArkLog.Verbose($"DictionarySerializer >> Offset: {offset}");

            IDictionaryEnumerator enumerator = _dict.GetEnumerator();

            enumerator.Reset();

            while (enumerator.MoveNext())
            {
                object value = enumerator.Value;
                object key = enumerator.Key;

                if (ReflectUtils.IsNull(key))
                    continue;
                else if (!ReflectUtils.IsSimple(key.GetType()) || (!ReflectUtils.IsNull(value) && !ReflectUtils.IsSimple(value.GetType())))
                    continue;
                else
                {
                    string keyStr = key.ToString();
                    string valueStr = ReflectUtils.IsNull(value) ? "null" : value.ToString();

                    if (value != null && value is DateTime time)
                        valueStr = time.ToString("G");

                    ArkLog.Verbose($"DictionarySerializer >> Current Key: {keyStr}");
                    ArkLog.Verbose($"DictionarySerializer >> Current Value: {valueStr}");

                    if (index == 0)
                        writer.WriteToFirst($"{keyStr} > {valueStr}");
                    else
                        writer.Write(offset, $"{DictPreset} {keyStr} > {valueStr}");
                }
            }
        }
    }
}
