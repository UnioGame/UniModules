using UniModule.UnityTools.AssetBundleManager.AssetBundleResources;
using UniModule.UnityTools.UniPool.Scripts;
using UniModule.UnityTools.ProfilerTools;

namespace UniModule.UnityTools.AssetBundleManager.LoaderExecutors {

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


