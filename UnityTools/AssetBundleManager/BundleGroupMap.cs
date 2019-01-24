using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UniModule.UnityTools.AssetBundleManager {

    public enum ResourceGroupTag {

        AfterBattle,
        CampaignClosed,
        Campaign2,
        Campaign3,
        Menu,
        BeforeMenu,
        BeforeBattle,
    }

    [Serializable]
    public class BundleGroupItem {

        public ResourceGroupTag GroupTag;
        public List<RuntimePlatform> TargetPlatforms = new List<RuntimePlatform>();
        public List<string> Bundles = new List<string>();
        public bool ForceUnload;
        public bool ForceUnloadMode = true;

        [HideInInspector] public bool ShowBundles;
        [HideInInspector] public bool ShowBundlesInput;
        [HideInInspector] public bool ShowRuntimePlatforms;

    }

    [CreateAssetMenu(fileName = "BundleGroupMap", menuName = "Asset Bundles/BundleGroupMap")]
    public class BundleGroupMap : ScriptableObject {

        public List<BundleGroupItem> BundleGroups = new List<BundleGroupItem>();

        public void Unload(ResourceGroupTag resourceGroupTag, RuntimePlatform platform) {

            if (BundleGroups == null) return;
            
            var groupItems = BundleGroups.Where(x => x.GroupTag == resourceGroupTag && 
                                                     (x.TargetPlatforms.Count == 0 || x.TargetPlatforms.Contains(platform)));
            foreach (var item in groupItems) {
                foreach (var bundle in item.Bundles) {

                    AssetBundleManager.Instance.UnloadAssetBundle(bundle, item.ForceUnload,item.ForceUnloadMode);

                }
            }

        }

    }
}
