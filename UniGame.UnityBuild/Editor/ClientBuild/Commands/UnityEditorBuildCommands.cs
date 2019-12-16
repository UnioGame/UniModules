namespace UniGreenModules.UniGame.UnityBuild.Editor.ClientBuild.Commands
{
    using BuildConfiguration;
    using UnityEditor;

    public class UnityEditorBuildCommands 
    {
        [MenuItem("UnityPlayerBuild/Android")]
        public static void BuildAndroid()
        {
            var argumentsProvider = new ArgumentsProvider(new string[] {
                $"{BuildArguments.BuildOutputFolderKey}:Build",
                $"{BuildArguments.BuildOutputNameKey}:android.apk",
            });

            var buildConfiguration = new EditorBuildConfiguration(argumentsProvider, 
                new BuildParameters(BuildTarget.Android, BuildTargetGroup.Android, argumentsProvider));
            
            UnityBuildTool.BuildPlayer(buildConfiguration);
        }

        [MenuItem("UnityPlayerBuild/iOS Release")]
        public static void BuildIOS()
        {
            var argumentsProvider = new ArgumentsProvider(new string[] {
                $"{BuildArguments.BuildOutputFolderKey}:Build",
            });
            
            
            var buildConfiguration = new EditorBuildConfiguration(argumentsProvider, 
                new BuildParameters(BuildTarget.iOS, BuildTargetGroup.iOS, argumentsProvider));

            
            UnityBuildTool.BuildPlayer(buildConfiguration);
        }

    }
}
