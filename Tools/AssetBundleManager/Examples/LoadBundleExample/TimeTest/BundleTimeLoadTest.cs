using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AssetBundlesModule;
using Assets.Scripts.ProfilerTools;
using UnityEngine;
using AssetBundlesModule_Old;
using UniRx;
using OldBundleManager = AssetBundlesModule_Old.AssetBundleManager;


public class BundleTimeLoadTest : MonoBehaviour {

    private AssetBundleSourceType _sourceType = AssetBundleSourceType.AsyncLocalFile;
    private string _bundleName = "data-assetsprites";
    private const string _budnledependepcies = "data-bundledependencies";
    private string _manifestName = "AssetBundles";
    private string _bundleTemplateName = "data-assetprefabs{0}";
    private string _baseUrl = "";
    private string _assetTemplate = "sprite_test {0}";
    private int count = 100;
    private int _bundlesCount = 30;
    private string[] _bundlesNames;

    private AssetBundleConfiguration _bundleConfiguration;
    private AssetBundleManager _assetBundleManager;

    #region inspector properties

    public bool LoadBundlesFromStreaming = false;

    public bool AsyncDependencies = false;

    public bool SimulateMode = false;

    public bool AsyncAnyBundleLoading = false;
    
    #endregion

    private void Awake() {


        _bundleConfiguration = new AssetBundleConfiguration(_manifestName, _baseUrl, SimulateMode, AsyncDependencies);
        _assetBundleManager = new AssetBundleManager(_bundleConfiguration);

        if (LoadBundlesFromStreaming) {
            InitializeStreamingBundles(_assetBundleManager);
        }
        else {
            InitializeTestBundles();
        }

    }

    private void InitializeTestBundles() {

        _bundlesNames = new string[_bundlesCount + 1];
        for (int i = 0; i < _bundlesCount; i++)
        {
            var bundle = string.Format(_bundleTemplateName, i + 1);
            _bundlesNames[i] = bundle;
        }
        _bundlesNames[_bundlesCount] = _budnledependepcies;


    }

    private void InitializeStreamingBundles(AssetBundleManager bundleManager) {
        
        var manifest = bundleManager.AssetBundleManifest;
        _bundlesNames = manifest.GetAllAssetBundles();

    }

    public string[] GetBundleNames()
    {

#if UNITY_EDITOR

        if (LoadBundlesFromStreaming == false)
        {
            var bundleNames = UnityEditor.AssetDatabase.GetAllAssetBundleNames();
            return bundleNames;
        }

#endif

        return _assetBundleManager.AssetBundleManifest.GetAllAssetBundles();
    }

    public void SetAssetBundleSourceType(AssetBundleSourceType sourceType) {
        _sourceType = sourceType;
    }

    public void OriginAsyncTest() {
        Observable.FromCoroutine(x => LoadAsyncOrigin(_bundlesNames)).Subscribe();
    }

    public void OriginTest()
    {
        LoadOrigin(_bundlesNames);
    }

    public void LoadBundlesWithSimulationMode()
    {
        StartCoroutine(LoadBundles(_bundlesNames, _assetBundleManager, AssetBundleSourceType.Simulation));
    }

    public void LoadBundlesWithDependencies() {
        Observable.FromCoroutine(x => 
            LoadBundles(_bundlesNames, _assetBundleManager, _sourceType)).Subscribe();
    }

    public void LoadOldBundlesAsync() {
        Observable.FromCoroutine(x => LoadBundlesOld(_bundlesNames)).Subscribe();    
    }

    public void LoadBundlesWithSyncDependencies()
    {
        Observable.FromCoroutine(x => LoadBundles(_bundlesNames, _assetBundleManager, _sourceType)).Subscribe();
    }

    public void LoadAllWithType() {
        
        StartCoroutine(LoadAllTypeAssets(_assetBundleManager, _sourceType));

    }

    public void LoadWithWeb() {
        
        StartCoroutine(LoadBundles(_bundlesNames, _assetBundleManager, AssetBundleSourceType.WWW));

    }

    public void LoadLocal() {
        StartCoroutine(LoadBundles(_bundlesNames, _assetBundleManager,AssetBundleSourceType.LocalFile));
    }

    public void LoadLocalAsync()
    {
        StartCoroutine(LoadBundles(_bundlesNames, _assetBundleManager, _sourceType));
    }

    public void LoadLocalAsyncAndWait()
    {
        var enumerator = LoadBundles(_bundlesNames, _assetBundleManager, _sourceType);
        enumerator.WaitCoroutine();
    }

    public void LoadBundlesAsync(string[] assetBundleNames, AssetBundleSourceType sourceType) {
        Observable.FromCoroutine(x => 
            LoadBundles(assetBundleNames, _assetBundleManager, sourceType)).Subscribe();
    }

