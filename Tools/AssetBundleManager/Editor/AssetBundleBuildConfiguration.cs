using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[Serializable]
public class BundleRuntimePlatformConfiguration {

    public List<BuildTarget> RuntimePlatforms = new List<BuildTarget>();
    public List<BuildAssetBundleOptions> AssetBundleOptions = new List<BuildAssetBundleOptions>() {
        BuildAssetBundleOptions.ChunkBasedCompression
    };

};

[CreateAssetMenu(menuName = "Asset Bundles/Create Configuration")]
public class AssetBundleBuildConfiguration : ScriptableObject {

    public List<BuildAssetBundleOptions> DefaultAssetBundleOptions = new List<BuildAssetBundleOptions>(){BuildAssetBundleOptions.ChunkBasedCompression};

    public List<BundleRuntimePlatformConfiguration> PlatformConfiguration;

    public BuildAssetBundleOptions GetRuntimeBundleOptions(BuildTarget platform) {

        var assetBundleOptions = BuildAssetBundleOptions.None;
        var configFound = false;

        var optionItems = PlatformConfiguration.Where(x => x.RuntimePlatforms.Contains(platform));

        foreach (var configuration in optionItems) {
            configFound = true;
            assetBundleOptions |= CreateOptions(configuration.AssetBundleOptions);
        }
        if (configFound == false) {
            assetBundleOptions = CreateOptions(DefaultAssetBundleOptions);
        }

        return assetBundleOptions;
    }
    
    private BuildAssetBundleOptions CreateOptions(List<BuildAssetBundleOptions> options) {

        var bundleOptions = BuildAssetBundleOptions.None;
        foreach (var option in options)
        {
            Debug.LogFormat("~~~Add BuildAssetBundleOption [{0}]", option);
            bundleOptions |= option;
        }

        return bundleOptions;
    }
}
