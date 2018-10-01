using System;
using System.Collections;
using Assets.Scripts.Extensions;
using Tools.UniRoutineTask;
using UnityToolsModule.Tools.UnityTools.UniRoutine;

namespace UniStateMachine
{
    public class RoutineStateExecutor : IStateExecutor<IStateBehaviour<IEnumerator>>
    {
        private IDisposable _disposables;
        private IStateBehaviour<IEnumerator> _state;
        private RoutineType _routineType = RoutineType.UpdateStep;

        public void SetRoutineType(RoutineType routineType)
        {
            _routineType = routineType;
        }

        public void Execute(IStateBehaviour<IEnumerator> state)
        {
            _state = state;
            _disposables = state.Execute().RunWithSubRoutines(_routineType);
        }

        public void Stop()
        {
            if(_state!=null)
                _state.Exit();
            _state = null;
            _disposables.Cancel();
            _disposables = null;
            _routineType = RoutineType.UpdateStep;
        }
    }
}
