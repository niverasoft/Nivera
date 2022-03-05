using System;
using System.Reflection;
using System.Reflection.Emit;

using AtlasLib.Utils;

using HarmonyLib;

namespace AtlasLib.Reflection.Patching.PropertyPatchers.Static
{
    internal class StaticPropertyPatcher<TValueType> : PropertyPatcherBase where TValueType : class
    {
        private MethodInfo _getPatch;
        private MethodInfo _setPatch;
        private Harmony _harmony;
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
            Assert.Equals(accessType, AccessType.Static, "You cannot patch non-static methods with a static patcher.");

            if (propertyAccessor == PropertyAccessor.Getter)
                PatchPropertyGetter();
            else
                PatchPropertySetter();
        }

        public override void RemovePatch(AccessType accessType, PropertyAccessor propertyAccessor)
        {
            Assert.Equals(accessType, AccessType.Static, "You cannot unpatch non-static methods with a static patcher.");

            if (propertyAccessor == PropertyAccessor.Getter)
                UnPatchPropertyGetter();
            else
                UnPatchPropertySetter();
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

        private void UnPatchPropertyGetter()
        {
            Assert.True(_patchedGetter);

            _harmony.Unpatch(Property.GetMethod, _getPatch);
        }

        private void UnPatchPropertySetter()
        {
            Assert.True(_patchedSetter);

            _harmony.Unpatch(Property.SetMethod, _setPatch);
        }

        private void PatchPropertyGetter()
        {
            DynamicMethod dynamicMethod = new DynamicMethod($"get_{Property.Name}", Constants.BoolType, new Type[] { _callback.ValueGet.GetType(), Property.PropertyType }, GetType());

            ILGenerator iLGenerator = dynamicMethod.GetILGenerator();

            iLGenerator.Emit(OpCodes.Ldarg_1);
            iLGenerator.Emit(OpCodes.Ldarg_0);
            iLGenerator.Emit(OpCodes.Ldfld, AccessTools.Field(typeof(StaticPropertyPatcher<TValueType>), "_callback"));
            iLGenerator.Emit(OpCodes.Ldfld, AccessTools.Field(typeof(StaticPropertyCallback<TValueType>), "ValueGet"));
            iLGenerator.EmitCall(OpCodes.Callvirt, _callback.ValueGet.Method, null);
            iLGenerator.Emit(OpCodes.Stobj, typeof(TValueType));
            iLGenerator.Emit(OpCodes.Ldc_I4_0);
            iLGenerator.Emit(OpCodes.Ldloc_0);
            iLGenerator.Emit(OpCodes.Br_S);
            iLGenerator.Emit(OpCodes.Ldloc_0);
            iLGenerator.Emit(OpCodes.Ret);

            _getPatch = _harmony.Patch(Property.GetMethod, new HarmonyMethod(dynamicMethod));
            _patchedGetter = true;
        }

        private void PatchPropertySetter()
        {
            DynamicMethod dynamicMethod = new DynamicMethod($"set_{Property.Name}", Constants.BoolType, new Type[] { _callback.ValueGet.GetType(), Property.PropertyType }, GetType());

            ILGenerator iLGenerator = dynamicMethod.GetILGenerator();

            iLGenerator.Emit(OpCodes.Ldarg_0);
            iLGenerator.Emit(OpCodes.Ldfld, AccessTools.Field(typeof(StaticPropertyPatcher<TValueType>), "_callback"));
            iLGenerator.Emit(OpCodes.Ldfld, AccessTools.Field(typeof(StaticPropertyCallback<TValueType>), "ValueSet"));
            iLGenerator.Emit(OpCodes.Ldarg_1);
            iLGenerator.EmitCall(OpCodes.Callvirt, _callback.ValueSet.Method, null);
            iLGenerator.Emit(OpCodes.Ldc_I4_0);
            iLGenerator.Emit(OpCodes.Stloc_0);
            iLGenerator.Emit(OpCodes.Br_S);
            iLGenerator.Emit(OpCodes.Ldloc_0);
            iLGenerator.Emit(OpCodes.Ret);

            _setPatch = _harmony.Patch(Property.SetMethod, new HarmonyMethod(dynamicMethod));
            _patchedSetter = true;
        }
    }
}
