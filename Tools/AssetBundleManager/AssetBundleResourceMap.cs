using System.Collections.Generic;
using System.Collections.ObjectModel;
using Assets.Scripts.ProfilerTools;

namespace AssetBundlesModule
{
    public class AssetBundleResourceMap : IAssetBundleResourceMap {

        private readonly Dictionary<string, IAssetBundleResource> _cacheMap;
        private List<string> _allNames;

        public ReadOnlyCollection<string> GetAllNames
        {
            get { return _allNames.AsReadOnly(); }
        }

        public AssetBundleResourceMap() {
            _cacheMap = new Dictionary<string, IAssetBundleResource>();
            _allNames = new List<string>();
        }

        public bool Add(string assetBundleName,IAssetBundleResource bundleResource) {

            if (_cacheMap.ContainsKey(assetBundleName))
            {
                GameLog.LogErrorFormat("Bundle Resource with name [{0}] already exists", assetBundleName);
                return false;
            }

            _cacheMap[assetBundleName] = bundleResource;
            _allNames.Add(assetBundleName);
            return true;
        }

        public IAssetBundleResource Get(string bundleName) {
            IAssetBundleResource resource = null;
            _cacheMap.TryGetValue(bundleName, out resource);
            return resource;
        }

        public bool Unload(string assetBundleName, bool force) {

            IAssetBundleResource resource = null;
            if (_cacheMap.TryGetValue(assetBundleName, out resource) == false) {
                GameLog.LogWarningFormat("Bundle Map Unload NULL with name {0}", assetBundleName);
                return false;
            }

            var result = resource.Unload(force);
            if (result) {
                _cacheMap.Remove(assetBundleName);
                _allNames.Clear();
                _allNames.Remove(assetBundleName);
            }
            return result;
        }

        //public bool Unload(string assetBundleName, bool force)
        //{
        //    IAssetBundleResource resource = null;
        //    if (_cacheMap.TryGetValue(assetBundleName, out resource) == false)
        //    {
        //        GameLog.LogWarningFormat("Bundle Map Unload NULL with name {0}", assetBundleName);
        //        return false;
        //    }

        //    var result = resource.Unload(force);
        //    if (result)
        //    {
        //        _cacheMap.Remove(assetBundleName);
        //        _allNames.Clear();
        //        _allNames.Remove(assetBundleName);
        //    }
        //    return result;
        //}

    }
}
