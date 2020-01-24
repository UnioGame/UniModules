using UniGreenModules.UniCore.Runtime.Interfaces;
using UniRx.Async;

namespace UniGreenModules.UniGameSystems.Runtime.Scriptable
{
    using System.Collections.Generic;
    using UniCore.Runtime.ObjectPool.Runtime;
    using UniCore.Runtime.ObjectPool.Runtime.Extensions;
    using UnityEngine;

    [CreateAssetMenu(menuName = "GameSystem/Sources/AsyncSources", fileName = nameof(AsyncContextDataSources))]
    public class AsyncContextDataSources : 
        AsyncContextDataSource
    {
        public List<AsyncContextDataSource> sources = new List<AsyncContextDataSource>();

        public override async UniTask<IContext> Register(IContext context)
        {
            var taskList = ClassPool.Spawn<List<UniTask<IContext>>>();

            foreach (var t in sources) {
                taskList.Add(t.Register(context));
            }
            
            await UniTask.WhenAll(taskList);
            
            taskList.DespawnCollection();

            return context;
        }

    }
}
