using System.Net;

namespace AutoUpdatingPlugin
{
    internal static class SelfUpdater
    {
        private const string URL = @"";

        internal static void CheckVersion()
        {
            Logger.Msg("Fetching updater version data...");
            string apiResponse = "";
            using (var client = new WebClient())
            {
                client.Headers["User-Agent"] = "AutoUpdatingPlugin";
                Logger.Msg("Attempting to download from API site...");
                apiResponse = client.DownloadString(URL);
            }

            Logger.Msg("Downloaded from the repository. Attempting to parse data...");

            MelonLoader.MelonLogger.Msg(apiResponse);
        }

        private static void EndProgram()
        {
            Logger.Msg("Ending Program in 10 seconds");
            var x = new System.Diagnostics.Stopwatch();
            x.Start();
            while (x.ElapsedMilliseconds < 10000) { }
            System.Environment.Exit(1);
        }
    }
}
