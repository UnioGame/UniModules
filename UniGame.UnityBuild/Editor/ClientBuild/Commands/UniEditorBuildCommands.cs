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
            UniBuildTool.ExecuteBuild("android.apk",BuildTarget.Android, BuildTargetGroup.Android);
        }

        [MenuItem("UniBuild/iOS")]
        public static void BuildIOS()
        {
            UniBuildTool.ExecuteBuild(string.Empty,BuildTarget.iOS, BuildTargetGroup.iOS);
        }
        
        [MenuItem("UniBuild/Windows")]
        public static void BuildWindows()
        {
            UniBuildTool.ExecuteBuild(string.Empty,BuildTarget.StandaloneWindows64, BuildTargetGroup.Standalone);
        }
        
    }
}
