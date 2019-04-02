using System.Collections;
using UniModule.UnityTools.AssetBundleManager.AssetBundleResources;

namespace UniModule.UnityTools.AssetBundleManager.LoaderExecutors
{
    public interface BundleLoadExecutor {

        IEnumerator LoadAssetBundleAsync(string assetBundleName, bool loadDependencies = true);

        LoadedAssetBundle LoadAssetBundle(string assetBundleName, bool loadDependencies = true);

    }
}
