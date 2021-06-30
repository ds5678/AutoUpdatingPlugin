using System;
using System.IO;

namespace AutoUpdatingPlugin
{
    internal static class FileUtils
    {
        internal static string GetModsFolder() => Path.Combine(MelonLoader.MelonUtils.GameDirectory, @"Mods");

        internal static string GetModComponentZipsFolder() => Path.Combine(GetModsFolder(), @"ModComponentZips");

        public static string GetDestination(string link)
        {
            if (string.IsNullOrWhiteSpace(link)) throw new ArgumentException("Invalid link argument");

            if (link.EndsWith(".modcomponent") || link.EndsWith(".modscene"))
            {
                return Path.Combine(GetModComponentZipsFolder(), Path.GetFileName(link));
            }
            else return Path.Combine(GetModsFolder(), Path.GetFileName(link));
        }
    }
}
