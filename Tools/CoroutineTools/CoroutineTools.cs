using System.Collections;
using System.Collections.Generic;
using Assets.Tools.Utils;
using Tools;

public static class CoroutineTools {

	public static void WaitCoroutine(this IEnumerator enumerator) {
		if (enumerator == null) return;
		while (enumerator.MoveNext()) {

		    var current = enumerator.Current;
            var currentEnumerator = current as IEnumerator;

		    if (currentEnumerator != null) {
				WaitCoroutine(currentEnumerator);
			}
		}
	}

    public static void WaitCoroutine<T>(this IEnumerator<T> enumerator) {
		if (enumerator == null) return;
		while (enumerator.MoveNext()) {
			var currentEnumerator = enumerator.Current as IEnumerator<T>;
			if (currentEnumerator != null) {
				WaitCoroutine<T>(currentEnumerator);
			}
		}
	}

    public static IEnumerator WaitCoroutines(this List<IEnumerator> enumerators) {

        var iterators = ClassPool.Spawn<List<CoroutineIterator>>();

        for (int i = 0; i < enumerators.Count; i++) {
            var it = ClassPool.Spawn<CoroutineIterator>();
            it.Initialize(enumerators[i]);
            iterators.Add(it);
        }   

        while (iterators.TrueForAll(x => x.IsDone) == false) {

            for (int i = 0; i < iterators.Count; i++) {
                var it = iterators[i];
                if(it.IsDone)continue;
                it.MoveNext();
            }

            yield return null;
        }

        iterators.DespawnRecursive();
    }


}
