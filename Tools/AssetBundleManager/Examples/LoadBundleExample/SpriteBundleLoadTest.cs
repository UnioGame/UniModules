using System;
using System.Collections;
using System.Collections.Generic;
using AssetBundlesModule;
using UnityEngine;
using UnityEngine.UI;

public class SpriteBundleLoadTest : MonoBehaviour
{
    private const string AssetBundleName = "sprites-data";
    private const string SpriteNameTemplate = "sprite_test {0}";

    private Coroutine _coroutine;
    private bool _isBundleLoaded;
    private IAssetBundleManager _bundleManager;
    private IAssetBundleResourceRequest _assetBundleResourceRequest;
    
    private List<string> _spritesNames = new List<string>();

    [SerializeField] private int _count;
    [SerializeField] private Button _button;
    [SerializeField] private Button _unloadABButton;
    [SerializeField] private Button _unloadABForceButton;
    [SerializeField] private Button _collectGCButton;
    [SerializeField] private Button _loadBundleButton;
    [SerializeField] private Dropdown _dropdown;
    [SerializeField] private Text _buttonName;
    
    private void Start()
    {
        _isBundleLoaded = false;
        _bundleManager = AssetBundleManager.Instance;
        _unloadABButton.onClick.AddListener(() => UnloadAssetBundle(false));
        _unloadABForceButton.onClick.AddListener(() => UnloadAssetBundle(true));
        _collectGCButton.onClick.AddListener(CollectGC);
        InitializeSpriteNames();
        IntializeDropDown();
    }

    private void InitializeSpriteNames()
    {
        for (int i = 0; i < _count; i++)
        {
            var index = i + 1;
            var spriteName = string.Format(SpriteNameTemplate,index);
            _spritesNames.Add(spriteName);
        }
        
    }

    private void IntializeDropDown()
    {
        _dropdown.options.Clear();
        _dropdown.AddOptions(_spritesNames);
        _dropdown.value = 1;
        _dropdown.onValueChanged.AddListener(x => OnSelectionChanged(_dropdown.value));
    }

    private void OnSelectionChanged(int index)
    {
        if (_coroutine != null)
            return;
        _coroutine = StartCoroutine(SetImage(index));
    }

    private IEnumerator SetImage(int index)
    {
        Sprite sprite = null;
        var spriteName = _spritesNames[index];
        yield return _bundleManager.LoadAssetAsync<Sprite>(AssetBundleName,spriteName,AssetBundleSourceType.LocalFile, x => sprite = x);
        if (!sprite)
        {
            Debug.LogError("Sprite asset" + spriteName + " not loaded");
        }
        _buttonName.text = spriteName;
        _button.image.sprite = sprite;
        _coroutine = null;
    }


    private void UnloadAssetBundle(bool forceUnload)
    {
        _bundleManager.UnloadAssetBundle(AssetBundleName,forceUnload);
        _assetBundleResourceRequest = null;
    }

    private void CollectGC()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
    }
}