namespace UniGreenModules.UniRoutine.Runtime {
    using System;
    using System.Collections.Generic;
    using Interfaces;
    using UnityEngine;

    public class UniRoutineRootObject : MonoBehaviour, IDisposable
    {
        private List<IUniRoutine> lateRoutines = new List<IUniRoutine>();

        public void Dispose()
        {
            lateRoutines.Clear();
        }
        
        public void AddLateRoutine(IUniRoutine routine)
        {
            lateRoutines.Add(routine);
        }
        
        private void LateUpdate()
        {
            for (int i = 0; i < lateRoutines.Count; i++) {
                var routine = lateRoutines[i];
                routine.Update();
            }
        }
    }

}
