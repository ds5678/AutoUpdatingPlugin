using System.Collections.Generic;

namespace AutoUpdatingPlugin
{
	internal static class IntersectedList
	{
		internal static readonly Dictionary<string, APIMod> stringApiMods = new Dictionary<string, APIMod>();
		internal static readonly Dictionary<string, InstalledModDetail> stringInstalledMods = new Dictionary<string, InstalledModDetail>();
		internal static readonly Dictionary<InstalledModDetail, APIMod> installedApiMods = new Dictionary<InstalledModDetail, APIMod>();

		internal static void GenerateLists()
		{
			Logger.Msg("Generating an intersection of installed mods and the supported api...");
			foreach (KeyValuePair<string, InstalledModDetail> installedMod in InstalledModList.installedMods)
			{
				bool foundApiEntry = false;
				foreach (KeyValuePair<string, APIMod> remoteMod in APIList.supportedMods)
				{
					if (installedMod.Key == remoteMod.Key)
					{
						foundApiEntry = true;
						stringApiMods.Add(remoteMod.Key, remoteMod.Value);
						stringInstalledMods.Add(installedMod.Key, installedMod.Value);
						installedApiMods.Add(installedMod.Value, remoteMod.Value);
						break;
					}
				}
				if (!foundApiEntry)
				{
					Logger.Warning($"There is no associated API entry for '{installedMod.Key}'");
				}
			}
			Logger.Msg($"Found {stringApiMods.Count} supported mods installed.");
		}
	}
}
