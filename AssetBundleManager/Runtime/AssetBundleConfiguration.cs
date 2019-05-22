using UniModule.UnityTools.AssetBundleManager.Interfaces;

namespace UniModule.UnityTools.AssetBundleManager
{
    using UniGreenModules.UniCore.Runtime.Utils;

    public class AssetBundleConfiguration : IAssetBundleConfiguration {

        private const string WebRequestBundleModeKey = "WebRequestBundleMode";
        private const string SimulateAssetBundlesKey = "SimulateAssetBundles";
        private const string LazyBundleModeKey = "LazyBundleMode";
        
        private readonly IBundleRequestFactory _requestFactory;

        private static bool? WebRequestBundleMode;
        private static bool? SimulateAssetBundleMode;
        private static bool? LazyBundleMode;
        
        public AssetBundleConfiguration(string manifestName, string baseUrl,bool simulateMode,bool loadDependenciesAsync = true) {
            ManifestName = manifestName;
            BaseUrl = baseUrl;
            SimulateMode = simulateMode;
            LoadDependenciesAsync = loadDependenciesAsync;

            RequestCache = new AssetBundlesRequestCache();
            ResourceMap = new AssetBundleResourceMap();

            _requestFactory = new BundleRequestOperationFactory(ManifestName,LoadDependenciesAsync, ResourceMap, RequestCache);

            AssetsBundleLoader = CreateLoader(_requestFactory);
        }

        #region static properties

        // Flag to indicate if we want to simulate assetBundles in Editor without building them actually.
        public static bool IsSimulateMode
        {
            get
            {
                return EditorSettingsProperty.GetBool(SimulateAssetBundlesKey, ref SimulateAssetBundleMode);
            }
            set
            {
                EditorSettingsProperty.SetBool(SimulateAssetBundlesKey,ref SimulateAssetBundleMode,value);
            }
        }

        public static bool IsWebRequestMode
        {
            get
            {
                return EditorSettingsProperty.GetBool(WebRequestBundleModeKey, ref WebRequestBundleMode);
            }
            set
            {
                EditorSettingsProperty.SetBool(WebRequestBundleModeKey, ref WebRequestBundleMode, value);
            }
        }

        // Flag to indicate if we want to simulate assetBundles in Editor without building them actually.
        public static bool IsLazyMode
        {
            get
            {
                return EditorSettingsProperty.GetBool(LazyBundleModeKey, ref LazyBundleMode);
            }
            set
            {
                EditorSettingsProperty.SetBool(LazyBundleModeKey,ref LazyBundleMode,value);
            }
        }

        #endregion

        public bool LoadDependenciesAsync { get; protected set; }
        public bool SimulateMode { get; protected set; }
        public string ManifestName { get; protected set; }
        public string BaseUrl { get; protected set; }
        public IAssetBundlesRequestCache RequestCache { get; protected set; }
        public IAssetsBundleLoader AssetsBundleLoader { get; protected set; }
        public IAssetBundleResourceMap ResourceMap { get; protected set; }

        #region private methods

        private IAssetsBundleLoader CreateLoader(IBundleRequestFactory requestFactory) {

            var loader = new AssetsBundleLoader(requestFactory,ManifestName, IsSimulateMode);

            return loader;
        }

        #endregion
    }
}
