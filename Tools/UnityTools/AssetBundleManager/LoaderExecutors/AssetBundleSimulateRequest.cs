using Assets.Tools.UnityTools.AssetBundleManager.AssetBundleResources;
using Assets.Tools.UnityTools.ObjectPool.Scripts;
using Assets.Tools.UnityTools.ProfilerTools;

namespace Assets.Tools.UnityTools.AssetBundleManager.LoaderExecutors {

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


