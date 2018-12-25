using System.Collections;
using System.Collections.Generic;
using Assets.Tools.UnityTools.AssetBundleManager.Interfaces;
using Assets.Tools.UnityTools.AsyncOperations;
using Assets.Tools.UnityTools.ObjectPool.Scripts;

namespace Assets.Tools.UnityTools.AssetBundleManager.LoaderExecutors
{

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
