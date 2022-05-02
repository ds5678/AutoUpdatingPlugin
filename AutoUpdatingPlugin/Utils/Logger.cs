namespace AutoUpdatingPlugin
{
	internal static class Logger
	{
#if NET6_0_OR_GREATER
		internal static void Msg(string message) => System.Console.WriteLine($"Info: {message}");
		internal static void Warning(string message) => System.Console.WriteLine($"Warning: {message}");
		internal static void Error(string message) => System.Console.WriteLine($"Error: {message}");
#else
        internal static void Msg(string message) => MelonLoader.MelonLogger.Msg(message);
        internal static void Warning(string message) => MelonLoader.MelonLogger.Warning(message);
        internal static void Error(string message) => MelonLoader.MelonLogger.Error(message);
#endif
	}
}
