using System;

namespace ArKLib.Reflection.Patching.PropertyPatchers.Instance
{
    public class InstancePropertyCallback<TValue> : CallbackBase where TValue : class
    {
        public Action<object, TValue> ValueSet;
        public Func<object, TValue> ValueGet;
    }
}
