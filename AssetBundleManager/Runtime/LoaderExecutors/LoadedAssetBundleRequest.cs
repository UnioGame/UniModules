using UniModule.UnityTools.AssetBundleManager.AssetBundleResources;

namespace UniModule.UnityTools.AssetBundleManager.LoaderExecutors
{
    public class LoadedAssetBundleRequest : AssetBundleRequest
    {

        public LoadedAssetBundleRequest(IAssetBundleResource bundleResource) : base() {

            BundleResource = bundleResource;
            BundleName = bundleResource.BundleName;

        }

    }
}
