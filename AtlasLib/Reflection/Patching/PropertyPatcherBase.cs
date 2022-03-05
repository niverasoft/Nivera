using System;
using System.Reflection;

namespace AtlasLib.Reflection.Patching
{
    public abstract class PropertyPatcherBase
    {
        public object CurValue { get; }

        public abstract bool IsPatchedGetter { get; }
        public abstract bool IsPatchedSetter { get; }

        public abstract Type DeclaringType { get; }
        public abstract PropertyInfo Property { get; }

        public abstract CallbackBase Callback { get; }

        public abstract void AddPatch(AccessType accessType, PropertyAccessor propertyAccessor);
        public abstract void RemovePatch(AccessType accessType, PropertyAccessor propertyAccessor);

        public abstract void SetValue(object value); 
    }
}