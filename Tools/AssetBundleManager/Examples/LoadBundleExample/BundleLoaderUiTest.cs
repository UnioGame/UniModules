using System.Collections;
using System.Collections.Generic;
using AssetBundlesModule;
using UnityEngine;
using UnityEngine.UI;

public class BundleLoaderUiTest : MonoBehaviour {

    [SerializeField]
    private Dropdown _bundleName;

    [SerializeField]
    private Button _loadButton;


    public void Load() {
        var index = _bundleName.value;
        var activeBundleName = _bundleName.options[index].text;
        var resource = AssetBundleManager.Instance.GetAssetBundleResource(activeBundleName);
        if (resource != null) {
            Debug.LogFormat("AssetBundle Loaded = {0}",activeBundleName);
        }
        else {
            Debug.LogFormat("Load AssetBundle {0} Failed", activeBundleName);
        }
    }

    private void Start() {
        _loadButton = GetComponent<Button>();
        if (_loadButton) {
            _loadButton.onClick.AddListener(Load);
        }
    }
}
