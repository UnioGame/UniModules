using System.Collections;
using System.Collections.Generic;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.ObjectPool.Scripts;
using Assets.Tools.UnityTools.ReorderableInspector;
using Assets.Tools.UnityTools.UniRoutine;
using UnityEngine;

namespace Assets.Tools.UnityTools.StateMachine.UniStateMachine
{
    [CreateAssetMenu(menuName = "States/States/UniParallelState", fileName = "UniParallelState")]
    public class UniParallelState : UniStateBehaviour
    {
        [SerializeField]
        [Reorderable]
        private List<UniStateParallelMode> _states = new List<UniStateParallelMode>();

        protected override IEnumerator ExecuteState(IContext context)
        {
            var disposableItems = new IDisposableItem[_states.Count];
            _context.AddValue(context,disposableItems);

            while (true)
            {
                //launch states
                for (int i = 0; i < _states.Count; i++)
                {
                    var state = _states[i].StateBehaviour;
                    var disposable = disposableItems[i];
                    
                    var validationResult = 
                        state is IValidator<IContext> validator ? 
                        validator.Validate(context) : true;

                    var isStateActive = disposable?.IsDisposed == false;
     
                    //continue state execution
                    if (validationResult && isStateActive)
                        continue;

                    //stop current execution
                    disposable?.Dispose();
                    
                    if(validationResult == false)
                        continue;

                    //if state can be restarted or it first launch
                    var canLaunchState = disposableItems[i] == null || _states[i].RestartOnComplete;

                    if (canLaunchState)
                    {
                        var routine = state.Execute(context);
                        disposable = routine.RunWithSubRoutines(_states[i].RoutineType);
                        disposableItems[i] = disposable;
                    }

                }

                yield return null;
            }

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
