using System;
using System.Reflection;

namespace AtlasLib.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class PatchMethodAttribute : Attribute
    {
        public PatchMethodAttribute(Type declaringType, string methodName, Type[] argumentTypes = null)
        {
            DeclaringType = declaringType;
            TargetMethod = argumentTypes != null ? declaringType.GetMethod(methodName, argumentTypes) : declaringType.GetMethod(methodName);

            Utils.Assert.NotNull(TargetMethod);
        }

        public Type DeclaringType { get; }
        public MethodInfo TargetMethod { get; }
    }
}
