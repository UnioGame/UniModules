using System.Collections;
using UniModule.UnityTools.AssetBundleManager.AssetBundleResources;
using UnityEngine;

namespace UniModule.UnityTools.AssetBundleManager.LoaderExecutors
{
    using UniGreenModules.UniCore.Runtime.ObjectPool;

    public class AssetBundleLocalFileRequest : AssetBundleRequest
    {
        protected override IEnumerator MoveNext()
        {
            yield break;
        }

        protected override void OnComplete() {
            base.OnComplete();

            if (BundleResource != null)
                return;
            var resource = ClassPool.Spawn<LoadedAssetBundle>();
            resource.Initialize(AssetBundle.LoadFromFile(Resource));
            BundleResource = resource;

        }

        protected override void OnReset() {
            base.OnReset();
            BundleResource = null;
        }

        protected override bool IsIterationActive() {
            return false;
        }

    }
}


