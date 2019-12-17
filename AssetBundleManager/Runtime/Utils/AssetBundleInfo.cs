namespace UniGreenModules.AssetBundleManager.Runtime.Utils
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using UniCore.Runtime.ProfilerTools;
    using UnityEngine;

    public class BundleItemInfo {

        public string Name;
        public List<string> AssetsNames = new List<string>();
        public List<string> AssetsStatuses = new List<string>();
        public List<long> AssetsSizeBytes = new List<long>();
        public List<string> AssetsSizeTexts = new List<string>();
        public List<float> AssetsSize = new List<float>();
        public bool ShowAssets;
        public long Size;
        public string SizeValue;

    }

    public class BundleSetInfo
    {
        public string Title;
        public List<BundleItemInfo> Bundles = new List<BundleItemInfo>();
        public bool ShowBundleList;
        public long Size;
        public string SizeText;

    }

    public class AssetBundleInfo {
        
        private static List<BundleSetInfo> _bundleInfos = new List<BundleSetInfo>();
        private const string _assetInfoFormat = "\t{0} [{1}] ~~[{2:0.####}]\n";
        private const string _budnelInfoStringFormat = "{0} ~~~ [{1:0.####}]\n";
        private const string _sizeFormat = "{0:0.####} {1}";

        public static BundleSetInfo SaveActiveState(string tag,bool calculateSize = false) {
            
            var info = _bundleInfos.FirstOrDefault(x => x.Title == tag);

            if (info == null) {
                
                tag = string.IsNullOrEmpty(tag) ? string.Empty : tag;
                info = CreateInfo(tag, GetBundleItemInfos(calculateSize));
                _bundleInfos.Add(info);
            }

            return info;
        }

        public static string[] GetLoadedBundles() {
            return AssetBundle.GetAllLoadedAssetBundles().Select(x => x.name).ToArray();
        }   

        public static void SaveToFile() {

            var fileName = "bundle_runtime_log";
            var path = Application.persistentDataPath + @"\" + fileName + ".txt";
#if UNITY_EDITOR
            path = UnityEditor.EditorUtility.SaveFilePanel("Save Logs", "", fileName, "txt");
#endif
            try {
                File.WriteAllText(path, GetTextData());
            }
            catch (Exception e) {
                Debug.LogError(e);
            }
           
        }

        public static string GetTextData(bool showFullInfo = false) {

            var fileData = string.Empty;
            foreach (var runtimeBundleInfo in _bundleInfos) {

                fileData += ToText(runtimeBundleInfo, showFullInfo);
                fileData += "================================\n\n";

            }

            return fileData;

        }

        public static void UnloadAndClean() {

            var map = AssetBundleManager.Instance.Configuration.ResourceMap;
            var assetBundles = map.GetAllNames;

            for (int i = 0; i < assetBundles.Count; i++) {

                var assetBundleName = assetBundles[i];
                Debug.LogFormat("Unload asset bundle [{0}]", assetBundleName);
                AssetBundleManager.Instance.UnloadAssetBundle(assetBundleName,true,true);
            }

            CleanUpMemory();
        }

        public static string ToText(BundleSetInfo info, bool showAssets = false) {

            var infoData = info.Title + "\n\n";
            foreach (var bundle in info.Bundles) {

                infoData += string.Format(_budnelInfoStringFormat, bundle.Name,bundle.SizeValue);
                if (showAssets) {

                    for (int i = 0; i < bundle.AssetsNames.Count; i++) {
                        var assetName = bundle.AssetsNames[i];
                        var asset = bundle.AssetsStatuses[i];
                        var size = bundle.AssetsSizeTexts[i];

                        infoData += string.Format(_assetInfoFormat, assetName ,asset, size);
                    }

                }
            }

            return infoData;
        }

        public static BundleSetInfo GetActiveInfo(bool calculateSize = false) {

            return CreateInfo("current", GetBundleItemInfos(calculateSize));

        }

        public static List<BundleSetInfo> GetHistory() {
            return _bundleInfos;
        }

        private static List<BundleItemInfo> GetBundleItemInfos(bool calculateSize = false) {
            var result = new List<BundleItemInfo>();
            var map = AssetBundleManager.Instance.Configuration.ResourceMap;
            var names = map.GetAllNames;
            for (int i = 0; i < names.Count; i++) {
                var bundleName = names[i];
                var item = GetBundleItemInfo(bundleName, calculateSize);
                if(item == null)continue;
                result.Add(item);
            }

            return result;
        }

        private static void CleanUpMemory() {

            Resources.UnloadUnusedAssets();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            Resources.UnloadUnusedAssets();

        }

        private static BundleItemInfo GetBundleItemInfo(string assetBundleName, bool calculateSize = false) {

            var map = AssetBundleManager.Instance.Configuration.ResourceMap;
            var resource = map.Get(assetBundleName);

            if (resource == null) return null;

            var cache = resource.CachedAssets;
            var assetsNames = cache.Values.Select(x => x.ToString()).ToList();
            

            var sizes = new List<float>();
            var sizesValues = new List<string>();
            var totalSize = 0L;
            foreach (var cacheItem in cache) {
				var size = calculateSize ? 
					ProfilerUtils.GetMemorySize(cacheItem.Value) : 0L;
                var sizeText = GetSizeString(size);
                sizes.Add(size);
                sizesValues.Add(sizeText);
                totalSize += size;
            }

            var item = new BundleItemInfo() {
                Name = assetBundleName,
                AssetsNames = resource.CachedAssets.Keys.ToList(),
                AssetsStatuses = assetsNames,
                AssetsSize = sizes,
                AssetsSizeTexts = sizesValues,
                Size = totalSize,
                SizeValue = GetSizeString(totalSize),
            };
            return item;
        }

        private static string GetSizeString(long size) {

            var kbSize = size / 1024.0f;
            var mbSize = kbSize / 1024.0f;

            var useMb = mbSize > 1;
            var result = string.Format(_sizeFormat,
                useMb ? mbSize : kbSize,
                useMb ? "MB" : "KB");
            return result;
        }

        private static BundleSetInfo CreateInfo(string tag, List<BundleItemInfo> infos) {

            var size = infos.Sum(x => x.Size);

            var info = new BundleSetInfo()
            {
                Bundles = infos,
                Title = string.IsNullOrEmpty(tag) ? string.Empty : tag,
                ShowBundleList = false,
                Size =  size,
                SizeText = GetSizeString(size),
            };
            return info;
        }
    }
}
