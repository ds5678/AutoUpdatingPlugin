using System;
using System.IO;

namespace AutoUpdatingPlugin
{
    internal static class ModComponentScanner
    {
        internal static void ScanForModComponentFiles()
        {
            string basedirectory = FileUtils.GetModComponentZipsFolder();

            if (!Directory.Exists(basedirectory))
            {
                Logger.Msg("No ModComponentZips folder. Creating...");
                Directory.CreateDirectory(basedirectory);
                return;
            }

            string[] mctbl = Directory.GetFiles(basedirectory, "*.modcomponent");
            if (mctbl.Length > 0)
            {
                for (int i = 0; i < mctbl.Length; i++)
                {
                    string filename = mctbl[i];
                    if (string.IsNullOrEmpty(filename)) continue;

                    if (filename.EndsWith(".dev.modcomponent"))
                    {
                        Logger.Msg($"Skipping development mod '{filename}'");
                        continue;
                    }

                    try
                    {
                        BuildInfoDetail buildInfo = InternalZipInspector.InspectZipFile(filename);

                        string modName;
                        string version = buildInfo.Version;

                        if (string.IsNullOrWhiteSpace(buildInfo.Name))
                        {
                            modName = Path.GetFileNameWithoutExtension(filename);
                        }
                        else modName = buildInfo.Name;

                        bool isAliasName = APIList.IsAliasName(modName);
                        if (isAliasName)
                        {
                            string newName = APIList.GetNewModName(modName);
                            Logger.Msg($"'{modName}' is obsolete. It will be replaced with '{newName}'.");
                            modName = newName;
                        }

                        if (InstalledModList.installedMods.TryGetValue(modName, out InstalledModDetail installedModDetail))
                        {
                            installedModDetail.files.Add(new InstalledFileDetail(modName, version, filename, InstalledFileType.ModComponent));
                            if (isAliasName) installedModDetail.TriggerOutdated();
                        }
                        else
                        {
                            InstalledModDetail newModDetail = new InstalledModDetail(modName);
                            newModDetail.files.Add(new InstalledFileDetail(modName, version, filename, InstalledFileType.ModComponent));
                            if (isAliasName) newModDetail.TriggerOutdated();
                            InstalledModList.installedMods.Add(modName, newModDetail);
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Msg($"Failed to read modcomponent file {filename}: {e}");
                    }
                }
            }
        }
    }
}
