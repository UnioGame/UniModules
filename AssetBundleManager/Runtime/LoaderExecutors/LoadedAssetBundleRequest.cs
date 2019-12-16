namespace UniGreenModules.AssetBundleManager.Runtime.LoaderExecutors
{
    using AssetBundleResources;

    public class LoadedAssetBundleRequest : AssetBundleRequest
    {

        public LoadedAssetBundleRequest(IAssetBundleResource bundleResource) : base() {

            BundleResource = bundleResource;
            BundleName = bundleResource.BundleName;

        }

    }
}
