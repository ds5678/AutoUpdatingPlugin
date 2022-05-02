namespace AutoUpdatingPlugin
{
#if NET6_0_OR_GREATER
	public static class Implementation
	{
#else
    internal sealed class Implementation : MelonLoader.MelonPlugin
    {
        public override void OnPreInitialization()
        {
            try
            {
                SelfUpdater.CheckVersion();

                AssetRipper.VersionUtilities.UnityVersion unityVersion = MelonLoader.InternalUtils.UnityInformationHandler.EngineVersion;
                if (unityVersion < AssetRipper.VersionUtilities.UnityVersion.Parse("2019.4.19"))
                {
                    Logger.Msg($"Skipping mod updates because TLD is outdated. Unity Version: {unityVersion}");
                    return;
                }

                
            }
            catch (System.Exception e)
            {
                Logger.Error("Failed to update mods:\n" + e);
            }
        }
#endif
		public static void UpdateMods()
		{
			ZipFileHandler.ExtractZipFilesInDirectory(FileUtils.ModsFolder);

			APIList.FetchRemoteMods();

			InstalledModList.ScanModFolder();

			IntersectedList.GenerateLists();

			ModUpdater.DownloadAndUpdateMods();

			DependencyHandler.InstallAllMissingDependencies();

			ZipFileHandler.ExtractZipFilesInDirectory(FileUtils.ModsFolder);
		}
	}
}
