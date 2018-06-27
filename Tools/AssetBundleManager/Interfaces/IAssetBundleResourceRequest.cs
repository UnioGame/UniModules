using Tools.AsyncOperations;

namespace AssetBundlesModule
{
    public interface IAssetBundleResourceRequest : IAsyncOperation
    {
        
        IAssetBundleResource Resource { get; }

        string ResourceName { get; }
        
    }
}