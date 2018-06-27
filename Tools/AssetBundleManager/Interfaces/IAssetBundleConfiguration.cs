using Assets.Scripts.Tools.AssetBundleManager;

namespace AssetBundlesModule {

    public interface IAssetBundleConfiguration {
        
        bool SimulateMode { get; }
        string ManifestName { get; }
        string BaseUrl { get; }
        IAssetBundleResourceMap AssetBundleResourceMap { get; }
        IAssetsBundleLoader AssetsBundleLoader { get; }

    }

}