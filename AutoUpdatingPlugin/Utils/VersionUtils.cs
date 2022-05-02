namespace AutoUpdatingPlugin
{
	internal static class VersionUtils
    {
        public static VersionData GetMaxVersion(VersionData[] versions)
        {
            if (versions is null || versions.Length == 0) return VersionData.ZERO;

            int currentMax = 0;
            for (int i = 1; i < versions.Length; i++)
            {
                if (versions[currentMax] < versions[i]) currentMax = i;
            }

            if (versions[currentMax] is null || !versions[currentMax].IsValidSemver) return VersionData.ZERO;
            else return versions[currentMax];
        }

        /// <summary>
        /// Gets the minimum version from an array
        /// </summary>
        /// <param name="versions"></param>
        /// <param name="excludeInvalidVersions">If true, null and invalid versions will be excluded from comparisons.</param>
        /// <returns></returns>
        public static VersionData GetMinVersion(VersionData[] versions, bool excludeInvalidVersions)
        {
            if (versions is null || versions.Length == 0) return VersionData.ZERO;

            int currentMin = 0;
            for (int i = 1; i < versions.Length; i++)
            {
                if (excludeInvalidVersions && (versions[i] is null || !versions[i].IsValidSemver))
                {
                    continue;
                }
                if (versions[currentMin] > versions[i]) currentMin = i;
            }

            if (versions[currentMin] is null || !versions[currentMin].IsValidSemver) return VersionData.ZERO;
            else return versions[currentMin];
        }
    }
}
