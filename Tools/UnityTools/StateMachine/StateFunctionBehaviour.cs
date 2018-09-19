using System;
using System.Collections;

namespace Assets.Scripts.Tools.StateMachine
{
	public class StateFunctionBehaviour : StateBehaviour
	{
		private Func<IEnumerator> _behaviourProvider;

		public void Initialize(Func<IEnumerator> behaviourProvider)
		{
			_behaviourProvider = behaviourProvider;
		}

		protected override IEnumerator ExecuteState()
		{
			if (_behaviourProvider == null)
				yield break;
			yield return _behaviourProvider();
		}

		protected override void OnStateStop()
		{
			_behaviourProvider = null;
			base.OnStateStop();
		}
	}
}
