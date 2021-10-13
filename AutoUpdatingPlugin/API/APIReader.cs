using MelonLoader.TinyJSON;
using System.Collections.Generic;

namespace AutoUpdatingPlugin
{
    internal static class APIReader
    {
        internal static readonly string[] compatibleFileTypes = new string[] { ".dll", ".modcomponent", ".modscene", ".zip" };
        internal static APIMod[] Deserialize(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new APIMod[0];

            List<APIMod> result = new List<APIMod>();

            var mods = JSON.Load(text) as ProxyObject;

            foreach (var mod in mods)
            {
                ProxyObject modData = mod.Value as ProxyObject;
                APIMod apiMod = new APIMod();
                apiMod.name = modData["Name"];
                apiMod.version = (VersionData)(string)(modData["Version"]);
                apiMod.aliases = MakeStringArray(modData["Aliases"] as ProxyArray);
                apiMod.dependencies = MakeStringArray(modData["Dependencies"] as ProxyArray);
                apiMod.enableUpdate = modData["Updater"]["enable_update"];
                apiMod.downloadlinks = MakeDownloadArray(modData["Updater"]["downloads"] as ProxyArray);
                result.Add(apiMod);
            }

            return result.ToArray();
        }

        public static string[] MakeStringArray(ProxyArray proxy)
        {
            string[] result = new string[proxy.Count];
            for (int i = 0; i < proxy.Count; i++)
            {
                result[i] = proxy[i];
            }
            return result;
        }

        public static string[] MakeDownloadArray(ProxyArray proxy)
        {
            List<string> result = new List<string>();
            for (int i = 0; i < proxy.Count; i++)
            {
                string link = proxy[i];
                if (IsCompatibleLink(link)) result.Add(link);
                else return new string[0];
            }
            return result.ToArray();
        }

        public static bool IsCompatibleLink(string link)
        {
            if (string.IsNullOrWhiteSpace(link)) return false;
            foreach (string fileType in compatibleFileTypes)
            {
                if (link.EndsWith(fileType)) return true;
            }
            return false;
        }
    }
}
