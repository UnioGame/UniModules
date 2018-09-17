using System.Collections;
using Assets.Scripts.ProfilerTools;
using Assets.Tools.Utils;
using UnityEngine;

namespace AssetBundlesModule
{

    public class AssetBundleLocalFileAsyncRequest : AssetBundleRequest {

        private AssetBundleCreateRequest _bundleCreateRequest;
        
        protected override IEnumerator MoveNext()
        {

            _bundleCreateRequest = AssetBundle.LoadFromFileAsync(Resource);
            yield return WaitBundle(_bundleCreateRequest);

        }

        private IEnumerator WaitBundle(AsyncOperation createRequest) {

            yield return createRequest;

        }

        protected override void OnComplete() {
            base.OnComplete();

            if (BundleResource != null)
                return;

            var resource = ClassPool.Spawn<LoadedAssetBundle>();
            resource.Initialize(_bundleCreateRequest.assetBundle);
            BundleResource = resource;
            _bundleCreateRequest = null;
        }

        protected override void OnInitialize()
        {

            if (string.IsNullOrEmpty(Resource))
            {
                Error = "Load Bundle from local file request ERROR : Resource is NULL";
                GameLog.LogError(Error);
                return;
            }

        }
    }

}



