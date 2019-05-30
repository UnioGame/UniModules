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
        private int minBuildNumber = 0;
        
        public override void Execute(IUniBuilderConfiguration configuration)
        {

            var buildParameters = configuration.BuildParameters;
            UpdateBuildVersion(buildParameters.BuildTarget, buildParameters.BuildNumber);
            
        }
        
        public void UpdateBuildVersion(BuildTarget buildTarget,int buildNumber) 
        {
            var buildVersionProvider = new BuildVersionProvider();
            var logBuilder = new StringBuilder(200);

            var activeBuildNumber = buildNumber + minBuildNumber;
            var bundleVersion = buildVersionProvider.GetBuildVersion(buildTarget, PlayerSettings.bundleVersion, activeBuildNumber);
            var resultBuildNumber = buildVersionProvider.GetActiveBuildNumber(buildTarget,activeBuildNumber);

            PlayerSettings.bundleVersion = bundleVersion;
            var buildNumberString =  resultBuildNumber.ToString();
            PlayerSettings.iOS.buildNumber = buildNumberString;
            PlayerSettings.Android.bundleVersionCode = resultBuildNumber;
            
            logBuilder.Append("\t Parameters build number : ");
            logBuilder.Append(buildNumber);
            logBuilder.AppendLine();
 
            logBuilder.Append("\t ResultBuildNumber build number : ");
            logBuilder.Append(resultBuildNumber);
            logBuilder.AppendLine();
                                  
            logBuilder.Append("\t PlayerSettings.bundleVersion : ");
            logBuilder.Append(bundleVersion);
            logBuilder.AppendLine();
            
            logBuilder.Append("\t PlayerSettings.iOS.buildNumber : ");
            logBuilder.Append(buildNumberString);
            logBuilder.AppendLine();
            
            logBuilder.Append("\t PlayerSettings.Android.bundleVersionCode : ");
            logBuilder.Append(resultBuildNumber);
            logBuilder.AppendLine();
            
        }
    }
    
}
