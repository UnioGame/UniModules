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

            var logBuilder = new StringBuilder(200);
            
            var activeBuildNumber = this.GetActiveBuildNumber(buildTarget);
            var resultBuildNumber = activeBuildNumber + buildNumber;          
            
            var version = GetBuildVersion(buildTarget,resultBuildNumber);
            PlayerSettings.bundleVersion = version;

            var buildNumberString =  resultBuildNumber.ToString();
            PlayerSettings.iOS.buildNumber = buildNumberString;
            PlayerSettings.Android.bundleVersionCode = resultBuildNumber;
            
            logBuilder.Append("\t Parameters build number : ");
            logBuilder.Append(buildNumber);
            logBuilder.AppendLine();
            
            logBuilder.Append("\t Active build number : ");
            logBuilder.Append(activeBuildNumber);
            logBuilder.AppendLine();
            
            logBuilder.Append("\t ResultBuildNumber build number : ");
            logBuilder.Append(resultBuildNumber);
            logBuilder.AppendLine();
                                  
            logBuilder.Append("\t BundleVersion build number : ");
            logBuilder.Append(version);
            logBuilder.AppendLine();
            
            logBuilder.Append("\t PlayerSettings.iOS.buildNumber : ");
            logBuilder.Append(buildNumberString);
            logBuilder.AppendLine();
            
            logBuilder.Append("\t PlayerSettings.Android.bundleVersionCode : ");
            logBuilder.Append(resultBuildNumber);
            logBuilder.AppendLine();
            
        }
        
        private string GetBuildVersion(BuildTarget buildTarget, int buildNumber) {
            
            var version       = PlayerSettings.bundleVersion;
            var versionLenght = GetVersionLength(buildTarget);
            var versionPoints = version.Split('.');
            
            var versionBuilder = new StringBuilder();

            for (var i = 0; i < versionLenght; i++) {
                versionBuilder.Append(versionPoints.Length > i ? versionPoints[i] : "0");
                versionBuilder.Append(".");
            }
                        
            versionBuilder.Append(buildNumber);
            return versionBuilder.ToString();
            
        }
        
        private int GetActiveBuildNumber(BuildTarget target) {

            var activeVersion = this.minBuildNumber;
            
            switch (target) {
                case BuildTarget.Android:
                    activeVersion += PlayerSettings.Android.bundleVersionCode;
                    break;
                case BuildTarget.iOS:
                    int.TryParse(PlayerSettings.iOS.buildNumber, out var iosBuildNumber);
                    activeVersion += iosBuildNumber;
                    break;
                default:
                    int.TryParse(PlayerSettings.bundleVersion, out var standaloneBuildNumber);
                    activeVersion += standaloneBuildNumber;
                    break;
            }

            return activeVersion;

        }

        private int GetVersionLength(BuildTarget buildTarget) {
            var length =  buildTarget == BuildTarget.iOS ? 2 : 3;
            return length;
        }
    }
    
}
