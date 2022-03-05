using System;

namespace AtlasLib.Reflection.Patching.PropertyPatchers.Static
{
    public class StaticPropertyCallback<TValue> : CallbackBase where TValue : class
    {
        public Action<TValue> ValueSet;
        public Func<TValue> ValueGet;
    }
}
