using UniModule.UnityTools.AssetBundleManager.Interfaces;
using UniModule.UnityTools.CoroutineTools;

namespace UniModule.UnityTools.AssetBundleManager.LoaderExecutors
{

    public class AssetBundleWithDependenciesSyncRequest : AssetBundleWithDependenciesBaseRequest
    {
        protected override void OnComplete()
        {

            for (int i = 0; i < Requests.Count; i++) {
                var request = Requests[i];
                if(request.IsDone)
                    continue;
                var awaiter = request.Execute();
                awaiter.WaitCoroutine();
            }

            base.OnComplete();
        }

        protected override bool IsIterationActive() {
            return false;
        }

        public AssetBundleWithDependenciesSyncRequest(IAssetBundleResourceMap resourceMap, IAssetBundlesRequestCache requestCache) : 
            base(resourceMap, requestCache) {
        }

    }
}
