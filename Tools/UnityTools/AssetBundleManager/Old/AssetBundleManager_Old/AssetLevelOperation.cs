using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Tools.UnityTools.AssetBundleManager.Old.AssetBundleManager_Old{

    public class AssetLevelOperation : AssetOperation
    {
        private AsyncOperation _asyncOperation;

        public string LevelName { get; protected set; }
        public bool IsAdditive { get; protected set; }

        public AssetLevelOperation(string assetbundleName, string levelName, bool isAdditive)
            :base(assetbundleName)
        {
            LevelName = levelName;
            IsAdditive = isAdditive;
        }

        protected override IEnumerator WaitBundleLoad()
        {
            _asyncOperation = IsAdditive ? 
                SceneManager.LoadSceneAsync(LevelName, LoadSceneMode.Additive) :
                SceneManager.LoadSceneAsync(LevelName);
            yield return _asyncOperation;
        }
    }
}
