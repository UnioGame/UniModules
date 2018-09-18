using System.Collections;
using System.Collections.Generic;
using Assets.Tools.Utils;
using UniRx;

namespace Tools.AsyncOperations
{
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

    }
}
