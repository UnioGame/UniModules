using System.Collections;
using Assets.Scripts.ProfilerTools;

namespace AssetBundlesModule {

    public class AssetBundleSimulateRequest : AssetBundleRequest {

        protected override void OnComplete() { 
            
            GameProfiler.BeginSample("AssetBundleSimulateRequest.OnComplete");

            BundleResource = new SimulateBundleResource(BundleName);
            base.OnComplete();

            GameProfiler.EndSample();

        }

        protected override bool IsIterationActive() {
            return false;
        }

    }

}


