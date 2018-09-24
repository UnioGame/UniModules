using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.UI.Windows.Tools.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class AssetBundlesTagBuilder : EditorWindow
{

    private string _assetBundlesPrefix;
    private List<AssetImporter> _selectedFolderImporters = new List<AssetImporter>();
    private List<string> _assetBundlePrefixes = new List<string>();
    private Vector2 _scrollPosition;
    private bool _usePathDirsNames = true;
    private bool _applyToAllAssets;
    private bool _addAssetNameToEnd = true;
    private bool _applyToSelectedOnly;

    private int _depth = 3;

    // Add menu named "My Window" to the Window menu
    [MenuItem("Tools/AssetBundles/Bundles Tags Builder")]
    static void Initialize()
    {
        // Get existing open window or if none, make a new one:
        AssetBundlesTagBuilder window = (AssetBundlesTagBuilder)EditorWindow.GetWindow(typeof(AssetBundlesTagBuilder));
        window.Show();
    }

    void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.Space();

        var updateTemplate = false;

        _selectedFolderImporters = AssetEditorTools.GetActiveAssets(false);
        if (_selectedFolderImporters.Count == 0)
        {
            EditorGUILayout.LabelField("No Folder selected");
            _assetBundlePrefixes.Clear();
        }
        if (_assetBundlePrefixes.Count < _selectedFolderImporters.Count)
        {
            UpdatePrefixes();
        }
        _assetBundlesPrefix = EditorGUILayout.TextField("Common prefix : ", _assetBundlesPrefix);
        var depth = EditorGUILayout.IntSlider("Template depth :", _depth,1,10);
        updateTemplate = depth != _depth;
        _depth = depth;

        _assetBundlesPrefix = string.IsNullOrEmpty(_assetBundlesPrefix) ? string.Empty : _assetBundlesPrefix.ToLower();

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

        var usePathsBuffer = _usePathDirsNames;
        _usePathDirsNames = EditorGUILayout.Toggle("Use Folders Names as template", _usePathDirsNames);
        _addAssetNameToEnd = EditorGUILayout.Toggle("Add Asset Name To End", _addAssetNameToEnd);
        _applyToSelectedOnly = EditorGUILayout.Toggle("Aply to Selected Only", _applyToSelectedOnly);
        _applyToAllAssets = EditorGUILayout.Toggle("Apply to All Assets", _applyToAllAssets);


        updateTemplate = GUILayout.Button("Update Prefixes") || updateTemplate;

        if (usePathsBuffer != _usePathDirsNames || updateTemplate)
        {
            UpdatePrefixes();
        }
        
        if (GUILayout.Button("Apply"))
        {
            Apply();
        }

        DrawSelected();
        
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    private void UpdatePrefixes()
    {
        for (int i = 0; i < _selectedFolderImporters.Count; i++)
        {
            var assetImporter = _selectedFolderImporters[i];
            var prefix = _usePathDirsNames
                ? AssetEditorTools.GetFoldersTemplateName(assetImporter.assetPath, _depth) : string.Empty;
            prefix = prefix.ToLower();
            if (string.IsNullOrEmpty(prefix)) {
                prefix = _assetBundlesPrefix;
            }
            else if (!string.IsNullOrEmpty(_assetBundlesPrefix))
            {
                prefix = string.Format("{0}-{1}", _assetBundlesPrefix, prefix);
            }
            if (_assetBundlePrefixes.Count > i)
            {
                _assetBundlePrefixes[i] = prefix;
            }
            else
            {
                _assetBundlePrefixes.Add(prefix);
            }
        }
        
    }

    private void Apply()
    {
        if (_selectedFolderImporters.Count == 0)
            return;
        for (int i = 0; i < _selectedFolderImporters.Count; i++)
        {
            var folder = _selectedFolderImporters[i];
            var path = folder.assetPath;
            var folderPrefix = _assetBundlePrefixes[i];
            if (File.Exists(path) || _applyToSelectedOnly) {
                ApplyTags(new string[]{path}, folderPrefix, _addAssetNameToEnd);
            }
            if(_applyToSelectedOnly)
                continue;
            ApplyToSubItems(folderPrefix, path, _addAssetNameToEnd, !_applyToAllAssets);
        }
    }

    public static void ApplyToSubItems(string prefix,string path,bool addAssetNameToTag, bool applyToFoldersOnly) {

        var targets = Directory.GetDirectories(path);

        ApplyTags(targets, prefix, addAssetNameToTag);

        if (applyToFoldersOnly) return;
        
        targets = Directory.GetFiles(path);
        if (targets == null || targets.Length == 0)
            return;
        ApplyTags(targets, prefix, addAssetNameToTag);
        
    }

    public static void ApplyTags(string[] targets, string prefix, bool addAssetNameToTag)
    {
        for (int j = 0; j < targets.Length; j++)
        {
            var path = targets[j];
            var childAsset = AssetImporter.GetAtPath(path);
            if (!childAsset)
            {
                Debug.LogErrorFormat("Asset at Path {0} is NULL", path);
                continue;
            }
            var tag = prefix;
            if (addAssetNameToTag) {
                var assetName = Path.GetFileNameWithoutExtension(path);
                assetName = string.IsNullOrEmpty(assetName) ? string.Empty : assetName.Replace(" ", "");
                if (string.IsNullOrEmpty(assetName) == false) {
                    tag = string.IsNullOrEmpty(prefix) ? assetName : string.Format("{0}-{1}", prefix, assetName);
                }
            }
            if (string.IsNullOrEmpty(tag))
                continue;
            childAsset.assetBundleName = tag;
            childAsset.SaveAndReimport();
        }
    }

    private void DrawSelected()
    {
        EditorGUILayout.Separator();
        EditorGUILayout.BeginVertical();
        for (int i = 0; i < _selectedFolderImporters.Count; i++)
        {
            var assetImporter = _selectedFolderImporters[i];

            _assetBundlePrefixes[i] = EditorGUILayout.TextField("Asset Bundle Prefix", _assetBundlePrefixes[i]);

            var asset = AssetDatabase.LoadAssetAtPath<Object>(assetImporter.assetPath);
            EditorGUILayout.ObjectField(new GUIContent("Selected Folder : "), asset, typeof(Object), false);
        }
        EditorGUILayout.EndVertical();
    }

}