    public void LoadAllFromWeb() {
        
        StartCoroutine(LoadBundles(_bundlesNames, _assetBundleManager, AssetBundleSourceType.WWW));

    }

    public void LoadAllLocalOneByOneLocal() {
        var configuration = new AssetBundleConfiguration(_manifestName, _baseUrl, false);
        StartCoroutine(LoadAllOneByOneTypeAssets(configuration, _sourceType));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bundles"></param>
    /// <param name="bundleManager"></param>
    /// <param name="sourceType"></param>
    /// <returns></returns>
    private IEnumerator LoadBundles(string[] bundles, AssetBundleManager bundleManager, AssetBundleSourceType sourceType) {
        
        var bundleResources = new IAssetBundleResource[bundles.Length];
        var bundlesIds = string.Join(":\n", bundles);
        
        var id = GameProfiler.BeginWatchRunTime("BUNDLE TEST :" + bundlesIds);

        for (var i = 0; i < bundles.Length; i++) {
            var bundle = bundles[i];
            var index = i;
            if (AsyncAnyBundleLoading) {
                Observable.FromCoroutine(x =>
                    LoadBundle(bundle, bundleManager, sourceType, y => bundleResources[index] = y)).Subscribe();
            }
            else {
                yield return LoadBundle(bundle, bundleManager, sourceType, y => bundleResources[index] = y);
            }
        }
        
        GameProfiler.StopWatchRunTime(id);
    }

    private IEnumerator LoadBundle(string bundle, AssetBundleManager bundleManager, 
        AssetBundleSourceType sourceType, Action<IAssetBundleResource> action) {

        yield return bundleManager.GetAssetBundleResourceAsync(bundle, sourceType, action);

    }

    private IEnumerator LoadBundlesOld(string[] bundles)
    {

        var bundlesIds = string.Join(":\n", bundles);
        var id = GameProfiler.BeginWatchRunTime("BUNDLE OLD TEST :" + bundlesIds);

        for (var i = 0; i < bundles.Length; i++)
        {
            var bundle = bundles[i];
            var provider = OldBundleManager.Instance.LoadAsset(bundle);
            yield return provider.Execute();
        }

        GameProfiler.StopWatchRunTime(id);

        yield break;
    }

    private IEnumerator LoadAllTypeAssets(AssetBundleManager bundleManager,AssetBundleSourceType sourceType) {
        
        var sprites = new List<Sprite>();

        var id = GameProfiler.BeginWatchRunTime("[LoadAllTypeAssets]");
        yield return bundleManager.LoadAssetsAsync<Sprite>(_bundleName, sourceType, x => sprites = x);
        GameProfiler.StopWatchRunTime(id);

        var names = sprites.Select(x => x.name).ToArray();
        var loadedNames = string.Join(", ", names);
        Debug.Log(loadedNames);

    }

    private IEnumerator LoadAllOneByOneTypeAssets(AssetBundleConfiguration configuration, AssetBundleSourceType sourceType)
    {

        var manager = new AssetBundleManager(configuration);
        var sprites = new List<Sprite>();
        var assetNames = new List<string>();

        for (int i = 1; i <= count; i++) {
            assetNames.Add(string.Format(_assetTemplate,i));    
        }

        var id = GameProfiler.BeginWatchRunTime("[LoadAllTypeAssets]");

        for (int i = 0; i < count; i++) {
            yield return manager.LoadAssetAsync<Sprite>(_bundleName,assetNames[i], sourceType, x => sprites.Add(x) );
        }

        GameProfiler.StopWatchRunTime(id);

        var names = sprites.Select(x => x.name).ToArray();
        var loadedNames = string.Join(", ", names);
        Debug.Log(loadedNames);

    }

    private IEnumerator LoadAsyncOrigin(string[] bundleNames) {
        
        var assetPath = Application.streamingAssetsPath + "/AssetBundles/{0}";
        var paths = bundleNames.Select(x => string.Format(assetPath, x)).ToList();

        var id = GameProfiler.BeginWatchRunTime("ORIGIN LOAD bundles");

        for (var i = 0; i < paths.Count; i++) {

            var path = paths[i];
            var request = AssetBundle.LoadFromFileAsync(path);
            yield return request;

        }
        
        GameProfiler.StopWatchRunTime(id);

    }

    private void LoadOrigin(string[] bundleNames)
    {

        var assetPath = Application.streamingAssetsPath + "\\AssetBundles\\{0}";
        var paths = bundleNames.Select(x => string.Format(assetPath, x)).ToList();

        var id = GameProfiler.BeginWatchRunTime("ORIGIN LOAD bundles");

        for (var i = 0; i < paths.Count; i++)
        {

            var path = paths[i];
            AssetBundle.LoadFromFile(path);

        }

        GameProfiler.StopWatchRunTime(id);

    }
}
