using System.Collections.Generic;

namespace UniModule.UnityTools.AssetBundleManager.Interfaces
{
    
    public interface IAssetBundlesRequestCache {

        List<string> CachedNames { get; }

        IAssetBundleRequest Get(string assetBundleName);

        bool Remove(string assetBundleName);

        bool Add(string assetBundleName,AssetBundleSourceType assetBundleSourceType, IAssetBundleRequest assetBundle);

    }

}