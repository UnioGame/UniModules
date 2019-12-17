using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UniGreenModules.AssetBundleManager.Editor.DependencyViewer
{
    public class AssetBundleDependencyViewer
    {

        private        string                                        _filter;
        private        Vector2                                       _scrollPosition;
        private        float                                         _abItemWidth = 300;
        private static List<DependencyViewerModel>                   _bundleModels;
        private static Dictionary<string, BundleAssetsViewerControl> _bundleAssetsViewer = new Dictionary<string, BundleAssetsViewerControl>();
        private static string[]                                      _abNames;

        public void Draw()
        {

            EditorGUILayout.BeginVertical();

            if (_bundleModels == null || GUILayout.Button("Refresh"))
                Refresh();

            if (GUILayout.Button("Export for GraphViz[DOT]")) {
                Export();
            }

            _filter = EditorGUILayout.TextField("Search:", _filter);


            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            ShowAssetsBundles();

            EditorGUILayout.EndScrollView();

            EditorGUILayout.EndVertical();
        }

        public void Export() {
            var exporter = new BundlesGraphExporter();
            exporter.Export(_bundleModels);
        }

        private void ShowAssetsBundles()
        {

            for (var index = 0; index < _bundleModels.Count; index++) {

                var bundleModel = _bundleModels[index];

                if (Validate(bundleModel) == false) continue;

                EditorGUILayout.Separator();
                EditorGUILayout.BeginHorizontal();

                bundleModel.Enabled = GUILayout.Toggle(bundleModel.Enabled, bundleModel.BundleName, GUILayout.Width(_abItemWidth),
                    GUILayout.ExpandWidth(false));
                EditorGUILayout.LabelField(string.Format("[{0}]", bundleModel.Dependencies.Length));

                EditorGUILayout.EndHorizontal();
                if (bundleModel.Enabled)
                {
                    ShowAssetBundleOperations(bundleModel);
                }
            }
        }

        private bool Validate(DependencyViewerModel model)
        {

            if (string.IsNullOrEmpty(_filter)) return true;

            if (model.BundleName.IndexOf(_filter, StringComparison.OrdinalIgnoreCase) < 0)
            {
                return false;
            }

            return true;
        }

        private void CollectBundleInfo()
        {

            if (_bundleModels == null)
            {
                _bundleModels = new List<DependencyViewerModel>();
            }

            _abNames = AssetDatabase.GetAllAssetBundleNames();
            for (var index = 0; index < _abNames.Length; index++)
            {
                var abItem = _abNames[index];
                EditorUtility.DisplayProgressBar("Refresh Bundle Info", string.Format("Refreshing...{0} [{1}:{2}]", abItem, index, _abNames.Length), index / ((float)_abNames.Length));
                var model = new DependencyViewerModel();
                model.BundleName   = abItem;
                model.Dependencies = AssetDatabase.GetAssetBundleDependencies(abItem, false);
                _bundleModels.Add(model);
            }

            _bundleModels.Sort((x, y) => Comparer<int>.Default.Compare(y.Dependencies.Length, x.Dependencies.Length));

            for (var i = 0; i < _bundleModels.Count; i++) {
                var bundleModel = _bundleModels[i];
                _bundleAssetsViewer[bundleModel.BundleName] = new BundleAssetsViewerControl(bundleModel);
            }

            EditorUtility.ClearProgressBar();
        }

        private void Refresh()
        {

            foreach (var bundleAssetsViewerControl in _bundleAssetsViewer) {
                bundleAssetsViewerControl.Value.CleanUp();
            }

            if (_bundleAssetsViewer != null) {
                _bundleAssetsViewer.Clear();
            }

            if (_bundleModels != null) {
                _bundleModels.Clear();
            }

            CollectBundleInfo();
        }

        private void ShowAssetBundleOperations(DependencyViewerModel model)
        {
            EditorGUILayout.BeginHorizontal();

            GUILayout.Space(20);

            EditorGUILayout.BeginVertical();

            model.ShowDependency = GUILayout.Toggle(model.ShowDependency, "Show Dependencies",
                GUILayout.ExpandWidth(false));
            model.RecursiveDependency = GUILayout.Toggle(model.RecursiveDependency, "Include Recursive Dependencies",
                GUILayout.ExpandWidth(false));
            model.ShowAssets = GUILayout.Toggle(model.ShowAssets, "Show Assets", GUILayout.ExpandWidth(false));

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();

            if (model.ShowDependency)
            {
                EditorGUILayout.Separator();
                GUILayout.Label("Dependencies :");
                ShowAssetBundlesDependencies(model);
            }

            if (model.ShowAssets)
            {
                EditorGUILayout.Separator();
                GUILayout.Label("Assets : ");
                var assetViewer = _bundleAssetsViewer[model.BundleName];
                assetViewer.Draw();
            }
        }

        private void ShowAssetBundlesDependencies(DependencyViewerModel model)
        {

            var dependencies = model.Dependencies;
            if (dependencies.Length == 0) return;
            var color = GUI.backgroundColor;
            GUI.backgroundColor = Color.green;
            EditorGUILayout.BeginHorizontal();

            GUILayout.Space(20);
            EditorGUILayout.BeginVertical();

            for (int i = 0; i < dependencies.Length; i++)
            {
                var dependency = dependencies[i];
                GUILayout.Label(dependency);
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
            GUI.backgroundColor = color;
        }


    }
}