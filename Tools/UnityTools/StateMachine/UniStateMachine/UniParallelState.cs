using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Modules.UnityToolsModule.Tools.UnityTools.Common;
using Assets.Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
using Assets.Scripts.Extension;
using Assets.Tools.Utils;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
using UnityEngine;

namespace UniStateMachine
{
    [CreateAssetMenu(menuName = "States/States/UniParallelState", fileName = "UniParallelState")]
    public class UniParallelState : UniStateBehaviour
    {
        [NonSerialized]
        private IContextProvider<IContext> _contextProvider;
        [NonSerialized]
        private IContextExecutor<IEnumerator> _executor;

        [SerializeField]
        private List<UniStateParallelMode> _states = new List<UniStateParallelMode>();

        protected override IEnumerator ExecuteState(IContext context)
        {
            var routineDisposables = ClassPool.Spawn<List<IDisposableItem>>();
            var completionSource = ClassPool.Spawn<CompletionConditionSource>();
            completionSource.Initialize(() => IsComplete(routineDisposables));

            _contextProvider.AddValue(context, routineDisposables);
            _contextProvider.AddValue(context, completionSource);

            //launch states
            for (int i = 0; i < _states.Count; i++)
            {
                var state = _states[i].StateBehaviour;
                var routine = state.Execute(context);
                var disposable = _executor.Execute(routine);
                routineDisposables.Add(disposable);
            }

            yield return completionSource.RoutineWaitUntil();

        }

        protected bool IsComplete(List<IDisposableItem> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (!item.IsDisposed)
                    return false;
            }

            return true;
        }

        protected override void OnInitialize()
        {
            _contextProvider = new ContextProviderProvider<IContext>();
            _executor = new UniRoutineExecutor();
            base.OnInitialize();
        }

        protected override void OnExit(IContext context)
        {

            var disposableItems = _contextProvider.Get<List<IDisposableItem>>(context);
            for (int i = 0; i < disposableItems.Count; i++)
            {
                var item = disposableItems[i];
                item.Dispose();
            }
            disposableItems.Despawn();

            var completionsSource = _contextProvider.Get<CompletionConditionSource>(context);
            completionsSource.Despawn();

            base.OnExit(context);
        }

    }
}
