using System.Collections.Generic;

namespace Assets.Tools.UnityTools.AssetBundleManager.Interfaces
{
    public interface IBundleRequestFactory {

        IAssetBundleRequest Create(string bundleName, AssetBundleSourceType sourceType);

        IAssetBundleRequest Create(string bundleName, List<string> dependencies, AssetBundleSourceType sourceType);

    }

}
