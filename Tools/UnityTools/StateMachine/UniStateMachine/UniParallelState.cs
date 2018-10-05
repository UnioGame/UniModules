using System.Collections;
using System.Collections.Generic;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.ObjectPool.Scripts;
using Assets.Tools.UnityTools.UniRoutine;
using UnityEngine;

namespace Assets.Tools.UnityTools.StateMachine.UniStateMachine
{
    [CreateAssetMenu(menuName = "States/States/UniParallelState", fileName = "UniParallelState")]
    public class UniParallelState : UniStateBehaviour
    {
        [SerializeField]
        private List<UniStateParallelMode> _states = new List<UniStateParallelMode>();

        protected override IEnumerator ExecuteState(IContext context)
        {
            var disposableItems = new IDisposableItem[_states.Count];
            _context.AddValue(context,disposableItems);

            var isActive = true;

            while (isActive)
            {
                isActive = false;
                //launch states
                for (int i = 0; i < _states.Count; i++)
                {
                    var state = _states[i].StateBehaviour;
                    var validationResult = state is IValidator<IContext> validator ? 
                        validator.Validate(context) : true;

                    isActive |= validationResult;

                    if (validationResult && state.IsActive(context))
                        continue;

                    disposableItems[i]?.Dispose();

                    var launchState = validationResult && 
                                      (disposableItems[i] == null || _states[i].RestartOnComplete);

                    if (launchState)
                    {
                        var routine = state.Execute(context);
                        var disposable = routine.RunWithSubRoutines(_states[i].RoutineType);
                        disposableItems[i] = disposable;
                    }

                }

                yield return null;
            }

        }

        protected bool IsComplete(IDisposableItem[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];
                if (!item.IsDisposed)
                    return false;
            }

            return true;
        }

        protected override void OnExit(IContext context)
        {

            //dispose all registered disposable items of context
            var disposableItems = _context.Get< IDisposableItem[]> (context);
            for (var i = 0; i < disposableItems.Length; i++)
            {
                var item = disposableItems[i];
                item?.Dispose();
            }
            disposableItems.Despawn();

            for (var i = 0; i < _states.Count; i++)
            {
                var state = _states[i];
                state.StateBehaviour.Exit(context);
            }

            _context.Remove<IDisposableItem[]>(context);
 
            base.OnExit(context);
        }

    }
}
