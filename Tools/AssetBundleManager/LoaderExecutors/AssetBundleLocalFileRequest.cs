using System.Collections;
using UnityEngine;

namespace AssetBundlesModule
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
            BundleResource = new LoadedAssetBundle(AssetBundle.LoadFromFile(Resource));

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


