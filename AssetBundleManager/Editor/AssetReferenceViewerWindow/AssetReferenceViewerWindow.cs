using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniGreenModules.AssetBundleManager.Editor.AssetReferenceViewerWindow
{
	public class AssetReferenceViewerWindow : EditorWindow
	{

		private Vector2 _scroll;
		private Object  _targetAsset;
		private string  _guid;

		private Object _assetGuidObject;

		// Add menu named "My Window" to the Window menu
		[MenuItem("Tools/GameData/Asset Info Viewer")]
		static void Initialize()
		{
			// Get existing open window or if none, make a new one:
			AssetReferenceViewerWindow window = (AssetReferenceViewerWindow)EditorWindow.GetWindow(typeof(AssetReferenceViewerWindow));
			window.Show();
		}

		void OnGUI() {
			Draw();
		}

		public void Draw()
		{
		

			_scroll = EditorGUILayout.BeginScrollView(_scroll);
			if (string.IsNullOrEmpty(_guid))
				_guid = string.Empty;
        
			GUILayout.BeginVertical();

			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal();

			var id = EditorGUILayout.TextField("Target GUID :",_guid).ToLowerInvariant();

			EditorGUILayout.EndHorizontal();

			if (string.Equals(id, _guid) == false) {
				_guid = id;
				UpdateGuidData(_guid);
			}

			_assetGuidObject = EditorGUILayout.ObjectField("Guid Asset", _assetGuidObject, typeof(Object), false);

        
			GUILayout.EndVertical();
			EditorGUILayout.EndScrollView();
		}

		private void UpdateGuidData(string guid) {

			var assetPath = AssetDatabase.GUIDToAssetPath(guid);
			_assetGuidObject = AssetDatabase.LoadAssetAtPath<Object>(assetPath);

		}

	}
}
