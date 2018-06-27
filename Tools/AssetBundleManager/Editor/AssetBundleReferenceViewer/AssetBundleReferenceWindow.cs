using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.UI.Windows.Tools.Editor;
using UnityEditor;
using UnityEngine;

public class AssetBundleReferenceWindow : EditorWindow {

    private static bool _hideWithZeroRefs = false;
	private static AssetBundleReferenceViewer _viewer = new AssetBundleReferenceViewer();
    private Vector2 _scroll = new Vector2();
    private static List<Object> _assets = new List<Object>();
    private static List<AssetBundleReferenceViewer> _assetsViewers = new List<AssetBundleReferenceViewer>();
    private static AssetBundleReferenceWindow _window;

    // Add menu named "My Window" to the Window menu
    [MenuItem("Tools/GameData/Asset Bundle References")]
	static void Initialize()
	{
        // Get existing open window or if none, make a new one:
	    _window = (AssetBundleReferenceWindow)EditorWindow.GetWindow(typeof(AssetBundleReferenceWindow));
	    _window.Show();
	}
	
	void OnGUI()
	{
	    GUILayout.BeginVertical();
	    if (GUILayout.Button("Clear"))
	        _assetsViewers.Clear();
	    _hideWithZeroRefs = GUILayout.Toggle(_hideWithZeroRefs, "Hide Zero Referencies");
        _scroll = EditorGUILayout.BeginScrollView(_scroll);
        _viewer.Draw();
        for (int i = 0; i < _assetsViewers.Count; i++) {
	        var view = _assetsViewers[i];
            if (_hideWithZeroRefs && view.FoundReferences == 0)
                continue;
            GUILayout.BeginHorizontal();
            view.Draw();
            GUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
	    GUILayout.EndVertical();
    }

    [MenuItem("Assets/Show AB References")]
    public static void ShowAssets() {
        var assets = AssetEditorTools.GetActiveAssets(false);
        _assets = assets.Select(x => AssetDatabase.LoadAssetAtPath<Object>(x.assetPath)).ToList();
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
