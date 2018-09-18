using System.Collections.Generic;

namespace AssetBundlesModule
{
    public interface IBundleRequestFactory {

        IAssetBundleRequest Create(string bundleName, AssetBundleSourceType sourceType);

        IAssetBundleRequest Create(string bundleName, List<string> dependencies, AssetBundleSourceType sourceType);

    }

}
