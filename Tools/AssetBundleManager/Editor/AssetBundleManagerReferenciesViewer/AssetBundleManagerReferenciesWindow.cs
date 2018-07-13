using System;
using System.Collections;
using System.Collections.Generic;
using AssetBundlesModule;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class AssetBundleProviderInfo {

    public string Name;
    public IAssetBundleResource AssetBundleResource;
    public bool ShowAssets;
    
    public int AssetsCount {
        get { return AssetBundleResource == null ? 0 : AssetBundleResource.AllAssetsNames.Length; }
    }
}

public class AssetBundleManagerReferenciesWindow : EditorWindow {

    private static AssetBundleManagerReferenciesWindow _window;
    private static Type AssetType = typeof(Object);

    private Vector2 _scroll = new Vector2();
    private Dictionary<string, AssetBundleProviderInfo> _providerInfos;

    private const string _bundleLableName = "{0} [{1}] :";
    private string[] _assetBundles;
 
    [MenuItem("Tools/GameData/Asset Bundle References")]
    static void Initialize()
    {
        // Get existing open window or if none, make a new one:
        _window = (AssetBundleManagerReferenciesWindow)EditorWindow.GetWindow(typeof(AssetBundleManagerReferenciesWindow));
        _window.Show();
    }

    void OnGUI() {

        InitializeWindow();

        GUILayout.BeginVertical();
        _scroll = EditorGUILayout.BeginScrollView(_scroll);

        EditorGUILayout.Toggle(AssetBundleManager.Instance.IsSumulateMode, "IsSumulateMode");

        DrawBundleList();

        EditorGUILayout.EndScrollView();
        GUILayout.EndVertical();
    }

    private void InitializeWindow() {

        if (_providerInfos == null)
            _providerInfos = new Dictionary<string, AssetBundleProviderInfo>();
        if (_assetBundles == null || _assetBundles.Length == 0) {
            _assetBundles = AssetDatabase.GetAllAssetBundleNames();
        }
    }

    private void DrawBundleList() {

        foreach (var assetProvider in _assetBundles) {

            var provider = UpdateBundleInfo(assetProvider);
            if (provider.AssetBundleResource == null)
                continue;
            DrawAssetProvider(provider);

        }

    }

    private void DrawAssetProvider(AssetBundleProviderInfo providerInfo) {

        GUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();

        var label = string.Format(_bundleLableName, providerInfo.Name, providerInfo.AssetsCount);

        EditorGUILayout.LabelField(label);
        providerInfo.ShowAssets = EditorGUILayout.Toggle(providerInfo.ShowAssets, "Show Assets");

        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Refresh Bundle Info")) {
            RefreshBundleInfo();
        }

        if (providerInfo.ShowAssets)
        {
            ShowBundleAssets(providerInfo);
        }

        GUILayout.EndVertical();
    }

    private void RefreshBundleInfo() {
        foreach (var assetBundle in _assetBundles) {
            UpdateBundleInfo(assetBundle);
        }
    }

    private AssetBundleProviderInfo UpdateBundleInfo(string bundleName) {

        AssetBundleProviderInfo providerInfo = null;

        if (!_providerInfos.TryGetValue(bundleName, out providerInfo)) {
            providerInfo = new AssetBundleProviderInfo() {
                Name = bundleName,
            };
            _providerInfos[bundleName] = providerInfo;
        }

        return providerInfo;
    }

    private void ShowBundleAssets(AssetBundleProviderInfo providerInfo) {

        var provider = providerInfo.AssetBundleResource;
        if(provider == null) return;
        
        if (provider == null)
        {
            GUILayout.Label(string.Format("bundle not loaded"));
            return;
        }

        GUILayout.BeginVertical();

        foreach (var asset in provider.CachedObjects) {

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.ObjectField(asset, AssetType, false);

            EditorGUILayout.EndHorizontal();

        }

        GUILayout.EndVertical();

    }
}
