using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Dropdown))]
public class BundleDropDownList : MonoBehaviour {

    private List<string> _allBundlesNames;
    private IAssetBundleManager _bundleBundleManager;
    private Dropdown _bundlesDropdown;

    public bool LoadFromManifest;

    public string SelectedBundleName { get; protected set; }

    public Action<string> OnValueChanged;

    private void IntializeDropDown()
    {

        _allBundlesNames = new List<string>(GetBundleNames());
        _bundlesDropdown.onValueChanged.RemoveAllListeners();

        if (_allBundlesNames == null || _allBundlesNames.Count == 0)
            return;

        _bundlesDropdown.onValueChanged.RemoveAllListeners();

        Filter(string.Empty);

        _bundlesDropdown.
            onValueChanged.AddListener(x => UpdateSelectedValue(_allBundlesNames[_bundlesDropdown.value]));

    }

    public void Filter(string filter) {
        _bundlesDropdown.options.Clear();
        var items = string.IsNullOrEmpty(filter)
            ? _allBundlesNames
            : _allBundlesNames.Where(x => x.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        _bundlesDropdown.AddOptions(items);
        _bundlesDropdown.value = 0;

        SelectedBundleName = _allBundlesNames[_bundlesDropdown.value];

    }

    private string[] GetBundleNames()
    {

#if UNITY_EDITOR

        if (LoadFromManifest == false) {
            var bundleNames = AssetDatabase.GetAllAssetBundleNames();
            return bundleNames;
        }

#endif

        return _bundleBundleManager.AssetBundleManifest.GetAllAssetBundles();
    }
    
    private void UpdateSelectedValue(string assetBundle) {
        SelectedBundleName = assetBundle;
        if (OnValueChanged != null)
            OnValueChanged(assetBundle);
    }

    private void Start() {
        _bundlesDropdown = GetComponent<Dropdown>();
        _bundleBundleManager = AssetBundleManager.Instance;
        IntializeDropDown();
    }
}
