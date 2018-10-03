using System;
using Assets.Tools.UnityTools.UniRoutine;

namespace Assets.Tools.UnityTools.StateMachine.UniStateMachine
{
    [Serializable]
    public class UniStateParallelMode
    {
        public UniStateBehaviour StateBehaviour;
        public RoutineType RoutineType = RoutineType.UpdateStep;
    }
}
