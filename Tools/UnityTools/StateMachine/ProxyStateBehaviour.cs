using System;
using System.Collections;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
using StateMachine.ContextStateMachine;

namespace UniStateMachine {

	public class ProxyStateBehaviour : ContextStateBehaviour {

		private Func<IContextProvider,IEnumerator> _updateFunction;
		private Action<IContextProvider> _onEnter;
		private Action<IContextProvider> _onExit;

		public void Initialize(Func<IContextProvider, IEnumerator> updateFunction,
		    Action<IContextProvider> onEnter = null,
		    Action<IContextProvider> onExit = null)
		{
			_updateFunction = updateFunction;
			_onEnter = onEnter;
			_onExit = onExit;
		}

		protected override void Initialize(IContextProvider context) {
			_onEnter?.Invoke(context);
		}

		protected override void OnExit(IContextProvider context) {
			_onExit?.Invoke(context);
			base.OnExit(context);
		}

		protected override IEnumerator ExecuteState(IContextProvider context)
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
