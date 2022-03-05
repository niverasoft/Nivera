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

            AtlasLogger.Fatal(ex);

            if (LibProperties.Log_AddStackTraceToThrowHelper)
            {
                MethodBase method = new StackTrace().GetFrame(1).GetMethod();

                AtlasLogger.Fatal($"Exception thrown by {method.DeclaringType.FullName}.{method.Name}");
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

            AtlasLogger.Fatal(ex);

            if (LibProperties.Log_AddStackTraceToThrowHelper)
            {
                MethodBase method = new StackTrace().GetFrame(1).GetMethod();

                AtlasLogger.Fatal($"Exception thrown by {method.DeclaringType.FullName}.{method.Name}");
            }

            throw ex;
        }
    }
}
