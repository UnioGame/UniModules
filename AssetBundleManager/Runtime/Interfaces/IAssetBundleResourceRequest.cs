using UniModule.UnityTools.AssetBundleManager.AssetBundleResources;

namespace UniModule.UnityTools.AssetBundleManager.Interfaces
{
    using UniGreenModules.UniCore.Runtime.Interfaces;

    public interface IAssetBundleResourceRequest : IAsyncOperation
    {
        
        IAssetBundleResource Resource { get; }

        string ResourceName { get; }
        
    }
}