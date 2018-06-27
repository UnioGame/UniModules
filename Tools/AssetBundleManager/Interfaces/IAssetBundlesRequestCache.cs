namespace AssetBundlesModule
{
    
    public interface IAssetBundlesRequestCache {

        IAssetBundleRequest Get(string assetBundleName);

        void Unload(string assetBundleName, bool force);

        bool Add(string assetBundleName,AssetBundleSourceType assetBundleSourceType, IAssetBundleRequest assetBundle);

    }

}