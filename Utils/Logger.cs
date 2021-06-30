using MelonLoader;

namespace AutoUpdatingPlugin
{
    internal static class Logger
    {
        internal static void Msg(string message) => MelonLogger.Msg(message);
        internal static void Msg(string message, params object[] parameters) => MelonLogger.Msg(message, parameters);
        internal static void Warning(string message, params object[] parameters) => MelonLogger.Warning(message, parameters);
        internal static void Error(string message, params object[] parameters) => MelonLogger.Error(message, parameters);
    }
}
