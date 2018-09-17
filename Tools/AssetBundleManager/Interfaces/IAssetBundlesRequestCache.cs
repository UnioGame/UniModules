using System.Collections.Generic;

namespace AssetBundlesModule
{
    
    public interface IAssetBundlesRequestCache {

        List<string> CachedNames { get; }

        IAssetBundleRequest Get(string assetBundleName);

        bool Remove(string assetBundleName);

        bool Add(string assetBundleName,AssetBundleSourceType assetBundleSourceType, IAssetBundleRequest assetBundle);

    }

}