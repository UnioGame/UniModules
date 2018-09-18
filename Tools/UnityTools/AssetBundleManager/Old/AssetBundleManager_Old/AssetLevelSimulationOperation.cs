using System.Collections;
using Assets.Scripts.ProfilerTools;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AssetBundlesModule_Old{

    public class AssetLevelSimulationOperation : AssetOperation
    {
        private AsyncOperation _asyncOperation;

        public AssetLevelSimulationOperation(string assetBundleName, 
            string levelName, bool isAdditive)
            :base(assetBundleName)
        {
#if UNITY_EDITOR

            var levelPaths = AssetDatabase.
                GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, levelName);
            if (levelPaths.Length == 0)
            {
                ///@TODO: The error needs to differentiate that an asset bundle name doesn't exist
                //        from that there right scene does not exist in the asset bundle...
                Debug.LogErrorFormat("There is no scene with name {0}  in {1}", levelName,assetBundleName);
                return;
            }
            if (isAdditive)
                _asyncOperation = EditorApplication.LoadLevelAdditiveAsyncInPlayMode(levelPaths[0]);
            else
                _asyncOperation = EditorApplication.LoadLevelAsyncInPlayMode(levelPaths[0]);
#endif
        }

        protected override IEnumerator WaitBundleLoad()
        {
            yield return _asyncOperation;
        }

    }

}
