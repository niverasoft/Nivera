using AtlasLib.Utils;
using AtlasLib.Reflection;

using System;

using NLog;

namespace AtlasLib
{
    public static class LibProperties
    {
        public static ILogger Logger;

        public static bool AddStackTraceToThrowHelper;
        public static bool HandleUnhandledExceptions;
        public static bool EnableDebugLog;
        public static bool EnableVerboseLog;

        public static void Load()
        {
            if (HandleUnhandledExceptions)
            {
                if (AppDomain.CurrentDomain != null)
                {
                    AppDomain.CurrentDomain.UnhandledException += (x, y) =>
                    {
                        if (y.ExceptionObject != null)
                        {
                            AtlasHelper.Fatal(y.ExceptionObject.ConvertTo<Exception>());
                        }
                    };
                }
            }
        }
    }
}