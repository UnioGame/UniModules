namespace UniGreenModules.UniGame.UnityBuild.Editor.ClientBuild.Commands
{
    using BuildConfiguration;
    using UniModules.UniGame.UnityBuild.Editor.ClientBuild.BuildConfiguration;
    using UnityEditor;
    using UnityEditor.Build.Reporting;

    public class UnityEditorBuildCommands 
    {
        [MenuItem("UnityPlayerBuild/Android")]
        public static void BuildAndroid()
        {
            ExecuteBuild("android.apk",BuildTarget.Android, BuildTargetGroup.Android);
        }

        [MenuItem("UnityPlayerBuild/iOS Release")]
        public static void BuildIOS()
        {
            ExecuteBuild(string.Empty,BuildTarget.iOS, BuildTargetGroup.iOS);
        }

        public static EditorBuildConfiguration CreateConfiguration(string outputFileName,BuildTarget buildTarget,BuildTargetGroup targetGroup)
        {
            var argumentsProvider = new ArgumentsProvider(new[] {
                $"{BuildArguments.BuildOutputFolderKey}:Builds",
                $"{BuildArguments.BuildOutputNameKey}:{outputFileName}",
            });

            var buildConfiguration = new EditorBuildConfiguration(
                argumentsProvider, 
                new BuildParameters(buildTarget, targetGroup, argumentsProvider));
            return buildConfiguration;
        }

        public static BuildReport ExecuteBuild(string outputFileName,BuildTarget buildTarget,BuildTargetGroup targetGroup)
        {
            var buildConfiguration = CreateConfiguration(outputFileName, buildTarget, targetGroup);
            return UnityBuildTool.BuildPlayer(buildConfiguration);
        }
        
        public static BuildReport ExecuteBuild(IUniBuildCommandsMap commandsMap)
        {
            var buildData = commandsMap.BuildData;
            var configuration = CreateConfiguration(buildData.ArtifactName, buildData.BuildTarget, buildData.BuildTargetGroup);
            return UnityBuildTool.BuildPlayer(configuration,commandsMap);
        }
    }
}
