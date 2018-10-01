using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Extensions;
using UnityEngine;
using UnityToolsModule.Tools.UnityTools.UniRoutine;

namespace UniStateMachine
{
    [CreateAssetMenu(menuName = "States/States/UniParallelState", fileName = "UniParallelState")]
    public class UniParallelState : UniStateBehaviour
    {
        [NonSerialized]
        private List<IDisposable> _stateDisposables;

        [SerializeField]
        private List<UniStateParallelMode> _states = new List<UniStateParallelMode>();

        protected override IEnumerator ExecuteState()
        {
            _stateDisposables.Cancel();
            _stateDisposables = _stateDisposables ?? new List<IDisposable>();
            
            //launch states
            for (int i = 0; i < _states.Count; i++)
            {
                var state = _states[i].StateBehaviour;
                state.Initialize(_contextProvider);

                var disposable = state.Execute().RunWithSubRoutines();
                _stateDisposables.Add(disposable);
            }
            while (IsActive)
            {
                yield return null;
            }
        }

        protected override void OnExit()
        {
            _stateDisposables.Cancel();
            for (int i = 0; i < _states.Count; i++)
            {
                var state = _states[i].StateBehaviour;
                if (state.IsActive)
                {
                    state.Exit();
                }
            }
            base.OnExit();
        }
    }
}
