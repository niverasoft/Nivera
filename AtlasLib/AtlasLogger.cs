using System.Reflection;
using System.Linq;
using System;
using System.Diagnostics;

using AtlasLib.Utils;
using AtlasLib.Logging;

﻿namespace AtlasLib
{
    public static class AtlasLogger
    {
        private static string _curFunctionString;

        private static void SetFunctionString()
        {
            StackTrace stackTrace = new StackTrace();

            foreach (StackFrame stackFrame in stackTrace.GetFrames())
            {
                MethodBase methodBase = stackFrame.GetMethod();

                Type declaringType = methodBase.DeclaringType;

                if (declaringType == typeof(AtlasLogger) || declaringType == typeof(ThrowHelper) || declaringType == typeof(Assert))
                    continue;
                else
                {
                    _curFunctionString = $"{methodBase.DeclaringType.FullName}::{methodBase.Name}({string.Join(", ", methodBase.GetParameters().Select(x => $"{x.ParameterType.FullName} {x.Name}"))})";

                    return;
                }
            }
        }

        public static void Info(object message)
        {
            LogBuilder logBuilder = new LogBuilder();

            logBuilder.AttachWord(DateTime.Now.ToLocalTime().ToString(LibProperties.Log_DateTimeFormat), ConsoleColor.White);
            logBuilder.AttachWord(">>", ConsoleColor.White);
            logBuilder.AttachWord("Information", ConsoleColor.Gray);

            if (LibProperties.Log_ShowCurrentFunctionInLog)
            {
                SetFunctionString();

                logBuilder.AttachWord(">>", ConsoleColor.White);
                logBuilder.AttachWord($"{_curFunctionString}", ConsoleColor.Gray);
            }

            logBuilder.AttachWord(">>", ConsoleColor.White);
            logBuilder.AttachWord(message.ToString(), ConsoleColor.Gray);
            logBuilder.Finish(LibProperties.Logger);
        }

        public static void Debug(object message)
        {
            if (!LibProperties.Log_EnableDebugLog)
                return;

            LogBuilder logBuilder = new LogBuilder();

            logBuilder.AttachWord(DateTime.Now.ToLocalTime().ToString(LibProperties.Log_DateTimeFormat), ConsoleColor.White);
            logBuilder.AttachWord(">>", ConsoleColor.White);
            logBuilder.AttachWord("Debug", ConsoleColor.Cyan);

            if (LibProperties.Log_ShowCurrentFunctionInLog)
            {
                SetFunctionString();

                logBuilder.AttachWord(">>", ConsoleColor.White);
                logBuilder.AttachWord($"{_curFunctionString}", ConsoleColor.Cyan);
            }

            logBuilder.AttachWord(">>", ConsoleColor.White);
            logBuilder.AttachWord(message.ToString(), ConsoleColor.Cyan);
            logBuilder.Finish(LibProperties.Logger);
        }

        public static void Error(object message)
        {
            LogBuilder logBuilder = new LogBuilder();

            logBuilder.AttachWord(DateTime.Now.ToLocalTime().ToString(LibProperties.Log_DateTimeFormat), ConsoleColor.White);
            logBuilder.AttachWord(">>", ConsoleColor.White);
            logBuilder.AttachWord("Error", ConsoleColor.Red);

            if (LibProperties.Log_ShowCurrentFunctionInLog)
            {
                SetFunctionString();

                logBuilder.AttachWord(">>", ConsoleColor.White);
                logBuilder.AttachWord($"{_curFunctionString}", ConsoleColor.Red);
            }

            logBuilder.AttachWord(">>", ConsoleColor.White);
            logBuilder.AttachWord(message.ToString(), ConsoleColor.Red);
            logBuilder.Finish(LibProperties.Logger);
        }

        public static void Fatal(object message)
        {
            SetFunctionString();

            LogBuilder logBuilder = new LogBuilder();

            logBuilder.AttachWord(DateTime.Now.ToLocalTime().ToString(LibProperties.Log_DateTimeFormat), ConsoleColor.White);
            logBuilder.AttachWord(">>", ConsoleColor.White);
            logBuilder.AttachWord("Fatal Error", ConsoleColor.DarkRed);
            logBuilder.AttachWord(">>", ConsoleColor.White);
            logBuilder.AttachWord($"{_curFunctionString}", ConsoleColor.DarkRed);
            logBuilder.AttachWord(">>", ConsoleColor.White);
            logBuilder.AttachWord(message.ToString(), ConsoleColor.DarkRed);
            logBuilder.Finish(LibProperties.Logger);
        }

        public static void Verbose(object message)
        {
            if (!LibProperties.Log_EnableVerboseLog)
                return;

            LogBuilder logBuilder = new LogBuilder();

            logBuilder.AttachWord(DateTime.Now.ToLocalTime().ToString(LibProperties.Log_DateTimeFormat), ConsoleColor.White);
            logBuilder.AttachWord(">>", ConsoleColor.White);
            logBuilder.AttachWord("Verbose", ConsoleColor.White);

            if (LibProperties.Log_ShowCurrentFunctionInLog)
            {
                SetFunctionString();

                logBuilder.AttachWord(">>", ConsoleColor.White);
                logBuilder.AttachWord($"{_curFunctionString}", ConsoleColor.White);
            }

            logBuilder.AttachWord(">>", ConsoleColor.White);
            logBuilder.AttachWord(message.ToString(), ConsoleColor.White);
            logBuilder.Finish(LibProperties.Logger);
        }

        public static void Warn(object message)
        {
            LogBuilder logBuilder = new LogBuilder();

            logBuilder.AttachWord(DateTime.Now.ToLocalTime().ToString(LibProperties.Log_DateTimeFormat), ConsoleColor.White);
            logBuilder.AttachWord(">>", ConsoleColor.White);
            logBuilder.AttachWord("Warning", ConsoleColor.Yellow);

            if (LibProperties.Log_ShowCurrentFunctionInLog)
            {
                SetFunctionString();

                logBuilder.AttachWord(">>", ConsoleColor.White);
                logBuilder.AttachWord($"{_curFunctionString}", ConsoleColor.Yellow);
            }

            logBuilder.AttachWord(">>", ConsoleColor.White);
            logBuilder.AttachWord(message.ToString(), ConsoleColor.Yellow);
            logBuilder.Finish(LibProperties.Logger);
        }
    }
}
