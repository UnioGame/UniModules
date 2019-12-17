namespace UniGreenModules.AssetBundleManager.Runtime.LoaderExecutors
{
    using System.Collections;
    using AssetBundleResources;

    public interface BundleLoadExecutor {

        IEnumerator LoadAssetBundleAsync(string assetBundleName, bool loadDependencies = true);

        LoadedAssetBundle LoadAssetBundle(string assetBundleName, bool loadDependencies = true);

    }
}
