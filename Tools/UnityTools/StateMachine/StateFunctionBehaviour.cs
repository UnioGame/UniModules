using System;
using System.Collections;

namespace Assets.Scripts.Tools.StateMachine
{
	public class StateFunctionBehaviour<TData> : StateBehaviour<TData>
	{
		private Func<TData,IEnumerator> _behaviourProvider;
		private Action _onInitialize;
		private Action _onExit;

		public void Initialize(Func<TData,IEnumerator> behaviourProvider, 
			Action onInitialize = null, Action onExit = null)
		{
			_behaviourProvider = behaviourProvider;
			_onInitialize = onInitialize;
			_onExit = onExit;
		}

		protected override IEnumerator ExecuteState(TData data)
		{
			if (_behaviourProvider == null)
				yield break;
			yield return _behaviourProvider(data);
		}

		protected override void OnStateStop()
		{
			_onExit?.Invoke();
			_behaviourProvider = null;
			_onExit = null;
			_onInitialize = null;
		}

		protected override void Initialize() {
			_onInitialize?.Invoke();
		}
		
	}
}
