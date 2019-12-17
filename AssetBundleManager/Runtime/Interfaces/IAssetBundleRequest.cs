namespace UniGreenModules.AssetBundleManager.Runtime.Interfaces {
    using AssetBundleResources;
    using UniCore.Runtime.Interfaces;

    public interface IAssetBundleRequest : IAsyncOperation {

        IAssetBundleResource BundleResource { get; }

        string Resource { get; }

        string BundleName { get; }

        void Initialize(string assetBundleName, string resource);

    }

}