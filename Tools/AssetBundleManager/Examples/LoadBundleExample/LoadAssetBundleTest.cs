using System;
using System.Collections;
using System.Collections.Generic;
using AssetBundlesModule;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class LoadAssetBundleTest : MonoBehaviour
{
    private Coroutine _coroutine;
    private bool _isBundleLoaded;
    private IAssetBundleManager _bundleManager;
    private Dictionary<string, IAssetBundleResourceRequest> _assetBundleProviders = new Dictionary<string, IAssetBundleResourceRequest>();

    [SerializeField] private Image _image;
    [SerializeField] private string _assetBundleName = "prefabs-bundle";
    [SerializeField] private string _assetName = "test_asset 1";
    [SerializeField] private List<string> _assetNames = new List<string>();
    [SerializeField] private bool _autoStart = false;
    [SerializeField] private bool _async = true;

    public void CollectGC()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
    }

    public void UnloadBundle()
    {
        UnloadAssetBundle(_assetBundleName,false);
    }

    public void UnloadBundleForce()
    {
        UnloadAssetBundle(_assetBundleName, true);
    }

    private IEnumerator Start()
    {
        _isBundleLoaded = false;
        _bundleManager = AssetBundleManager.Instance;
        if (!_autoStart) {
            yield break;
        }
        if (_async) {   
                yield return LoadAsset(_assetBundleName, _assetName);            
        }
        else {
            LoadAssetSync(_assetBundleName,_assetName);
        }
        
    }

    public void LoadBundle()
    {
        if (_coroutine != null)
            return;
        _coroutine = StartCoroutine(LoadBundleAsync(_assetBundleName));
    }

    public void LoadAsset()
    {
        if(_coroutine!=null)
            return;
            
        _coroutine = StartCoroutine(LoadAsset(_assetBundleName, _assetName));
    }

    private IEnumerator LoadBundleAsync(string assetBundle)
    {
        if (_assetBundleProviders.ContainsKey(assetBundle))
            yield break;
        //var assetProvider = _bundleManager.LoadAssetBundleResource(assetBundle);
        //yield return ((ICommandRoutine) assetProvider).Execute();
        //_assetBundleProviders[assetBundle] = assetProvider;     
    }

    private void LoadAssetSync(string assetBundleName,string assetName) {
        if (string.IsNullOrEmpty(assetName))
            return;
        var asset = LoadAsset<Sprite>(assetBundleName, assetName);
        var position = new Vector3(0, 0, 900);
        if (!asset)
        {
            Debug.LogError("Asset " + _assetName + " not loaded");
            return;
        }
        _image.sprite = asset;

    }

    private T LoadAsset<T>(string assetBundleName, string assetName) where T: Object {
        T asset = null;
        var routine =  AssetBundleManager.Instance.LoadAssetAsync<T>(assetBundleName,assetName,AssetBundleSourceType.LocalFile, x => { asset = x; });
        routine.WaitCoroutine();
        return asset;
    }

    private IEnumerator LoadAsset(string assetBundleName,string assetName)
    {
        if (!_assetBundleProviders.ContainsKey(assetBundleName))
            yield return LoadBundleAsync(assetBundleName);
        yield return LoadAssetAsync(assetBundleName, assetName);
        foreach (var asset in _assetNames) {
            yield return LoadAssetAsync(assetBundleName, asset);
        }
        _coroutine = null;
    }

    private IEnumerator LoadAssetAsync(string assetBundleName,string assetName) {
        
        Object go = null;
        yield return _bundleManager.LoadAssetAsync<Transform>(assetBundleName,assetName, AssetBundleSourceType.AsyncLocalFile, x => go = x);
        if (!go)
        {
            Debug.LogError("Asset " + assetName + " not loaded");
            yield break;
        }
        var position = new Vector3(0,0,900);
        var item = GameObject.Instantiate(go,position, Quaternion.identity,Camera.main.transform);
        Debug.LogFormat("LOAD ASSET {0} FROM BUNDLE {1}",assetName,assetBundleName);
    }
    
    private void UnloadAssetBundle(string bundleName,bool forceUnload)
    {
        _bundleManager.UnloadAssetBundle(_assetBundleName, forceUnload);
        _assetBundleProviders.Remove(bundleName);
    }

}
