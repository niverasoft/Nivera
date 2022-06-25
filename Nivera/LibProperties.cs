using Nivera.Unity.CompatLayer;
using Nivera.Reflection;
using Nivera.Logging;
using Nivera.Properties;

using System;

namespace Nivera
{
    public static class LibProperties
    {
        public static readonly int BinaryVersion;
        public static readonly int LibraryVersion;

        static LibProperties()
        {
            BinaryVersion = int.Parse(Resources.BinaryVersion);
            LibraryVersion = int.Parse(Resources.LibraryVersion);
        }

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
            NiveraLog.Info($"Loading Nivera library ({BinaryVersion}:{LibraryVersion})..");

            NiveraLog.Verbose($"Log_DateTimeFormat = {Log_DateTimeFormat}");
            NiveraLog.Verbose($"Log_EnableDebugLog = {Log_EnableDebugLog}");
            NiveraLog.Verbose($"Log_EnableVerboseLog = {Log_EnableVerboseLog}");
            NiveraLog.Verbose($"Log_ShowCurrentFunctionInLog = {Log_ShowCurrentFunctionInLog}");
            NiveraLog.Verbose($"Log_AddStackTraceToThrowHelper = {Log_AddStackTraceToThrowHelper}");

            if (System_HandleUnhandledExceptions)
            {
                NiveraLog.Info($"System_HandleUnhandledExceptions = True");

                if (AppDomain.CurrentDomain != null)
                {
                    NiveraLog.Info($"App Domain found.");

                    AppDomain.CurrentDomain.UnhandledException += (x, y) =>
                    {
                        if (y.ExceptionObject != null)
                        {
                            NiveraLog.Fatal(y.ExceptionObject.ConvertTo<Exception>());
                        }
                    };
                }
                else
                {
                    NiveraLog.Error($"This app does not have a required App Domain.");
                }
            }

            if (System_UseUnityCompatModule)
            {
                NiveraLog.Info("System_UseUnityCompatModule = True");

                LibUnityCompatModule.EnableUnityCompatibility();

                NiveraLog.Info("Enabled Unity Engine compatibility.");
            }

            NiveraLog.Info($"Finished loading.");
        }
    }
}