using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.ProfilerTools;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssetBundlesModule_Old{

// Class takes care of loading assetBundle and its dependencies automatically, loading variants automatically.
    public class AssetBundleManager : IAssetManager
    {
        #region static data
        
        private static AssetBundleManager _this;

        public static AssetBundleManager Instance
        {
            get
            {
                if (_this == null)
                    _this = new AssetBundleManager(AssetSimulationOperationProvider.IsSimulateMode);
                return _this;
            }
        }

        public static bool IsSumulateMode { get; protected set; }

        #endregion

        #region private properties

        private IAssetProvider _minifestAssetBundle;
        private string _assetsUrl = string.Empty;
        private Dictionary<string, string> _assetsPaths;
        private Dictionary<string, IAssetProvider> _assetProviders;

        #endregion

        #region constructor

        protected AssetBundleManager(bool simulateMode)
        {
            _assetsPaths = new Dictionary<string, string>();
            _assetProviders = new Dictionary<string, IAssetProvider>();
            IsSumulateMode = simulateMode;
            AssetsUrl = AssetsBundleLoader.AssetsPath;
        }

        #endregion
        
        public AssetBundleManifest AssetBundleManifest
        {
            get
            {
                if (AssetsBundleLoader.AssetBundleManifest == null) {
                    AssetsBundleLoader.LoadManifest();
                }
                return AssetsBundleLoader.AssetBundleManifest;
            }
        }

        public Dictionary<string, IAssetProvider> AssetProviders {
            get { return _assetProviders; }
        }

        // The base downloading url which is used to generate the full downloading url with the assetBundle names.
        public string AssetsUrl
        {
            get { return _assetsUrl; }
            set { _assetsUrl = value; }
        }

        #region public methods

        // Unload assetbundle and its dependencies.
        public void UnloadAssetBundle(string assetBundleName,bool force = false)
        {
            // If we're in Editor simulation mode, we don't have to load the manifest assetBundle.
            if (IsSumulateMode)
                return;
            var assetPath = GetAssetPath(assetBundleName);
            AssetsBundleLoader.UnloadAssetBundleInternal(assetPath,force);
            AssetsBundleLoader.UnloadDependencies(assetPath,force);
            _assetProviders.Remove(assetBundleName);
        }

        public IAssetProvider LoadAsset(string assetBundleName)
        {
            return CreateAssetProvider(assetBundleName);
        }

        #region sync operations

        public T LoadAsset<T>(string assetBundleName, string assetName) where  T : Object{
            var provider = CreateAssetProvider(assetBundleName);
            provider.Load();
            return provider.LoadAsset<T>(assetName);
        }

        #endregion

        #region async operations


        public IEnumerator CreateAssetProvider<T>(string assetBundleName, Action<T> callback)
            where T: Object
        {
            var provider = CreateAssetProvider(assetBundleName);
            yield return provider.Execute();
            yield return provider.LoadAssetAsync<T>(callback);
        }

        public IEnumerator LoadAssetsAsync<T>(Action<List<T>> callback) where T : Object {
            var bundleName = typeof(T).Name.ToLower();
            yield return LoadAssetsAsync<T>(bundleName, callback);
        }

        public IEnumerator LoadAssetsAsync<T>(string bundleName, Action<List<T>> callback) where T : Object
        {
            var provider = CreateAssetProvider(bundleName);
            yield return provider.Execute();
            yield return provider.LoadAssetsAsync<T>(callback);
        }

        public IEnumerator LoadAssetsAsync(Type type,string bundleName, Action<List<Object>> callback) 
        {
            var provider = CreateAssetProvider(bundleName);
            yield return provider.Execute();
            yield return provider.LoadAssetsAsync(type,callback);
        }

        public IEnumerator LoadAssetByTypeNameAsync<T>(string name, Action<T> callback) where T : Component
        {
            var provider = CreateAssetProvider(name);
            yield return provider.Execute();
            yield return provider.LoadAssetByTypeNameAsync<T>(callback);
        }

        public IEnumerator LoadAllPrefabAssetsAsync<T>(string name,Action<List<T>> callback) where T : Component
        {
            var provider = CreateAssetProvider(name);
            yield return provider.Execute();
            yield return provider.LoadPrefabAssetsAsync<T>(callback);
        }
        
        public IEnumerator LoadLevelAsync(string assetBundleName, string levelName, bool isAdditive)
        {
            var assetPath = GetAssetPath(assetBundleName);
            Debug.LogFormat("Loading {0} from {1} bundle", levelName, assetPath);
            AssetOperation operation = null;
            operation = IsSumulateMode
                ? (AssetOperation) new AssetLevelSimulationOperation(assetPath, levelName, isAdditive)
                : new AssetLevelOperation(assetPath, levelName, isAdditive);
            yield return operation.Execute();
        }

        public IEnumerator LoadPrefabAssetAsync<T>(string name, Action<T> action)
            where T : Component
        {
        var provider = CreateAssetProvider(name);
        yield return provider.Execute();
        yield return provider.LoadPrefabAssetAsync<T>(name,action);
    }

    #endregion

    public void UnloadAllAssets(bool force = false)
    {
        Debug.LogError("UnloadAllAssets NotImplementedException");
    }
    
    public string GetAssetPath(string assetBundleName)
    {
        if (IsSumulateMode)
            return assetBundleName;
        if (!_assetsPaths.ContainsKey(assetBundleName))
            _assetsPaths[assetBundleName] = AssetsUrl + assetBundleName;
        return _assetsPaths[assetBundleName];
    }

    #endregion

    #region private methtods

    // Load asset from the given assetBundle.
    private IAssetProvider CreateAssetProvider(string assetBundleName)
    {
        if (_assetProviders.ContainsKey(assetBundleName))
            return _assetProviders[assetBundleName];
        var assetPath = GetAssetPath(assetBundleName);

        GameLog.LogFormat("Create AssetProvider [{0}]", assetPath);

        var operation = IsSumulateMode
            ? (IAssetProvider)new AssetBundleOperationSimulation(assetBundleName)
            : new AssetBundleOperation(assetBundleName,assetPath,true);    
        _assetProviders[assetBundleName] = operation;
        return operation;
    }

    #endregion

}
}