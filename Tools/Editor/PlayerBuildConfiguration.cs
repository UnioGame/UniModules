using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Tools.BuildTools {
  
    [CreateAssetMenu(menuName = "Build/PlayerBuild Configuration")]
    public class PlayerBuildConfiguration : ScriptableObject {
    
        public List<BuildOptions> DefaultAssetBundleOptions = new List<BuildOptions>();
    
        public List<PlayerBuildOption> PlatformConfiguration;
        
        public BuildOptions GetRuntimeBundleOptions(BuildTargetGroup platform) {
    
            var assetBundleOptions = BuildOptions.None;
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
        
        private BuildOptions CreateOptions(List<BuildOptions> options) {
    
            var bundleOptions = BuildOptions.None;
            foreach (var option in options)
            {
                Debug.LogFormat("~~~Add BuildAssetBundleOption [{0}]", option);
                bundleOptions |= option;
            }
    
            return bundleOptions;
        }
    }
    
    [Serializable]
    public class PlayerBuildOption {
    
        public List<BuildTargetGroup> RuntimePlatforms = new List<BuildTargetGroup>();
        public List<BuildOptions> AssetBundleOptions = new List<BuildOptions>();
        
    } 

}

