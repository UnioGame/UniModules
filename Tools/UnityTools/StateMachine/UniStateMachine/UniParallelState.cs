using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Extensions;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
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

        protected override IEnumerator ExecuteState(IContextProvider context)
        {
            _stateDisposables.Cancel();
            _stateDisposables = _stateDisposables ?? new List<IDisposable>();
            
            //launch states
            for (int i = 0; i < _states.Count; i++)
            {
                var state = _states[i].StateBehaviour;
 
                var disposable = state.Execute(context)
                    .RunWithSubRoutines();
                _stateDisposables.Add(disposable);
            }

            while (IsActive(context))
            {
                yield return null;
            }
            
            yield break;
        }

        protected override void OnExit(IContextProvider context)
        {
            _stateDisposables.Cancel();
            for (int i = 0; i < _states.Count; i++)
            {
                var state = _states[i].StateBehaviour;
                if (state.IsActive(context))
                {
                    state.Exit(context);
                }
            }
            base.OnExit(context);
        }
    }
}
