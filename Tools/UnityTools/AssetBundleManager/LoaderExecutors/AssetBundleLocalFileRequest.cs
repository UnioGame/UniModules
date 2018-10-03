using System.Collections;
using Assets.Tools.UnityTools.AssetBundleManager.AssetBundleResources;
using Assets.Tools.UnityTools.ObjectPool.Scripts;
using UnityEngine;

namespace Assets.Tools.UnityTools.AssetBundleManager.LoaderExecutors
{
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


