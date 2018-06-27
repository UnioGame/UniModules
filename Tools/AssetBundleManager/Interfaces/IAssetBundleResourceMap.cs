using AssetBundlesModule;

namespace AssetBundlesModule
{
    public interface IAssetBundleResourceMap {

        bool Add(string assetBundlename,IAssetBundleResource bundleResource);
        bool Unload(string assetBundleName,bool force);

        IAssetBundleResource Get(string bundleName);

    }
}