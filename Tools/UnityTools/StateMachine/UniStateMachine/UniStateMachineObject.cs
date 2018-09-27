using System;
using System.Collections;
using Assets.Scripts.Tools.StateMachine;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
using UnityEngine;

namespace UniStateMachine
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

		public IStateSelector<IStateBehaviour<IEnumerator>> StateSelector => _stateSelector;
			
		#region private methods

		protected override void OnContextChanged(IContextProvider contextProvider)
		{
			_stateSelector.Initialize(contextProvider);
		}

		protected override IStateBehaviour<IEnumerator> Create()
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