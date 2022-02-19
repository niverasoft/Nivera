using System;
using System.Diagnostics;
using System.Reflection;

namespace AtlasLib.Utils
{
    public static class ThrowHelper
    {
        public static void Throw<T>() where T : Exception, new()
        {
            throw new T();
        }

        public static void LogAndThrow<T>() where T : Exception, new()
        {
            Exception ex = new T();

            AtlasHelper.Fatal(ex);

            if (LibProperties.AddStackTraceToThrowHelper)
            {
                MethodBase method = new StackTrace().GetFrame(1).GetMethod();

                AtlasHelper.Fatal($"Exception thrown by {method.DeclaringType.FullName}.{method.Name}");
            }

            throw ex;
        }

        public static void Throw(object message)
        {
            throw new Exception(message.ToString());
        }

        public static void LogAndThrow(object message)
        {
            Exception ex = new Exception(message.ToString());

            AtlasHelper.Fatal(ex);

            if (LibProperties.AddStackTraceToThrowHelper)
            {
                MethodBase method = new StackTrace().GetFrame(1).GetMethod();

                AtlasHelper.Fatal($"Exception thrown by {method.DeclaringType.FullName}.{method.Name}");
            }

            throw ex;
        }
    }
}
