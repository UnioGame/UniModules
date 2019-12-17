namespace UniGreenModules.AssetBundleManager.Runtime
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Interfaces;
    using UniCore.Runtime.CoroutineTools;
    using UniCore.Runtime.ObjectPool.Runtime;
    using UniCore.Runtime.ProfilerTools;
    using UnityEngine;

    public class AssetsBundleLoader : IAssetsBundleLoader {

        private readonly IBundleRequestFactory _bundleRequestFactory;
        private readonly string _manifestName;
        private readonly bool _simulateMode = false;
        private readonly Dictionary<string, List<string>> _dependencies;

        private AssetBundleManifest _manifest;

        public AssetBundleManifest AssetBundleManifest
        {
            get
            {
                if (_manifest == null) {
                    _manifest = LoadManifest(_manifestName);
                }

                return _manifest;
            }
        }

        #region constructor

        public AssetsBundleLoader(
            IBundleRequestFactory requestFactory, string manifestName, bool simulateMode) {
            _bundleRequestFactory = requestFactory;
            _manifestName = manifestName;
            _simulateMode = simulateMode;
            _dependencies = new Dictionary<string, List<string>>();
        }

        #endregion

        #region public methods

        // Load AssetResource and its dependencies.
        public IAssetBundleRequest GetAssetBundleRequest(string assetBundleName,AssetBundleSourceType sourceType,bool loadDependencies = true) {

            IAssetBundleRequest request = null;

            var dependencies = loadDependencies
                ? GetBundleDependencies(assetBundleName)
                : ClassPool.Spawn<List<string>>();

            GameProfiler.BeginSample("AssetBundleLoader.CreateBundleRequestWithDependencies");

            request = CreateBundleRequestWithDependencies(assetBundleName, dependencies, sourceType);

            GameProfiler.EndSample();

            return request;

        }

        public List<string> GetBundleDependencies(string assetBundleName) {

            List<string> dependencies = null;
            if (_dependencies.TryGetValue(assetBundleName,out dependencies)) {
                return dependencies;
            }
            
            GameProfiler.BeginSample("AssetBundleLoader.GetBundleDependencies");

            dependencies = ClassPool.Spawn<List<string>>();

            if (_simulateMode == false) {
                dependencies.AddRange(AssetBundleManifest.GetAllDependencies(assetBundleName));
            }

            _dependencies[assetBundleName] = dependencies;

            GameProfiler.EndSample();

            return dependencies;
        }

        #endregion

        #region private methods

        private IAssetBundleRequest CreateBundleRequestWithDependencies(string targetBundle, List<string> dependencies, AssetBundleSourceType sourceType)
        {

            var aggregateRequest = _bundleRequestFactory.Create(targetBundle, dependencies, sourceType);

            LogLoadingBundle(targetBundle, dependencies, sourceType);

            return aggregateRequest;
        }

        [Conditional("LOGS_ENABLED")]
        private void LogLoadingBundle(string targetBundle, List<string> dependencies,AssetBundleSourceType sourceType) {
           GameLog.Log(string.Format("LOAD BUNDLE [{2}]: [{0}] \n{1}", targetBundle,string.Join("\n", dependencies.ToArray()), sourceType));
        }
        
        private IAssetBundleRequest GetBundleRequest(string assetBundleName,AssetBundleSourceType sourceType) {
            
            var bundleRequest = _bundleRequestFactory.Create(assetBundleName, sourceType);
            return bundleRequest;

        }

        private AssetBundleManifest LoadManifest(string assetBundleName)
        {

            var manifestBundleRequest = GetBundleRequest(assetBundleName, AssetBundleSourceType.LocalFile);
            manifestBundleRequest.Execute().WaitCoroutine();
            var manifestBundle = manifestBundleRequest.BundleResource;
            var manifest = manifestBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            return manifest;

        }


        #endregion
    }
}
