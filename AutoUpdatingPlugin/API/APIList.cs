using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace AutoUpdatingPlugin
{
	internal static class APIList
	{
		private static readonly Dictionary<string, string> oldToNewModNames = new Dictionary<string, string>()
		{
			// Used in case something is missing on the API
		};

		internal static readonly Dictionary<string, APIMod> allMods = new Dictionary<string, APIMod>();
		internal static readonly Dictionary<string, APIMod> supportedMods = new Dictionary<string, APIMod>();
		internal static void FetchRemoteMods()
		{
			Logger.Msg("Fetching remote mods...");
			string apiResponse = "";
			using (WebClient? client = new WebClient())
			{
				client.Headers["User-Agent"] = "AutoUpdatingPlugin";
				Logger.Msg("Attempting to download from API site...");
				apiResponse = client.DownloadString("http://tld.xpazeapps.com/api.json");
			}

			Logger.Msg("Downloaded from API site. Attempting to parse data...");

			APIMod[] apiMods = APIReader.Deserialize(apiResponse);

			supportedMods.Clear();

			foreach (APIMod mod in apiMods)
			{
				allMods.Add(mod.name, mod);

				if (!mod.enableUpdate)
				{
					Logger.Msg($"Automatic updating for {mod.name} has been disabled by the mod author.");
					continue;
				}

				if (mod.ContainsModSceneFile())
				{
					Logger.Msg($"Automatic updating for {mod.name} has been disabled due to potentially large file sizes.");
					continue;
				}

				if (mod.downloadlinks.Length == 0)
				{
					Logger.Msg($"Automatic updating for {mod.name} has been disabled due to having no valid download links.");
					continue;
				}

				// Aliases
				foreach (string alias in mod.aliases)
				{
					if (alias != mod.name && !oldToNewModNames.ContainsKey(alias))
					{
						oldToNewModNames[alias] = mod.name;
					}
				}

				// Add to known mods
				supportedMods.Add(mod.name, mod);
			}

			Logger.Msg("API returned " + apiMods.Length + " mods, including " + supportedMods.Count + " supported mods.");
		}
		internal static string GetNewModName(string currentName)
		{
			return oldToNewModNames.TryGetValue(currentName, out string? newName) ? newName : currentName;
		}
		internal static bool IsAliasName(string currentName) => oldToNewModNames.ContainsKey(currentName);

		internal static string[] GetModNames() => supportedMods.Keys.ToArray();
		internal static string[] GetSortedModNames()
		{
			List<string>? result = new List<string>(supportedMods.Keys.ToArray());
			result.Sort();
			return result.ToArray();
		}

		internal static Dictionary<string, APIMod> SortedDictionary()
		{
			IOrderedEnumerable<KeyValuePair<string, APIMod>>? sortedDict = from entry in supportedMods orderby entry.Key ascending select entry;
			return sortedDict.ToDictionary(pair => pair.Key, pair => pair.Value);
		}
	}
}