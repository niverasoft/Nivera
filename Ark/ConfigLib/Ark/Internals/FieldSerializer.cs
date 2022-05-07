using System.Reflection;

namespace ArKLib.ConfigLib.Ark.Internals
{
    public class FieldSerializer
    {
        private FieldInfo _fieldInfo;
        private object _typeInstanceRef;
        private string _key;

        public FieldSerializer(string _key, FieldInfo _fieldInfo, object _typeInstanceRef)
        {
            this._fieldInfo = _fieldInfo;
            this._typeInstanceRef = _typeInstanceRef;
            this._key = _key;
        }

        public void Serialize(ConfigWriter writer)
        {
            new ValueSerializer(_key, _fieldInfo.GetValue(_typeInstanceRef)).Serialize(writer);
        }
    }
}
