using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
using Assets.Tools.Utils;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
using UnityEngine;
using UnityToolsModule.Tools.UnityTools.UniRoutine;

namespace UniStateMachine
{
    [CreateAssetMenu(menuName = "States/States/UniParallelState", fileName = "UniParallelState")]
    public class UniParallelState : UniStateBehaviour
    {
        [SerializeField]
        private List<UniStateParallelMode> _states = new List<UniStateParallelMode>();

        protected override IEnumerator ExecuteState(IContext context)
        {
            //var routineDisposables = ClassPool.Spawn<List<IDisposableItem>>();

            ////launch states
            //for (int i = 0; i < _states.Count; i++)
            //{
            //    var state = _states[i].StateBehaviour;
            //    var disposable = state.Execute(context)
            //        .RunWithSubRoutines();
            //    _stateDisposables.Add(disposable);
            //}

            //while (IsActive(context))
            //{
            //    yield return null;
            //}

            //routineDisposables.Despawn();

            yield break;
        }

        protected override void OnExit(IContext context)
        {
            //_stateDisposables.Cancel();
            //for (int i = 0; i < _states.Count; i++)
            //{
            //    var state = _states[i].StateBehaviour;
            //    if (state.IsActive(context))
            //    {
            //        state.Exit(context);
            //    }
            //}
            //base.OnExit(context);
        }

    }
}
