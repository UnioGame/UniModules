namespace UniGreenModules.AssetBundleManager.Editor.AssetBundleReferenceViewer
{
    using UnityEditor;

    public class AssetBundleReferenceWindow : UnityEditor.EditorWindow {

        private static AssetBundleReferenceWindow _window;
        private static global::UniGreenModules.AssetBundleManager.Editor.AssetBundleReferenceViewer.AssetBundleReferenceViewer _viewer = new global::UniGreenModules.AssetBundleManager.Editor.AssetBundleReferenceViewer.AssetBundleReferenceViewer();

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
