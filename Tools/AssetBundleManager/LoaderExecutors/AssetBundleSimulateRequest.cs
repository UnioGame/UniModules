using System.Collections;
using Assets.Scripts.ProfilerTools;
using Assets.Tools.Utils;

namespace AssetBundlesModule {

    public class AssetBundleSimulateRequest : AssetBundleRequest {

        protected override void OnComplete() { 
            
            GameProfiler.BeginSample("AssetBundleSimulateRequest.OnComplete");

            var resource = ClassPool.Spawn<SimulateBundleResource>();
            resource.Initialize(BundleName);
            BundleResource = resource;
            base.OnComplete();

            GameProfiler.EndSample();

        }

        protected override bool IsIterationActive() {
            return false;
        }

    }

}


