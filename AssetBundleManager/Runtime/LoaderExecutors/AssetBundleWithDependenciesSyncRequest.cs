namespace UniGreenModules.AssetBundleManager.Runtime.LoaderExecutors
{
    using Interfaces;
    using UniCore.Runtime.CoroutineTools;

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
