using Assets.Tools.UnityTools.AssetBundleManager.AssetBundleResources;
using Assets.Tools.UnityTools.Interfaces;

namespace Assets.Tools.UnityTools.AssetBundleManager.Interfaces
{
    public interface IAssetBundleResourceRequest : IAsyncOperation
    {
        
        IAssetBundleResource Resource { get; }

        string ResourceName { get; }
        
    }
}