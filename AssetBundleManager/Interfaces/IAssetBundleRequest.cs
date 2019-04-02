using UniModule.UnityTools.AssetBundleManager.AssetBundleResources;
using UniModule.UnityTools.Interfaces;

namespace UniModule.UnityTools.AssetBundleManager.Interfaces {

    public interface IAssetBundleRequest : IAsyncOperation {

        IAssetBundleResource BundleResource { get; }

        string Resource { get; }

        string BundleName { get; }

        void Initialize(string assetBundleName, string resource);

    }

}