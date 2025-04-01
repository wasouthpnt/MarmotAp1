


namespace MarmotAp.Helpers
{
    public class VersionAndBuild
    {
        public VersionAndBuild()
        {
            VersionTracking.Track();
        }
        public string GetVersionNumber()
        {
            // Current app version
            var currentVersion = VersionTracking.CurrentVersion;
            return currentVersion.ToString();
        }
        public string GetBuildNumber()
        {
            // Current build
            var currentBuild = VersionTracking.CurrentBuild;
            return currentBuild.ToString();
        }
    }
}

