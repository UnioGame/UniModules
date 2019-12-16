namespace UniGreenModules.AssetBundleManager.Runtime.Interfaces
{
    using AssetBundleResources;
    using UniCore.Runtime.Interfaces;

    public interface IAssetBundleResourceRequest : IAsyncOperation
    {
        
        IAssetBundleResource Resource { get; }

        string ResourceName { get; }
        
    }
}