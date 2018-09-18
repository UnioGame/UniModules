using Assets.Scripts.Tools.AssetBundleManager;

namespace AssetBundlesModule {

    public interface IAssetBundleConfiguration {
        
        bool SimulateMode { get; }
        string ManifestName { get; }
        string BaseUrl { get; }

        IAssetsBundleLoader AssetsBundleLoader { get; }
        IAssetBundleResourceMap ResourceMap { get; }
        IAssetBundlesRequestCache RequestCache { get; }

    }

}