using UnityEditor;

namespace UniGreenModules.UnityBuild.Editor.ClientBuild.Commands
{
    public class UnityEditorBuildCommands 
    {
        [MenuItem("UnityPlayerBuild/Android")]
        public static void BuildAndroid()
        {
            var argumentsProvider = new ArgumentsProvider(new string[] {
                $"{BuildArguments.BuildOutputFolderKey}:Build",
                $"{BuildArguments.BuildOutputNameKey}:android.apk",
            });
            UnityBuildTool.BuildPlayer(BuildTarget.Android,BuildTargetGroup.Android, argumentsProvider);
        }

        [MenuItem("UnityPlayerBuild/iOS Release")]
        public static void BuildIOS()
        {
            var argumentsProvider = new ArgumentsProvider(new string[] {
                $"{BuildArguments.BuildOutputFolderKey}:Build",
            });
            UnityBuildTool.BuildPlayer(BuildTarget.iOS,BuildTargetGroup.iOS, argumentsProvider);
        }

    }
}
