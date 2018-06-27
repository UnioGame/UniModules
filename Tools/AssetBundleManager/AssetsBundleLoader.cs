using System.Collections.Generic;
using System.Diagnostics;
using Assets.Tools.Utils;
using Assets.Scripts.ProfilerTools;
using UnityEngine;

namespace AssetBundlesModule
{
    public class AssetsBundleLoader : IAssetsBundleLoader {

        private readonly IAssetBundlesRequestCache _assetBundlesRequestCache;
        private readonly IBundleRequestFactory _bundleRequestFactory;
        private readonly string _manifestName;
        private readonly bool _simulateMode = false;
        private readonly Dictionary<string, List<string>> _dependencies;
        private readonly Dictionary<string, IAssetBundleRequest> _requests;
        
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

        public AssetsBundleLoader(IAssetBundlesRequestCache assetBundlesRequestCache, 
            IBundleRequestFactory requestFactory, string manifestName, bool simulateMode) {
            _assetBundlesRequestCache = assetBundlesRequestCache;
            _bundleRequestFactory = requestFactory;
            _manifestName = manifestName;
            _simulateMode = simulateMode;
            _dependencies = new Dictionary<string, List<string>>();
            _requests = new Dictionary<string, IAssetBundleRequest>();
        }

        #endregion

        #region public methods

        // Load AssetResource and its dependencies.
        public IAssetBundleRequest GetAssetBundleRequest(string assetBundleName,AssetBundleSourceType sourceType,bool loadDependencies = true)
        {

            IAssetBundleRequest request = null;
            if (_requests.TryGetValue(assetBundleName, out request)) {
                return request;
            }
            
            var dependencies = loadDependencies
                ? GetBundleDependencies(assetBundleName)
                : ClassPool.Spawn<List<string>>();

            request = CreateBundleRequestWithDependencies(assetBundleName, dependencies, sourceType);

            _requests.Add(assetBundleName,request);
            
            return request;

        }

        public void UnloadDependencies(string assetBundleName, bool forceUnload = false) {

            var dependencies = GetBundleDependencies(assetBundleName);

            foreach (var dependency in dependencies)
            {
                UnloadAssetBundleInternal(dependency, forceUnload);
            }
        }

        public void UnloadAssetBundleInternal(string assetBundleName, bool forceUnload = false)
        {
            _assetBundlesRequestCache.Unload(assetBundleName,forceUnload);
        }
        
        public List<string> GetBundleDependencies(string assetBundleName) {

            List<string> dependencies = null;
            if (_dependencies.TryGetValue(assetBundleName,out dependencies)) {
                return dependencies;
            }
            
            dependencies = ClassPool.Spawn<List<string>>();

            if (_simulateMode) {
#if UNITY_EDITOR
                dependencies.AddRange(UnityEditor.AssetDatabase.GetAssetBundleDependencies(assetBundleName, true));
#endif
            }
            else {
                dependencies.AddRange(AssetBundleManifest.GetAllDependencies(assetBundleName));
            }

            _dependencies[assetBundleName] = dependencies;
            return dependencies;
        }

        #endregion

        #region private methods

        private IAssetBundleRequest CreateBundleRequestWithDependencies(string targetBundle, List<string> dependencies, AssetBundleSourceType sourceType)
        {
            var targetRequest = GetBundleRequest(targetBundle, sourceType);

            var dependenciesReqiests = ClassPool.Spawn<List<IAssetBundleRequest>>();
            //put all requests to bundles into cache
            for (var i = 0; i < dependencies.Count; i++)
            {
                var request = GetBundleRequest(dependencies[i], sourceType);
                dependenciesReqiests.Add(request);
            }

            var aggregateRequest = _bundleRequestFactory.Create(targetRequest, dependenciesReqiests, sourceType);

            LogLoadingBundle(targetBundle, dependencies, sourceType);

            return aggregateRequest;
        }

        [Conditional("LOGS_ENABLED")]
        private void LogLoadingBundle(string targetBundle, List<string> dependencies,AssetBundleSourceType sourceType) {
           GameLog.Log(string.Format("LOAD BUNDLE [{2}]: [{0}] \n{1}", targetBundle,string.Join("\n", dependencies.ToArray()), sourceType));
        }
        
        private IAssetBundleRequest GetBundleRequest(string assetBundleName,AssetBundleSourceType sourceType) {
            
            var bundleRequest = _assetBundlesRequestCache.Get(assetBundleName);
            if (bundleRequest != null)
                return bundleRequest;

            bundleRequest = _bundleRequestFactory.Create(assetBundleName, sourceType);
            var validation = _assetBundlesRequestCache.Add(assetBundleName, sourceType, bundleRequest);

            if (validation == false) {
                GameLog.LogErrorFormat("Add bundleRequest [{0}] to Chache error",assetBundleName);
            } 
                
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
