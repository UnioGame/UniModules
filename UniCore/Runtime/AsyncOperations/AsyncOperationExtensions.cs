namespace UniGreenModules.UniCore.Runtime.AsyncOperations
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using ObjectPool;
    using ObjectPool.Runtime;
    using ObjectPool.Runtime.Extensions;
    using UniRx;
    using UniRx.Async;
    using UnityEngine;

    public static class AsyncOperationExtensions
    {

        public static IEnumerator WaitAll(this List<IEnumerator> operations) {

            var counter = ClassPool.Spawn<List<int>>();
            counter.Add(0);

            for (var i = 0; i < operations.Count; i++) {
                var index = i;
                Observable.FromCoroutine(x => operations[index]).DoOnCompleted(() => counter[0]++).Subscribe();
            }

            while (counter[0] < operations.Count) {
                yield return null;
            }

            counter.Despawn();
        }
        
        
        public static IEnumerator AwaitAsUniTask<T>(this Task<T> task)
        {

            yield return task.AsUniTask().ToCoroutine();

        }
        
        public static IEnumerator AwaitTask<T>(this Task<T> task)
        {

            while (!task.IsCompleted) {
                yield return null;                
            }

            if (task.IsFaulted) {
                Debug.LogError($"{nameof(task)} Filed");
            }

        }

        
        public static IEnumerator AwaitTask<T>(this Task<T> task, CancellationToken cancellationToken)
        {

            while (!task.IsCompleted) {
                
                yield return null;
                
            }

            if (task.IsFaulted) {
                Debug.LogError($"{nameof(task)} Filed");
            }

        }
    }
}
