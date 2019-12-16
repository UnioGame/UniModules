using System.Collections;
using UnityEngine;

namespace UniGreenModules.UniRoutine.Examples.UpdateExamples
{
    using UniRx;

    public class MicroRoutineUpdater : MonoBehaviour
    {
        public int updateCount;
        public int routineCount;
        // Start is called before the first frame update
        private void Start()
        {
            for (int i = 0; i < routineCount; i++) {
                Observable.FromMicroCoroutine(() => OnUpdate(i)).Subscribe();
            }
        }

        private IEnumerator OnUpdate(int number)
        {
            var counter = 0;
            while (this && isActiveAndEnabled) {
                counter++;
                if (counter > updateCount) {
                    Debug.Log($"UniRx MicroRoutine: COUNTER #{number}  FINISHED");
                    yield break;
                }

                yield return null;
            }
        }
    }
}
