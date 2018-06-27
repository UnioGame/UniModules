using System.Collections;
using AssetBundlesModule;
using UnityEngine;

namespace AssetBundlesModule_Old
{
    public static class AssetsLoader  {

        public static IEnumerator InstantiateGameObjectAsync(AssetBundleOperation request)
        {
            // This is simply to get the elapsed time for this phase of AssetLoading.
            var startTime = Time.realtimeSinceStartup;
            // Load asset from assetBundle.
            if (request == null)
            {
                Debug.LogErrorFormat("Request is NULL");
                yield break;
            }
            yield return request;
            // Calculate and display the elapsed time.
            var elapsedTime = Time.realtimeSinceStartup - startTime;
            Debug.Log(string.Format("request complete. Time : {0} seconds", elapsedTime));
        }
    }
}
