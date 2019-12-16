using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniGreenModules.AssetBundleManager.Editor.DependencyViewer
{
    using AssetBundleReferenceViewer;

    public class BundleAssetsViewerControl {

        private readonly DependencyViewerModel _model;

        private Vector2                                       _scroll;
        private List<string>                                  _assetsInBundleCache = new List<string>();
        private Dictionary<string, bool>                      _showAssetRefs       = new Dictionary<string, bool>();
        private Dictionary<string,Object>                     _assets              = new Dictionary<string, Object>();
        private Dictionary<string,AssetBundleReferenceViewer> _referenceViewers    = new Dictionary<string, AssetBundleReferenceViewer>();
        private bool                                          _initialized;

        public BundleAssetsViewerControl(DependencyViewerModel model) {
            _assets = new Dictionary<string, Object>();
            _model  = model;
            var assets =  AssetDatabase.GetAssetPathsFromAssetBundle(model.BundleName);
            _assetsInBundleCache = new List<string>(assets);
        }

        public void Draw() {

            if (_initialized == false) {
                Refresh();
                _initialized = true;
            }

            EditorGUI.indentLevel++;

            EditorGUILayout.BeginVertical(GUILayout.Height(400));

            _scroll = EditorGUILayout.BeginScrollView(_scroll);

            ShowAssets();

            EditorGUILayout.EndScrollView();

            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;
        }

        public void CleanUp() {
            _showAssetRefs.Clear();
            _assetsInBundleCache.Clear();
            _referenceViewers.Clear();
            _assets.Clear();
        }

        private void ShowAssets()
        {
            if (_assetsInBundleCache.Count == 0) return;

            EditorGUILayout.LabelField(string.Format("Dependency of [{0}] assets [{1}]",_model.BundleName, _assetsInBundleCache.Count));

            EditorGUI.indentLevel++;
            var color = GUI.backgroundColor;

            GUI.backgroundColor = Color.gray;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
        
            for (int i = 0; i < _assetsInBundleCache.Count; i++)
            {
                var asset = _assetsInBundleCache[i];
                ShowBudnleAsset(asset);
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel--;
            GUI.backgroundColor = color;
        }

        private void Refresh() {

            for (var i = 0; i < _assetsInBundleCache.Count; i++) {
                var assetPath = _assetsInBundleCache[i];
                EditorUtility.DisplayProgressBar("OnEnter AssetBundle Dependencies", "refreshing...",i / (float)_assetsInBundleCache.Count);
                var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
                _assets[assetPath]        = asset;
                _showAssetRefs[assetPath] = false;

                var viewer = new AssetBundleReferenceViewer();
                viewer.Initialize(asset, false);
                _referenceViewers[assetPath] = viewer;
                _showAssetRefs[assetPath]    = false;
            }
            EditorUtility.ClearProgressBar();
        }

        private void ShowBudnleAsset(string assetPath) {

            var targetAsset       = _assets[assetPath];
            var referenciesViewer = _referenceViewers[assetPath];

            if (referenciesViewer.FoundReferences <= 0) {
                _showAssetRefs[assetPath] = false;
                return;
            }

            var shown = _showAssetRefs[assetPath];

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical(GUILayout.Height(shown ? 300 : 40));

            EditorGUILayout.ObjectField(targetAsset.name, targetAsset, targetAsset.GetType(), false);

            EditorGUILayout.LabelField(string.Join(": ", referenciesViewer.BundleReferences));

            EditorGUILayout.Space();

            ShowAssetReferences(assetPath);
        
            EditorGUILayout.EndVertical();
        }

        private void ShowAssetReferences(string assetPath) {
            _showAssetRefs[assetPath] = EditorGUILayout.Foldout(_showAssetRefs[assetPath], assetPath);
            if (_showAssetRefs[assetPath])
            {
                var control = _referenceViewers[assetPath];
                if (control.FoundReferences > 0)
                {
                    EditorGUILayout.BeginVertical();
                    control.Draw();
                    EditorGUILayout.EndVertical();
                }
            }
        }
    }
}
