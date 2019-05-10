namespace Build {
    using UnityEditor;

    public interface IBuildParameters {
        BuildTarget BuildTarget { get; }
        int BuildNumber { get; }
        string OutputFile { get; }
        string OutputFolder { get; }
        BuildOptions BuildOptions { get; }

        string ProjectId { get; }

        string BundleId { get; }
    }
}