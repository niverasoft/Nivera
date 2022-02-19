using Fasterflect;

using AtlasLib.Utils;

using System;
using System.Linq;
using System.Reflection;

namespace AtlasLib.Reflection
{
    public class ReflectMethodHelper
    {
        private MethodInvoker _invoker;

        public string FullName { get => TargetType.Name() + "::" + TargetMethod.Name; }
        public Type TargetType { get => _invoker.Method.DeclaringType; }
        public MethodInfo TargetMethod { get => _invoker.Method; }
        public Type[] TypeParameters { get => _invoker.Method.GetParameters().Select(x => x.ParameterType).ToArray(); }

        private ReflectMethodHelper(MethodInvoker methodInvoker)
            => _invoker = methodInvoker;

        public object InvokeInstance(object instance, params object[] parameters)
        {
            Assert.NotNull(instance);
            return _invoker.Invoke(instance, parameters);
        }

        public object InvokeStatic(params object[] parameters)
        {
            return _invoker.Invoke(null, parameters);
        }

        internal static ReflectMethodHelper Create(MethodInfo methodInfo)
        {
            Assert.NotNull(methodInfo);
            MethodInvoker methodInvoker = methodInfo.DelegateForCallMethod();
            Assert.NotNull(methodInvoker);
            return new ReflectMethodHelper(methodInvoker);
        }
    }
}
