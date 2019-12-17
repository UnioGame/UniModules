using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UniGreenModules.AssetBundleManager.Editor.AssetBundleManagerRuntimeWindow
{
    using Runtime.Utils;
    using UniCore.EditorTools.Editor;

    public class AssetBundleManagerRuntimeWindow : EditorWindow {

        private static AssetBundleManagerRuntimeWindow _window;

        private Vector2             _scroll = new Vector2();
        private List<BundleSetInfo> _bundleInfos;
        private List<BundleSetInfo> _selectedForComparing;
        private string              _saveTitle = "";
        private bool                _showCurrentBundleInfo;
        private BundleSetInfo       _currentInfo;

        [MenuItem("Tools/AssetBundles/Runtime View")]
        static void Initialize()
        {
            // Get existing open window or if none, make a new one:
            _window = (AssetBundleManagerRuntimeWindow)EditorWindow.GetWindow(typeof(AssetBundleManagerRuntimeWindow));
            _window.Show();
        }

        void OnGUI() {

            if (Application.isPlaying == false) {
                EditorGUILayout.LabelField("Enter in Play mode");
                return;
            }

            InitializeWindow();

            _bundleInfos = AssetBundleInfo.GetHistory();

            GUILayout.BeginVertical();

            _scroll = EditorGUILayout.BeginScrollView(_scroll);

            _saveTitle = GUILayout.TextField(_saveTitle);
            if (GUILayout.Button("save current")) {
                AssetBundleInfo.SaveActiveState(_saveTitle);
            }
            if (GUILayout.Button("save current with size"))
            {
                AssetBundleInfo.SaveActiveState(_saveTitle,true);
            }
            if (GUILayout.Button("save to file")) {
                AssetBundleInfo.SaveToFile();
            }
            if (GUILayout.Button("unload all"))
            {
                AssetBundleInfo.UnloadAndClean();
            }
            GUILayout.Space(5);

            DrawComparisonElement();

            GUILayout.Space(5);

            _showCurrentBundleInfo = EditorGUILayout.Toggle("show current state", _showCurrentBundleInfo);
            if (_showCurrentBundleInfo) {
                var active = AssetBundleInfo.GetActiveInfo();
                _currentInfo.Bundles = active.Bundles;
                DrawViewer(_currentInfo);
            }

            for (int i = 0; i < _bundleInfos.Count; i++) {
                var info = _bundleInfos[i];
                DrawViewer(info);
            }

            EditorGUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private void InitializeWindow() {
            if (_bundleInfos == null) {
                _bundleInfos          = new List<BundleSetInfo>();
                _selectedForComparing = new List<BundleSetInfo>();
                _currentInfo = new BundleSetInfo() {
                    Bundles = new List<BundleItemInfo>(),
                    Title   = "Active List"
                };
            }
        }

        private const string _bundleTitleFormat = "{0} [{1}]";

        private void DrawViewer(BundleSetInfo info) {

            GUILayout.BeginVertical();

            var bundleGroupTitle = string.Format("{0} [{1}]", info.Title, info.SizeText);
            info.ShowBundleList = EditorGUILayout.Foldout(info.ShowBundleList, bundleGroupTitle);

            if (info.ShowBundleList) {
                EditorGUI.indentLevel++;
                for (int i = 0; i < info.Bundles.Count; i++) {
                    var resource = info.Bundles[i];

                    EditorGUILayout.BeginVertical();

                    var bundleTitle = string.Format(_bundleTitleFormat, resource.Name, resource.SizeValue);
                    resource.ShowAssets = EditorGUILayout.Foldout(resource.ShowAssets, bundleTitle);
                    if (resource.ShowAssets) {

                        EditorGUI.indentLevel++;

                        var names = resource.AssetsNames;
                        for (int j = 0; j < names.Count; j++) {
                            var assetName = names[j];
                            assetName = string.Format(_bundleTitleFormat, 
                                assetName, resource.AssetsSizeTexts[j]);
                            var asset = resource.AssetsStatuses[j];

                            EditorGUILayout.LabelField(assetName);
                            EditorGUILayout.LabelField(asset);
                        }

                        EditorGUI.indentLevel--;

                    }

                    EditorGUILayout.EndVertical();

                }
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Separator();
            GUILayout.Space(5);
        
            GUILayout.EndVertical();
        }

        private Vector2 _comparisonScroll;
        private void DrawComparisonElement() {


            EditorGUILayout.BeginVertical();
            _comparisonScroll = EditorGUILayout.BeginScrollView(_comparisonScroll,GUILayout.MaxHeight(50));

            for (int i = 0; i < _bundleInfos.Count; i++) {
                var info         = _bundleInfos[i];
                var selection    = _selectedForComparing.Contains(info);
                var newSelection = EditorGUILayout.Toggle(info.Title, selection);
                if (newSelection != selection) {
                    if (newSelection) {
                        _selectedForComparing.Add(info);
                    }
                    else {
                        _selectedForComparing.Remove(info);
                    }
                }
            }
        
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            if (GUILayout.Button("compare"))
            {
                if (_selectedForComparing.Count >= 2) {
                    Compare(_selectedForComparing[0], _selectedForComparing[1]);
                }
            }

        }

        private void Compare(BundleSetInfo firstSetInfo, BundleSetInfo secondSetInfo) {

            CompareWindow.Show(firstSetInfo.Bundles.Select(x => x.Name).ToArray(), secondSetInfo.Bundles.Select(x => x.Name).ToArray());

        }
    }
}
