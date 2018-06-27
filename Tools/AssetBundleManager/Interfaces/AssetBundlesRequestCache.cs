using System.Collections.Generic;
using Assets.Tools.Utils;
using Assets.Scripts.ProfilerTools;

namespace AssetBundlesModule
{
    public class AssetBundlesRequestCache : IAssetBundlesRequestCache {

        protected Dictionary<string, AssetBundleCacheItem> LoadedAssetBundles =
            new Dictionary<string, AssetBundleCacheItem>();

        #region public methods

        public IAssetBundleRequest Get(string assetBundleName) {

            AssetBundleCacheItem bundle;
            if (LoadedAssetBundles.TryGetValue(assetBundleName, out bundle)) {
                UpdateReferencies(bundle,1);
                return bundle.Request;
            }

            return null;
        }

        public bool Add(string assetBundleName,AssetBundleSourceType assetBundleSourceType, IAssetBundleRequest assetBundleRequest) {

            AssetBundleCacheItem item;
            if (LoadedAssetBundles.TryGetValue(assetBundleName,out item))
                return false;
            
            var bundleCacheItem = ClassPool.Spawn<AssetBundleCacheItem>();
            bundleCacheItem.Name = assetBundleName;
            bundleCacheItem.SourceType = assetBundleSourceType;
            bundleCacheItem.Request = assetBundleRequest;
            bundleCacheItem.Referencies = 1;
            
            LoadedAssetBundles[assetBundleName] = bundleCacheItem;
            
            return true;
        }

        public void Unload(string assetBundleName, bool force) {

            AssetBundleCacheItem bundle;
            if (LoadedAssetBundles.TryGetValue(assetBundleName, out bundle)) {
                
                UpdateReferencies(bundle,-1);
                if (force || bundle.Referencies <= 0) {
                    LoadedAssetBundles.Remove(assetBundleName);
                    var request = bundle.Request;
                    request.BundleResource.Unload(force);
                }
            }

        }

        #endregion

        private void UpdateReferencies(AssetBundleCacheItem item, int delta) {
            item.Referencies += delta;
        }
        
    }

}