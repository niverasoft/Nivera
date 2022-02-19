using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using HarmonyLib;

using AtlasLib.Utils;

namespace AtlasLib.Harmony
{
    public class StaticPropertyPatcher<T>
    {
        private DynamicMethod _getPatchMethod;
        private DynamicMethod _setPatchMethod;
        private MethodInfo _harmonyGetPatch;
        private MethodInfo _harmonySetPatch;
        private MethodBase _origGetMethodBase;
        private MethodBase _origSetMethodBase;
        private MethodBuilder _origGetMethod;
        private MethodBuilder _origSetMethod;
        private HarmonyLib.Harmony _harmonyInstance;

        public readonly Func<T> Getter;
        public readonly Action<T> Setter;
        public readonly PropertyInfo Target;

        public StaticPropertyPatcher(PropertyInfo target, Func<T> getter, Action<T> setter)
        {
            Assert.NotNull(target);
            Assert.EitherNull(getter, setter);
            Assert.Equals(typeof(T), getter?.Method.GetGenericArguments().First() ?? setter?.Method.GetGenericArguments().First());

            Target = target;
            Getter = getter;
            Setter = setter;

            _harmonyInstance = new HarmonyLib.Harmony(target.DeclaringType.FullName + "." + target.Name);
        }

        public void UnpatchGetter()
        {
            Assert.NotNull(_origGetMethodBase);
            Assert.NotNull(_harmonyGetPatch);

            _harmonyInstance.Unpatch(_origGetMethod, _harmonyGetPatch);
        }

        public void UnpatchSetter()
        {
            Assert.NotNull(_origSetMethodBase);
            Assert.NotNull(_harmonySetPatch);

            _harmonyInstance.Unpatch(_origSetMethod, _harmonySetPatch);
        }

        public void PatchGetter()
        {
            Assert.NotNull(Target.GetMethod);
            _getPatchMethod = new DynamicMethod($"get_{Target.Name}", Target.PropertyType, null);
            _origGetMethodBase = Target.GetMethod;
            GenerateIlForGetPatchMethod(_getPatchMethod.GetILGenerator());
            _harmonyGetPatch = _harmonyInstance.Patch(Target.GetMethod, new HarmonyMethod(_getPatchMethod));
        }

        public void PatchSetter()
        {
            Assert.NotNull(Target.SetMethod);
            _setPatchMethod = new DynamicMethod($"set_{Target.Name}", Constants.VoidType, new Type[] { typeof(T) });
            _origSetMethodBase = Target.SetMethod;
            GenerateIlForSetPatchMethod(_setPatchMethod.GetILGenerator());
            _harmonySetPatch = _harmonyInstance.Patch(Target.SetMethod, new HarmonyMethod(_setPatchMethod));
        }

        private void GenerateIlForGetPatchMethod(ILGenerator ilGenerator)
        {
            ilGenerator.Emit(OpCodes.Nop);
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldfld, AccessTools.Field(typeof(StaticPropertyPatcher<T>), nameof(Getter)));
            ilGenerator.EmitCall(OpCodes.Callvirt, AccessTools.Method(typeof(Func<T>), "Invoke", null), null);
            ilGenerator.Emit(OpCodes.Pop);
            ilGenerator.Emit(OpCodes.Ldc_I4_0);
            ilGenerator.Emit(OpCodes.Stloc_0);
            ilGenerator.Emit(OpCodes.Br_S);
            ilGenerator.Emit(OpCodes.Ldloc_0);
            ilGenerator.Emit(OpCodes.Ret);
        }

        private void GenerateIlForSetPatchMethod(ILGenerator ilGenerator)
        {
            ilGenerator.Emit(OpCodes.Nop);
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldfld, AccessTools.Field(typeof(StaticPropertyPatcher<T>), nameof(Setter)));
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.EmitCall(OpCodes.Callvirt, AccessTools.Method(typeof(Action<T>), "Invoke", new Type[] { typeof(T) }, null), null);
            ilGenerator.Emit(OpCodes.Nop);
            ilGenerator.Emit(OpCodes.Ldc_I4_0);
            ilGenerator.Emit(OpCodes.Stloc_0);
            ilGenerator.Emit(OpCodes.Br_S);
            ilGenerator.Emit(OpCodes.Ldloc_0);
            ilGenerator.Emit(OpCodes.Ret);
        }

        // this is a really, REALLY janky solution
        private void CreateGetMethodAssembly(MethodBody getterBody)
        {
            AssemblyName assemblyName = new AssemblyName($"get_{Target.Name}");
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule($"Module_get_{Target.Name}");
            MethodBuilder methodBuilder = moduleBuilder.DefineGlobalMethod("OriginalGetter", MethodAttributes.Public | MethodAttributes.Static, Target.PropertyType, Type.EmptyTypes);
            methodBuilder.SetMethodBody(getterBody.GetILAsByteArray(), getterBody.MaxStackSize, null, getterBody.ExceptionHandlingClauses.Select(x => new ExceptionHandler(x.TryOffset,
                x.TryLength, x.FilterOffset, x.HandlerOffset, x.HandlerLength, x.Flags, x.CatchType.MetadataToken)), null);
            _origGetMethod = methodBuilder;
        }
    }
}
