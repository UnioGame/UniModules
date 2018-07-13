using System;
using System.Collections;
using System.Collections.Generic;
using AssetBundlesModule;
using Assets.Scripts.ProfilerTools;
using UnityEngine;
using Object = UnityEngine.Object;

public class AssetBundleManager : IAssetBundleManager
{
    private readonly IAssetBundleResourceMap _map;
    private const AssetBundleSourceType _defaultResourceType = AssetBundleSourceType.AsyncLocalFile;
    private readonly IAssetsBundleLoader _bundleLoader;

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
        _map = Configuration.AssetBundleResourceMap;
        _bundleLoader = Configuration.AssetsBundleLoader;
    }

    #endregion

    public bool IsSumulateMode
    {
        get { return Configuration.SimulateMode; }
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

        sourceType = IsSumulateMode ? AssetBundleSourceType.Simulation : sourceType;
        sourceType = !Application.isEditor && sourceType == AssetBundleSourceType.Simulation
            ? _defaultResourceType
            : sourceType;
        
        var request = _bundleLoader.GetAssetBundleRequest(assetBundleName, sourceType);
        var awaiter = request.Execute();

        if (request.BundleResource == null) {
            var id = GameProfiler.BeginWatch(string.Format("GetAssetBundleResource {0} from {1} ", assetBundleName, sourceType));

            yield return awaiter;

            GameProfiler.StopWatch(id);
        }
        var resource = request.BundleResource;

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
    public void UnloadAssetBundle(string assetBundleName, bool force = false)
    {
        // If we're in Editor simulation mode, we don't have to load the manifest assetBundle.
        if (IsSumulateMode)
            return;

        _map.Unload(assetBundleName,force);

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

        GameProfiler.BeginSample("AssetBundleManager.GetBundleResourceRequest");

        var mode = IsSumulateMode ? AssetBundleSourceType.Simulation : AssetBundleSourceType.LocalFile;
        var request = _bundleLoader.GetAssetBundleRequest(assetBundleName, mode);

        GameProfiler.EndSample();

        if (request.BundleResource != null)
            return request.BundleResource;

        GameProfiler.BeginSample("AssetBundleManager.GetBundleResourceExecute");

        var awaiter = request.Execute();
        awaiter.WaitCoroutine();

        GameProfiler.EndSample();

        return request.BundleResource;


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

