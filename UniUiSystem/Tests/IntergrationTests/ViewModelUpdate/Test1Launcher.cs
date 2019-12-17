using UnityEngine;

namespace UniGreenModules.UniUiSystem.Tests.IntergrationTests.ViewModelUpdate
{
    using System.Collections;
    using UniRoutine.Runtime;
    using UniRoutine.Runtime.Extension;
    using UniRx;

    public class Test1Launcher : MonoBehaviour
    {
        public Test1ModelView view;
        public Test1MessageModel model;
        
        // Start is called before the first frame update
        private void Start()
        {
            view.Initialize(model);
            
            OnUpdateItems("UpdateStep").ExecuteRoutine(RoutineType.Update).AddTo(this);
            OnUpdateItems("FixedUpdate").ExecuteRoutine(RoutineType.FixedUpdate).AddTo(this);
            OnUpdateItems("EndOfFrame").ExecuteRoutine(RoutineType.EndOfFrame).AddTo(this);
            
        }

        private IEnumerator OnUpdateItems(string updaterName)
        {
            var couter = 0;
            
            while (isActiveAndEnabled) {

                yield return this.WaitForSeconds(0.5f);

                Debug.Log($"UPDATER : {updaterName} value : {couter}");
                
                model.Message.Value = couter.ToString();
                
                couter++;

            }
        }

    }
}
