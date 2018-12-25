using Assets.Tools.UnityTools.AssetBundleManager.AssetBundleResources;

namespace Assets.Tools.UnityTools.AssetBundleManager.LoaderExecutors
{
    public class LoadedAssetBundleRequest : AssetBundleRequest
    {

        public LoadedAssetBundleRequest(IAssetBundleResource bundleResource) : base() {

            BundleResource = bundleResource;
            BundleName = bundleResource.BundleName;

        }

    }
}
