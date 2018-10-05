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

        /// <summary>
        /// Stop all state machines for all contexts
        /// </summary>
	    public override void Dispose()
        {
            //stop states for all state contexts
            var contexts = _context.Contexts;
            foreach (var context in contexts)
            {
                Exit(context);
            }
            _context?.Release();

            base.Dispose();
	    }

	    #region private methods

        /// <summary>
        /// create new state machine with IEnumerator awaiter states
        /// </summary>
        /// <returns>reactive state behaviour</returns>
	    protected override IContextStateBehaviour<IEnumerator> Create()
		{

            var executor = new UniRoutineExecutor();
		    var stateMachine = new ContextStateMachine<IEnumerator>(executor);
            var reactiveState = new ContextReactiveStateMachine();

		    reactiveState.Initialize(_stateSelector, stateMachine);
            return reactiveState;
		}

        /// <summary>
        /// return own state for each context
        /// </summary>
        /// <param name="context">state context</param>
        /// <returns>relative context state behaciour</returns>
	    protected override IContextStateBehaviour<IEnumerator> GetBehaviour(IContext context)
        {
            if(_context == null)
                _context = new ContextProviderProvider<IContext>();
            //get state for target cotext
            var state = _context.Get<IContextStateBehaviour<IEnumerator>>(context);
            //create state if not exists
            if (state == null)
            {
                state = Create();
                _context.AddValue(context,state);
            }

            return state;
        }

	    #endregion
	}
}