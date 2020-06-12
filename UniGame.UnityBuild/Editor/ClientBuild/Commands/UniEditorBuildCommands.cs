namespace UniGreenModules.UniGame.UnityBuild.Editor.ClientBuild.Commands
{
    using UniModules.UniGame.UnityBuild.Editor.ClientBuild.BuildConfiguration;
    using UnityEditor;

    public class UniEditorBuildCommands 
    {
        [MenuItem("UniBuild/Android")]
        public static void BuildAndroid()
        {
            UniBuildTool.ExecuteBuild("android.apk",BuildTarget.Android, BuildTargetGroup.Android);
        }

        //public static void Build(string guid) => UniBuildTool.BuildByConfigurationId("");

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
