namespace UniGreenModules.AssetBundleManager.Runtime.Interfaces
{
    using System.Collections.Generic;

    public interface IBundleRequestFactory {

        IAssetBundleRequest Create(string bundleName, AssetBundleSourceType sourceType);

        IAssetBundleRequest Create(string bundleName, List<string> dependencies, AssetBundleSourceType sourceType);

    }

}
