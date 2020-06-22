namespace UniGreenModules.UniGame.UnityBuild.Editor.ClientBuild.Commands
{
    using BuildConfiguration;
    using UniModules.UniGame.UnityBuild.Editor.ClientBuild.BuildConfiguration;
    using UnityEditor;
    using UnityEditor.Build.Reporting;

    public class UniEditorBuildCommands 
    {
        [MenuItem("UniBuild/Android")]
        public static void BuildAndroid()
        {
            ExecuteBuild("android.apk",BuildTarget.Android, BuildTargetGroup.Android);
        }

        [MenuItem("UniBuild/iOS")]
        public static void BuildIOS()
        {
            ExecuteBuild(string.Empty,BuildTarget.iOS, BuildTargetGroup.iOS);
        }
        
        [MenuItem("UniBuild/Windows")]
        public static void BuildWindows()
        {
            ExecuteBuild(string.Empty,BuildTarget.StandaloneWindows64, BuildTargetGroup.Standalone);
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
