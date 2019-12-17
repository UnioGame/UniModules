namespace UniGreenModules.AssetBundleManager.Runtime.LoaderExecutors
{
    using System.Collections;
    using AssetBundleResources;
    using UniCore.Runtime.ObjectPool.Runtime;
    using UnityEngine;

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


