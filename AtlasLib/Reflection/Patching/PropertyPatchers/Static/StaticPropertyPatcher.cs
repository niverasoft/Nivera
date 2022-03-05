using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using AtlasLib.Utils;

namespace AtlasLib.Reflection.Patching.PropertyPatchers.Static
{
    internal class StaticPropertyPatcher<TValueType> : PropertyPatcherBase where TValueType : class
    {
        private StaticPropertyCallback<TValueType> _callback;
        private PropertyInfo _property;
        private bool _patchedGetter;
        private bool _patchedSetter;

        public new TValueType CurValue { get => _patchedGetter ? _callback.ValueGet() : _property.GetValue(null).ConvertTo<TValueType>(); }

        public override bool IsPatchedGetter { get => _patchedGetter; }
        public override bool IsPatchedSetter { get => _patchedSetter; }

        public override Type DeclaringType => _property.DeclaringType;
        public override PropertyInfo Property => _property;
        public override CallbackBase Callback => _callback;

        public override void AddPatch(AccessType accessType, PropertyAccessor propertyAccessor)
        {

        }

        public override void RemovePatch(AccessType accessType, PropertyAccessor propertyAccessor)
        {

        }

        public override void SetValue(object value)
        {
            Assert.IsType<TValueType>(value);

            if (_patchedSetter)
            {
                _callback.ValueSet(value.ConvertTo<TValueType>());
            }
            else
            {
                _property.SetValue(null, value);
            }
        }
    }
}
