using UniModule.UnityTools.AssetBundleManager.AssetBundleResources;

namespace UniModule.UnityTools.AssetBundleManager.LoaderExecutors {
    using UniGreenModules.UniCore.Runtime.ObjectPool;
    using UniGreenModules.UniCore.Runtime.ProfilerTools;

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


