namespace UniGreenModules.UnityBuild.Editor.UnityCloudBuild
{
    internal class CloudBuildArgs
    {
        public CloudBuildArgs(int buildNumber, string bundleId, string projectId, string scmCommitId, string scmBranch, string cloudBuildTargetName)
        {
            BuildNumber = buildNumber;
            BundleId    = bundleId;
            ProjectId   = projectId;
            ScmCommitId = scmCommitId;
            ScmBranch = scmBranch;
            CloudBuildTargetName = cloudBuildTargetName;
        }

        /// <summary>
        /// The Unity Cloud Build “build number” corresponding to this build.
        /// </summary>
        public int BuildNumber { get; }

        /// <summary>
        /// The bundleIdentifier configured in Unity Cloud Build (iOS and Android only).
        /// </summary>
        public string BundleId { get; }

        /// <summary>
        /// The Unity project identifier.
        /// </summary>
        public string ProjectId { get; }

        /// <summary>
        /// The commit or changelist that was built.
        /// </summary>
        public string ScmCommitId { get; }

        /// <summary>
        /// The name of the branch that was built.
        /// </summary>
        public string ScmBranch { get; }

        /// <summary>
        /// The name of the build target that was built (pipeline identifier).
        /// </summary>
        public string CloudBuildTargetName { get; }
    }
}