using System;
using System.Collections;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
using StateMachine.ContextStateMachine;

namespace UniStateMachine {

	public class ProxyStateBehaviour : ContextStateBehaviour {

		private Func<IContext,IEnumerator> _updateFunction;
		private Action<IContext> _onEnter;
		private Action<IContext> _onExit;

		public void Initialize(Func<IContext, IEnumerator> updateFunction,
		    Action<IContext> onEnter = null,
		    Action<IContext> onExit = null)
		{
			_updateFunction = updateFunction;
			_onEnter = onEnter;
			_onExit = onExit;
		}

		protected override void Initialize(IContext context) {
			_onEnter?.Invoke(context);
		}

		protected override void OnExit(IContext context) {
			_onExit?.Invoke(context);
			base.OnExit(context);
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
