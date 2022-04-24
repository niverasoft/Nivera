using System.Reflection;

namespace ArkLib.ConfigLib.Ark.Internals
{
    public class PropertySerializer
    {
        private PropertyInfo _propertyInfo;
        private object _typeInstanceRef;
        private string _key;

        public PropertySerializer(string _key, PropertyInfo _propertyInfo, object _typeInstanceRef)
        {
            this._propertyInfo = _propertyInfo;
            this._typeInstanceRef = _typeInstanceRef;
            this._key = _key;
        }

        public void Serialize(ConfigWriter writer)
        {
            new ValueSerializer(_key, _propertyInfo.GetValue(_typeInstanceRef)).Serialize(writer);
        }
    }
}
