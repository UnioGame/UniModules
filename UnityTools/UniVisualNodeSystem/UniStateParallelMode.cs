using System;
using UniModule.UnityTools.UniRoutine;
using UnityEngine.Serialization;

namespace UniStateMachine
{
    [Serializable]
    public class UniStateParallelMode
    {
        [FormerlySerializedAs("State")] 
        public UniStateTransition Transition;
        public RoutineType RoutineType = RoutineType.UpdateStep;
        public bool RestartOnComplete = true;
    }
}
