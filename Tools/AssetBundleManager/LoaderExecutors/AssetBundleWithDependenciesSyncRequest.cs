using System.Collections;
using System.Collections.Generic;
using Assets.Tools.Utils;

namespace AssetBundlesModule
{

    public class AssetBundleWithDependenciesSyncRequest : AssetBundleWithDependenciesBaseRequest
    {
        protected override void OnComplete()
        {

            for (int i = 0; i < _allRequests.Count; i++) {
                var request = _allRequests[i];
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

    }
}
