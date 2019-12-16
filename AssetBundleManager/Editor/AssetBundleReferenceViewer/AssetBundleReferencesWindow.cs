using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UniGreenModules.AssetBundleManager.Editor.AssetBundleReferenceViewer
{
	using UniCore.EditorTools.Editor.AssetOperations;

	public class AssetBundleReferencesWindow : EditorWindow {

		private static bool                             _hideWithZeroRefs = false;
		private static AssetBundleReferenceViewer       _viewer           = new AssetBundleReferenceViewer();
		private        Vector2                          _scroll           = new Vector2();
		private static List<Object>                     _assets           = new List<Object>();
		private static List<AssetBundleReferenceViewer> _assetsViewers    = new List<AssetBundleReferenceViewer>();
		private static AssetBundleReferencesWindow      _window;

		// Add menu named "My Window" to the Window menu
		[MenuItem("Tools/AssetBundles/Asset References Viewer")]
		static void Initialize()
		{
			// Get existing open window or if none, make a new one:
			_window = (AssetBundleReferencesWindow)EditorWindow.GetWindow(typeof(AssetBundleReferencesWindow));
			_window.Show();
		}
	
		void OnGUI()
		{
			EditorGUILayout.BeginVertical();
			if (GUILayout.Button("Clear"))
				_assetsViewers.Clear();
			_hideWithZeroRefs = EditorGUILayout.Toggle(_hideWithZeroRefs, "Hide Zero Referencies");
			_scroll           = EditorGUILayout.BeginScrollView(_scroll);
			_viewer.Draw();
			for (int i = 0; i < _assetsViewers.Count; i++) {
				var view = _assetsViewers[i];
				if (_hideWithZeroRefs && view.FoundReferences == 0)
					continue;
				EditorGUILayout.BeginHorizontal();
				view.Draw();
				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();
		}

		[MenuItem("Assets/Show AB References")]
		public static void ShowAssets() {
			var assets = AssetEditorTools.GetActiveAssets(false);
			_assets        = assets.Select(x => AssetDatabase.LoadAssetAtPath<Object>(x.assetPath)).ToList();
			_assetsViewers = new List<AssetBundleReferenceViewer>();
			for (int i = 0; i < _assets.Count; i++) {
				var viewer = new AssetBundleReferenceViewer();
				viewer.Initialize(_assets[i],false);
				viewer.Refresh();
				_assetsViewers.Add(viewer);
			}
			if(!_window) Initialize();
		}

	}
}
