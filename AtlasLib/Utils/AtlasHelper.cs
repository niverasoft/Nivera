using AtlasLib.Logging;

namespace AtlasLib.Utils
{
    internal static class AtlasHelper
    {
        internal static void Info(object message)
        {
            LibProperties.Logger?.Write(message.ToString().WithTag(BasicLogTag.Information));
        }

        internal static void Debug(object message)
        {
            LibProperties.Logger?.Write(message.ToString().WithTag(BasicLogTag.Debug));
        }

        internal static void Error(object message)
        {
            LibProperties.Logger?.Write(message.ToString().WithTag(BasicLogTag.Error));
        }

        internal static void Fatal(object message)
        {
            LibProperties.Logger?.Write(message.ToString().WithTag(BasicLogTag.Fatal));
        }

        internal static void Verbose(object message)
        {
            LibProperties.Logger?.Write(message.ToString().WithTag(BasicLogTag.Verbose));
        }

        internal static void Warn(object message)
        {
            LibProperties.Logger?.Write(message.ToString().WithTag(BasicLogTag.Warning));
        }
    }
}
