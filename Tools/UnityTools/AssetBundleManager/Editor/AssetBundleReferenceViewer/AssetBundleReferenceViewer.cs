using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class AssetBundleReferenceViewer {

    private Vector2 _scroll;
    private Object _targetAsset;
	private bool _showAssetsOnBundle = true;
	private bool _excludeTargetBundle = true;
	private string _targetBundleName;
    private bool _showUpdateButton = true;
    private string _assetPath;
    private string _infoLabel;
    private string[] _linkedAssetBundleNames;
	
	private Dictionary<string, HashSet<Object>> _assetsBundleReferences = new Dictionary<string, HashSet<Object>>();
    private Dictionary<string,bool> _foldoutBundles = new Dictionary<string, bool>();

    public int FoundReferences {
        get { return _assetsBundleReferences.Count; }
    }

    public string[] BundleReferences {
        get { return _linkedAssetBundleNames; }
    }

    public void Initialize(Object target, bool showUpdateButton = true) {
        _targetAsset = target;
        _showUpdateButton = showUpdateButton;
        Refresh();
    }

	public void Draw()
	{
		GUILayout.BeginVertical();

        EditorGUILayout.Space();

		GUILayout.BeginHorizontal();

	    if (_targetAsset) {
	        _targetBundleName = AssetBundlesEditorOperations.GetBundleName(_targetAsset);
	    }
        EditorGUILayout.LabelField("Target Bundle: ", _targetBundleName);
		var asset = EditorGUILayout.ObjectField("Asset Object :", _targetAsset,typeof(Object),true,GUILayout.Width(400)); 
		
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		
		_showAssetsOnBundle = EditorGUILayout.Toggle("Show Bundle Assets :", _showAssetsOnBundle);
		_excludeTargetBundle = EditorGUILayout.Toggle("Exclude Target Bundle :", _excludeTargetBundle);

		GUILayout.EndHorizontal();
        EditorGUILayout.LabelField(_infoLabel);
        EditorGUILayout.Space();

	    if (asset != _targetAsset)
		{
			_assetsBundleReferences.Clear();
		}
		_targetAsset = asset;
		
		if (!_targetAsset)
		{
			GUILayout.Label("Empty target asset :(");
			return;
		}
		
		GUILayout.Space(10);
		
        if(_showUpdateButton) {
            if (GUILayout.Button("Find AssetBundle Referencies"))
                Refresh();
        }
	    _scroll = EditorGUILayout.BeginScrollView(_scroll, GUILayout.ExpandHeight(true));
        if (_assetsBundleReferences.Count > 0)
			ShowReferences(_assetsBundleReferences);
	    EditorGUILayout.EndScrollView();
        GUILayout.EndVertical();
	}

    public void Refresh() {
        FindAllReferences(_targetAsset);
        _linkedAssetBundleNames = _assetsBundleReferences.Select(x => x.Key).ToArray();
        _infoLabel = string.Format("Reference [{0}] : {1}", _assetsBundleReferences.Count,
            string.Join(": ", _linkedAssetBundleNames));
        foreach (var referenc in _assetsBundleReferences) {
            _foldoutBundles[referenc.Key] = false;
        }
    }

	private void FindAllReferences(Object asset) {
	    if (asset == false) {
            _assetsBundleReferences.Clear();
	        return;
	    }
	    _assetsBundleReferences = AssetBundlesEditorOperations.FindAllReferencesToAnotherBundle(asset, _excludeTargetBundle);
    }
	
	private void ShowReferences(Dictionary<string,HashSet<Object>> refs)
	{
		if (refs.Count == 0)
			return;

		EditorGUILayout.BeginHorizontal();
        GUILayout.Space(20);

		EditorGUILayout.BeginVertical();

		foreach (var assets in refs)
		{
            EditorGUILayout.Separator();
			EditorGUILayout.LabelField(assets.Key);
		    _foldoutBundles[assets.Key] = EditorGUILayout.Foldout(_foldoutBundles[assets.Key], assets.Key);
		    if (_foldoutBundles[assets.Key]) {
		        foreach (var asset in assets.Value) {
		            if (asset)
		                EditorGUILayout.ObjectField(asset.name, asset, asset.GetType(), false);
		        }
		    }
		}

		EditorGUILayout.EndVertical();
		
		EditorGUILayout.EndHorizontal();
	}


}
