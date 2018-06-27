using System.Collections;
using AssetBundlesModule;

namespace Assets.Scripts.Tools.AssetBundleManager.LoaderExecutors
{
    public interface BundleLoadExecutor {

        IEnumerator LoadAssetBundleAsync(string assetBundleName, bool loadDependencies = true);

        LoadedAssetBundle LoadAssetBundle(string assetBundleName, bool loadDependencies = true);

    }
}
