using MelonLoader;
using System;
using System.Threading;

namespace AutoUpdatingPlugin
{
    internal class Implementation : MelonPlugin
    {
        private const float postUpdateDisplayDuration = 3;
        public override void OnPreInitialization()
        {
            try
            {
                Thread.Sleep(500);

                SelfUpdater.CheckVersion();

                string unityVersion = MelonUtils.GetUnityVersion();
                if ((VersionData)unityVersion < (VersionData)"2019.4.19")
                {
                    Logger.Msg($"Skipping mod updates because TLD is outdated. Unity Version: {unityVersion}");
                    return;
                }

                ZipFileHandler.ExtractZipFilesInDirectory(FileUtils.GetModsFolder());

                APIList.FetchRemoteMods();

                InstalledModList.ScanModFolder();

                IntersectedList.GenerateLists();

                ModUpdater.DownloadAndUpdateMods();

                DependencyHandler.InstallAllMissingDependencies();

                ZipFileHandler.ExtractZipFilesInDirectory(FileUtils.GetModsFolder());

                Thread.Sleep((int)(postUpdateDisplayDuration * 1000));
            }
            catch (Exception e)
            {
                Logger.Error("Failed to update mods:\n" + e);
            }
        }


    }
}
