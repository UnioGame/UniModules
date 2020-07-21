namespace UniGreenModules.UniGame.SerializableContext.Runtime.Scriptable
{
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using UniCore.Runtime.Interfaces;
    using UniModules.UniGame.Context.Runtime.Abstract;
    using UnityEngine;

    [CreateAssetMenu(menuName = "UniGame/GameSystem/Sources/AsyncSourceQueue", fileName = nameof(AsyncContextQueueDataSources))]
    public class AsyncContextQueueDataSources : 
        AsyncContextDataSource
    {
        public List<AsyncContextDataSource> sources = new List<AsyncContextDataSource>();

        public override async UniTask<IContext> RegisterAsync(IContext context)
        {
            //await each data source registration as queue
            foreach (var t in sources) {
                await t.RegisterAsync(context);
            }
            
            return context;
        }

    }
}
