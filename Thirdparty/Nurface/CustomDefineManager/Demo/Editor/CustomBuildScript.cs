using UnityEditor;
using System.Diagnostics;
using CustomDefineManagement;

public class ScriptBatch {
    // This will enable a menu item "MyTools/Build GoogleVR" which enables/disables directives upon build:
    // Un-comment to use.
    /*
    [MenuItem("MyTools/Build GoogleVR")]
    public static void BuildGoogleVR() {

        CustomDefineManager.DisableDirective("VR_GEARVR");
        CustomDefineManager.EnableDirective("VR_GOOGLEVR");
        
        // Get filename.
        string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
        string[] levels = new string[] { "Assets/SceneTest.unity" };

        // Build player.
        BuildPipeline.BuildPlayer(levels, path + "/GoogleVR.apk", BuildTarget.Android, BuildOptions.None);

    }

    [MenuItem("MyTools/Build GearVR")]
    public static void BuildGearVR() {

        CustomDefineManager.DisableDirective("VR_GOOGLEVR");
        CustomDefineManager.EnableDirective("VR_GEARVR");

        // Get filename.
        string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
        string[] levels = new string[] { "Assets/SceneTest.unity" };

        // Build player.
        BuildPipeline.BuildPlayer(levels, path + "/GearVR.apk", BuildTarget.Android, BuildOptions.None);

    }
    */
}