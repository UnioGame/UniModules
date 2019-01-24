using System.Collections;
using UniModule.UnityTools.AssetBundleManager.Interfaces;

namespace UniModule.UnityTools.AssetBundleManager.LoaderExecutors
{

    public class AssetBundleWithDependenciesSequentRequest : AssetBundleWithDependenciesBaseRequest
    {
        protected override IEnumerator MoveNext() {     
            
            for (var i = 0; i < Requests.Count; i++) {
                var request = Requests[i];
                if(request.IsDone == false)
                    yield return Requests[i].Execute();
            }
            
        }

        public AssetBundleWithDependenciesSequentRequest(IAssetBundleResourceMap resourceMap,
                                                         IAssetBundlesRequestCache requestCache) : 
            base(resourceMap,requestCache) {
        }

    }
}
