using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UniGreenModules.AssetBundleManager.Editor.AssetBundlesOperations
{
    using UniCore.EditorTools.Editor.AssetOperations;

    public static class AssetBundlesEditorOperations
    {

        public static void RemoveBundlesTagsFromFolder()
        {
            SetBundlesTagsToFolders(null);
        }

        public static void SetBundlesTagsToFolders(string assetBundleTag, string assetBundleVariant = null)
        {
            var selectedDir = AssetEditorTools.GetActiveAssets();
            for (int i = 0; i < selectedDir.Count; i++)
            {
                var dir = selectedDir[i];
                ApplyBundleTag(dir.assetPath, assetBundleTag, assetBundleVariant);
            }
        }

        public static void SetAssetBundlesTag(string assetPath, string assetBundleTag, string assetBundleVariant = null)
        {
            var asset = AssetImporter.GetAtPath(assetPath);
            if (asset == null)
            {
                Debug.LogErrorFormat("Asset Importer is NULL {0}", assetPath);
                return;
            }
            asset.SetAssetBundleNameAndVariant(assetBundleTag, assetBundleVariant);

        }

        private static void ApplyBundleTag(string directoryPath, string assetBundleTag, string assetBundleVariant)
        {
            SetAssetBundlesTag(directoryPath, assetBundleTag, assetBundleVariant);
            var allFolderAssets = AssetDatabase.FindAssets("t:Object", new[] { directoryPath });
            for (int i = 0; i < allFolderAssets.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(allFolderAssets[i]);
                SetAssetBundlesTag(path, assetBundleTag, assetBundleVariant);
            }
            Debug.LogFormat("Apply bundle Tag {0} with variant {1} to {2} Assets", assetBundleTag, assetBundleVariant, allFolderAssets.Length);
        }

        public static Dictionary<string, HashSet<Object>> FindAllReferencesToAnotherBundle(Object asset, bool excludesourceAssetBundle = true)
        {
            var bundleReferencies = new Dictionary<string, HashSet<Object>>();
            if (asset == false) return bundleReferencies;
            var sourceBundleName = GetBundleName(asset);
            var root             = new[] { asset };
            var refs             = EditorUtility.CollectDependencies(root);
            for (var i = 0; i < refs.Length; i++)
            {
                var assetItem = refs[i];
                if (!assetItem) continue;
                var refAssetBundle = GetBundleName(assetItem);
                if (string.IsNullOrEmpty(refAssetBundle)) continue;
                if (excludesourceAssetBundle && string.Equals(refAssetBundle, sourceBundleName)) {
                    continue;
                }
                if (!bundleReferencies.ContainsKey(refAssetBundle))
                {
                    bundleReferencies[refAssetBundle] = new HashSet<Object>();
                }
                bundleReferencies[refAssetBundle].Add(assetItem);
            }
            return bundleReferencies;
        }

        public static string GetBundleName(string asset)
        {
            return AssetDatabase.GetImplicitAssetBundleName(asset);
        }

        public static string GetBundleName(Object asset) {
            if (asset == false) return string.Empty;
            var path = AssetDatabase.GetAssetPath(asset);
            if (string.IsNullOrEmpty(path))
                return string.Empty;
            var bundleName = AssetDatabase.GetImplicitAssetBundleName(path);
            return bundleName;
        }

    }
}
