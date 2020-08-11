namespace UniGreenModules.UniCore.Runtime.AsyncOperations
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Cysharp.Threading.Tasks;
    using ObjectPool.Runtime;
    using ObjectPool.Runtime.Extensions;
    using UniRx;
    using UnityEngine;

    public static class AsyncOperationExtensions
    {
        public static async UniTask<T> WaitOrCancel<T>(this UniTask<T> task, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await UniTask.WhenAny(task, token.WhenCanceled());
            token.ThrowIfCancellationRequested();

            return await task;
        }

        public static async UniTask WaitOrCancel(this UniTask task, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await UniTask.WhenAny(task, token.WhenCanceled());
            token.ThrowIfCancellationRequested();

            await task;
        }

        public static async Task<T> WaitOrCancel<T>(this Task<T> task, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await Task.WhenAny(task, token.WhenCanceledTask());
            token.ThrowIfCancellationRequested();

            return await task;
        }

        public static async Task WaitOrCancel(this Task task, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await Task.WhenAny(task, token.WhenCanceledTask());
            token.ThrowIfCancellationRequested();

            await task;
        }

        public static UniTask WhenCanceled(this CancellationToken cancellationToken)
        {
            var completionSource = new UniTaskCompletionSource<bool>();
            cancellationToken.Register(x=>((UniTaskCompletionSource<bool>)x).TrySetResult(true), completionSource);
            return completionSource.Task;
        }

        public static Task WhenCanceledTask(this CancellationToken cancellationToken)
        {
            var completionSource = new TaskCompletionSource<bool>();
            cancellationToken.Register(x=>((TaskCompletionSource<bool>)x).SetResult(true), completionSource);
            return completionSource.Task;
        }

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

            counter.DespawnCollection<List<int>,int>();
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
