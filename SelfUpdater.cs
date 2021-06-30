using MelonLoader.TinyJSON;
using System;
using System.IO;
using System.Net;
using System.Threading;

namespace AutoUpdatingPlugin
{
    internal static class SelfUpdater
    {
        private const string DownloadURL = @"https://github.com/ds5678/AutoUpdatingPlugin/releases/latest/download/AutoUpdatingPlugin.dll";
        private const string VersionURL = @"https://raw.githubusercontent.com/ds5678/AutoUpdatingPlugin/master/Version.json";

        internal static void CheckVersion()
        {
            Logger.Msg("Fetching updater version data...");
            string apiResponse = "";
            using (var client = new WebClient())
            {
                client.Headers["User-Agent"] = "AutoUpdatingPlugin";
                Logger.Msg("Attempting to download from API site...");
                apiResponse = client.DownloadString(VersionURL);
            }

            if (string.IsNullOrWhiteSpace(apiResponse))
            {
                Logger.Error("Failed to download the data.");
            }
            else
            {
                Logger.Msg("Downloaded from the repository. Attempting to parse data...");

                var data = JSON.Load(apiResponse) as ProxyObject;

                string version = data["Version"];
                Logger.Msg(version);
                if ((VersionData) BuildInfo.Version >= (VersionData) version)
                {

                    Logger.Msg("The Auto Updating Plugin is up-to-date.");
                }
                else
                {
                    Logger.Msg("The Auto Updating Plugin is out-dated. Updating now...");
                    string path = FileUtils.GetPathSelf();
                    Logger.Msg(path);
                    try
                    {
                        bool errored = false;
                        using (var client = new WebClient())
                        {
                            bool downloading;
                            byte[] buffer;
                            client.DownloadDataCompleted += (sender, e) =>
                            {
                                if (e.Error != null)
                                {
                                    Logger.Error("Failed to download a newer version of the Auto Updating Plugin:\n" + e.Error);
                                    errored = true;
                                }
                                else buffer = e.Result;

                                downloading = false;
                            };
                            downloading = true;
                            buffer = null;
                            client.DownloadDataAsync(new Uri(DownloadURL));

                            while (downloading)
                                Thread.Sleep(50);


                            if (!errored)
                            {
                                try
                                {
                                    File.WriteAllBytes(path, buffer);
                                }
                                catch (Exception e)
                                {
                                    Logger.Error("Failed to save replacement files for a newer version of the Auto Updating Plugin:\n" + e);
                                    return;
                                }
                            }
                        }
                        Logger.Msg("The Auto Updating Plugin has been successfully updated. The game must be relaunched for these changes to take effect.");
                        EndProgram();
                    }
                    catch (Exception e)
                    {
                        Logger.Error("Failed to update the Auto Updating Plugin:\n" + e);
                    }
                }
            }
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
