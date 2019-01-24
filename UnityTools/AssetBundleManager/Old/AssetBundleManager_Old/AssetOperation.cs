using System.Collections;
using UniModule.UnityTools.ProfilerTools;
using UnityEngine;

namespace UniModule.UnityTools.AssetBundleManager.Old.AssetBundleManager_Old{
    
    public abstract class AssetOperation : IAssetOperation
    {
        #region public properties

        public string Error { get; protected set; }

        public string AssetName { get; protected set; }

        public bool IsDone { get; protected set; }

        #endregion

        #region constructor

        public AssetOperation(string assetName)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                Debug.LogError("NULL ASSET URL");
            }
            AssetName = assetName;
        }

        #endregion
        
        #region public methods

        public void Reset()
        {
            AssetName = null;
            Error = null;
            IsDone = false;
        }

        public IEnumerator Execute()
        {
            GameLog.Log("WaitBundleLoad");
            yield return WaitBundleLoad();
            GameLog.Log("OnEnter");
            yield return Initialize();
            IsDone = true;
        }

        #endregion

        #region private methods


        protected T GetAsset<T>(Object asset)
            where T : Object
        {
            if (!asset) return null;
            var result = asset as T;
            if (result != null) return result;
            var targetObject = asset as GameObject;
            return targetObject ? targetObject.GetComponent<T>() : null;
        }

        protected virtual IEnumerator WaitBundleLoad()
        {
            yield return AssetsBundleLoader.LoadAssetBundleAsync(AssetName);
        }

        protected virtual IEnumerator Initialize()
        {
            yield break;
        }

        #endregion

    }

}
