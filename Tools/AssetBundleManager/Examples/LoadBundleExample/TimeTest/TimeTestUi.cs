using System;
using System.Collections.Generic;
using System.Linq;
using AssetBundlesModule;
using UnityEngine;
using UnityEngine.UI;

public class TimeTestUi : MonoBehaviour {


    private static List<AssetBundleSourceType> SourceTypes = Enum.GetValues(typeof(AssetBundleSourceType)).OfType<AssetBundleSourceType>().ToList();
    private static List<string> SourceTypeNames = Enum.GetNames(typeof(AssetBundleSourceType)).ToList();

    private string[] AssetBundlesNames;

    private Dictionary<string, Action> _actions;
    public BundleTimeLoadTest BundleTimeLoadTest;
    public Dropdown TestItemsDropdown;
    public Dropdown LoadTypesDropdown;
    public Button Launch;
    public Button LoadWithType;
    public Dropdown BundleDropDownList;

    public InputField InputFiSearchField;

	// Use this for initialization
	void Start () {

        LoadTypesDropdown.options.Clear();
        LoadTypesDropdown.AddOptions(SourceTypeNames);
	    LoadTypesDropdown.value = 0;

        _actions = new Dictionary<string, Action>() {
	        {"LoadLocalAsyncAndWait",BundleTimeLoadTest.LoadLocalAsyncAndWait },
	        {"LoadBundles & Dependencies",BundleTimeLoadTest.LoadBundlesWithDependencies },

	        {"OLD LoadBundles Async",BundleTimeLoadTest.LoadOldBundlesAsync },

	        {"ORIGIN Async",BundleTimeLoadTest.OriginAsyncTest },
	        {"ORIGIN Sync",BundleTimeLoadTest.OriginTest },

            {"LoadBundles SyncDependencies",BundleTimeLoadTest.LoadBundlesWithSyncDependencies },
	        {"LoadBundles Simulate Mode",BundleTimeLoadTest.LoadBundlesWithSimulationMode },
            {"LoadAll One By One Local",BundleTimeLoadTest.LoadAllLocalOneByOneLocal },
            {"LoadAllWithType",BundleTimeLoadTest.LoadAllWithType },
            {"LoadLocal Sync",BundleTimeLoadTest.LoadLocal },
	        {"LoadLocalAsync",BundleTimeLoadTest.LoadLocalAsync },
            {"LoadAllFromWeb",BundleTimeLoadTest.LoadAllFromWeb },
	        {"LoadWithWeb",BundleTimeLoadTest.LoadWithWeb },
        };

	    var names = _actions.Keys.ToList();
        TestItemsDropdown.options.Clear();
        TestItemsDropdown.AddOptions(names);
	    TestItemsDropdown.value = 0;

        Launch.onClick.AddListener(LaunchTest);



	    LoadWithType.onClick.AddListener(() => {
	        var index = BundleDropDownList.value;
	        var value = BundleDropDownList.options[index];
            var bundles = new string[]{ value.text };
	        var type = SourceTypes[LoadTypesDropdown.value];
            BundleTimeLoadTest.LoadBundlesAsync(bundles, type);
	    });
	    LoadTypesDropdown.onValueChanged.AddListener(x => {

	        var type = SourceTypes[x];
            BundleTimeLoadTest.SetAssetBundleSourceType(type);

	    });

        InputFiSearchField.onValueChanged.AddListener((x) => UpdateBundlesList(x));
        UpdateBundlesList(string.Empty);
    }

    private void LaunchTest() {
        var itemName = TestItemsDropdown.options[TestItemsDropdown.value].text;
        _actions[itemName]();
    }

    private void UpdateBundlesList(string filter) {
        AssetBundlesNames = BundleTimeLoadTest.GetBundleNames();
        if(BundleDropDownList.options.Count>0)
            BundleDropDownList.ClearOptions();
        BundleDropDownList.AddOptions(AssetBundlesNames.Where(x => {
            return string.IsNullOrEmpty(filter) ||
                   x.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0;
        }).ToList());
        BundleDropDownList.value = 0;
    }

}
