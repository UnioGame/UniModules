using System.Collections;
using Assets.Tools.UnityTools.AssetBundleManager.AssetBundleResources;

namespace Assets.Tools.UnityTools.AssetBundleManager.LoaderExecutors
{
    public interface BundleLoadExecutor {

        IEnumerator LoadAssetBundleAsync(string assetBundleName, bool loadDependencies = true);

        LoadedAssetBundle LoadAssetBundle(string assetBundleName, bool loadDependencies = true);

    }
}
