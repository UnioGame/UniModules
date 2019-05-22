using UniModule.UnityTools.AssetBundleManager.AssetBundleResources;

namespace UniModule.UnityTools.AssetBundleManager.Interfaces {
    using UniGreenModules.UniCore.Runtime.Interfaces;

    public interface IAssetBundleRequest : IAsyncOperation {

        IAssetBundleResource BundleResource { get; }

        string Resource { get; }

        string BundleName { get; }

        void Initialize(string assetBundleName, string resource);

    }

}