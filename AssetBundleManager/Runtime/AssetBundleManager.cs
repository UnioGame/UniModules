using System;
using System.Collections;
using System.Collections.Generic;
using UniModule.UnityTools.AssetBundleManager.AssetBundleResources;
using UniModule.UnityTools.AssetBundleManager.Interfaces;
using UniModule.UnityTools.CoroutineTools;
using UniModule.UnityTools.UniPool.Scripts;
using UniModule.UnityTools.ProfilerTools;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniModule.UnityTools.AssetBundleManager
{
    public class AssetBundleManager : IAssetBundleManager
    {
        private const AssetBundleSourceType _defaultResourceType = AssetBundleSourceType.AsyncLocalFile;
        private readonly IAssetsBundleLoader _bundleLoader;
        private readonly IAssetBundleResourceMap _resourceMap;
        private AssetBundleManifest _assetBundleManifest;

        #region static data

        private static AssetBundleManager _this;

        public static AssetBundleManager Instance
        {
            get
            {
                if (_this == null)
                {
                    var configuration = CreateConfiguration();
                    _this = new AssetBundleManager(configuration);
                }
                return _this;
            }
        }

        private static AssetBundleConfiguration CreateConfiguration()
        {
            var configuration = new AssetBundleConfiguration("AssetBundles", string.Empty, AssetBundleConfiguration.IsSimulateMode);
            return configuration;
        }

        #endregion

        #region constructor

        public AssetBundleManager(AssetBundleConfiguration configuration)
        {

            Configuration = configuration;
            _bundleLoader = Configuration.AssetsBundleLoader;
            _resourceMap = Configuration.ResourceMap;

        }

        #endregion

        public bool IsSumulateMode
        {
            get { return Configuration.SimulateMode; }
        }

        private BundleGroupMap _bundleGroupMap;
        public BundleGroupMap BundleGroupMap {
            get {
                //todo fix it
                if (_bundleGroupMap == null) {
                    _bundleGroupMap = Resources.Load<BundleGroupMap>("BundleGroupMap");
                }
                return _bundleGroupMap;
            }
        }

        public IAssetBundleConfiguration Configuration { get; protected set; }

        public AssetBundleManifest AssetBundleManifest
        {
            get
            {
                if (_assetBundleManifest == null) {
                    _assetBundleManifest = _bundleLoader.AssetBundleManifest;
                }

                return _assetBundleManifest;
            }
        }

        #region public methods

        public IAssetBundleResource GetAssetBundleResource(string assetBundleResource) {

            IAssetBundleResource resource = null;

            var request = GetAssetBundleResourceAsync(assetBundleResource,AssetBundleSourceType.LocalFile, x => resource = x);
            request.WaitCoroutine();
            return resource;

        }
    
        public IEnumerator GetAssetBundleResourceAsync(string assetBundleName, AssetBundleSourceType sourceType,
            Action<IAssetBundleResource> resourceAction) {

            IAssetBundleResource resource = null;
            sourceType = IsSumulateMode ? AssetBundleSourceType.Simulation : sourceType;
            sourceType = !Application.isEditor && sourceType == AssetBundleSourceType.Simulation
                ? _defaultResourceType
                : sourceType;

            yield return MakeRequest(assetBundleName, sourceType, x => resource = x);
        
            if (resource == null)
            {
                Debug.LogErrorFormat("Null IAssetBundleResource with name [{0}]", assetBundleName);
                yield break;
            }
            if (resourceAction != null)
            {
                resourceAction(resource);
            }

        }
    
        // Unload assetbundle and its dependencies.
        public void UnloadAssetBundle(string assetBundleName, bool force = false,bool unloadMode = false)
        {
            // If we're in Editor simulation mode, we don't have to load the manifest assetBundle.
            if (IsSumulateMode)
                return;
            _resourceMap.Unload(assetBundleName,force,unloadMode);

        }
    
        #region sync operations


        public T LoadAsset<T>(string assetBundleName)
            where T : Object {

            var resource = GetBundleResource(assetBundleName);
            return resource.LoadAsset<T>();

        }

        public T LoadAsset<T>(string assetBundleName, string assetName)
            where T : Object
        {
            var resource = GetBundleResource(assetBundleName);
            return resource.LoadAsset<T>(assetName);
        }

        public List<Object> LoadAssetWithSubAssets(string assetBundleName,string assetName)
        {
            var resource = GetBundleResource(assetBundleName);
            return resource.LoadAssetWithSubAssets(assetName);
        }

        public List<T> LoadAssets<T>(string assetBundleName) where T : Object
        {
            var resource = GetBundleResource(assetBundleName);
            return resource.LoadAssets<T>();
        }


        #endregion

        #region async operations


        public IEnumerator LoadAssetAsync<T>(string assetBundleName, AssetBundleSourceType sourceType, Action<T> callback)
            where T : Object
        {
            yield return MakeAssetRequestAsync(assetBundleName, x => x.LoadAssetAsync<T>(callback), sourceType);
        }

        public IEnumerator LoadAssetAsync<T>(string assetBundleName, string assetName, AssetBundleSourceType sourceType, Action<T> callback)
            where T : Object
        {
            yield return MakeAssetRequestAsync(assetBundleName, x => x.LoadAssetAsync<T>(assetName, callback), sourceType);
        }

        public IEnumerator LoadAssetsAsync<T>(AssetBundleSourceType sourceType,Action<List<T>> callback) where T : Object
        {
            var bundleName = typeof(T).Name.ToLower();
            yield return LoadAssetsAsync<T>(bundleName, sourceType, callback);
        }

        public IEnumerator LoadAssetsAsync<T>(string assetBundleName, AssetBundleSourceType sourceType, Action<List<T>> callback ) where T : Object
        {
            yield return MakeAssetRequestAsync(assetBundleName, x => x.LoadAllAssetsAsync<T>(callback), sourceType);
        }

        public IEnumerator LoadAssetsAsync(Type type, string assetBundleName, AssetBundleSourceType sourceType, Action<List<Object>> callback)
        {
            yield return MakeAssetRequestAsync(assetBundleName, x => x.LoadAllAssetsAsync(type, callback), sourceType);
        }

        #endregion

        public void UnloadAllAssets(bool force = false)
        {
            Debug.LogError("UnloadAllAssets NotImplementedException");
        }

        #endregion

        #region private methtods

        private IAssetBundleResource GetBundleResource(string assetBundleName) {

            IAssetBundleResource resource = null;
            var mode = IsSumulateMode ? AssetBundleSourceType.Simulation : AssetBundleSourceType.LocalFile;
            var awaiter = MakeRequest(assetBundleName, mode, x => resource = x);

            GameProfiler.BeginSample("AssetBundleManager.GetBundleResourceExecute");
        
            awaiter.WaitCoroutine();

            GameProfiler.EndSample();

            return resource;

        }

        private IEnumerator MakeRequest(string assetBundleName, AssetBundleSourceType sourceType, Action<IAssetBundleResource> action) {


            var request = _bundleLoader.GetAssetBundleRequest(assetBundleName, sourceType);
        
            if (request.BundleResource == null)
            {
                var id = GameProfiler.BeginWatch(string.Format("GetAssetBundleResource {0} from {1} ",
                    assetBundleName, sourceType));

                var awaiter = request.Execute();
                yield return awaiter;

                GameProfiler.StopWatch(id);
            }

            var resource = request.BundleResource;
            request.Despawn();
            if (action != null)
                action(resource);
        }

        private IEnumerator MakeAssetRequestAsync(string assetBundleName, Func<IAssetBundleResource,IEnumerator> resourceAction, 
            AssetBundleSourceType sourceType) {

            IAssetBundleResource resource = null;

            yield return GetAssetBundleResourceAsync(assetBundleName, sourceType, x => resource = x);

            if(resourceAction == null || resource == null)
                yield break;
        
            yield return resourceAction(resource);
        }


        #endregion

    }
}

