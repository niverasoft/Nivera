using ArKLib.Unity.CompatLayer;
using ArKLib.Reflection;
using ArKLib.Logging;

using System;

namespace ArKLib
{
    public static class LibProperties
    {
        public static ILogger Logger;

        public static bool System_HandleUnhandledExceptions;
        public static bool System_UseUnityCompatModule;

        public static string Log_DateTimeFormat = "t";
        public static bool Log_EnableDebugLog;
        public static bool Log_EnableVerboseLog;
        public static bool Log_ShowCurrentFunctionInLog;
        public static bool Log_AddStackTraceToThrowHelper;

        public static void Load()
        {
            if (System_HandleUnhandledExceptions)
            {
                if (AppDomain.CurrentDomain != null)
                {
                    AppDomain.CurrentDomain.UnhandledException += (x, y) =>
                    {
                        if (y.ExceptionObject != null)
                        {
                            ArkLog.Fatal(y.ExceptionObject.ConvertTo<Exception>());
                        }
                    };
                }
            }

            if (System_UseUnityCompatModule)
                LibUnityCompatModule.EnableUnityCompatibility();
        }
    }
}