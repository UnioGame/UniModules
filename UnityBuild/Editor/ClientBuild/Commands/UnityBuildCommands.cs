using Plavalaguna.Joy.Modules.UnityBuild;
using UnityEditor;

public class UnityBuildCommands 
{
    [MenuItem("UnityPlayerBuild/Android")]
    public static void BuildAndroid()
    {
        var argumentsProvider = new ArgumentsProvider(new string[] {
            $"{BuildArguments.BuildOutputFolderKey}:Build",
            $"{BuildArguments.BuildOutputNameKey}:android.apk",
        });
        UnityBuildTool.BuildPlayer(BuildTarget.Android, argumentsProvider);
    }

    [MenuItem("UnityPlayerBuild/iOS Release")]
    public static void BuildIOS()
    {
        var argumentsProvider = new ArgumentsProvider(new string[] {
            $"{BuildArguments.BuildOutputFolderKey}:Build",
        });
        UnityBuildTool.BuildPlayer(BuildTarget.iOS, argumentsProvider);
    }

}
