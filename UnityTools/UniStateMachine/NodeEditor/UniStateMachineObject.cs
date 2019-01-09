using System;
using System.Collections;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.StateMachine.ContextStateMachine;
using Assets.Tools.UnityTools.StateMachine.Interfaces;
using Assets.Tools.UnityTools.UniRoutine;
using UnityEngine;

namespace UniStateMachine
{
	[Serializable]
	[CreateAssetMenu(menuName = "UniStateMachine/States/StateMachine", fileName = "StateMachine")]
	public class UniStateMachineObject :
		UniGraphNode 
	{
		#region protected methods

		[SerializeField] 
		protected UniStateTransition _stateSelector;

        #endregion

		public IUniStateTransition StateSelector => _stateSelector;

		public void SetSelector(UniStateTransition selector) {
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

				var state = _stateSelector.SelectState(context);

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
					
					_context.UpdateValue(context,disposableItem);
					_context.UpdateValue(context,activeState);
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