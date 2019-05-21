namespace UniGreenModules.UnityBuild.Editor.ClientBuild.Commands.PreBuildCommands 
{
    using System.Text;
    using Interfaces;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// update current project version
    /// </summary>
    [CreateAssetMenu(menuName = "UnityBuild/PreBuildCommands/Update Project Version", fileName = "UpdateVersionCommand")]
    public class UpdateVersionCommand : UnityPreBuildCommand
    {
        [SerializeField]
        private int minBuildNumber = 1;
        
        public override void Execute(IUniBuilderConfiguration configuration)
        {

            var buildParameters = configuration.BuildParameters;
            UpdateBuildVersion(buildParameters.BuildTarget, buildParameters.BuildNumber);
            
        }
        
        public void UpdateBuildVersion(BuildTarget buildTarget,int buildNumber)
        {
            var buildVersionHandler = new BuildVersionProvider();
            var logBuilder = new StringBuilder(200);

            var activeBuildNumber = buildNumber + minBuildNumber;
            var bundleVersionCode = buildVersionHandler.GetActiveBuildNumber(buildTarget,activeBuildNumber);
            var buildNumberString =  bundleVersionCode.ToString();
            var bundleVersion = PlayerSettings.bundleVersion;
            
            var version = buildVersionHandler.GetBuildVersion(buildTarget,bundleVersion,buildNumber);
            
            PlayerSettings.bundleVersion = version;
            PlayerSettings.iOS.buildNumber = buildNumberString;
            PlayerSettings.Android.bundleVersionCode = bundleVersionCode;
            
            logBuilder.Append("\t Parameters build number : ");
            logBuilder.Append(buildNumber);
            logBuilder.AppendLine();

            logBuilder.Append("\t PlayerSettings.bundleVersion : ");
            logBuilder.Append(version);
            logBuilder.AppendLine();
            
            logBuilder.Append("\t PlayerSettings.iOS.buildNumber : ");
            logBuilder.Append(buildNumberString);
            logBuilder.AppendLine();
            
            logBuilder.Append("\t PlayerSettings.Android.bundleVersionCode : ");
            logBuilder.Append(bundleVersionCode);
            logBuilder.AppendLine();
            
        }

    }
    
}
