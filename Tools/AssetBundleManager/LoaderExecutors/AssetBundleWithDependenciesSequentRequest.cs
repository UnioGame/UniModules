using System.Collections;

namespace AssetBundlesModule
{

    public class AssetBundleWithDependenciesSequentRequest : AssetBundleWithDependenciesBaseRequest
    {
        protected override IEnumerator MoveNext() {     
            
            for (var i = 0; i < _allRequests.Count; i++) {
                var request = _allRequests[i];
                if(request.IsDone == false)
                    yield return _allRequests[i].Execute();
            }
            
        }

    }
}
