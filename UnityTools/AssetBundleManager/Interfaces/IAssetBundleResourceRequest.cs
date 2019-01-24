using UniModule.UnityTools.AssetBundleManager.AssetBundleResources;
using UniModule.UnityTools.Interfaces;

namespace UniModule.UnityTools.AssetBundleManager.Interfaces
{
    public interface IAssetBundleResourceRequest : IAsyncOperation
    {
        
        IAssetBundleResource Resource { get; }

        string ResourceName { get; }
        
    }
}