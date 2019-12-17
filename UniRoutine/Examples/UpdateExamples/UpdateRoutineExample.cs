using System.Collections;
using UnityEngine;

namespace UniGreenModules.UniRoutine.Examples.UpdateExamples
{
    using Runtime;

    public class UpdateRoutineExample : MonoBehaviour
    {
        public int         updateCount;
        public int         routineCount;
        public RoutineType RoutineType = RoutineType.Update;
    
        // Start is called before the first frame update
        private void Start()
        {
            for (int i = 0; i < routineCount; i++) {
                OnUpdate(i).ExecuteRoutine(RoutineType, false);
            }
        }

        private IEnumerator OnUpdate(int number)
        {
            var counter = 0;
            while (this && isActiveAndEnabled) {
                counter++;
                if (counter > updateCount) {
                    Debug.Log($"ROUTINE: COUNTER #{number} {RoutineType} FINISHED");
                    yield break;
                }

                yield return null;
            }
        }

    }
}
