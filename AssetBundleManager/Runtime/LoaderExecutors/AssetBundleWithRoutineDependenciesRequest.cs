namespace UniGreenModules.AssetBundleManager.Runtime.LoaderExecutors
{
    using System.Collections;
    using System.Collections.Generic;
    using Interfaces;
    using UniCore.Runtime.AsyncOperations;
    using UniCore.Runtime.ObjectPool.Runtime;
    using UniCore.Runtime.ObjectPool.Runtime.Extensions;

    public class AssetBundleWithRoutineDependenciesRequest : AssetBundleWithDependenciesBaseRequest
    {
        protected override IEnumerator MoveNext() {
            
            var requestAwaiters = ClassPool.Spawn<List<IEnumerator>>();

            requestAwaiters.Add(_bundleRequest.Execute());
            for (var i = 0; i < _dependencies.Count; i++)
            {
                requestAwaiters.Add(_dependencies[i].Execute());
            }

            yield return requestAwaiters.WaitAll();

            requestAwaiters.Despawn();

        }

        public AssetBundleWithRoutineDependenciesRequest(IAssetBundleResourceMap resourceMap,
                                                         IAssetBundlesRequestCache requestCache) : 
            base(resourceMap,requestCache) {
        }

    }
}
