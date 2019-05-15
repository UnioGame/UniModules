namespace UniGreenModules.UnityBuild.Editor.ClientBuild.Interfaces {
    using UnityEditor;

    public interface IBuildParameters {
        
        BuildTarget BuildTarget { get; }

        BuildTargetGroup BuildTargetGroup { get; }

        int BuildNumber { get; }
        string OutputFile { get; }
        string OutputFolder { get; }
        BuildOptions BuildOptions { get; }

        string ProjectId { get; }

        string BundleId { get; }

        void SetBuildOptions(BuildOptions targetBuildOptions, bool replace = true);
    }
}