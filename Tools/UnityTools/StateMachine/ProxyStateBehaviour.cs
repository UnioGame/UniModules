using System;
using System.Collections;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.StateMachine.ContextStateMachine;

namespace Assets.Tools.UnityTools.StateMachine {

	public class ProxyStateBehaviour : ContextStateBehaviour {

		private Func<IContext,IEnumerator> _updateFunction;
	    private Action<IContextProvider<IContext>> _onInitialize;
		private Action<IContext> _onExit;

        /// <summary>
        /// set state functions
        /// </summary>
        /// <param name="updateFunction"></param>
        /// <param name="onInitialize">initialize action, call only once</param>
        /// <param name="onExit"></param>
		public void Initialize(Func<IContext, IEnumerator> updateFunction,
		    Action<IContextProvider<IContext>> onInitialize = null,
		    Action<IContext> onExit = null)
		{
			_updateFunction = updateFunction;
		    _onInitialize = onInitialize;
			_onExit = onExit;
		}

		protected override void OnInitialize(IContextProvider<IContext> stateContext) {
		    _onInitialize?.Invoke(stateContext);
		}

	    public override void Dispose()
	    {
	        _updateFunction = null;
	        _onInitialize = null;
	        _onExit = null;
	    }

	    protected override void OnExit(IContext context) {
			_onExit?.Invoke(context);
		}

		protected override IEnumerator ExecuteState(IContext context)
		{
			if (_updateFunction != null) {
				yield return _updateFunction(context);
			}
			else {
				yield break;
			}
		}

	}
}
