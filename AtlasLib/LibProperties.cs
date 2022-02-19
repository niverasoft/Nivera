using AtlasLib.Logging;
using AtlasLib.Utils;
using AtlasLib.Reflection;

using System;

namespace AtlasLib
{
    public static class LibProperties
    {
        public static ILogger Logger;

        public static bool AddStackTraceToThrowHelper;
        public static bool HandleUnhandledExceptions;

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