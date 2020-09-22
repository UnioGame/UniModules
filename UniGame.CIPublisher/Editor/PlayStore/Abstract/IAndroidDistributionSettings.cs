namespace ConsoleGPlayAPITool
{
    public interface IAndroidDistributionSettings
    {
        string PackageName       { get; }
        string JsonKeyPath       { get; }
        string ArtifactPath      { get; }
        string RecentChanges     { get; }
        string RecentChangesLang { get; }
        string TrackBranch       { get; }
        string ReleaseName       { get; }
        int    UserFraction      { get; }

        bool IsAppBundle { get; }

        string TrackStatus { get; }
    }
}