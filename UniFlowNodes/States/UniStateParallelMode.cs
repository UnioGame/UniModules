namespace UniGreenModules.UniFlowNodes.States
{
    using System;
    using UniRoutine.Runtime;

    [Serializable]
    public class UniStateParallelMode
    {
        public UniStateTransition Transition;
        public RoutineType RoutineType = RoutineType.Update;
        public bool RestartOnComplete = true;
    }
}
