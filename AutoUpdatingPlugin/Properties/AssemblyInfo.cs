using System.Reflection;
using System.Runtime.InteropServices;
using BuildInfo = AutoUpdatingPlugin.BuildInfo;

[assembly: ComVisible(false)]
[assembly: Guid("cf5eba4d-26d9-43fd-9797-8a63514efa5f")]

[assembly: AssemblyTitle(BuildInfo.Name)]
[assembly: AssemblyDescription(BuildInfo.Description)]
[assembly: AssemblyCompany(BuildInfo.Company)]
[assembly: AssemblyProduct(BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + BuildInfo.Author)]
[assembly: AssemblyTrademark(BuildInfo.Company)]
[assembly: AssemblyCulture("")]

[assembly: AssemblyVersion(BuildInfo.Version)]
[assembly: AssemblyFileVersion(BuildInfo.Version)]

[assembly: MelonLoader.MelonInfo(typeof(AutoUpdatingPlugin.Implementation), BuildInfo.Name, BuildInfo.Version, BuildInfo.Author, BuildInfo.DownloadLink)]
[assembly: MelonLoader.MelonGame("Hinterland", "TheLongDark")]
