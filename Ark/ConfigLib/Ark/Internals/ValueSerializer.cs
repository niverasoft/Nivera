using System;
using System.Collections;

using ArKLib.Reflection;

namespace ArKLib.ConfigLib.Ark.Internals
{
    public class ValueSerializer
    {
        private Type _valueType;
        private object _value;
        private string _key;

        public ValueSerializer(string key, object value)
        {
            _value = value;
            _valueType = value?.GetType();
            _key = key;
        }

        public void Serialize(ConfigWriter writer)
        {
            if (ReflectUtils.IsNull(_value))
            {
                writer.Write(0, $"{_key} = null");
                return;
            }

            if (ReflectUtils.IsDictionary(_valueType))
            {
                writer.Write(0, $"{_key} =");

                new DictionarySerializer(_key, _value as IDictionary).Serialize(writer);
            }
            else if (ReflectUtils.IsArray(_valueType))
            {
                writer.Write(0, $"{_key} =");

                new ArraySerializer(_key, _value as Array).Serialize(writer);
            }
            else if (ReflectUtils.IsList(_valueType))
            {
                writer.Write(0, $"{_key} =");

                new ListSerializer(_key, _value as IEnumerable).Serialize(writer);
            }
            else
            {
                if (!ReflectUtils.IsSimple(_valueType))
                    return;

                if (_value is DateTime dateTime)
                {
                    writer.Write(0, $"{_key} = {dateTime.ToString("G")}");
                }
                else if (_value is Enum en)
                {
                    writer.Write(0, $"{_key} = {en}");
                }
                else 
                {
                    writer.Write(0, $"{_key} = {_value}");
                }
            }
        }
    }
}
