using UnityEditor;

namespace UniGreenModules.AssetBundleManager.Editor.DependencyViewer
{
	public class AssetBundleDependencyWindow : EditorWindow
	{

		private static AssetBundleDependencyViewer _viewer = new AssetBundleDependencyViewer();
	
		// Add menu named "My Window" to the Window menu
		[MenuItem("Tools/AssetBundles/Asset Dependencies Viewer")]
		static void Initialize()
		{
			// Get existing open window or if none, make a new one:
			AssetBundleDependencyWindow window = (AssetBundleDependencyWindow)EditorWindow.GetWindow(typeof(AssetBundleDependencyWindow));
			window.Show();
		}
	
		void OnGUI()
		{
			_viewer.Draw();
		}

	}
}
