namespace UniGreenModules.UniNodeSystem.Runtime
{
    using System;
    using UniTools.UniRoutine.Runtime;
    using UnityEngine.Serialization;

    [Serializable]
    public class UniStateParallelMode
    {
        public UniStateTransition Transition;
        public RoutineType RoutineType = RoutineType.Update;
        public bool RestartOnComplete = true;
    }
}
