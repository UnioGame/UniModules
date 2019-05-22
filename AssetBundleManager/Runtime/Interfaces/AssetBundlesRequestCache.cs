using System.Collections.Generic;
using System.Linq;

namespace UniModule.UnityTools.AssetBundleManager.Interfaces
{
    using UniGreenModules.UniCore.Runtime.ObjectPool;

    public class AssetBundlesRequestCache : IAssetBundlesRequestCache {

        protected Dictionary<string, RequestCacheItem> LoadedRequests =
            new Dictionary<string, RequestCacheItem>();

        public List<string> CachedNames {
            get { return LoadedRequests.Keys.ToList(); }
        }

        #region public methods

        public IAssetBundleRequest Get(string assetBundleName) {

            RequestCacheItem bundle;
            if (LoadedRequests.TryGetValue(assetBundleName, out bundle)) {
                UpdateReferences(bundle,1);
                return bundle.Request;
            }

            return null;
        }

        public bool Add(string assetBundleName,AssetBundleSourceType assetBundleSourceType, IAssetBundleRequest assetBundleRequest) {

            RequestCacheItem item;
            if (LoadedRequests.TryGetValue(assetBundleName,out item))
                return false;
            
            var bundleCacheItem = ClassPool.Spawn<RequestCacheItem>();
            bundleCacheItem.Name = assetBundleName;
            bundleCacheItem.SourceType = assetBundleSourceType;
            bundleCacheItem.Request = assetBundleRequest;
            bundleCacheItem.References = 1;
            
            LoadedRequests[assetBundleName] = bundleCacheItem;
            
            return true;
        }

        public bool Remove(string assetBundleName) {

            RequestCacheItem bundle;
            if (LoadedRequests.TryGetValue(assetBundleName, out bundle)) {
                
                UpdateReferences(bundle,-1);
                if (bundle.References <= 0) {
                    LoadedRequests.Remove(assetBundleName);
                    var request = bundle.Request;
                    request.Despawn();
                }

            }

            return false;
        }

        #endregion

        private void UpdateReferences(RequestCacheItem item, int delta) {
            item.References += delta;
        }
        
    }

}