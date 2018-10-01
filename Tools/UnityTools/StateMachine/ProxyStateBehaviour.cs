using System;
using System.Collections;

namespace UniStateMachine {

	public class ProxyStateBehaviour : StateBehaviour {

		private Func<IEnumerator> _updateFunction;
		private Action _onEnter;
		private Action _onExit;

		public void Initialize(Func<IEnumerator> updateFunction, Action onEnter = null, Action onExit = null) {
			_updateFunction = updateFunction;
			_onEnter = onEnter;
			_onExit = onExit;
		}

		protected override void Initialize() {
			_onEnter?.Invoke();
		}

		protected override void OnStateStop() {
			_onExit?.Invoke();
			base.OnStateStop();
		}

		protected override IEnumerator ExecuteState() {
			if (_updateFunction != null) {
				yield return _updateFunction();
			}
			else {
				yield break;
			}
		}

	}
}
