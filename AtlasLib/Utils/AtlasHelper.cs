namespace AtlasLib.Utils
{
    internal static class AtlasHelper
    {
        internal static void Info(object message)
        {
            LibProperties.Logger?.Info(message.ToString());
        }

        internal static void Debug(object message)
        {
            if (!LibProperties.EnableDebugLog)
                return;

            LibProperties.Logger?.Debug(message.ToString());
        }

        internal static void Error(object message)
        {
            LibProperties.Logger?.Error(message.ToString());
        }

        internal static void Fatal(object message)
        {
            LibProperties.Logger?.Fatal(message.ToString());
        }

        internal static void Verbose(object message)
        {
            if (!LibProperties.EnableVerboseLog)
                return;

            LibProperties.Logger?.Trace(message.ToString());
        }

        internal static void Warn(object message)
        {
            LibProperties.Logger?.Warn(message.ToString());
        }
    }
}
