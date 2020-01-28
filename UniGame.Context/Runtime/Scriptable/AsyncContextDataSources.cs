namespace UniGreenModules.UniGame.SerializableContext.Runtime.Scriptable
{
    using System.Collections.Generic;
    using Context.Runtime.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ObjectPool.Runtime;
    using UniCore.Runtime.ObjectPool.Runtime.Extensions;
    using UniRx.Async;
    using UnityEngine;

    [CreateAssetMenu(menuName = "UniGame/GameSystem/Sources/AsyncSources", fileName = nameof(AsyncContextDataSources))]
    public class AsyncContextDataSources : 
        AsyncContextDataSource
    {
        public List<AsyncContextDataSource> sources = new List<AsyncContextDataSource>();

        public override async UniTask<IContext> RegisterAsync(IContext context)
        {
            var taskList = ClassPool.Spawn<List<UniTask<IContext>>>();

            foreach (var t in sources) {
                taskList.Add(t.RegisterAsync(context));
            }
            
            await UniTask.WhenAll(taskList);
            
            taskList.DespawnCollection();

            return context;
        }

    }
}
