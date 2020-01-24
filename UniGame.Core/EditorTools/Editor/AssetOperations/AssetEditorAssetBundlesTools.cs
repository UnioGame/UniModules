namespace UniGreenModules.UniCore.EditorTools.Editor.AssetOperations
{
    using System;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public partial class AssetEditorTools
    {

        public static void ApplyBundleToAsset<T>() where T : Object
        {
            ApplyBundleToAsset<T>(typeof(T).Name.ToLowerInvariant());
        }

        public static string GetValidBundleTag(string bundleTag)
        {
            if (string.IsNullOrEmpty(bundleTag)) return string.Empty;
            var parentName = bundleTag.Replace(".", "-");
            parentName = parentName.Replace(" ", "-");
            parentName = parentName.Replace("?", "");
            parentName = parentName.Replace("_", "-");
            return parentName;
        }

        public static void ApplyBundleToAssetWithTemplate(Type targetType, string bundleTag, string variant = null)
        {
            var assets       = GetAssetImporters(targetType);
            var useTemplates = IsContainsDuplicatedAssets(assets);
            ApplyBundleTagToAsset(assets, bundleTag, variant, useTemplates);
        }

        public static void ApplyBundleToAsset(Type targetType, string bundleTag, string variant = null, bool useTemplate = false)
        {
            var assets       = GetAssetImporters(targetType);
            var isDuplicated = IsContainsDuplicatedAssets(assets);
            if (isDuplicated) {
                Debug.LogWarningFormat("FOUND DUPLICATED items in {0} BUNDLE Tag", bundleTag);
            }

            ApplyBundleTagToAsset(assets, bundleTag, variant);
        }

        public static void ApplyBundleTagToAsset(List<AssetImporter> assets, string bundleTag, string variant = null,
            bool useTemplate = false, int depth = 4, int startDepth = 0)
        {
            for (var i = 0; i < assets.Count; i++) {
                EditorUtility.DisplayProgressBar("Apply Bundle Tag", string.Format("Process...[{0}:{1}]", i, assets.Count), i / (float) assets.Count);
                ApplyBundleTag(assets[i], bundleTag, variant, useTemplate, depth);
            }

            EditorUtility.ClearProgressBar();
        }

        public static string GetBundleName(string path)
        {
            if (string.IsNullOrEmpty(path) == true) return string.Empty;
            var abName = AssetDatabase.GetImplicitAssetBundleName(path);
            return string.IsNullOrEmpty(abName) ? string.Empty : abName;
        }

        public static void ApplyBundleTag(Object[] assets, string bundleTag, string variant = null, bool useTemplate = false, int depth = 4, int startDepth = 0)
        {
            var count = assets.Length;
            for (var i = 0; i < count; i++) {
                var asset = assets[i];
                if (asset == null)
                    continue;
                EditorUtility.DisplayProgressBar("Apply Screen Bundle Tag",
                    string.Format("Process...[{0}:{1}]", i, count), i / (float) count);

                var path     = AssetDatabase.GetAssetPath(asset);
                var importer = AssetImporter.GetAtPath(path);
                AssetEditorTools.ApplyBundleTag(importer, bundleTag, variant, useTemplate, depth, startDepth);
            }

            EditorUtility.ClearProgressBar();
        }

        public static bool IsInBundle(Object asset)
        {
            if (asset == null) return false;
            var path = AssetDatabase.GetAssetPath(asset);
            return IsInBundle(path);
        }

        public static bool IsInBundle(string path)
        {
            if (string.IsNullOrEmpty(path) == true) return false;
            var abName = AssetDatabase.GetImplicitAssetBundleName(path);
            return string.IsNullOrEmpty(abName) == false;
        }

        public static string ApplyBundleTag(string assetPath, string bundleTag, string variant = null, bool useTemplate = false, int depth = 4, int startDepth = 0)
        {
            if (string.IsNullOrEmpty(assetPath)) return string.Empty;
            var importer = AssetImporter.GetAtPath(assetPath);
            return ApplyBundleTag(importer, bundleTag, variant, useTemplate, depth, startDepth);
        }

        public static string ApplyBundleTag(AssetImporter importer, string bundleTag, string variant = null, bool useTemplate = false, int depth = 4, int startDepth = 0)
        {
            if (importer == null) return string.Empty;

            var assetBundleName = bundleTag;
            if (useTemplate) {
                var tag = (string.IsNullOrEmpty(bundleTag) ? string.Empty : bundleTag + "-");
                assetBundleName = tag + GetFoldersTemplateName(importer.assetPath, depth, false, startDepth);
            }

            assetBundleName = GetValidBundleTag(assetBundleName);
            var assetBundleVariant = GetValidBundleTag(variant);

            if (importer.assetBundleName == assetBundleName)
                return importer.assetBundleName;

            importer.SetAssetBundleNameAndVariant(assetBundleName, assetBundleVariant);

            return assetBundleName;
        }

        public static string GetBundleTag(string bundleTag, string assetPath = null, bool useTemplate = false, int depth = 4, int startDepth = 0)
        {
            var assetBundleName = bundleTag;
            if (string.IsNullOrEmpty(assetPath) == false && useTemplate) {
                var tag = (string.IsNullOrEmpty(bundleTag) ? string.Empty : bundleTag + "-");
                assetBundleName = tag + GetFoldersTemplateName(assetPath, depth, false, startDepth);
            }

            assetBundleName = GetValidBundleTag(assetBundleName);
            return assetBundleName;
        }

        public static void ApplyBundleToAsset(Type targetType, bool useTemplate = false)
        {
            if (useTemplate) {
                ApplyBundleToAssetWithTemplate(targetType, targetType.Name);
                return;
            }

            ApplyBundleToAsset(targetType, targetType.Name);
        }

        public static void ApplyBundleToAsset<T>(string bundleTag, string variant = null) where T : Object
        {
            ApplyBundleToAsset(typeof(T), bundleTag, variant);
        }

    }
}