using UnityEngine;

namespace UniGreenModules.UniRoutine.Examples.ExtensionsExamples
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Runtime;
    using Runtime.Extension;
    using UniRx;
    using Random = UnityEngine.Random;

    public struct SimulationItem
    {
        public int Id;
        public IDisposable Disposable;
        public string Name;
    }
    
    public class WaitRoutineTest : MonoBehaviour
    {
        public int count = 100;

        private int activeCounter;
        
        private List<int> types = new List<int>() { 0,2,4 };
        private List<SimulationItem> disposables = new List<SimulationItem>();
        
        // Start is called before the first frame update
        private void Start()
        {
            for (int i = 0; i < count; i++) {
                StartRoutine();
            }
            OnUpdate().ExecuteRoutine().
                AddTo(this);
        }

        private IEnumerator OnUpdate()
        {
            while (this && isActiveAndEnabled) {

                var action = Random.Range(0, 2);

                if (disposables.Count >= count)
                    action = 0;
                
                if (action == 0 && disposables.Count > 0) {
                    StopRoutine();
                }
                else {
                    StartRoutine();
                }
                
                yield return null;
            }
        }

        private void StopRoutine()
        {
            var index = Random.Range(0, disposables.Count);
            var routine = disposables[index];
            disposables.Remove(routine);
            routine.Disposable.Dispose();
            Debug.Log($"DISPOSE {routine.Name} ROUTINE: Dispose ID {routine.Id}");
        }

        
        private void StartRoutine()
        {
            var id = activeCounter++;
            var methodName = string.Empty;
            var type = types[Random.Range(0, types.Count)];
            
            IDisposable disposable = null;
            switch (type) {
                case 0:
                    disposable = this.WaitForSeconds(Random.Range(0f, 10f)).
                        ContinueWith(() => Debug.Log($"ROUTINE: WaitForSeconds => ContinueWith ID {id}")).
                        ExecuteRoutine();
                    methodName = "WaitForSeconds.ContinueWith";
                    break;
                case 1:
                    disposable = this.OnUpdate(x => {
                        Debug.Log($"ROUTINE: OnUpdate ID {id}");
                        return true;
                    }).ExecuteRoutine();
                    methodName = "OnUpdate";
                    break;
                case 2:
                    disposable = this.WaitForSecondUnscaled(Random.Range(0f, 10f)).
                        ContinueWith(() => Debug.Log($"ROUTINE: WaitForSecondUnscaled => ContinueWith ID {id}")).
                        ExecuteRoutine();
                    methodName = "WaitForSecondUnscaled.ContinueWith";
                    break;
                case 3:
                    disposable = this.WaitUntil(() => {
                            Debug.Log($"ROUTINE: WaitUntil ID {id}");
                            return false;
                        }).
                        ExecuteRoutine();
                    methodName = "WaitUntil";
                    break;
                case 4:
                    disposable = this.DoDelayed(() => Debug.Log($"ROUTINE: DoDelayed ID {id}")).
                        ContinueWith(() => Debug.Log($"ROUTINE: DoDelayed => ContinueWith ID {id}")).
                        ExecuteRoutine();
                    methodName = "DoDelayed.ContinueWith";
                    break;
            }
            
            disposables.Add(new SimulationItem() {
                Disposable = disposable,
                Id = id,
                Name = methodName,
            });
        }
        
    }
}
