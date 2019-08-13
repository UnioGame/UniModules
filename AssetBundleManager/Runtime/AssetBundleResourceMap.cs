using System.Collections.Generic;
using System.Collections.ObjectModel;
using UniModule.UnityTools.AssetBundleManager.AssetBundleResources;
using UniModule.UnityTools.AssetBundleManager.Interfaces;

namespace UniModule.UnityTools.AssetBundleManager
{
    using UniGreenModules.UniCore.Runtime.ObjectPool;
    using UniGreenModules.UniCore.Runtime.ProfilerTools;

    public class AssetBundleResourceMap : IAssetBundleResourceMap {

        private readonly Dictionary<string, AssetBundleResourceModel> _cacheMap;
        private readonly Dictionary<IAssetBundleResource, int> _bundleReferences;
        private const int _increaseDelta = 1;
        private const int _decreseDelta = -1;

        private List<string> _allNames;

        public ReadOnlyCollection<string> GetAllNames
        {
            get { return _allNames.AsReadOnly(); }
        }
        
        public AssetBundleResourceMap() {
            _cacheMap = new Dictionary<string, AssetBundleResourceModel>();
            _allNames = new List<string>();
            _bundleReferences = new Dictionary<IAssetBundleResource, int>();
        }

        public bool Add(AssetBundleResourceModel resourceModel) {

            if (AddResource(resourceModel) == false) return false;
            var resource = resourceModel.BundleResource;
            if (_bundleReferences.ContainsKey(resource) == false)
                _bundleReferences[resource] = 0;

            UpdateReferences(resourceModel.Dependecies, _increaseDelta);
            
            return true;
        }

        public IAssetBundleResource Get(string bundleName) {
            AssetBundleResourceModel resource = null;
            _cacheMap.TryGetValue(bundleName, out resource);
            return resource == null ? null : resource.BundleResource;
        }

        public bool Unload(string assetBundleName, bool force,bool forceUnloadMode) {

            AssetBundleResourceModel resource = null;
            if (_cacheMap.TryGetValue(assetBundleName, out resource) == false) {
                return false;
            }

            var bundleResource = resource.BundleResource;
            var result = false;
            var references = _bundleReferences[bundleResource];
            if (force == true || references == 0) {
                result = bundleResource.Unload(force,forceUnloadMode);
            }

            if (result) {

                UpdateReferences(resource.Dependecies,_decreseDelta);
                UnloadDependencies(resource.Dependecies);
                CleanUpAssetBundleResource(resource.BundleResource);
            }
            return result;
        }

        private bool TryUnload(string assetBundleName) {
            AssetBundleResourceModel resource = null;
            if (_cacheMap.TryGetValue(assetBundleName, out resource) == false)
            {
                return false;
            }

            var bundleResource = resource.BundleResource;
            if (bundleResource.TryUnload(true)) {
                CleanUpAssetBundleResource(bundleResource);
                return true;
            }

            return false;
        }

        private bool AddResource(AssetBundleResourceModel resourceModel) {

            var resource = resourceModel.BundleResource;
            var assetBundleName = resource.BundleName;

            if (_cacheMap.ContainsKey(assetBundleName))
            {
                return false;
            }

            _cacheMap[assetBundleName] = resourceModel;
            _allNames.Add(assetBundleName);
            
            return true;
        }

        private void CleanUpAssetBundleResource(IAssetBundleResource resource) {

            _cacheMap.Remove(resource.BundleName);
            _allNames.Remove(resource.BundleName);
            _bundleReferences.Remove(resource);

            resource.Despawn();
        }

        private void UpdateReferences(List<IAssetBundleResource> assetBundleNames, int delta) {
            for (int i = 0; i < assetBundleNames.Count; i++) {
                var resource = assetBundleNames[i];
                UpdateReference(resource, delta);
            }
        }

        private void UpdateReference(IAssetBundleResource resource, int delta) {
            int referencies = 0;
            _bundleReferences.TryGetValue(resource, out referencies);
            _bundleReferences[resource] = System.Math.Max(referencies + delta, 0);
        }

        private void UnloadDependencies(List<IAssetBundleResource> assetBundleNames) {

            for (int i = 0; i < assetBundleNames.Count; i++) {
                var resource = assetBundleNames[i];
                var bundleName = resource.BundleName;
                var referencies = 0;
                if(_bundleReferences.TryGetValue(resource, out referencies) == false)
                    continue;
                if(referencies > 0)
                    continue;
                TryUnload(bundleName);
            }

        }
    }
}
