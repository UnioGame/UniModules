using System.Collections;
using Assets.Scripts.Tools.AssetBundleManager.AssetBundleResources;

namespace AssetBundlesModule {

    public class AssetBundleSimulateRequest : AssetBundleRequest {

        protected override void OnComplete() { 

            BundleResource = new SimulateBundleResource(BundleName);
            base.OnComplete();

        }

        protected override bool IsIterationActive() {
            return false;
        }

    }

}


