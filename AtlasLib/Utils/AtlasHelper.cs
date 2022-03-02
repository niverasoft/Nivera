using System.Reflection;

﻿namespace AtlasLib.Utils
{
    internal static class AtlasHelper
    {
        private static Dictionary<MethodBase, string> _functionNames = new Dictionary<MethodBase, string>();)
        private static MethodBase _curFunction;
        private static string _curFunctionString;
        
        private static void SetFunctionString(MethodBase nextFunc)
        {
          if (_curFunction == null || nextFunc != _curFunction)
          {
            _curFunction = nextFunc;
            _curFunctionString = $"{nextFunction.DeclaringType.FullName}::{nextFunction.Name}({string.Join(", ", nextFunc.GetParameters().Select(x => $"{x.ParameterType.FullName} {x.Name}"))})";
          }
        }
        
        internal static void Info(object message)
        { 
            string msg = message.ToString();
            
            if (LibProperties.ShowCurrentFunctionInLog)
            {
               SetFunctionString(new StackTrace().GetFrame(1).Method);
               
               msg += $"From Function: {_curFunctionString}";
            }
            
            LibProperties.Logger?.Info(msg);
        }

        internal static void Debug(object message)
        {
            if (!LibProperties.EnableDebugLog)
                return;

            string msg = message.ToString();
            
            if (LibProperties.ShowCurrentFunctionInLog)
            {
               SetFunctionString(new StackTrace().GetFrame(1).Method);
               
               msg += $"From Function: {_curFunctionString}";
            }
            
            LibProperties.Logger?.Debug(msg);
        }

        internal static void Error(object message)
        {
            string msg = message.ToString();
            
            if (LibProperties.ShowCurrentFunctionInLog)
            {
               SetFunctionString(new StackTrace().GetFrame(1).Method);
               
               msg += $"From Function: {_curFunctionString}";
            }
            
            LibProperties.Logger?.Error(msg);
        }

        internal static void Fatal(object message)
        {
            string msg = message.ToString();
            
            if (LibProperties.ShowCurrentFunctionInLog)
            {
               SetFunctionString(new StackTrace().GetFrame(1).Method);
               
               msg += $"From Function: {_curFunctionString}";
            }
            
            LibProperties.Logger?.Fatal(msg);
        }

        internal static void Verbose(object message)
        {
            if (!LibProperties.EnableVerboseLog)
                return;

            string msg = message.ToString();
            
            if (LibProperties.ShowCurrentFunctionInLog)
            {
               SetFunctionString(new StackTrace().GetFrame(1).Method);
               
               msg += $"From Function: {_curFunctionString}";
            }
            
            LibProperties.Logger?.Verbose(msg);
        }

        internal static void Warn(object message)
        {
            string msg = message.ToString();
            
            if (LibProperties.ShowCurrentFunctionInLog)
            {
               SetFunctionString(new StackTrace().GetFrame(1).Method);
               
               msg += $"From Function: {_curFunctionString}";
            }
            
            LibProperties.Logger?.Warn(msg);
        }
    }
}
