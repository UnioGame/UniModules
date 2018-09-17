using System.Collections.Generic;
using System.IO;
using System.Linq;
using AssetBundlesModule;
using Assets.UI.Windows.Tools.Editor;
using UnityEditor;
using UnityEngine;

public class BundleGroupMapWindow : EditorWindow {

    private static BundleGroupMapWindow _window;
    private BundleGroupMap _groupMap;
    private Vector2 _scroll = new Vector2();

    [MenuItem("Tools/AssetBundles/Bundle Group View")]
    static void Initialize() {

        var asset = AssetEditorTools.GetAssets<BundleGroupMap>().FirstOrDefault();
        // Get existing open window or if none, make a new one:
        _window = (BundleGroupMapWindow)EditorWindow.GetWindow(typeof(BundleGroupMapWindow));
        _window.Initialize(asset);
        _window.Show();

    }

    public void Initialize(BundleGroupMap groupMap) {
        _groupMap = groupMap;
    }

    void OnGUI() {

        InitializeWindow();

        GUILayout.BeginVertical();
        _scroll = EditorGUILayout.BeginScrollView(_scroll);
        
        var editor = Editor.CreateEditor(_groupMap);
        editor.OnInspectorGUI();

        EditorGUILayout.EndScrollView();
        GUILayout.EndVertical();
    }

    private void InitializeWindow() {

    }

    private void DrawViewer() {

        GUILayout.BeginVertical();

        GUILayout.EndVertical();
    }
}
