using UnityEditor;

namespace Assets.UI.Windows.AddonsDefault.WindowSystemResources.Tools.AssetBundleManager.Editor.AssetBundleReferenceViewer
{

    public class AssetBundleReferenceWindow : UnityEditor.EditorWindow {

        private static AssetBundleReferenceWindow _window;
        private static global::AssetBundleReferenceViewer _viewer = new global::AssetBundleReferenceViewer();

        // Add menu named "My Window" to the Window menu
        [MenuItem("Tools/AssetBundles/Reference Viewer")]
        static void Initialize() {
            // Get existing open window or if none, make a new one:
            _window = (AssetBundleReferenceWindow) EditorWindow.GetWindow(typeof(AssetBundleReferenceWindow));
            _window.Show();

        }

        private void OnGUI() {
            EditorGUILayout.BeginVertical();
            _viewer.Draw();
            EditorGUILayout.EndVertical();
        }

    }

}
