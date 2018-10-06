using System;
using System.Collections;
using Assets.Tools.UnityTools.Common;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.StateMachine.ContextStateMachine;
using Assets.Tools.UnityTools.StateMachine.Interfaces;
using Assets.Tools.UnityTools.UniRoutine;
using UnityEngine;

namespace Assets.Tools.UnityTools.StateMachine.UniStateMachine
{
	[Serializable]
	[CreateAssetMenu(menuName = "UniStateMachine/StateMachine", fileName = "StateMachine")]
	public class UniStateMachineObject :
		UniStateBehaviour 
	{
		#region protected methods

		[SerializeField] 
		protected UniStateSelector _stateSelector;

        #endregion

		public IContextSelector<IEnumerator> StateSelector => _stateSelector;

		public void SetSelector(UniStateSelector selector) {
			_stateSelector = selector;
		}

	    #region private methods

		protected override void OnExit(IContext context) {

			var state = _context.Get<IContextState<IEnumerator>>(context);
			var disposable = _context.Get<IDisposableItem>(context);
			
			disposable?.Dispose();
			state?.Exit(context);

		}

		protected override IEnumerator ExecuteState(IContext context) {

			IContextState<IEnumerator> activeState = null;
			IDisposableItem disposableItem = null;
			
			while (IsActive(context)) {

				var state = _stateSelector.Select(context);

				var isSameState = activeState == state;
				var isStoped = disposableItem == null || disposableItem.IsDisposed;
				
				if (isSameState && !isStoped) {
					yield return null;
					continue;
				}
				
				//stop active state data
				if (activeState != state) 
				{
					_context.Remove<IDisposableItem>(context);
					_context.Remove<IContextState<IEnumerator>>(context);
					
					disposableItem?.Dispose();
					disposableItem = null;
					
					activeState?.Exit(context);
					activeState = state;
				}

				if (activeState != null) {
					var awaiter = activeState.Execute(context);
					disposableItem = awaiter.RunWithSubRoutines();
					
					_context.AddValue(context,disposableItem);
					_context.AddValue(context,activeState);
				}
				
				yield return null;

			}
			
		}

		/// <summary>
        /// create new state machine with IEnumerator awaiter states
        /// </summary>
        /// <returns>reactive state behaviour</returns>
	    private IContextState<IEnumerator> Create()
		{
            var executor = new UniRoutineExecutor();
		    var stateMachine = new ContextStateMachine<IEnumerator>(executor);
            var reactiveState = new ContextReactiveStateMachine();

		    reactiveState.Initialize(_stateSelector, stateMachine);
            return reactiveState;
		}


	    #endregion
	}
}