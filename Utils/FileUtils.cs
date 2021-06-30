using Mono.Cecil;
using System;
using System.IO;
using System.Linq;

namespace AutoUpdatingPlugin
{
    internal static class FileUtils
    {
        internal static string GetPluginsFolder() => Path.Combine(MelonLoader.MelonUtils.GameDirectory, @"Plugins");

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

        internal static string GetPathSelf()
        {
            foreach(string filename in Directory.GetFiles(GetPluginsFolder(),"*.dll"))
            {
                try
                {
                    using (AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(filename, new ReaderParameters { ReadWrite = true }))
                    {

                        CustomAttribute melonInfoAttribute = assembly.CustomAttributes.FirstOrDefault(a =>
                            a.AttributeType.Name == "MelonModInfoAttribute" || a.AttributeType.Name == "MelonInfoAttribute");

                        if (melonInfoAttribute == null)
                            continue;

                        string name = melonInfoAttribute.ConstructorArguments[1].Value as string;

                        if (name == BuildInfo.Name) return filename;
                    }
                }
                catch (Exception)
                {
                    Logger.Msg("Failed to read assembly " + filename);
                }
            }

            return "";
        }
    }
}
