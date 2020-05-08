namespace UniGreenModules.UniGame.UnityBuild.Editor.ClientBuild.Commands
{
    using BuildConfiguration;
    using UnityEditor;

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


        public static void ExecuteBuild(string outputFileName,BuildTarget buildTarget,BuildTargetGroup targetGroup)
        {
            var argumentsProvider = new ArgumentsProvider(new[] {
                $"{BuildArguments.BuildOutputFolderKey}:Builds",
                $"{BuildArguments.BuildOutputNameKey}:{outputFileName}",
            });

            var buildConfiguration = new EditorBuildConfiguration(
                argumentsProvider, 
                new BuildParameters(buildTarget, targetGroup, argumentsProvider));
            
            UnityBuildTool.BuildPlayer(buildConfiguration);
        }
    }
}
