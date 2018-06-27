using System.Collections.Generic;

namespace AssetBundlesModule
{
    public interface IBundleRequestFactory {

        IAssetBundleRequest Create(string bundleName, AssetBundleSourceType sourceType);

        IAssetBundleRequest Create(IAssetBundleRequest bundleName, List<IAssetBundleRequest> dependencies, AssetBundleSourceType sourceType);

    }

}
