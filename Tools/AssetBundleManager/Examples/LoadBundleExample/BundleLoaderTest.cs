using System;
using System.Collections;
using System.Collections.Generic;
using AssetBundlesModule;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BundleLoaderTest : MonoBehaviour
{
    private Coroutine _coroutine;
    private bool _isBundleLoaded;
    private IAssetBundleManager _bundleManager;
    private IAssetBundleResourceRequest _assetBundleResourceRequest;
    private IAssetBundleResource _assetBundleResource;
    private string _status;
    private string _assetName;

    private Dictionary<string,IAssetBundleResource> _bundleResources = new Dictionary<string, IAssetBundleResource>();
    private List<string> _spritesNames = new List<string>();
    private List<string> _assets = new List<string>();

    [SerializeField] private Transform _origin;
    [SerializeField] private int _count;
    [SerializeField] private Button _button;
    [SerializeField] private Button _unloadABButton;
    [SerializeField] private Button _unloadABForceButton;
    [SerializeField] private Button _collectGCButton;
    [SerializeField] private BundleDropDownList _bundlesDropdown;
    [SerializeField] private Dropdown _assetsDropdown;
    [SerializeField] private Text _buttonName;
    
    private void Start()
    {
        _isBundleLoaded = false;
        _bundleManager = AssetBundleManager.Instance;
        _unloadABButton.onClick.AddListener(() => UnloadAssetBundle(false));
        _unloadABForceButton.onClick.AddListener(() => UnloadAssetBundle(true));
        _collectGCButton.onClick.AddListener(CollectGC);
        IntializeDropDown();
    }

    private void IntializeDropDown() {

        _bundlesDropdown.OnValueChanged += UpdateSelectedBundle;
        UpdateSelectedBundle(_bundlesDropdown.SelectedBundleName);
    }

    private void UpdateSelectedBundle(string bundleName) {
        

        ClearAssetsDropDown();
        
        if (_assets.Count == 0) return;

        _assetsDropdown.AddOptions(_assets);
        _assetsDropdown.value = 0;
        _assetName = _assets[_assetsDropdown.value];

        _assetsDropdown.onValueChanged.AddListener(x => { _assetName = _assets[x]; });

    }

    private void ClearAssetsDropDown() {

        _assetsDropdown.onValueChanged.RemoveAllListeners();
        _assetsDropdown.value = 0;
        _assetsDropdown.options.Clear();

    }


    private void UpdateBundleResource(IAssetBundleResource resource) {

        _assets.Clear();
        if (resource == null)
            return;
        _assets.AddRange(resource.AllAssetsNames);
        UpdateSelectedBundle(_bundlesDropdown.SelectedBundleName);

    }


    private string[] GetBundleNames() {

#if UNITY_EDITOR

        var bundleNames = AssetDatabase.GetAllAssetBundleNames();
        return bundleNames;

#endif
        
        return _bundleManager.AssetBundleManifest.GetAllAssetBundles();
    }

    public void LoadBundleResource() {

        var bundleName = _bundlesDropdown.SelectedBundleName;
        _assetBundleResource =  _bundleManager.GetAssetBundleResource(bundleName);
        _bundleResources[bundleName] = _assetBundleResource;
        UpdateBundleResource(_assetBundleResource);

    }

    public void LoadAsset() {
        StartCoroutine(Load(_assetBundleResource, _assetName));
    }

    private IEnumerator Load(IAssetBundleResource assetBundle, string assetName)
    {
        Transform go = null;
        yield return assetBundle.LoadAssetAsync<Transform>(assetName, x => go = x);
        if (!go) {
            UpdateStatus(string.Format("Asset {0} {1} loading failed" , assetBundle.BundleName, assetName));
        }
        else {
            UpdateStatus(string.Format("Asset {0} {1} Loaded", assetBundle.BundleName, assetName));
        }
        Instantiate(go,_origin.position,Quaternion.identity);
        _coroutine = null;
    }

    private void UpdateStatus(string status) {
        _status = status;
        Debug.LogError(_status);
        _buttonName.text = _status;
    }

    private void UnloadAssetBundle(bool forceUnload)
    {
        _bundleManager.UnloadAssetBundle(_bundlesDropdown.SelectedBundleName,forceUnload);
        _assetBundleResourceRequest = null;
    }

    private void CollectGC()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
    }
}