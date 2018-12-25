#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Assets.Tools.UnityTools.AssetBundleManager.Old.AssetBundleManager_Old{
    
    public class AssetSimulationOperationProvider
    {
        private const string SimulateAssetBundlesKey = "SimulateAssetBundles";
        private const string LazyBundleModeKey = "LazyBundleMode";
        private static int SimulateAssetBundleMode = -1;
        private static int LazyBundleMode = -1;

        // Flag to indicate if we want to simulate assetBundles in Editor without building them actually.
        public static bool IsSimulateMode
        {
            get
            {
#if UNITY_EDITOR
                if (SimulateAssetBundleMode == -1)
                    SimulateAssetBundleMode = EditorPrefs.
                        GetBool(SimulateAssetBundlesKey, true) ? 1 : 0;

                return SimulateAssetBundleMode != 0;
#else
                return false;
#endif
            }
            set
            {
#if UNITY_EDITOR
                var newValue = value ? 1 : 0;
                if (newValue != SimulateAssetBundleMode)
                {
                    SimulateAssetBundleMode = newValue;
                    EditorPrefs.SetBool(SimulateAssetBundlesKey, value);
                }
#endif
            }
        }

        
        // Flag to indicate if we want to simulate assetBundles in Editor without building them actually.
        public static bool IsLazyMode
        {
            get
            {
#if UNITY_EDITOR
                if (LazyBundleMode == -1)
                    LazyBundleMode = EditorPrefs.
                        GetBool(LazyBundleModeKey, true) ? 1 : 0;

                return LazyBundleMode != 0;
#else
                return false;
#endif
            }
            set
            {
#if UNITY_EDITOR
                var newValue = value ? 1 : 0;
                if (newValue != LazyBundleMode)
                {
                    LazyBundleMode = newValue;
                    EditorPrefs.SetBool(LazyBundleModeKey, value);
                }
#endif
            }
        }


    }
}
