using System;
using UnityEngine.Serialization;

namespace UniStateMachine
{
    using UniTools.UniRoutine.Runtime;

    [Serializable]
    public class UniStateParallelMode
    {
        [FormerlySerializedAs("State")] 
        public UniStateTransition Transition;
        public RoutineType RoutineType = RoutineType.UpdateStep;
        public bool RestartOnComplete = true;
    }
}
