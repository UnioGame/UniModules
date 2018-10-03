using Assets.Tools.UnityTools.AssetBundleManager.AssetBundleResources;
using Assets.Tools.UnityTools.Interfaces;

namespace Assets.Tools.UnityTools.AssetBundleManager.Interfaces {

    public interface IAssetBundleRequest : IAsyncOperation {

        IAssetBundleResource BundleResource { get; }

        string Resource { get; }

        string BundleName { get; }

        void Initialize(string assetBundleName, string resource);

    }

}