using UniGreenModules.UniCore.Runtime.Interfaces;
using UniRx.Async;

namespace UniGreenModules.UniGameSystems.Runtime.Scriptable
{
    using System.Collections.Generic;
    using UniCore.Runtime.ObjectPool.Runtime;
    using UniCore.Runtime.ObjectPool.Runtime.Extensions;
    using UnityEngine;

    [CreateAssetMenu(menuName = "GameSystem/Sources/AsyncSourceQueue", fileName = nameof(AsyncContextQueueDataSources))]
    public class AsyncContextQueueDataSources : 
        AsyncContextDataSource
    {
        public List<AsyncContextDataSource> sources = new List<AsyncContextDataSource>();

        public override async UniTask<IContext> Register(IContext context)
        {
            foreach (var t in sources) {
                await t.Register(context);
            }
            
            return context;
        }

    }
}
