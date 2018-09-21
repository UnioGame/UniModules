using System;
using System.Collections;
using Assets.Scripts.Tools.StateMachine;
using UnityEngine;


namespace Assets.Scripts.Tools.StateMachine
{
	[Serializable]
	[CreateAssetMenu(menuName = "UniStateMachine/StateMachine")]
	public class UniStateMachineObject<TSelector> :
		ScriptableObjectRoutine<IEnumerator>, IStateBehaviour<IEnumerator>
		where TSelector : IStateSelector<IStateBehaviour<IEnumerator>>
	{
		private IStateBehaviour<IEnumerator> _behaviour;

		#region protected methods

		[SerializeField] 
		protected TSelector _stateSelector;

		#endregion

		public TSelector StateSelector => _stateSelector;

		public bool IsActive => _behaviour.IsActive;

		public void SetSelector(TSelector selector)
		{
			_stateSelector = selector;
		}
		
		public void Exit()
		{
			_behaviour.Exit();
		}

		#region private methods

		protected override void OnInitialize()
		{
			_behaviour = Create();
		}

		protected override IEnumerator OnExecute()
		{
			yield return _behaviour.Execute();
		}

		private IStateBehaviour<IEnumerator> Create()
		{
			var executor = new RxStateExecutor();
			var stateMachine = new StateMachine<IEnumerator>(executor);
			var stateFactory = new DummyStateFactory<IStateBehaviour<IEnumerator>>();
			var stateValidator = new DummyStateValidator<IStateBehaviour<IEnumerator>>();


			var stateManager = new StateManager<IStateBehaviour<IEnumerator>, IStateBehaviour<IEnumerator>>(
				stateMachine,
				stateFactory,
				stateValidator
			);

			var reactiveState = new ReactiveStateMachine<IStateBehaviour<IEnumerator>>();
			reactiveState.Initialize(_stateSelector, stateManager);

			return reactiveState;
		}

		#endregion
	}
}